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
    
    public partial class TAIKHOAN
    {
        public int MA_TK { get; set; }
        public int MA_NV { get; set; }
        public string TENDANGNHAP_TK { get; set; }
        public string MATKHAU_TK { get; set; }
        public string QUYEN_TK { get; set; }
    
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
