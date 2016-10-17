using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common.Models;

namespace Tazeyab.Common.Content
{
    public interface IContactBusiness
    {
        IQueryable<ContactMessage> GetList();
        OperationStatus AddContact(ContactMessage contact);
        OperationStatus Edit(ContactMessage contact);
    }
}
