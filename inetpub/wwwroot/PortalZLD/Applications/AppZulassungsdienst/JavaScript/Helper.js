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

var found = 0;
var oldKunnr;
var oldMatnr;
var ArrayMengeERL;
function FilterItems(value, ddlClientID,txtMenge, lblMenge) {

    ddl = ddlClientID;
//    if (ddl.options[ddl.selectedIndex].value != "") {
//       oldKunnr = ddl.options[ddl.selectedIndex].value;
//    }
	found = 0;

	arrTexts = new Array();

	for (i = 0; i < ddl.options.length; i++) {
		arrTexts[i] = new Array();
		arrTexts[i][0] = ddl.options[i].value;
		arrTexts[i][1] = i; 
	}
	arrTexts.sort();

	for (var i = 0; i < arrTexts.length; i++) {

		if (arrTexts[i][0].substr(0, value.length) == value.toUpperCase()) {
			ddl.selectedIndex = arrTexts[i][1];
			found = 1;
			if (ArrayMengeERL != null) {
			    for (var i = 0; i < ArrayMengeERL.length; i++) {
			        var valueArray = ArrayMengeERL[i];
			        if (valueArray[0] == value && valueArray[1] == "X") {
			            txtMenge.style.display = "block"; lblMenge.style.display = "block";
			        }
			        else if (valueArray[0] == value && valueArray[1] == "") {
			            if (txtMenge != null) { txtMenge.style.display = "none";txtMenge.value = "1"; if (lblMenge.style.display != "block") { lblMenge.style.display = "none"; }  }
			        }
			        else if (value == "") {
			            if (txtMenge != null) { txtMenge.style.display = "none"; txtMenge.value = "1"; if (lblMenge.style.display != "block") { lblMenge.style.display = "none"; } }
                    }
			    }
			}
			break;
		}
	}
	if (found == 0) {
		ddl.selectedIndex = 0;        
	}
}

function SetNurEinKennzFuerDL(value, zeilenIndex, chkNurEinKennzID) {
    chk = chkNurEinKennzID;
    if (zeilenIndex == 0) {
        if (value.toString() == "8") {
            chk.checked = true;
        } else {
            chk.checked = false;
        }
    }
}

function DisableButton(buttonID) {
    btn = buttonID;
    btn.disabled = 'true';
}

function FilterSTVA(value, ddlClientID, KennzID) {

	ddl = ddlClientID;
	if (KennzID != null)
	{ KennzID.value = value; }
	for (var i = 0; i < ddl.options.length ; i++) {

		if (ddl.options[i].value.substr(0, value.length) == value.toUpperCase()) {
			ddl.selectedIndex = i;

			break;
		}
	}   
}
function FilterKennz(Control,e) {

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
	if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 16) || (key == 13) || (key == 27) || (key == 37) || (key == 39)) {
		return false;
	}
	else {
		Control.value = Control.value.toUpperCase();

		Control.focus();
	}
}



function SetFocus(Control) {
	Control.focus();
}


// Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
// Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
var ArraySonderStva;

function SetDDLValueSTVA(Textbox, ddlClientID, KennzID)
 {
	
	ddl = ddlClientID;
	if (ddl.options[ddl.selectedIndex].value != "0") 
		{
			if (KennzID != null)
			{
				KennzID.value = ddl.options[ddl.selectedIndex].value;
				if (ArraySonderStva != null)
				 {
					 for (var i = 0; i < ArraySonderStva.length; i++) 
					 {
						 var value = ArraySonderStva[i];
						 if (value[0] == KennzID.value) {
							 KennzID.value = value[1];
						 }
					 }
				}
			}
		}

		Textbox.value = ddl.options[ddl.selectedIndex].value;
	}

	// Überprüfung ob Barkunde, Pauschalkunde, CPD-Kunde 
	// Auswahl Barkunde == chkBar checked (value[1])
	// Auswahl Pauschalkunde = Label Pauschal.Value = Pauschalkunde (value[2])
	// Auswahl CPD-Kunde = clearen der Bank.- und Adressfelder  (value[3])
    // Nicht nur ODER sondern auch UND!!
var ArrayBarkunde;
function SetDDLValuewithBarkunde(Textbox, ddlClientID, chkBarkunde, cmdCreateDisabled) {

    ddl = ddlClientID;
    if (ddl.options[ddl.selectedIndex].value != "0") {
        if (ddl.options[ddl.selectedIndex].value != Textbox.value) {
            ClearAdresseAndBank();
        }
        if (document.getElementById('ctl00_ContentPlaceHolder1_GridView1_ctl02_txtSearch') != null && document.getElementById('ctl00_ContentPlaceHolder1_GridView1_ctl02_lblID_POS') != null)
        { proofdisablingControls(document.getElementById('ctl00_ContentPlaceHolder1_GridView1_ctl02_txtSearch').value, ddl.options[ddl.selectedIndex].value,
                document.getElementById('ctl00_ContentPlaceHolder1_GridView1_ctl02_lblID_POS').innerHTML, document.getElementById('ctl00_ContentPlaceHolder1_hfMatnr').value);
        }   
        Textbox.value = ddl.options[ddl.selectedIndex].value;
        if (ArrayBarkunde != null) {
            for (var i = 0; i < ArrayBarkunde.length; i++) {
                var value = ArrayBarkunde[i];

                if (value[0] == Textbox.value && value[1] == "X") {
                    chkBarkunde.checked = value[1];
                    if (value[0] == Textbox.value && value[2] == "X") {
                        document.getElementById('ctl00_ContentPlaceHolder1_Pauschal').innerHTML = 'Pauschalkunde';
                        return;
                    }
                    else { document.getElementById('ctl00_ContentPlaceHolder1_Pauschal').innerHTML = ''; }
                    return;
                }
                else { chkBarkunde.checked = false; }
                if (value[0] == Textbox.value && value[2] == "X") {
                    document.getElementById('ctl00_ContentPlaceHolder1_Pauschal').innerHTML = 'Pauschalkunde';
                    return;
                }
                else { document.getElementById('ctl00_ContentPlaceHolder1_Pauschal').innerHTML = ''; }
                if (value[0] == Textbox.value && value[3] == "") {
                    ClearAdresseAndBank();
                }

            }
        }


    }
    else { Textbox.value = ""; }
}



function SetTexttValue(ddlClientID, Textbox, txtMenge, lblMenge, lblPosID, lblOldMatnr) {
    Textbox.value = ddlClientID.options[ddlClientID.selectedIndex].value;
    if (ArrayMengeERL != null) {
        for (var i = 0; i < ArrayMengeERL.length; i++) {
            var valueArray = ArrayMengeERL[i];
            if (valueArray[0] == Textbox.value && valueArray[1] == "X") {
                txtMenge.style.display = "block"; lblMenge.style.display = "block";
            }
            else if (valueArray[0] == Textbox.value && valueArray[1] == "") {
                if (txtMenge != null) { txtMenge.style.display = "none"; lblMenge.style.display = "none"; }
            }
            else if (Textbox.value == "") {
                if (txtMenge != null) { txtMenge.style.display = "none";lblMenge.style.display = "none"; }
            }
        }
    }

    if (lblOldMatnr != null) {
        var textKunnr = document.getElementById('ctl00_ContentPlaceHolder1_ddlKunnr');
        proofdisablingControls(Textbox.value, textKunnr.options[textKunnr.selectedIndex].value, lblPosID.innerHTML, lblOldMatnr.innerHTML);
    }

}
//function SetDDLValue(Textbox, ddlClientID) {
function SetDDLValue(Textbox, ddlClientID, lblPosID, lblOldMatnr) {

    ddl = ddlClientID;
    if (ddl.options[ddl.selectedIndex].value != "0" && found == 1) {
        Textbox.value = ddlClientID.options[ddlClientID.selectedIndex].value;
        if (lblOldMatnr != null) {
            var textKunnr = document.getElementById('ctl00_ContentPlaceHolder1_ddlKunnr');
            proofdisablingControls(Textbox.value, textKunnr.options[textKunnr.selectedIndex].value, lblPosID.innerHTML, lblOldMatnr.innerHTML);
        }
    }
    else { Textbox.value = ""; }
}

function SetDDLValueWithoutDisablingButtons(Textbox, ddlClientID) {

    ddl = ddlClientID;
    if (ddl.options[ddl.selectedIndex].value != "0" && found == 1) {
        Textbox.value = ddlClientID.options[ddlClientID.selectedIndex].value;
    }
    else { Textbox.value = ""; }
}

function proofdisablingControls(Value, KunnrValue, PosID, Matnr) {

    if (document.getElementById('ctl00_ContentPlaceHolder1_hfMatnr') != null) {
       // var hfMatnr = document.getElementById('ctl00_ContentPlaceHolder1_hfMatnr');
        var hfKunnr = document.getElementById('ctl00_ContentPlaceHolder1_hfKunnr');
        if (document.getElementById('ctl00_ContentPlaceHolder1_cmdCreate') != null) {
            if (Matnr != Value && PosID == "10") {
                EnableDisableControls(true);
            }
            else if (Matnr != Value && PosID != "10") {
                EnableDisableControls2(true);
            }
            else if (KunnrValue != hfKunnr.value) {
            EnableDisableControls(true);
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_cmdCreate').disabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_cmdNewDLPrice').disabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_lbtnBank').disabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_cmdFindPrize').disabled = false;
            }
        }
    }
}

function proofdisablingControls2(Value, KunnrValue) {

    if (document.getElementById('ctl00_ContentPlaceHolder1_hfMatnr') != null) {
        var hfMatnr = document.getElementById('ctl00_ContentPlaceHolder1_hfMatnr');
        var hfKunnr = document.getElementById('ctl00_ContentPlaceHolder1_hfKunnr');
        if (document.getElementById('ctl00_ContentPlaceHolder1_cmdCreate') != null) {
            if (hfMatnr.value != Value) {
                EnableDisableControls(true); //document.getElementById('ctl00_ContentPlaceHolder1_cmdFindPrize').disabled = false;
            }
            else if (KunnrValue != hfKunnr.value) {
                EnableDisableControls(true);
            }
        }
    }
}

function EnableDisableControls(disabled)
{
    document.getElementById('ctl00_ContentPlaceHolder1_cmdCreate').disabled = disabled;
    document.getElementById('ctl00_ContentPlaceHolder1_cmdNewDLPrice').disabled = disabled;
    document.getElementById('ctl00_ContentPlaceHolder1_lbtnBank').disabled = disabled;
    document.getElementById('ctl00_ContentPlaceHolder1_cmdFindPrize').disabled = !disabled;
    //document.getElementById('ctl00_ContentPlaceHolder1_cmdCreate1').disabled = disabled;
    
}
function EnableDisableControls2(disabled) {
    
    document.getElementById('ctl00_ContentPlaceHolder1_cmdCreate').disabled = disabled;
    document.getElementById('ctl00_ContentPlaceHolder1_lbtnBank').disabled = disabled;
    document.getElementById('ctl00_ContentPlaceHolder1_cmdFindPrize').disabled = disabled;
    var textKunnr = document.getElementById('ctl00_ContentPlaceHolder1_ddlKunnr');
    proofdisablingControls2(document.getElementById('ctl00_ContentPlaceHolder1_GridView1_ctl02_txtSearch').value, textKunnr.value);
}
function SetDDLValueProofCPDMask(ddlClientID, Textbox) {

    if (ddlClientID.options[ddlClientID.selectedIndex].value != oldKunnr) 
    {
        ClearAdresseAndBank();
    }
    if (ddlClientID.options[ddlClientID.selectedIndex].value != "0" && found == 1) {
        Textbox.value = ddlClientID.options[ddlClientID.selectedIndex].value;
    }
    else { Textbox.value = ""; }

}
function SetTextValueProofCPDMask(ddlClientID, Textbox) {

    if (ddlClientID.options[ddlClientID.selectedIndex].value != oldKunnr) {
        ClearAdresseAndBank();
    }
    Textbox.value = ddlClientID.options[ddlClientID.selectedIndex].value;

}
function ClearAdresseAndBank()
{
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtName1').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtName2').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtStrasse').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtPlz').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtOrt').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtKontoinhaber').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtIBAN').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtSWIFT').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_txtGeldinstitut').value = "Wird automatisch gefüllt!";
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_chkEinzug').checked = false;
    document.getElementById('ctl00_ContentPlaceHolder1_ucBankdatenAdresse_chkRechnung').checked = false;
}


function Left(str, n){
	if (n <= 0)
		return "";
	else if (n > String(str).length)
		return str;
	else
		return String(str).substring(0,n);
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
	if (Day == 0) 
    {
        datum.setDate(datum.getDate());
        // ggf. Feiertage überspringen
        while (!keinFeiertag(datum)[0]) 
        {
            datum.setDate(datum.getDate() + 1);
        }
    }

    // Gestern
	if (Day == -1) 
    {
        if (datum.getDay() == 1) 
        {
	        datum.setDate(datum.getDate() - 3);
		}
	    else if (datum.getDay() == 0) 
        {
	        datum.setDate(datum.getDate() - 2);
		}
	    else 
        {
		    datum.setDate(datum.getDate() - 1);
		}
		// ggf. Feiertage überspringen
		while (!keinFeiertag(datum)[0])
        {
		    datum.setDate(datum.getDate() - 1);
		}
	}

    // Morgen
    if (Day == 1) {
        var tomorrowSuccess = false;
        while (!tomorrowSuccess) {
            if (datum.getDay() == 5) {
                datum.setDate(datum.getDate() + 3);
            } else if (datum.getDay() == 6) {
                datum.setDate(datum.getDate() + 2);
            } else {
                datum.setDate(datum.getDate() + 1);
            }
            // ggf. Feiertage überspringen
            while (!keinFeiertag(datum)[0]) {
                datum.setDate(datum.getDate() + 1);
            }

            tomorrowSuccess = (datum.getDay() != 6 && datum.getDay() != 7 && keinFeiertag(datum));
        }
    }

	var t;
	t = document.getElementById(Textbox);
	t.value = datum.localeFormat('ddMMyy');
}



function expandcollapse(obj, row) {
	var div = document.getElementById(obj);
	var img = document.getElementById('img' + obj);

	if (div.style.display == "none") {
		div.style.display = "block";
		if (row == 'alt') {
			img.src = "/PortalZLD/Images/minus.gif";
		}
		else {
			img.src = "/PortalZLD/Images/minus.gif";
		}
		img.alt = "Detailsicht schließen.";
	}
	else {
		div.style.display = "none";
		if (row == 'alt') {
			img.src = "/PortalZLD/Images/Plus1.gif";
		}
		else {
			img.src = "/PortalZLD/Images/Plus1.gif";
		}
		img.alt = "Detailsicht öffnen.";
	}
}

function setFocusAfterInput(obj) {

	if (obj.value.indexOf("}", 0) != -1) {
		obj.value = obj.value.replace("}", "");
		obj.value = obj.value.replace("|", "");
		setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$txtEAN\',\'\')', 0);
	}
}


// Wareneingangdetails
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

function only_O_K_F(e) {
	var key;
	var keychar;
	var event;

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

	if ((key == null) || (key == 111) || (key == 107) || (key == 102) || (key == 70) || (key == 79) || (key == 75)) {
	
		var formTextBox 
		if (window.event) {
			formTextBox = document.getElementById(window.event.srcElement.id);
		}
		else if (e) {
		formTextBox = document.getElementById(e.currentTarget.id);
		}
		formTextBox.value = keychar.toUpperCase();
		return true;
	}
	else
		return false;
}

function Calculate(txtGeb, txtGesamtAlt, lblGesamt, lblLoeschKZ,
		 rbEC, lblGesamtEC, rbBar, lblGesamtBar, rbRE, lblGesamtRE, iMenge) 
{
	if (lblLoeschKZ.innerText != "L") {

		var Geb = document.getElementById(txtGeb);
		var FormGesamtAlt = document.getElementById(txtGesamtAlt);
		var LabelGesamt = document.getElementById(lblGesamt);
		var EC = document.getElementById(rbEC);
		var LabelGesamtEC = document.getElementById(lblGesamtEC);
		var Bar = document.getElementById(rbBar);
		var LabelGesamtBar = document.getElementById(lblGesamtBar);
		var RE = document.getElementById(rbRE);
		var LabelGesamtRE = document.getElementById(lblGesamtRE);
 
		if (Geb.value.length == 0) { Geb.value = "0,00" }
		if (FormGesamtAlt.value.length == 0) { FormGesamtAlt.value = "0,00" }
		var i = 0.00;
		var Gesamt = LabelGesamt.innerHTML.replace(/,/, '.');
		var GesamtAlt = FormGesamtAlt.value.replace(/,/, '.');
		i = (parseFloat(Gesamt) - parseFloat(GesamtAlt * iMenge));
		i += parseFloat(Geb.value.replace(/,/, '.') * iMenge);
		LabelGesamt.innerHTML = CurrencyFormatted(i);
	   

		if (EC.checked == true) 
		{
			var iEC = 0.00;
			var GesamtEC = LabelGesamtEC.innerHTML.replace(/,/, '.');
			iEC = (parseFloat(GesamtEC) - parseFloat(GesamtAlt * iMenge));
			iEC += parseFloat(Geb.value.replace(/,/, '.') * iMenge);
			LabelGesamtEC.innerHTML = CurrencyFormatted(iEC);
			
		 }
		if (Bar.checked == true) {
			var iBar = 0.00;
			var GesamtBar = LabelGesamtBar.innerHTML.replace(/,/, '.');
			iBar = (parseFloat(GesamtBar) - parseFloat(GesamtAlt * iMenge));
			iBar += parseFloat(Geb.value.replace(/,/, '.') * iMenge);
			LabelGesamtBar.innerHTML = CurrencyFormatted(iBar);
		}
		if (RE.checked == true) {
			var iRE = 0.00;
			var GesamtRE = LabelGesamtRE.innerHTML.replace(/,/, '.');
			iRE = (parseFloat(GesamtRE) - parseFloat(GesamtAlt * iMenge));
			iRE += parseFloat(Geb.value.replace(/,/, '.') * iMenge);
			LabelGesamtRE.innerHTML = CurrencyFormatted(iRE);
		}

		FormGesamtAlt.value = Geb.value;
	}

}

function CalculateGebAmt(txtGeb, txtGesamtAlt, lblGesamt, lblLoeschKZ, iMenge) 
{
	if (lblLoeschKZ.innerText != "L") 
	
	{

		var Geb = document.getElementById(txtGeb);
		var FormGesamtAlt = document.getElementById(txtGesamtAlt);
		var LabelGesamt = document.getElementById(lblGesamt);
		if (Geb.value.length == 0) { Geb.value = "0,00" }
		if (FormGesamtAlt.value.length == 0) { FormGesamtAlt.value = "0,00" }
		var i = 0.00;
		var Gesamt = LabelGesamt.innerHTML.replace(/,/, '.');
		var GesamtAlt = FormGesamtAlt.value.replace(/,/, '.');
		i = (parseFloat(Gesamt) - parseFloat(GesamtAlt * iMenge));
		i += parseFloat(Geb.value.replace(/,/, '.') * iMenge);
		LabelGesamt.innerHTML = CurrencyFormatted(i);
		FormGesamtAlt.value = Geb.value;
	}

}

function CalculateRadioChanged(txtGeb, lblLoeschKZ, rbEC, lblGesamtEC, rbBar, lblGesamtBar, rbRE, lblGesamtRE, LastChecked) 
{
	if (lblLoeschKZ.innerText != "L") 
	{

		var Geb = document.getElementById(txtGeb);
		var EC = document.getElementById(rbEC);
		var LabelGesamtEC = document.getElementById(lblGesamtEC);
		var Bar = document.getElementById(rbBar);
		var LabelGesamtBar = document.getElementById(lblGesamtBar);
		var RE = document.getElementById(rbRE);
		var LabelGesamtRE = document.getElementById(lblGesamtRE);
		var LastChecked = document.getElementById(LastChecked);

		if (Geb.value.length == 0) { Geb.value = "0,00" }
		if (EC.checked == true) {
			var iEC = 0.00;
			var GesamtEC = LabelGesamtEC.innerHTML.replace(/,/, '.');
			iEC = (parseFloat(GesamtEC) + parseFloat(Geb.value.replace(/,/, '.')));
			LabelGesamtEC.innerHTML = CurrencyFormatted(iEC);
			if (LastChecked.value == "Bar")
			 {
				 iEC = 0.00;
				 var GesamtBar = LabelGesamtBar.innerHTML.replace(/,/, '.');
				 iEC = (parseFloat(GesamtBar) - parseFloat(Geb.value.replace(/,/, '.')));
				 LabelGesamtBar.innerHTML = CurrencyFormatted(iEC);
			 }
			 if (LastChecked.value == "RE") {
				 iEC = 0.00;
				 var GesamtRE = LabelGesamtRE.innerHTML.replace(/,/, '.');
				 iEC = (parseFloat(GesamtRE) - parseFloat(Geb.value.replace(/,/, '.')));
				 LabelGesamtRE.innerHTML = CurrencyFormatted(iEC);
			 }
			 LastChecked.value = "EC";
		}
		if (Bar.checked == true) {
		 
			var iBar = 0.00;
			var GesamtBar = LabelGesamtBar.innerHTML.replace(/,/, '.');
			iBar = (parseFloat(GesamtBar) + parseFloat(Geb.value.replace(/,/, '.')));
			LabelGesamtBar.innerHTML = CurrencyFormatted(iBar);
			if (LastChecked.value == "EC") {
				iBar = 0.00;
				var GesamtEC = LabelGesamtEC.innerHTML.replace(/,/, '.');
				iBar = (parseFloat(GesamtEC) - parseFloat(Geb.value.replace(/,/, '.')));
				LabelGesamtEC.innerHTML = CurrencyFormatted(iBar);
			}
			if (LastChecked.value == "RE") {
				iBar = 0.00;
				var GesamtRE = LabelGesamtRE.innerHTML.replace(/,/, '.');
				iBar = (parseFloat(GesamtRE) - parseFloat(Geb.value.replace(/,/, '.')));
				LabelGesamtRE.innerHTML = CurrencyFormatted(iBar);
			}
			LastChecked.value = "Bar";
		}
		if (RE.checked == true) {
			var iRE = 0.00;
			var GesamtRE = LabelGesamtRE.innerHTML.replace(/,/, '.');
			iRE = (parseFloat(GesamtRE) + parseFloat(Geb.value.replace(/,/, '.')));
			LabelGesamtRE.innerHTML = CurrencyFormatted(iRE);
			if (LastChecked.value == "EC") {
				iRE = 0.00;
				var GesamtBar = LabelGesamtEC.innerHTML.replace(/,/, '.');
				iRE = (parseFloat(GesamtBar) - parseFloat(Geb.value.replace(/,/, '.')));
				LabelGesamtBar.innerHTML = CurrencyFormatted(iRE);
			}
			if (LastChecked.value == "EC") {
				iRE = 0.00;
				var GesamtEC = LabelGesamtEC.innerHTML.replace(/,/, '.');
				iRE = (parseFloat(GesamtEC) - parseFloat(Geb.value.replace(/,/, '.')));
				LabelGesamtEC.innerHTML = CurrencyFormatted(iRE);
			}
			LastChecked.value = "RE";
		}
	}

}


function CurrencyFormatted(amount) 
{
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

