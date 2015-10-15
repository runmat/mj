$(document).ready(function () {

    $('input[name$="datum"]').each(function () {
        //$(this).datepicker();
    });

    //FormPrepare();
});

function FormPrepare(setFirstFocus) {
    if (setFirstFocus)
        $("input:text:visible:first").focus();

    FormEnterKeyToTab();
}

function FormEnterKeyToTab() {
    //
    // enter key to TAB on all inputs (forms)
    $('input').keydown(function (e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key == 13) {
            e.preventDefault();
            var inputs = $(this).closest('form').find(':input:text:visible');
            inputs.eq(inputs.index(this) + 1).focus();
        }
    });
}

function NumbersOnly(e, decimal) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    }
    else if (e) {
        key = e.which;
    }
    else {
        return true;
    }
    keychar = String.fromCharCode(key);

    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
    }
    else if ((("0123456789").indexOf(keychar) > -1)) {
        return true;
    }
    else if (decimal && (keychar == ",")) {
        return true;
    }
    else
        return false;
}

function LettersOnly(e, upperCaseOnly) {
    var key;

    if (window.event) {
        key = window.event.keyCode;
    }
    else if (e) {
        key = e.which;
    }
    else {
        return true;
    }

    var keychar = String.fromCharCode(key);

    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
    }
    else if ((key > 64 && key < 91) || (!upperCaseOnly && key > 96 && key < 123)) {
        return true;
    }
    else if ((("ÄÖÜ").indexOf(keychar) > -1) || (!upperCaseOnly && ("äöü").indexOf(keychar) > -1)) {
        return true;
    }
    else
        return false;
}

function jsAppend(jsFile) {
    var jsScript = document.createElement('script');
    jsScript.type = "text/javascript";
    jsScript.src = jsFile;
    document.getElementsByTagName('head')[0].appendChild(jsScript);
}

// Matthias Jenzen, 08.01.2013
//   For IE 7.0 and lower: 
//   Append special json extension script 
if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
    var ieversion = new Number(RegExp.$1); // capture x.x portion and store as a number
    if (ieversion <= 7) {
        //alert(ieversion);
        jsAppend('~/Scripts/IE7/json.js');
    }
}

