<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Zulg_01.aspx.vb" Inherits="AppUeberf.Zulg_01" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                            <asp:Label ID="lblPageTitle" runat="server">Fahrzeugdaten</asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 144px" valign="top" width="144">
                            <asp:Calendar ID="calVon" runat="server" Visible="False" BorderColor="Black" BorderStyle="Solid"
                                CellPadding="0" Width="120px">
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
                            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <p align="right">
                                            <strong>Schritt&nbsp;1 von 2</strong></p>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="lblKundeName1" runat="server" Width="225px" Font-Italic="True" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblKundeStrasse" runat="server" Width="278px" Font-Italic="True" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="lblKundeAnsprechpartner" runat="server" Width="307px" Font-Italic="True"
                                            Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblKundePlzOrt" runat="server" Width="134px" Font-Italic="True" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="Label5" runat="server" Width="206px" Font-Bold="True">Dienstleistungsdetails</asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px; height: 12px" width="424">
                                    </td>
                                    <td style="height: 12px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px; height: 12px" width="424">
                                        <asp:Label ID="Label1" runat="server" Width="150px" Height="22px">Haltername:*</asp:Label><asp:TextBox
                                            ID="txtHalter" runat="server" Width="266px"></asp:TextBox>
                                    </td>
                                    <td style="height: 12px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424" height="21">
                                        <asp:Label ID="Label3" runat="server" Width="150px" Height="22px">Halter-PLZ:*</asp:Label><asp:TextBox
                                            ID="txtHalterPLZ" runat="server" Width="88px" MaxLength="5"></asp:TextBox>
                                    </td>
                                    <td height="21">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424" height="21">
                                        <asp:Label ID="Label6" runat="server" Width="150px" Height="22px">gew. Zulassungsdatum:</asp:Label><asp:TextBox
                                            ID="txtAbgabetermin" runat="server" Width="88px"></asp:TextBox><asp:LinkButton ID="btnVon"
                                                runat="server" Width="100px" CssClass="StandardButtonTable"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                                <act:MaskedEditExtender ID="MEEtxtAbgabetermin" runat="server" TargetControlID="txtAbgabetermin" Mask="99/99/9999" MaskType="Date" UserDateFormat="DayMonthYear"></act:MaskedEditExtender>
                                    </td>
                                    <td height="21">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424" height="21">
                                    </td>
                                    <td height="21">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="Label12" runat="server" Width="150px" Font-Bold="True">Zulassungskreis:</asp:Label><asp:TextBox
                                            ID="txtZulkreis" runat="server" Width="39px" Font-Bold="True" Enabled="False"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:LinkButton
                                                ID="btnZulkreis" runat="server" Width="177px" CssClass="StandardButtonTable"> &#149;&nbsp;Zulassungskeis ermitteln*</asp:LinkButton>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="Label2" runat="server" Width="150px">1. Wunschkennzeichen:</asp:Label><asp:TextBox
                                            ID="txtKennzeichen1" runat="server" Width="39px" Enabled="False"></asp:TextBox><asp:Label
                                                ID="Label10" runat="server" Width="11px" Font-Bold="True"> -</asp:Label><asp:TextBox
                                                    ID="txtKennZusatz1" runat="server" Width="149px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="Label7" runat="server" Width="150px">2. Wunschkennzeichen:</asp:Label><asp:TextBox
                                            ID="txtKennzeichen2" runat="server" Width="39px" Enabled="False"></asp:TextBox><asp:Label
                                                ID="Label9" runat="server" Width="11px" Font-Bold="True"> -</asp:Label><asp:TextBox
                                                    ID="txtKennZusatz2" runat="server" Width="149px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="Label8" runat="server" Width="150px">3. Wunschkennzeichen:</asp:Label><asp:TextBox
                                            ID="txtKennzeichen3" runat="server" Width="39px" Enabled="False"></asp:TextBox><asp:Label
                                                ID="Label11" runat="server" Width="11px" Font-Bold="True"> -</asp:Label><asp:TextBox
                                                    ID="txtKennZusatz3" runat="server" Width="149px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        <asp:Label ID="Label4" runat="server" Width="150px">Referenz-Nr.:*</asp:Label><asp:TextBox
                                            ID="txtRef" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 424px" width="424">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 144px" valign="top" width="144">
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 144px" valign="top" width="144">
                        </td>
                        <td valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 144px" valign="top" width="144">
                        </td>
                        <td valign="top">
                            <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td style="width: 324px">
                                        <p align="right">
                                            <asp:ImageButton ID="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif">
                                            </asp:ImageButton></p>
                                    </td>
                                    <td style="width: 79px">
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 324px">
                                        &nbsp;
                                    </td>
                                    <td style="width: 79px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 324px">
                                    </td>
                                    <td style="width: 79px">
                                        <asp:Label ID="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="Red">*=Pflichtfeld</asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 324px">
                                    </td>
                                    <td style="width: 79px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblError" runat="server" Width="770px" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 144px" valign="top" width="144">
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
