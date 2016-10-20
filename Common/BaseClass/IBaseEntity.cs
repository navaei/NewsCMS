using System;

namespace Mn.NewsCms.Common.BaseClass
{
    public interface IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        TPrimaryKey Id { get; set; }
        TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new();
    }
    public interface IBaseEntity : IBaseEntity<Int64>
    {
        Int64 Id { get; set; }
        TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new();
    }
}
