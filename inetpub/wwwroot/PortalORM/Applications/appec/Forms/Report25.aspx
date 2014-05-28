<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report25.aspx.vb" Inherits="AppEC.Report25" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body style="margin-left:0; margin-top:0;" ms_positioning="FlowLayout">
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
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            Bitte geben Sie die Auswahlkriterien ein.
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <asp:Calendar ID="calVon" runat="server" Visible="False" Width="120px" BorderStyle="Solid"
                                BorderColor="Black" CellPadding="0">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                            <asp:Calendar ID="calBis" runat="server" Visible="False" Width="120px" BorderStyle="Solid"
                                BorderColor="Black" CellPadding="0">
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
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="TextLarge" valign="top" align="left">
                                                    <table class="BorderLeftBottom" id="Table5" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap;">
                                                            </td>
                                                            <td style="white-space: nowrap;">
                                                                Eingangsdatum von:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEingangsdatumVon" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="btnVon" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                Eingangsdatum bis:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEingangsdatumBis" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="btnBis" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen...</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="cmdDetails" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Detailanzeige&nbsp;&#187;</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="rowResultate" runat="server">
                                                <td class="TextLarge" valign="top" align="left">
                                                    <asp:Label ID="lblResults" runat="server" style="padding-left:2px; float:left;"></asp:Label>                                                    
                                                    <asp:ImageButton ID="lnkExcel" ImageUrl="~/Images/excel.gif" runat="server" Height="20px"
                                                        Width="20px" ImageAlign="Top" Visible="false" style="margin-bottom:4px; margin-right:4px; float:right;"></asp:ImageButton>
                                                    <br />
                                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="744px" AutoGenerateColumns="False"
                                                        AllowSorting="True" AllowPaging="True">
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Anzahl" SortExpression="Anzahl" HeaderText="Anzahl">
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                            HorizontalAlign="Center" CssClass="TextExtraLarge" Wrap="False"></PagerStyle>
                                                    </asp:DataGrid>
                                                </td>
                                                <td width="100%"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
