using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;

namespace ProjectManagement.EntityFrameworkCore.Seed.Host
{
    public class LinkedResourceCreator
    {
        private readonly ProjectManagementDbContext _context;

        public LinkedResourceCreator(ProjectManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            SeedLinkedResources();
        }

        private void SeedLinkedResources()
        {
            if (!_context.LinkedResources.Any())
            {
                var linkedResourcesToAdd = _context.ProjectUserBillAccounts
                    .Join(_context.ProjectUserBills,
                        puba => new { puba.UserId, puba.ProjectId },
                        pub => new { pub.UserId, pub.ProjectId },
                        (puba, pub) => new LinkedResource
                        {
                            UserId = puba.UserBillAccountId,
                            ProjectUserBillId = _context.ProjectUserBills
                                .FirstOrDefault(x => x.UserId == puba.UserId && x.ProjectId == puba.ProjectId) != null
                                    ? _context.ProjectUserBills.FirstOrDefault(x => x.UserId == puba.UserId && x.ProjectId == puba.ProjectId).Id
                                    : 0
                        })
                    .ToList();

                var distinctLinkedResources = linkedResourcesToAdd
                    .GroupBy(lr => new { lr.UserId, lr.ProjectUserBillId })
                    .Select(group => group.First())
                    .ToList();

                _context.LinkedResources.AddRange(distinctLinkedResources);
                _context.SaveChanges();
            }
        }
    }
}
