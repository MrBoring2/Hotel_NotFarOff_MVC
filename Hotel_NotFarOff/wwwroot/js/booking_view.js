$(document).ready(function () {


    $(function () {
        $.ajax({
            url: '../../Booking/GetStatusesTitlesList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "json",
            contentType: 'application/json',
            success: function (response) {
                var select = $("#bookingStatuses")
                response.forEach(function (d) {
                    select.append('<option asp-for="BookingStatusId" value="' + d.id + '">' + d.title + '</option>');
                })
                bookingStatus = $("#bookingStatus").val()
                select.val(bookingStatus)
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
            window.location.href = '/Admin/BookingList';
        }
    }


})