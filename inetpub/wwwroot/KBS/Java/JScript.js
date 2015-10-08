var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_initializeRequest(initializeRequest);

var postbackElement;

function initializeRequest(sender, args) {

    if (prm.get_isInAsyncPostBack()) {

        //debugger
        args.set_cancel(true);

    }

}        

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
    else if (decimal && ((keychar == ".") || (keychar == ","))) {
        return true;
    }
    else
        return false;
}


// für Change02_1
function SelectRbandChk(gridcount, isChecked) {
    var idRoot = 'ctl00_ContentPlaceHolder1_GridView1_ctl';
    var idrbTail = '_rbPositionAbgeschlossenJA';
    var idrbTailRbNein = '_rbPositionAbgeschlossenNEIN';
    var idChkTail = '_chkVollstaendig';
    var idtxtTail = '_txtPositionLieferMenge';

    var count = 2;
    var total = 0.0;
    var txtctrl = null;
    var chkctrl = null;
    var rbctrl = null;
    var rbctrlNein = null;
    
    var i = 0;
    while (i < gridcount) {

        var i2 = i + 2;
        if (i2 < 10) {
            count = '0' + i2;
        }
        else {
            count = i2;
        }
        //
        // ID für CheckBox und Radiobutton zusammenbauen.
        //
        rbctrl = document.getElementById(idRoot + count + idrbTail);
        chkctrl = document.getElementById(idRoot + count + idChkTail);
        txtctrl = document.getElementById(idRoot + count + idtxtTail);
        rbctrlNein = document.getElementById(idRoot + count + idrbTailRbNein);
        if (rbctrl != null && rbctrlNein != null) {
            if (isChecked == true) {
                if (rbctrl.checked || rbctrlNein.checked) {
                    chkctrl.checked = false;
                    //                rbctrl.disabled = true;
                    txtctrl.disabled = true;
                    //                rbctrlNein.disabled = true;
                }
                else if (txtctrl.value == '') {
                    chkctrl.checked = true;
                    rbctrl.disabled = true;
                    txtctrl.disabled = true;
                    rbctrlNein.disabled = true;
                }
            }
            else {

                chkctrl.checked = false;
                rbctrl.disabled = false;
                txtctrl.disabled = false;
                rbctrlNein.disabled = false;
                rbctrl.checked = false;
                rbctrlNein.checked = false;

            }
        }
        else if (isChecked == true) {

                chkctrl.checked = true;
                txtctrl.disabled = true;
        }
        else {
            chkctrl.checked = false;
            txtctrl.disabled = false;
        }
        
        i++
      }
   
}


function checkedRow(ChkBox, TxtBox, rbJa, rbNein) {
    var formCheckBox = document.getElementById(ChkBox);
    var formTextBox = document.getElementById(TxtBox);
    var formrbJa = document.getElementById(rbJa);
    var formrbNein = document.getElementById(rbNein);

    formTextBox.disabled = formCheckBox.checked;
    if (formrbJa != null && formrbNein != null) { 
        if (formCheckBox.checked == false) {
            formrbJa.checked = false;
            formrbNein.checked = false;    
         }
    
        formrbJa.disabled = formCheckBox.checked;
        formrbNein.disabled = formCheckBox.checked;
   }
}
function MengeChanged(HiddenInput, TxtBox) {
    var formTextBox = document.getElementById(TxtBox);
    var formHiddenInput = document.getElementById(HiddenInput);
    formHiddenInput.value = formTextBox.value;

}


// für Change01
function setFocusAfterInput(obj) {

    if (obj.value.indexOf("}", 0) != -1) {
        obj.value = obj.value.replace("}", "");
        obj.value = obj.value.replace("|", "");
        setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$txtEAN\',\'\')', 0);
    }
}

function Calculate(lblEinheit, txtMenge, lblErgebnis, lblGesamt) {
    var formEinheit = document.getElementById(lblEinheit).innerText;
    var formMenge = document.getElementById(txtMenge).value;
    var formErgebnis = document.getElementById(lblErgebnis);
    var formGesamt = document.getElementById(lblGesamt);

    if (formMenge.length > 0) {
        formEinheit = formEinheit.replace(",", ".");
        var Ergebnis
        Ergebnis = (parseFloat(formEinheit) * parseFloat(formMenge)).toFixed(2);
        formErgebnis.value = Ergebnis.replace(".", ",");
        if (formGesamt.value.length > 0) 
        {
            formGesamt.value = CalculateGesamt();
        }
        else {
            formGesamt.value = formErgebnis.value;
            

        }
        formGesamt.value = formGesamt.value.replace(".", ",");
    }
}
function CalculateGesamt() 
{

    var Gesamt = "0";
    var Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblSchein500").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt*1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblSchein200").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblSchein100").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblSchein50").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblSchein20").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblSchein10").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblSchein5").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

//---
    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck2").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck1").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck050").value;
    Ergebnis = Ergebnis.replace(",", ".")
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck020").value;
    Ergebnis = Ergebnis.replace(",", ".")
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck010").value;
    Ergebnis=Ergebnis.replace(",", ".")
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck005").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Ergebnis = Ergebnis.replace(",", ".")
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck002").value;
    if (Ergebnis == "") { Ergebnis = "0" }
    Ergebnis = Ergebnis.replace(",", ".")
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;

    Ergebnis = document.getElementById("ctl00_ContentPlaceHolder1_lblStueck001").value;
    Ergebnis = Ergebnis.replace(",", ".")
    if (Ergebnis == "") { Ergebnis = "0" }
    Gesamt = parseFloat(Ergebnis).toFixed(2) * 1 + Gesamt * 1;
    
    return Gesamt.toFixed(2) 
    

 }


 $(document).ready(function () {
     // Sendebuttons nach dem Draufklicken deaktivieren, um (versehentliche) Mehrfachabsendungen zu verhindern
     $(".SendeButton").click(function () {
         DisableButtonWithDelay(this);
     });
 });

 function DisableButton(btn) {
     $(btn).attr('disabled', 'disabled');
 }

 // Verzögerung für IE7-/IE8-Kompatibilität erforderlich
 function DisableButtonWithDelay(btn) {
     setTimeout(function () { DisableButton(btn); }, 100);
 }

 // WERKTAGE (FÜR DATEPICKER) ERMITTELN

 listeFesteFeiertage = [
  [1, 1],
  [5, 1],
  [10, 3],
  [12, 25],
  [12, 26]
];

 // div-Operation
 function berechneDiv(zahl1, zahl2) {
     if (zahl1 * zahl2 > 0) {
         return Math.floor(zahl1 / zahl2);
     }
     else {
         return Math.ceil(zahl1 / zahl2);
     }
 }


 // Erweiterte Gaußsche Osterformel
 function berechneOstersonntag(jahr) {
     var k = berechneDiv(jahr, 100);
     var m = 15 + berechneDiv(3 * k + 3, 4) - berechneDiv(8 * k + 13, 25);
     var s = 2 - berechneDiv(3 * k + 3, 4);
     var a = jahr % 19;
     var d = (19 * a + m) % 30;
     var r = berechneDiv(berechneDiv(d + a, 11), 29);
     var og = 21 + d - r;
     var sz = 7 - ((jahr + berechneDiv(jahr, 4) + s) % 7);
     var oe = 7 - ((og - sz) % 7);
     var os = og + oe;
     // Monat auch hier wieder 0..11
     if (os > 31) {
         return new Date(2013, 3, os - 31);
     }
     else {
         return new Date(2013, 2, os);
     }
 }

 function keinFeiertag(date) {
     var jahr = date.getFullYear();
     var monat = date.getMonth();
     var tag = date.getDate();
     // statische Feiertage
     for (i = 0; i < listeFesteFeiertage.length; i++) {
         // getMonth liefert 0..11, deshalb -1
         if ((monat == listeFesteFeiertage[i][0] - 1)
          && (tag == listeFesteFeiertage[i][1])) {
             return [false, ''];
         }
     }
     // dynamische Feiertage
     // Ostersonntag
     var ostersonntag = berechneOstersonntag(jahr);
     if ((monat == ostersonntag.getMonth())
          && (tag == ostersonntag.getDate())) {
         return [false, ''];
     }
     // Karfreitag
     var feiertag = new Date(ostersonntag.getFullYear(), ostersonntag.getMonth(), ostersonntag.getDate());
     feiertag.setDate(ostersonntag.getDate() - 2);
     if ((monat == feiertag.getMonth())
          && (tag == feiertag.getDate())) {
         return [false, ''];
     }
     // Ostermontag
     feiertag = new Date(ostersonntag.getFullYear(), ostersonntag.getMonth(), ostersonntag.getDate());
     feiertag.setDate(ostersonntag.getDate() + 1);
     if ((monat == feiertag.getMonth())
              && (tag == feiertag.getDate())) {
         return [false, ''];
     }
     // Christi Himmelfahrt
     feiertag = new Date(ostersonntag.getFullYear(), ostersonntag.getMonth(), ostersonntag.getDate());
     feiertag.setDate(ostersonntag.getDate() + 39);
     if ((monat == feiertag.getMonth())
              && (tag == feiertag.getDate())) {
         return [false, ''];
     }
     // Pfingstmontag
     feiertag = new Date(ostersonntag.getFullYear(), ostersonntag.getMonth(), ostersonntag.getDate());
     feiertag.setDate(ostersonntag.getDate() + 50);
     if ((monat == feiertag.getMonth())
              && (tag == feiertag.getDate())) {
         return [false, ''];
     }
     return [true, ''];
 }

 function istKeinWochenende(date) {
     var wochentag = date.getDay();
     if ((wochentag == 0) || (wochentag == 6)) {
         return [false, ''];
     }
     return [true, ''];
 }

 // Werktagsermittlung (mit Datepicker)
 function nurWerktageDatepicker(date) {
     var keinWochenende = $.datepicker.noWeekends(date);
     if (keinWochenende[0]) {
         return keinFeiertag(date);
     } else {
         return keinWochenende;
     }
 }

 // Werktagsermittlung (ohne Datepicker, verarbeitet auch Text im Format "TTMMJJ")
 function nurWerktage(date) {
     if ((date != null) && (date != "")) {
         var tempDate;
         if (date instanceof Date) {
             tempDate = date;
         }
         else {
             jahr = 2000 + parseInt(date.substring(4, 6), 10);
             monat = parseInt(date.substring(2, 4), 10) - 1;
             tag = parseInt(date.substring(0, 2), 10);
             tempDate = new Date(jahr, monat, tag);
         }
         var keinWochenende = istKeinWochenende(tempDate);
         if (keinWochenende[0]) {
             return keinFeiertag(tempDate);
         } else {
             return keinWochenende;
         }
     }
     return [true, ''];
 }

 function SetDate(Day, Textbox) {

     var datum = new Date();

     // Heute
     if (Day == 0) {
         datum.setDate(datum.getDate());
         // ggf. Feiertage überspringen
         while (!keinFeiertag(datum)[0]) {
             datum.setDate(datum.getDate() + 1);
         }
     }

     // Gestern
     if (Day == -1) {
         if (datum.getDay() == 1) {
             datum.setDate(datum.getDate() - 3);
         }
         else if (datum.getDay() == 0) {
             datum.setDate(datum.getDate() - 2);
         }
         else {
             datum.setDate(datum.getDate() - 1);
         }
         // ggf. Feiertage überspringen
         while (!keinFeiertag(datum)[0]) {
             datum.setDate(datum.getDate() - 1);
         }
     }

     // Morgen
     if (Day == 1) {
         if (datum.getDay() == 5) {
             datum.setDate(datum.getDate() + 3);
         }
         else if (datum.getDay() == 6) {
             datum.setDate(datum.getDate() + 2);
         }
         else {
             datum.setDate(datum.getDate() + 1);
         }
         // ggf. Feiertage überspringen
         while (!keinFeiertag(datum)[0]) {
             datum.setDate(datum.getDate() + 1);
         }
     }

     var t;
     t = document.getElementById(Textbox);
     t.value = datum.localeFormat('ddMMyy');
 }

 function FilterKennz(Control, e) {
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
     
     if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 16) || (key == 13) || (key == 27) || (key == 37) || (key == 39)) {
         return false;
     }

     Control.value = Control.value.toUpperCase();
     Control.focus();

     return true;
 }
