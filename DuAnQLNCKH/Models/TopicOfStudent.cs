//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuAnQLNCKH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TopicOfStudent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TopicOfStudent()
        {
            this.DetailStatementSts = new HashSet<DetailStatementSt>();
        }
    
        public string IdTp { get; set; }
        public string Name { get; set; }
        public string NameSt { get; set; }
        public string IdSV { get; set; }
        public string Emmail { get; set; }
        public int IdP { get; set; }
        public DateTime DateSt { get; set; }
        public Nullable<int> Times { get; set; }
        public Nullable<double> Expense { get; set; }
        public string Status { get; set; }
        public string Progress { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailStatementSt> DetailStatementSts { get; set; }
        public virtual PointTable PointTable { get; set; }
    }
}