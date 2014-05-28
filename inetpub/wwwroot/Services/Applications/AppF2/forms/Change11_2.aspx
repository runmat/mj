<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change11_2.aspx.vb" Inherits="AppF2.Change11_2"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .style1
        {
            color: #CC0000;
        }
        .style2
        {
            height: 25px;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label" />
                        </h1>
                    </div>
                    <uc1:Kopfdaten ID="kopfdaten" runat="server" />
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <%--<AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="rb_VersandStandard">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="views" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rb_VersandExpress">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="views" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rb_Zweigstellen">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="views" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rb_Zulassungsstellen">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="views" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rb_Manuell">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="views" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="chk_Abmeldung">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="views" />
                                    <telerik:AjaxUpdatedControl ControlID="cmdSave" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="cmdSave">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="views" />
                                    <telerik:AjaxUpdatedControl ControlID="cmdSave" />
                                    <telerik:AjaxUpdatedControl ControlID="cmdDownload" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>--%>
                    </telerik:RadAjaxManager>
                    <asp:MultiView ActiveViewIndex="0" runat="server" ID="views">
                        <asp:View runat="server" ID="versandartView">
                            <table id="Table2" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td class="StandardTableAlternate">
                                        Zustellart:
                                    </td>
                                    <td class="StandardTableAlternate" nowrap>
                                        <asp:RadioButton ID="rb_VersandStandard" runat="server" Text="rb_VersandStandard"
                                            Checked="True" GroupName="Versandart" AutoPostBack="True"></asp:RadioButton>
                                    </td>
                                    <td nowrap class="StandardTableAlternate">
                                        &nbsp;
                                    </td>
                                    <td nowrap>
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" nowrap>
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="StandardTableAlternate">
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" nowrap colspan="5">
                                        keine Sendungsverfolgung möglich, keine Laufzeitgarantie
                                    </td>
                                </tr>
                                <tr>
                                    <td class="StandardTableAlternate">
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" nowrap>
                                        <asp:RadioButton ID="rb_VersandExpress" runat="server" Text="rb_VersandExpress" AutoPostBack="True" />
                                    </td>
                                    <td nowrap class="StandardTableAlternate">
                                        &nbsp;
                                    </td>
                                    <td nowrap>
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" nowrap>
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trDHL" runat="server" visible="false">
                                    <td class="StandardTableAlternate">
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" nowrap>
                                        <asp:Image ID="imgDHL" runat="server" ImageUrl="/Portal/Images/DHL.png" />
                                    </td>
                                    <td class="StandardTableAlternate" nowrap colspan="4">
                                        <table>
                                            <tr>
                                                <td rowspan="0">
                                                    DHL-Express: Rechnungsstellung erfolgt monatlich direkt an den Anforderer durch
                                                    den DAD
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="0">
                                                    <asp:RadioButton ID="rb_0900" runat="server" Text="rb_0900" GroupName="Versandart" />
                                                    &nbsp;<asp:Label ID="lbl_0900" runat="server" Text="lbl_0900" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="0">
                                                    <asp:RadioButton ID="rb_1000" runat="server" Text="rb_1000" GroupName="Versandart" />
                                                    &nbsp;<asp:Label ID="lbl_1000" runat="server" Text="lbl_1000" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="0">
                                                    <asp:RadioButton ID="rb_1200" runat="server" Text="rb_1200" GroupName="Versandart" />
                                                    &nbsp;<asp:Label ID="lbl_1200" runat="server" Text="lbl_1200" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Alle Kosten verstehen sich netto zzgl. Mwst. (auch Samstags Auslieferungen)
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trTNT" runat="server" visible="false">
                                    <td class="StandardTableAlternate">
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" nowrap>
                                        <asp:Image ID="imgTNT" runat="server" ImageUrl="/Portal/Images/TNT.png" />
                                    </td>
                                    <td class="StandardTableAlternate" nowrap colspan="4">
                                        <table>
                                            <tr>
                                                <td rowspan="0">
                                                    TNT-Express: Rechnungsstellung erfolgt direkt an den Anforderer durch TNT
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:RadioButton ID="rb_Sendungsverfolgt" runat="server" Text="rb_Sendungsverfolgt"
                                                        GroupName="Versandart" />
                                                    &nbsp;<asp:Label ID="lbl_Sendungsverfolgt" runat="server" Visible="False" Text="lbl_Sendungsverfolgt" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="0" class="style2">
                                                    <asp:RadioButton ID="rb_0900TNT" runat="server" Text="rb_0900TNT" GroupName="Versandart" />
                                                    &nbsp;<asp:Label ID="lbl_0900TNT" runat="server" Text="lbl_0900TNT" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="0">
                                                    <asp:RadioButton ID="rb_1000TNT" runat="server" Text="rb_1000TNT" GroupName="Versandart" />
                                                    &nbsp;<asp:Label ID="lbl_1000TNT" runat="server" Text="lbl_1000TNT" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="0">
                                                    <asp:RadioButton ID="rb_1200TNT" runat="server" Text="rb_1200TNT" GroupName="Versandart" />
                                                    &nbsp;<asp:Label ID="lbl_1200TNT" runat="server" Text="lbl_1200TNT" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Alle Kosten verstehen sich netto zzgl. Mwst. (nur Auslieferungen Montag - Freitag)
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trHinweis" runat="server" visible="false">
                                    <td class="StandardTableAlternate">
                                        &nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" nowrap valign="top">
                                        Hinweis:
                                    </td>
                                    <td class="StandardTableAlternate" colspan="4">
                                        Der Expressversand erfolgt&nbsp; auf Kosten des Anforderers. Bitte beachten Sie
                                        hierzu die Beförderungsbedingungen
                                        <br />
                                        des jeweiligen Dienstleisters.
                                    </td>
                                </tr>
                                <tr>
                                    <td class="StandardTableAlternate" colspan="6">
                                        Achtung: Auslieferungen erfolgen täglich bei Beauftragung vor
                                        <asp:Label ID="lbl_Uhrzeit" runat="server" Text="Uhrzeit" />.
                                        <asp:Label ID="lbl_Hinweistext" runat="server" Text="" />
                                    </td>
                                </tr>
                            </table>
                            <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                <tr id="trZeigeVorgegebeneAdressen" runat="server">
                                    <td class="StandardTableAlternate" valign="middle" width="170">
                                        <asp:RadioButton ID="rb_Zweigstellen" runat="server" GroupName="grpVersand" Checked="True"
                                            Text="Versandadressen:" AutoPostBack="True" />
                                    </td>
                                    <td class="StandardTableAlternate" align="left" valign="bottom" width="100%">
                                        <asp:DropDownList ID="cmbZweigstellen" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trZeigeZULST" runat="server">
                                    <td class="StandardTableAlternate" valign="middle" width="170">
                                        <asp:RadioButton ID="rb_Zulassungsstellen" runat="server" GroupName="grpVersand"
                                            Text="Zulassungsstellen:" AutoPostBack="true" />
                                    </td>
                                    <td class="StandardTableAlternate" align="left" valign="bottom" width="100%">
                                        <asp:DropDownList ID="cmbZulassungstellen" Visible="False" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trZeigeManuelleAdresse" runat="server">
                                    <td class="StandardTableAlternate" width="17">
                                        <table id="Table22" cellspacing="0" cellpadding="5" width="147" align="left" bgcolor="white"
                                            border="0">
                                            <tr>
                                                <td class="StandardTableAlternate" align="left" width="173" height="29">
                                                    <asp:RadioButton ID="rb_Manuell" runat="server" GroupName="grpVersand" Text="manuelle Eingabe:"
                                                        AutoPostBack="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="188" height="27">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="188" height="27">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="188" height="27">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="StandardTableAlternate" nowrap>
                                        <table id="tbl_Adresse" visible="False" runat="server" height="155" cellspacing="0"
                                            cellpadding="0" align="left" bgcolor="white" border="0">
                                            <tr>
                                                <td class="StandardTableAlternate" width="200">
                                                    <asp:Label ID="Label3" runat="server">Name:</asp:Label>&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:TextBox ID="txt_Name" runat="server" Width="255px" MaxLength="40"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" width="188">
                                                </td>
                                                <td class="StandardTableAlternate" width="133%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="200">
                                                    <asp:Label ID="lbl_Name2" runat="server">lbl_Name2</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:TextBox ID="txt_Name2" runat="server" Width="255px" MaxLength="40"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" width="188">
                                                    &nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" width="133%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="200">
                                                    <asp:Label ID="Label2" runat="server">Strasse:</asp:Label>&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" width="188">
                                                    <asp:TextBox ID="txt_Strasse" runat="server" Width="255px" MaxLength="60"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" width="188">
                                                    &nbsp;&nbsp;<asp:Label ID="lbl_Nummer" runat="server">Nr.:</asp:Label>&nbsp;
                                                    <asp:TextBox ID="txt_Nummer" runat="server" Width="45px" MaxLength="10"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" width="133%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="200">
                                                    <asp:Label ID="lbl_PLZ" runat="server">PLZ:</asp:Label>&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:TextBox ID="txt_PLZ" runat="server" Width="99px" MaxLength="10"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" width="188" height="27">
                                                </td>
                                                <td class="StandardTableAlternate" width="133%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="200">
                                                    <asp:Label ID="lbl_Ort" runat="server">Ort:</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:TextBox ID="txt_Ort" runat="server" Width="255px" MaxLength="40"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" width="188">
                                                </td>
                                                <td class="StandardTableAlternate" width="133%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="200">
                                                    <asp:Label ID="lbl_Land" runat="server">Land:</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:DropDownList ID="ddl_Land" runat="server" Enabled="False">
                                                        <asp:ListItem Value="0" Selected="True">DE</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="StandardTableAlternate" width="188">
                                                </td>
                                                <td class="StandardTableAlternate" width="133%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="top" width="173">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td class="TextLarge" valign="top" align="left" width="100%">
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr id="ZeigeTEXT50" runat="server">
                                    <td class="StandardTableAlternate" valign="top" nowrap width="173">
                                        Kunde für Anforderungen mit<br>
                                        erweitertem Zahlungsziel<br>
                                        (Delayed Payment) endgültig
                                    </td>
                                    <td class="StandardTableAlternate" valign="top" align="left" width="100%">
                                        <asp:TextBox ID="txtTEXT50" runat="server" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="rfvTEXT50" runat="server" ErrorMessage="Eingabe erforderlich"
                                            ControlToValidate="txtTEXT50" Enabled="false" />
                                    </td>
                                </tr>
                                <tr id="trInfTxt" runat="server">
                                    <td class="InfoText" valign="top" nowrap width="553" colspan="2">
                                        <strong><u><span class="style1">Hinweis:</span><br class="style1">
                                        </u></strong><span class="style1">Die Deutsche Post AG garantiert für diese Sendungen
                                            keine Lauf- und Zustellungszeiten</span><br class="style1">
                                        <span class="style1">und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
                                        </span>
                                        <br class="style1">
                                        <br class="style1">
                                        <span class="style1"><strong>&nbsp;&nbsp;&nbsp;-95% aller Sendungen werden dem Empfänger
                                            innerhalb von 24 Stunden zugestellt,</strong></span><br class="style1">
                                        <span class="style1"><strong>&nbsp;&nbsp;&nbsp;-3% aller Sendungen benötigen zwischen
                                            24 und 48 Stunden bis zur Zustellung.</strong></span><br class="style1">
                                        <br class="style1">
                                        <span class="style1">Bitte beachten Sie hierzu auch die Beförderungsbedingungen der
                                            Deutschen Post AG.</span>
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="halterInfoView" runat="server">
                            <div style="width: 100%;">
                                <div style="float: left; width: 40%">
                                    <table width="100%" cellpadding="3px" cellspacing="0" style="padding: 5px">
                                        <caption style="padding: 5px; font-weight: bold; text-align: left;">
                                            Halterinformationen</caption>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_HTitle" runat="server" Text="Anrede" AssociatedControlID="ddl_HTitle" />
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddl_HTitle" runat="server" Width="100%">
                                                    <asp:ListItem Text="-- Bitte wählen -- " Value="" Selected="True" />
                                                    <asp:ListItem Text="Herr" Value="Herr" />
                                                    <asp:ListItem Text="Frau" Value="Frau" />
                                                    <asp:ListItem Text="Firma" Value="Firma" />
                                                </asp:DropDownList>
                                                <asp:CustomValidator ControlToValidate="ddl_HTitle" ID="cv_HTitle" runat="server"
                                                    OnServerValidate="ValidateTitle" ErrorMessage="Bitte eine Anrede auswählen."
                                                    ValidateEmptyText="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_HFirstName" runat="server" Text="Vorname" AssociatedControlID="txt_HFirstName" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txt_HFirstName" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_HLastName" runat="server" Text="Name / Firma" AssociatedControlID="txt_HLastName" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txt_HLastName" runat="server" Width="100%" />
                                                <asp:RequiredFieldValidator ControlToValidate="txt_HLastName" ID="rfv_HLastName"
                                                    runat="server" ErrorMessage="Name / Firma bitte ausfüllen" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_HStreet" runat="server" Text="Strasse / Hausnummer" AssociatedControlID="txt_HStreet" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txt_HStreet" runat="server" Width="100%" />
                                                <asp:RequiredFieldValidator ControlToValidate="txt_HStreet" ID="rfv_HStreet" runat="server"
                                                    ErrorMessage="Strasse / Hausnummer bitte ausfüllen" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_HPostCodeCity" runat="server" Text="PLZ / Ort" AssociatedControlID="txt_HPostCode" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_HPostCode" runat="server" MaxLength="5" Width="50px" />
                                                <asp:RequiredFieldValidator ControlToValidate="txt_HPostCode" ID="rfv_HPostCode"
                                                    runat="server" ErrorMessage="PLZ bitte ausfüllen" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_HCity" runat="server" Width="100%" />
                                                <asp:RequiredFieldValidator ControlToValidate="txt_HCity" ID="rfv_HCity" runat="server"
                                                    ErrorMessage="Ort bitte ausfüllen" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_HReference" runat="server" Text="Referenz" AssociatedControlID="txt_HReference" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txt_HReference" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_HZulOrt" runat="server" Text="Zulassungsort" AssociatedControlID="txt_HZulOrt" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txt_HZulOrt" runat="server" Width="100%" />
                                                <asp:RequiredFieldValidator ControlToValidate="txt_HZulOrt" ID="rfv_HZulOrt" runat="server"
                                                    ErrorMessage="Bitte geben Sie den Zulassungsort an." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding-top:5px">
                                                <asp:Label ID="lbl_HZulDatum" runat="server" Text="Zulassungsdatum" AssociatedControlID="txt_HZulDatum" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txt_HZulDatum" runat="server" Width="100%" />
                                                <ajaxToolkit:CalendarExtender ID="ce_HZulDatum" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_HZulDatum">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="mee_HZulDatum" runat="server" TargetControlID="txt_HZulDatum"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </ajaxToolkit:MaskedEditExtender>
                                                <asp:RequiredFieldValidator ControlToValidate="txt_HZulDatum" ID="rfv_HZulDatum"
                                                    runat="server" ErrorMessage="Bitte wählen Sie ein Datum."></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ControlToValidate="txt_HZulDatum" ID="cv_HZulDatum" runat="server"
                                                    OnServerValidate="ValidateZulassungsDatum" ErrorMessage="Zulassungsdatum darf nicht in der Vergangenheit oder am Wochenende liegen."  style="float:left" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float: left; width: 30%">
                                    <table width="100%" cellpadding="3px" cellspacing="0" style="padding: 5px;">
                                        <caption style="padding: 5px; font-weight: bold; text-align: left;">
                                            Bemerkungen</caption>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txt_HComment" runat="server" TextMode="MultiLine" Rows="8" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float: left; width: 30%">
                                    <table width="100%" cellpadding="3px" cellspacing="0" style="padding: 5px">
                                        <caption style="padding: 5px; font-weight: bold; text-align: left;">
                                            Vorhandene Unterlagen</caption>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_HPerso" runat="server" Text="Personalausweis im Original" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_HVollmacht" runat="server" Text="Vollmacht im Original" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_HECCopy" runat="server" Text="Kopie (beidseitig) der EC-Karte" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_HEVBNr" runat="server" Text="EVB Nummer" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_HWunschKennz" runat="server" Text="Wunschkennzeichen" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="clear: both; width: 0px; height: 0px;" />
                            </div>
                </asp:View>
                <asp:View ID="haendlerZulView" runat="server">
                    <table width="60%" cellpadding="3px" cellspacing="0" style="padding: 5px">
                        <caption style="padding: 5px; font-weight: bold; text-align: left;">
                            Händlereigene Zulassung</caption>
                        <tr>
                            <td class="StandardTableAlternate">
                                <asp:Label ID="lbl_Zulassungsdatum" runat="server" Text="Zulassungsdatum" AssociatedControlID="txt_Zulassungsdatum" />
                            </td>
                            <td class="StandardTableAlternate">
                                <asp:TextBox ID="txt_Zulassungsdatum" CssClass="TextBoxNormal" Width="100px" runat="server" />
                                <ajaxToolkit:CalendarExtender ID="ce_Zulassungsdatum" runat="server" Format="dd.MM.yyyy"
                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_Zulassungsdatum">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="mee_Zulassungsdatum" runat="server" TargetControlID="txt_Zulassungsdatum"
                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                </ajaxToolkit:MaskedEditExtender>
                                <asp:RequiredFieldValidator ControlToValidate="txt_Zulassungsdatum" ID="rfv_Zulassungsdatum"
                                    runat="server" ErrorMessage="Bitte wählen Sie ein Datum"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="StandardTableAlternate">
                                <asp:CheckBox ID="chk_Abmeldung" runat="server" Text="Abmeldung beauftragen" AutoPostBack="true"
                                    OnCheckedChanged="AbmeldungChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="StandardTableAlternate">
                                <asp:Label ID="lbl_Abmeldung" runat="server" Text="Abmeldung am" AssociatedControlID="txt_Abmeldung"
                                    Enabled="false" />
                            </td>
                            <td class="StandardTableAlternate">
                                <asp:TextBox ID="txt_Abmeldung" CssClass="TextBoxNormal" Width="100px" runat="server"
                                    Enabled="false" />
                                <ajaxToolkit:CalendarExtender ID="ce_Abmeldung" runat="server" Format="dd.MM.yyyy"
                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_Abmeldung">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="mee_Abmeldung" runat="server" TargetControlID="txt_Abmeldung"
                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                </ajaxToolkit:MaskedEditExtender>
                                <asp:RequiredFieldValidator ControlToValidate="txt_Abmeldung" ID="rfv_Abmeldung"
                                    runat="server" ErrorMessage="Bitte wählen Sie ein Datum" Enabled="false"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="StandardTableAlternate">
                                <asp:Label ID="lbl_Reference" runat="server" Text="Referenz" AssociatedControlID="txt_Reference" />
                            </td>
                            <td class="StandardTableAlternate">
                                <asp:TextBox ID="txt_Reference" CssClass="TextBoxNormal" Width="100px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="versandConfirm" runat="server">
                    <table cellspacing="0" cellpadding="5" width="100%" border="0" bgcolor="white" class="TableKontingent">
                        <tr>
                            <td class="StandardTableAlternate" valign="top">
                                Zustellart:
                            </td>
                            <td class="StandardTableAlternate" valign="top" align="left" colspan="2" width="100%">
                                <asp:Label ID="lblVersandart" runat="server"></asp:Label><asp:Label ID="lblMaterialNummer"
                                    runat="server" Visible="False"></asp:Label><asp:Label ID="lblVersandhinweis" runat="server"
                                        Visible="False"> - Gilt nicht für gesperrt angelegte Anforderungen!</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextLarge" valign="top">
                                Adresse:
                            </td>
                            <td class="TextLarge" valign="top" align="left" colspan="2">
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <telerik:RadGrid ID="selFzgGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                        BorderWidth="0" Culture="de-DE" OnNeedDataSource="GridNeedDataSource">
                        <MasterTableView CommandItemDisplay="Top" Summary="Gewählte Fahrzeuge">
                            <ItemStyle Wrap="false" />
                            <AlternatingItemStyle Wrap="false" />
                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false"
                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                            <Columns>
                                <%--Selektion--%>
                                <telerik:GridTemplateColumn DataField="Selected" SortExpression="Selected">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# Eval("Selected") %>' Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <%--Fahrgestellnummer--%>
                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" />
                                <%--ZBII/Fahrzeugbriefnummer (laut SAP:Technische Identnummer)--%>
                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" UniqueName="TIDNR" />
                                <%--EQUI-COC-Bescheinigung vorhanden=X, E--%>
                                <telerik:GridTemplateColumn DataField="ZZCOCKZ" SortExpression="ZZCOCKZ" UniqueName="ZZCOCKZ">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# Eval("ZZCOCKZ") = "X" %>' Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <%--Kfz-Kennzeichen--%>
                                <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM" />
                                <%--Datum, an dem der Satz hinzugefügt wurde--%>
                                <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" DataFormatString="{0:dd.MM.yyyy}"
                                    UniqueName="ERDAT" />
                                <%--DAD FFD KFZ Brief bezahlt--%>
                                <telerik:GridTemplateColumn DataField="ZZBEZAHLT" SortExpression="ZZBEZAHLT" UniqueName="ZZBEZAHLT">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# Eval("ZZBEZAHLT") = "X" %>' Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <%--Abrufgrund--%>
                                <telerik:GridTemplateColumn DataField="AUGRU" SortExpression="AUGRU" UniqueName="AUGRU">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="augruCombox" runat="server" AutoPostBack="true" DataValueField="SapWert"
                                            DataTextField="WebBezeichnung" DataSourceID="augruSource" SelectedValue='<%# Eval("AUGRU") %>'
                                            Enabled="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="COMMENT" SortExpression="COMMENT" UniqueName="COMMENT" />
                                <telerik:GridBoundColumn DataField="TEXT300" SortExpression="TEXT300" UniqueName="TEXT300" />
                                <%--Fahrzeugnummer (laut SAP:DAD Referenzfeld)--%>
                                <telerik:GridBoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" UniqueName="ZZREFERENZ1" />
                                <%--Equipmentnummer--%>
                                <telerik:GridBoundColumn DataField="EQUNR" SortExpression="EQUNR" UniqueName="EQUNR" />
                                <%--Lizenznummer des Equipments--%>
                                <telerik:GridBoundColumn DataField="LIZNR" SortExpression="LIZNR" UniqueName="LIZNR" />
                                <%--DAD FFD Finanzierungsart--%>
                                <telerik:GridBoundColumn DataField="ZZFINART" SortExpression="ZZFINART" UniqueName="ZZFINART" />
                                <%--Laufzeit in Tagen--%>
                                <telerik:GridBoundColumn DataField="ZZLAUFZEIT" SortExpression="ZZLAUFZEIT" UniqueName="ZZLAUFZEIT" />
                                <%--Label/Marke + ASL Klärfall--%>
                                <telerik:GridBoundColumn DataField="ZZLABEL" SortExpression="ZZLABEL" UniqueName="ZZLABEL" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </asp:View>
                <asp:View ID="doneView" runat="server">
                    <div style="width: 100%">
                        <asp:Label ID="lbl_Message" runat="server" Style="line-height: 3em; margin-left: 50px;
                            font-weight: bold;" />
                    </div>
                    <div style="width: 100%">
                        <asp:HyperLink ID="cmdDownload" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="false" Font-Underline="false" Style="margin-left: 50px;"
                            NavigateUrl="Change11_2.aspx?Download=1" Visible="false" Text="Download" />
                    </div>
                </asp:View>
                </asp:MultiView>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                <div id="dataFooter">
                    <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                        CausesValidation="False" Font-Underline="False" OnClick="SendClick">» Absenden</asp:LinkButton>
                </div>
                <asp:SqlDataSource ID="augruSource" runat="server" CancelSelectOnNullParameter="true"
                    DataSourceMode="DataSet" EnableCaching="true" SelectCommand="SELECT GrundID, WebBezeichnung, SapWert, MitZusatzText, Zusatzbemerkung, VersandadressArt, Eingeschraenkt FROM CustomerAbrufgruende WHERE CustomerID=@cID AND GroupID=@gID AND AbrufTyp='endg';">
                    <SelectParameters>
                        <asp:Parameter Name="cID" />
                        <asp:Parameter Name="gID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
