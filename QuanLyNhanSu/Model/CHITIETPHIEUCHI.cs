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
    
    public partial class CHITIETPHIEUCHI
    {
        public int MA_CTPC { get; set; }
        public int MA_PC { get; set; }
        public string NOIDUNG_CTPC { get; set; }
        public Nullable<decimal> TRIGIA_CTPC { get; set; }
    
        public virtual PHIEUCHI PHIEUCHI { get; set; }
    }
}
