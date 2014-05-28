<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="_Report00.aspx.vb" Inherits="AppDCL._Report00" %>

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
    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
    <table width="100%" align="center">
        <tr>
            <td height="25">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2" height="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Abfragekriterien)</asp:Label><asp:HyperLink
                                ID="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx"
                                CssClass="PageNavigation">Abfragekriterien</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="StandardTableButtonFrame" valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        <asp:Label ID="lblFahrernrtext" runat="server" Font-Bold="True">Bitte Fahrer-Nummer eingeben:&nbsp;</asp:Label>
                                        <asp:TextBox ID="txtFahrernr" TabIndex="1" runat="server"></asp:TextBox>&nbsp;
                                        <asp:LinkButton ID="btnFahrernr" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Aufträge suchen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelExtraLarge">
                                        <table id="Table9" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="left" colspan="2">
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
