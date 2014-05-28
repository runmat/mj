
function jsAppend(jsFile) {
    var jsScript = document.createElement('script');
    jsScript.type = "text/javascript";
    jsScript.src = jsFile;
    document.getElementsByTagName('head')[0].appendChild(jsScript);
}

// Matthias Jenzen, 09.01.2013
//   For IE 7.0 and lower: 
//   Append special json extension script 
//
//   depends on:
//    - IE7/json.js
//
if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
    var ieversion = new Number(RegExp.$1); // capture x.x portion and store as a number
    if (ieversion <= 7) {
        //alert(ieversion);
        jsAppend('/Services/PageElements/SearchForm/Scripts/IE7/json.js');
    }
}



//
// generic textbox string filter handler 
// (by M. Jenzen, 08.01.2013)
//
// depends on: 
//  - jquery-x.x.x.min.js
//  - jquery-textbox-selection.js
//
function TextBoxStringFilteringOneCharacter(event, filterType, handler) {

    var tb = $(event.target);
    var text = tb.val();
    var ch = event.ch;

    if (event.which != null) {
        if (event.which == 13) {
            // prevent ENTER to ensure correct textbox string filtering on "lostfocus"
            TextBoxStringFilteringAllCharacters(tb, filterType, handler);
            return true;
        }
        if (event.which <= 30 || (event.ctrlKey && event.which == 118)) // <=> STRG + V (118)
            return true;
        ch = String.fromCharCode(event.which);
    }

    var info = tb.getSelection();
    if (info != null) {
        //alrt(info.length);
        if (info.length > 0) {
            text = '';
            tb.replaceSelection('');
            tb.setCaretPos(9999);
        }
    }

    var isAlphaLower = (ch >= 'a' && ch <= 'z');
    var isAlphaUpper = (ch >= 'A' && ch <= 'Z');
    var isAlpha = isAlphaLower || isAlphaUpper;
    var isDigit = (ch >= '0' && ch <= '9');
    var isMinus = (ch == '-');
    var isStar = (ch == '*');
    var isSpace = ch == ' ';
    var containsStar = (text.indexOf('*') != -1 || isStar);
    var containsMinus = text.indexOf('-') != -1;

    var params = {
        text: text,
        textLength: text.length + 1,
        ch: ch,
        isAlphaLower: isAlphaLower,
        isAlphaUpper: isAlphaUpper,
        isAlpha: isAlpha,
        isDigit: isDigit,
        isMinus: isMinus,
        isStar: isStar,
        isSpace: isSpace,
        containsStar: containsStar,
        containsMinus: containsMinus,
        success: false
    };
    //alrt(ch);
    var paramString = JSON.stringify(params);

    // call custom char filter handler here: (M. Jenzen, 08.01.2013)
    var resultJson = eval(handler + '(' + paramString + ')');
    if (!resultJson.success)
        return false;

    ch = resultJson.ch;
    if (event.preventDefault != null)
        event.preventDefault();

    if (event.isFocusOutEvent != null) {
        text += ch;
        tb.val(text);
    }
    else {
        tb.insertAtCaretPos(ch);
    }

    return true;
}

function TextBoxStringFilteringToolTip(filterType) {
    var html = $('#' + filterType + '_ToolTip').html();
    
    var placeHolderValue = eval(filterType + '_Param.maxLen');
    html = html.replace("[maxLen]", placeHolderValue);

    return html;
}

function TextBoxStringFilteringPrepare(tb, hidden, filterType) {

    var maxLen = eval(filterType + '_Param.maxLen');
    hidden.val(maxLen);

    $.extend(tb, {
         isLocked: false,
         filterType: filterType
     });

    var handler = filterType + '_TbCheck';

    tb.keypress(function (event) {
        return TextBoxStringFilteringOneCharacter(event, filterType, handler);
    });

    tb.focusout(function () {
        if (tb.isLocked) return true;
        tb.isLocked = true;

        var tbFocusedOrg = tb;

        TextBoxStringFilteringAllCharacters(tb, filterType, handler);

        if ($.browser.msie) {
            // focus auf "altes" Input Element setzen:
            tbFocusedOrg.focus();
        }

        tb.isLocked = false;
        return true;
    });
}

function TextBoxStringFilteringAllCharacters(tb, filterType, handler) {
    var text = tb.val();
    tb.val('');

    for (var i = 0; i < text.length; i++) {
        var e = {
            target: tb.get(),
            ch: text.substr(i, 1),
            isFocusOutEvent: true
        };
        TextBoxStringFilteringOneCharacter(e, filterType, handler);
    }

    text = tb.val();
    var maxLen = eval(filterType + '_Param.maxLen');
    
    //var filterStarAutoPosition = eval(filterType + '_Param.filterStarAutoPosition');
//    var filterStarAutoPosition = 'both';

//    if (text.length < maxLen && filterStarAutoPosition != 'none') {
//        if (filterStarAutoPosition == 'left')
//            tb.val(StringPlaceStarLeft(text));
//        if (filterStarAutoPosition == 'right')
//            tb.val(StringPlaceStarRight(text));
//        if (filterStarAutoPosition == 'both') {
//            tb.val(StringPlaceStarRight(text));
//            text = tb.val();
//            tb.val(StringPlaceStarLeft(text));
//        }
//    }
}

function StringPlaceStarLeft(text) {
    if (text == null || text == '') return text;
    if (text.substr(0, 1) == '*') return text;
    text = '*' + text;
    return text;
}

function StringPlaceStarRight(text) {
    if (text == null || text == '') return text;
    if (text.substr(text.length-1, 1) == '*') return text;
    text = text + '*';
    return text;
}

function alrt(text) {
    var p = $('#output');
    p.html(p.html() + text);
}
