using Newtonsoft.Json.Linq;
using ProjectManagement.Authorization.Users;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.NccCore.BackgroundJob
{
    public class InActiveUserBackgroundJobArgs
    {
        public IEnumerable<long> Users { get; set; }
    }
}
