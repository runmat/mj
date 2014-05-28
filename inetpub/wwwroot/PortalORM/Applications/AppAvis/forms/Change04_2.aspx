<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_2.aspx.vb" Inherits="AppAvis.Change04_2" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            font-weight: bold;
        }
        .style3
        {
            font-size: x-small;
        }
    </style>
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
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server"> (Anlegen)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdNew" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Neuanlage</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Calendar ID="calSperreAb" runat="server" BackColor="White" BorderColor="#999999"
                                CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Width="120px"
                                Visible="False">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                            </asp:Calendar>
                            <asp:Calendar ID="calSperreBis" runat="server" BackColor="White" BorderColor="#999999"
                                CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Width="120px"
                                Visible="False">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                            </asp:Calendar>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                        <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" Visible="False"
                                            NavigateUrl="javascript:window.close()">Fenster schließen</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" valign="top">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"
                                            ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="1" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                    <b>Carport</b><strong>:*</strong>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <strong>
                                                        <asp:DropDownList ID="drpCarport" runat="server" Width="244px" AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150">
                                                    <strong>Farbe:</strong>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <strong>
                                                        <asp:DropDownList ID="drpFarbe" runat="server" Width="157px" AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150">
                                                    Liefermonat:*
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <strong>
                                                        <asp:DropDownList ID="drpLiefermonat" runat="server" Width="157px" Height="16px"
                                                            AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150">
                                                    <strong>Hersteller:*</strong>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <strong>
                                                        <asp:DropDownList ID="drpHersteller" runat="server" Width="157px" AppendDataBoundItems="True"
                                                            AutoPostBack="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="19">
                                                    <strong>Modellgruppe:*</strong>
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:DropDownList ID="drpModellgruppe" runat="server" Width="157px" Enabled="False"
                                                            AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="style3">(Zum Aktivieren bitte Hersteller auswählen.)</span></strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="19">
                                                    <b>Kraftstoff</b><strong>:</strong>
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:DropDownList ID="drpKraftstoff" runat="server" Width="157px" AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="19">
                                                    <strong>Navi:</strong>
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:DropDownList ID="drpNavi" runat="server" Width="157px" AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="19">
                                                    <b>Reifenart</b><strong>:*</strong>
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:DropDownList ID="drpReifenart" runat="server" Width="157px" AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150" height="19">
                                                    Aufbauart:
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:DropDownList ID="drpAufbauart" runat="server" Width="157px" AppendDataBoundItems="True">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="19">
                                                    <b>Typ</b><strong>:*</strong>
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:TextBox ID="txtTyp" runat="server"></asp:TextBox></strong>
                                                </td>
                                            </tr>
                                        <tr>
                                            <td  class="style1" valign="center" width="150" height="19">
                                                Händlernummer:</td>
                                            <td  class="TextLarge" valign="center" height="19">
                                                 <asp:TextBox ID="txtHaendler" runat="server" MaxLength="10"></asp:TextBox></td>
                                        </tr>                                            
                                            <tr>
                                                <td class="TextLarge" valign="center" width="150" height="19">
                                                    &nbsp;
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150" height="19">
                                                    Datum Sperre ab:
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:TextBox ID="txtDatumAb" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:ImageButton ID="ibtDatumAb" runat="server" ImageUrl="/PortalORM/images/calendar.jpg" />
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150" height="19">
                                                    Datum Sperre bis:
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:TextBox ID="txtDatumBis" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:ImageButton ID="ibtDatumBis" runat="server" ImageUrl="/PortalORM/images/calendar.jpg"
                                                            Style="width: 16px" />
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150" height="19">
                                                    &nbsp;
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150" height="19">
                                                    Anzahl Fahrzeuge:
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:TextBox ID="txtAnzFahrzeuge" runat="server" MaxLength="4" Width="44px"></asp:TextBox>
                                                        <span class="style3">(Pflichtfeld)</span></strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" valign="center" width="150" height="19">
                                                    Sperrvermerk:
                                                </td>
                                                <td class="TextLarge" valign="center" height="19">
                                                    <strong>
                                                        <asp:TextBox ID="txtSperrvermerk" runat="server" MaxLength="20" Width="268px"></asp:TextBox></strong>
                                                </td>
                                            </tr>
                                        </table>
                                        &nbsp;* = Relevantes Blockkriterium
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblErrMessage" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
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
