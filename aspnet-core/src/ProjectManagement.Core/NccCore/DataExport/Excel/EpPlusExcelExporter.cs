using Abp.Collections.Extensions;
using Microsoft.Extensions.Hosting;
using NccCore.DataExport.Dto;
using NccCore.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace NccCore.DataExport.Excel
{
    public class EpPlusExcelExporter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public EpPlusExcelExporter(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public byte[] CreateExcelPackage(Action<ExcelPackage> creator)
        {
            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                return excelPackage.GetAsByteArray();
            }
        }

        public void AddHeader(ExcelWorksheet sheet, int startRowIndex, params object[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, startRowIndex, headerTexts[i]);
            }
        }

        public void AddHeader(ExcelWorksheet sheet, IList<KeyValuePair<string, string>> headerValue)
        {
            foreach (var item in headerValue)
            {
                if (sheet.Names.ContainsKey(item.Key))
                {
                    using (ExcelNamedRange namedRange = sheet.Names[item.Key])
                    {
                        for (int rowIndex = namedRange.Start.Row; rowIndex <= namedRange.End.Row; rowIndex++)
                        {
                            for (int columnIndex = namedRange.Start.Column; columnIndex <= namedRange.End.Column; columnIndex++)
                            {
                                sheet.Cells[rowIndex, columnIndex].Value = item.Value;
                            }
                        }
                    }
                }
            }
        }

        public void AddObjects<T>(
            ExcelWorksheet sheet,
            int startRowIndex,
            IList<T> items,
            IList<string> properties,
            IList<KeyValuePair<string, string>> headerValue,
            bool autoStt)
        {
            var propertySelectors = TypeDescriptor.GetProperties(typeof(T));

            // Add header
            foreach (var item in headerValue.EmptyIfNull())
            {
                if (sheet.Names.ContainsKey(item.Key))
                {
                    using (ExcelNamedRange namedRange = sheet.Names[item.Key])
                    {
                        for (int rowIndex = namedRange.Start.Row; rowIndex <= namedRange.End.Row; rowIndex++)
                        {
                            for (int columnIndex = namedRange.Start.Column; columnIndex <= namedRange.End.Column; columnIndex++)
                            {
                                sheet.Cells[rowIndex, columnIndex].Value = item.Value;
                            }
                        }
                    }
                }

            }

            if (items.IsNullOrEmpty() || propertySelectors.Count <= 0)
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < properties.Count; j++)
                {
                    var cell = sheet.Cells[i + startRowIndex, j + 1];

                    var destinationProperty = propertySelectors.Find(properties[j], true);
                    if (autoStt && j == 0)
                    {
                        cell.Value = i + 1;
                    }
                    else
                    {
                        cell.Value = destinationProperty?.GetValue(items[i]);
                    }

                    FormatDefault(cell);
                    cell.Style.Font.Bold = false;
                }
            }
        }

        public void AddGroupObject<T, Q>(
            ExcelWorksheet sheet,
            int startRowIndex,
            IList<ExcelGroupData<T, Q>> items,
            IList<string> properties,
            IList<string> GroupProperties,
            IList<KeyValuePair<string, string>> HeaderValue,
            bool autoStt,
            bool localSum = false)
        where T : class
        where Q : class
        {

            var propertySelectors = TypeDescriptor.GetProperties(typeof(T));
            var headerPropertySelector = TypeDescriptor.GetProperties(typeof(Q));

            foreach (var item in HeaderValue)
            {
                if (sheet.Names.ContainsKey(item.Key))
                {
                    using (ExcelNamedRange namedRange = sheet.Names[item.Key])
                    {
                        for (int rowIndex = namedRange.Start.Row; rowIndex <= namedRange.End.Row; rowIndex++)
                        {
                            for (int columnIndex = namedRange.Start.Column; columnIndex <= namedRange.End.Column; columnIndex++)
                            {
                                sheet.Cells[rowIndex, columnIndex].Value = item.Value;
                            }
                        }
                    }
                }

            }

            if (items.IsNullOrEmpty() || propertySelectors.Count <= 0)
            {
                return;
            }

            var currentGroupRow = startRowIndex;
            for (var i = 0; i < items.Count; i++)
            {
                // Add header row
                for (var j = 1; j < GroupProperties.Count; j++)
                {
                    var cell = sheet.Cells[currentGroupRow, j + 1];

                    var destinationProperty = headerPropertySelector.Find(GroupProperties[j], true);
                    if (autoStt && j == 0)
                    {
                        cell.Value = "";
                    }
                    else
                    {
                        cell.Value = destinationProperty?.GetValue(items[i].DataGroupRow);
                    }
                    cell.Style.Font.Bold = true;
                    FormatDefault(cell);

                }

                currentGroupRow += 1;

                // Add data row
                for (int k = 0; k < items[i].DataRow.Count; k++)
                {
                    for (var j = 0; j < properties.Count; j++)
                    {
                        var cell = sheet.Cells[k + currentGroupRow, j + 1];

                        var destinationProperty = propertySelectors.Find(properties[j], true);
                        if (autoStt && j == 0)
                        {
                            cell.Value = k + 1;
                        }
                        else
                        {
                            cell.Value = destinationProperty?.GetValue(items[i].DataRow[k]);
                        }

                        if (cell.Value?.GetType() == typeof(DateTime))
                        {
                            cell.Style.Numberformat.Format = "dd/mm/yyyy";
                        }

                        cell.Style.WrapText = true;
                        cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Name = "Times New Roman";
                    }
                }
                currentGroupRow += items[i].DataRow.Count;

                // Add sum row
                if (localSum)
                {
                    for (var j = 1; j < properties.Count; j++)
                    {
                        var cell = sheet.Cells[currentGroupRow, j + 1];

                        var destinationProperty = propertySelectors.Find(properties[j], true);
                        var value = destinationProperty?.GetValue(items[i].SumRow);
                        if (autoStt && j == 0)
                        {
                            cell.Value = "";
                        }
                        else
                        {
                            cell.Value = value;
                        }
                        cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Name = "Times New Roman";

                    }
                    currentGroupRow++;
                }
            }
        }

        public byte[] Export<T>(
            string templatePath,
            IList<T> data,
            IList<string> properties,
            IList<KeyValuePair<string, string>> HeaderValue,
            bool autoStt = false)
            where T : class
        {
            return CreateExcelPackage(
               excelPackage =>
                {
                    if (File.Exists(templatePath))
                    {
                        using (var memoryStream = new MemoryStream(File.ReadAllBytes(templatePath)))
                        {
                            using (var excelPackageIn = new ExcelPackage(memoryStream))
                            {
                                var sheetIn = excelPackageIn.Workbook.Worksheets[0];
                                var sheet = excelPackage.Workbook.Worksheets.Add(sheetIn.Name, sheetIn);

                                var maxRow = sheet.Dimension.End.Row;
                                var rowStartIndex = 1;

                                for (var i = 1; i <= maxRow; i++)
                                {
                                    var cell = sheet.Cells[i, 1];

                                    if (cell != null && cell.Value != null && cell.Value.ToString().Equals("data", StringComparison.OrdinalIgnoreCase))
                                    {
                                        rowStartIndex = i;
                                        break;
                                    }
                                }

                                sheet.InsertRow(rowStartIndex, data.Count);
                                AddObjects(sheet, rowStartIndex, data, properties, HeaderValue, autoStt);
                            }
                        }
                    }
                });
        }

        public byte[] ExportWithGroupData<T, Q>(
            string templatePath,
            IList<ExcelGroupData<T, Q>> data,
            IList<string> properties,
            IList<string> GroupProperties,
            IList<KeyValuePair<string, string>> HeaderValue,
            bool autoStt = false,
            bool localSum = false
            )
            where T : class
            where Q : class
        {
            return CreateExcelPackage(
               excelPackage =>
               {

                   if (File.Exists(templatePath))
                   {
                       using (var memoryStream = new MemoryStream(File.ReadAllBytes(templatePath)))
                       {
                           using (var excelPackageIn = new ExcelPackage(memoryStream))
                           {
                               var sheetIn = excelPackageIn.Workbook.Worksheets[0];
                               var sheet = excelPackage.Workbook.Worksheets.Add(sheetIn.Name, sheetIn);

                               var maxRow = sheet.Dimension.End.Row;
                               var rowStartIndex = 0;

                               for (var i = 1; i <= maxRow; i++)
                               {
                                   var cell = sheet.Cells[i, 1];

                                   if (cell != null && cell.Value != null && cell.Value.ToString().Equals("data", StringComparison.OrdinalIgnoreCase))
                                   {
                                       rowStartIndex = i;
                                       break;
                                   }
                               }
                               var endRow = 0;
                               if (localSum)
                               {
                                   endRow = data.Count * 2 + data.Sum(d => d.DataRow.Count) - 1;
                               }
                               else
                               {
                                   endRow = data.Count + data.Sum(d => d.DataRow.Count) - 1;
                               }
                               sheet.InsertRow(rowStartIndex, endRow);
                               AddGroupObject(sheet, rowStartIndex, data, properties, GroupProperties, HeaderValue, autoStt, localSum);
                           }
                       }
                   }
               });
        }

        public void FormatDefault(ExcelRange cell)
        {
            if (cell.Value?.GetType() == typeof(DateTime))
            {
                cell.Style.Numberformat.Format = "dd/mm/yyyy";
            }

            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

            if (cell.Value?.GetType().IsNumeric() == true)
            {
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            }

            cell.Style.WrapText = true;
            cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Font.Size = 12;
            cell.Style.Font.Name = "Times New Roman";

        }
    }
}