<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07.aspx.vb" Inherits="AppCSC.Change07" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" nowrap colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> - Vorgangssuche</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="121">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="121"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center">
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td class="TextLarge" width="150">
                                                </td>
                                                <td class="TextLarge">
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
                        <td valign="top" width="121">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="121">
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
