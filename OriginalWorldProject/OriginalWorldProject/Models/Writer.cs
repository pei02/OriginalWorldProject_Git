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
    
    public partial class Writer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Writer()
        {
            this.Report_table = new HashSet<Report_table>();
            this.Track_writer = new HashSet<Track_writer>();
            this.Works = new HashSet<Works>();
            this.Writer_home = new HashSet<Writer_home>();
        }
    
        public string WriterID { get; set; }
        public string Pseudonym { get; set; }
        public bool W_status { get; set; }
        public string MemberID { get; set; }
        public int Writer_application_ID { get; set; }
    
        public virtual Member Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report_table> Report_table { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Track_writer> Track_writer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Works> Works { get; set; }
        public virtual Writer_application Writer_application { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Writer_home> Writer_home { get; set; }
    }
}