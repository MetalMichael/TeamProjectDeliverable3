var viewtable;

$(document).ready(function () {
    $('#Rounds').change(function () {
        filter();
    });

    $('#Semester').change(function () {
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
    var sem = $('#Semester').val();

    var semester = $('#Rounds').val().substr(0, 1);
    var round = $('#Rounds').val().substr(1, 1);

    var status = $('#Status').val().trim();

    var moduleCode = $('#ModuleCode').val().trim();

    bool semdiff = false;
    if (sem.indexOf("<") == -1) {
        semdiff = true;
        viewtable.fnFilter(sem, 0);
    }

    if (semester != "<") {
        if (semdiff && sem != semester) {
            semester = sem;
        }
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