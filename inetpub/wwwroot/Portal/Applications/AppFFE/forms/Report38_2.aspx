<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report38_2.aspx.vb" Inherits="AppFFE.Report38_2" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                        <asp:HyperLink ID="lnkBack" runat="server" 
                                            NavigateUrl="javascript:history.back()">zurück</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge">
                                                    &nbsp; &nbsp;&nbsp;
                                                </td>
                                                <td align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelExtraLarge">
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label><asp:Label ID="lblError"
                                                        runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />
                                                    <asp:LinkButton ID="lnkCreateExcel" CssClass="ExcelButton" runat="server" Visible="False"><strong>Excelformat</strong></asp:LinkButton>&nbsp;<strong>Anzahl 
                                                    Vorgänge / Seite</strong>
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="DataGrid1" runat="server" CellPadding="0" Width="100%" BackColor="White"
                                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" PageSize="50">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                HorizontalAlign="Left" Position="Top" CssClass="TextExtraLarge" Wrap="False"
                                                Mode="NumericPages"></PagerStyle>
                                            <Columns>
                                                <asp:TemplateColumn SortExpression="Lfd_Nummer" HeaderText="col_LfdNummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LfdNummer" CommandName="sort" CommandArgument="Lfd_Nummer"
                                                            runat="server">col_LfdNummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label Visible="True" runat="server" ID="Label99">
																	<%#DataBinder.Eval(Container, "DataItem.Lfd_Nummer")%>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Finanzierungsnummer" HeaderText="col_Finanzierungsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Finanzierungsnummer" CommandName="sort" CommandArgument="Finanzierungsnummer"
                                                            runat="server">col_Finanzierungsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label Visible="True" runat="server" ID="Label4">
																	<%#DataBinder.Eval(Container, "DataItem.Finanzierungsnummer")%>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZBII-Nummer" HeaderText="col_NummerZBII">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZBII" CommandName="sort" CommandArgument="ZBII-Nummer"
                                                            runat="server">col_NummerZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label Visible="True" runat="server" ID="Label71">
																	<%#DataBinder.Eval(Container, "DataItem.ZBII-Nummer")%>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Fälligkeitsdatum" HeaderText="col_Faelligkeitsdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Faelligkeitsdatum" CommandName="sort" CommandArgument="Fälligkeitsdatum"
                                                            runat="server">col_Faelligkeitsdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label Visible="True" runat="server" ID="Label7">
																	<%#DataBinder.Eval(Container, "DataItem.Fälligkeitsdatum","{0:d}")%>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Kontingentart" HeaderText="col_Kontingentart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kontingentart" CommandName="sort" CommandArgument="Kontingentart"
                                                            runat="server">col_Kontingentart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label Visible="True" runat="server" ID="Label7X">
																	<%#DataBinder.Eval(Container, "DataItem.Kontingentart")%>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Erledigt" HeaderText="col_Erledigt">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Erledigt" CommandName="sort" CommandArgument="Erledigt" runat="server">col_Erledigt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label Visible="True" runat="server" ID="Label7X2">
																	<%#DataBinder.Eval(Container, "DataItem.Erledigt", "{0:d}")%>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
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
