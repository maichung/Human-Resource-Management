using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhanSu.Model
{
    public class ThongTinChamCong
    {
        public NHANVIEN NhanVien { get; set; }
        public bool HanhChinh { get; set; }
        public bool TangCa { get; set; }        
        public DateTime? GioBatDau { get; set; }
        public DateTime? GioKetThuc { get; set; }
        public DateTime? NgayChamCong { get; set; }

        public ThongTinChamCong() { }

        public ThongTinChamCong(ThongTinChamCong a)
        {
            NhanVien = a.NhanVien;
            HanhChinh = a.HanhChinh;
            TangCa = a.TangCa;
            GioBatDau = a.GioBatDau;
            GioKetThuc = a.GioKetThuc;
            NgayChamCong = a.NgayChamCong;
        }
    }
}
