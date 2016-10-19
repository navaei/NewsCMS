using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class PostBiz : BaseBusiness<Post>, IPostBiz
    {
        public Post Get(long postId)
        {
            return base.GetList().SingleOrDefault(p => p.Id == postId);
        }
        public Post Get(string name)
        {
            return base.GetList().SingleOrDefault(p => p.Name.ToLower() == name.ToLower());
        }
        public OperationStatus CreateEdit(Post post)
        {
            if (string.IsNullOrEmpty(post.Name))
                post.Name = post.Title.RemoveBadCharacterInURL();

            if (post.Id == 0)
            {
                post.PublishDate = post.PublishDate.HasValue ? post.PublishDate.Value : DateTime.Now;
                post.MetaData = new Mn.Framework.Common.Model.MetaData()
                {
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsDeleted = false
                };
                return base.Create(post);
            }
            else
            {
                post.MetaData.DateUpdated = DateTime.Now;
                return base.Update(post);
            }
        }

        public IQueryable<Post> GetList()
        {
            return base.GetList();
        }

        public OperationStatus Delete(Post post)
        {
            post.MetaData.IsDeleted = true;
            return this.CreateEdit(post);
        }

        public OperationStatus Delete(int postId)
        {
            var post = this.Get(postId);
            post.MetaData.IsDeleted = true;
            return this.CreateEdit(post);
        }
    }
}
