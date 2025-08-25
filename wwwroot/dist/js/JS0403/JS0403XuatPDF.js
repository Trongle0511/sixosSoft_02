$("#btnExportPDFGoiKham").on("click", function () {
    var request = {
        TuNgay: $("#ngayTuNgay").val(),
        DenNgay: $("#ngayDenNgay").val(),
        TenChiNhanh: _tenChiNhanh,
        Data: allData // allData là danh sách báo cáo
    };

    $.ajax({
        url: "/api/xuatbcpdf/exportpdf",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(request),
        xhrFields: {
            responseType: 'blob' // nhận PDF dạng blob
        },
        success: function (data) {
            const blob = new Blob([data], { type: 'application/pdf' });
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'BaoCaoHoatDongKham.pdf';
            link.click();
        },
        error: function (xhr) {
            alert("Có lỗi xảy ra: " + xhr.responseJSON?.message);
        }
    });
});
