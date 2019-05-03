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
        //private static readonly Lazy<SqlServerDbContext> LazyContext =
        //    new Lazy<SqlServerDbContext>(() => new SqlServerDbContext(Const.DBCONNECT));
        //private SqlServerDbContext Context => LazyContext.Value;

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

        //public void Reference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> navigationProperty)
        //    where TEntity : class
        //    where TProperty : class
        //{

        //    Context.Entry(entity).Reference(navigationProperty).Load();
        //}

        //public void Collection<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
        //    where TEntity : class
        //    where TElement : class
        //{
        //    lock (Context) {
        //        Context.Entry(entity).Collection(navigationProperty).Load();
        //    }
        //}

    }
}
