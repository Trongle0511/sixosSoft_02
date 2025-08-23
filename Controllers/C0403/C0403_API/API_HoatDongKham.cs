using Microsoft.AspNetCore.Mvc;
using SixOs_Soft_demo_01.Service.S0403.SI0403;
using SixOs_Soft_demo_01.Models.M0403.DTO0403;


namespace SixOs_Soft_demo_01.Controllers.C0403.C0403_API
{
    [Route("api/hoat_dong_kham")]
    [ApiController]
    public class API_HoatDongKham : Controller
    {
        private readonly I0403BCHoatDongKhamService _baoCaoService;

        public API_HoatDongKham(I0403BCHoatDongKhamService baoCaoService)
        {
            _baoCaoService = baoCaoService;
        }

        [HttpPost]
        public async Task<IActionResult> GetHoatDongKhamBenhPost([FromBody] HoatDongKhamRequest request)
        {
            try
            {
                var data = await _baoCaoService.GetHoatDongKhamBenh(request);
                return Ok(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
