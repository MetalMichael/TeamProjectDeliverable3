$(document).ready(function () {
    $('tr').each(function () {
        var status = $(this).find('.status').html()
        if (typeof status == "string") {
            if (status.indexOf("Pending") > -1) {

            } else if (status.indexOf("Accepted") > -1) {
                $(this).find('.controls').hide();
                if ($(this).find('.rooms').html() !== $(this).find('.acceptedRooms').html()) {
                    $(this).css('background-color', 'orange');
                } else {
                    $(this).css('background-color', 'green');
                }
            } else {
                $(this).find('.controls').hide();
                $(this).css('background-color', 'red');
            }
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

