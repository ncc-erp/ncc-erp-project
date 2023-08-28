using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectManagement.Utils
{
    public class FileUtils
    {
        public static string FullFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }
            if (Constants.ConstantUploadFile.Provider == Constants.ConstantUploadFile.AMAZONE_S3)
            {
                return Constants.ConstantAmazonS3.CloudFront.TrimEnd('/') + "/" + filePath;
            }
            else
            {
                return Constants.ConstantInternalUploadFile.RootUrl.TrimEnd('/') + "/" + filePath;
            }
        }

        public static string GetFileExtension(IFormFile file)
        {
            if (file == default || string.IsNullOrEmpty(file.FileName))
            {
                return "";
            }
            return Path.GetExtension(file.FileName).Substring(1).ToLower();
        }

        public static string GetFileName(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }
            if (filePath.Contains("/"))
            {
                return filePath.Substring(filePath.LastIndexOf("/") + 1);
            }
            return filePath;

        }

        public static byte[] DeleteXlsxSheet(string sheetDelete, string sheetHiden, MemoryStream s)
        {
            using (var excelPackage = new ExcelPackage(s))
            {
                var worksheetDel = excelPackage.Workbook.Worksheets
                    .SingleOrDefault(x => x.Name == sheetDelete);
                var worksheetHiden = excelPackage.Workbook.Worksheets
                    .SingleOrDefault(x => x.Name == sheetHiden);
                if (worksheetHiden != null)
                    worksheetHiden.Hidden = eWorkSheetHidden.VeryHidden;
                if (worksheetDel != null)
                    excelPackage.Workbook.Worksheets.Delete(worksheetDel);
                return excelPackage.GetAsByteArray();
            }
        }
    }
}
