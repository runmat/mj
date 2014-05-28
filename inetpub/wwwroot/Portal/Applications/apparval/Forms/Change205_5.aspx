<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change205_5.aspx.vb" Inherits="AppARVAL.Change205_5" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>--%>
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
    <table cellspacing="0" cellpadding="2" width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Calendar ID="calZul" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid"
                                Width="120px" Visible="False">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;<asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx"
                                            Visible="False">Fahrzeugsuche</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3" height="41">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge" align="left" width="618" height="9">
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                                    <td nowrap align="right" height="9">
                                                        <p align="right">
                                                            &nbsp;
                                                            <asp:DropDownList ID="ddlPageSize" runat="server" Height="14px" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </p>
                                                    </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False"
                                            AllowPaging="True" AllowSorting="True" bodyHeight="400" CssClass="tableMain"
                                            bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="id" SortExpression="id" HeaderText="ID">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Erstellt" SortExpression="Erstellt" HeaderText="Erstellt">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="Equipment" SortExpression="Equipment"
                                                    HeaderText="Equi"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Fahrgestellnr" SortExpression="Fahrgestellnr" HeaderText="Fahrg.Nr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Lvnr" SortExpression="Lvnr" HeaderText="LV-Nr."></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Versandadresse" SortExpression="Versandadresse" HeaderText="Versandadr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="VersandadresseName1" SortExpression="VersandadresseName1"
                                                    HeaderText="VersandadresseName1"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="VersandadresseName2" SortExpression="VersandadresseName2"
                                                    HeaderText="VersandadresseName2"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="VersandadresseStr" SortExpression="VersandadresseStr"
                                                    HeaderText="VersandadresseStr"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="VersandadresseNr" SortExpression="VersandadresseNr"
                                                    HeaderText="VersandadresseNr"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="VersandadressePlz" SortExpression="VersandadressePlz"
                                                    HeaderText="VersandadressePlz"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="VersandadresseOrt" SortExpression="VersandadresseOrt"
                                                    HeaderText="VersandadresseOrt"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="VersandadresseLand" HeaderText="VersandadresseLand" SortExpression="VersandadresseLand"
                                                    Visible="False"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="Haendlernummer" SortExpression="Haendlernummer"
                                                    HeaderText="H&#228;nderlnummer"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="Kennzeichen" SortExpression="Kennzeichen"
                                                    HeaderText="Kennzeichen"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="TIDNr" SortExpression="TIDNr" HeaderText="TIDNr">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="LIZNr" SortExpression="LIZNr" HeaderText="LIZNr">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="Materialnummer" SortExpression="Materialnummer"
                                                    HeaderText="Materialnummer"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="VersandartShow" SortExpression="VersandartShow" HeaderText="Versandart">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnFreigeben" runat="server" CssClass="StandardButtonTable" Width="100px"
                                                            Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>'
                                                            CommandName="Freigeben">Freigeben</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="StandardButtonTable" Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>'
                                                            CommandName="delete">Storno</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"
                                                Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <!--#include File="../../../PageElements/Footer.html" -->
                            <br>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
