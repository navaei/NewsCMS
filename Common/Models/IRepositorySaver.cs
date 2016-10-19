using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mn.NewsCms.Common.Models
{
    public interface IRepositorySaver
    {
        void AddItems(List<FeedItem> items);       

        bool AddItem(FeedItem item);      

    }
}
