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
    
    public partial class BANGLUONG : BaseViewModel
    {
        private int _MA_BL;
        public int MA_BL { get => _MA_BL; set { _MA_BL = value; OnPropertyChanged(); } }
        private int _MA_NV;
        public int MA_NV { get => _MA_NV; set { _MA_NV = value; OnPropertyChanged(); } }
        private Nullable<System.DateTime> _THANG_BL;
        public Nullable<System.DateTime> THANG_BL { get => _THANG_BL; set { _THANG_BL = value; OnPropertyChanged(); } }
        private Nullable<decimal> _TONGLUONG_BL;
        public Nullable<decimal> TONGLUONG_BL { get => _TONGLUONG_BL; set { _TONGLUONG_BL = value; OnPropertyChanged(); } }
    
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}