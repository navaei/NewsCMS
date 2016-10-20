using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mn.NewsCms.Common.BaseClass;

namespace Mn.NewsCms.Common.BaseClass
{
    public class BaseBusiness<TEntity, TPrimaryKey> : IDisposable, NewsCms.Common.BaseClass.IBaseBusiness<TEntity, TPrimaryKey>
        where TEntity : class, Mn.NewsCms.Common.BaseClass.IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {

        Mn.NewsCms.Common.BaseClass.IUnitOfWork _dbContext;
        public BaseBusiness(Mn.NewsCms.Common.BaseClass.IUnitOfWork dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return _dbContext.Set<TEntity>().Where(predicate).SingleOrDefault();
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }
        public virtual TEntity Get(TPrimaryKey Id)
        {
            return GetList().SingleOrDefault(x => x.Id.Equals(Id));
        }

        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _dbContext.Set<TEntity>().Where(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetList<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy)
        {
            try
            {
                return GetList(predicate).OrderBy(orderBy);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetList<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            try
            {
                return GetList().OrderBy(orderBy);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetList()
        {
            return GetList<TEntity>();
        }
        public virtual IQueryable<T> GetList<T>() where T : class, IBaseEntity<TPrimaryKey>
        {
            try
            {
                return _dbContext.Set<T>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public virtual IQueryable<T> GetList2<T>() where T : BaseEntity
        {
            try
            {
                return _dbContext.Set<T>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> Include(string relatedEntity)
        {
            try
            {
                return _dbContext.Set<TEntity>().Include(relatedEntity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] selectors)
        {
            try
            {
                var contx = _dbContext.Set<TEntity>();
                foreach (var selector in selectors)
                    contx.Include(selector);
                return contx;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public virtual IQueryable<TEntity> Include<TEntity>(this ObjectQuery<TEntity> query, Expression<Func<TEntity, object>> selector)
        //{
        //    string propertyName = GetPropertyName(selector);
        //    return query.Include(propertyName);
        //}
        public virtual OperationStatus Save()
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                opStatus.Status = _dbContext.SaveAllChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error saving " + typeof(TEntity) + ".", exp);
            }

            return opStatus;
        }

        public virtual OperationStatus Create(TEntity entity)
        {
            return Create<TEntity>(entity);
        }
        public virtual OperationStatus Create<T>(T entity) where T : class, Mn.NewsCms.Common.BaseClass.IBaseEntity<TPrimaryKey>
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                _dbContext.Set<T>().Add(entity);
                opStatus.Status = _dbContext.SaveAllChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }

            return opStatus;
        }

        public virtual OperationStatus Delete(TEntity entity)
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                _dbContext.Set<TEntity>().Remove(entity);
                opStatus.Status = _dbContext.SaveAllChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }

            return opStatus;
        }

        public virtual OperationStatus Delete(TPrimaryKey entityId)
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                var entity = _dbContext.Set<TEntity>().Find(entityId);
                _dbContext.Set<TEntity>().Remove(entity);
                opStatus.Status = _dbContext.SaveAllChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }

            return opStatus;
        }
        public virtual OperationStatus Delete<T>(TPrimaryKey entityId) where T : class, Mn.NewsCms.Common.BaseClass.IBaseEntity<TPrimaryKey>
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                var entity = _dbContext.Set<T>().Find(entityId);
                _dbContext.Set<T>().Remove(entity);
                opStatus.Status = _dbContext.SaveAllChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }

            return opStatus;
        }
        public virtual OperationStatus DeleteWhere(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                foreach (var entity in query)
                {
                    _dbContext.Entry(entity).State = EntityState.Deleted;
                }
                var count = _dbContext.SaveAllChanges();
                opStatus.Status = count > 0;
                opStatus.RecordsAffected = count;
                return opStatus;
            }
            catch (Exception exp)
            {
                return OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
            }
        }
        public virtual OperationStatus Update(TEntity entity)
        {
            return Update<TEntity>(entity);
        }
        public virtual OperationStatus Update<T>(T entity) where T : class, IBaseEntity<TPrimaryKey>
        {

            OperationStatus opStatus = new OperationStatus { Status = true };
            if (entity == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = _dbContext.Entry<T>(entity);
            //entity.MetaData.DateUpdated = DateTime.UtcNow;

            var set = _dbContext.Set<TEntity>();
            TEntity attachedEntity = set.Local.SingleOrDefault(e => e.Id.Equals(entity.Id));  // You need to have access to key

            if (attachedEntity != null)
            {
                var attachedEntry = _dbContext.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
            }
            else
            {
                entry.State = EntityState.Modified; // This should attach entity
            }
            try
            {
                opStatus.Status = _dbContext.SaveAllChanges() > 0;

            }
            catch (Exception ex)
            {
                opStatus = OperationStatus.CreateFromException("Error updating " + typeof(TEntity) + ".", ex);
                opStatus.Exception = ex;
            }
            // }

            return opStatus;
        }

        public virtual OperationStatus UpdateProperty(TPrimaryKey entityId, string propertyName, object propertyValue)
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                var entity = _dbContext.Set<TEntity>().Find(entityId);
                _dbContext.Entry(entity).Property(propertyName).CurrentValue = propertyValue;
                opStatus.Status = _dbContext.SaveAllChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus.Exception = exp;
                opStatus = OperationStatus.CreateFromException("Error updating " + typeof(TEntity) + ".", exp);
            }

            return opStatus;
        }

        public OperationStatus SqlCommandExecute(string cmdText, params object[] parameters)
        {
            var opStatus = new OperationStatus { Status = true };

            try
            {
                //opStatus.RecordsAffected = DataContext.ExecuteStoreCommand(cmdText, parameters);
                //TODO: [Papa] = Have not tested this yet.
                opStatus.RecordsAffected = _dbContext.Database.ExecuteSqlCommand(cmdText, parameters);
            }
            catch (Exception exp)
            {
                OperationStatus.CreateFromException("Error executing store command: ", exp);
            }
            return opStatus;
        }

        public Task<List<T>> SqlCommandSelect<T>(string cmdText, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery<T>(cmdText, parameters).ToListAsync();
        }

        public void Dispose()
        {
            //if (DataContext != null)
            //    DataContext.Dispose();
            //if (DataContext != null)
            //{
            //    DataContext.Dispose();
            //    //_dataContext = null;
            //}
        }

        #region privateMethod
        //private string GetPropertyName<T>(Expression<Func<T, object>> expression)
        //{
        //    MemberExpression memberExpr = expression.Body as MemberExpression;
        //    if (memberExpr == null)
        //        throw new ArgumentException("Expression body must be a member expression");
        //    return memberExpr.Member.Name;
        //}
        #endregion
    }

    public class BaseBusiness<TEntity> : BaseBusiness<TEntity, long>
          where TEntity : class, IBaseEntity<long>
    {
        public BaseBusiness(IUnitOfWork dbContext) : base(dbContext)
        {
        }
    }
}
