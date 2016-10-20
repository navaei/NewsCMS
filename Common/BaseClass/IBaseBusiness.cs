using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mn.NewsCms.Common.BaseClass
{
    public interface IBaseBusiness<TEntity, TPrimaryKey>
        where TEntity : Mn.NewsCms.Common.BaseClass.IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetList<TKey>(Expression<Func<TEntity, bool>> predicate,
                                          Expression<Func<TEntity, TKey>> orderBy);

        IQueryable<TEntity> GetList<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IQueryable<TEntity> GetList();

        OperationStatus Save();
        OperationStatus Create(TEntity entity);
        OperationStatus Delete(TEntity entity);
        OperationStatus Delete(TPrimaryKey entityId);
        OperationStatus DeleteWhere(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        OperationStatus Update(TEntity entity);
        OperationStatus UpdateProperty(TPrimaryKey entityId, string propertyName, object propertyValue);

        OperationStatus SqlCommandExecute(string cmdText, params object[] parameters); 
        Task<List<T>> SqlCommandSelect<T>(string cmdText, params object[] parameters);
    }

    public interface IBaseBusiness<TEntity> : IBaseBusiness<TEntity, long>
        where TEntity : Mn.NewsCms.Common.BaseClass.IBaseEntity<long>
    {

    }
}
