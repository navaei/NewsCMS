using Mn.Framework.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mn.NewsCms.Common
{
    public class PhotoItemBaseAttribute
    {

    }
    public class NewspaperItemAttribute : PhotoItemBaseAttribute
    {
        public string Cat { get; set; }
        public string WebSite { get; set; }
    }
    public enum PhotoType : byte
    {
        NewsPaper = 0
    }
    public class PhotoItem : BaseEntity<long>
    {
        [Column("PhotoItemId")]
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
        public PhotoType PhotoType { get; set; }
        [MaxLength(256)]
        public string PhotoURL { get; set; }
        [MaxLength(256)]
        public string PhotoThumbnail { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }

        [Column("Attributes")]
        [MaxLength(512)]
        public string AttributesSerialized { get; set; }
       
        [NotMapped]
        public PhotoItemBaseAttribute Attribute
        {
            get
            {
                if (PhotoType == Common.PhotoType.NewsPaper)
                {
                    return JsonConvert.DeserializeObject<NewspaperItemAttribute>(AttributesSerialized);
                }

                return new PhotoItemBaseAttribute();
            }
            set
            {
                AttributesSerialized = JsonConvert.SerializeObject(value);
            }
        }
        public Nullable<long> CatId { get; set; }
        public DateTime? CreationDate { get; set; }

        [ForeignKey("CatId")]
        public virtual Category Category { get; set; }
    }
}
