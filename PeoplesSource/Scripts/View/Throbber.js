var throbberContainer;

function InitThrobber(container) {
    throbberContainer = $(container)

    throbberContainer.throbber({
        fgcolor: "#606662" //(dark grey)
    });
    throbberContainer.hide();    
    $(document).ajaxStart(function () {
        StartThrobber();
    }).ajaxComplete(function () {
        StopThrobber();
    });
}

function StartThrobber() {    
    $("#throbber").css("top", ($(window).height() - $("#throbber").height()) / 2 + $(window).scrollTop() + "px");
    $("#throbber").css("left", ($(window).width() - $("#throbber").width()) / 2 + $(window).scrollLeft() + "px");
    throbberContainer.show();
    throbberContainer.throbber('enable');
}

function StopThrobber() {    
    throbberContainer.throbber('disable');
    throbberContainer.hide();
}

