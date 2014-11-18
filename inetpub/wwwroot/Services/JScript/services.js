// Allg. Javascript-Funktionen und -Werte für den Services-Bereich

$(document).ready(function () {
    // Sendebuttons nach dem Draufklicken deaktivieren, um (versehentliche) Mehrfachabsendungen zu verhindern
    $(".SendeButton").click(function () {
        DisableButtonWithDelay(this);
    });
    // Sendebuttons mit Klasse SendeButtonHide nach dem Draufklicken verstecken, um (versehentliche) Mehrfachabsendungen zu verhindern
    $(".SendeButtonHide").click(function () {
        HideButtonWithDelay(this);
    });
});

function DisableButton(btn) {
    $(btn).attr('disabled', 'disabled');
}

function HideButton(btn) {
    $(btn).css('visibility', 'hidden');
}

// Verzögerung für IE7-/IE8-Kompatibilität erforderlich
function DisableButtonWithDelay(btn) {
    setTimeout(function () { DisableButton(btn); }, 100);
}

// Verzögerung für IE7-/IE8-Kompatibilität erforderlich
function HideButtonWithDelay(btn) {
    setTimeout(function () { HideButton(btn); }, 100);
}

function IsIEVersionOrLower(ieVersion) {
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
        var ieversion = new Number(RegExp.$1);
        if (ieversion <= ieVersion) {
            return true;
        }
    }

    return false;
}