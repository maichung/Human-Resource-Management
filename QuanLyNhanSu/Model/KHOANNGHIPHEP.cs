//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyNhanSu.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class KHOANNGHIPHEP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHOANNGHIPHEP()
        {
            this.NGHIPHEP = new HashSet<NGHIPHEP>();
        }
    
        public int MA_KNP { get; set; }
        public int MA_NV { get; set; }
        public int MA_LNP { get; set; }
        public Nullable<int> SONGAYNGHI_KNP { get; set; }
    
        public virtual LOAINGHIPHEP LOAINGHIPHEP { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NGHIPHEP> NGHIPHEP { get; set; }
    }
}
