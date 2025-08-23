using Microsoft.EntityFrameworkCore;
using SixOs_Soft_demo_01.Data;
using SixOs_Soft_demo_01.Models.M0403;
using SixOs_Soft_demo_01.Models.M0403.DTO0403;

namespace SixOs_Soft_demo_01.Service.S0403.SI0403
{

    public class S0403BCHoatDongKham : I0403BCHoatDongKhamService
    {
        private readonly AppDbContext _context;

        public S0403BCHoatDongKham(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<M0403_HoatDongKham>> GetHoatDongKhamBenh(HoatDongKhamRequest dto)
        {
            var result = await _context.HoatDongKhamBenh
                .FromSqlRaw("EXEC S0403_BCHoatDongKhamBenh @TuNgay={0}, @DenNgay={1}, @IdChiNhanh={2}",
                    dto.TuNgay,
                    dto.DenNgay,
                    dto.IdChiNhanh)
                .ToListAsync();

            return result;
        }
    }
}
