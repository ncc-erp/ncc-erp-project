using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.EntityFrameworkCore
{
    public static class ProjectManagementDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ProjectManagementDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ProjectManagementDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
