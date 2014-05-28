var startmenu = 'off'; 	// Grundeinstellung des Startmenus
var startbutton = 'off'; // Grundeinstellung des Startbuttons
var $resized = ''; 	// Angabe für Footer-Resize
var resizetimout = 100; 	// Timeout, bevor nach Resize der Footer positioniert wird
var openlayer = ''; 	// Aktuell geöffneter Layer in der Seite (derzeit für die Hilfe-Overlays in den Formularen)
var lastcontentblock = 1; // Aktuell darstellter Contentblock auf der Index-Seite
var lastformid = ''; 	// Aktuell geöffneter Formularlayer
var currentOpenFormId = -1;



// JQuery-Funktionen Ausführen


// changed, M. Jenzen, 07.01.2013
$(function () {
    FormInitStyles();

    // changed, M. Jenzen, 07.01.2013
    // Wichtig: "ezMark" sollte nur beim erstmaligen Load einer Seite ausgeführt werdenb und nicht bei jedem Ajax-Refresh:
    //ezMarkInit();
});

// changed, M. Jenzen, 07.01.2013
function ezMarkInit() {
    // GRAFISCHE FORMULARBUTTONS INITIALISIEREN
    // Wichtig: "ezMark" sollte nur beim erstmaligen Load einer Seite ausgeführt werden und nicht bei jedem Ajax-Refresh:
    $('.listecheckbox input, .formselects input, .globalcheckbox input').ezMark();
}


// added, M. Jenzen, 03.01.2013
function FormInitValidationErrorStyles() {
    $('.input-validation-error.formtext').each(function () {
        // normale Textboxen
        var startDiv = $(this).prev('.formfeld_start,.formfeld_start_wide');
        var endDiv = $(this).next('.formfeld_end,.formfeld_end_wide');
        FormTextboxAddClass($(this), startDiv, 'input-validation-error2', 'start');
        FormTextboxAddClass($(this), endDiv, 'input-validation-error2', 'end');
    });
    $('.input-validation-error.t-input').each(function () {
        // Telerik Textboxen (DatePicker, etc)
        var startDiv = $(this).parent().parent().prev('.formfeld_start');
        var endDiv = $(this).parent().parent().next('.formfeld_end');
        FormTextboxAddClass($(this), startDiv, 'input-validation-error2', 'start');
        FormTextboxAddClass($(this), endDiv, 'input-validation-error2', 'end');
    });
    $('.input-validation-error.t-header').each(function () {
        // Telerik Drowdown 
        FormDropdownSetClass($(this), 'input-validation-error2');
    });
    $('.input-validation-error,.ez-hide').each(function () {
        // stylisch Radiobuttons and Checkboxen
        var cssClass = 'input-validation-error-ez-radio';
        var innerWrapper = $(this).parent();
        innerWrapper.find('label').eq(0).addClass(cssClass);
        var wrapper = innerWrapper.parent();
        //wrapper.find('label').eq(0).addClass(cssClass);
        //alert('');
    });
}

// added, M. Jenzen, 03.01.2013
function FormTextboxAddClass(tb, div, cssClass, sWhat) {
    var dstClass = cssClass + ' formfeld_' + sWhat;

    div.addClass(dstClass);
    dstClass = cssClass + ' formtext';
    tb.addClass(dstClass);
}

// added, M. Jenzen, 03.01.2013
function FormTextboxRemoveClass(tb, endDiv, cssClass) {
    endDiv.removeClass(cssClass);
    tb.removeClass(cssClass);
}

// added, M. Jenzen, 03.01.2013
function FormTelerikTextboxSetClass(tb, endDiv, cssClass) {
    var errorClass = cssClass + ' formfeld_end_wide';

    endDiv.addClass(errorClass);
    errorClass = cssClass + ' formtext';
    tb.addClass(errorClass);
}

function FormDropdownSetClass(dropDown, cssClass) {
    var outerDiv = dropDown.parent();
    cssClass = outerDiv.attr('class').replace('formfeld-div', cssClass);
    //alert(cssClass);
    outerDiv.toggleClass(cssClass);
}


function FormInitStyles() {

    $('.t-autocomplete').removeClass('t-widget');

    // added, M. Jenzen, 03.01.2013
    FormInitValidationErrorStyles();

    // removed, M. Jenzen, 03.01.2013
    //    $('input[type="text"]').each(function () {
    //        if ($(this).val() == $(this)[0].defaultValue) {
    //            if ($(this).val() != '') 
    //                $(this).css("color", "#7f7f7f");
    //        }
    //    });

    ezMarkInit();

    dragDropInit();

    // Tooltip Migration
    $('.autoTooltip').each(function () {
        if ($(this).find('.InnerTooltip').html() == null) {
            $(this).append('<div class="toolTipWrapper"><div class="InnerTooltip"></div></div>');
            $(this).find('.InnerTooltip').html('<p>' + $(this).attr('title') + '</p>');
            $(this).hover(function () {
                $(this).children('.toolTipWrapper').fadeIn(300);
            }, function () {
                $(this).children('.toolTipWrapper').hide();
            });

        }
    });
    
    $('.helpicon').hover(function () {
        $(this).attr('src', '../images/button_help_on.gif');
        $(this).parent().children('.helplayer').css("display", "block");
    }, function () {
        $(this).attr('src', '../images/button_help.gif');
        $(this).parent().children('.helplayer').css("display", "none");
    });

    $('.datenbutton_small').hover(function () {
        $(this).children('.helplayer').fadeIn(300);
    }, function () {
        $(this).children('.helplayer').hide();
    });

    $('.searchbutton').hover(function () {
        $(this).children('.helplayer').fadeIn(300);
    }, function () {
        $(this).children('.helplayer').hide();
    });

    $('.recyclebutton').hover(function () {
        $(this).children('.helplayer').fadeIn(300);
    }, function () {
        $(this).children('.helplayer').hide();
    });

    $('.removebutton').hover(function () {
        $(this).children('.helplayer').fadeIn(300);
    }, function () {
        $(this).children('.helplayer').hide();
    });

    $('.routebutton').hover(function () {
        $(this).children('.helplayer').fadeIn(300);
    }, function () {
        $(this).children('.helplayer').hide();
    });

    $('.geolocationbutton').hover(function () {
        $(this).children('.helplayer').fadeIn(300);
    }, function () {
        $(this).children('.helplayer').hide();
    });

    $('.drophelpbutton').hover(function () {
        $(this).children('.helplayer').fadeIn(300);
    }, function () {
        $(this).children('.helplayer').hide();
    });

    // STARTMENU ÖFFNEN

    $("#startbutton").hover(
        function() {
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



    // changed, M. Jenzen, 03.01.2013
        $(".formfeld > input").focusin(
        function () {
            if ($(this).parent().hasClass('fielddisabled') == false) {
                var startDiv = $(this).prev('.formfeld_start,.formfeld_start_wide');
                var endDiv = $(this).next('.formfeld_end,.formfeld_end_wide');
                if (!$(this).hasClass('input-validation-error')) {
                    FormTextboxAddClass($(this), startDiv, 'textbox_focus', 'start');
                    FormTextboxAddClass($(this), endDiv, 'textbox_focus', 'end');
                }
            }
        }
    );
        $(".t-input").focusin(
        function () {
            if ($(this).parent().hasClass('fielddisabled') == false) {
                var startDiv = $(this).parent().parent().prev('.formfeld_start');
                var endDiv = $(this).parent().parent().next('.formfeld_end');
                if (!$(this).hasClass('input-validation-error')) {
                    FormTextboxAddClass($(this), startDiv, 'textbox_focus', 'start');
                    FormTextboxAddClass($(this), endDiv, 'textbox_focus', 'end');
                }
            }
        }
    );

    // changed, M. Jenzen, 03.01.2013
    $(".formfeld > input").focusout(
    function () {
            if ($(this).parent().hasClass('fielddisabled') == false) {
                var startDiv = $(this).prev('.formfeld_start,.formfeld_start_wide');
                var endDiv = $(this).next('.formfeld_end,.formfeld_end_wide');
                FormTextboxRemoveClass($(this), endDiv, 'textbox_focus');
                FormTextboxRemoveClass($(this), startDiv, 'textbox_focus');
            }
        }
    );
    $(".t-input").focusout(
    function () {
            if ($(this).parent().hasClass('fielddisabled') == false) {
                var startDiv = $(this).parent().parent().prev('.formfeld_start');
                var endDiv = $(this).parent().parent().next('.formfeld_end');
                FormTextboxRemoveClass($(this), endDiv, 'textbox_focus');
                FormTextboxRemoveClass($(this), startDiv, 'textbox_focus');
            }
        }
    );
    

    $('.datepicker_end td:last-child').removeClass('datepicker_end');


    $('.jqcalendar').datepicker({
        beforeShowDay: datepickerDisableSpecificWeekDays
    });
    $('.datepicker').click(function () {
        $(this).parent().parent().find('input.jqcalendar').trigger('click');
        $(this).parent().parent().find('input').datepicker("show");
    });

    // 0 = sunday, 1 = monday, 2 = tuesday, 3 = wednesday, 4 = thursday, 5 = friday, 6 = saturday
    var daysToDisable = [0];

    function datepickerDisableSpecificWeekDays(date) {
        var day = date.getDay();
        for (var i = 0; i < daysToDisable.length; i++) {
            if ($.inArray(day, daysToDisable) != -1) {
                return [false];
            }
        }
        return [true];
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


function OpenFormsEvent() {
    this.eventHandlers = new Array();
}

OpenFormsEvent.prototype.addHandler = function (eventHandler) {
    this.eventHandlers.push(eventHandler);
};
OpenFormsEvent.prototype.execute = function (formId, onSuccessFunction) {
    for (var i = 0; i < this.eventHandlers.length; i++) {
        if (!this.eventHandlers[i](formId, onSuccessFunction))
            return false;
    }
    return true;
};

var openFormsEvent = new OpenFormsEvent();


function initOpenforms() {
    lastformid = '';
    openforms(1);
}

function tryOpenforms(formid) {
    if (lastformid) {
        openFormsEvent.execute(lastformid, 'openforms(' + formid + ')');
    }
    else
        openforms(formid);
}

function initOpenformsRaw(formid) {
    var textlayer = '#form' + formid;
    var mehrlayer = '#formopener' + formid;
    var hinweispflicht = '#hinweispflicht' + formid;

    $(textlayer).slideDown(300);
    //ActivateIds(formid);

    $(mehrlayer).css("backgroundPosition", "0px 0px");
    $(hinweispflicht).show();
}

function openforms(formid) {

    currentOpenFormId = -1;
    
    if ($.browser.msie == true && $.browser.version <= 1) {


    } else {
        if (lastformid && !formid) {
            var textlayerold = '#form' + lastformid;
            var mehrlayerold = '#formopener' + lastformid;
            var hinweispflichtold = '#hinweispflicht' + lastformid;

            $(textlayerold).slideUp(300);

            $(mehrlayerold).css("backgroundPosition", "0px -34px");
            $(hinweispflichtold).hide();

            FormInitValidatedFormStylesOneForm(lastformid);
            lastformid = '';
        }

        if (formid) {
            if (lastformid && formid == lastformid) {
                var textlayerold = '#form' + lastformid;
                var mehrlayerold = '#formopener' + lastformid;
                var hinweispflichtold = '#hinweispflicht' + lastformid;
                
                $(textlayerold).slideUp(300);

                $(mehrlayerold).css("backgroundPosition", "0px -34px");
                $(hinweispflichtold).hide();

                FormInitValidatedFormStylesOneForm(lastformid);
                lastformid = '';
            }
            else {
                if (lastformid) {
                    var textlayerold = '#form' + lastformid;
                    var mehrlayerold = '#formopener' + lastformid;
                    var hinweispflichtold = '#hinweispflicht' + lastformid;

                    $(textlayerold).slideUp(300);

                    $(mehrlayerold).css("backgroundPosition", "0px -34px");
                    $(hinweispflichtold).hide();

                    lastformid = '';
                }
                var textlayer = '#form' + formid;
                var mehrlayer = '#formopener' + formid;
                var hinweispflicht = '#hinweispflicht' + formid;

                $(textlayer).slideDown(300);
                ActivateIds(formid);
                
                $(mehrlayer).css("backgroundPosition", "0px 0px");
                $(hinweispflicht).show();

                lastformid = formid;
                currentOpenFormId = formid;
            }
        }
    }
    // setTimeout(function(){setfooter();},305);
}

function ChangeIdsForForm(formId, activateIds) {
    var hidePostfix = "_XhiddenX";
    var form = $('#ajaxFormContainer' + formId);

    if (!form.attr('id'))
        return false;

    form.find("*[id]").each(function () {

        var id = $(this).attr("id");
        if (id == "IsEmpty") {
            $(this).attr("id", id + formId);
            return;
        }

        if (activateIds) {
            $(this).attr("id", $(this).attr("id").replace(hidePostfix, ""));
            if ($(this).attr("name"))
                $(this).attr("name", $(this).attr("name").replace(hidePostfix, ""));
        }
        else {
            if ($(this).attr("id").indexOf(hidePostfix) <= 0) {
                $(this).attr("id", $(this).attr("id") + hidePostfix);
                if ($(this).attr("name"))
                    $(this).attr("name", $(this).attr("name") + hidePostfix);
            }
        }
    });

    return true;
}

function Xopenform1() {
    if ($.browser.msie == true && $.browser.version <= 1) { }
    else {
            var textlayer = '#form1';
            var mehrlayer = '#formopener1';
            var hinweispflicht = '#hinweispflicht1';
            $(textlayer).show();
            $(mehrlayer).css("backgroundPosition", "0px -34px");
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


var hiddenDropItemsLeftIds, hiddenDropItemsRightIds;
function dragDropValuesServerToClient(leftIds, rightIds) {

    hiddenDropItemsLeftIds = leftIds;
    hiddenDropItemsRightIds = rightIds;

    var leftItems = $('#' + leftIds).val();
    if (leftItems != null) {
        dragDropValuesServerToClient_FillDragBox(leftItems.split(','), 1);
    }

    var rightItems = $('#' + rightIds).val();
    if (rightItems != null)
        dragDropValuesServerToClient_FillDragBox(rightItems.split(','), 2);
    
    check_dropped();
}

function dragDropValuesServerToClient_FillDragBox(itemsSplit, dragBoxId) {

    $.each(itemsSplit, function (index, value) {
        var moveId = $('#dl_' + value);
        var divToMove = $(moveId);
        if (divToMove.attr('id') != 'undefined') {
            var movedDiv = divToMove.appendTo('#dropzone' + dragBoxId + ' .droparea');
            if (movedDiv.attr('id') != null)
                movedDiv.attr('id', movedDiv.attr('id').replace('dl_', 'dl' + dragBoxId));
        }
    });
}

function dragDropValuesClientToServer() {
    dragDropValuesClientToServer_GetDragBoxItems(1, hiddenDropItemsLeftIds);
    dragDropValuesClientToServer_GetDragBoxItems(2, hiddenDropItemsRightIds);
}

function dragDropValuesClientToServer_GetDragBoxItems(dropBoxId, hiddenDropItemsIds) {
    var droppedItems = '';
    $('#dropzone' + dropBoxId).find('.dropitem').each(function () {
        if ($(this).hasClass('ui-draggable-dragging')) {

        } else {
            //var itemhtml = $(this).find('.text').html();
            var itemId = $(this).attr('id').substr(3);
            droppedItems = droppedItems + itemId + ',';
        }
    });
    if (droppedItems != '') {
        droppedItems = droppedItems.substr(0, droppedItems.length - 1);
    }

    $('#' + hiddenDropItemsIds).val(droppedItems);
}

function dragDropInit() {
    //
    // Drag and Drop 
    //

    var dId = 0;

    $('.dropitem').each(function () {
        if ($(this).parent().attr('class') != 'dropHidden') {
            $(this).attr('id', 'dItem-' + dId);
            dId++;
        }
    });

    if (typeof ($('#droppedItems').val()) != 'undefined' && $('#droppedItems').val() != '') {
        var items = $('#droppedItems').val();
        var itemsSplit = items.split(',');
        $.each(itemsSplit, function (index, value) {
            var moveId = $('#dropzone2').children('.droparea').find('div:contains(\'' + value + '\')').attr('id');
            $('#' + moveId).appendTo('#dropzone1 .droparea');
        });
    }

    $('.dropitem').draggable({
        start: function (event, ui) {
            var cssClass = $(event.target).attr('class');
            if (cssClass == 'deleteItem' || cssClass == 'takeItem')
                return false;
            return true;
        },
        revert: 'invalid', // when not dropped, the item will revert back to its initial position
        containment: 'document',
        helper: 'clone',
        cursor: 'move'
    });

    $('#dropzone1').children('.droparea').droppable({
        accept: '.dropitem',
        activeClass: 'activeDrop',
        drop: function (event, ui) {
            var $item = ui.draggable;

            if ($item.hasClass('input-validation-error')) {
                $item.removeClass('input-validation-error');
                FormHideValidationError(1);
            }

            $item
                .prependTo($('#dropzone1').children('.droparea'))
                .fadeIn();
            check_dropped();
        }
    });

    $('#dropzone2').children('.droparea').droppable({
        accept: '.dropitem',
        activeClass: 'activeDrop',
        drop: function (event, ui) {
            var $item = ui.draggable;
            $item
                .prependTo($('#dropzone2').children('.droparea'))
                .fadeIn();
            check_dropped();
        }
    });

    $('.dropitem').click(function (event) {
        var $target = $(event.target);
        if ($target.is('.takeItem')) {
            $(this).prependTo($('#dropzone1').children('.droparea'));
            check_dropped();
        } else if ($target.is('.deleteItem')) {
            $(this).prependTo($('#dropzone2').children('.droparea'));
            check_dropped();
        }
        return false;
    });
}

function check_dropped() {
    //
    // Check for dropped Items
    //

    dragDropValuesClientToServer();

    var droppedItems = '';
    var dPreis = 0;
    $('#dropzone1').find('.dropitem').each(function () {
        if ($(this).hasClass('ui-draggable-dragging')) {

        } else {
            var itemhtml = $(this).find('.text').html();
            var itempreis = parseInt($(this).find('.hiddenDropItemPreis').val());
            droppedItems = droppedItems + itemhtml + ',';
            dPreis = dPreis + itempreis;
        }
    });
    if (droppedItems != '') {
        droppedItems = droppedItems.substr(0, droppedItems.length - 1);
    }


    if (dPreis == 0) {
        $('.dropItemsSumme').animate({ 'opacity': 0 }, 1000);
    } else {
        //$('.showPreis').removeClass('hidePreis');
        $('.dropItemsSumme').animate({ 'opacity': 1 }, 1000);
    }


    var s = formatCurrency("&euro;", dPreis);
    $('#dropItemsSummePreis').html(s);
}

function formatCurrency(sSymbol, vValue) {
    var aDigits = vValue.toFixed(2).split(".");
    aDigits[0] = aDigits[0].split("").reverse().join("").replace(/(\d{3})(?=\d)/g, "$1.").split("").reverse().join("");
    return aDigits.join(",") + " " + sSymbol;
}

