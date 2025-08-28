// ==================== BIẾN GLOBAL ====================
let allData = [];
let currentPage = 1;
let rowsPerPage = 10;

// ==================== LƯU VÀ LOAD PAGE SIZE ====================
$(document).ready(function () {
    // Lấy rowsPerPage từ localStorage khi load trang
    let savedPageSize = localStorage.getItem("rowsPerPage");
    if (savedPageSize) {
        rowsPerPage = parseInt(savedPageSize);
        $("#pageSizeSelect").val(rowsPerPage); // set lại select
    }

    // Render bảng lần đầu (nếu có dữ liệu cũ)
    if (allData.length > 0) {
        renderTable(allData);
        renderPagination();
    }

    // Khi thay đổi rowsPerPage thì lưu vào localStorage
    $("#pageSizeSelect").on("change", function () {
        rowsPerPage = parseInt($(this).val());
        localStorage.setItem("rowsPerPage", rowsPerPage); // lưu lại
        currentPage = 1;
        renderTable();
        renderPagination();
    });
});

// ==================== GỌI API ====================
$("#btnFilter").on("click", function () {
    let tuNgay = $("#ngayTuNgay").val();
    let denNgay = $("#ngayDenNgay").val();

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
                allData = response.data || [];
                currentPage = 1;
                renderTable(allData);
                renderPagination();
            } else {
                console.error(response.message || "Lỗi khi tải dữ liệu");
            }
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

// ==================== RENDER TABLE ====================
function renderTable() {
    let tbody = $("table tbody");
    tbody.empty();

    if (allData.length === 0) {
        tbody.append(`<tr><td colspan="15" class="text-center">Không có dữ liệu</td></tr>`);
        $("#pageInfo").text("");
        return;
    }

    let start = (currentPage - 1) * rowsPerPage;
    let end = start + rowsPerPage;
    let pageData = allData.slice(start, end);

    pageData.forEach((item, index) => {
        tbody.append(`
            <tr>
                <td>${start + index + 1}</td>
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

    // Cập nhật thông tin trang
    let totalPages = Math.ceil(allData.length / rowsPerPage);
    $("#pageInfo").text(`Trang ${currentPage}/${totalPages} - Tổng ${allData.length} bản ghi`);
}

// ==================== RENDER PHÂN TRANG ====================
function renderPagination() {
    let totalPages = Math.ceil(allData.length / rowsPerPage);
    let pagination = $("#pagination");
    pagination.empty();

    if (totalPages <= 1) return;

    // Nút Trước
    pagination.append(`
        <li class="page-item ${currentPage === 1 ? "disabled" : ""}">
            <a class="page-link" href="#" data-page="${currentPage - 1}">&laquo;</a>
        </li>
    `);

    // Hiển thị số trang
    for (let i = 1; i <= totalPages; i++) {
        pagination.append(`
            <li class="page-item ${i === currentPage ? "active" : ""}">
                <a class="page-link" href="#" data-page="${i}">${i}</a>
            </li>
        `);
    }

    // Nút Sau
    pagination.append(`
        <li class="page-item ${currentPage === totalPages ? "disabled" : ""}">
            <a class="page-link" href="#" data-page="${currentPage + 1}">&raquo;</a>
        </li>
    `);
}

// ==================== BẮT SỰ KIỆN CLICK PHÂN TRANG ====================
$(document).on("click", "#pagination .page-link", function (e) {
    e.preventDefault();
    let page = parseInt($(this).data("page"));
    let totalPages = Math.ceil(allData.length / rowsPerPage);

    if (page >= 1 && page <= totalPages) {
        currentPage = page;
        renderTable();
        renderPagination();
    }
});