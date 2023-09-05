using Abp.Application.Services;
using NccCore.IoC;

namespace ProjectManagement.Manager
{
    public class BaseManager : ApplicationService
    {
        protected IWorkScope WorkScope { get; set; }

        public BaseManager(IWorkScope workScope)
        {
            WorkScope = workScope;
        }
    }
}