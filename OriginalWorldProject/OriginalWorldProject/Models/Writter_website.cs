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
    
    public partial class Writter_website
    {
        public string Website_ID { get; set; }
        public string Website_link { get; set; }
        public string Website_type_ID { get; set; }
        public string WriterhomeID { get; set; }
    
        public virtual Website_type Website_type { get; set; }
        public virtual Writer_home Writer_home { get; set; }
    }
}