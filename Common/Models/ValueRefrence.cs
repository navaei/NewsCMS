using System;
using System.Collections.Generic;

namespace Mn.NewsCms.Common.Models
{
    public partial class ValueRefrence
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public string CoulmnDescription { get; set; }
    }
}
