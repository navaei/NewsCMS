using System;
using System.ComponentModel.DataAnnotations;

namespace Mn.NewsCms.Common.BaseClass
{
    public class MetaData
    {
        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; } // even thought not required, EF designates the col as not null, must provide value
        public bool IsDeleted { get; set; }
    }
}
