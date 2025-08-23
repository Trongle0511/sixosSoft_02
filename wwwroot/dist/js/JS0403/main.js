// Gọi API khi click button
$("#btnFilter").on("click", function () {
    let tuNgay = $("#ngayTuNgay").val();
    let denNgay = $("#denNgay").val();

    // Request body
    let request = {
        idChiNhanh: _idcn,
        tuNgay: tuNgay,
        denNgay: denNgay
    };

    // Hiện loading spinner
    $("#loadingSpinner").show();

    $.ajax({
        url: "https://localhost:7129/api/hoat_dong_kham",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (response) {
            if (response.success) {
                let data = response.data;
                renderTable(data);
                console.log("Response data:", data);
            } else {
                console.error(response.message || "Lỗi khi tải dữ liệu");
            }
            console.log("Request body:", request);
            console.log("Response:", response);
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);

        },
        complete: function () {
            // Ẩn loading spinner
            $("#loadingSpinner").hide();
        }
    });
});
function renderTable(data) {
    let tbody = $("table tbody");
    tbody.empty(); // Xóa dữ liệu cũ

    if (data.length === 0) {
        tbody.append(`<tr><td colspan="14" class="text-center">Không có dữ liệu</td></tr>`);
        return;
    }

    data.forEach((item, index) => {
        tbody.append(`
            <tr>
                <td>${index + 1}</td>
                <td class="dichvu">${item.tenDichVu || ""}</td>
                <td>${item.tongSoLanKham || 0}</td>
                <td>${item.yhcT_SoLanKham || 0}</td>
                <td>${item.tE6Tuoi_SLK || 0}</td>
                <td>${item.bhyT_SLK || 0}</td>
                <td>${item.vienPhi_SLK || 0}</td>
                <td>${item.khongThuDuoc_SLK || 0}</td>
                <td>${item.capCuu_SLK || 0}</td>
                <td>${item.soNguoiBenhVaoVien || 0}</td>
                <td>${item.soNguoiBenhChuyenVien || 0}</td>
                <td>${item.soNguoiBenh_DTNT || 0}</td>
                <td>${item.yhcT_DTNT || 0}</td>
                <td>${item.tE6Tuoi_DTNT || 0}</td>
                <td>${item.soNgay_DTNT || 0}</td>
            </tr>
        `);
    });
}