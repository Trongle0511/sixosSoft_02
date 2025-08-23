using System.ComponentModel.DataAnnotations;

namespace SixOs_Soft_demo_01.Models.M0403.DTO0403
{
    public class HoatDongKhamRequest
    {
        [Required]
        public string? TuNgay { get; set; }     // dd-MM-yyyy

        [Required]
        public string? DenNgay { get; set; }    // dd-MM-yyyy

        [Required]
        public int IdChiNhanh { get; set; }

    }
}
