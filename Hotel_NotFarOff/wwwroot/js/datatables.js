$(document).ready(function () {
    $('#roomsTable').DataTable({
        ajax: {
            url: "../Rooms/GetList",
            type: "POST",
        },
        processing: true,
        serverSide: true,
        filter: true,
        columns: [
            { data: "RoomNumber", name: "RoomNumber" },
            {
                data: "IsBooked", name: "IsBooked",
                "render": function (data) {
                    if (data == true) {
                       
                        return "Забронирован"
                    }
                    else return "Свободен"
                }
            },
            { data: "RoomCategory.Title", name: "RoomCategory.Title" },
            {
                "data": null,
                "orderable": false,
                "render": function () {
                    return `<a class='btn btn-primary btn-sm' href='#' style='margin-right:3px'><i class='fas fa-folder'></i>Детали</a>
                            <a class='btn btn-info btn-sm' href='#' style='margin-right:3px'><i class='fas fa-pencil-alt' style='margin-right:3px'></i>Редактировать</a>
                            <a class='btn btn-danger btn-sm' href='#' style='margin-right:3px'><i class='fas fa-trash'></i>Удалить</a>`;
                },
                "width": "20%"
            }
        ]
    })
})