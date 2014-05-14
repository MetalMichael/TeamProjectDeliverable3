//functionality for checking the weeks
$(document).ready(function () {
    $('#room-features input[type="hidden"').remove();
    $($('#Priority').siblings()[0]).remove();
    $($('#AdHoc').siblings()[0]).remove();

    $('#AdHoc').change(function () {
        if ($('#Priority').prop("checked")) {
            $('#Priority').click();
        }
        if ($(this).prop("checked")) {
            $($('#Semester').val(1).children()[1]).prop("disabled", "disabled");
            $($('#Semester').children()[0]).removeAttr("disabled");

        } else {
            $($('#Semester').children()[1]).removeAttr("disabled");
            $($('#Semester').val(2).children()[0]).prop("disabled", "disabled");
        }
    });

    $($('#Semester').val(2).children()[0]).prop("disabled", "disabled");

    $('#Priority').change(function () {
        if ($('#AdHoc').prop("checked")) {
            $('#AdHoc').click();
        }
    });

    $('#ModuleCode').change(function () {
        $('#ModuleCodes').val($(this).val());
    });

    $('#ModuleCodes').change(function () {
        $('#ModuleCode').val($(this).val());
    });
    $('#ModuleCodes').val($('#ModuleCode').val());

    $('#module-filter').keyup(function () {
        var filter = $('#module-filter').val().toLowerCase();
        var codes = [];

        $('#ModuleCode').children().each(function () {
            if ($(this).html().toLowerCase().indexOf(filter) == -1) {
                $(this).hide();
            } else {
                $(this).show();
                codes.push($(this).val());
            }
        });

        $('#ModuleCodes').children().each(function () {
            if (codes.indexOf($(this).val()) == -1) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });

    function clearAll() {
        for (var i = 1; i < 16; i++) {
            var cb = '#Week' + i;
            $(cb).prop('checked', false);
        }
    }
    $('#clearAll').click(function () {
        clearAll();
        $("#Week1").change();
    });
    $('#checkAll').click(function () {
        for (var i = 1; i < 16; i++) {
            var cb = '#Week' + i;
            $(cb).prop('checked', true);
        }
        $("#Week1").change();
    });
    $('#oneToTwelve').click(function () {
        clearAll();
        for (var i = 1; i < 13; i++) {
            var cb = '#Week' + i;
            $(cb).prop('checked', true);
        }
        $("#Week1").change();
    });
    $('#checkEven').click(function () {
        clearAll();
        for (var i = 1; i < 16; i++) {
            if (i % 2 == 0) {
                var cb = '#Week' + i;
                $(cb).prop('checked', true);
            }
        }
        $("#Week1").change();
    });
    $('#checkOdd').click(function () {
        clearAll();
        for (var i = 1; i < 16; i++) {
            if (i % 2 == 1) {
                var cb = '#Week' + i;
                $(cb).prop('checked', true);
            }
        }
        $("#Week1").change();
    });

    /*
    //selects the correct semester based on todays date
    var today = new Date();
    var sem_start_1 = new Date('09/28/2013');
    var sem_end_1 = new Date('01/02/2014');
    var sem_start_2 = new Date('02/02/2014');
    var sem_end_2 = new Date('06/30/2014');

    //if you are currently in semester 1, default semester is set to 2, and vice-versa
    if (today >= sem_start_1 && today <= sem_end_1) {

    //select semester 2, as date is inside semester 1
    $("input[type='radio'][name='semester'][value='2']").prop('checked', true);

    //enables correct ad-hoc mode for correct semester, e.g. in semester 1, default semester is 2, so if you click ad-hoc it goes back to semester 1, and clicking semester 2 removes ad-hoc
    var $checks = $('input[name="adhoc"]').click(function () {
    if (this.checked) {
    $rsdios.prop('checked', false)
    $("#sem1").prop('checked', true);

    $(".all_weeks").attr('checked', false);
    $("#allWeeks").attr('checked', false);
    $("#defWeeks").attr('checked', false);
    $("#oddWeeks").attr('checked', false);
    $("#evenWeeks").attr('checked', false);
    $('.weeks:first').trigger('change');
    }
    });
    var $rsdios = $('input[id="sem2"]').click(function () {
    if (this.checked) {
    $checks.prop('checked', false)
    }
    });

    }

    if (today >= sem_start_2 && today <= sem_end_2) {

    //select semester 1, as date is inside semester 2
    $("input[type='radio'][name='semester'][value='1']").prop('checked', true);

    //enables correct ad-hoc mode for correct semester, e.g. in semester 2, default semester is 1, so if you click ad-hoc it goes back to semester 2, and clicking semester 1 removes ad-hoc
    var $checks = $('input[name="adhoc"]').click(function () {
    if (this.checked) {
    $rsdios.prop('checked', false)
    $("#sem2").prop('checked', true);

    $(".all_weeks").attr('checked', false);
    $("#allWeeks").attr('checked', false);
    $("#defWeeks").attr('checked', false);
    $("#oddWeeks").attr('checked', false);
    $("#evenWeeks").attr('checked', false);
    $('.weeks:first').trigger('change');
    }
    });
    var $rsdios = $('input[id="sem1"]').click(function () {
    if (this.checked) {
    $checks.prop('checked', false)
    }
    });

    }

    $('input[type="radio"].semester').bind('keyup change', function () {

    // get elements that are empty.
    var empty = $('input[type="radio"].semester').map(function (index, el) {
    return !$(el).val().length ? el : null;
    }).get();

    // could also be placed outside of the function
    var tick = $('#tick');
    var cross = $('#cross');

    // check if there are any empty elements, if there are none, show numbers, else hide number.
    !empty.length ? tick.show() : tick.hide();
    !empty.length ? cross.hide() : cross.show();
    });

    //Select module table, change cross to tick
    $('#module_title').bind('keyup change', function () {

    // get elements that are empty.
    var empty = $('#module_title').map(function (index, el) {
    return !$(el).val().length ? el : null;
    }).get();

    // could also be placed outside of the function
    var tick = $('#tick2');
    var cross = $('#cross2');

    // check if there are any empty elements, if there are none, show numbers, else hide number.
    !empty.length ? tick.show() : tick.hide();
    !empty.length ? cross.hide() : cross.show();
    });

    //Select weeks table, change cross to tick
    $('.weeks').bind('keyup change', function () {


    // get elements that are empty.
    var checked = $('.weeks').filter(':checked');

    // could also be placed outside of the function
    var tick = $('#tick3');
    var cross = $('#cross3');

    // check if there are any empty elements, if there are none, show numbers, else hide number.
    checked.length ? tick.show() : tick.hide();
    checked.length ? cross.hide() : cross.show();
    });

    //Select time table, change cross to tick
    $('.day, #start_time').bind('keyup click', function () {

    // get elements that are empty.
    var empty = $('.day, #start_time').map(function (index, el) {
    return !$(el).val().length ? el : null;
    }).get();

    // could also be placed outside of the function
    var tick = $('#tick4');
    var cross = $('#cross4');

    // check if there are any empty elements, if there are none, show numbers, else hide number.
    !empty.length ? tick.show() : tick.hide();
    !empty.length ? cross.hide() : cross.show();
    });

    //Select roomtype table, change cross to tick
    $('.roomtype').bind('keyup change', function () {

    // get elements that are empty.
    var empty = $('.roomtype').map(function (index, el) {
    return !$(el).val().length ? el : null;
    }).get();

    // could also be placed outside of the function
    var tick = $('#tick5');
    var cross = $('#cross5');

    // check if there are any empty elements, if there are none, show numbers, else hide number.
    !empty.length ? tick.show() : tick.hide();
    !empty.length ? cross.hide() : cross.show();
    });

    //Select room_info table, change cross to tick
    $('#no_students, #no_rooms').bind('keyup change', function () {

    // get elements that are empty.
    var empty = $('#no_students, #no_rooms').map(function (index, el) {
    return !$(el).val().length ? el : null;
    }).get();

    // could also be placed outside of the function
    var tick = $('#tick6');
    var cross = $('#cross6');

    // check if there are any empty elements, if there are none, show numbers, else hide number.
    !empty.length ? tick.show() : tick.hide();
    !empty.length ? cross.hide() : cross.show();
    });
    */
});