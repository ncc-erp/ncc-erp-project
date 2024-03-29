﻿namespace ProjectManagement.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly ProjectManagementDbContext _context;

        public InitialHostDbBuilder(ProjectManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new LinkedResourceMigrate(_context).Create();
            _context.SaveChanges();
        }
    }
}
