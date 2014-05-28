var startmenu = 'off'; 	// Grundeinstellung des Startmenus
var startbutton = 'off'; // Grundeinstellung des Startbuttons
var $resized = ''; 	// Angabe für Footer-Resize
var resizetimout = 100; 	// Timeout, bevor nach Resize der Footer positioniert wird
var openlayer = ''; 	// Aktuell geöffneter Layer in der Seite (derzeit für die Hilfe-Overlays in den Formularen)
var lastcontentblock = 1; // Aktuell darstellter Contentblock auf der Index-Seite
var lastformid = ''; 	// Aktuell geöffneter Formularlayer


// JQuery-Funktionen Ausführen

$(function () {
    $('input[type="text"]').each(function () {
        if ($(this).val() == $(this)[0].defaultValue) {
            $(this).css("color", "#7f7f7f");
        }
    });

    // GRAFISCHE FORMULARBUTTONS INITIALISIEREN
    $('.listecheckbox input, .formselects input, .globalcheckbox input').ezMark();

    $('.helpicon').hover(function () {
        $(this).attr('src', '/AutohausPortal/images/button_help_on.gif');
        $(this).parent().children('.helplayer').css("display", "block");
    }, function () {
        $(this).attr('src', '/AutohausPortal/images/button_help.gif');
        $(this).parent().children('.helplayer').css("display", "none");
    });

    // Tabellen Class Änderungen
    $('.GridTableHead th:first-child').addClass('colheaderstart');
    $('.GridTableHead th:last-child').addClass('colheaderend');

    $('.RadGrid th:first-child').addClass('colheaderstart');
    $('.RadGrid th:last-child').addClass('colheaderend');

    // KLICK AUF checkall-CHECKBOXEN
    $('.checkall').click(function () {
        $('.GridView').find('input:checkbox').attr('checked', this.childNodes[0].childNodes[0].checked);
        if (this.childNodes[0].childNodes[0].checked) {
            $('.GridView').find('input:checkbox').parent().addClass('ez-checked');
        } else {
            $('.GridView').find('input:checkbox').parent().removeClass('ez-checked');
        }

    });

    // GRAFISCHE DROPDOWNS INITIALISIEREN
    $(document).ready(function () {
        $("#create").click(function () {
            $("SELECT").selectBox();
        });
        $("#destroy").click(function () {
            $("SELECT").selectBox('destroy');
        });
        $("#enable").click(function () {
            $("SELECT").selectBox('enable');
        });
        $("#disable").click(function () {
            $("SELECT").selectBox('disable');
        });
        $("SELECT").selectBox();
    });

    // STARTMENU ÖFFNEN

    $("#startbutton").hover(
     function () {
         if (startbutton == 'off') {
             startbutton = 'on';
             startmenu = 'on';
             $("#startbutton").css("backgroundPosition", "0px -105px");
             $("#startmenu").hide().slideDown(200);
         }
     }
   );

    // STARTMENU SCHLIESSEN
    $("#startmenu").hover(
     function () {
     },
   function () {
       closestartmenu();
   }
   );

    // GLOW UM FORMFELDER

    $(".formfeld > input").focusin(
     function () {
         SetRemoveOuterGlow(this, true, false);
     }
   );

    $(".formfeld > input").focusout(
     function () {
         SetRemoveOuterGlow(this, false, false);
     }
   );

    $(".formfeld > div > input").focusin(
     function () {
         SetRemoveOuterGlow(this, true, true);
     }
   );

    $(".formfeld > div > input").focusout(
     function () {
         SetRemoveOuterGlow(this, false, true);
     }
   );

    $(".RadInput > input, .RadPicker > input").focusin(
     function () {
         SetRemoveOuterGlow(this, true, false);
     }
   );

    $(".RadInput > input, .RadPicker > input").focusout(
     function () {
         SetRemoveOuterGlow(this, false, false);
     }
   );

    $('.RadPicker td:last-child').addClass('datepicker_end');
    $('.datepicker_end td:last-child').removeClass('datepicker_end');

    // SUB-NAVIGATION AUF STARTSEITE

    $(".tnbutton").hover(
     function () { tnbuttonon(this.id); },
     function () { tnbuttonoff(this.id); }
   );


    // BUTTON-ZUSTÄNDE DES FORMÖFFNERS

    $(".formopener").hover(
     function () {

         if (lastformid) {
             var lastformlayer = 'formopener' + lastformid;
         }

         if (lastformlayer && this.id != lastformlayer) {
             $("#" + this.id).css("backgroundPosition", "0px -126px");
         }
         if (lastformlayer && this.id == lastformlayer) {
             $("#" + this.id).css("backgroundPosition", "0px -84px");
         }

         if (!lastformid) {
             $("#" + this.id).css("backgroundPosition", "0px -126px");
         }
     },
     function () {
         if (lastformid) {
             var lastformlayer = 'formopener' + lastformid;
         }
         if (lastformlayer && this.id != lastformlayer) {
             $("#" + this.id).css("backgroundPosition", "0px 0px");
         }
         if (lastformlayer && this.id == lastformlayer) {
             $("#" + this.id).css("backgroundPosition", "0px -42px");
         }
         if (!lastformid) {
             $("#" + this.id).css("backgroundPosition", "0px 0px");
         }

     }
   );

    $('.jqcalendar').datepicker();
    $('.jqcalendarWerktage').datepicker("option", { beforeShowDay: nurWerktage });
    $('.datepicker').click(function () {
        $(this).parent().parent().find('input.jqcalendar').trigger('click');
        $(this).parent().parent().find('input').datepicker("show");
    });

});

// set/remove outer glow
function SetRemoveOuterGlow(ctrl, setGlow, insideDiv) {
    if (insideDiv == true) {
        if ($(ctrl).parent().parent().hasClass('fielddisabled') == false) {
            var startDiv = $(ctrl).parent().prev('.formfeld_start');
            var endDiv = $(ctrl).parent().next('.formfeld_end,.formfeld_end_wide');
            if (setGlow == true) {
                startDiv.addClass('textbox_focus');
                $(ctrl).addClass('textbox_focus');
                endDiv.addClass('textbox_focus');
            } else {
                startDiv.removeClass('textbox_focus');
                $(ctrl).removeClass('textbox_focus');
                endDiv.removeClass('textbox_focus');
            }
        }
    } else {
        if ($(ctrl).parent().hasClass('fielddisabled') == false) {
            var startDiv = $(ctrl).prev('.formfeld_start');
            var endDiv = $(ctrl).next('.formfeld_end,.formfeld_end_wide');
            if (setGlow == true) {
                startDiv.addClass('textbox_focus');
                $(ctrl).addClass('textbox_focus');
                endDiv.addClass('textbox_focus');
            } else {
                startDiv.removeClass('textbox_focus');
                $(ctrl).removeClass('textbox_focus');
                endDiv.removeClass('textbox_focus');
            }
        }
    }
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
    if ( zahl1*zahl2 > 0 ) {
        return Math.floor( zahl1/zahl2 );
    }
    else {
        return Math.ceil ( zahl1/zahl2 );
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
    for (var i = 0; i < listeFesteFeiertage.length; i++) {
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

function nurWerktage(date) {
    var keinWochenende = $.datepicker.noWeekends(date);
    if (keinWochenende[0]) {
        return keinFeiertag(date);
    } else {
        return keinWochenende;
    }
}


// STARTMENU SCHLIESSEN

function closestartmenu() {
    if (startmenu == 'on' && startbutton == 'on') {
        startbutton = 'off';
        startmenu = 'off';
        $("#startbutton").css("backgroundPosition", "0px 0px");
        $("#startmenu").slideUp(500);
        setTimeout(function () { $("#startmenu").hide(); }, 200);
    }
}


// FORMULARLAYER ÖFFNEN UND SCHLIESSEN


function openforms(formid) {
    if ($.browser.msie == true && $.browser.version <= 7) {


    } else {
        if (lastformid && !formid) {
            var textlayerold = '#form' + lastformid;
            var mehrlayerold = '#formopener' + lastformid;
            var hinweispflichtold = '#hinweispflicht' + lastformid;
            $(textlayerold).slideUp(300);
            $(mehrlayerold).css("backgroundPosition", "0px 0px");
            $(hinweispflichtold).hide();
            lastformid = '';
        }

        if (formid) {
            if (lastformid && formid == lastformid) {
                var textlayerold = '#form' + lastformid;
                var mehrlayerold = '#formopener' + lastformid;
                var hinweispflichtold = '#hinweispflicht' + lastformid;
                $(textlayerold).slideUp(300);
                $(mehrlayerold).css("backgroundPosition", "0px 0px");
                $(hinweispflichtold).hide();
                lastformid = '';
            }
            else {
                if (lastformid) {
                    var textlayerold = '#form' + lastformid;
                    var mehrlayerold = '#formopener' + lastformid;
                    var hinweispflichtold = '#hinweispflicht' + lastformid;
                    $(textlayerold).slideUp(300);
                    $(mehrlayerold).css("backgroundPosition", "0px 0px");
                    $(hinweispflichtold).hide();
                    lastformid = '';
                }
                var textlayer = '#form' + formid;
                var mehrlayer = '#formopener' + formid;
                var hinweispflicht = '#hinweispflicht' + formid;
                $(textlayer).slideDown(300);
                $(mehrlayer).css("backgroundPosition", "0px -42px");
                $(hinweispflicht).show();
                lastformid = formid;
            }
        }
    }
    // setTimeout(function(){setfooter();},305);
}

function openform1() {
    if ($.browser.msie == true && $.browser.version <= 7) { }
    else {
            var textlayer = '#form1';
            var mehrlayer = '#formopener1';
            var hinweispflicht = '#hinweispflicht1';
            $(textlayer).show();
            $(mehrlayer).css("backgroundPosition", "0px -42px");
            $(hinweispflicht).show();
            lastformid = 1;

            var textlayer2 = '#form2';
            var mehrlayer2 = '#formopener2';
            var hinweispflicht2 = '#hinweispflicht2';
            $(textlayer2).hide();
            $(mehrlayer2).css("backgroundPosition", "0px 0px");
            $(hinweispflicht2).hide();

            var textlayer3 = '#form3';
            var mehrlayer3 = '#formopener3';
            var hinweispflicht3 = '#hinweispflicht3';
            $(textlayer3).hide();
            $(mehrlayer3).css("backgroundPosition", "0px 0px");
            $(hinweispflicht3).hide();

     }
    // setTimeout(function(){setfooter();},305);
}

// Funktionen bei Resize der Seite ausführen
function doResize() {
    // Mindestbreite für Formularbereich
    if (scalemcb) {
        setmcb();
    }
    setfooter();
}


// Footer korrekt positionieren

function setfooter() {
    var getheight = $(window).height();
    var getcontentheight = $("#maincontainer").height();

    // test = getheight + '|'+getcontentheight;

    if ((getcontentheight + 80) < getheight) {
        var footermarginplus = (getheight - (getcontentheight + 80)) + 'px';
        $("#footer").css("marginTop", footermarginplus);
        // alert(test);
    }
    else {
        $("#footer").css("marginTop", "0px");
    }
}

// Mindestbreite für Formularbereich eistellen

function setmcb() {
    var getwidth = $(window).width();
    if (getwidth < scalemcb) {
        $("#maincontainer_breit").css("width", scalemcb + "px");
        $("#footer").css("width", scalemcb + "px");
    }
    else {
        $("#footer").css("width", "100%");
    }
}


// SUB-NAVIGATION AUF STARTSEITE

// Buttons der Subnavigation einschalten

function tnbuttonon(buttonid) {
    var num = parseInt(buttonid.substring(2));

    var prevnum = (num - 1);
    var buttonleft = '#tn' + prevnum + ' .tnbuttonright';

    // Hover letzter Button
    if (num == maxtn) {
        var buttonright = '#tn' + num + ' .tnbuttonrightlast';
        var bgright = '0px -47px';
        if (prevnum == tnon) {
            var bgleft = '0px -141px';
        }
        else {
            var bgleft = '0px -47px';
        }
    }
    // rechts daneben aktiver Button
    else if (((num + 1) == tnon)) {
        var buttonright = '#tn' + num + ' .tnbuttonrightnexton';
        bgright = '0px -141px';
        var buttonleft = '#tn' + prevnum + ' .tnbuttonright';
        var bgleft = '0px -47px';
    }
    else {
        var buttonright = '#tn' + num + ' .tnbuttonright';
        var bgright = '0px -94px';
        if (((num - 1) == tnon)) {
            var bgleft = '0px -141px';
        }
        else {
            var bgleft = '0px -47px';
        }
    }

    var button = '#tn' + num + ' .tnbuttoninner';
    var bgbutton = '0px -47px';
    $(button).css("backgroundPosition", bgbutton);
    $(buttonright).css("backgroundPosition", bgright);

    // ERSTER BUTTON:
    if (num == 1) {
        var buttonleft = '.tnbuttonleft';
        var bgleft = '0px -47px';
    }
    $(buttonleft).css("backgroundPosition", bgleft);
}

// Buttons der Subnavigation ausschalten

function tnbuttonoff(buttonid) {
    var num = parseInt(buttonid.substring(2));

    var prevnum = (num - 1);
    var buttonleft = '#tn' + prevnum + ' .tnbuttonright';

    // Hover letzter Button
    if (num == maxtn) {
        var buttonright = '#tn' + num + ' .tnbuttonrightlast';
        var bgright = '0px 0px';
        if (prevnum == tnon) {
            var bgleft = '0px -94px';
        }
        else {
            var bgleft = '0px 0px';
        }
    }
    // rechts daneben aktiver Button
    else if (((num + 1) == tnon)) {
        var buttonright = '#tn' + num + ' .tnbuttonrightnexton';
        var bgright = '0px -47px';
        var buttonleft = '#tn' + prevnum + ' .tnbuttonright';
        var bgleft = '0px 0px';
    }
    else {
        var buttonright = '#tn' + num + ' .tnbuttonright';
        var bgright = '0px 0px';
        if (((num - 1) == tnon)) {
            var bgleft = '0px -94px';
        }
        else {
            var bgleft = '0px 0px';
        }
    }

    var button = '#tn' + num + ' .tnbuttoninner';
    var bgbutton = '0px 0px';

    $(button).css("backgroundPosition", bgbutton);
    $(buttonright).css("backgroundPosition", bgright);

    // ERSTER BUTTON:
    if (num == 1) {
        var buttonleft = '.tnbuttonleft';
        var bgleft = '0px 0px';
    }
    $(buttonleft).css("backgroundPosition", bgleft);
}


// die 4 oberen Teaser auf der Starttseite nacheinander einblenden

function showstartteaser() {
    setTimeout(function () { $("#teaser1").fadeIn(300); }, 100);
    setTimeout(function () { $("#teaser2").fadeIn(300); }, 400);
    setTimeout(function () { $("#teaser3").fadeIn(300); }, 700);
    setTimeout(function () { $("#teaser4").fadeIn(300); }, 1000);
}


// Datepicker darstellen

$.datepicker.regional['de'] = { clearText: 'löschen', clearStatus: 'aktuelles Datum löschen',
    closeText: 'schließen', closeStatus: 'ohne Änderungen schließen',
    prevText: '<zurück', prevStatus: 'letzten Monat zeigen',
    nextText: 'Vor>', nextStatus: 'nächsten Monat zeigen',
    currentText: 'heute', currentStatus: '',
    monthNames: ['Januar', 'Februar', 'März', 'April', 'Mai', 'Juni',
                'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'],
    monthNamesShort: ['Jan', 'Feb', 'Mär', 'Apr', 'Mai', 'Jun',
                'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dez'],
    monthStatus: 'anderen Monat anzeigen', yearStatus: 'anderes Jahr anzeigen',
    weekHeader: 'Wo', weekStatus: 'Woche des Monats',
    dayNames: ['Sonntag', 'Montag', 'Dienstag', 'Mittwoch', 'Donnerstag', 'Freitag', 'Samstag'],
    dayNamesShort: ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'],
    dayNamesMin: ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'],
    dayStatus: 'Setze DD als ersten Wochentag', dateStatus: 'Wähle D, M d',
    dateFormat: 'dd.mm.yy', firstDay: 1,
    initStatus: 'Wähle ein Datum', isRTL: false
};
$.datepicker.setDefaults($.datepicker.regional['de']);

function showdatepicker(formname) {
    $('#kalender').show();
    $("#kalender").datepicker(

    {
        showOn: 'both',
        closeText: "X",


        onSelect: function (dateText, inst) {
            $('[name=' + formname + ']').val(dateText);
            $('#kalender').hide();
        }
    }
    );
}


// Layer darstellen, bzw. abschalten - verwendet bei den Hilfebuttons

function opencloselayer(layer) {
    if (openlayer || openlayer == layer) {
        $("#" + openlayer).css("display", "none");
        openlayer = "";
        if (openlayer != layer) {
            $("#" + layer).css("display", "block");
            openlayer = layer;
        }
    }
    else if (layer) {
        if (openlayer) {
            $("#" + openlayer).css("display", "none");
        }
        $("#" + layer).css("display", "block");
        openlayer = layer;
    }
}


// Contenblöcke auf Startseite mittels Subnavigation austauschen

function changemaincontent(layernumber) {
    var url = '../html/subnavi' + layernumber + '.html';
    $.ajax({ type: "GET",
        url: url,
        dataType: "html",
        success: function (html) {
            $('.subnavi').empty().append(html);
        }
    });

    $("#contentblock" + lastcontentblock).fadeOut(500);
    $("#contentblock" + layernumber).fadeIn(500);

    lastcontentblock = layernumber;
}

function disableDefaultValue(id) {
    var txtBox = document.getElementById(id);
    if (txtBox != null) 
    {
        txtBox.value = txtBox.defaultValue;
        txtBox.defaultValue = '';
    }
    //$(txtBox).css("color", "#000");
}
function enableDefaultValue(id) {
    var txtBox = document.getElementById(id);
    if (txtBox != null) { txtBox.defaultValue = txtBox.value; }
}


// Da beim Ajax-Postback die controls nicht noch einmal graphisch Initilisiert werden ist 
// das ein workaround der nach ClientEvents-OnResponseEnd des RadAjaxManagers aufgerufen wird

function initiate2() {
    $('input[type="text"]').each(function () {
        if ($(this).val() == $(this)[0].defaultValue) {
            $(this).css("color", "#7f7f7f");
        }

    });

     $(".formfeld > input").focusin(
         function () {
             SetRemoveOuterGlow(this, true, false);
         }
       );
     $(".formfeld > input").focusout(
         function () {
             SetRemoveOuterGlow(this, false, false);
         }
       );
     $(".formfeld > div > input").focusin(
         function () {
             SetRemoveOuterGlow(this, true, true);
         }
       );
     $(".formfeld > div > input").focusout(
         function () {
             SetRemoveOuterGlow(this, false, true);
         }
       );

     // GRAFISCHE FORMULARBUTTONS INITIALISIEREN
     // dynFields sind dynamisch erstellte Eingabefelder (z.B. der Repeater in Kennzeichenbestellung.aspx)
     //if (this.__EVENTTARGET != 'ctl00$ContentPlaceHolder1$ddlKunnr1') {
     //  $('.dynFields > div > .listecheckbox input,.dynFields > div > .formselects input, .dynFields > div > .globalcheckbox input').ezMark();
     //}
     $('.dynFields .listecheckbox input, .dynFields .formselects input, .dynFields .globalcheckbox input').ezMark();

     // GRAFISCHE DROPDOWNS INITIALISIEREN
     $(document).ready(function () {
         $("#create").click(function () {
             $("SELECT").selectBox();
         });
         $("#destroy").click(function () {
             $("SELECT").selectBox('destroy');
         });
         $("#enable").click(function () {
             $("SELECT").selectBox('enable');
         });
         $("#disable").click(function () {
             $("SELECT").selectBox('disable');
         });
         $("SELECT").selectBox();
     });
}

// Für optimale Darstellung des Footers Timeout

//var resizeTimer = null;
//$(window).bind('resize', function() {
//if (resizeTimer) clearTimeout(resizeTimer);
//resizeTimer = setTimeout(doResize, resizetimout);
//});
function initiate3() {
    // GRAFISCHE FORMULARBUTTONS INITIALISIEREN
    $('.listecheckbox input, .formselects input, .globalcheckbox input').ezMark();


    // Tabellen Class Änderungen
    $('.GridTableHead th:first-child').addClass('colheaderstart');
    $('.GridTableHead th:last-child').addClass('colheaderend');


    // KLICK AUF checkall-CHECKBOXEN
    $('.checkall').click(function () {
        $('.GridView').find('input:checkbox').attr('checked', this.childNodes[0].childNodes[0].checked);
        if (this.childNodes[0].childNodes[0].checked) {
            $('.GridView').find('input:checkbox').parent().addClass('ez-checked');
        } else {
            $('.GridView').find('input:checkbox').parent().removeClass('ez-checked');
        }

    });
}

function initiate4() {
    // Tabellen Class Änderungen
    $('.GridTableHead th:first-child').addClass('colheaderstart');
    $('.GridTableHead th:last-child').addClass('colheaderend');

    $('.RadGrid th:first-child').addClass('colheaderstart');
    $('.RadGrid th:last-child').addClass('colheaderend');
}
function SetEinkennzeichen(ddlFahrzeugart, chkEinKz) {
    var myindex = ddlFahrzeugart.selectedIndex;
    var SelValue = ddlFahrzeugart.options[myindex].value;
    if (SelValue == 3 || SelValue == 5) {
        $('#' + chkEinKz.id).attr('checked');
        $('#' + chkEinKz.id).parent().addClass('ez-checked');
        chkEinKz.checked = true;
    }
    else { chkEinKz.checked = false; $('#' + chkEinKz.id).parent().removeClass('ez-checked') ; }
    return true;
}

function setZahlartEinzug() {
    $('.formselects input[type=radio][name$=Zahlungsart][value=rbEinzug]').attr('checked', true);
    // Radiobutton-Rendering per ezmark-Funktion antriggern
    $('.formselects[id$=divZahlungsart] .ez-radio').removeClass('ez-radio');
    $('.formselects[id$=divZahlungsart] input').ezMark();
}

function resetZahlart() {
    $('.formselects input[type=radio][name$=Zahlungsart][value=rbEinzug]').attr('checked', false);
    $('.formselects input[type=radio][name$=Zahlungsart][value=rbRechnung]').attr('checked', false);
    $('.formselects input[type=radio][name$=Zahlungsart][value=rbBar]').attr('checked', false);
    // Radiobutton-Rendering per ezmark-Funktion antriggern
    $('.formselects[id$=divZahlungsart] .ez-radio').removeClass('ez-radio');
    $('.formselects[id$=divZahlungsart] input').ezMark();
}

function LogPageVisit(appId, href) {
    // vorerst kein Logging bei IE8 oder geringer, da dort Probleme auftreten
    if (IsIEVersionOrLower(8)) {
        return true;
    }

    var url = '/AutohausPortal/Log.aspx?APP-ID=' + appId;
    $.get(url).always(function () {
        window.location.href = href;
    });

    return true;
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