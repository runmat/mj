<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="AppBPLG.Change04" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server">
    </uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" border="0">
        <tr>
            <td colspan="2">
                <uc1:Header ID="ucHeader" runat="server">
                </uc1:Header>
            </td>
        </tr>
        <tr>
            <td class="PageNavigation" colspan="2">
                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TaskTitle" valign="top" colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="TABLEX" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td valign="top" width="80">
                <asp:LinkButton ID="lb_Suche" runat="server" CssClass="StandardButton">Suche</asp:LinkButton>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td width="80">
                &nbsp;
            </td>
            <td align="left">
                <table id="Table3" cellspacing="0" width="100%" bgcolor="white" border="0">
                    <tr class="TextLarge">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_FIN"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFIN" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="StandardTableAlternate">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_LIZNR"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLIZNR" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="TextLarge">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_Haendlernummer"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHaendlernummer" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="StandardTableAlternate">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_Endkundennummer"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndkundennummer" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="80">
                &nbsp;
            </td>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="80">
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
