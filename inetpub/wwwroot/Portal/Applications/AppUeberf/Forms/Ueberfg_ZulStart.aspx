<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Ueberfg_ZulStart.aspx.vb"
    Inherits="AppUeberf.Ueberfg_ZulStart" %>

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
                            <td class="PageNavigation" colspan="2" height="19">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>
                                <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="100%">
                                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="TaskTitle" valign="top" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PageNavigation" valign="top">
                                            <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="middle" width="100%">
                                            <table class="BorderLeftBottom" id="Table1" cellspacing="0" cellpadding="5" width="400"
                                                bgcolor="white" border="0">
                                                <tr>
                                                    <td class="TextLarge" valign="center" nowrap width="49">
                                                    </td>
                                                    <td class="" valign="center" width="295" nowrap>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" valign="center" nowrap width="49">
                                                        <strong>Beauftragung:</strong>
                                                    </td>
                                                    <td class="" valign="center" nowrap width="295">
                                                        <asp:CheckBox ID="chkZul" runat="server" Text="Zulassung" Checked="True" AutoPostBack="True">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" valign="center" nowrap width="49">
                                                    </td>
                                                    <td valign="center" nowrap width="295">
                                                        <asp:CheckBox ID="chkUeberf" runat="server" Text="Überführung" Checked="True" AutoPostBack="True">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" valign="center" nowrap width="49">
                                                    </td>
                                                    <td class="" valign="center" width="295">
                                                        <p align="right">
                                                            <asp:ImageButton ID="cmdRight1" runat="server" Width="73px" ImageUrl="/Portal/Images/arrowUeberfRight.gif"
                                                                Height="34px"></asp:ImageButton></p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <p align="right">
                                    &nbsp;</p>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
