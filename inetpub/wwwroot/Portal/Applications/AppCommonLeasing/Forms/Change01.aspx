<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="AppCommonLeasing.Change01" %>

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
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;
                            </td>
            </td>
        </tr>
        <tr>
            <td valign="top" width="120">
                <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                    <tr id="trCreate" runat="server">
                        <td valign="center">
                            <asp:LinkButton ID="lb_Weiter" OnClientClick="Show_BusyBox1();" Text="Weiter" runat="server"
                                CssClass="StandardButton"></asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="center">
                            &nbsp;</td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td width="100">
            </td>
            <td valign="top">
                <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                    <tr class="TextLarge">
                        <td  align="left" nowrap="nowrap">
                            <asp:Label ID="lbl_AnzahlTage" runat="server"></asp:Label>
                            
                        </td>
                        <td  align="center">
                            <asp:TextBox ID="txtFaelligkeit" runat="server" Font-Bold="True" Height="21px" Width="22px"></asp:TextBox>
                        </td>
                        <td width="100%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="StandardTableAlternate">
                        <td  align="left">
                            <asp:Label ID="lbl_Kurzwahl" runat="server"></asp:Label>
                        </td >
                            <td align="left">
                                <asp:RadioButtonList   CellPadding="3" ID="rblKurzwahl" RepeatDirection="Horizontal" runat="server"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                    <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                    <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                </asp:RadioButtonList>
                           
                          
                         
                           </td>
                        <td width="100%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" width="100">
                &nbsp;
            </td>
            <td align="left" colspan="2">
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
