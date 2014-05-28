<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report200.aspx.vb"
    Inherits="AppArval.Report200" MasterPageFile="../../../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                            </h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            bitte geben Sie ihre Suchkriterien ein
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td colspan="3" class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Durchführungsdatum&nbsp;von:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAbmeldedatumVon" runat="server"></asp:TextBox>&nbsp;(TT.MM.JJJJ)&nbsp;
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
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnCal1" runat="server"><img alt="Kalender"   src="../../../Images/calendar.jpg"  /></asp:LinkButton>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Durchführungsdatum&nbsp;bis:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAbmeldedatumBis" runat="server"></asp:TextBox>&nbsp;(TT.MM.JJJJ)&nbsp;
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
                                        <td>
                                            <asp:LinkButton ID="btnCal2" runat="server"><img alt="Kalender"   src="../../../Images/calendar.jpg"  /></asp:LinkButton>
                                        </td>
                                        <td width="100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" Visible="False"></asp:TextBox><asp:TextBox
                                                ID="txtKennzeichen" runat="server" Visible="False"></asp:TextBox>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdCreate" Text="Erstellen" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
