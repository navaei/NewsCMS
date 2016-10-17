using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tazeyab.Common.Models
{
    public class ProjectSetup : BaseEntity
    {
        [Display(Name = "عنوان")]
        [MaxLength(32)]
        public string Title { get; set; }
        [Display(Name = "مقدار")]
        [MaxLength(1024)]
        public string Value { get; set; }
        [Display(Name = "شرح")]
        [MaxLength(128)]
        public string Meaning { get; set; }
    }
}
