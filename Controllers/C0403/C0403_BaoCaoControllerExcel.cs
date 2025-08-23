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
        //if (data == null || data.Count == 0)
        //{
        //    return BadRequest("Không có dữ liệu để export");
        //}
        if (request.Data == null || request.Data.Count == 0)
        {
            return BadRequest("Không có dữ liệu để export");
        }

        // Parse chuỗi thành DateTime
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("HoatDongKham");

            int row = 1;

            // ====== HEADER ======
            ws.Cell(row, 1).Value = "SỞ Y TẾ TP. HỒ CHÍ MINH";
            ws.Range(row, 1, row, 13).Merge().Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            row++;

            ws.Cell(row, 1).Value = "BỆNH VIỆN UNG BƯỚU TP.HCM - " + request.TenChiNhanh;
            ws.Range(row, 1, row, 13).Merge().Style
                .Font.SetBold()
                .Font.SetFontSize(14)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            row += 2;

            ws.Cell(row, 1).Value = "HOẠT ĐỘNG KHÁM BỆNH";
            ws.Range(row, 1, row, 13).Merge().Style
                .Font.SetBold()
                .Font.SetFontSize(14)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            row++;

            ws.Cell(row, 1).Value = $"Từ ngày: {request.TuNgay:dd/MM/yyyy} đến ngày {request.DenNgay:dd/MM/yyyy}";
            ws.Range(row, 1, row, 13).Merge()
                .Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            row += 2;

            // ====== TABLE HEADER ======
            ws.Cell(row, 1).Value = "STT";
            ws.Cell(row, 2).Value = "Dịch vụ";
            ws.Cell(row, 3).Value = "Tổng số";
            ws.Cell(row, 4).Value = "YHCT";
            ws.Cell(row, 5).Value = "TE<6t";
            ws.Cell(row, 6).Value = "Viện phí";
            ws.Cell(row, 7).Value = "Bảo hiểm y tế";
            ws.Cell(row, 8).Value = "Không thu được";
            ws.Cell(row, 9).Value = "Cấp cứu";
            ws.Cell(row, 10).Value = "Số người bệnh vào viện";
            ws.Cell(row, 11).Value = "Số người bệnh chuyển viện";
            ws.Cell(row, 12).Value = "Số người bệnh ĐTNT";
            ws.Cell(row, 13).Value = "Số ngày ĐTNT";

            ws.Range(row, 1, row, 13).Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.LightGray)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin);

            row++;

            // ====== DATA ======
            int stt = 1;
            foreach (var item in request.Data)
            {
                ws.Cell(row, 1).Value = stt++;
                ws.Cell(row, 2).Value = item.tenDichVu;
                ws.Cell(row, 3).Value = item.TongSoLanKham;
                ws.Cell(row, 4).Value = item.YHCT_SoLanKham;
                ws.Cell(row, 5).Value = item.TE6Tuoi_SLK;
                ws.Cell(row, 6).Value = item.VienPhi_SLK;
                ws.Cell(row, 7).Value = item.BHYT_SLK;
                ws.Cell(row, 8).Value = item.KhongThuDuoc_SLK;
                ws.Cell(row, 9).Value = item.CapCuu_SLK;
                ws.Cell(row, 10).Value = item.SoNguoiBenhVaoVien;
                ws.Cell(row, 11).Value = item.SoNguoiBenhChuyenVien;
                ws.Cell(row, 12).Value = item.SoNguoiBenh_DTNT;
                ws.Cell(row, 12).Value = item.YHCT_DTNT;
                ws.Cell(row, 13).Value = item.TE6Tuoi_DTNT;
                ws.Cell(row, 13).Value = item.SoNgay_DTNT;

                // kẻ border cho từng dòng
                ws.Range(row, 1, row, 13).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Border.SetInsideBorder(XLBorderStyleValues.Thin);

                row++;
            }

            // ====== TOTAL ======
            ws.Cell(row, 1).Value = "TỔNG SỐ:";
            ws.Range(row, 1, row, 2).Merge().Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            for (int col = 3; col <= 13; col++)
            {
                ws.Cell(row, col).FormulaA1 = $"SUM({ws.Cell(6, col).Address}:{ws.Cell(row - 1, col).Address})";
                ws.Cell(row, col).Style.Font.SetBold();
            }

            // 👉 Thêm border cho cả dòng tổng
            ws.Range(row, 1, row, 13).Style
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin);

            // ====== SIGNATURES ======
            row += 3; // cách ra vài dòng sau bảng

            // Cột 1: Người lập biểu
            ws.Cell(row, 2).Value = "NGƯỜI LẬP BIỂU";
            ws.Cell(row, 2).Style.Font.SetBold();
            ws.Cell(row, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Cột 6: Trưởng phòng KH tổng hợp
            ws.Range(row, 6, row, 8).Merge().Value = "TRƯỞNG PHÒNG KẾ HOẠCH TỔNG HỢP";
            ws.Range(row, 6, row, 8).Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Cột 11: Ngày … tháng … năm …
            ws.Cell(row, 11).Value = $"Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
            ws.Cell(row, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                           .Font.SetItalic();

            // Dòng tiếp theo: Giám đốc
            row++;
            ws.Cell(row, 11).Value = "GIÁM ĐỐC";
            ws.Cell(row, 11).Style.Font.SetBold();
            ws.Cell(row, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

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
