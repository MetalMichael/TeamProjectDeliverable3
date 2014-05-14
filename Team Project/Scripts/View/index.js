$(document).ready(function () {
    $('tr').each(function () {
        var status = $(this).find('.status').html()
        if (typeof status == "string") {
            //Pending
            if (status.indexOf("Pending") > -1) {
                $(this).find('.resubmit').hide();
            //Accepted
            } else if (status.indexOf("Accepted") > -1) {
                $(this).find('.controls').hide();
                if ($(this).find('.rooms').html() !== $(this).find('.acceptedRooms').html()) {
                    $(this).css('background-color', '#FFC65D');
                } else {
                    $(this).css('background-color', '#4DA269');
                }
            //Failed
            } else {
                $(this).find('.edit, .delete').hide();
                $(this).css('background-color', '#E64545');
            }
        }
    });

    $('.delete').click(function () {
        if (confirm("Are you sure you wish to delete this Request?\nThis cannot be undone")) {
            return true;
        }
        return false;
    });

});

