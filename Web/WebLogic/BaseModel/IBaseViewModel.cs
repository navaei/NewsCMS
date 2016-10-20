using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Web.Model
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
