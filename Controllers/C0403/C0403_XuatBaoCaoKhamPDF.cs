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
                        page.DefaultTextStyle(x => x.FontFamily(Fonts.TimesNewRoman));

                        // Header
                        page.Header().Column(col =>
                        {
                            col.Item().Row(row =>
                            {
                                row.ConstantItem(60).AlignMiddle().PaddingRight(10).Image("wwwroot/dist/img/logo.png");

                                row.RelativeItem().Column(col1 =>
                                {
                                    col1.Item().Text("BỆNH VIỆN UNG BƯỚU " + request.TenChiNhanh)
                                            .FontSize(14).Bold();
                                    col1.Item().Text("Đ/C: Số 12 Đường 400, Phường Tăng Nhơn Phú, TP. Hồ Chí Minh").FontSize(10);
                                    col1.Item().Text("Điện thoại: (028) 36227722 – (028) 38433021").FontSize(10);
                                    col1.Item().Text("Email: bvubhcm@gmail.com").FontSize(10);
                                });
                            });

                            col.Item().PaddingTop(10).AlignCenter()
                               .Text("BÁO CÁO HOẠT ĐỘNG KHÁM").Bold().FontSize(14);

                            col.Item().AlignCenter().Text("Đơn vị thống kê")
                                .Italic().FontSize(10);
                            col.Item().PaddingBottom(10).AlignRight().Text($"Từ ngày: {tuNgay:dd/MM/yyyy} Đến ngày: {denNgay:dd/MM/yyyy}")
                               .FontSize(10);

                        });

                        // Nội dung bảng
                        page.Content().Column(col =>
                        {
                            // Bảng dữ liệu
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(30); // STT
                                    columns.RelativeColumn(2);  // Dịch vụ
                                    for (int i = 0; i < 13; i++) columns.RelativeColumn(1); // Các cột số liệu
                                });
                                // ===== HEADER BẢNG =====
                                table.Header(header =>
                                {
                                    // --- DÒNG 1 ---
                                    header.Cell().RowSpan(3).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("STT").Bold().FontSize(9);
                                    header.Cell().RowSpan(3).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Dịch vụ").Bold().FontSize(9);
                                    header.Cell().ColumnSpan(7).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Số lần khám").Bold().FontSize(9);
                                    header.Cell().RowSpan(3).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Số người bệnh vào viện").Bold().FontSize(9);
                                    header.Cell().RowSpan(3).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Số người bệnh chuyển viện").Bold().FontSize(9);
                                    header.Cell().ColumnSpan(4).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Điều trị ngoại trú").Bold().FontSize(9);

                                    // --- DÒNG 2 ---
                                    header.Cell().RowSpan(2).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Tổng số").Bold().FontSize(9);
                                    header.Cell().ColumnSpan(6).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Trong đó").Bold().FontSize(9);
                                    header.Cell().RowSpan(2).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Số người bệnh").Bold().FontSize(9);
                                    header.Cell().RowSpan(2).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("YHCT").Bold().FontSize(9);
                                    header.Cell().RowSpan(2).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("TE<6 tuổi").Bold().FontSize(9);
                                    header.Cell().RowSpan(2).Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Số ngày").Bold().FontSize(9);

                                    // --- DÒNG 3 ---
                                    header.Cell().Border(1).AlignCenter().AlignMiddle().Padding(2).Text("YHCT").Bold().FontSize(9);
                                    header.Cell().Border(1).AlignCenter().AlignMiddle().Padding(2).Text("TE<6 tuổi").Bold().FontSize(9);
                                    header.Cell().Border(1).AlignCenter().AlignMiddle().Padding(2).Text("BHYT").Bold().FontSize(9);
                                    header.Cell().Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Viện phí").Bold().FontSize(9);
                                    header.Cell().Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Không thu được").Bold().FontSize(9);
                                    header.Cell().Border(1).AlignCenter().AlignMiddle().Padding(2).Text("Cấp cứu").Bold().FontSize(9);
                                });




                                // Data
                                int stt = 1;
                                foreach (var item in request.Data)
                                {
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(stt++.ToString());
                                    table.Cell().Border(1).AlignLeft().Padding(2).Text(item.tenDichVu ?? "");
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.TongSoLanKham.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.YHCT_SoLanKham.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.TE6Tuoi_SLK.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.BHYT_SLK.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.VienPhi_SLK.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.KhongThuDuoc_SLK.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.CapCuu_SLK.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.SoNguoiBenhVaoVien.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.SoNguoiBenhChuyenVien.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.SoNguoiBenh_DTNT.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.YHCT_DTNT.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.TE6Tuoi_DTNT.ToString());
                                    table.Cell().Border(1).AlignCenter().Padding(2).Text(item.SoNgay_DTNT.ToString());
                                }
                            });

                            // Chữ ký
                            col.Item().EnsureSpace(150).PaddingTop(10).Row(row =>
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
                        // --- ĐÁNH SỐ TRANG ---
                        page.Footer()
                            .AlignRight() // Căn góc phải, có thể đổi AlignCenter hoặc AlignLeft
                            .Text(text =>
                            {
                                text.Span("Trang ").FontSize(9);
                                text.CurrentPageNumber().FontSize(9).Bold();
                                text.Span(" / ").FontSize(9);
                                text.TotalPages().FontSize(9);
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
