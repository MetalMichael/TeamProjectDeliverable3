$(document).ready(function () {
    $('tr').each(function () {
        var status = $(this).find('.status').html()
        if (typeof status == "string" && status.indexOf("Pending") == -1) {
            $(this).find('.controls').hide();
        }
    });

    $('.delete').click(function () {
        if (confirm("Are you sure you wish to delete this Request?\nThis cannot be undone")) {
            return true;
        }
        return false;
    });

    $('#requests-table').dataTable();
});

