$(document).ready(function(){
 // Booking table cross / tick   **NEW
$('input[type="radio"][name="Day"]').bind('keyup change', function () {

    // get elements that are empty
    var empty = $('input[type="radio"][name="Day"]').map(function (index, el) {
        return !$(el).val().length ? el : null;
    }).get();

    // could also be placed outside of the function
    var tick = $('#tick1');
    var cross = $('#cross1');

    // check if there are any empty elements, if there are none, show numbers, else hide number.
    !empty.length ? tick.show() : tick.hide();
    !empty.length ? cross.hide() : cross.show();
});   
}

//Booking table change cross to tick     **OLD
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