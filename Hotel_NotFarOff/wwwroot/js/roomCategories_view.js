$(document).ready(function () {
    


    //$(function () {
    //    $.ajax({
    //        url: '../../RoomCategories/GetTitlesList',
    //        type: 'GET',
    //        cache: false,
    //        async: true,
    //        dataType: "json",
    //        contentType: 'application/json',
    //        success: function (response) {
    //            var select = $("#roomCategories")
    //            response.forEach(function (d) {
    //                select.append('<option asp-for="RoomCategoryId" value="' + d.id + '">' + d.title + '</option>');
    //            })
    //            roomCategory = $("#roomCategory").val()
    //            select.val(roomCategory)
    //        },
    //        error: function (request, ajaxOptions, exception) {
    //            alert(request);
    //            alert(exception);
    //        }
    //    })
    //});
    $("#main-image-input").change(function () {
        updateImageDisplay(this, "main-image")
    });
    $("#room-images-0-input").change(function () {
        updateImageDisplay(this, "room-images-0")
    });
    $("#room-images-1-input").change(function () {
        updateImageDisplay(this, "room-images-1")
    });
    $("#room-images-2-input").change(function () {
        updateImageDisplay(this, "room-images-2")
    });
    completed = function (message, statusCode) {
        alert(message.responseText)
        if (statusCode == "success") {
            window.location.href = '/Admin/RoomCategoriesList';
        }
    }
    //$("#submitButton").click(function () {
    //    var input = document.getElementById("main-image-input")
    //    alert(input.files[0])
    //})

})
$('#Services_dropdown').multi({
    non_selected_header: 'Сервисы',
    selected_header: 'Выбранные сервисы',
    enable_search: true,
    search_placeholder: "Поиск..."
});
function updateImageDisplay(inputName, imageName) {
    alert(inputName)
    if (inputName.files && inputName.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + imageName).attr('src', e.target.result);
        }
        reader.readAsDataURL(inputName.files[0]);
    }
}