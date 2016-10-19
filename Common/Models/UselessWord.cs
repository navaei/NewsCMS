using System;
using System.Collections.Generic;

namespace Mn.NewsCms.Common.Models
{
    public partial class UselessWord
    {
        public decimal Id { get; set; }
        public string Value { get; set; }
        public Nullable<int> WordType { get; set; }
    }
}
