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
            var isLinkResourcesNull = !_context.LinkedResources.Any();

            if (isLinkResourcesNull)
            {
                var projectUserBillAccount = _context.ProjectUserBillAccounts
                    .Select(s => new { s.TenantId, s.UserId, s.ProjectId, s.UserBillAccountId })
                    .ToList();


                var linkedResources = (from puba in projectUserBillAccount
                        from pub in _context.ProjectUserBills
                        where puba.TenantId == pub.TenantId && puba.UserBillAccountId == pub.UserId && puba.ProjectId == puba.ProjectId
                        select new LinkedResource
                        {
                            TenantId = puba.TenantId,
                            UserId = puba.UserId,
                            ProjectUserBillId = pub.Id,
                        }).ToList();

                _context.LinkedResources.AddRange(linkedResources);
                _context.SaveChanges();
            }

        }
    }
}
