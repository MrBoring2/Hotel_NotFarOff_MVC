$(document).ready(function () {
    $(function () {
        GetStudents();
    });

    $('#btnSearch').on('click', function (e) {
        var adultCount = $('#guest option:selected').val()
        var childCount = $('#child option:selected').val()
        GetStudents(parseInt(adultCount) + parseInt(childCount));
    });

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
                $("#room-list").html(result);
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


