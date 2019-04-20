using System.Data.Entity;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    [DbConfigurationType(typeof(SqlServerDbConfiguration))]
    internal class SqlServerDbContext : System.Data.Entity.DbContext
    {
        public SqlServerDbContext() : base("name=MigrationConnect") { }

        public SqlServerDbContext(string connectionString)
            : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("v4");

            foreach (var assembly in ClassHelper.FromAssemblies())
                modelBuilder.Configurations.AddFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
