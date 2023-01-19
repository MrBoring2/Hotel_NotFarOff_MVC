$(document).ready(function () {
    $(function () {
        $.ajax({
            url: '../Employees/GetPostsTitlesList',
            type: 'GET',
            cache: false,
            async: true,
            dataType: "json",
            contentType: 'application/json',
            success: function (response) {
                var select = $("#employeePosts")
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

    var dataTable = $('#employeesTable').DataTable({
        ajax: {
            url: "../Employees/GetList",
            type: "POST",
            data: function (data) {
                var employeePost = $("#employeePosts").val()
                var employeeGender = $("#employeeGenders").val()

                data.filterEmployeePost = employeePost
                data.filterEmployeeGender = employeeGender
            }
        },
        processing: true,
        serverSide: true,
        filter: true,
        columns: [
            { data: "Id", name: "Id" },
            { data: "FullName", name: "FullName" },
            { data: "Passport", name: "Passport" },
            { data: "PhoneNumber", name: "PhoneNumber" },
            {
                data: "DateOfBirth", name: "DateOfBirth", "render": function (data) {
                    return new Date(Date.parse(data)).toLocaleDateString("ru-RU")
                }
            },
            { data: "Gender", name: "Gender" },
            { data: "Post.Title", name: "Post.Title" },
            { data: "Account.Login", name: "Account.Login" },
            {
                "data": null,
                "orderable": false,
                "render": function (data, type, full, meta) {
                    return `<a class='btn btn-primary btn-sm' href='/Admin/EmployeeDetails/` + full.Id + `' style='margin-right:3px'><i class='fas fa-folder'></i>Детали</a>
                            <a class='btn btn-info btn-sm' href='/Admin/EmployeeEdit/` + full.Id + `' style='margin-right:3px'><i class='fas fa-pencil-alt' style='margin-right:3px'></i>Редактировать</a>
                            <a class='btn btn-danger btn-sm' href='#' onclick=DeleteData('` + full.Id + `'); style='margin-right:3px'><i class='fas fa-trash'></i>Удалить</a>`;
                },
                "width": "22%"
            }
        ]
    })
    $('#employeePosts').change(function () {
        dataTable.draw();
    });
    $('#employeeGenders').change(function () {
        dataTable.draw();
    });

})

function DeleteData(employeeId) {
    if (confirm("Вы уверены, что хотите удалить?")) {
        Delete(employeeId);
    } else {
        return false;
    }
}


function Delete(employeeId) {
    var url = "../Employees/Delete/" + employeeId;
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            oTable = $('#employeesTable').DataTable();
            oTable.draw();
            alert("Сотрудник успешно удалён")
        },
        error: function (errormessage) {
            alert(errormessage.responseText)
        }
    });
}

