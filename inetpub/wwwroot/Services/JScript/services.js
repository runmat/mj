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

function LogPageVisit(appId, href) {
    if (IsIEVersionOrLower(8)) {

        // 12.05.2014
        // Kunde Athlon hat Probleme unsere Services Menüpunkte mit seinen IE 8 Browsern aufzurufen.
        // Wir vermuten, dass hier das Javacsript mäßig zwischengeschaltete Logging in Kombination mit alten IE Browser Versionen u. U. die Ursache ist.

        // Ich habe daher in Absprache mit Hinrich das Logging bei IE Browsern <= Version 8  vorerst abgeschaltet.
        // Hinrich fragt nun beim Kunden nach, ob er jetzt wieder mit seinem IE 8 arbeiten kann.
        // Falls nein, liegt die Ursache woanders und ich drehe diese Maßnahme entsprechend wieder zurück.

        return true;
    }

    // Ermittle den Wert des HiddenInput idOfSkipPageVisitLogInput um fest zu stellen ob für diesen das Logging eingerichtet werden soll oder nicht
    // Variable idOfSkipPageVisitLogInput ist in der ServicesMenue.ascx deklariert worden.
    var idOfHiddenField = '#' + idOfSkipPageVisitLogInput;

    if ($(idOfHiddenField).val() == 'true') {
        return true;
    }

    var url = '/Services/Log.aspx?APP-ID=' + appId;
    $.get(url).always(function () {
        window.location.href = href;
    });

    return false;
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