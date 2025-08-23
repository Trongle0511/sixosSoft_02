using Microsoft.AspNetCore.Mvc;
using SixOs_Soft_demo_01.Service.S0403.SI0403;

namespace SixOs_Soft_demo_01.Controllers.C0403
{
    // đổi thành link của mình
    [Route("bao_cao_hoat_dong_kham_benh")]
    public class C0403TenGoiNhoController : Controller
    {
        //private string _maChucNang = "/bao_cao_hoat_dong_kham_benh";
        //private IMemoryCachingServices _memoryCache;

        private readonly I0403BCHoatDongKhamService _service;
        public C0403TenGoiNhoController(I0403BCHoatDongKhamService service /*, IMemoryCachingServices memoryCache*/)
        {
            _service = service;
            //_memoryCache = memoryCache;
        }
        public async Task<IActionResult> Index()
        {
            //var quyenVaiTro = await _memoryCache.getQuyenVaiTro(_maChucNang);
            //if (quyenVaiTro == null)
            //{
            //    return RedirectToAction("NotFound", "Home");
            //}
            //ViewBag.quyenVaiTro = quyenVaiTro;
            //ViewData["Title"] = CommonServices.toEmptyData(quyenVaiTro);

            ViewBag.quyenVaiTro = new
            {
                Them = true,
                Sua = true,
                Xoa = true,
                Xuat = true,
                CaNhan = true,
                Xem = true,
            };
            return View("~/Views/V0403/V0403bchdkhambenh/Index.cshtml");
        }
    }
}
