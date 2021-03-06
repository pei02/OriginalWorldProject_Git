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

    [MetadataType(typeof(MetadataWriter_application))]
    public partial class Writer_application
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Writer_application()
        {
            this.Writer = new HashSet<Writer>();
        }
    
        public int Writer_application_ID { get; set; }
        public string Pseudonym { get; set; }
        public string Introduction { get; set; }
        public string Works_name { get; set; }
        public string Work_preview { get; set; }
        public string Approval_status_ID { get; set; }
        public string Administrator_ID { get; set; }
        public string MemberID { get; set; }
        public Nullable<System.DateTime> Review_date { get; set; }
        public System.DateTime Date_of_Application { get; set; }
    
        public virtual Administrator Administrator { get; set; }
        public virtual Approval_status Approval_status { get; set; }
        public virtual Member Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Writer> Writer { get; set; }
    }
}
