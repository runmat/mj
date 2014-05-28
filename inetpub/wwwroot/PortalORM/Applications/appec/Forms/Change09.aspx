<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09.aspx.vb" Inherits="AppEC.Change09" %>

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
                            <asp:Calendar ID="calVon" runat="server" BorderStyle="Solid" BorderColor="Black"
                                CellPadding="0" Width="120px" Visible="False">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                            <asp:Calendar ID="calBis" runat="server" BorderStyle="Solid" BorderColor="Black"
                                CellPadding="0" Width="120px" Visible="False">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                            <asp:Calendar ID="calAbmeldeDatVon" runat="server" BorderStyle="Solid" BorderColor="Black"
                                CellPadding="0" Width="120px" Visible="False">
                                <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                            </asp:Calendar>
                            <asp:Calendar ID="calAbmeldeDatBis" runat="server" BorderStyle="Solid" BorderColor="Black"
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
                                        <table class="BorderLeftBottom" id="Table2" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    Kennzeichen:</td>
                                                <td>
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                </td>
                                                <td nowrap>
                                                    <FONT color="red">
                                                            <asp:image id="Image1" runat="server" 
                                                                ToolTip="Platzhaltersuche mit *. Minimum 4 Stellen. Beispiel: HH-A*" 
                                                                ImageUrl="/PortalORM/Images/info.gif"></asp:image></FONT></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    Fahrgestellnummer:</td>
                                                <td>
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                </td>
                                                <td nowrap>
                                                                                                             <FONT color="red">   <asp:image id="Image2" runat="server" 
                                                                ToolTip="Platzhaltersuche mit *. Minimum 8 Stellen. Beispiel: WAUZZZ8E*" 
                                                                ImageUrl="/PortalORM/Images/info.gif"></asp:image></FONT></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    Anlagedatum Von:                                                 </td>
                                                <td>
                                                    <asp:TextBox ID="txtAnlagedatumVon" runat="server" ToolTip="Zulaufdatum Von"></asp:TextBox>
                                                    <asp:Label ID="lblInputReq" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td nowrap>
                                                    <asp:LinkButton ID="btnVon" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    Anlagedatum Bis:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAnlagedatumBis" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                    <asp:Label ID="Label1" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:LinkButton ID="btnBis" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    Web User:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWebuser" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                </td>
                                                <td nowrap="nowrap">
                                                </td>
                                            </tr>
                                            <tr >
                                                <td> 
                                                    &nbsp;</td>
                                                <td>
                                                    Meldungen ohne Abmeldung:</td>
                                                <td>
                                                    <asp:CheckBox ID="chkOhneAbmeld" runat="server" AutoPostBack="True" />
                                                </td>
                                                <td nowrap="nowrap">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    Meldungen mit Abmeldung:</td>
                                                <td>
                                                    <asp:CheckBox ID="chkMitAbmeld" runat="server" AutoPostBack="True" />
                                                </td>
                                                <td nowrap="nowrap">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr id="trAbmeldeDatVon" runat="server" visible="false">
                                                <td>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    Abmeldung Von:</td>
                                                <td>
                                                    <asp:TextBox ID="txtAbmeldungVon" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                    <asp:Label ID="lblInputReq0" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnAbmeldVon" runat="server" CssClass="StandardButtonTable" 
                                                        Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr  id="trAbmeldeDatBis" runat="server" visible="false">
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    Abmeldung Bis:</td>
                                                <td>
                                                    <asp:TextBox ID="txtAbmeldungBis" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                    <asp:Label ID="Label3" runat="server" CssClass="TextError">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnAbmeldBis" runat="server" CssClass="StandardButtonTable" 
                                                        Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    Station:</td>
                                                <td>
                                                    <asp:TextBox ID="txtStation" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td valign="top">
                                                    Mahnstufe zum Kennz.-Eingang:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMahnstufen" runat="server" Width="155px">
                                                        <asp:ListItem Value="0">- Auswahl -</asp:ListItem>
                                                        <asp:ListItem Value="*">alle Mahnstufen</asp:ListItem>
                                                        <asp:ListItem Value="1">Mahnstufe 1</asp:ListItem>
                                                        <asp:ListItem Value="2">Mahnstufe 2</asp:ListItem>
                                                        <asp:ListItem Value="3">Mahnstufe 3</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                    
                                            <tr>
                                                <td>
                                                  &nbsp;</td>
                                                <td>
                                                    nur Stornierte:</td>
                                                <td>
                                                    <asp:CheckBox ID="chkStorno" runat="server" /></td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <asp:Label ID="Label2" runat="server" CssClass="TextError">*</asp:Label>
                                        <asp:Label ID="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False"> Format: TT.MM.JJJJ</asp:Label>
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
