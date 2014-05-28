<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="AppCheck.Change01" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        #upFile
        {
            width: 539px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tbody>
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
                                    <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="PageNavigation" colspan="2">
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
                                            <td valign="middle" width="150">
                                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton" Height="18px"
                                                    Width="100px"> •&nbsp;Absenden</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="150">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="150">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="150">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    </table>
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td valign="top" align="left">
                                                    <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                                        <tr>
                                                            <td class="TaskTitle">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TaskTitle">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" nowrap align="left">
                                                                Dateiauswahl:&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" nowrap align="left">
                                                                <input id="upFile" type="file" size="49" name="File1" runat="server">
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
                                <td valign="top">
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
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <!--#include File="../../../PageElements/Footer.html" -->
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
