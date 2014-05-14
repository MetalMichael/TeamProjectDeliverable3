var viewtable;

$(document).ready(function () {
    $('#Rounds').change(function () {
        filter();
    });

    $('#Status').change(function () {
        filter();
    });

    $('#Module').change(function () {
        $('#ModuleCode').val($(this).val());
        filter();
    });

    $('#ModuleCode').change(function () {
        $('#Module').val($(this).val());
        filter();
    });

    viewtable = $('#requests-table').dataTable();
});

function filter() {
    var semester = $('#Rounds').val().substr(0, 1);
    var round = $('#Rounds').val().substr(1, 1);

    var status = $('#Status').val().trim();

    var moduleCode = $('#ModuleCode').val().trim();

    if (semester != "<") {
        viewtable.fnFilter(semester, 0);
        viewtable.fnFilter(round, 1);
    } else {
        viewtable.fnFilter('', 0);
        viewtable.fnFilter('', 1);
    }
    if (status.indexOf("<") == -1) {
        console.log(status);
        viewtable.fnFilter(status, 10);
    } else {
        viewtable.fnFilter('', 10);
    }
    if (moduleCode != "") {
        viewtable.fnFilter(moduleCode, 2);
    } else {
        viewtable.fnFilter('', 2);
    }
}