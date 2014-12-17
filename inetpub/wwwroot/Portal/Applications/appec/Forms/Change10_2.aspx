<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10_2.aspx.vb" Inherits="AppEC.Change10_2" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1 
        {
            width: 185px;
        }
        .tablestyle1 td 
        {
            height: 25px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
         <script type="text/javascript">
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
        </script>      
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            &nbsp;Bitte Daten eingeben.
                        </td>
                    </tr>
                    <tr>
                        <td width="100">
                            <table bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td >
                                        <asp:LinkButton ID="cmdBack" Height="16px" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="100">
                            &nbsp;
                        </td>
                        <td>
                            <table class="BorderLeftBottom tablestyle1" style="padding-left: 10px; padding-right: 10px" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td nowrap colspan="3">
                                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"/>
                                        &nbsp;
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" CssClass="TextError" ForeColor="#009933"/>
                                    </td>
                                </tr>
                                <tr id="trAltNeu" runat="server">
                                    <td valign="top" class="style1">
                                        &nbsp;
                                    </td>
                                    <td valign="top" class="style1">
                                        Alt
                                    </td>
                                    <td valign="top" class="style1">
                                        Neu
                                    </td>
                                </tr>
                                <tr id="trHersteller" runat="server">
                                    <td class="style1">
                                        Hersteller*:
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:DropDownList ID="ddlHersteller" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Model-Id<span id="requiredModelId" runat="server">*</span>:
                                    </td>
                                    <td class="style1">
                                        <asp:Label ID="lblModellID" runat="server"/>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtModellId" runat="server" Font-Size="12px" CssClass="InputDisableStyle"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Modellbezeichnung<span id="requiredBezeichnung" runat="server">*</span>:
                                    </td>
                                    <td class="style1" nowrap>
                                        <asp:Label ID="lblModellBez" runat="server"/>
                                    </td>
                                    <td class="style1" nowrap>
                                        <asp:TextBox ID="txtModellBez" runat="server" Font-Size="12px" CssClass="InputDisableStyle"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        SIPP-Code<span id="requiredSippCode" runat="server">*</span>:
                                    </td>
                                    <td class="style1">
                                        <asp:Label ID="lblSippCode" runat="server"/>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtSippcode" runat="server" Font-Size="12px" CssClass="InputDisableStyle" MaxLength="4"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Laufzeit in Tagen:
                                    </td>
                                    <td class="style1">
                                        <asp:Label ID="lblLaufzeit" runat="server"/>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtLaufzeit" runat="server" Font-Size="12px" onKeyPress="return numbersonly(event, false)" CssClass="InputDisableStyle" MaxLength="4"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Laufzeitbindung:
                                    </td>
                                    <td class="style1">
                                        <asp:CheckBox ID="cbxLaufzBind" runat="server" TextAlign="Left" Enabled="False"/>
                                    </td>
                                    <td class="style1">
                                        <asp:CheckBox ID="cbxLaufzBindNeu" runat="server" TextAlign="Left"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Fahrzeuggruppe LKW:
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblFzggruppeLkw" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblFzggruppeLkwNeu" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Wintertaugliche Bereifung:
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblWinterreifen" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblWinterreifenNeu" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Anhängerkupplung:
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblAhk" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblAhkNeu" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Navigationssystem:
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblNavi" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblNaviNeu" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Securiti Fleet:
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblSecuritiFleet" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblSecuritiFleetNeu" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        Leasing:
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblLeasing" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:RadioButtonList ID="rblLeasingNeu" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Ja" Text="Ja"/>
                                            <asp:ListItem Value="Nein" Text="Nein"/>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblRequiredFieldsHint" runat="server">* = Pflichtfelder</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <p align="right">
                                            <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton" Height="16px" Width="78px"> &#149;&nbsp;Speichern</asp:LinkButton>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
