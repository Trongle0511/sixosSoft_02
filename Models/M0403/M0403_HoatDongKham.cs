
using System.ComponentModel.DataAnnotations.Schema;

namespace SixOs_Soft_demo_01.Models.M0403
{
    [Table("T0403_BCHoatDongKhamBenh")]
    public class M0403_HoatDongKham
    {
        public string? TenDichVu { get; set; }
        public DateTime NgayKhamBenh { get; set; }
        public int TongSoLanKham { get; set; }
        public int YHCT_SoLanKham { get; set; }
        public int TE6Tuoi_SLK { get; set; }
        public int BHYT_SLK { get; set; }
        public int VienPhi_SLK { get; set; }
        public int KhongThuDuoc_SLK { get; set; }
        public int CapCuu_SLK { get; set; }
        public int SoNguoiBenhVaoVien { get; set; }
        public int SoNguoiBenhChuyenVien { get; set; }
        public int SoNguoiBenh_DTNT { get; set; }
        public int YHCT_DTNT { get; set; }
        public int TE6Tuoi_DTNT { get; set; }
        public int SoNgay_DTNT { get; set; }
    }
}
