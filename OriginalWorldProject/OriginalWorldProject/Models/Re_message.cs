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
    
    public partial class Re_message
    {
        public int Re_message_num { get; set; }
        public string Moodsymbol_num { get; set; }
        public int RtamessageID { get; set; }
    
        public virtual Moodshee Moodshee { get; set; }
        public virtual Rta_message Rta_message { get; set; }
    }
}
