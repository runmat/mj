<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change08_4.aspx.vb" Inherits="CKG.Components.ComCommon.Change08_4" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>--%>
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
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td colspan="3">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Bestätigung)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <p>
                                            <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:LinkButton><u></u></p>
                                    </td>
                                </tr>
                            </table>
                            <p align="right">
                                &nbsp;</p>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:HyperLink>&nbsp;<asp:HyperLink
                                                ID="lnkAdressAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_3.aspx">Adressauswahl</asp:HyperLink><asp:Label
                                                    ID="lblAddress" runat="server" Visible="False"></asp:Label><asp:Label ID="lblMaterialNummer"
                                                        runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge" valign="top" align="left">
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="Table7" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr id="tr_VersandAdresse" runat="server">
                                                <td class="" valign="top" nowrap>
                                                    <asp:Label ID="lbl_VersandAdresse" runat="server" Font-Underline="True">lbl_VersandAdresse</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" align="left" colspan="2">
                                                    <asp:Label ID="lblXVersand" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr_Versandart" runat="server">
                                                <td class="" valign="top" nowrap>
                                                    <asp:Label ID="lbl_Versandart" runat="server" Font-Underline="True">lbl_Versandart</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" align="left" colspan="2">
                                                    <asp:Label ID="lblXVersandartData" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr_Versandgrund" runat="server">
                                                <td class="" valign="top" nowrap>
                                                    <asp:Label ID="lbl_Versandgrund" runat="server" Font-Underline="True">lbl_Versandgrund</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" align="left" colspan="2">
                                                    <asp:Label ID="lblXVersandGrundData" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr_Fahrzeuge" runat="server">
                                                <td class="StandardTableAlternate" valign="top">
                                                    <p>
                                                        <asp:Label ID="lbl_Fahrzeuge" runat="server">lbl_Fahrzeuge</asp:Label></p>
                                                    <p>
                                                        &nbsp;</p>
                                                    <p>
                                                        &nbsp;</p>
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" align="left" colspan="2">
                                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        Width="100%" bodyHeight="250" CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader">
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Leasingvertragsnummer">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Leasingvertragsnummer" runat="server" CommandName="Sort"
                                                                        CommandArgument="LIZNR">col_Leasingvertragsnummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Ordernummer">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Ordernummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Ordernummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn SortExpression="STATUS" HeaderText="col_Status">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STATUS") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn Visible="False" HeaderText="Anfordern">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk0001" runat="server" Enabled="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                            PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <p align="left">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <!--#include File="../../../PageElements/Footer.html" -->
                                        <p align="left">
                                            &nbsp;</p>
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
