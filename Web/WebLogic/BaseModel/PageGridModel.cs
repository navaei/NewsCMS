﻿namespace Mn.NewsCms.Web.WebLogic.BaseModel
{
    public class PageGridModel
    {
        public PageGridModel()
        {
            GridEvents = "onGridDataBound";
        }
        public string GridEvents { get; set; }
        public ColumnActionMenu GridMenu { get; set; }
    }
}
