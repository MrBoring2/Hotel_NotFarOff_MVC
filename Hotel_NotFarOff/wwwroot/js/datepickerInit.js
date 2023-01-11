$(document).ready(function () {
    $(function () {
        var dateIn = new Date()
        $("#date-in").datepicker('setDate', dateIn)
        var dateOut = new Date(dateIn)
        dateOut.setDate(dateIn.getDate() + 1)
        $("#date-out").datepicker('setDate', dateOut)
    })
})