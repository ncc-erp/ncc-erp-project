using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ProjectManagement.EntityFrameworkCore
{
    public static class ProjectManagementDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ProjectManagementDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, options =>
            {
                options.CommandTimeout(180);
            });
        }

        public static void Configure(DbContextOptionsBuilder<ProjectManagementDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection, options =>
            {
                options.CommandTimeout(180);
            });
        }
    }
}
