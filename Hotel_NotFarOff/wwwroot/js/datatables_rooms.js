$(document).ready(function () {
    $(function () {
        $.ajax({
            url: '../RoomCategories/GetTitlesList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "json",
            contentType: 'application/json',
            success: function (response) {
                var select = $("#roomCategories")
                response.forEach(function (d) {
                    select.append('<option value="' + d.id + '">' + d.title + '</option>');
                })
            },
            error: function (request, ajaxOptions, exception) {
                alert(request);
                alert(exception);
            }
        })
    });

    var dataTable = $('#roomsTable').DataTable({
        ajax: {
            url: "../Rooms/GetList",
            type: "POST",
            data: function (data) {
                var roomCategory = $("#roomCategories").val()
                var roomStatus = $("#roomStatuses").val()

                data.filterRoomCategory = roomCategory
                data.filterRoomStatus = roomStatus
            }

        },
        processing: true,
        serverSide: true,
        filter: true,
        columns: [
            { data: "Id", name: "Id" },
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
                "render": function (data, type, full, meta) {
                    return `<a class='btn btn-primary btn-sm' href='/Admin/RoomDetails/` + full.Id + `' style='margin-right:3px'><i class='fas fa-folder'></i>Детали</a>
                            <a class='btn btn-info btn-sm' href='/Admin/RoomEdit/` + full.Id + `' style='margin-right:3px'><i class='fas fa-pencil-alt' style='margin-right:3px'></i>Редактировать</a>
                            <a class='btn btn-danger btn-sm' href='#' onclick=DeleteData('` + full.Id + `'); style='margin-right:3px'><i class='fas fa-trash'></i>Удалить</a>`;
                },
                "width": "22%"
            }
        ]
    })
    $('#roomCategories').change(function () {
        dataTable.draw();
    });
    $('#roomStatuses').change(function () {
        dataTable.draw();
    });


})

function DeleteData(roomId) {
    if (confirm("Вы уверены, что хотите удалить?")) {
        Delete(roomId);
    } else {
        return false;
    }
}


function Delete(roomId) {
    var url = "../Rooms/Delete/" + roomId;
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            oTable = $('#roomsTable').DataTable();
            oTable.draw();
            alert("Номер успешно удалён")
        },
        error: function (errormessage) {
            alert(errormessage.responseText)
        }
    });
}

