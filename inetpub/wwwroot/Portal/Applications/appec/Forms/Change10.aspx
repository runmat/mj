<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10.aspx.vb" Inherits="AppEC.Change10" %>

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
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">

    <script language="JavaScript" type="text/javascript">
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
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2" height="19">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>
                                <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                            </td>
                        </tr>
                        <td valign="top" width="100%">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top" width="100%">
                                        &nbsp;Bitte Model-ID eingeben.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="middle" width="100%">
                                        <table class="BorderLeftBottom" cellspacing="0" cellpadding="5" width="400" bgcolor="white" border="0">
                                            <tr>
                                                <td class="TextLarge" align="left" nowrap="nowrap">
                                                    Model-ID:
                                                </td>
                                                <td align="left" style="padding-left: 15px" width="100%">
                                                    <asp:TextBox onKeyPress="return numbersonly(event, false)" ID="txtModelID" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top" align="left" nowrap="nowrap">
                                                    Aktion:
                                                </td>
                                                <td class="TextLarge" style="padding-left: 15px" align="left" width="100%">
                                                    <asp:RadioButtonList ID="rblGesamt" runat="server">
                                                        <asp:ListItem Value="Ja" Selected="True">Änderung Gesamtfahrzeugbestand</asp:ListItem>
                                                        <asp:ListItem Value="Nein">Änderung Fahrzeugbestand zur Zulassung</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="center" width="100%">
                                                    <p align="right">
                                                        <asp:LinkButton ID="btnNewModelId" runat="server" Height="16px" CssClass="StandardButton"
                                                            Width="128px">Neue Model-ID&nbsp;&#187;</asp:LinkButton>
                                                        <asp:LinkButton ID="btnConfirm" runat="server" Height="16px" CssClass="StandardButton"
                                                            Width="78px" style="margin-left: 10px">Weiter&nbsp;&#187;</asp:LinkButton>
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <p align="right">
                    <!--#include File="../../../PageElements/Footer.html" -->
                </p>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
