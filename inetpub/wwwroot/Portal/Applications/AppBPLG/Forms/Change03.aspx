<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="AppBPLG.Change03" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
    <table width="100%" align="center">
        <tr>
            <td colspan="2">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td class="PageNavigation" colspan="3">
                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                <asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugsuche)</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TaskTitle" colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td width="100">
                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:LinkButton>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <br />
                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr id="tr_LIZNR" class="TextLarge" runat="server">
                                    <td class="TextLarge" width="160">
                                        <asp:Label ID="lbl_LIZNR" runat="server">lbl_LIZNR</asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txt_LIZNR" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
</body>
</html>
