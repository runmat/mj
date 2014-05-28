<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberf02.aspx.vb" Inherits="AppUeberf.Ueberf02"%>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CKG.Portal.PageElements" Assembly="CKG.Portal"   %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="left" cellSpacing="0" cellPadding="0">
				<tr>
					<td style="WIDTH: 959px"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD width="959" colSpan="3" style="WIDTH: 959px">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="966" border="0" style="WIDTH: 966px; HEIGHT: 889px">
							<TR>
								<TD class="PageNavigation" colSpan="3" style="WIDTH: 892px"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Fahrzeugdaten</asp:label>)</TD>
							</TR>
							<tr>
								<TD style="WIDTH: 143px" rowspan="50" vAlign="top" width="143" align="left"><asp:calendar id="calVon" runat="server" Width="120px" CellPadding="0" BorderStyle="Solid" BorderColor="Black" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:Panel id="pnlPlaceHolder" runat="server" Width="144px"></asp:Panel></TD>
								<TD style="WIDTH: 757px" vAlign="top" align="left" colSpan="2"><uc1:ProgressControl id="ProgressControl1" runat="server"></uc1:ProgressControl></TD>
							</tr>
							<TR>
								<TD style="WIDTH: 418px">
									<asp:Panel id="pnlPlaceholder3" runat="server" Width="404px"></asp:Panel></TD>
								<TD style="WIDTH: 418px">
									<asp:Panel id="pnlPlaceholder2" runat="server" Width="405px"></asp:Panel></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="lblFahrzeugdaten" runat="server" Width="105px" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"></TD>
								<TD style="HEIGHT: 12px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="Label2" runat="server" Width="192px">Kennzeichen</asp:label><asp:textbox id="txtKennzeichen1" runat="server" Width="39px" MaxLength="3"></asp:textbox><asp:label id="Label10" runat="server" Width="11px" Font-Bold="True"> -</asp:label><asp:textbox id="txtKennzeichen2" runat="server" Width="149px" MaxLength="7"></asp:textbox></TD>
								<TD height="21"><asp:label id="Label11" runat="server" Width="283px" Font-Bold="True" Height="10px">Fahrzeug zugelassen und betriebsbereit?*</asp:label><asp:radiobuttonlist id="rdbZugelassen" runat="server" Width="121px" CellPadding="0" CellSpacing="0" TextAlign="Left" RepeatDirection="Horizontal">
										<asp:ListItem Value="Ja">Ja</asp:ListItem>
										<asp:ListItem Value="Nein">Nein</asp:ListItem>
									</asp:radiobuttonlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px; HEIGHT: 11px"><asp:label id="Label16" runat="server" Width="192px">Fahrzeugwert Netto-Zeitwert*</asp:label><asp:dropdownlist id="drpFahrzeugwert" runat="server" Width="201px"></asp:dropdownlist></TD>
								<TD style="HEIGHT: 11px"><asp:label id="Label12" runat="server" Width="87px" Font-Bold="True" Height="25px">Bereifung*</asp:label><asp:radiobuttonlist id="rdbBereifung" runat="server" Width="137px" Height="20px" TextAlign="Left" RepeatDirection="Horizontal">
										<asp:ListItem Value="Sommer">Sommer</asp:ListItem>
										<asp:ListItem Value="Winter">Winter</asp:ListItem>
										<asp:ListItem Value="Ganzjahresreifen">Ganzjahresreifen</asp:ListItem>
									</asp:radiobuttonlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="Label5" runat="server" Width="206px" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="Label6" runat="server" Width="192px">Überführung bis:</asp:label><asp:textbox id="txtAbgabetermin" runat="server" Width="88px"></asp:textbox><asp:linkbutton id="btnVon" runat="server" Width="85px" CssClass="StandardButtonTable"> &#149;&nbsp;Kalender...</asp:linkbutton><asp:checkbox id="chkFix" runat="server" Width="44px" Text="Fix"></asp:checkbox></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px; HEIGHT: 18px"><FONT color="#0000ff">Bitte beachten Sie die 
										Standardvorlaufzeit von 4 Werktagen!</FONT></TD>
								<TD style="HEIGHT: 18px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="Label7" runat="server" Width="192px">Wagen vollgetankt übergeben:</asp:label><asp:checkbox id="chkWagenVolltanken" runat="server"></asp:checkbox></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="Label8" runat="server" Width="192px">Wagenwäsche:</asp:label><asp:checkbox id="chkWw" runat="server"></asp:checkbox></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="Label9" runat="server" Width="192px">Fahrzeugeinweisung:</asp:label><asp:checkbox id="chkEinweisung" runat="server"></asp:checkbox></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"><asp:label id="Label15" runat="server" Width="192px">Rote Kennzeichen erforderlich:</asp:label><asp:checkbox id="chkRotKenn" runat="server"></asp:checkbox></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px"></TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 418px" colSpan="2" vAlign="top">
									<P>
										<asp:CheckBox id="chkWinterreifenHandling" runat="server" Width="374px" Font-Bold="True" Text="Winterreifenhandling durch Kroschke Car Logistic" AutoPostBack="True"></asp:CheckBox><asp:panel id="pnlWinterreifen" runat="server" Width="773px" Visible="False" Height="100%">
											<TABLE id="Table3" style="WIDTH: 769px; HEIGHT: 166px" cellSpacing="1" cellPadding="1" width="769" border="0">
												<TR>
													<TD style="HEIGHT: 33px"><FONT color="#0000ff">Hinweis: Bei Vorgabe eines Fixtermins 
															kann es zu Problemen bei der Beschaffung von Rädern und Montageterminen kommen!</FONT></TD>
												</TR>
												<TR>
													<TD>
														<asp:RadioButton id="rdoWinterreifenAbWerk" runat="server" Text="Winterräder ab Werk geliefert - montieren lassen" AutoPostBack="True" GroupName="Winterräder" Enabled="False"></asp:RadioButton></TD>
												</TR>
												<TR>
													<TD>
														<asp:RadioButton id="rdoWinterreifenBesorgen" runat="server" Width="341px" Text="Winterräder besorgen + montieren lassen" AutoPostBack="True" GroupName="Winterräder" Enabled="False"></asp:RadioButton></TD>
												</TR>
												<TR>
													<TD noWrap>
														<asp:CheckBox id="chkWinterreifenGroesser" runat="server" Width="387px" Text="Größere Reifendimensionen gewünscht, wenn zugelassen:" Enabled="False"></asp:CheckBox>
														<asp:TextBox id="txtWinterreifenGroesse" runat="server" Width="166px" Enabled="False"></asp:TextBox></TD>
												</TR>
												<TR>
													<TD>
														<asp:Label id="Label1" runat="server" Height="25px">Felgen:</asp:Label>
														<asp:RadioButtonList id="rdlFelgen" runat="server" RepeatDirection="Horizontal" Enabled="False">
															<asp:ListItem Value="Standard" Selected="True">Standard</asp:ListItem>
															<asp:ListItem Value="Zubeh&#246;r-LM">Zubeh&#246;r-LM</asp:ListItem>
															<asp:ListItem Value="Herst.-LM">Herst.-LM</asp:ListItem>
														</asp:RadioButtonList></TD>
												</TR>
												<TR>
													<TD>
														<asp:Label id="Label3" runat="server" Height="25px">Radkappen:</asp:Label>
														<asp:RadioButtonList id="rdlRadkappen" runat="server" RepeatDirection="Horizontal" Enabled="False">
															<asp:ListItem Value="Zubeh&#246;r">Zubeh&#246;r</asp:ListItem>
															<asp:ListItem Value="Hersteller">Hersteller</asp:ListItem>
															<asp:ListItem Value="Keine" Selected="True">Keine</asp:ListItem>
														</asp:RadioButtonList></TD>
												</TR>
												<TR>
													<TD>
														<asp:Label id="Label4" runat="server" Height="25px">Zweiter Radsatz:</asp:Label>
														<asp:RadioButtonList id="rdlZweiterRadsatz" runat="server" RepeatDirection="Horizontal" Enabled="False">
															<asp:ListItem Value="ins Fzg. legen" Selected="True">ins Fzg. legen</asp:ListItem>
															<asp:ListItem Value="am Ziel einlagern">am Ziel einlagern</asp:ListItem>
															<asp:ListItem Value="einlagern - siehe Bemerkung">einlagern - siehe Bemerkung</asp:ListItem>
														</asp:RadioButtonList></TD>
												</TR>
											</TABLE>
										</asp:panel></P>
								</TD>
							</TR>
							<tr>
								<td colspan="2" valign="top"><asp:label id="Label18" runat="server" Width="103px" Height="81px">Bemerkung:</asp:label><cc1:textareacontrol id="txtBemerkung" runat="server" Width="424px" Height="80px" MaxLength="256" TextMode="MultiLine"></cc1:textareacontrol></td>
							</tr>
							<tr>
								<td style="WIDTH: 100%" colSpan="2"><asp:label id="lblError" runat="server" Height="17px" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td style="WIDTH: 100%; HEIGHT: 100%" colSpan="2" vAlign="top">
									<TABLE id="Table2" style="WIDTH: 808px; HEIGHT: 75px" cellSpacing="1" cellPadding="1" width="808" border="0">
										<TR>
											<TD style="WIDTH: 368px"></TD>
											<TD style="WIDTH: 122px"></TD>
											<TD vAlign="top"></TD>
										</TR>
										<TR>
											<TD align="right" style="WIDTH: 368px"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Customize/Ueberf/arrowUeberfLeft.gif"></asp:imagebutton></TD>
											<TD style="WIDTH: 122px" align="center"><asp:linkbutton id="lnkAnschlussfahrt" runat="server" CssClass="SpecialButtonTable">Anschlussfahrt</asp:linkbutton></TD>
											<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Customize/Ueberf/arrowUeberfRight.gif"></asp:imagebutton></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 368px"></TD>
											<TD style="WIDTH: 122px"></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
