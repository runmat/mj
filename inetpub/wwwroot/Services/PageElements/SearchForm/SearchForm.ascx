<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchForm.ascx.vb" Inherits="CKG.Services.SearchForm" %>
<%@ Register TagPrefix="uc" TagName="SearchFormTypes" Src="~/PageElements/SearchForm/SearchFormTypes.ascx" %>


<uc:SearchFormTypes runat="server" />

<table runat="server" id="tab1" cellpadding="0" cellspacing="0">
    <tbody>
            
        <tr class="formquery" id="tr_AmtlKennzeichen" runat="server">
            <td nowrap="nowrap" class="firstLeft active">
                <asp:Label ID="lbl_AmtlKennzeichenLabel" runat="server" Width="130px" />
            </td>
            <td class="active" width="100%">
                <input type="hidden" ID="AmtlKennzeichenHidden" runat="server" /> 
                <asp:TextBox ID="txtAmtlKennzeichen" runat="server" CssClass="TextBoxNormal" />
                <asp:Image ID="imgAmtlKennzeichen" runat="server" ImageUrl="/Services/images/fragezeichen_normal.png" border="0" alt=""  />
            </td>
        </tr>
            
        <tr class="formquery" id="tr_Fahrgestellnummer" runat="server">
            <td nowrap="nowrap" class="firstLeft active">
                <asp:Label ID="lbl_FahrgestellnummerLabel" runat="server" Width="130px" />
            </td>
            <td class="active" width="100%">
                <input type="hidden" ID="FahrgestellnummerHidden" runat="server" /> 
                <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" />
                <asp:Image ID="imgFahrgestellnummer" runat="server" ImageUrl="/Services/images/fragezeichen_normal.png" border="0" alt=""  />
            </td>
        </tr>
            
        <tr class="formquery" id="tr_Brief" runat="server">
            <td nowrap="nowrap" class="firstLeft active">
                <asp:Label ID="lbl_BriefLabel" runat="server" Width="130px" />
            </td>
            <td class="active" width="100%">
                <input type="hidden" ID="BriefHidden" runat="server" /> 
                <asp:TextBox ID="txtBrief" runat="server" CssClass="TextBoxNormal" />
                <asp:Image ID="imgBrief" runat="server" ImageUrl="/Services/images/fragezeichen_normal.png" border="0" alt=""  />
            </td>
        </tr>
            
        <tr class="formquery" id="tr_Referenz1" runat="server">
            <td nowrap="nowrap" class="firstLeft active">
                <asp:Label ID="lbl_Referenz1Label" runat="server" Width="130px" />
            </td>
            <td class="active" width="100%">
                <input type="hidden" ID="Referenz1Hidden" runat="server" /> 
                <asp:TextBox ID="txtReferenz1" runat="server" CssClass="TextBoxNormal" />
                <asp:Image ID="imgReferenz1" runat="server" ImageUrl="/Services/images/fragezeichen_normal.png" border="0" alt=""  />
            </td>
        </tr>
            
        <tr class="formquery" id="tr_Referenz2" runat="server">
            <td nowrap="nowrap" class="firstLeft active">
                <asp:Label ID="lbl_Referenz2Label" runat="server" Width="130px" />
            </td>
            <td class="active" width="100%">
                <input type="hidden" ID="Referenz2Hidden" runat="server" /> 
                <asp:TextBox ID="txtReferenz2" runat="server" CssClass="TextBoxNormal" />
                <asp:Image ID="imgReferenz2" runat="server" ImageUrl="/Services/images/fragezeichen_normal.png" border="0" alt=""  />
            </td>
        </tr>

        <tr class="formquery"><td colspan="2">&nbsp;</td></tr>
        <tr class="formquery" colspan="2"><td style="background-color: #dfdfdf;" colspan="2" width="100%">&nbsp;</td></tr>
    </tbody>
</table>

 
<script type="text/javascript">

    $(function () {
        TextBoxStringFilteringInitFor('AmtlKennzeichen', 'Kennzeichen');
        TextBoxStringFilteringInitFor('Fahrgestellnummer', 'Fahrgestell');
        TextBoxStringFilteringInitFor('Brief', 'KfzBrief');
        TextBoxStringFilteringInitFor('Referenz1', 'ReferenzFeld');
        TextBoxStringFilteringInitFor('Referenz2', 'ReferenzFeld');
    });

    function TextBoxStringFilteringInitFor(controlName, filterType) {
        var tb = $('input[id$="' + controlName + '"]');
        var hidden = $('input[id$="' + controlName + 'Hidden"]');

        TextBoxStringFilteringPrepare(tb, hidden, filterType);

        var img = $('img[id$="' + controlName + '"]');
        var lbl = $('span[id$="' + controlName + 'Label"]');

        img.mouseover(function (e) {
            img.attr('src', "/Services/images/fragezeichen_hover.png");
            return overlib(TextBoxStringFilteringToolTip(filterType), CAPTION, 'Hinweis zu Suchfeld &nbsp; \'' + lbl.html() + '\'', FGCOLOR, '#F6F6F6', BGCOLOR, '#D6D6D6', TEXTCOLOR, '#000000', CAPCOLOR, '#444444', WIDTH, 400);
        });
        img.mouseout(function(e) {
            img.attr('src', "/Services/images/fragezeichen_normal.png");
            return nd();
        });
    }

</script>