using Abp.Application.Services;
using NccCore.IoC;
using ProjectManagement.Entities;
using System.Linq;

namespace ProjectManagement.Services.ProjectUserBills
{
    public class ProjectUserBillManager : ApplicationService
    {
        private readonly IWorkScope workScope;

        public ProjectUserBillManager(IWorkScope workScope)
        {
            this.workScope = workScope;
        }
        public IQueryable<ProjectUserBill> GetSubProjectBills(long parentProjectId)
        {
            var query = from p in workScope.GetAll<Project>()
                        where p.ParentInvoiceId == parentProjectId
                        join pub in workScope.GetAll<ProjectUserBill>()
                         on p.Id equals pub.ProjectId
                        select pub;
            return query.OrderBy(p => p.Project.Name).ThenBy(p => p.User.EmailAddress);
        }
    }
}
