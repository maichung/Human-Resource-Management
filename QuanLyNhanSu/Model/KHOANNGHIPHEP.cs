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
    using QuanLyNhanSu.ViewModel;
    using System;
    using System.Collections.Generic;
    
    public partial class KHOANNGHIPHEP : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHOANNGHIPHEP()
        {
            this.NGHIPHEP = new HashSet<NGHIPHEP>();
        }
    
        private int _MA_KNP;
        public int MA_KNP { get => _MA_KNP; set { _MA_KNP = value; OnPropertyChanged(); } }
        private int _MA_NV;
        public int MA_NV { get => _MA_NV; set { _MA_NV = value; OnPropertyChanged(); } }
        private int _MA_LNP;
        public int MA_LNP { get => _MA_LNP; set { _MA_LNP = value; OnPropertyChanged(); } }
        private Nullable<int> _SONGAYNGHI_KNP;
        public Nullable<int> SONGAYNGHI_KNP { get => _SONGAYNGHI_KNP; set { _SONGAYNGHI_KNP = value; OnPropertyChanged(); } }

        public virtual LOAINGHIPHEP LOAINGHIPHEP { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NGHIPHEP> NGHIPHEP { get; set; }
    }
}
