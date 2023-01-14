
$(document).ready(function () {

    completed = function (message) {
        alert(message.responseText)
        window.location.href = 'Home/Index';
    }

    $(function () {

        $('#searchForm').submit();
        var dateIn = $("#date-in").val().replace(' 0:00:00', "").split('.')

        dateIn = new Date(dateIn[2], dateIn[1] - 1, dateIn[0])
        var dateOut = $("#date-out").val().replace(' 0:00:00', "").split('.')
        dateOut = new Date(dateOut[2], dateOut[1] - 1, dateOut[0])

        $("#date-in").datepicker('setDate', new Date(dateIn))
        $("#date-out").datepicker('setDate', new Date(dateOut))
    });


    $('#btnSearch').on('click', function (e) {
        $('#room-list').empty();
        $("#booking-info").empty();
    });
    $('#btnRemoveFilter').on('click', function (e) {
        $("#roomCategoryId").val(0);
        var dateIn = new Date()
        $("#date-in").datepicker('setDate', dateIn)
        var dateOut = new Date(dateIn)
        dateOut.setDate(dateIn.getDate() + 1)
        $("#date-out").datepicker('setDate', dateOut)
        $('#room-list').empty();
        $("#booking-info").empty();
        $('#searchForm').submit();
    });

    $('#room-list').on('click', "#btnConfirm", function (e) {

        var adultCount = $('#guest option:selected').val()
        var childCount = $('#child option:selected').val()
        var checkIn = $("#date-in").datepicker('getDate')
        var checkOut = $("#date-out").datepicker('getDate')
        var roomCategoryId = $(this).parent().find('#roomCategoryId').val()

        var maxGuests = $(this).parent().find('#maxGuests').val()

        if (checkOut <= checkIn)
            alert("Дата выезда болжна быть больше даты заезда")
        else if (parseInt(adultCount) + parseInt(childCount) > parseInt(maxGuests))
            alert("Выбрано " + (parseInt(adultCount) + parseInt(childCount)) + " чел. Вместимость этого номера максимум " + maxGuests + " чел.")
        else
            Confirm(checkIn, checkOut, adultCount, childCount, roomCategoryId)



    })

    function GetRooms(guestCount, roomCategoryId) {
        $.ajax({
            url: '/Booking/RoomList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "html",
            data: {
                guestCount: guestCount,
                roomCategoryId: roomCategoryId
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
