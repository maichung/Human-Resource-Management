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
    
    public partial class LICHSUNHANVIEN
    {
        public int MA_LSNV { get; set; }
        public int MA_NV { get; set; }
        public string MOTA_LSNV { get; set; }
        public Nullable<System.DateTime> THOIGIAN_LSNV { get; set; }
    
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}