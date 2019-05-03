using System.Data.Entity;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    [DbConfigurationType(typeof(SqlServerDbConfiguration))]
    internal class SqlServerDbContext : System.Data.Entity.DbContext
    {
        public SqlServerDbContext(string connectionString)
            : base(connectionString) {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Notify");

            var modelAssemblies = ClassHelper.FromAssemblies();
            foreach (var assembly in modelAssemblies)
                modelBuilder.Configurations.AddFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
