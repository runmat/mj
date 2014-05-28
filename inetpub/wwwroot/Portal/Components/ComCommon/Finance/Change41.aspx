<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change41.aspx.vb" Inherits="CKG.Components.ComCommon.Change41" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SucheHaendler" Src="PageElements/SucheHaendler.ascx" %>
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
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;
                            </td>
            </td>
        </tr>
        <tr>
            <td valign="top" width="100">
                <table id="Table2" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                    <tr id="trCreate" runat="server">
                        <td valign="center" width="100">
                            <asp:LinkButton ID="lbSuche" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="center" width="100">
                            <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" Visible="false" CssClass="StandardButton">&#149;&nbsp;Neue Suche</asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td valign="top" align="left">
                                <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                    <tr>
                                        <td width="100">
                                        </td>
                                        <td colspan="1" align="left" class="TaskTitle">
                                            <asp:Label runat="server" ID="lbl_AnzeigeHaendlerSuche" Font-Underline="true" Font-Bold="true"
                                                ForeColor="Black" Font-Size="Larger" Text="lbl_AnzeigeHaendlerSuche"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100">
                                        </td>
                                        <td colspan="2" style="border-color: #f5f5f5; border-style: solid; border-width: 3;">
                                            <uc1:SucheHaendler ID="SucheHaendler1" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" width="150">
                &nbsp;
            </td>
            <td valign="top">
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
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
    </form>
</body>
</html>
