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
    
    public partial class Track_work
    {
        public int Track_workID { get; set; }
        public string MemberID { get; set; }
        public string WorksID { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual Works Works { get; set; }
    }
}
