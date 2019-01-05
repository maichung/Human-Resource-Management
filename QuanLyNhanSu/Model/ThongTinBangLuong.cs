using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhanSu.Model
{
    public class ThongTinBangLuong
    {
        public NHANVIEN NhanVien { get; set; }
        public KHOANLUONG KhoanLuong { get; set; }
        public int CalculatedLuong { get; set; }
    }
}
