//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace OriginalWorldProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(MetadataAnnouncement))]
    public partial class Announcement
    {
        public int Announcement_ID { get; set; }
        public string Announcement_title { get; set; }
        public string Announcement_content { get; set; }
        public System.DateTime Announcement_time { get; set; }
        public string Announcement_type_ID { get; set; }
        public string Administrator_ID { get; set; }
        public bool Ann_Display { get; set; }
    
        public virtual Administrator Administrator { get; set; }
        public virtual Announcement_type Announcement_type { get; set; }
    }
}
