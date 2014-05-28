<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01.aspx.vb" Inherits="AppCommonCarRent.Report01" %>


<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style2
        {
            width: 713px;
        }
        .ListSearchExt
        {
            font-weight: bolder;
        }
    </style>
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
                                &nbsp;<asp:LinkButton ID="lb_zurueck" runat="server" Visible="True">lb_zurueck</asp:LinkButton>
                            </td>
            </td>
        </tr>
        <tr>
            <td valign="top" width="120">
                <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                    <tr runat="server">
                        <td>
                            &nbsp;
                            <td>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
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
            <td width="100">
            </td>
            <td valign="top">
                <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                    <tr>
                        <td colspan="1" align="left" class="style2">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="75%" border="0">
                                <tr runat="server" id="SelectionRow">
                                    <td valign="top" align="left">
                                        <table id="table17" cellspacing="0" cellpadding="0" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <div id="Suche1" style="border-style: groove; border-width: medium; background: #FFFFCC;">
                                                        <table id="table18" cellspacing="1" cellpadding="1" width="50%">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Standort:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="500px" ID="ddlStandort" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Absender:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="500px" ID="ddlAbsender" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style5">
                                                                    Liefermonat:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="200px" ID="ddlLiefermonat" runat="server" AutoPostBack="True"
                                                                        Style="height: 22px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td nowrap>
                                                                    Hersteller:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="200px" ID="ddlHersteller" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td nowrap>
                                                                    Typ:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="200px" ID="ddlTyp" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Farbe
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="200px" ID="ddlFarbe" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td nowrap>
                                                                    Bereifung:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="150px" ID="ddlBereifung" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style5">
                                                                    Getriebe:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="150px" ID="ddlGetriebe" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style5">
                                                                    Kraftstoffart:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="150px" ID="ddlKraftstoffart" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style5">
                                                                    Navi:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList Width="150px" ID="ddlNavi" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="lb_Create" runat="server" CssClass="StandardButton">Weiter</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" class="style2">
                            <asp:Label ID="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:Label>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
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
