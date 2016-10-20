using System.Linq;
using Mn.NewsCms.Common.Models;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

namespace Mn.NewsCms.Common
{
    public interface IAdsBiz
    {
        IQueryable<Ad> GetList();
        IQueryable<Ad> GetList(int? tagId, int? catId);
        OperationStatus CreateEdit(Ad ads);
        string GetAds(int width, int? catId, int? tagId);
    }
}
