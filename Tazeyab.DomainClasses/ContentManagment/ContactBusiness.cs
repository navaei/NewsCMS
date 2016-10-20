using System.Linq;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Content;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class ContactBusiness : BaseBusiness<ContactMessage>, IContactBusiness
    {
        public ContactBusiness(IUnitOfWork dbContext) : base(dbContext)
        {
        }

        public IQueryable<ContactMessage> GetList()
        {
            return base.GetList();
        }
        public OperationStatus AddContact(ContactMessage contact)
        {
            return base.Create(contact);
        }
        public OperationStatus Edit(ContactMessage contact)
        {
            return base.Update(contact);
        }
    }
}
