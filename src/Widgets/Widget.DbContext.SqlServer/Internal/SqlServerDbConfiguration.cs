using System.Data.Entity;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    internal class SqlServerDbConfiguration : DbConfiguration
    {
        public SqlServerDbConfiguration()
        {
            SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
            SetProviderFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
            SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
        }
    }
}
