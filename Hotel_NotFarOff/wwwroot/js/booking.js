$(document).ready(function () {
    $(function () {

        var dateIn = $("#date-in").val().replace(' 0:00:00', "").split('.')
       
        dateIn = new Date(dateIn[2], dateIn[1] - 1, dateIn[0])
        var dateOut = $("#date-out").val().replace(' 0:00:00', "").split('.')
        dateOut = new Date(dateOut[2], dateOut[1] - 1, dateOut[0])

        $("#date-in").datepicker('setDate', new Date(dateIn))
        $("#date-out").datepicker('setDate', new Date(dateOut))

        $('#booking-info').hide()
        var adultCount = $('#guest option:selected').val()
        var childCount = $('#child option:selected').val()
        GetRooms(parseInt(adultCount) + parseInt(childCount))
    });


    $('#btnSearch').on('click', function (e) {
        var adultCount = $('#guest option:selected').val()
        var childCount = $('#child option:selected').val()
        GetRooms(parseInt(adultCount) + parseInt(childCount));
    });

    $('#room-list').on('click', "#btnConfirm", function (e) {

        var adultCount = $('#guest option:selected').val()
        var childCount = $('#child option:selected').val()
        var checkIn = $("#date-in").datepicker('getDate')
        var checkOut = $("#date-out").datepicker('getDate')
        var roomCategoryId = $(this).parent().find('#roomCategoryId').val()
        Confirm(checkIn, checkOut, adultCount, childCount, roomCategoryId)

    })

    function GetRooms(guestCount) {
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
                $("#booking-info").empty();
                $('#booking-info').hide()
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
                $('#booking-info').show()
                $("#booking-info").html(result);
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


