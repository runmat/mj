<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Input_002_01.aspx.vb"
    Inherits="AppASL.Input_002_01" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server">(Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" nowrap colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td class="TextHeader">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton" Width="130px">Report erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Calendar ID="calDatum" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid"
                                Width="120px" Visible="False">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                            <asp:TextBox ID="txtField" runat="server" Width="50px" Visible="False">0</asp:TextBox>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="" valign="top">
                                        <table id="Table1" cellspacing="2" cellpadding="1" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="" valign="middle" nowrap width="143">
                                                    Amtl. Kennzeichen
                                                </td>
                                                <td class="" valign="middle" nowrap>
                                                    <asp:TextBox ID="txtKennzeichenVon" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td class="" valign="middle" width="100%">
                                                    <asp:TextBox ID="txtKennzeichenBis" runat="server" Width="100px" Visible="False"></asp:TextBox>&nbsp;(M-X1000)&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="" valign="middle" width="143" height="17">
                                                    Fahrgestellnummer
                                                </td>
                                                <td class="" valign="middle" nowrap height="17">
                                                    <asp:TextBox ID="txtFahrgestellVon" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td class="" valign="middle" width="100%" height="17">
                                                    <asp:TextBox ID="txtFahrgestellBis" runat="server" Width="100px" Visible="False"></asp:TextBox>&nbsp;(WDX11111111111111)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="" valign="middle" width="143">
                                                    Leasingvertragsnr.
                                                </td>
                                                <td class="" valign="middle">
                                                    <asp:TextBox ID="txtLeasVVon" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td class="" valign="middle" width="100%">
                                                    <asp:TextBox ID="txtLeasVBis" runat="server" Width="100px" Visible="False"></asp:TextBox>&nbsp;(1000000012)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="143">
                                                    Kundennummer
                                                </td>
                                                <td valign="middle">
                                                    <asp:TextBox ID="txtKundennr" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td valign="middle" width="100%">
                                                    &nbsp;(23632)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="143">
                                                    Konzernnummer
                                                </td>
                                                <td valign="middle">
                                                    <asp:TextBox ID="txtKonzernnr" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td valign="middle" width="100%">
                                                    &nbsp;(12345)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top" nowrap colspan="3">
                                                    <hr width="100%" size="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" width="143">
                                                    <asp:RadioButtonList ID="rbMahnung" runat="server" Visible="False">
                                                        <asp:ListItem Value="MALL" Selected="True">Alle</asp:ListItem>
                                                        <asp:ListItem Value="M1LN">Stufe 1 LN</asp:ListItem>
                                                        <asp:ListItem Value="M2LN">Stufe 2 LN</asp:ListItem>
                                                        <asp:ListItem Value="M3LN">Stufe 3 LN</asp:ListItem>
                                                        <asp:ListItem Value="M4LN">Stufe 4 LN</asp:ListItem>
                                                        <asp:ListItem Value="M1VG">Stufe 1 VG</asp:ListItem>
                                                        <asp:ListItem Value="M2VG">Stufe 2 VG</asp:ListItem>
                                                        <asp:ListItem Value="M3VG">Stufe 3 VG</asp:ListItem>
                                                        <asp:ListItem Value="M4VG">Stufe 4 VG</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:RadioButtonList ID="rbSelect" runat="server" Visible="False" Height="49px">
                                                        <asp:ListItem Value="H" Selected="True">Historie</asp:ListItem>
                                                        <asp:ListItem Value="M">Mahnstufen</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:CheckBox ID="lblKF" runat="server" Visible="False" Text="nur Kl�rf�lle"></asp:CheckBox>
                                                </td>
                                                <td id="rbAuswahl" runat="server" visible="False" valign="top" nowrap width="200"
                                                    colspan="2">
                                                    <asp:RadioButtonList ID="rbStatus" runat="server">
                                                        <asp:ListItem Value="ALL" Selected="True">Alle</asp:ListItem>
                                                        <asp:ListItem Value="DAD1">Angelegt beim DAD</asp:ListItem>
                                                        <asp:ListItem Value="LEA1">Beim LN</asp:ListItem>
                                                        <asp:ListItem Value="DAD2">Zur&#252;ck vom LN</asp:ListItem>
                                                        <asp:ListItem Value="VER1">Bei Versicherung</asp:ListItem>
                                                        <asp:ListItem Value="DAD3">Zur&#252;ck von Versicherung</asp:ListItem>
                                                        <asp:ListItem Value="DAD4">Leasingende &#252;berschritten</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
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
                        <td valign="top" width="111">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="111">
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
