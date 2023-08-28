using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Helper
{
    public static class FilesHelper
    {
        public static string SetFileName(string fileName)
        {
            return fileName.Replace("/", "").Replace(":", "").Replace(" ", "_")+".xlsx";
        }
        public static string SetFileNameAsPDF(string fileName)
        {
            return fileName.Replace("/", "").Replace(":", "").Replace(" ", "_") + ".pdf";
        }
    }
}
