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
    else if (decimal && (keychar == ".")) {
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