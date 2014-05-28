<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_02.aspx.vb" Inherits="AppUeberf.Ueberfg_02" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD width="100%" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Fahrzeugdaten</asp:label>)</TD>
							</TR>
							<tr>
								<TD style="WIDTH: 144px" vAlign="top" width="144"><asp:calendar id="calVon" runat="server" Width="120px" CellPadding="0" BorderStyle="Solid" BorderColor="Black" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></TD>
								<TD vAlign="top">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
										<TR>
											<TD style="WIDTH: 463px" width="463">
												<P align="right"><STRONG>Schritt&nbsp;3 von 4</STRONG></P>
											</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="lblKundeName1" runat="server" Width="225px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundeStrasse" runat="server" Width="278px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="lblKundeAnsprechpartner" runat="server" Width="307px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundePlzOrt" runat="server" Width="134px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="lblFahrzeugdaten" runat="server" Width="105px" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px; HEIGHT: 17px" width="463"><asp:label id="Label1" runat="server" Width="192px" Height="22px"> Fahrzeugtyp*</asp:label>
                                                <asp:textbox id="txtHerstTyp" runat="server" Width="200px" MaxLength="10"></asp:textbox></TD>
											<TD style="HEIGHT: 17px"><asp:label id="Label11" runat="server" Width="283px" Font-Bold="True" Height="10px">Fahrzeug zugelassen und betriebsbereit?*</asp:label></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 29px" noWrap>
												<table cellSpacing="0" cellPadding="1" border="0">
													<TBODY>
														<tr>
															<td vAlign="middle"><asp:label id="Label19" runat="server" Width="192px" Height="13px">Fahrzeugklasse in Tonnen*</asp:label></td>
															<td><asp:radiobuttonlist id="rdbFahrzeugklasse" runat="server" Width="200px" CellPadding="0" Height="17px" CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
																	<asp:ListItem Value="P">&lt; 3,5</asp:ListItem>
																	<asp:ListItem Value="G">3,5 - 7,5</asp:ListItem>
																	<asp:ListItem Value="L">&gt; 7,5</asp:ListItem>
																</asp:radiobuttonlist></td>
														</tr>
													</TBODY>
												</table>
											</TD>
											<TD style="HEIGHT: 29px"><asp:radiobuttonlist id="rdbZugelassen" runat="server" Width="121px" CellPadding="0" Height="11px" CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="Ja">Ja</asp:ListItem>
													<asp:ListItem Value="Nein">Nein</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 29px" noWrap></TD>
											<TD style="HEIGHT: 29px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"></TD>
											<TD><asp:label id="Label99" runat="server" Width="223px" Font-Bold="True">Zulassung durch Kroschke?*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label2" runat="server" Width="192px">Kennzeichen*</asp:label><asp:textbox id="txtKennzeichen1" runat="server" Width="39px"></asp:textbox><asp:label id="Label10" runat="server" Width="11px" Font-Bold="True"> -</asp:label><asp:textbox id="txtKennzeichen2" runat="server" Width="149px"></asp:textbox></TD>
											<TD><asp:radiobuttonlist id="rdbHinZulKCL" runat="server" Width="121px" CellPadding="0" Height="11px" CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="Ja">Ja</asp:ListItem>
													<asp:ListItem Value="Nein">Nein</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label3" runat="server" Width="192px">Fgst.-Nummer:</asp:label>
                                                <asp:textbox id="txtVin" runat="server" Width="200px" MaxLength="17"></asp:textbox></TD>
											<TD><asp:label id="Label16" runat="server" Width="94px" Font-Bold="True">Fahrzeugwert*</asp:label><asp:dropdownlist id="drpFahrzeugwert" runat="server" Width="166px"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label4" runat="server" Width="192px">Referenz-Nr.:</asp:label>
                                                <asp:textbox id="txtRef" runat="server" Width="200px" MaxLength="20"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463">&nbsp;</TD>
											<TD><asp:label id="Label12" runat="server" Width="251px" Font-Bold="True">Bereifung*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label5" runat="server" Width="206px" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
											<TD><asp:radiobuttonlist id="rdbBereifung" runat="server" Width="137px" Height="19px" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="Sommer">Sommer</asp:ListItem>
													<asp:ListItem Value="Winter">Winter</asp:ListItem>
													<asp:ListItem Value="Ganzjahresreifen">Ganzjahresreifen</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label6" runat="server" Width="192px">Überführung bis:</asp:label><asp:textbox id="txtAbgabetermin" runat="server" Width="88px"></asp:textbox><asp:linkbutton id="btnVon" runat="server" Width="100px" CssClass="StandardButtonTable"> &#149;&nbsp;Kalender...</asp:linkbutton></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label7" runat="server" Width="192px">Wagen vollgetankt übergeben:</asp:label><asp:checkbox id="chkWagenVolltanken" runat="server"></asp:checkbox></TD>
											<TD><asp:label id="Label18" runat="server" Width="343px" Font-Bold="True" Height="10px">Expressüberführung(kostenpflichtig)*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label8" runat="server" Width="192px">Wagenwäsche:</asp:label><asp:checkbox id="chkWw" runat="server"></asp:checkbox></TD>
											<TD><asp:radiobuttonlist id="rdExpress" runat="server" Width="121px" CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="Ja">Ja</asp:ListItem>
													<asp:ListItem Value="Nein">Nein</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label9" runat="server" Width="192px">Fahrzeugeinweisung:</asp:label><asp:checkbox id="chkEinweisung" runat="server"></asp:checkbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"><asp:label id="Label15" runat="server" Width="192px">Rote Kennzeichen erforderlich:</asp:label><asp:checkbox id="chkRotKenn" runat="server"></asp:checkbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 463px" width="463"></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<td vAlign="top" colSpan="2"><asp:label id="Label13" runat="server" Width="103px" Height="81px">Bemerkung:</asp:label><cc1:textareacontrol id="txtBemerkung" runat="server" Width="424px" Height="80px" MaxLength="256" TextMode="MultiLine"></cc1:textareacontrol><asp:linkbutton id="linkbtTexte" runat="server">Standardtext</asp:linkbutton></td>
							</TR>
							<TR>
								<TD style="WIDTH: 144px; HEIGHT: 3px" vAlign="top" width="144"></TD>
								<TD style="HEIGHT: 3px" vAlign="top">&nbsp;</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top">
									<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 324px">
												<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
											</TD>
											<TD style="WIDTH: 79px"><asp:linkbutton id="lnkAnschlussfahrt" runat="server" CssClass="SpecialButtonTable">Anschlussfahrt</asp:linkbutton></TD>
											<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif"></asp:imagebutton></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px">&nbsp;</TD>
											<TD style="WIDTH: 79px"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px"></TD>
											<TD style="WIDTH: 79px"><asp:label id="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="Red">*=Pflichtfeld</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px"></TD>
											<TD style="WIDTH: 79px"></TD>
											<TD></TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" Width="770px" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
