//const { data } = require("jquery");

$(document).ready(function () {
    $(function () {
        GetStudents();
    });

    $('#btnSearch').on('click', function (e) {
        var adultCount = $('#guest option:selected').val()
        var childCount = $('#child option:selected').val()
        GetStudents(parseInt(adultCount) + parseInt(childCount));
    });

    $('#room-list').on('click', "#btnConfirm", function (e) {

        var adultCount = $('#guest option:selected').val()
        var childCount = $('#child option:selected').val()
        var checkIn = $("#date-in").datepicker('getDate')
        var checkOut = $("#date-out").datepicker('getDate')
        var roomCategoryId = $(this).parent().find('#roomCategoryId').val()
        Confirm(checkIn, checkOut, adultCount, childCount, roomCategoryId)

    })

    function GetStudents(guestCount) {
        $.ajax({
            url: '/Booking/RoomList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "html",
            data: {
                guestCount: guestCount
            }
        })
            .done(function (result) {
                $("#booking-confirm").empty();
                $("#room-list").html(result);
            }).fail(function (xhr) {
                console.log('error : ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            });

    }

    function Confirm(checkIn, checkOut, adultCount, childCount, roomCategoryId) {
        data = {
            CheckIn: checkIn,
            CheckOut: checkOut,
            AdultCount: adultCount,
            ChildCount: childCount,
            RoomCategoryId: roomCategoryId
        }

        $.ajax({
            url: '/Booking/Confirm',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            cache: false,
            async: true,
            dataType: "html",
            data: JSON.stringify(data)
        })
            .done(function (result) {
                $('#room-list').empty();
                $("#booking-confirm").html(result);
            }).fail(function (xhr) {
                console.log('error : ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            });
    }

    var mh = 0;
    $(".mini-room-detail").each(function () {
        var h_block = parseInt($(this).height());
        if (h_block > mh) {
            mh = h_block;
        };
    });
    $(".mini-room-detail").height(mh);
});


