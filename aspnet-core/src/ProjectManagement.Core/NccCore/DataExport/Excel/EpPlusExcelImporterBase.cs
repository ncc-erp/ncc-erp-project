using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using NccCore.DataExport.Dto;

namespace NccCore.DataExport.Excel
{
    public static class EpPlusExcelImporterBase
    {
        /// <summary>
        /// Import excel file to collection
        /// Header at row num 1
        /// Column names are equal to property names
        /// Excel file has only 1 work sheet
        /// </summary>
        /// <typeparam name="T">Output item type</typeparam>
        /// <param name="stream">Memory stream with file content</param>
        /// <param name="columnNameIndexPairs">Pairs of key - column name, value - column index</param>
        /// <returns>Collection</returns>
        public static List<T> GetCollectionFromExcelFile<T>(Stream stream, Dictionary<string, int> columnNameIndexPairs = null, int? startDataRowIndex = null) where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var result = new List<T>();
            using (var excelPackage = new ExcelPackage(stream))
            {
                var sheet = excelPackage.Workbook.Worksheets[0];
                var start = sheet.Dimension.Start;
                var end = sheet.Dimension.End;
                // Pair column name and column index
                if (columnNameIndexPairs == null)
                {
                    columnNameIndexPairs = new Dictionary<string, int>();
                    for (int i = start.Column; i <= end.Column; i++)
                    {
                        var cellValue = sheet.Cells[start.Row, i].Value?.ToString();
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            columnNameIndexPairs.Add(cellValue.Trim(), i);
                        }
                    }
                }
                if (!startDataRowIndex.HasValue)
                {
                    startDataRowIndex = start.Row + 1;
                }
                for (int rowIndex = startDataRowIndex.Value; rowIndex <= end.Row; rowIndex++)
                {
                    var instance = (T)Activator.CreateInstance(type);
                    foreach (var header in columnNameIndexPairs)
                    {
                        var property = properties.FirstOrDefault(i => i.Name.ToLowerInvariant() == header.Key.ToLowerInvariant());
                        if (property == null)
                        {
                            continue;
                        }
                        var targetType = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) ?
                            Nullable.GetUnderlyingType(property.PropertyType) :
                            property.PropertyType;
                        var cellValue = sheet.Cells[rowIndex, header.Value].Value;
                        var cellValueStr = cellValue?.ToString();
                        var propertyValue = string.IsNullOrEmpty(cellValueStr) ? null : Convert.ChangeType(cellValue, targetType);
                        property.SetValue(instance, propertyValue);
                    }
                    if (instance is IHasRowIndex)
                    {
                        var rowIndexProperty = type.GetProperty(nameof(IHasRowIndex.RowIndex));
                        rowIndexProperty.SetValue(instance, rowIndex);
                    }
                    result.Add(instance);
                }
            }

            return result;
        }

        public static List<T> GetCollectionFromExcelFile<T>(Stream stream, string sheetName, Dictionary<string, int> columnNameIndexPairs = null, int? startDataRowIndex = null) where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var result = new List<T>();
            using (var excelPackage = new ExcelPackage(stream))
            {
                var sheet = excelPackage.Workbook.Worksheets[sheetName];
                var start = sheet.Dimension.Start;
                var end = sheet.Dimension.End;
                // Pair column name and column index
                if (columnNameIndexPairs == null)
                {
                    columnNameIndexPairs = new Dictionary<string, int>();
                    for (int i = start.Column; i <= end.Column; i++)
                    {
                        var cellValue = sheet.Cells[start.Row, i].Value?.ToString();
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            columnNameIndexPairs.Add(cellValue.Trim(), i);
                        }
                    }
                }
                if (!startDataRowIndex.HasValue)
                {
                    startDataRowIndex = start.Row + 1;
                }
                for (int rowIndex = startDataRowIndex.Value; rowIndex <= end.Row; rowIndex++)
                {
                    var instance = (T)Activator.CreateInstance(type);
                    foreach (var header in columnNameIndexPairs)
                    {
                        var property = properties.FirstOrDefault(i => i.Name.ToLowerInvariant() == header.Key.ToLowerInvariant());
                        if (property == null)
                        {
                            continue;
                        }
                        var targetType = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) ?
                            Nullable.GetUnderlyingType(property.PropertyType) :
                            property.PropertyType;
                        var cellValue = sheet.Cells[rowIndex, header.Value].Value;
                        var cellValueStr = cellValue?.ToString();
                        if (targetType == typeof(string))
                        {
                            cellValueStr = cellValueStr?.Normalize();
                        }
                        var propertyValue = string.IsNullOrEmpty(cellValueStr) ? null : Convert.ChangeType(cellValue, targetType);
                        property.SetValue(instance, propertyValue);
                    }
                    result.Add(instance);
                }
            }

            return result;
        }
    }
}
