<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Neukundenanlage.aspx.cs"
    Inherits="AppZulassungsdienst.forms.Neukundenanlage" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="firstLeft active" style="font-size: 12px">
                                            <asp:Label ID="lblNoData" runat="server" Text="Bitte füllen Sie alle Pflichtfelder(*) aus!"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Anlagetyp
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="border: none">
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 17%">
                                                        Typ*:
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        <span>
                                                            <asp:RadioButton ID="rbBarkunde" GroupName="Kunde" Text="Barkunde" runat="server"
                                                                TabIndex="2" AutoPostBack="True" Checked="True" OnCheckedChanged="rbBarkunde_CheckedChanged" />
                                                        </span>
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px;" colspan="3">
                                                        <span>
                                                            <asp:RadioButton ID="rbLieferscheinKunde" GroupName="Kunde" runat="server" Text="Lieferscheinkunde"
                                                                TabIndex="3" AutoPostBack="True" OnCheckedChanged="rbLieferscheinKunde_CheckedChanged" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                    </td>
                                                    <td class="firstLeft active" style="width: 165px; height: 25px;">
                                                        <img alt="" style="width: 25px; height: 25px; padding-right: 5px; float: left" src="/PortalZLD/Images/Pfeil_nachrechts.jpg" />
                                                        <asp:Label ID="Label1" Height="15px" Style="padding-top: 10px" runat="server" Text="Einzugsermächtigung"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" style="width: 50px;">
                                                        <span>
                                                            <asp:RadioButton ID="rbEinzugJa" GroupName="Einzug" Text="Ja" 
                                                            runat="server" TabIndex="4" AutoPostBack="True" 
                                                            oncheckedchanged="rbEinzugJa_CheckedChanged" />
                                                        </span>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <span>
                                                            <asp:RadioButton ID="rbEinzugNein" GroupName="Einzug" Text="Nein" runat="server"
                                                                TabIndex="5" AutoPostBack="True" 
                                                            oncheckedchanged="rbEinzugNein_CheckedChanged" />
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 19px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Kundendaten
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 17%">
                                                        Anrede:*
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px; width: 83%" colspan="2">
                                                        <span style="padding-right: 50px">
                                                            <asp:RadioButton ID="rbFirma" GroupName="Anrede" Text="Firma" runat="server" TabIndex="6" />
                                                        </span><span style="padding-right: 55px">
                                                            <asp:RadioButton ID="rbHerr" GroupName="Anrede" Text="Herr" runat="server" TabIndex="7" />
                                                        </span><span>
                                                            <asp:RadioButton ID="rbFrau" GroupName="Anrede" Text="Frau" runat="server" TabIndex="8" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Branche*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 25%">
                                                        <asp:DropDownList ID="ddlBranche" runat="server" AutoPostBack="True" TabIndex="9"
                                                            OnSelectedIndexChanged="ddlBranche_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblBrancheFrei" Font-Bold="true" Font-Size="12px" runat="server" Visible="false"
                                                            Text="*"></asp:Label>
                                                        <asp:TextBox ID="txtBrancheFrei" Width="315px" Visible="false" runat="server" TabIndex="10"
                                                            MaxLength="20" CssClass="TextBoxNormal"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 17%">
                                                        Name 1 / Firma*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%">
                                                        <asp:TextBox ID="txtName1" CssClass="TextBoxNormal" Width="565px" runat="server"
                                                            MaxLength="40" TabIndex="11"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 130px">
                                                        Name 2 / Zusatz:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtName2" CssClass="TextBoxNormal" Width="565px" runat="server"
                                                            MaxLength="40" TabIndex="12"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 17%">
                                                        Straße*:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtStrasse" CssClass="TextBoxNormal" Width="400" runat="server"
                                                            MaxLength="60" TabIndex="13"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active" style="width: 10%; font-size: 12px">
                                                        Haus-Nr.*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 26%">
                                                        <asp:TextBox ID="txtHausnr" CssClass="TextBoxNormal" Width="49" runat="server" MaxLength="10"
                                                            TabIndex="14"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 17%">
                                                        Postleitzahl*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 90px">
                                                        <asp:TextBox ID="txtPlz" CssClass="TextBoxNormal" Width="80" runat="server" MaxLength="10"
                                                            TabIndex="15"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px; width: 35px">
                                                        Ort*:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 575px">
                                                        <asp:TextBox ID="txtOrt" CssClass="TextBoxNormal" Width="399" runat="server" MaxLength="40"
                                                            TabIndex="16"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; width: 17%">
                                                        Land*:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:DropDownList ID="ddLand" Width="150" runat="server" TabIndex="17" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddLand_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Ust-ID Nummer<asp:Label ID="lblStar" runat="server" Visible="false" Text="*"></asp:Label>:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 50%">
                                                        <asp:TextBox ID="txtUIDNummer" CssClass="TextBoxNormal" Width="266" runat="server"
                                                            MaxLength="20" TabIndex="18"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Kommunikation
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="2" style="font-size: 12px;">
                                                        Ansprechpartner
                                                    </td>
                                                </tr>
                                                <tr class="formRadioButton" visible="false">
                                                    <td class="firstLeft active" style="width: 17%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        Anrede
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        <span style="padding-right: 55px">
                                                            <asp:RadioButton ID="rbHerrKom" GroupName="AnredeKom" Text="Herr" runat="server"
                                                                TabIndex="19" />
                                                        </span><span>
                                                            <asp:RadioButton ID="rbFrauKom" GroupName="AnredeKom" Text="Frau" runat="server"
                                                                TabIndex="20" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="width: 17%; font-size: 12px; height: 22px;">
                                                        Vorname:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%; height: 22px">
                                                        <asp:TextBox ID="txtVornameAnPartner" CssClass="TextBoxNormal" Width="270" MaxLength="35"
                                                            runat="server" TabIndex="21"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; height: 22px;">
                                                        Name:
                                                    </td>
                                                    <td class="firstLeft active" style="height: 22px;">
                                                        <asp:TextBox ID="txtNameAnPartner" CssClass="TextBoxNormal" Width="270" MaxLength="35"
                                                            runat="server" TabIndex="22"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px; height: 22px;">
                                                        Funktion:
                                                    </td>
                                                    <td class="firstLeft active" style="height: 22px;">
                                                        <asp:DropDownList ID="ddlFunktion" Width="273" runat="server" TabIndex="23">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Telefon:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtTelefon" CssClass="TextBoxNormal" Width="270" MaxLength="30"
                                                            runat="server" TabIndex="24"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Mobil:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtMobil" Width="270" MaxLength="30" CssClass="TextBoxNormal" runat="server"
                                                            TabIndex="25"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Fax:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtFax" CssClass="TextBoxNormal" Width="270" MaxLength="30" runat="server"
                                                            TabIndex="26"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        E-Mail:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtMail" CssClass="TextBoxNormal" Width="270" MaxLength="100" runat="server"
                                                            TabIndex="27"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        &nbsp;
                                                    </td>
                                                    <td class="firstLeft active">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="width: 17%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        Einzugsermächtigung
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        <span style="padding-right: 55px">
                                                            <asp:RadioButton ID="rbJa" GroupName="Einzug2" Text="ja" runat="server" 
                                                            TabIndex="28"  oncheckedchanged="rbJa_CheckedChanged" AutoPostBack="True" />
                                                        </span><span>
                                                            <asp:RadioButton ID="rbNein" Checked="true" GroupName="Einzug2" Text="nein" runat="server"
                                                                TabIndex="29" AutoPostBack="True" 
                                                            oncheckedchanged="rbNein_CheckedChanged" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        IBAN:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtIBAN" Width="270" MaxLength="34" CssClass="TextBoxNormal TextUpperCase"
                                                            runat="server" TabIndex="30"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        SWIFT-BIC:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtSWIFT" CssClass="TextBoxNormal" Width="270" MaxLength="11" runat="server"
                                                            TabIndex="31" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Bank:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtBankname" Width="270" MaxLength="30" CssClass="TextBoxNormal"
                                                            runat="server" TabIndex="32" Text="Wird automatisch gefüllt!" 
                                                            Enabled ="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        &nbsp;
                                                    </td>
                                                    <td class="firstLeft active">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="width: 17%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        Gebühr mit Umsatzsteuer:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        <span style="padding-right: 55px">
                                                            <asp:RadioButton ID="rbSteuerJa" GroupName="Steuer" Text="ja" 
                                                            runat="server" TabIndex="33" 
                                                             />
                                                        </span><span>
                                                            <asp:RadioButton ID="rbSteuerNein" Checked="true" GroupName="Steuer" Text="nein"
                                                                runat="server" TabIndex="34" 
                                                             />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Tour:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:DropDownList ID="ddlTour" runat="server" AutoPostBack="True" TabIndex="35">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Umsatzerwartung/Monat:
                                                    </td>
                                                    <td class="firstLeft active">
                                                        <asp:TextBox ID="txtUmsatz" CssClass="TextBoxNormal" Width="270" MaxLength="10" runat="server"
                                                            TabIndex="36"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="width: 17%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        Kreditversicherung:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        <span style="padding-right: 55px">
                                                            <asp:RadioButton ID="rbKreditJa" GroupName="Kredit" Text="ja" 
                                                            runat="server" TabIndex="37" />
                                                        </span><span>
                                                            <asp:RadioButton ID="rbKreditNein" Checked="true" GroupName="Kredit" Text="nein"
                                                                runat="server" TabIndex="38" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="width: 17%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        Auskunft:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        <span style="padding-right: 55px">
                                                            <asp:RadioButton ID="rbAuskunftJa" GroupName="Auskunft" Text="ja" 
                                                            runat="server" TabIndex="39" />
                                                        </span><span>
                                                            <asp:RadioButton ID="rbAuskunftNein" Checked="true" GroupName="Auskunft" Text="nein"
                                                                runat="server" TabIndex="40" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        &nbsp;
                                                    </td>
                                                    <td class="firstLeft active">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                                <tr class="formRadioButton">
                                                    <td class="firstLeft active" style="width: 17%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        Bemerkung:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 100%; font-size: 12px; height: 22px; padding-top: 10px;">
                                                        <asp:TextBox ID="txtBemerkung" CssClass="TextBoxNormal" Width="565px" runat="server"
                                                            MaxLength="30" TabIndex="41"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        &nbsp;
                                                    </td>
                                                    <td class="firstLeft active">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" Style="display: none" />
                            <cc1:ModalPopupExtender runat="server" ID="MPENeukundeResultat" BackgroundCssClass="divProgress"
                                Enabled="true" PopupControlID="NeukundeResultat" TargetControlID="MPEDummy">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="NeukundeResultat" HorizontalAlign="Center" runat="server" Style="display: none">
                                <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                    style="border: solid 1px #646464">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="firstLeft active" nowrap="nowrap">
                                            <asp:Label ID="lblNeukundeResultatMeldung" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lbNeukundeResultat" Text="OK" Height="16px" Width="78px" runat="server"
                                                CssClass="Tablebutton"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div id="dataFooter">
                                <asp:LinkButton ID="lb_Neu" Visible="false" Text="Neuer Kunde" Height="16px" Width="78px"
                                    runat="server" CssClass="Tablebutton" OnClick="lb_Neu_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                    CssClass="Tablebutton" TabIndex="42" OnClick="lbAbsenden_Click"></asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
