using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    public sealed class DbContextProxy : IDbContext
    {
        private static readonly Lazy<SqlServerDbContext> LazyContext =
            new Lazy<SqlServerDbContext>(() => new SqlServerDbContext(Const.DBCONNECT));
        private SqlServerDbContext Context => LazyContext.Value;

        public string WidgetName => Const.WIDGET_NAME;

        public List<TElement> FromSql<TElement>(string sql, params object[] parameters)
        {
            return Context.Database.SqlQuery<TElement>(sql, parameters).ToList();
        }

        public int InsertEntities<TEntity>(params TEntity[] entities) where TEntity : class
        {
            using (var db = new SqlServerDbContext(Const.DBCONNECT)) {
                foreach (var entity in entities)
                    db.Set<TEntity>().Add(entity);
                return db.SaveChanges();
            }
        }

        public IQueryable<TEntity> QueryEntities<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public void Reference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> navigationProperty)
            where TEntity : class
            where TProperty : class
        {
            Context.Entry(entity).Reference(navigationProperty).Load();
        }

        public void Collection<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
            where TEntity : class
            where TElement : class
        {
            Context.Entry(entity).Collection(navigationProperty).Load();
        }

    }
}
