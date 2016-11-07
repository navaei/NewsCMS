namespace Mn.NewsCms.WebCore.WebLogic.BaseModel
{
    //public interface IBaseViewModel
    //{
    //    int Id { get; set; }
    //}
    public interface IBaseViewModel<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
