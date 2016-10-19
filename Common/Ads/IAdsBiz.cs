using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common.Models;

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
