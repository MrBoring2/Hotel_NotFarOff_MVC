$(document).ready(function () {
    $('#roomsTable').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Rooms/GetList",
            "type": "POST",
            "datatype": "json"
        }
    })
})