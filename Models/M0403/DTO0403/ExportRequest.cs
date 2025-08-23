

namespace SixOs_Soft_demo_01.Models.M0403.DTO0403
{
    public class ExportRequest
    {
        public int IdChiNhanh { get; set; }

        public string TenChiNhanh { get; set; }
        public String? TuNgay { get; set; }
        public String? DenNgay { get; set; }
        public List<HoatDongKhamDto> Data { get; set; }
    }

}
