using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Model;

namespace Tazeyab.Common.Bots
{
    public class AudioMessage : BaseEntity
    {
        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }

        [MaxLength(128)]
        public string ChatMessage { get; set; }
        public long ChatId { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
