//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlarmTagProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblAlarmField
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAlarmField()
        {
            this.tblAlarmFieldsDatas = new HashSet<tblAlarmFieldsData>();
        }
    
        public int FieldID { get; set; }
        public string Field_Type { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAlarmFieldsData> tblAlarmFieldsDatas { get; set; }
    }
}