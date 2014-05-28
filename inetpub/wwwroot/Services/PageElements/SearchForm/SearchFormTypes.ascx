<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchFormTypes.ascx.vb" Inherits="CKG.Services.SearchFormTypes" %>


<script type="text/javascript" src="/Services/JScript/JQuery/jquery-1.8.0.min.js"></script>
<script type="text/javascript" src="/Services/JScript/overlib_mini.js"></script>

<script type="text/javascript" src="/Services/PageElements/SearchForm/Scripts/jquery-textbox-selection.js"></script>
<script type="text/javascript" src="/Services/PageElements/SearchForm/Scripts/TextBoxKeyPressFiltering.js"></script>


<style type="text/css">
    ul.toolTipList li {
        padding-bottom: 6px;
    }
    ul.toolTipList  {
        margin-top: 10px;
    }
</style>




<div id="Fahrgestell_ToolTip" style="display: none">
    <ul class="toolTipList">
        <li>Länge maximal <strong>[maxLen]</strong> Zeichen</li>
        <li>Alphanumerisch</li>
        <li style="color:red">Zeichen <strong>'O'</strong> wird umgewandelt in <strong>'0' (numerisch Null)</strong></li>
        <li style="color:red">Zeichen <strong>'I'</strong> wird umgewandelt in <strong>'L'</strong></li>
        <li>Fragmentsuche mit <strong>* (Sternchen)</strong> möglich</li>
    </ul>
</div>

<div id="KfzBrief_ToolTip" style="display: none">
    <ul class="toolTipList">
        <li>Länge maximal <strong>[maxLen]</strong> Zeichen</li>
        <li>Alphanumerisch</li>
        <li>Leerzeichen werden ignoriert</li>
        <li>Die ersten 2 Zeichen sind Buchstaben</li>
        <li>Ab dem 3. Zeichen Ziffern</li>
        <li>Fragmentsuche mit <strong>* (Sternchen)</strong> möglich</li>
    </ul>
</div>

<div id="Kennzeichen_ToolTip" style="display: none">
    <ul class="toolTipList">
        <li>Länge maximal <strong>[maxLen]</strong> Zeichen</li>
        <li>Alphanumerisch</li>
        <li>Leerzeichen werden ignoriert</li>
        <li>Mindestens ein Bindestrich muss eingegeben werden</li>
        <li style="color:red">Nach den ersten 3 Zeichen muss mindestens ein Bindestrich oder ein Stern erscheinen</li>
        <li>Fragmentsuche mit <strong>* (Sternchen)</strong> möglich</li>
    </ul>
</div>

<div id="ReferenzFeld_ToolTip" style="display: none">
    <ul class="toolTipList">
        <li>Länge maximal <strong>[maxLen]</strong> Zeichen</li>
        <li>Alphanumerisch</li>
        <li>Leerzeichen werden ignoriert</li>
        <li>Fragmentsuche mit <strong>* (Sternchen)</strong> möglich</li>
    </ul>
</div>


<script type="text/javascript">

    //
    //
    // customized keypress char filter handler 
    // (by M. Jenzen, 08.01.2013)
    //
    // depends on: 
    //  - TextBoxKeyPressFiltering.js
    //


    //
    // *** Fahrgestell ***   customized keypress char filter handler 
    //
    var Fahrgestell_Param = { maxLen: 17 };

    function Fahrgestell_TbCheck(val) {

        var lengthOk = (val.textLength <= Fahrgestell_Param.maxLen);
        var keyOk = (val.isAlpha || val.isDigit || val.isMinus || val.isStar);

        if (!keyOk || !lengthOk) {
            val.success = false;
            return val;
        }

        var ch = val.ch;
        if (val.isAlphaLower)
            ch = ch.toUpperCase();
        if (ch == 'O') ch = '0';
        if (ch == 'I') ch = 'L';

        val.ch = ch;
        val.success = true;
        return val;

    }

    //
    // *** KFZ-Brief ***   customized keypress char filter handler 
    //
    var KfzBrief_Param = { maxLen: 8 };

    function KfzBrief_TbCheck(val) {

        var lengthOk = (val.textLength <= KfzBrief_Param.maxLen);
        var keyOk = (val.isAlpha || val.isDigit || val.isStar);
        if (keyOk)
            keyOk = val.containsStar || (val.textLength <= 2 && val.isAlpha) || (val.textLength > 2 && val.isDigit);
        
        if (!keyOk || !lengthOk) {
            val.success = false;
            return val;
        }

        var ch = val.ch;
        if (val.isAlphaLower)
            ch = ch.toUpperCase();

        val.ch = ch;
        val.success = true;
        return val;

    }

    //
    // *** KFZ-Kennzeichen ***   customized keypress char filter handler 
    //
    var Kennzeichen_Param = { maxLen: 10 };

    function Kennzeichen_TbCheck(val) {

        var lengthOk = (val.textLength <= Kennzeichen_Param.maxLen);
        var keyOk = (val.isAlpha || val.isDigit || val.isStar);
        if (val.text.indexOf('-') == -1) {
            // nur einen einzigen Bindestrich insgesamt erlauben:
            keyOk |= val.isMinus;
        }

        if (!keyOk || !lengthOk) {
            val.success = false;
            return val;
        }

        if (!val.containsStar && !val.containsMinus)
            if (val.textLength == 4 && (!val.isMinus && !val.isStar)) {
                // bei genau 4 Zeichen erwarten wir jetzt aber als 4. Zeichen den Bindestrich oder den Stern 
                // (es sei denn, die ersten 4 Zeichen enthalten bereits einen Stern)
                val.success = false;
                return val;
            }

        var ch = val.ch;
        if (val.isAlphaLower)
            ch = ch.toUpperCase();

        val.ch = ch;
        val.success = true;
        return val;

    }

    //
    // *** Freies Referenzfeld ***   customized keypress char filter handler 
    //
    var ReferenzFeld_Param = { maxLen: 16 };

    function ReferenzFeld_TbCheck(val) {

        var lengthOk = (val.textLength <= ReferenzFeld_Param.maxLen);
        var keyOk = (val.isAlpha || val.isDigit || val.isStar || val.isSpace);

        if (!keyOk || !lengthOk) {
            val.success = false;
            return val;
        }

        val.success = true;
        return val;

    }

</script>
