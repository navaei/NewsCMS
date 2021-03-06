﻿using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mn.NewsCms.Common.Models
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
