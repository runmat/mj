<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadEdit03.aspx.vb" Inherits="AppUeberf.UploadEdit03" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CKG.Portal.PageElements" Assembly="CKG.Portal"   %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    
    <script language="javascript" src="../Controls/CustomDialog.js"></script>
    
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    
    <style type="text/css">
    div.transbox
 {
	width: 400px;
    height: 180px;
    background-color: #ffffff;
    filter:alpha(opacity=60);
    opacity:0.6;
 }
    </style>
        <script type="text/javascript">

            function ShowMessage() {
                SetText('Ja', 'Nein');

                DisplayConfirmMessage('Eventuelle Änderungen speichern?', 225, 75)

                SetDefaultButton('btnConfOK');

                var element = document.getElementById('FormDiv');
                element.className = 'transbox'
                return false;

            }
</script>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <div id="divConfMessage" runat="server" style="background-image: url(/Portal/Images/divBody.jpg);
        display: none; z-index: 200;">
        <div style="background-image: url(/Portal/Images/headbox.jpg); text-align: center"
            id="confirmText">
        </div>
        <div style="z-index: 105; height: 30%; background-image: url(/Portal/Images/divBody.jpg);
            text-align: center">
        </div>
        <div style="z-index: 105; height: 70%; background-image: url(/Portal/Images/divBody.jpg);
            text-align: center">
            <asp:Button ID="btnConfOK" Width="75px" runat="server" Text="OK"></asp:Button>
            <asp:Button ID="btnConfCancel" Width="75px" runat="server" Text="Cancel"></asp:Button>
        </div>
    </div>
    <div id="FormDiv">
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
                            <td style="width: 174px" valign="top" width="174">
                                <table id="Table12" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td width="150">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="150">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="150">
                                            <asp:ImageButton ID="cmdBack0" runat="server" Width="73px" Height="40px" ImageUrl="/Portal/Images/BackToMap.jpg"
                                                ToolTip="Zurück zur Übersicht"></asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="150">
                                            <asp:Calendar ID="calVon" runat="server" Width="120px" CellPadding="0" BorderStyle="Solid"
                                                BorderColor="Black" Visible="False">
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
                            <td valign="top">
                                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <p align="right">
                                                <strong>Schritt&nbsp;3 von 4</strong></p>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="lblFahrzeugdaten" runat="server" Width="105px" Font-Bold="True">Fahrzeugdaten</asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px; height: 17px" width="463">
                                            <asp:Label ID="Label1" runat="server" Width="192px" Height="22px"> Fahrzeugtyp*</asp:Label><asp:TextBox
                                                ID="txtHerstTyp" runat="server" Width="200px" MaxLength="25"></asp:TextBox>
                                        </td>
                                        <td style="height: 17px">
                                            <asp:Label ID="Label11" runat="server" Width="283px" Font-Bold="True" Height="10px">Fahrzeug zugelassen und betriebsbereit?*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 29px" nowrap>
                                            <table cellspacing="0" cellpadding="1" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td valign="middle">
                                                            <asp:Label ID="Label19" runat="server" Width="192px" Height="13px">Fahrzeugklasse in Tonnen*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdbFahrzeugklasse" runat="server" Width="200px" CellPadding="0"
                                                                Height="17px" CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
                                                                <asp:ListItem Value="P">&lt; 3,5</asp:ListItem>
                                                                <asp:ListItem Value="G">3,5 - 7,5</asp:ListItem>
                                                                <asp:ListItem Value="L">&gt; 7,5</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style="height: 29px">
                                            <asp:RadioButtonList ID="rdbZugelassen" runat="server" Width="121px" CellPadding="0"
                                                Height="11px" CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
                                                <asp:ListItem Value="Ja">Ja</asp:ListItem>
                                                <asp:ListItem Value="Nein">Nein</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 29px" nowrap>
                                        </td>
                                        <td style="height: 29px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                        </td>
                                        <td>
                                            <asp:Label ID="Label99" runat="server" Width="175px" Font-Bold="True">Zulassung durch KCL?*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label2" runat="server" Width="192px">Kennzeichen*</asp:Label><asp:TextBox
                                                ID="txtKennzeichen1" runat="server" Width="39px"></asp:TextBox><asp:Label ID="Label10"
                                                    runat="server" Width="11px" Font-Bold="True"> -</asp:Label><asp:TextBox ID="txtKennzeichen2"
                                                        runat="server" Width="149px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdbHinZulKCL" runat="server" Width="121px" CellPadding="0"
                                                Height="11px" CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
                                                <asp:ListItem Value="Ja">Ja</asp:ListItem>
                                                <asp:ListItem Value="Nein">Nein</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label3" runat="server" Width="192px">Fgst.-Nummer:</asp:Label><asp:TextBox
                                                ID="txtVin" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Width="94px" Font-Bold="True">Fahrzeugwert*</asp:Label><asp:DropDownList
                                                ID="drpFahrzeugwert" runat="server" Width="166px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label4" runat="server" Width="192px">Referenz-Nr.:</asp:Label><asp:TextBox
                                                ID="txtRef" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Width="251px" Font-Bold="True">Bereifung*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label5" runat="server" Width="206px" Font-Bold="True">Dienstleistungsdetails</asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdbBereifung" runat="server" Width="137px" Height="19px"
                                                RepeatDirection="Horizontal" TextAlign="Left">
                                                <asp:ListItem Value="Sommer">Sommer</asp:ListItem>
                                                <asp:ListItem Value="Winter">Winter</asp:ListItem>
                                                <asp:ListItem Value="Ganzjahresreifen">Ganzjahresreifen</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label6" runat="server" Width="192px">Überführung bis:</asp:Label><asp:TextBox
                                                ID="txtAbgabetermin" runat="server" Width="88px"></asp:TextBox><asp:LinkButton ID="btnVon"
                                                    runat="server" Width="100px" CssClass="StandardButtonTable"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label7" runat="server" Width="192px">Wagen vollgetankt übergeben:</asp:Label><asp:CheckBox
                                                ID="chkWagenVolltanken" runat="server"></asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Width="343px" Font-Bold="True" Height="10px">Expressüberführung(kostenpflichtig)*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label8" runat="server" Width="192px">Wagenwäsche:</asp:Label><asp:CheckBox
                                                ID="chkWw" runat="server"></asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdExpress" runat="server" Width="121px" CellPadding="0"
                                                CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
                                                <asp:ListItem Value="Ja">Ja</asp:ListItem>
                                                <asp:ListItem Value="Nein">Nein</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label9" runat="server" Width="192px">Fahrzeugeinweisung:</asp:Label><asp:CheckBox
                                                ID="chkEinweisung" runat="server"></asp:CheckBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                            <asp:Label ID="Label15" runat="server" Width="192px">Rote Kennzeichen erforderlich:</asp:Label><asp:CheckBox
                                                ID="chkRotKenn" runat="server"></asp:CheckBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 463px" width="463">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px" valign="top" width="144">
                                &nbsp;
                            </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px" valign="top" width="144">
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label13" runat="server" Width="103px" Height="81px">Bemerkung:</asp:Label><cc1:TextAreaControl
                                    ID="txtBemerkung" runat="server" Width="424px" Height="80px" MaxLength="256"
                                    TextMode="MultiLine"></cc1:TextAreaControl><asp:LinkButton ID="linkbtTexte" runat="server">Standardtext</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px; height: 3px" valign="top" width="144">
                            </td>
                            <td style="height: 3px" valign="top">
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
                                        <td style="width: 80px">
                                            <asp:LinkButton ID="lnkAnschlussfahrt" runat="server" CssClass="SpecialButtonTable"
                                                Visible="False">Anschlussfahrt</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px">
                                        </td>
                                        <td class="style1">
                                            <asp:Label ID="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="Red">*=Pflichtfeld</asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px">
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
    </div>
    </form>
</body>
</html>
