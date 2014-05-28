<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report203_02.aspx.vb"
    Inherits="AppARVAL.Report203_02" %>

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
    <table height="698" width="993" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server" Visible="False"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" width="991" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                            <asp:Label ID="lblPageTitle" runat="server"> Detaildaten</asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" align="right" width="991" colspan="2">
                            &nbsp;
                            <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()"
                                CssClass="TaskTitle">Fenster schließen</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top" width="991">
                            <table id="Table6" cellspacing="0" cellpadding="5" width="100%" border="0">
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table id="Table13" height="81" cellspacing="0" cellpadding="5" bgcolor="white" border="0">
                                            <tr id="Tr5" runat="server">
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label38" runat="server" CssClass="DetailTableFont">Kundennr:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" width="312" colspan="3">
                                                    <asp:Label ID="lblKunnr" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size="" Width="165px"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="126">
                                                    <asp:Label ID="Label22" runat="server" CssClass="DetailTableFont" Font-Size="">Vollmacht:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="111">
                                                    <asp:Label ID="lblVollmacht" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="107">
                                                    <asp:Label ID="Label21" runat="server" CssClass="DetailTableFont" Font-Size=""> Register:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" width="218" colspan="2" height="26">
                                                    <asp:Label ID="lblRegister" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label10" runat="server" CssClass="DetailTableFont">Halter:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="312" colspan="3">
                                                    <asp:Label ID="lblHalter" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Width="297px"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="126">
                                                    <asp:Label ID="Label1" runat="server" CssClass="DetailTableFont" Font-Size="">Gewerbeanmeld.:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="111">
                                                    <asp:Label ID="lblGewerbe" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="107">
                                                    <asp:Label ID="Label2" runat="server" CssClass="DetailTableFont" Font-Size="">Einzugserm.:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" width="217" colspan="2" height="26">
                                                    <asp:Label ID="lblEinzug" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label37" runat="server" CssClass="DetailTableFont"> HOrt:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="312" colspan="3">
                                                    <asp:Label ID="lblHOrt" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size="" Width="218px"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="126">
                                                    <asp:Label ID="Label4" runat="server" CssClass="DetailTableFont" Font-Size="">Vollst.:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="111">
                                                    <asp:Label ID="lblVollst" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="107">
                                                    <asp:Label ID="Label23" runat="server" CssClass="DetailTableFont" Font-Size="">Personalausweis:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" width="217" colspan="2" height="26">
                                                    <asp:Label ID="lblPerso" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size="" Width="143px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td valign="top" nowrap align="left" width="312" colspan="3">
                                                    <asp:Label ID="lblKUNNR_SAP" runat="server" Visible="False">lblKUNNR_SAP</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="126">
                                                    <asp:Label ID="Label3" runat="server" CssClass="DetailTableFont" Font-Size="">Versich.Bestätigung:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="111">
                                                    <asp:Label ID="lblKarte" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="107">
                                                </td>
                                                <td valign="top" nowrap align="left" width="217" colspan="2" height="26">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="Table10" height="333" cellspacing="0" cellpadding="5" bgcolor="white"
                                            border="0">
                                            <tr>
                                                <td class="TaskTitle" valign="top" nowrap align="left" width="245" height="1">
                                                </td>
                                                <td class="TaskTitle" valign="top" nowrap align="left" width="604" colspan="3" height="1">
                                                </td>
                                                <td class="TaskTitle" valign="top" nowrap align="left" width="115" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="245" height="13">
                                                    <asp:Label ID="Label12" runat="server" CssClass="DetailTableFont" Font-Size="" Width="170px">Ausstellungsdatum Vollmacht:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="604" colspan="3" height="13">
                                                    <asp:Label ID="lblDateVollm" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="115" height="13">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="245" height="24">
                                                    <asp:Label ID="Label18" runat="server" CssClass="DetailTableFont" Font-Size="" Width="225px">Beschafftungsdatum Handelsregister/ Gewerbeanmeldung:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" width="604" colspan="3" height="24">
                                                    <asp:Label ID="lbl_DateGew" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" width="115" height="24">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="" valign="top" nowrap align="left" width="245" height="26">
                                                    <asp:Label ID="Label27" runat="server" CssClass="DetailTableFont" Font-Size="" Width="203px">neue Beschaffung der Vollmacht/Registerauszüge am:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" width="604" colspan="3">
                                                    <asp:Label ID="lblVollmRegDate" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="115">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="245" height="1">
                                                    <asp:Label ID="Label8" runat="server" CssClass="DetailTableFont" Font-Size="" Width="150px">Besonderheiten Kunde:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="604" colspan="3" height="1">
                                                    <asp:Label ID="lblBemerk" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size="" Width="578px" Height="14px"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="115" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TaskTitle" valign="top" nowrap align="left" width="245" height="1">
                                                </td>
                                                <td class="TaskTitle" valign="top" nowrap align="left" width="604" colspan="3" height="1">
                                                </td>
                                                <td class="TaskTitle" valign="top" nowrap align="left" width="115" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="245" height="1">
                                                    <asp:Label ID="Label7" runat="server" CssClass="DetailTableFont" Font-Size=""> Nummer der Dauer-EVB:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="604" colspan="3" height="1">
                                                    <asp:TextBox ID="txt_NummerEVB" runat="server" Width="100px" MaxLength="7"></asp:TextBox>
                                                    <asp:Label ID="lblEVB" runat="server" Visible="False" CssClass="DetailTableFont"
                                                        Font-Size="" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="115" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="245" height="1">
                                                    <asp:Label ID="Label24" runat="server" CssClass="DetailTableFont" Font-Size="">Datum gültig ab:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="604" colspan="3" height="1">
                                                    <asp:TextBox ID="txtDatumvon" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Label ID="lblvon" runat="server" Visible="False" CssClass="DetailTableFont"
                                                        Font-Size="" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lblDateForm1" runat="server" Visible="False" CssClass="DetailTableFont"
                                                        Font-Size="" Font-Bold="True">(TT.MM.JJJJ)</asp:Label>&nbsp;
                                                    <asp:LinkButton ID="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                </td>
                                                <td valign="top" nowrap align="left" width="115" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="245" height="1">
                                                    <asp:Label ID="Label26" runat="server" CssClass="DetailTableFont" Font-Size="">Datum gültig bis:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="604" colspan="3" height="1">
                                                    <asp:TextBox ID="txtDatumbis" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Label ID="lblBis" runat="server" Visible="False" CssClass="DetailTableFont"
                                                        Font-Size="" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lblDateForm2" runat="server" CssClass="DetailTableFont" Font-Size=""
                                                        Font-Bold="True" Visible="False">(TT.MM.JJJJ)</asp:Label>&nbsp;
                                                    <asp:LinkButton ID="btnCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                </td>
                                                <td valign="top" nowrap align="left" width="115" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="245" height="1">
                                                </td>
                                                <td valign="top" nowrap align="left" width="604" colspan="3" height="1">
                                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Speichern</asp:LinkButton>
                                                </td>
                                                <td valign="top" nowrap align="left" width="115" height="1">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td width="990">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" Font-Size="" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td width="990">
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
