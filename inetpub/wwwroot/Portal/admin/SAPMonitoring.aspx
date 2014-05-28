<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SAPMonitoring.aspx.vb" Inherits="CKG.Admin.SAPMonitoring" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server">Administration</asp:Label><asp:Label ID="lblPageTitle"
                                runat="server">  (Datenselektion)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table5" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            &nbsp;
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="TblSearch" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                <tr id="trSearch" runat="server">
                                    <td align="left">
                                        <table bgcolor="white" border="0">
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    ab Datum:
                                                </td>
                                                <td valign="bottom" width="160">
                                                    <asp:TextBox ID="txtAbDatum" runat="server" Width="130px"></asp:TextBox><asp:Button
                                                        ID="btnOpenSelectAb" runat="server" Width="30px" CausesValidation="False" Text="..."
                                                        Height="22px"></asp:Button>
                                                </td>
                                                <td valign="bottom">
                                                    <asp:Calendar ID="calAbDatum" runat="server" Width="160px" CellPadding="0" BorderColor="Black"
                                                        BorderStyle="Solid" Visible="False">
                                                        <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                                        <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                                        <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                                        <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                                        <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                                        <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                                    </asp:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    bis Datum:
                                                </td>
                                                <td valign="bottom" width="160">
                                                    <asp:TextBox ID="txtBisDatum" runat="server" Width="130px"></asp:TextBox><asp:Button
                                                        ID="btnOpenSelectBis" runat="server" Width="30px" CausesValidation="False" Text="..."
                                                        Height="22px"></asp:Button>
                                                </td>
                                                <td valign="bottom">
                                                    <asp:Calendar ID="calBisDatum" runat="server" Width="160px" CellPadding="0" BorderColor="Black"
                                                        BorderStyle="Solid" Visible="False">
                                                        <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                                        <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                                        <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                                        <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                                        <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                                        <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                                    </asp:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    Kriterium
                                                </td>
                                                <td valign="bottom" width="160" colspan="2">
                                                    <asp:RadioButton ID="rbBAPI" runat="server" Text="BAPI" GroupName="grpKriterium"
                                                        AutoPostBack="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:RadioButton ID="rbASPX" runat="server" Text="ASPX-Seite" Checked="True" GroupName="grpKriterium"
                                                        AutoPostBack="True"></asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" width="100">
                                                    Auswahl:
                                                </td>
                                                <td valign="bottom" width="160" colspan="2">
                                                    <asp:DropDownList ID="ddlAuswahl" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                                    <table id="TblLog" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                        <tr>
                                            <td class="TaskTitle" valign="top">
                                                &nbsp;
                                                <asp:Label ID="Label2" runat="server"> Datenanzeige</asp:Label>
                                            </td>
                                        </tr>
										<tr>
											<td><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></td>
										</tr>
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" CellPadding="0" BackColor="White"
                                                    AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Testbenutzer">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="BAPI" SortExpression="BAPI" HeaderText="BAPI"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Parameter">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Start" SortExpression="Start" HeaderText="Start"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Ende" SortExpression="Ende" HeaderText="Ende"></asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="Dauer" HeaderText="Dauer">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer"))) %>'
                                                                    Height="10px" BackColor="#8080FF">
                                                                </asp:Label>
                                                                <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="Erfolg" HeaderText="Erfolg">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox2" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Erfolg") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Fehlermeldung" SortExpression="Fehlermeldung" HeaderText="Fehlermeldung">
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    <PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;"
                                                        Mode="NumericPages"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
										<tr>
											<td>
                                                <DBWC:HierarGrid ID="HGZ" runat="server" Width="100%" BorderStyle="None" BorderColor="#999999"
                                                    CellPadding="0" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                    TemplateDataMode="Table" LoadControlMode="UserControl" TemplateCachingBase="Tablename"
                                                    BorderWidth="1px" AllowSorting="True">
                                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="GridTableItem"></ItemStyle>
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Seite" SortExpression="Seite" HeaderText="Seite"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Anwendung" SortExpression="Anwendung" HeaderText="Anwendung">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Zugriffe SAP" SortExpression="Zugriffe SAP" HeaderText="Zugriffe&lt;br&gt;SAP">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kundennr.">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Customername" SortExpression="Customername" HeaderText="Kundenname">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AccountingArea" SortExpression="AccountingArea" HeaderText="AccountingArea">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Testbenutzer">
                                                            <HeaderStyle Width="100px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Checkbox4" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="Start ASPX" HeaderText="Start ASPX">
                                                            <HeaderStyle Width="150px"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="Hyperlink4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Start ASPX") %>'
                                                                    ToolTip='<%# DataBinder.Eval(Container, "DataItem.Ende ASPX") %>'>
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="Dauer ASPX" HeaderText="Dauer">
                                                            <HeaderStyle HorizontalAlign="Center" Width="205px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            <ItemTemplate>
                                                                <table id="Table18" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="50">
                                                                            ASPX
                                                                        </td>
                                                                        <td width="30" align="right">
                                                                            <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer ASPX") %>'>
                                                                            </asp:Label>&nbsp;
                                                                        </td>
                                                                        <td width="125">
                                                                            <asp:Label ID="Label5" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer ASPX"))) %>'
                                                                                Height="10px" BackColor="#8080FF">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="25">
                                                                            SAP
                                                                        </td>
                                                                        <td width="30" align="right">
                                                                            <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer SAP") %>'>
                                                                            </asp:Label>&nbsp;
                                                                        </td>
                                                                        <td width="125">
                                                                            <asp:Label ID="Label7" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer SAP"))) %>'
                                                                                Height="10px" BackColor="Highlight">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </DBWC:HierarGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td width="120">
                                    &nbsp;
                                </td>
                                <td valign="top" align="left">
                                    <asp:Label ID="lblError" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="120">
                                    &nbsp;
                                </td>
                                <td valign="top" align="left">
                                    <!--#include File="../PageElements/Footer.html" -->
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>
