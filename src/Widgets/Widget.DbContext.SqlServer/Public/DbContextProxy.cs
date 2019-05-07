using Guanwu.Notify.Widget.Persistence;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    public sealed class DbContextProxy : IDbContext
    {
        public string WidgetName => Const.WIDGET_NAME;

        public List<TElement> FromSql<TElement>(string sql, params object[] parameters)
        {
            using (var context = new SqlServerDbContext(Const.DBCONNECT)) {
                return context.Database.SqlQuery<TElement>(sql, parameters).ToList();
            }
        }

        public int InsertEntities<TEntity>(params TEntity[] entities) where TEntity : class
        {
            using (var context = new SqlServerDbContext(Const.DBCONNECT)) {
                context.Set<TEntity>().AddRange(entities);
                return context.SaveChanges();
            }
        }

        public IQueryable<TEntity> QueryEntities<TEntity>() where TEntity : class
        {
            var context = new SqlServerDbContext(Const.DBCONNECT);
            return context.Set<TEntity>().AsNoTracking().AsQueryable();
        }

    }
}
