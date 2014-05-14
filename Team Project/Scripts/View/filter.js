$(document).ready(function () {
    $('#Rounds').change(function () {
        filter();
    });

    $('#Status').change(function () {
        filter();
    });

    $('#ModuleCode').change(function () {
        filter();
    });

    $('#Module').change(function () {
        filter();
    });

    $('#Module').change(function () {
        $('#ModuleCode').val($(this).val());
    });

    $('#ModuleCode').change(function () {
        $('#Module').val($(this).val());
    });
});

function filter() {
    var semester = $('#Rounds').val().substr(0, 1);
    var round = $('#Rounds').val().substr(1, 1);

    var status = $('#Status').val();

    var moduleCode = $('#ModuleCode').val();

    $('tbody tr').each(function () {
        $(this).show();
        if (semester != "<") {
            if ($(this).find(".semester").text().trim() != semester) {
                $(this).hide();
                console.log("hiding 1");
                return true;
            }
            if ($(this).find(".round").text().trim() != round) {
                $(this).hide();
                console.log("hiding 2");
                return true;
            }
        }
        if (status.indexOf("<") == -1) {
            if ($(this).find(".status").text().trim() != status) {
                $(this).hide();
                console.log("hiding 3");
                return true;
            }
        }
        if (moduleCode != "") {
            if ($(this).find(".moduleCode").text().trim() != moduleCode) {
                $(this).hide();
                console.log("hiding 4");
                return true;
            }
        }
    });
}