<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report23.aspx.vb" Inherits="AppEC.Report23" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
                        <td class="PageNavigation" colspan="2" height="19">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2" height="19">
                            Bitte geben Sie die Auswahlkriterien ein.
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <asp:Calendar ID="CalStillVon" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid"
                                Width="120px" Visible="False">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                            <asp:Calendar ID="CalStillBis" runat="server" Visible="False" Width="120px" BorderStyle="Solid"
                                BorderColor="Black" CellPadding="0">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                           <asp:Calendar ID="CalMeldungVon" runat="server" BorderStyle="Solid" BorderColor="Black"
                                CellPadding="0" Width="120px" Visible="False">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                            <asp:Calendar ID="calMeldungBis" runat="server" BorderStyle="Solid" BorderColor="Black"
                                CellPadding="0" Width="120px" Visible="False">
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
                                    <td class="" valign="top">
                                        <table class="BorderLeftBottom" id="Table5" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td nowrap>
                                                    &nbsp;
                                                </td>
                                                <td nowrap colspan="3">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    Meldungsdatum Von:</td>
                                                <td>
                                                    <asp:TextBox ID="txtMeldungVon" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                    <asp:Label ID="lblInputReq0" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnAbmeldVon" runat="server" CssClass="StandardButtonTable" 
                                                        Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    Meldungsdatum Bis:</td>
                                                <td>
                                                    <asp:TextBox ID="txtMeldungBis" runat="server" ToolTip="Zulaufdatum Bis" 
                                                        Height="22px"></asp:TextBox>
                                                    <asp:Label ID="Label3" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnAbmeldBis" runat="server" CssClass="StandardButtonTable" 
                                                        Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>   
                                            <tr>
                                                <td nowrap>
                                                </td>
                                                <td nowrap>
                                                    Stilllegungsdatum von:
                                                </td>
                                                <td nowrap>
                                                    <asp:TextBox ID="txtStilllegungVon" runat="server"></asp:TextBox>
                                                    <asp:Label ID="lblInputReq" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td nowrap>
                                                    <asp:LinkButton ID="lbStilllegungVon" runat="server" CssClass="StandardButtonTable" Width="100px">&#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap>
                                                </td>
                                                <td nowrap>
                                                    Stilllegungsdatum bis:
                                                </td>
                                                <td nowrap>
                                                    <asp:TextBox ID="txtStilllegungBis" runat="server"></asp:TextBox>
                                                    <asp:Label ID="Label1" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td nowrap>
                                                    <asp:LinkButton ID="lbStilllegungBis" runat="server" CssClass="StandardButtonTable" Width="100px">&#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>                                                                                     
                                            <tr>
                                                <td nowrap>
                                                    &nbsp;
                                                </td>
                                                <td nowrap>
                                                </td>
                                                <td nowrap>
                                                </td>
                                                <td nowrap>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap>
                                                </td>
                                                <td nowrap>
                                                </td>
                                                <td nowrap>
                                                </td>
                                                <td nowrap>
                                                    <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="Label2" runat="server" CssClass="TextError">*</asp:Label>
                                        <asp:Label ID="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False">Format: TT.MM.JJJJ.</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
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
