using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.NewsCms.Common
{
    public interface IPostBiz
    {
        Post Get(long postId);
        Post Get(string name);
        IQueryable<Post> GetList();
        OperationStatus CreateEdit(Post post);
        OperationStatus Delete(Post post);
        OperationStatus Delete(int postId);
    }
}
