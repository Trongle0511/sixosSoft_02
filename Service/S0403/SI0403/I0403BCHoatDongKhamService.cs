
using SixOs_Soft_demo_01.Models.M0403;
using SixOs_Soft_demo_01.Models.M0403.DTO0403;


namespace SixOs_Soft_demo_01.Service.S0403.SI0403
{
    public interface I0403BCHoatDongKhamService
    {
        Task<List<M0403_HoatDongKham>> GetHoatDongKhamBenh( HoatDongKhamRequest dto);
    }
}
