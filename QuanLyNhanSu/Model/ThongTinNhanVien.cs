using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuanLyNhanSu.Model
{
    public class ThongTinNhanVien
    {
        public NHANVIEN NhanVien { get; set; }
        public String NgaySinh { get; set; }
        public BitmapImage Avatar { get; set; }
    }
}
