<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_4.aspx.vb" Inherits="AppBPLG.Change02_4" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td colspan="3">
                <uc1:header id="ucHeader" runat="server">
                </uc1:header>
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
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            &nbsp;
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" 
                                            NavigateUrl="Change02.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change02_2.aspx"
                                            Visible="False">Fahrzeugauswahl</asp:HyperLink>&nbsp;<asp:HyperLink ID="lnkAdressAuswahl"
                                                runat="server" CssClass="TaskTitle" NavigateUrl="Change02_3.aspx" 
                                            Visible="False">Adressauswahl</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <uc1:kopfdaten id="Kopfdaten1" runat="server">
                                        </uc1:kopfdaten>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                  <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <strong><i>
                                            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label></i>
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <br />
                                        <table class="TableKontingent" id="Table7" cellspacing="0" cellpadding="5" width="100%"
                                            bgcolor="white" border="0">
                                            <tr>
                                                <td class="StandardTableAlternate" valign="top">
                                                    Versandart:
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" align="left" width="100%" colspan="2">
                                                    <asp:Label ID="lblVersandart" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblVersandhinweis"
                                                        runat="server" Visible="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" valign="top">
                                                    Adresse:
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" align="left" colspan="2">
                                                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody"
                                            CssClass="tableMain" bodyHeight="400" Width="100%" AutoGenerateColumns="False"
                                            AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Kontonummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Kontonummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" NAME="Label1" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'
                                                            ID="Label2" NAME="Label2">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'
                                                            ID="Label3" NAME="Label3">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'
                                                            ID="Label4" NAME="Label4">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM"
                                                            CommandName="Sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" NAME="Label5" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="Bezahlt">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Bezahlt" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="CoC">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_COC" runat="server" CommandArgument="LICENSE_NUM" CommandName="Sort">col_COC</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Checkbox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="MANDT" HeaderText="Versandart">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemp2" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="1" %>'>Temporär</asp:Label>
                                                        <asp:Label ID="lblEndg" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="2" %>'>Endgültig</asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="AUGRU" HeaderText="Abrufgrund">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAUGRU" runat="server" NAME="Label6" ToolTip='<%# DataBinder.Eval(Container, "DataItem.ANFNR") %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.AUGRU_Klartext") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="VBELN" SortExpression="VBELN" HeaderText="Auftragsnr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="COMMENT" SortExpression="COMMENT" HeaderText="Kommentar">
                                                </asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <!--#include File="../../../PageElements/Footer.html" -->
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
