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
    
    public partial class CHAMCONGNGAY : BaseViewModel
    {
        private int _MA_CCN;
        public int MA_CCN { get => _MA_CCN; set { _MA_CCN = value; OnPropertyChanged(); } }
        private int _MA_NV;
        public int MA_NV { get => _MA_NV; set { _MA_NV = value; OnPropertyChanged(); } }
        private int _MA_LCC;
        public int MA_LCC { get => _MA_LCC; set { _MA_LCC = value; OnPropertyChanged(); } }
        private Nullable<System.DateTime> _THOIGIANBATDAU_CCN;
        public Nullable<System.DateTime> THOIGIANBATDAU_CCN { get => _THOIGIANBATDAU_CCN; set { _THOIGIANBATDAU_CCN = value; OnPropertyChanged(); } }
        private Nullable<System.DateTime> _THOIGIANKETTHUC_CCN;
        public Nullable<System.DateTime> THOIGIANKETTHUC_CCN { get; set; }
    
        public virtual LOAICHAMCONG LOAICHAMCONG { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
