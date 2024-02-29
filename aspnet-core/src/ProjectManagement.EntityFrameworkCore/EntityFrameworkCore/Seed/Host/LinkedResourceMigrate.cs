using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;

namespace ProjectManagement.EntityFrameworkCore.Seed.Host
{
    public class LinkedResourceMigrate
    {
        private readonly ProjectManagementDbContext _context;

        public LinkedResourceMigrate(ProjectManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            SeedLinkedResources();
        }

        private void SeedLinkedResources()
        {
            var linkResources = _context.LinkedResources.FirstOrDefault();
            var projectUserBillAccount = _context.ProjectUserBillAccounts.ToList();

            if (linkResources == null && projectUserBillAccount != null)
            {
                var linkedResourcesFromProjectUserBillAccount = projectUserBillAccount
                    .Join(_context.ProjectUserBills,
                          account => new { account.UserId, account.ProjectId },
                          bill => new { bill.UserId, bill.ProjectId },
                          (account, bill) => new LinkedResource
                          {
                              UserId = account.UserBillAccountId,
                              ProjectUserBillId = bill.Id,
                          })
                    .ToList();
                foreach (var linkedResource in linkedResourcesFromProjectUserBillAccount)
                {
                    _context.LinkedResources.Add(linkedResource);
                }
                _context.SaveChanges();
            }

        }
    }
}
