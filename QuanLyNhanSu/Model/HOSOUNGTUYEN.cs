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
    
    public partial class HOSOUNGTUYEN
    {
        public int MA_HSUT { get; set; }
        public int MA_UV { get; set; }
        public string VITRICONGVIEC_HSUT { get; set; }
        public Nullable<System.DateTime> NGAYNOP_HSUT { get; set; }
        public string TRANGTHAI_HSUT { get; set; }
        public byte[] CV_HSUT { get; set; }
    
        public virtual UNGVIEN UNGVIEN { get; set; }
    }
}
