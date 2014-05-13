// JavaScript source code
function negativar() {
    // the css we are going to inject
    var css = 'html {-webkit-filter: invert(100%);' +
        '-moz-filter: invert(100%);' +
        '-o-filter: invert(100%);' +
        '-ms-filter: invert(100%); ' +
        'filter: invert(100%); ' +
        'filter: url("data:image/svg+xml;utf8,<svg xmlns=\'http://www.w3.org/2000/svg\'><filter id=\'invert\'><feColorMatrix in=\'SourceGraphic\' type=\'matrix\' values=\'-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0\'/></filter></svg>#invert"); }'
    ,

    head = document.getElementsByTagName('head')[0],
    style = document.createElement('style');

    // a hack, so you can "invert back" clicking the bookmarklet again
    if (!window.counter) { window.counter = 1; } else {
        window.counter++;
        if (window.counter % 2 == 0) { var css = 'html {-webkit-filter: invert(0%); -moz-filter:    invert(0%); -o-filter: invert(0%); -ms-filter: invert(0%); }' }
    };

    style.type = 'text/css';
    if (style.styleSheet) {
        style.styleSheet.cssText = css;
    } else {
        style.appendChild(document.createTextNode(css));
    }

    //injecting the css to the head
    head.appendChild(style);
}

function increaseFont() {
    $('body').css("font-size", function () {
        return parseInt($(this).css('font-size')) + 1 + 'px';
    });
}

function decreaseFont() {
    $('body').css("font-size", function () {
        return parseInt($(this).css('font-size')) - 1 + 'px';
    });
}

function resetFont() {
    $('body').css("font-size", function () {
        $(this).css('font-size', '100%');
    });
}