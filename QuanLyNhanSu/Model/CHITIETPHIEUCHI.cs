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
    
    public partial class CHITIETPHIEUCHI : BaseViewModel
    {
        private int _MA_CTPC;
        public int MA_CTPC { get => _MA_CTPC; set { _MA_CTPC = value; OnPropertyChanged(); } }
        private int _MA_PC;
        public int MA_PC { get => _MA_PC; set { _MA_PC = value; OnPropertyChanged(); } }
        private string _NOIDUNG_CTPC;
        public string NOIDUNG_CTPC { get => _NOIDUNG_CTPC; set { _NOIDUNG_CTPC = value; OnPropertyChanged(); } }
        private Nullable<decimal> _TRIGIA_CTPC;
        public Nullable<decimal> TRIGIA_CTPC { get => _TRIGIA_CTPC; set { _TRIGIA_CTPC = value; OnPropertyChanged(); } }

        public virtual PHIEUCHI PHIEUCHI { get; set; }
    }
}
