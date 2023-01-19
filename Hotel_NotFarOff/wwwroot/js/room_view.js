$(document).ready(function () {


    $(function () {
        $.ajax({
            url: '../../RoomCategories/GetTitlesList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "json",
            contentType: 'application/json',
            success: function (response) {
                var select = $("#roomCategories")
                response.forEach(function (d) {
                    select.append('<option asp-for="RoomCategoryId" value="' + d.id + '">' + d.title + '</option>');
                })
                roomCategory = $("#roomCategory").val()
                select.val(roomCategory)
            },
            error: function (request, ajaxOptions, exception) {
                alert(request);
                alert(exception);
            }
        })
    });
    completed = function (message, statusCode) {
        alert(message.responseText)
        if (statusCode == "success") {
            window.location.href = '/Admin/RoomsList';
        }
    }


})