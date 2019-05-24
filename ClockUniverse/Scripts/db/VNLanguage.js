$(document).ready(function () {
    $('#mytable').DataTable({
        "language":
        {
            "decimal": "",
            "emptyTable": "Không có dữ liệu trùng khớp",
            "info": "Hiển thị từ _START_ đến _END_ của _TOTAL_ dữ liệu",
            "infoEmpty": "Hiển thị 0 đến 0 của 0 dữ liệu",
            "infoFiltered": "(Lọc từ _MAX_ tổng dữ liệu)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Hiển thị _MENU_ Dữ liệu",
            "loadingRecords": "Đang tải...",
            "processing": "Đang vận hành...",
            "search": "Tìm kiếm:",
            "zeroRecords": "Không dữ liệu nào được tìm thấy",
            "paginate": {
                "first": "Trang đầu",
                "last": "Trang cuối",
                "next": "Trang kế tiếp",
                "previous": "Trang trước"
            },
            "aria": {
                "sortAscending": ": Sắp xếp tăng dần",
                "sortDescending": ": Sắp xếp giảm dần"
            }
        }

    });
});

