using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.EntityFrameworkCore.Seed.Host
{
    public class PUBandPUBACreator
    {
        public List<ProjectUserBillAccount> ProjectUserBillAccounts => GetProjectUserBillAccounts();

        public List<ProjectUserBill> ProjectUserBills => GetProjectUserBills();

        private readonly ProjectManagementDbContext _context;

        public PUBandPUBACreator(ProjectManagementDbContext context)
        {
            _context = context;
        }

        private List<ProjectUserBillAccount> GetProjectUserBillAccounts()
        {
            return _context.ProjectUserBillAccounts.ToList();
        }

        private List<ProjectUserBill> GetProjectUserBills()
        {
            return _context.ProjectUserBills.ToList();
        }

        public void Create()
        {
            CreateProjectUserBillAccounts();
            CreateProjectUserBills();
            CreateLinkedResources(); // Thêm phương thức này để tạo dữ liệu cho linked resource
        }

        private void CreateProjectUserBillAccounts()
        {
            foreach (var projectUserBillAccount in ProjectUserBillAccounts)
            {
                AddProjectUserBillAccount(projectUserBillAccount);
            }
        }

        private void CreateProjectUserBills()
        {
            foreach (var projectUserBill in ProjectUserBills)
            {
                AddProjectUserBill(projectUserBill);
            }
        }

        private void CreateLinkedResources()
        {
            foreach (var projectUserBillAccount in ProjectUserBillAccounts)
            {
                var existingLinkedResource = _context.LinkedResources
                    .FirstOrDefault(p => p.UserId == projectUserBillAccount.UserId
                                         && p.UserBillAccountId == projectUserBillAccount.UserBillAccountId);

                if (existingLinkedResource == null)
                {
                    var linkedResource = new LinkedResource
                    {
                        UserId = projectUserBillAccount.UserId,
                        UserBillAccountId = projectUserBillAccount.UserBillAccountId
                    };

                    _context.LinkedResources.Add(linkedResource);
                }
            }

            _context.SaveChanges(); // Đặt lệnh SaveChanges ngoài vòng lặp để chỉ cần gọi một lần sau khi tạo tất cả linked resources.
        }


        private void AddProjectUserBillAccount(ProjectUserBillAccount projectUserBillAccount)
        {
            // Tạo tài khoản hoá đơn dự án nếu chưa tồn tại
            var existingAccount = _context.ProjectUserBillAccounts
                .FirstOrDefault(p => p.UserId == projectUserBillAccount.UserId
                                     && p.ProjectId == projectUserBillAccount.ProjectId);

            if (existingAccount == null)
            {
                _context.ProjectUserBillAccounts.Add(projectUserBillAccount);
            }
        }

        private void AddProjectUserBill(ProjectUserBill projectUserBill)
        {
            // Tạo hoá đơn dự án nếu chưa tồn tại
            var existingBill = _context.ProjectUserBills
                .FirstOrDefault(p => p.UserId == projectUserBill.UserId
                                     && p.ProjectId == projectUserBill.ProjectId);

            if (existingBill == null)
            {
                _context.ProjectUserBills.Add(projectUserBill);
            }
        }

    }
}
