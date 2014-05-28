<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADZul.aspx.vb" Inherits="AppUeberf.UeberfDADZul" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style2
            {
                width: 320px;
            }
            .style3
            {
                width: 100%;
                height: 17px;
            }
            .style4
            {
                width: 141px;
            }
            .style6
            {
                width: 12px;
            }
            .style7
            {
                width: 118px;
            }
            .style8
            {
                width: 13px;
            }
            .style9
            {
                width: 78px;
            }
            .style10
            {
                width: 56px;
            }
            .style11
            {
                width: 141px;
            }
            .style13
            {
                width: 120px;
            }
            .style22
            {
                width: 121%;
                height: 31px;
            }
            .style25
            {
                width: 65px;
            }
            .style26
            {
                width: 82px;
            }
            .style27
            {
                width: 22px;
            }
            .style28
            {
                width: 28px;
            }

            .style29
            {
                width: 141px;
            }
            .style31
            {
                width: 141px;
            }
            .style32
            {
                width: 141px;
            }

            .style37
            {
                width: 89px;
            }

            .style38
            {
                width: 141px;
                font-weight: bold;
                color: #009900;
            }

            .style39
            {
                width: 100%;
            }

            .style40
            {
                width: 362px;
            }
            .style41
            {
                width: 104px;
            }

            .style42
            {
                color: #009900;
                font-weight: bold;
            }

            .style43
            {
                width: 141px;
                font-size: xx-small;
            }
            .style44
            {
                font-size: xx-small;
                color: #FF0000;
            }

            </style>
	    </HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" cellSpacing="0" cellPadding="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td vAlign="top" align="left" width="100%" colSpan="3">
						<table id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;( <asp:label id="lblPageTitle" runat="server">Zulassung</asp:label>)</td></tr><tr>
								<td style="WIDTH: 144px" vAlign="top" width="174">
									<table id="Table12" cellSpacing="0" cellPadding="0" border="0">
										<tr>
											<td width="150">
												<asp:linkbutton id="lbtWeiter" tabIndex="12" runat="server" 
                                                    CssClass="StandardButtonTable" Width="100px"> •&nbsp;Weiter</asp:linkbutton></td></tr><tr>
											<td vAlign="middle" width="150"><asp:linkbutton id="lbtBack" tabIndex="12" 
                                                    runat="server" CssClass="StandardButtonTable" Width="100px"> •&nbsp;Zurück</asp:linkbutton></td></tr><tr>
											<td vAlign="middle" width="150">
									<asp:calendar id="calVon" runat="server" Width="53px" CellPadding="0" BorderStyle="Solid" 
                                                    BorderColor="Black" Visible="False" Height="133px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:calendar id="calEVBVon" runat="server" Width="53px" CellPadding="0" BorderStyle="Solid" 
                                                    BorderColor="Black" Visible="False" Height="133px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:calendar id="calEVBBis" runat="server" Width="53px" CellPadding="0" BorderStyle="Solid" 
                                                    BorderColor="Black" Visible="False" Height="133px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></td>
										</tr>
									</table>
								</td>
								<td style="WIDTH: 917px" vAlign="top">
									<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td style="WIDTH: 437px" width="437">
												<asp:Literal ID="ltAnzeige" runat="server"></asp:Literal></td></tr><tr>
											<td style="WIDTH: 437px" width="437">
												<asp:label id="lblError" runat="server" Width="821px" EnableViewState="False" CssClass="TextError"></asp:label></td></tr><tr>
											<td style="WIDTH: 437px" width="437" class="style38">
												Leasingnehmerdaten</td></tr><tr>
											<td style="WIDTH: 437px" width="437">
												<asp:Panel ID="pnl1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                    BorderWidth="1px" Width="825px">
                                                    <table cellpadding="0" cellspacing="0" class="style39">
                                                        <tr>
                                                    <td class="style29">
                                                        Leasingnehmernr.*</td><td class="style2">
                                                        <asp:TextBox ID="txtLnNummer" runat="server" TabIndex="1" MaxLength="20"></asp:TextBox><asp:ImageButton ID="ibtRefresh" runat="server" 
                                                                    ImageUrl="/Portal/images/refresh.gif" ToolTip="Aktualisieren" 
                                                            Height="16px" Width="16px" />
                                                    &nbsp;<asp:ImageButton ID="ibtSearchLN" runat="server" 
                                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                                    </td>
                                                    <td class="style41">
                                                        Vertragsnr.</td><td>
                                                                <asp:Label ID="lblVertragsnummer" runat="server"></asp:Label>
                                                            </td></tr><tr>
                                                    <td class="style29">
                                                        Leasingnehmer*</td><td class="style2">
                                                        <asp:TextBox ID="txtLnName" runat="server" Width="245px" TabIndex="2" MaxLength="35"></asp:TextBox></td>
                                                            <td class="style41">
                                                                Fahrzeugtyp*</td><td>
                                                                <asp:TextBox ID="txtLnFahrzeugtyp" runat="server" TabIndex="7"></asp:TextBox>
                                                            </td></tr><tr>
                                                    <td class="style29">
                                                        Strasse, Nr.*</td><td class="style2">
                                                        <asp:TextBox ID="txtLnStrasse" runat="server" Width="244px" TabIndex="3" MaxLength="35"></asp:TextBox></td>
                                                            <td class="style41">
                                                                FIN*</td><td>
                                                                <asp:TextBox ID="txtFahrgestellnummer" runat="server" TabIndex="8" 
                                                                    MaxLength="17"></asp:TextBox>
                                                            </td></tr><tr>
                                                    <td class="style29">
                                                        PLZ*, Ort*</td><td class="style2">
                                                        <asp:TextBox ID="txtLnPLZ" runat="server" Width="60px" TabIndex="5" MaxLength="10"></asp:TextBox>&nbsp;<asp:TextBox 
                                                                    ID="txtLnOrt" runat="server" Width="233px" TabIndex="6"></asp:TextBox></td>
                                                            <td class="style41">
                                                                Briefnummer*</td><td>
                                                                <asp:TextBox ID="txtBriefnummer" runat="server" TabIndex="9" MaxLength="8"></asp:TextBox>
                                                            </td></tr><tr>
                                                    <td class="style29">
                                                        Land*</td><td class="style2">
                                                        <asp:DropDownList ID="drpLnLand" runat="server" TabIndex="7">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style41">
                                                        &nbsp;</td><td>
                                                        &nbsp;</td></tr>
                                                    </table>
                                                </asp:Panel>
                                            </td></tr>
                                            <tr><td>&nbsp;&nbsp;</td></tr>
                                            
                                            <tr><td class="style38">Halter</td></trHalter</td></tr>
                                            
                                            <tr><td>
                                                <asp:Panel ID="pnl2" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                    BorderWidth="1px" Width="825px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
<td class="style29">
                                                        Name1*</td><td class="style2">
                                                        <asp:TextBox ID="txtShName" runat="server" TabIndex="11" Width="245px" MaxLength="35"></asp:TextBox><asp:ImageButton ID="ibtSearchHalter" runat="server" 
                                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                                            </td>
                                                    <td class="style40">
                                                        <asp:CheckBox ID="chkHalterIstLN" runat="server" AutoPostBack="True" 
                                                            Text="Halter gleich Leasingnehmer" />
                                                    </td>
                                                    <td>
                                                        &nbsp;</td></tr><tr>
                                                    <td class="style29">
                                                                                                                Name2</td><td class="style2">
                                                        <asp:TextBox ID="txtShName2" runat="server" TabIndex="12" Width="245px" MaxLength="35"></asp:TextBox></td>
                                                            <td class="style40">
                                                        &nbsp;</td><td>
                                                        &nbsp;</td></tr><tr>
                                                    <td class="style29">
                                                        Strasse, Nr.*</td><td class="style2">
                                                        <asp:TextBox ID="txtShStrasse" runat="server" Width="295px" TabIndex="13" 
                                                                    MaxLength="35"></asp:TextBox></td>
                                                            <td class="style40">
                                                        &nbsp;</td><td>
                                                        &nbsp;</td></tr><tr>
                                                    <td class="style29">
                                                        PLZ, Ort*</td><td class="style2">
                                                        <asp:TextBox ID="txtShPLZ" runat="server" Width="60px" TabIndex="14" MaxLength="10"></asp:TextBox>&nbsp;<asp:TextBox 
                                                                    ID="txtShOrt" runat="server" Width="233px" TabIndex="15" MaxLength="35"></asp:TextBox></td>
                                                            <td class="style40">
                                                        &nbsp;</td><td>
                                                        &nbsp;</td></tr><tr>
                                                    <td class="style29">
                                                        Land*</td><td class="style2">
                                                        <asp:DropDownList ID="drpShLand" runat="server" TabIndex="18">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style40">
                                                        &nbsp;</td><td>
                                                                &nbsp;</td></tr>
                                                    </table>
                                                </asp:Panel>
                                                </td></tr>
                                                <tr><td>&nbsp;&nbsp;</td></tr>
                                                <tr><td class="style38">Zulassungsdaten                              <tr><td>
                                                    <asp:Panel ID="pnl3" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                        BorderWidth="1px" Width="825px">
                                                        <table cellpadding="0" cellspacing="0" class="style39">
                                                            <tr>
                                                                <td>
                                                <table class="style3" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td class="style11">Zulassungsdatum*</td><td>
                                                        <asp:TextBox ID="txtZulassungsdatum" runat="server" TabIndex="15"></asp:TextBox>&nbsp;<asp:ImageButton ID="ibtCalZul" runat="server" ImageUrl="/Portal/images/calendar.jpg" />
                                                    </td>                                                        
                                                    </tr>
                                                <tr>
                                                    <td class="style11">Terminart</td><td>
                                                        <asp:RadioButtonList ID="rdoZulassungsdatum" runat="server" 
                                                            RepeatDirection="Horizontal" TabIndex="16">
                                                            <asp:ListItem Value="1">passend zur Überführung</asp:ListItem><asp:ListItem Value="2">frühestens am</asp:ListItem><asp:ListItem Value="3">spätestens am</asp:ListItem><asp:ListItem Value="4" Selected="True">Fixtermin</asp:ListItem></asp:RadioButtonList></td></tr>
                                                <tr>
                                                    <td class="style11">Zulassungskreis</td><td>
												<asp:textbox id="txtZulkreis" runat="server" Width="39px" Font-Bold="True" Enabled="False"></asp:textbox>&nbsp;<asp:linkbutton 
                                                            id="btnZulkreis" runat="server" Width="177px" CssClass="StandardButtonTable" 
                                                            TabIndex="17"> •&nbsp;Zulassungskeis ermitteln</asp:linkbutton></td></tr></table><table class="style3" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td class="style4">
                                                        Wunschkennzeichen</td><td class="style6">
                                                        1. </td><td class="style7"><asp:TextBox ID="txtWKennzeichen1" runat="server" 
                                                            Width="78px" TabIndex="18"></asp:TextBox></td><td class="style8">
                                                        2.</td><td class="style9">
                                                        <asp:TextBox ID="txtWKennzeichen2" runat="server" Width="78px" TabIndex="19"></asp:TextBox></td><td class="style6">
                                                        3.</td><td>
                                                        <asp:TextBox ID="txtWKennzeichen3" runat="server" Width="78px" TabIndex="20"></asp:TextBox></td><td class="style10">Res. Nr.</td><td>
                                                        <asp:TextBox ID="txtResNr" runat="server" Width="58px" TabIndex="21"></asp:TextBox></td><td>Res. Name</td><td>
                                                        <asp:TextBox ID="txtResName" runat="server" Width="131px" TabIndex="22"></asp:TextBox></td></tr></table><table class="style3" cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td class="style43">
                                                            Bei reservierten Wunschkennzeichen:</td>
                                                        <td class="style44" colspan="2">
                                                            Ausdrucke der Reservierungsbestätigung dringend dem DAD zukommen lassen!</td></tr>
                                                                        <tr>
                                                                            <td class="style11">
                                                                                Feinstaubplakette</td>
                                                                            <td class="style7">
                                                                                <asp:CheckBox ID="chkFeinstaub" runat="server" TabIndex="23" />
                                                                            </td>
                                                                            <td class="style13">
                                                                                Kfz-Steuerzahler</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="drpKfzSteuer" runat="server" TabIndex="24">
                                                                                    <asp:ListItem Selected="True" Value="H">Halter</asp:ListItem>
                                                                                    <asp:ListItem Value="L">Leasinggeber</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table></td></tr>
                                                        </table>
                                                    </asp:Panel>
                                                    </td></tr>
                                                    <tr><td>&nbsp;&nbsp;</td></tr>
                                                    <tr><td class="style42">Abweichender 
                                                        Versicherungsnehmer</td></tr>
                                                    <tr><td>
                                                        <asp:Panel ID="pnl4" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                            BorderWidth="1px" Width="825px">
                                                            <table cellpadding="0" cellspacing="0" class="style39">
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="1" cellspacing="0" class="style3">
                                                                            <tr>
                                                                                <td class="style4">
                                                                                    Auswahl*</td>
                                                                                <td>
                                                                                    <asp:RadioButtonList ID="rdoVersicherung" runat="server" AutoPostBack="True" 
                                                                                        RepeatDirection="Horizontal" TabIndex="25">
                                                                                        <asp:ListItem Selected="True" Value="0">Halter</asp:ListItem>
                                                                                        <asp:ListItem Value="1">Leasingnehmer</asp:ListItem>
                                                                                        <asp:ListItem Value="2">Leasinggesellschaft</asp:ListItem>
                                                                                        <asp:ListItem Value="3">Dritte</asp:ListItem>
                                                                                    </asp:RadioButtonList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="1" cellspacing="0">
                                                                            <tr>
                                                                                <td class="style4">
                                                                                    Vers.-gesellschaft</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtVersGesellschaft" runat="server" TabIndex="26" 
                                                                                        Width="229px" MaxLength="132"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="1" cellspacing="0" class="style22">
                                                                            <tr>
                                                                                <td class="style31">
                                                                                    eVB-Nummer*</td>
                                                                                <td class="style37">
                                                                                    <asp:TextBox ID="txtEVBNummer" runat="server" TabIndex="27" Width="74px" 
                                                                                        MaxLength="7"></asp:TextBox>
                                                                                </td>
                                                                                <td class="style25">
                                                                                    gültig von</td>
                                                                                <td class="style26">
                                                                                    <asp:TextBox ID="txtEVBVon" runat="server" TabIndex="28" Width="76px"></asp:TextBox>
                                                                                </td>
                                                                                <td class="style27">
                                                                                    <asp:ImageButton ID="ibtCalEVBVon" runat="server" 
                                                                                        ImageUrl="/Portal/images/calendar.jpg" />
                                                                                </td>
                                                                                <td class="style28">
                                                                                    bis</td>
                                                                                <td class="style9">
                                                                                    <asp:TextBox ID="txtEVBBis" runat="server" TabIndex="29" Width="76px"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ibtCalEVBBis" runat="server" 
                                                                                        ImageUrl="/Portal/images/calendar.jpg" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="1" cellspacing="0" class="style22">
                                                                            <tr>
                                                                                <td class="style32">
                                                                                    Versicherungsnehmer*</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtVersNehmer" runat="server" TabIndex="30" Width="193px" 
                                                                                        MaxLength="35"></asp:TextBox>
                                                                                    <asp:ImageButton ID="ibtSearchVersicherer" runat="server" 
                                                                                        ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="style32">
                                                                                    Strasse, Nr.*</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtVersStrasse" runat="server" TabIndex="31" Width="191px" 
                                                                                        MaxLength="35"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="style32">
                                                                                    PLZ, Ort*</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtVersPLZ" runat="server" TabIndex="32" Width="64px" 
                                                                                        MaxLength="10"></asp:TextBox>
                                                                                    &nbsp;<asp:TextBox ID="txtVersOrt" runat="server" TabIndex="33" Width="212px" 
                                                                                        MaxLength="35"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="style32">
                                                                                    Land*</td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="drpVersLand" runat="server" TabIndex="34">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        </td></tr>
                                            <tr><td>
                                                

                                                &nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>
                                            &nbsp;</td></tr>
                                            <tr><td>
                                                &nbsp;</td></tr>
                                                <tr><td>
                                                    &nbsp;</td></tr></table></table></td></tr></table></form></body></HTML>