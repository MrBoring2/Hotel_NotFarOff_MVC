$(document).ready(function () {

    $('#roomCategoriesTable').DataTable({
        ajax: {
            url: "../RoomCategories/ForAdminList",
            type: "POST",

        },
        processing: true,
        serverSide: true,
        filter: true,
        columns: [
            { data: "Id", name: "Id" },
            { data: "Title", name: "Title" },
            { data: "PricePerDay", name: "PricePerDay" },
            { data: "RoomCount", name: "RoomCount" },
            { data: "NumbeOfSeats", name: "NumbeOfSeats" },
            { data: "RoomSize", name: "RoomSize" },
            {
                data: "MainImage", name: "MainImage",
                "render": function (d) {
                    return '<img src="data:image/png;base64,' + d + '"  style="width:100%;"/>'
                },
                "width": "20%"
            },
            {
                "data": null,
                "orderable": false,
                "render": function (data, type, full, meta) {
                    return `<a class='btn btn-primary btn-sm' href='/Admin/RoomCategoryDetails/` + full.Id + `' style='margin-right:3px'><i class='fas fa-folder'></i>Детали</a>
                            <a class='btn btn-info btn-sm' href='/Admin/RoomCategoryEdit/` + full.Id + `' style='margin-right:3px'><i class='fas fa-pencil-alt' style='margin-right:3px'></i>Редактировать</a>
                            <a class='btn btn-danger btn-sm' href='#' onclick=DeleteData('` + full.Id + `'); style='margin-right:3px'><i class='fas fa-trash'></i>Удалить</a>`;
                },
                "width": "22%"
            }
        ]
    })
   
})

function DeleteData(roomId) {
    if (confirm("Вы уверены, что хотите удалить?")) {
        Delete(roomId);
    } else {
        return false;
    }
}


function Delete(roomId) {
    var url = "../RoomCategories/Delete/" + roomId;
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            oTable = $('#roomCategoriesTable').DataTable();
            oTable.draw();
            alert("Категория номера успешно удалёна")
        },
        error: function (errormessage) {
            alert(errormessage.responseText)
        }
    });
}
