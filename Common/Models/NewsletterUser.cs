using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Mn.NewsCms.Common.Models
{
   public class NewsletterUser:BaseEntity<Guid>
    {
       [Column("NewsletterUserID")]
       public override Guid Id
       {
           get
           {
               return base.Id;
           }
           set
           {
               base.Id = value;
           }
       }

       public string Email { get; set; }
       public string BlogTitle { get; set; }
       public string BlogAddress { get; set; }
       public byte? PageRank { get; set; }
    }
}
