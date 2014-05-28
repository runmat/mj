<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02_2.aspx.vb" Inherits="AppF1.Report02_2" %>

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
                                        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="javascript:history.back()">zurück</asp:HyperLink>
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
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
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
                                                    <strong>
                                                        <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />
                                                        <asp:LinkButton CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton></strong>&nbsp;Ergebnisse/Seite:&nbsp;
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True"
                                            bodyHeight="400" CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader"
                                            PageSize="50" BackColor="White" AutoGenerateColumns="False">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                HorizontalAlign="Left" Position="Top" CssClass="TextExtraLarge" Wrap="False"
                                                Mode="NumericPages"></PagerStyle>
                                            <Columns>
                                            
                                            <asp:TemplateColumn SortExpression="Händlernummer" HeaderText="col_Händlernummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Händlernummer" CommandName="sort" CommandArgument="Händlernummer"
                                                            runat="server">col_Händlernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yqxdsdfwqwaa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Händlernummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                                                           
                                                <asp:TemplateColumn SortExpression="Finanzierungsnummer" HeaderText="col_Finanzierungsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Finanzierungsnummer" CommandName="sort" CommandArgument="Finanzierungsnummer"
                                                            runat="server">col_Finanzierungsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yqxdwqwaa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Finanzierungsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" CommandName="sort" CommandArgument="Fahrgestellnummer"
                                                            runat="server">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ZBII-Nummer" SortExpression="ZBII-Nummer" HeaderText="Nummer ZBII">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Ort" SortExpression="Ort" HeaderText="Ort"></asp:BoundColumn>
                                               
                                                <asp:BoundColumn DataFormatString="{0:d}" DataField="Versanddatum" SortExpression="Versanddatum"
                                                    HeaderText="Versanddatum"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Abrufgrund" SortExpression="Abrufgrund" HeaderText="Abrufgrund">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Anforderungsdatum" SortExpression="Anforderungsdatum"
                                                    HeaderText="Anforderungsdatum" DataFormatString="{0:d}"></asp:BoundColumn>
                                                
                                                <asp:TemplateColumn SortExpression="text" HeaderText="col_text">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_text" CommandName="sort" CommandArgument="text" runat="server">col_text</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelcya1yqxdwqwa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.text") %>'>
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
