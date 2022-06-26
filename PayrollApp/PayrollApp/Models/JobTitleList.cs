//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayrollApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobTitleList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobTitleList()
        {
            this.EmployeeLists = new HashSet<EmployeeList>();
        }
    
        public int ID { get; set; }
        public int JobCode { get; set; }
        public string JobName { get; set; }
        public string JobDesc { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeList> EmployeeLists { get; set; }
    }
}