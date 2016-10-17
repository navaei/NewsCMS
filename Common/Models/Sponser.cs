using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public partial class Sponser
    {
        public int Id { get; set; }
        public string SponserName { get; set; }
        public string SponserLink { get; set; }
        public string SponserImage { get; set; }
        public string OfficalWebSite { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string KeyWords { get; set; }
        public Nullable<int> Actived { get; set; }
        public string Controller { get; set; }
        public string Description { get; set; }
    }
}
