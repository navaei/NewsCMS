using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mn.NewsCms.Common
{
    public partial class UpdateDuration : BaseEntity
    {
        public UpdateDuration()
        {
            this.Feeds = new List<Feed>();
        }

        [Column("UpdateDurationId")]
        public override long Id
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
        public string Title { get; set; }
        public int PriorityLevel { get; set; }
        public string Code { get; set; }
        public string DelayTime { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<int> MaxSleepTime { get; set; }
        public Nullable<bool> IsParting { get; set; }
        public Nullable<bool> IsLocalyUpdate { get; set; }
        public int FeedsCount { get; set; }
        public int StartIndex { get; set; }
        public virtual ICollection<Feed> Feeds { get; set; }
        public string ServiceLink { get; set; }
        public int StartSleepTimeHour { get; set; }
        public int EndSleepTimeHour { get; set; }
        public bool EnabledForUpdate { get; set; }
    }
}
