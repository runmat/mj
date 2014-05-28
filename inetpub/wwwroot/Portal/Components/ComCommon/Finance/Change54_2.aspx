<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../PageElements/BusyIndicator.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change54_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change54_2" %>

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
    
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

    <form id="Form1" method="post" runat="server">
      <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lb_Weiter" OnClientClick="Show_BusyBox1();" Text="Weiter" runat="server"
                                            CssClass="StandardButton"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge" >
                                                    <p>
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                            ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                                    <p>
                                                        <asp:Label ID="label1" runat="server" EnableViewState="False"></asp:Label></p>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" 
                                                        ImageUrl="../../../Images/excel.gif" Visible="False" Width="20px" />
                                                    <span lang="de">&nbsp;</span>Ergebnisse/Seite<span lang="de">:</span> &nbsp;<asp:DropDownList
                                                        ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400"
                                            Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="EQUNR"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxaq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelx1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Ordernummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ordernummer" runat="server" CommandName="Sort" CommandArgument="Ordernummer">col_Ordernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ordernummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Kontingentart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kontingentart" runat="server" CommandName="Sort" CommandArgument="Kontingentart">col_Kontingentart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Finanzierungsart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Finanzierungsart" runat="server" CommandName="Sort" CommandArgument="Finanzierungsart">col_Finanzierungsart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Finanzierungsart") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Abrufgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abrufgrund" CommandArgument="Abrufgrund" CommandName="Sort"
                                                            runat="server">col_Abrufgrund</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2d" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abrufgrund") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Versanddatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versanddatum" CommandArgument="Versanddatum" CommandName="Sort"
                                                            runat="server">col_Versanddatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2y" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Aktion">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Aktion" CommandArgument="Aktion" CommandName="Sort" runat="server">col_Aktion</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlAktion" Enabled="false" AutoPostBack="true" runat="server">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Aendern">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Aendern" runat="server">col_Aendern</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                            <asp:LinkButton Visible="true" ID="lbAendern" CssClass="StandardButtonTable" CommandName="Aendern"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' runat="server">Ändern</asp:LinkButton>
                                                            <asp:Label Visible="false" ID="lblMessage" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                              
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3"  valign="top" align="left">
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
