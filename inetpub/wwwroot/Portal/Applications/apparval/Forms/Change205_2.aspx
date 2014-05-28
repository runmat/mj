<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change205_2.aspx.vb" Inherits="AppARVAL.Change205_2" %>

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
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:Label>
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
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
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
                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:HyperLink>
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
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>&nbsp;
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
                                                <asp:BoundColumn Visible="False" DataField="Equnr" SortExpression="Equnr" HeaderText="Equipment">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Anfordern">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0000" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Leasingvertrags-Nr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="Kfz-Briefnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kfz-Kennzeichen">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1"
                                                    HeaderText="Ordernummer"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="STATUS" SortExpression="STATUS" HeaderText="Status">
                                                </asp:BoundColumn>
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
                            * Dieser Brief wurde bereits angefordert und befindet sich in der Autorisierung
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
