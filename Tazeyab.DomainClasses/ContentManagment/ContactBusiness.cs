using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common.Content;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class ContactBusiness : BaseBusiness<ContactMessage>, IContactBusiness
    {
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
