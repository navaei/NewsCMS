using System;
using System.ComponentModel.DataAnnotations;
using Mn.NewsCms.Common.BaseClass;
using IBaseEntity = Mn.NewsCms.Common.BaseClass.IBaseEntity;

namespace Mn.NewsCms.Common
{
    public abstract class BaseEntity : IBaseEntity
    {

        [Key]
        public virtual Int64 Id { get; set; }
        public TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new()
        {
            var vm = new TViewModel();
            vm = Helper.AutoMapper.Map<TViewModel>(this);
            return vm;
        }
        public TViewModel ToViewModel<TViewModel>(TViewModel dbEntity) where TViewModel : class ,new()
        {
            Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TViewModel;
        }
    }
    public abstract class BaseEntity<TPrimaryKey> : BaseClass.IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {

        [Key]
        public virtual TPrimaryKey Id { get; set; }

        public TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new()
        {
            var vm = new TViewModel();
            vm = Helper.AutoMapper.Map<TViewModel>(this);
            return vm;
        }
        public TViewModel ToViewModel<TViewModel>(TViewModel dbEntity) where TViewModel : class ,new()
        {
            Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TViewModel;
        }
    }
}
