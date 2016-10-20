using Mn.Framework.Web.Model;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Web.WebLogic.BaseModel
{
    public abstract class BaseViewModel : IBaseViewModel<long>
    {
        public long Id { get; set; }

        public override string ToString()
        {
            return base.ToString().Split('.')[base.ToString().Split('.').Length - 1].Replace("ViewModel", "");
        }
    }

    public abstract class BaseViewModel<TBaseEntity, TPrimaryKey> : IBaseViewModel<TPrimaryKey>
        where TBaseEntity : BaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        public TPrimaryKey Id { get; set; }
        public override string ToString()
        {
            return base.ToString().Split('.')[base.ToString().Split('.').Length - 1].Replace("ViewModel", "");
        }
        public TModel ToModel<TModel>() where TModel : class ,new()
        {
            return Common.Helper.AutoMapper.Map<TModel>(this);
        }
        public TModel ToModel<TModel>(TModel dbEntity) where TModel : BaseEntity<TPrimaryKey>, new()
        {
            Common.Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TModel;
        }
    }

    public abstract class BaseViewModel<TBaseEntity> : IBaseViewModel<long>
        where TBaseEntity : BaseEntity
    {
        public long Id { get; set; }
        public string PageTitle { get; set; }
        public override string ToString()
        {
            return base.ToString().Split('.')[base.ToString().Split('.').Length - 1].Replace("ViewModel", "");
        }
        public TModel ToModel<TModel>() where TModel : class ,new()
        {
            return Mn.NewsCms.Common.Helper.AutoMapper.Map<TModel>(this);
        }
        public TModel ToModel<TModel>(TModel dbEntity) where TModel : BaseEntity, new()
        {
            Common.Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TModel;
        }
    }
}
