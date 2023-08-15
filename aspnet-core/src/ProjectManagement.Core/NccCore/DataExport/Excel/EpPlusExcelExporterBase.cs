using Abp.Collections.Extensions;
using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace NccCore.DataExport.Excel
{
    public abstract class EpPlusExcelExporterBase 
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        protected EpPlusExcelExporterBase(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            string folder = _hostingEnvironment.WebRootPath;
            var filePath = Path.Combine(folder, fileName);
            var file = new FileInfo(filePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                excelPackage.SaveAs(file);
            }

            return new FileDto
            {
                FileName = fileName,
                FileSize = file.Length,
                ServerPath = fileName,
                FilePath = filePath
            };
        }

        protected void AddHeader(ExcelWorksheet sheet, int startRowIndex, params string[] headerTexts)
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

        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, int startRowIndex, string headerText)
        {
            var cell = sheet.Cells[startRowIndex, columnIndex];
            cell.Value = headerText;
            cell.Style.Font.Bold = true;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                    sheet.Cells[i + startRowIndex, j + 1].Style.WrapText = true;
                }
            }
        }

        protected ExcelRange AddHeader(ExcelWorksheet sheet, int columnStartIndex, int columnEndIndex, int rowStartIndex, int rowEndIndex, string headerText)
        {
            var cellMerge = sheet.Cells[rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex];
            cellMerge.Merge = true;

            cellMerge.Value = headerText;
            cellMerge.Style.WrapText = true;
            cellMerge.Style.Font.Bold = true;
            cellMerge.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cellMerge.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            return cellMerge;
        }

        protected ExcelRange AddObject(ExcelWorksheet sheet, int columnStartIndex, int columnEndIndex, int rowStartIndex, int rowEndIndex, string headerText)
        {
            var cellMerge = sheet.Cells[rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex];
            cellMerge.Merge = true;

            cellMerge.Value = headerText;
            cellMerge.Style.WrapText = true;
            cellMerge.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            return cellMerge;
        }
    }
}