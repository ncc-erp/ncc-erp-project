using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Constants
{
    public class ConstantUploadFile
    {
        public static string Provider { get; set; }
        public static string[] AllowImageFileTypes { get; set; }
        public static string[] AllowTimesheetFileTypes { get; set; }
        public static string[] AllowCVFileTypes { get; set; }


        public static readonly string AMAZONE_S3 = "AWS";
        public static readonly string INTERNAL = "Internal";
        public const string Project_Tool = "project_tool";
        public static string AvatarFolder { get; set; }
        public const string CV_Folder = "cv";
        public const string TIMESHEET_FOLDER = "timesheets";
        public const string UPLOAD_FOLDER = "uploads";
    }
}
