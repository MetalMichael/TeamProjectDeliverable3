$(document).ready(function () {
    $('tr').each(function () {
        var status = $(this).find('.status').html()
        if (typeof status == "string" && status.indexOf("Pending") == -1) {
            $(this).find('.edit').hide();
        }
    });

    $('#requests-table').dataTable();
});

