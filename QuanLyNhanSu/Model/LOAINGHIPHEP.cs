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

    public partial class LOAINGHIPHEP : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOAINGHIPHEP()
        {
            this.KHOANNGHIPHEP = new HashSet<KHOANNGHIPHEP>();
        }

        private int _MA_LNP;
        public int MA_LNP { get => _MA_LNP; set { _MA_LNP = value; OnPropertyChanged(); } }
        private string _TEN_LNP;
        public string TEN_LNP { get => _TEN_LNP; set { _TEN_LNP = value; OnPropertyChanged(); } }
        private Nullable<bool> _COLUONG_LNP;
        public Nullable<bool> COLUONG_LNP { get => _COLUONG_LNP; set { _COLUONG_LNP = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KHOANNGHIPHEP> KHOANNGHIPHEP { get; set; }
    }
}
