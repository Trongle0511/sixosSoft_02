using Microsoft.EntityFrameworkCore;

namespace SixOs_Soft_demo_01.Models.M0403
{
        public class M0403BCHoatDongKhamSTO : DbContext
        {
            // Constructor nhận options từ DI
            public M0403BCHoatDongKhamSTO(DbContextOptions<M0403BCHoatDongKhamSTO> options) : base(options)
            {
            }

            // DbSet cho M0403_HoatDongKham để FromSqlRaw/Interpolated dùng
            public DbSet<M0403_HoatDongKham> HoatDongKhamBenh { get; set; }


            // Nếu muốn, bạn có thể override OnModelCreating
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Map tên table chính xác nếu cần
                modelBuilder.Entity<M0403_HoatDongKham>()
                    .ToTable("T0403_BCHoatDongKhamBenh").HasNoKey();


                // Nếu key nào đó cần khai báo (ví dụ Id), map ở đây
                // modelBuilder.Entity<M0403_HoatDongKham>().HasKey(x => x.Id);
            }
        

    }
}
