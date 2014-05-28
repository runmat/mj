// Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
// Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
var ArraySonderStva;

function numbersonly(e, decimal) {
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

function SetFocus(Control) {
    Control.focus();
}

function CurrencyFormatted(amount) {
    var i = parseFloat(amount);
    if (isNaN(i)) { i = 0.00; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    i = parseInt((i + .005) * 100);
    i = i / 100;
    s = new String(i);
    if (s.indexOf('.') < 0) { s += '.00'; }
    if (s.indexOf('.') == (s.length - 2)) { s += '0'; }
    s = minus + s;
    s = s.replace(/\./g, ',');
    return s;
}

