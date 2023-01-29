$(document).ready(function () {
    $(function () {
        $.ajax({
            url: '../Booking/GetStatusesTitlesList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "json",
            contentType: 'application/json',
            success: function (response) {
                var select = $("#bookingStatuses")
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

    var dataTable = $('#bookingTable').DataTable({
        ajax: {
            url: "../Booking/GetList",
            type: "POST",
            data: function (data) {
                var bookingStatus = $("#bookingStatuses").val()

                data.filterBookingStatus = bookingStatus
            }
        },
        processing: true,
        serverSide: true,
        filter: true,
        columns: [
            { data: "Id", name: "Id" },
            { data: "TenantFio", name: "TenantFio" },
            { data: "PhoneNumber", name: "PhoneNumber" },
            {
                data: "CheckIn", name: "CheckIn", "render": function (data) {
                    return new Date(Date.parse(data)).toLocaleDateString("ru-RU")
                }
            },
            {
                data: "CheckOut", name: "CheckOut", "render": function (data) {
                    return new Date(Date.parse(data)).toLocaleDateString("ru-RU")
                }
            },
            { data: "BookingStatus.Title", name: "BookingStatus.Title" },
            { data: "Room.RoomNumber", name: "Room.RoomNumber" },
            {
                data: "CreatedDate", name: "CreatedDate", "render": function (data) {
                    var date = new Date(Date.parse(data))
                    return date.toLocaleString("ru")
                }
            },
            {
                "data": null,
                "orderable": false,
                "render": function (data, type, full, meta) {
                    return `<a class='btn btn-primary btn-sm' href='/Admin/BookingDetails/` + full.Id + `' style='margin-right:3px'><i class='fas fa-folder'></i>Детали</a>
                            <a class='btn btn-info btn-sm' href='/Admin/BookingEdit/` + full.Id + `' style='margin-right:3px'><i class='fas fa-pencil-alt' style='margin-right:3px'></i>Редактировать</a>`;
                },
                "width": "22%"
            }
        ]

    })
    $("#bookingStatuses").change(function () {
        dataTable.draw();
    });
})

