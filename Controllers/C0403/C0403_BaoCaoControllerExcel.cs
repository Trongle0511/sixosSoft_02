using Azure.Core;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SixOs_Soft_demo_01.Models.M0403.DTO0403;
using System.Data;

public class BaoCaoController : Controller
{
    [HttpPost]
    public IActionResult ExportExcel([FromBody] ExportRequest request)
    {
        if (request.Data == null || request.Data.Count == 0)
        {
            return BadRequest("Không có dữ liệu để export");
        }

        // Parse chuỗi thành DateTime
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("HoatDongKham");

            int row = 1;

            // ====== CỘT TRÁI (LOGO + thông tin bệnh viện) ======
            ws.Column(1).Width = 50;

            // Chèn logo
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dist", "img", "logo.png");
            if (System.IO.File.Exists(logoPath))
            {
                var image = ws.AddPicture(logoPath)
                              .MoveTo(ws.Cell(1, 1)) // A1
                              .Scale(0.20);          // tùy chỉnh tỉ lệ
            }

            // Dòng 1
            ws.Range(row, 2, row, 5).Merge()
                .SetValue("BỆNH VIỆN UNG BƯỚU " + request.TenChiNhanh);
            ws.Range(row, 2, row, 3).Style
                .Font.SetBold()
                .Font.SetFontSize(12)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            row++;

            // Dòng 2
            ws.Range(row, 2, row, 5).Merge()
                .SetValue("Đ/C: Số 12 Đường 400, Phường Tăng Nhơn Phú, TP. Hồ Chí Minh");
            ws.Range(row, 2, row, 5).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            row++;

            // Dòng 3
            ws.Range(row, 2, row, 5).Merge()
                .SetValue("ĐT: (028) 36227722 – (028) 38433021 | Email: bvubhcm@gmail.com");
            ws.Range(row, 2, row, 5).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


            // ====== CỘT PHẢI (Tiêu đề báo cáo) ======
            int rightStartCol = 12;   // bắt đầu từ cột 5
            int rightEndCol = 16;  // đến cột cuối cùng

            row = 1;
            // Dòng 1: Tiêu đề lớn (giữ căn giữa)
            ws.Range(row, rightStartCol, row, rightEndCol).Merge()
                .SetValue("BÁO CÁO TỔNG HỢP KHU VỰC PHÒNG KHÁM CHUYÊN GIA");
            ws.Range(row, rightStartCol, row, rightEndCol).Style
                .Font.SetBold().Font.SetFontSize(13)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row++;
            // Dòng 2: Đơn vị thống kê (căn phải)
            ws.Range(row, rightStartCol, row, rightEndCol).Merge()
                .SetValue("Đơn vị thống kê");
            ws.Range(row, rightStartCol, row, rightEndCol).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row++;
            // Dòng 3: Khoảng thời gian (căn phải)
            ws.Range(row, rightStartCol, row, rightEndCol).Merge()
                .SetValue($"Từ ngày: {request.TuNgay:dd-MM-yyyy} đến ngày: {request.DenNgay:dd-MM-yyyy}");
            ws.Range(row, rightStartCol, row, rightEndCol).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


            // 👉 Cách 2 dòng trống giữa tiêu đề và bảng
            row += 2;

            // ====== TABLE HEADER ======
            // ====== TABLE HEADER ======
            int headerRow1 = row;
            int headerRow2 = row + 1;
            int headerRow3 = row + 2;

            // Hàng 1
            ws.Cell(headerRow1, 2).Value = "STT";
            ws.Range(headerRow1, 2, headerRow3, 2).Merge(); // rowspan=3

            ws.Cell(headerRow1, 3).Value = "Dịch vụ";
            ws.Range(headerRow1, 3, headerRow3, 3).Merge(); // rowspan=3

            ws.Cell(headerRow1, 4).Value = "Số lần khám";
            ws.Range(headerRow1, 4, headerRow1, 10).Merge(); // colspan=7

            ws.Column(11).Width = 30;
            ws.Cell(headerRow1, 11).Value = "Số người bệnh vào viện";
            ws.Range(headerRow1, 11, headerRow3, 11).Merge(); // rowspan=3

            ws.Column(12).Width = 30;
            ws.Cell(headerRow1, 12).Value = "Số người bệnh chuyển viện";
            ws.Range(headerRow1, 12, headerRow3, 12).Merge(); // rowspan=3

            ws.Cell(headerRow1, 13).Value = "Điều trị ngoại trú";
            ws.Range(headerRow1, 13, headerRow1, 16).Merge(); // colspan=4

            // Hàng 2
            ws.Cell(headerRow2, 4).Value = "Tổng số";
            ws.Range(headerRow2, 4, headerRow3, 4).Merge(); // rowspan=2

            ws.Cell(headerRow2, 5).Value = "Trong đó";
            ws.Range(headerRow2, 5, headerRow2, 10).Merge(); // colspan=6

            ws.Cell(headerRow2, 13).Value = "Số người bệnh";
            ws.Range(headerRow2, 13, headerRow3, 13).Merge(); // rowspan=2

            ws.Cell(headerRow2, 14).Value = "YHCT";
            ws.Range(headerRow2, 14, headerRow3, 14).Merge(); // rowspan=2

            ws.Cell(headerRow2, 15).Value = "TE<6tuổi";
            ws.Range(headerRow2, 15, headerRow3, 15).Merge(); // rowspan=2

            ws.Cell(headerRow2, 16).Value = "Số ngày";
            ws.Range(headerRow2, 16, headerRow3, 16).Merge(); // rowspan=2

            // Hàng 3 (các cột con của "Trong đó")
            ws.Cell(headerRow3, 5).Value = "YHCT";
            ws.Cell(headerRow3, 6).Value = "TE<6tuổi";
            ws.Cell(headerRow3, 7).Value = "BHYT";
            ws.Cell(headerRow3, 8).Value = "Viện phí";
            ws.Cell(headerRow3, 9).Value = "Không thu được";
            ws.Cell(headerRow3, 10).Value = "Cấp cứu";

            // ===== Style chung cho header =====
            ws.Range(headerRow1, 2, headerRow3, 16).Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetWrapText(true) // 👈 Thêm dòng này
                .Fill.SetBackgroundColor(XLColor.LightGray)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin);

            // Sau khi tạo header thì row nhảy xuống dưới
            row = headerRow3 + 1;

            // ====== DATA ======
            int stt = 1;
            int dataStartRow = row;

            foreach (var item in request.Data)
            {
                ws.Cell(row, 2).Value = stt++;
                ws.Cell(row, 3).Value = item.tenDichVu;
                ws.Cell(row, 4).Value = item.TongSoLanKham;
                ws.Cell(row, 5).Value = item.YHCT_SoLanKham;
                ws.Cell(row, 6).Value = item.TE6Tuoi_SLK;
                ws.Cell(row, 7).Value = item.VienPhi_SLK;
                ws.Cell(row, 8).Value = item.BHYT_SLK;
                ws.Cell(row, 9).Value = item.KhongThuDuoc_SLK;
                ws.Cell(row, 10).Value = item.CapCuu_SLK;
                ws.Cell(row, 11).Value = item.SoNguoiBenhVaoVien;
                ws.Cell(row, 12).Value = item.SoNguoiBenhChuyenVien;
                ws.Cell(row, 13).Value = item.SoNguoiBenh_DTNT;
                ws.Cell(row, 14).Value = item.YHCT_DTNT;
                ws.Cell(row, 15).Value = item.TE6Tuoi_DTNT;
                ws.Cell(row, 16).Value = item.SoNgay_DTNT;

                // border cho từng dòng data
                ws.Range(row, 2, row, 16).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                     .Border.SetInsideBorder(XLBorderStyleValues.Thin);
                row++;
            }

            // ====== TOTAL ======
            ws.Cell(row, 2).Value = "TỔNG SỐ:";
            ws.Range(row, 2, row, 3).Merge().Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // 👉 SUM từ cột 4 → 16
            for (int col = 4; col <= 16; col++)
            {
                ws.Cell(row, col).FormulaA1 = $"SUM({ws.Cell(dataStartRow, col).Address}:{ws.Cell(row - 1, col).Address})";
                ws.Cell(row, col).Style.Font.SetBold();
            }

            // border dòng tổng
            ws.Range(row, 2, row, 16).Style
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin);

            // ====== SIGNATURES ======
            row += 3; // cách ra vài dòng sau bảng

            // Cột 1: Người lập biểu
            ws.Cell(row, 5).Value = "NGƯỜI LẬP BIỂU";
            ws.Cell(row, 5).Style.Font.SetBold();
            ws.Cell(row, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Cột 6: Trưởng phòng KH tổng hợp
            ws.Range(row, 9, row, 11).Merge().Value = "TRƯỞNG PHÒNG KẾ HOẠCH TỔNG HỢP";
            ws.Range(row, 9, row, 11).Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Cột 14: Giám đốc (lên trên trước)
            ws.Cell(row, 14).Value = "GIÁM ĐỐC";
            ws.Cell(row, 14).Style.Font.SetBold();
            ws.Cell(row, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Dòng tiếp theo: Ngày … tháng … năm …
            row++;
            ws.Cell(row, 14).Value = $"Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
            ws.Cell(row, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                           .Font.SetItalic();

            // AutoFit
            ws.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "BaoCao.xlsx");
            }
        }
    }

}
