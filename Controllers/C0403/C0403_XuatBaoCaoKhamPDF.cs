using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SixOs_Soft_demo_01.Models.M0403.DTO0403;
using System.Globalization;
using System.IO;

namespace SixOs_Soft_demo_01.Controllers.C0403
{
    [Route("api/xuatbcpdf")]
    [ApiController]
    public class C0403_XuatBaoCaoKhamPDF : ControllerBase
    {
        [HttpPost("exportpdf")]
        public IActionResult ExportPDF([FromBody] ExportRequest request)
        {
            if (request == null || request.Data == null || request.Data.Count == 0)
            {
                return BadRequest("Thiếu dữ liệu báo cáo!");
            }

            try
            {
                DateTime? tuNgay = null, denNgay = null;

                if (!string.IsNullOrEmpty(request.TuNgay))
                    tuNgay = DateTime.ParseExact(request.TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(request.DenNgay))
                    denNgay = DateTime.ParseExact(request.DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(20);

                        // Header
                        page.Header().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                            col.Item().Text("BỆNH VIỆN UNG BƯỚU " + request.TenChiNhanh)
                                    .FontSize(14).Bold();

                                col.Item().Text("Đ/C: Số 12 Đường 400, Phường Tăng Nhơn Phú, TP. Hồ Chí Minh");
                                col.Item().Text("Điện thoại: (028) 36227722 – (028) 38433021");
                                col.Item().Text("Email: bvubhcm@gmail.com");
                            });

                            row.RelativeItem().AlignCenter().Column(col =>
                            {
                                col.Item().Text("BÁO CÁO HOẠT ĐỘNG KHÁM")
                                    .Bold().FontSize(14);
                                col.Item().Text("Đơn vị thống kê")
                                    .Italic().FontSize(10);
                                col.Item().AlignRight().Text($"Từ ngày: {tuNgay:dd/MM/yyyy} Đến ngày: {denNgay:dd/MM/yyyy}")
                                    .FontSize(10);
                            });
                        });

                        // Table content
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30); // STT
                                columns.RelativeColumn(2);  // Dịch vụ
                                columns.ConstantColumn(40); // Tổng số
                                columns.ConstantColumn(40); // YHCT
                                columns.ConstantColumn(50); // TE<6
                                columns.ConstantColumn(50); // BHYT
                                columns.ConstantColumn(50); // Viện phí
                                columns.ConstantColumn(60); // Không thu
                                columns.ConstantColumn(50); // Cấp cứu
                                columns.ConstantColumn(50); // Vào viện
                                columns.ConstantColumn(60); // Chuyển viện
                                columns.ConstantColumn(60); // Ngoại trú - NB
                                columns.ConstantColumn(60); // Ngoại trú - YHCT
                                columns.ConstantColumn(60); // Ngoại trú - TE<6
                                columns.ConstantColumn(60); // Ngoại trú - số ngày
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Text("STT").Bold();
                                header.Cell().Text("Dịch vụ").Bold();
                                header.Cell().Text("Tổng số").Bold();
                                header.Cell().Text("YHCT").Bold();
                                header.Cell().Text("TE<6").Bold();
                                header.Cell().Text("BHYT").Bold();
                                header.Cell().Text("Viện phí").Bold();
                                header.Cell().Text("Không thu").Bold();
                                header.Cell().Text("Cấp cứu").Bold();
                                header.Cell().Text("Vào viện").Bold();
                                header.Cell().Text("Chuyển viện").Bold();
                                header.Cell().Text("Ngoại trú - NB").Bold();
                                header.Cell().Text("Ngoại trú - YHCT").Bold();
                                header.Cell().Text("Ngoại trú - TE<6").Bold();
                                header.Cell().Text("Ngoại trú - số ngày").Bold();
                            });

                            // Data
                            int stt = 1;
                            foreach (var item in request.Data)
                            {
                                table.Cell().Border(1).AlignMiddle().Text(stt++.ToString());
                                table.Cell().Border(1).Text(item.tenDichVu ?? "");
                                table.Cell().Border(1).Text(item.TongSoLanKham.ToString());
                                table.Cell().Border(1).Text(item.YHCT_SoLanKham.ToString());
                                table.Cell().Border(1).Text(item.TE6Tuoi_SLK.ToString());
                                table.Cell().Border(1).Text(item.VienPhi_SLK.ToString());
                                table.Cell().Border(1).Text(item.BHYT_SLK.ToString());
                                table.Cell().Border(1).Text(item.KhongThuDuoc_SLK.ToString());
                                table.Cell().Border(1).Text(item.CapCuu_SLK.ToString());
                                table.Cell().Border(1).Text(item.SoNguoiBenhVaoVien.ToString());
                                table.Cell().Border(1).Text(item.SoNguoiBenhChuyenVien.ToString());
                                table.Cell().Border(1).Text(item.SoNguoiBenh_DTNT.ToString());
                                table.Cell().Border(1).Text(item.YHCT_DTNT.ToString());
                                table.Cell().Border(1).Text(item.TE6Tuoi_DTNT.ToString());
                                table.Cell().Border(1).Text(item.SoNgay_DTNT.ToString());
                            }
                        });

                        // Footer chữ ký
                        page.Footer().Column(col =>
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text("THỦ TRƯỞNG ĐƠN VỊ\n(Ký, họ tên, đóng dấu)").AlignCenter().FontSize(10);
                                row.RelativeItem().Text("THỦ QUỸ\n(Ký, họ tên)").AlignCenter().FontSize(10);
                                row.RelativeItem().Text("KẾ TOÁN\n(Ký, họ tên)").AlignCenter().FontSize(10);
                                row.RelativeItem().Column(c2 =>
                                {
                                    c2.Item().Text($"Ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}")
                                        .AlignCenter().FontSize(10);
                                    c2.Item().Text("NGƯỜI LẬP BẢNG\n(Ký, họ tên)").AlignCenter().FontSize(10);
                                });
                            });
                        });
                    });
                });

                var pdfStream = new MemoryStream();
                document.GeneratePdf(pdfStream);
                pdfStream.Position = 0;

                var fileName = $"BaoCaoHoatDongKham_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                return File(pdfStream, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
