using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Guanwu.Notify.Widget
{
    public interface IDbContext : ICrossDomainWidget
    {
        List<TElement> FromSql<TElement>(string sql, params object[] parameters);
        IQueryable<TEntity> QueryEntities<TEntity>() where TEntity : class;
        int InsertEntities<TEntity>(params TEntity[] entities) where TEntity : class;
        void Reference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> navigationProperty) where TEntity : class where TProperty : class;
        void Collection<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty) where TEntity : class where TElement : class;
    }
}
