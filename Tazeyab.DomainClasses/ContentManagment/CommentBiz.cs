using Mn.NewsCms.Common.BaseClass;
using System;
using System.Linq;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class CommentBiz : BaseBusiness<Comment>, ICommentBiz
    {

        public CommentBiz(IUnitOfWork dbContext) : base(dbContext)
        {
        }

        public Comment Get(int id)
        {
            return this.GetList().SingleOrDefault(c => c.Id == id);
        }
        public IQueryable<Comment> GetList(int postId)
        {
            return this.GetList().Where(c => c.PostId == postId);
        }

        public IQueryable<Comment> GetList()
        {
            return base.GetList();
        }

        public OperationStatus CreateEdit(Comment comment)
        {
            OperationStatus status;
            if (comment.Id == 0)
            {
                comment.CreateDate = DateTime.Now;
                status = base.Create(comment);
            }
            else
            {
                comment.UpdateDate = DateTime.Now;
                status = base.Update(comment);
            }

            return status;
        }
        public OperationStatus Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
