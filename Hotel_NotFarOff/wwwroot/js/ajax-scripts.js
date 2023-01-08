$(function () {
    $("#get_student_list").click(function () {
        $.ajax({
            type: "GET",
            url: "/Rooms/ChagnePage",
            data: { "roomCategoryId": $("$roomId").val(), "page":  }
            success: function (response) {
                $('#studentList').empty();
                $('#studentList').html(response);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
});