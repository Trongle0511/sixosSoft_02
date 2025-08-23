
    $(function () {
        // Khởi tạo datepicker
        $('#ngayTuNgay, #denNgay').datepicker({
            format: "dd-mm-yyyy",
            todayHighlight: true,
            autoclose: true,
            orientation: "bottom auto",
            language: "vi"
        });

    // Thêm danh sách năm (10 năm gần đây, giảm dần)
    let yearNow = new Date().getFullYear();
        for (let y = yearNow; y >= yearNow - 10; y--) {
        $("#yearSelect").append(`<option value="${y}">${y}</option>`);
        }
    $("#yearSelect").val(yearNow);

    // Xử lý hiển thị group khi chọn giai đoạn
    $("#selectGiaiDoan").on("change", function () {
        let val = $(this).val();
    $("#yearGroup, #quarterGroup, #monthGroup").addClass("d-none");

    if (val === "nam") {
        $("#yearGroup").removeClass("d-none");
    setYearRange($("#yearSelect").val());
            }
    else if (val === "quy") {
        $("#yearGroup, #quarterGroup").removeClass("d-none");
    // Lấy quý hiện tại
    let monthNow = new Date().getMonth() + 1;
    let currentQuarter = Math.ceil(monthNow / 3);
    $("#selectQuarter").val(currentQuarter);
    setQuarterRange($("#yearSelect").val(), currentQuarter);
            }
    else if (val === "thang") {
        $("#yearGroup, #monthGroup").removeClass("d-none");
    // Lấy tháng hiện tại
    let monthNow = new Date().getMonth() + 1;
    $("#selectMonth").val(monthNow);
    setMonthRange($("#yearSelect").val(), monthNow);
            }
    else if (val === "ngay") {
        // reset về hôm nay
        $('#ngayTuNgay').datepicker("update", new Date());
    $('#denNgay').datepicker("update", new Date());
            }
        }).trigger("change");

    // Khi thay đổi năm
    $("#yearSelect").on("change", function () {
        let mode = $("#selectGiaiDoan").val();
    if (mode === "nam") {
        setYearRange($(this).val());
            } else if (mode === "quy") {
        setQuarterRange($(this).val(), $("#selectQuarter").val());
            } else if (mode === "thang") {
        setMonthRange($(this).val(), $("#selectMonth").val());
            }
        });

    // Khi thay đổi quý
    $("#selectQuarter").on("change", function () {
        setQuarterRange($("#yearSelect").val(), $(this).val());
        });

    // Khi thay đổi tháng
    $("#selectMonth").on("change", function () {
        setMonthRange($("#yearSelect").val(), $(this).val());
        });

    // Hàm set range cho năm
    function setYearRange(year) {
        let start = new Date(year, 0, 1);   // 1-1-year
    let end = new Date(year, 11, 31);  // 31-12-year
    $('#ngayTuNgay').datepicker("update", start);
    $('#denNgay').datepicker("update", end);
        }

    // Hàm set range cho quý
    function setQuarterRange(year, quarter) {
        quarter = parseInt(quarter);
    let startMonth = (quarter - 1) * 3;
    let start = new Date(year, startMonth, 1);
    let end = new Date(year, startMonth + 3, 0); // ngày cuối quý
    $('#ngayTuNgay').datepicker("update", start);
    $('#denNgay').datepicker("update", end);
        }

    // Hàm set range cho tháng
    function setMonthRange(year, month) {
        month = parseInt(month) - 1;
    let start = new Date(year, month, 1);
    let end = new Date(year, month + 1, 0);
    $('#ngayTuNgay').datepicker("update", start);
    $('#denNgay').datepicker("update", end);
        }
    });
