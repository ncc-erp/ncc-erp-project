using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;

namespace ProjectManagement.NccCore.BackgroundJob
{
    public class InActiveUserBackgroundJob : BackgroundJob<InActiveUserBackgroundJobArgs>, ITransientDependency
    {
        private readonly IRepository<User, long> _repository;

        public InActiveUserBackgroundJob(IRepository<User, long> repository)
        {
            _repository = repository;
        }

        public override void Execute(InActiveUserBackgroundJobArgs args)
        {
            //InActive remain users
            try
            {
                Queue<long> users = new Queue<long>(args.Users);
                while (users.Count > 0)
                {
                    var user = _repository.Get(users.Peek());
                    user.IsActive = false;
                    _repository.Update(user);
                    users.Dequeue();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
