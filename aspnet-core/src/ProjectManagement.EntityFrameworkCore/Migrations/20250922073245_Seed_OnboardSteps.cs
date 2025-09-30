using Microsoft.EntityFrameworkCore.Migrations;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;

namespace ProjectManagement.Migrations
{
    public partial class Seed_OnboardSteps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.Now;
            migrationBuilder.InsertData(
                table: "OnboardSteps",
                columns: new[] { "Id", "Title", "Content", "Order", "CreationTime", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Checklist 1: PM tạo meeting onboarding",
                        "Việc giới thiệu new member, all old member thường ở daily meeting<br>PM giới thiệu hoặc member tự giới thiệu các thông tin: tên, tuổi, văn phòng, vị trí (dev fe, dev be, dev fullstack, qa, lead fe, ..)",
                        1, now, false },
                    { 2, "Checklist 2: PM nói expect đối với new member",
                        "Mong muốn nhân viên có thái độ, cách làm việc, mức độ perform như thế nào,...",
                        2, now, false },
                    { 3, "Checklist 3: PM yêu cầu member đọc kỹ Các lỗi ngớ ngẩn gây hậu quả nghiêm trọng",
                        "PM cần yêu cầu member dành ra tối thiểu 30 phút để đọc kỹ.<br>Điều nào không hiểu thì phải hỏi lại ngay.<br>PM cần hỏi lại member xem đã hiểu hết chưa, còn chỗ nào thắc mắc không.<br>Chỗ nào member chưa hiểu thì PM cần giải thích cho đến khi member hiểu rõ.",
                        3, now, false },
                    { 4, "Checklist 4: PM nói rõ cho new member về account, stakeholder",
                        "PM share thông tin account trên passbolt cho member<br>Nói rõ cho member: mình đại diện cho bên nào<br>PM share thông tin internal team, external team (trên ops) cho member",
                        4, now, false },
                    { 5, "Checklist 5: PM phổ biến cho member quy trình làm task",
                        "nhận task ở đâu, từ ai<br>kéo task (todo,inprogress, PR review, ....)<br>clear/confirm req của task trên đâu (notion/jira, ...)",
                        5, now, false },
                    { 6, "Checklist 6: PM phổ biến cho member quy trình git flow của dự án",
                        "phần này PM cần đưa lên OPS và gửi link cho member đọc",
                        6, now, false },
                    { 7, "Checklist 7: PM assign mentor cho new member",
                        "Intern bắt buộc phải có mentor",
                        7, now, false }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 7; i++)
            {
                migrationBuilder.DeleteData(
                    table: "OnboardSteps",
                    keyColumn: "Id",
                    keyValue: i
                );
            }
        }
    }
}
