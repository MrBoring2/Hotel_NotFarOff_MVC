$(document).ready(function () {


    $(function () {
        $.ajax({
            url: '../../Employees/GetPostsTitlesList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "json",
            contentType: 'application/json',
            success: function (response) {
                var select = $("#posts")
                response.forEach(function (d) {
                    select.append('<option asp-for="PostId" value="' + d.id + '">' + d.title + '</option>');
                })
                post = $("#post").val()
                select.val(post)
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
            window.location.href = '/Admin/EmployeesList';
        }
    }

   
})