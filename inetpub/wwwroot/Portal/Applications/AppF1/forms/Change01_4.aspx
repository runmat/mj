<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_4.aspx.vb" Inherits="AppF1.Change01_4" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>


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
    
    <uc1:BusyIndicator runat="server" />

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
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Bestätigung)</asp:Label>
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
                                        <asp:LinkButton ID="cmdSave" OnClientClick="Show_BusyBox1();" runat="server" CssClass="StandardButton">&#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx" CssClass="TaskTitle"
                                            Visible="False">Fahrzeugauswahl</asp:HyperLink>&nbsp;<asp:HyperLink ID="lnkAdressAuswahl"
                                                runat="server" NavigateUrl="Change04_3.aspx" CssClass="TaskTitle" Visible="False">Adressauswahl</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge" valign="top" align="left">
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="Table7" cellspacing="0" cellpadding="5" width="100%" border="0" bgcolor="white"
                                            class="TableKontingent">
                                            <tr>
                                                <td class="StandardTableAlternate" valign="top">
                                                    Zustellart:
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" align="left" colspan="2" width="100%">
                                                    <asp:Label ID="lblVersandart" runat="server"></asp:Label><asp:Label ID="lblMaterialNummer"
                                                        runat="server" Visible="False"></asp:Label><asp:Label ID="lblVersandhinweis" runat="server"
                                                            Visible="False"> - Gilt nicht für gesperrt angelegte Anforderungen!</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top">
                                                    Adresse:
                                                </td>
                                                <td class="TextLarge" valign="top" align="left" colspan="2">
                                                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody"
                                            CssClass="tableMain" bodyHeight="400" Width="100%" AutoGenerateColumns="False"
                                            AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="col_Entfernen" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Entfernen" runat="server">col_Entfernen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton Visible="true" ID="lbEntfernen" CommandName="entfernen" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                            runat="server"><img src=../../../Images/loesch.gif border=0> </asp:LinkButton>
                                                        <asp:Label Visible="false" ID="lblMessage" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnr">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnr" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Kontonummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Kontonummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'
                                                            ID="Label2">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="false" SortExpression="TEXT300" HeaderText="col_Anfragenr">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anfragenr" runat="server" CommandName="Sort" CommandArgument="TEXT300">col_Anfragenr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TEXT300") %>'
                                                            ID="Label3">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'
                                                            ID="Label4">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM"
                                                            CommandName="Sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'
                                                            ID="Label6">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZBEZAHLT" HeaderText="col_Bezahlt">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Bezahlt" runat="server" CommandArgument="ZZBEZAHLT" CommandName="Sort">col_Bezahlt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkBezahlt" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'
                                                            Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZCOCKZ" HeaderText="col_COC">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_COC" runat="server" CommandArgument="ZZCOCKZ" CommandName="Sort">col_COC</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Checkbox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Textbox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="COMMENT" HeaderText="col_Kommentar" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kommentar" runat="server" CommandArgument="COMMENT" CommandName="Sort">col_Kommentar</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.COMMENT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Temp" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Temp" runat="server">col_Temp</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0001" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Endg">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Endg" runat="server">col_Endg</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0002" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_DP" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_DP" runat="server">col_DP</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0004" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Retail" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Retail" runat="server">col_Retail</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0003" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_kfkl" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_kfkl" runat="server">col_kfkl</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0006" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="AUGRU" HeaderText="col_Abrufgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abrufgrund" runat="server">col_Abrufgrund</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label10" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.TEXT300") %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.AUGRU_Klartext") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
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
