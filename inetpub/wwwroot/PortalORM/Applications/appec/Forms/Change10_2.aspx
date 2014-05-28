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
            height: 23px;
        }
        .style2
        {
            width: 185px;
        }
        .style3
        {
            height: 23px;
            width: 185px;
        }
        .style4
        {
            width: 181px;
        }
        .style5
        {
            height: 23px;
            width: 181px;
        }
    </style>
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
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                        <td class="" valign="top" width="100">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td >
                                        <asp:LinkButton ID="cmdBack"  Height="16px" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" style="padding-bottom: 2px" width="150">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="" valign="top">
                                    </td>
                                </tr>
                            </table>
                            <table class="BorderLeftBottom" id="Table3" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="2" bgcolor="white" border="0">
                                            <tr>
                                                <td valign="middle" nowrap colspan="4">
                                                    <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;<asp:Label
                                                        ID="lblMessage" runat="server" EnableViewState="False" CssClass="TextError" ForeColor="#009933"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" nowrap>
                                                    &nbsp;
                                                </td>
                                                <td class="" valign="middle" nowrap>
                                                    &nbsp;
                                                </td>
                                                <td class="style2" valign="middle" nowrap>
                                                    Alt
                                                </td>
                                                <td valign="top" nowrap class="style4">
                                                    Neu
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" nowrap class="style1">
                                                </td>
                                                <td class="style1" valign="middle" nowrap>
                                                    Model-Id:
                                                </td>
                                                <td class="style3" valign="middle" nowrap>
                                                    <asp:Label ID="lblModellID" runat="server"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap class="style5">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" nowrap>
                                                </td>
                                                <td valign="middle" nowrap>
                                                    <asp:Label ID="Label1" runat="server">Modellbezeichnung:</asp:Label>
                                                </td>
                                                <td valign="middle" nowrap class="style2">
                                                    <asp:Label ID="lblModellBez" runat="server"></asp:Label>
                                                </td>
                                                <td valign="middle" nowrap class="style4">
                                                    <asp:TextBox ID="txtModellBez" runat="server" Font-Size="12px" CssClass="InputDisableStyle" ></asp:TextBox>
                                            </tr>
                                            <tr>
                                                <td valign="middle" nowrap>
                                                </td>
                                                <td class="" valign="middle" nowrap>
                                                    SIPP-Code:
                                                </td>
                                                <td class="style2" valign="middle">
                                                    <asp:Label ID="lblSippCode" runat="server"></asp:Label>
                                                </td>
                                                <td valign="top" class="style4">
                                                    <asp:TextBox ID="txtSippcode" runat="server" Font-Size="12px" CssClass="InputDisableStyle" MaxLength="4"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle">
                                                </td>
                                                <td class="" valign="middle">
                                                    Laufzeit in Tagen:
                                                </td>
                                                <td class="style2" valign="middle">
                                                    <asp:Label ID="lblLaufzeit" runat="server"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap class="style4">
                                                    <asp:TextBox ID="txtLaufzeit" runat="server" Font-Size="12px" onKeyPress="return numbersonly(event, false)"  CssClass="InputDisableStyle" MaxLength="4"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle">
                                                    &nbsp;
                                                </td>
                                                <td class="" valign="middle">
                                                    Laufzeitbindung
                                                </td>
                                                <td class="style2" valign="middle">
                                                    <asp:CheckBox ID="cbxLaufz" runat="server" TextAlign="Left" Enabled="False"></asp:CheckBox>
                                                </td>
                                                <td valign="top" nowrap class="style4">
                                                    <asp:CheckBox ID="cbxLaufzBindNeu" runat="server" TextAlign="Left"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle">
                                                </td>
                                                <td valign="middle">
                                                    <asp:DropDownList ID="ddlModellHidden" runat="server" EnableViewState="True" Enabled="False"
                                                        Width="0px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="middle" colspan="1" class="style2">
                                                    <p align="right">
                                                        &nbsp;</p>
                                                </td>
                                                <td valign="middle" class="style4">
                                                    <p align="right">
                                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"  Height="16px" Width="78px"> &#149;&nbsp;Speichern</asp:LinkButton></p>
                                                </td>
                                            </tr>
                                        </table>
                                        <input id="txtHerstellerHidden" type="hidden" size="1" runat="server">
                                        <input id="txtHerstellerBezeichnungHidden" type="hidden" size="1" runat="server">
                                    </td>
                                </tr>
                            </table>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
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
