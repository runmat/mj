<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report206.aspx.vb" Inherits="AppARVAL.Report206" %>

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
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0" bgcolor="white">
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                    <p>
                                                        Kfz-Kennzeichen:</p>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtAmtlKennzeichen" runat="server"></asp:TextBox>&nbsp;(XX-Y1234)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                    <p>
                                                        Leasingvertrags-Nr:</p>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtOrdernummer" runat="server"></asp:TextBox>&nbsp;(12345678)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtBriefnummer" runat="server" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
