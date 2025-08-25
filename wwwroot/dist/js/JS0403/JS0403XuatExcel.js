$("#btnExportExcelGoiKham").on("click", function () {
    if (allData.length === 0) {
        alert("Không có dữ liệu để xuất!");
        return;
    }
    let tuNgay = $("#ngayTuNgay").val();
    let denNgay = $("#ngayDenNgay").val();

    let request = {
        tuNgay: tuNgay,
        denNgay: denNgay,
        TenChiNhanh: _tenChiNhanh,
        IdChiNhanh: _idcn,
        data: allData   // dữ liệu đã load từ API trước đó
    };
    $.ajax({
        type: "POST",
        url: "https://localhost:7129/BaoCao/ExportExcel",
        contentType: "application/json",
        data: JSON.stringify(request),   // dùng dữ liệu đã lọc
        xhrFields: { responseType: 'blob' },
        success: function (blob) {
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = "BaoCao.xlsx";
            link.click();
        },
        error: function () {
            alert("Xuất Excel thất bại!");
        }
    });
});
