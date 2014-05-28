<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_04.aspx.vb" Inherits="AppUeberf.Ueberfg_04" %>
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
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" width="100%" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Zusammenstellung</asp:label>)</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 1040px; HEIGHT: 30px" colSpan="2">
									<TABLE id="Table3" style="WIDTH: 1027px; HEIGHT: 27px" cellSpacing="1" cellPadding="1" width="1027" border="0">
										<TR>
											<TD style="WIDTH: 462px"></TD>
											<TD style="WIDTH: 283px"><asp:label id="lblSchritt" runat="server" Font-Bold="True" Width="274px">Schritt 4 von 4</asp:label></TD>
											<TD></TD>
										</TR>
									</TABLE>
									&nbsp;</TD>
							</TR>
							<TR>
								<TD class="PageNavigation" style="WIDTH: 1040px" colSpan="2"></TD>
							</TR>
							<tr>
								<TD style="WIDTH: 174px" vAlign="top" width="174">
									<TABLE id="Table12" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<TD style="WIDTH: 917px" vAlign="top">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TBODY>
											<TR>
												<TD style="WIDTH: 560px" width="560"><asp:label id="lblKundeName1" runat="server" Font-Bold="True" Width="225px" Font-Italic="True"></asp:label></TD>
												<TD><asp:label id="lblKundeStrasse" runat="server" Font-Bold="True" Width="442px" Font-Italic="True"></asp:label></TD>
											</TR>
											<TR>
												<TD style="WIDTH: 560px" width="560"><asp:label id="lblKundeAnsprechpartner" runat="server" Font-Bold="True" Width="307px" Font-Italic="True"></asp:label></TD>
												<TD><asp:label id="lblKundePlzOrt" runat="server" Font-Bold="True" Width="405px" Font-Italic="True"></asp:label></TD>
											</TR>
											<TR>
												<TD style="WIDTH: 560px" width="560" height="20"></TD>
												<TD></TD>
											</TR>
											<TR>
												<TD style="WIDTH: 560px" width="560">
													<P>
														<asp:label id="lbl_Auftragsnr" runat="server" Width="462px" Font-Bold="True" Visible="False"></asp:label></P>
												</TD>
								</TD>
								<TD></TD>
							<TR>
								<TD style="WIDTH: 560px" width="560" height="20"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560">
									<asp:label id="Label33" runat="server" Width="180px" Font-Bold="True">Rechnungszahler</asp:label></TD>
								<TD>
									<asp:label id="Label34" runat="server" Width="302px" Font-Bold="True">Postalischer Rechnungsempfänger</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560">
									<asp:label id="Label36" runat="server" Width="110px"> Name:</asp:label>
									<asp:label id="lblRegName" runat="server"></asp:label></TD>
								<TD>
									<asp:label id="Label39" runat="server" Width="110px"> Name:</asp:label>
									<asp:label id="lblRechName" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label37" runat="server" Width="110px">Straße, Nr:</asp:label><asp:label id="lblRegStrasse" runat="server"></asp:label></TD>
								<TD><asp:label id="Label41" runat="server" Width="110px">Straße, Nr:</asp:label><asp:label id="lblRechStrasse" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label38" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblRegOrt" runat="server"></asp:label></TD>
								<TD><asp:label id="Label43" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblRechOrt" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label5" runat="server" Font-Bold="True" Width="180px">Abholung</asp:label></TD>
								<TD><asp:label id="Label15" runat="server" Font-Bold="True" Width="146px">Anlieferung</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label6" runat="server" Width="110px"> Name:</asp:label><asp:label id="lblAbName" runat="server"></asp:label></TD>
								<TD><asp:label id="Label13" runat="server" Width="106px">Name:</asp:label><asp:label id="lblAnName" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560" height="21"><asp:label id="Label7" runat="server" Width="110px">Straße, Nr:</asp:label><asp:label id="lblAbStrasse" runat="server"></asp:label></TD>
								<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Straße, Nr:</asp:label><asp:label id="lblAnStrasse" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label8" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblAbOrt" runat="server"></asp:label></TD>
								<TD><asp:label id="Label18" runat="server" Width="106px">PLZ Ort:</asp:label><asp:label id="lblAnOrt" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label9" runat="server" Width="110px">Ansprechpartner:</asp:label><asp:label id="lblAbAnsprechpartner" runat="server"></asp:label></TD>
								<TD><asp:label id="Label19" runat="server" Width="106px">Ansprechpartner:</asp:label><asp:label id="lblAnAnspechpartner" runat="server" Width="265px"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label16" runat="server" Width="110px">1. Telefon:</asp:label><asp:label id="lblAbTelefon" runat="server"></asp:label></TD>
								<TD><asp:label id="Label20" runat="server" Width="106px">1. Telefon:</asp:label><asp:label id="lblAnTelefon" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label27" runat="server" Width="110px">2. Telefon:</asp:label><asp:label id="lblAbTelefon2" runat="server"></asp:label></TD>
								<TD><asp:label id="Label28" runat="server" Width="110px">2. Telefon:</asp:label><asp:label id="lblAnTelefon2" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label29" runat="server" Width="110px">Fax:</asp:label><asp:label id="lblAbFax" runat="server"></asp:label></TD>
								<TD><asp:label id="Label30" runat="server" Width="110px">Fax:</asp:label><asp:label id="lblAnFax" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="lblFahrzeugdaten" runat="server" Font-Bold="True" Width="186px">Fahrzeugdaten</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ:</asp:label><asp:label id="lblHerst" runat="server"></asp:label></TD>
								<TD><asp:label id="Label11" runat="server" Width="243px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lblZugelassen" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label35" runat="server" Width="110px">Fahrzeugklasse:</asp:label><asp:label id="lblFahrzeugklasse" runat="server"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen:</asp:label><asp:label id="lblKennzeichen" runat="server"></asp:label></TD>
								<TD>
									<asp:label id="Label45" runat="server" Width="141px">Zulassung durch KCL:</asp:label>
									<asp:label id="lblHinZulKCL" runat="server" Width="241px"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer:</asp:label><asp:label id="lblVin" runat="server"></asp:label></TD>
								<TD><asp:label id="Label26" runat="server" Width="89px">Fahrzeugwert:</asp:label><asp:label id="lblFahrzeugwert" runat="server" Width="241px"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr.:</asp:label><asp:label id="lblRef" runat="server"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560">&nbsp;</TD>
								<TD><asp:label id="Label12" runat="server" Width="89px">Bereifung:</asp:label><asp:label id="lblBereifung" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label10" runat="server" Font-Bold="True" Width="188px">Dienstleistungsdetails</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label14" runat="server" Width="157px">Überführung bis:</asp:label><asp:label id="lblDatumUeberf" runat="server"></asp:label></TD>
								<TD><asp:label id="Label22" runat="server" Width="128px">Wagenwäsche:</asp:label><asp:label id="lblWW" runat="server" Width="66px"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label21" runat="server" Width="157px">Wagen volltanken:</asp:label><asp:label id="lblTanken" runat="server"></asp:label></TD>
								<TD><asp:label id="Label23" runat="server" Width="128px">Fahrzeugeinweisung:</asp:label><asp:label id="lblEinw" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 560px" width="560"><asp:label id="Label25" runat="server" Width="157px">Rotes Kennzeichen:</asp:label><asp:label id="lblRotKenn" runat="server"></asp:label></TD>
								<TD><asp:label id="Label40" runat="server" Width="128px">Expressüberführung:</asp:label><asp:label id="lblExpress" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
						<P><asp:label id="Label24" runat="server" Width="149px">Bemerkung:</asp:label><asp:label id="lblBem" runat="server" Width="428px"></asp:label>
							<hr color="#000000">
						<P></P>
						<TABLE id="Table5" style="WIDTH: 918px; HEIGHT: 67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="lbl1Anschluss" runat="server" Font-Bold="True" Width="229px">Anschlussfahrt</asp:label></TD>
								<TD><asp:label id="lbl1FzgDaten" runat="server" Font-Bold="True" Width="202px">Fahrzeugdaten</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="lbl1Name" runat="server" Width="107px">Name:</asp:label><asp:label id="lbl2ReName" runat="server"></asp:label></TD>
								<TD><asp:label id="lbl1Herst" runat="server" Width="110px">Hersteller:</asp:label><asp:label id="lbl2ReHerst" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="lbl1StrNr" runat="server" Width="107px">Straße, Nr.:</asp:label><asp:label id="lbl2ReStrasse" runat="server"></asp:label></TD>
								<TD><asp:label id="Label44" runat="server" Width="110px">Fahrzeugklasse:</asp:label><asp:label id="lbl2ReFahrzeugklasse" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="lbl1PLZOrt" runat="server" Width="107px">PLZ Ort:</asp:label><asp:label id="lbl2RePlzOrt" runat="server"></asp:label></TD>
								<TD><asp:label id="lbl1Kenn" runat="server" Width="110px">Kennzeichen:</asp:label><asp:label id="lbl2ReKenn" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="lbl1Ansprech" runat="server" Width="107px">Ansprechpartner:</asp:label><asp:label id="lbl2ReAnsprech" runat="server"></asp:label></TD>
								<TD><asp:label id="lbl1Vin" runat="server" Width="110px">Fgst.-Nummer:</asp:label><asp:label id="lbl2ReVin" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="lbl1Telefon" runat="server" Width="107px">1. Telefon:</asp:label><asp:label id="lbl2ReTelefon" runat="server"></asp:label></TD>
								<TD><asp:label id="lbl1Ref" runat="server" Width="110px">Referenz-Nr.:</asp:label><asp:label id="lbl2ReRef" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="Label31" runat="server" Width="107px">2. Telefon:</asp:label><asp:label id="lbl2ReTelefon2" runat="server"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"><asp:label id="Label32" runat="server" Width="107px">Fax:</asp:label><asp:label id="lbl2ReFax" runat="server"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"></TD>
								<TD><asp:label id="lbl1FzgZugelassen" runat="server" Width="243px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lbl2ReZugelassen" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"></TD>
								<TD>
									<asp:label id="Label42" runat="server" Width="170px" Visible="False">Zulassung durch Kroschke:</asp:label>
									<asp:label id="lblAnZulKCL" runat="server" Visible="False"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 436px"></TD>
								<TD><asp:label id="lbl1Bereifung" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lbl2ReBereif" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
						&nbsp;</TD>
				</TR>
			</table>
			<TABLE id="Table2" style="WIDTH: 1040px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="1040" border="0">
				<TR>
					<TD style="WIDTH: 479px">
						<P align="right"><asp:linkbutton id="cmdNewOrder" runat="server" Visible="False" CssClass="StandardButton" Height="23px"> Neuer Auftrag</asp:linkbutton><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
					</TD>
					<TD style="WIDTH: 110px"><asp:button id="cmdSave" runat="server" Width="95px" Text="Beauftragen"></asp:button></TD>
					<TD style="WIDTH: 299px"><asp:linkbutton id="cmdNewOrderHoldData" runat="server" Width="280px" Visible="False" CssClass="StandardButton" Height="23px"> Neuer Auftrag mit identischen Adressen</asp:linkbutton></TD>
					<TD><asp:button id="cmdPrint" runat="server" Width="98px" Visible="False" Text="Druckansicht"></asp:button></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 479px"></TD>
					<TD style="WIDTH: 110px"></TD>
					<TD style="WIDTH: 299px"></TD>
				</TR>
			</TABLE>
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="WIDTH: 118px"></TD>
					<TD style="WIDTH: 612px"><asp:label id="lblError" runat="server" Width="802px" CssClass="TextError" EnableViewState="False"></asp:label></TD>
				</TR>
			</TABLE>
			</TD></TR>
			<TR>
				<TD style="WIDTH: 1033px" vAlign="top" width="1033"></TD>
				<TD style="WIDTH: 42px" vAlign="top"></TD>
			</TR>
			</TBODY></TABLE></TD></TR></TABLE></form>
	</body>
</HTML>
