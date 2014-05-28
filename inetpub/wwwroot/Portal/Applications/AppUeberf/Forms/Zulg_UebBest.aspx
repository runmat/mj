<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Zulg_UebBest.aspx.vb" Inherits="AppUeberf.Zulg_UebBest" %>
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
											<TD style="WIDTH: 283px"><asp:label id="lblSchritt" runat="server" Font-Bold="True" Width="274px">Schritt 5 von 5</asp:label></TD>
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
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="lblKundeName1" runat="server" Font-Bold="True" Width="225px" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundeStrasse" runat="server" Font-Bold="True" Width="442px" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="lblKundeAnsprechpartner" runat="server" Font-Bold="True" Width="307px" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundePlzOrt" runat="server" Font-Bold="True" Width="405px" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label31" runat="server" Font-Bold="True" Width="180px">Rechnungszahler</asp:label></TD>
											<TD><asp:label id="Label32" runat="server" Font-Bold="True" Width="272px">Postalischer Rechnungsempfänger</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label36" runat="server" Width="110px"> Name:</asp:label><asp:label id="lblRegName" runat="server"></asp:label></TD>
											<TD><asp:label id="Label39" runat="server" Width="110px"> Name:</asp:label><asp:label id="lblRechName" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label37" runat="server" Width="110px">Straße, Nr:</asp:label><asp:label id="lblRegStrasse" runat="server"></asp:label></TD>
											<TD><asp:label id="Label35" runat="server" Width="110px">Straße, Nr:</asp:label><asp:label id="lblRechStrasse" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label38" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblRegOrt" runat="server"></asp:label></TD>
											<TD><asp:label id="Label50" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblRechOrt" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">
												<asp:label id="lbl_Auftragsnr" runat="server" Width="462px" Font-Bold="True" Visible="False"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label5" runat="server" Font-Bold="True" Width="180px">Abholung</asp:label></TD>
											<TD><asp:label id="Label15" runat="server" Font-Bold="True" Width="146px">Anlieferung</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label6" runat="server" Width="110px"> Name:</asp:label><asp:label id="lblAbName" runat="server"></asp:label></TD>
											<TD><asp:label id="Label13" runat="server" Width="106px">Name:</asp:label><asp:label id="lblAnName" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437" height="21"><asp:label id="Label7" runat="server" Width="110px">Straße, Nr:</asp:label><asp:label id="lblAbStrasse" runat="server"></asp:label></TD>
											<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Straße, Nr:</asp:label><asp:label id="lblAnStrasse" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label8" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblAbOrt" runat="server"></asp:label></TD>
											<TD><asp:label id="Label18" runat="server" Width="106px">PLZ Ort:</asp:label><asp:label id="lblAnOrt" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label9" runat="server" Width="110px">Ansprechpartner:</asp:label><asp:label id="lblAbAnsprechpartner" runat="server"></asp:label></TD>
											<TD><asp:label id="Label19" runat="server" Width="106px">Ansprechpartner:</asp:label><asp:label id="lblAnAnspechpartner" runat="server" Width="265px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label16" runat="server" Width="110px">1. Telefon:</asp:label><asp:label id="lblAbTelefon" runat="server"></asp:label></TD>
											<TD><asp:label id="Label20" runat="server" Width="106px">1. Telefon:</asp:label><asp:label id="lblAnTelefon" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">
												<asp:label id="Label54" runat="server" Width="110px">2. Telefon:</asp:label>
												<asp:label id="lblAbTelefon2" runat="server"></asp:label></TD>
											<TD>
												<asp:label id="Label55" runat="server" Width="106px">2. Telefon:</asp:label>
												<asp:label id="lblAnTelefon2" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label51" runat="server" Width="110px">Fax:</asp:label><asp:label id="lblAbFax" runat="server"></asp:label></TD>
											<TD><asp:label id="Label52" runat="server" Width="106px">Fax:</asp:label><asp:label id="lblAnFax" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="lblFahrzeugdaten" runat="server" Font-Bold="True" Width="186px">Fahrzeugdaten</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ:</asp:label><asp:label id="lblHerst" runat="server"></asp:label></TD>
											<TD><asp:label id="Label11" runat="server" Width="241px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lblZugelassen" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">
												<asp:label id="Label56" runat="server" Width="110px">Fahrzeugklasse:</asp:label>
												<asp:label id="lblFahrzeugklasse" runat="server"></asp:label></TD>
											<TD>
												<asp:label id="Label59" runat="server" Width="141px">Zulassung durch KCL:</asp:label>
												<asp:label id="lblHinZulKCL" runat="server" Width="241px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen:</asp:label><asp:label id="lblKennzeichen" runat="server"></asp:label></TD>
											<TD>
												<asp:label id="Label26" runat="server" Width="87px">Fahrzeugwert:</asp:label>
												<asp:label id="lblFahrzeugwert" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer</asp:label><asp:label id="lblVin" runat="server"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr</asp:label><asp:label id="lblRef" runat="server"></asp:label></TD>
											<TD>
												<asp:label id="Label12" runat="server" Width="87px">Bereifung:</asp:label>
												<asp:label id="lblBereifung" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label10" runat="server" Font-Bold="True" Width="188px">Dienstleistungsdetails</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label14" runat="server" Width="157px">Überführung bis:</asp:label><asp:label id="lblDatumUeberf" runat="server"></asp:label></TD>
											<TD><asp:label id="Label22" runat="server" Width="128px">Wagenwäsche:</asp:label><asp:label id="lblWW" runat="server" Width="66px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label21" runat="server" Width="157px">Wagen volltanken:</asp:label><asp:label id="lblTanken" runat="server"></asp:label></TD>
											<TD><asp:label id="Label23" runat="server" Width="128px">Fahrzeugeinweisung:</asp:label><asp:label id="lblEinw" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label25" runat="server" Width="157px">Rotes Kennzeichen:</asp:label><asp:label id="lblRotKenn" runat="server"></asp:label></TD>
											<TD><asp:label id="Label53" runat="server" Width="128px">Expressüberführung:</asp:label><asp:label id="lblExpress" runat="server"></asp:label></TD>
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
											<TD><asp:label id="lbl1Herst" runat="server" Width="95px">Hersteller:</asp:label><asp:label id="lbl2ReHerst" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1StrNr" runat="server" Width="107px">Straße, Nr.:</asp:label><asp:label id="lbl2ReStrasse" runat="server"></asp:label></TD>
											<TD>
												<asp:label id="Label57" runat="server" Width="95px">Fahrzeugklasse:</asp:label>
												<asp:label id="lbl2ReFahrzeugklasse" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1PLZOrt" runat="server" Width="107px">PLZ Ort:</asp:label><asp:label id="lbl2RePlzOrt" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl1Kenn" runat="server" Width="95px">Kennzeichen:</asp:label><asp:label id="lbl2ReKenn" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1Ansprech" runat="server" Width="107px">Ansprechpartner:</asp:label><asp:label id="lbl2ReAnsprech" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl1Vin" runat="server" Width="95px">Fgst.-Nummer:</asp:label><asp:label id="lbl2ReVin" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1Telefon" runat="server" Width="107px">Telefon:</asp:label><asp:label id="lbl2ReTelefon" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl1Ref" runat="server" Width="95px">Referenz-Nr.:</asp:label><asp:label id="lbl2ReRef" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px">
												<asp:label id="Label58" runat="server" Width="107px">Fax:</asp:label>
												<asp:label id="lbl2ReFax" runat="server"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"></TD>
											<TD><asp:label id="lbl1FzgZugelassen" runat="server" Width="245px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lbl2ReZugelassen" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"></TD>
											<TD>
												<asp:label id="Label60" runat="server" Width="141px">Zulassung durch KCL:</asp:label>
												<asp:label id="lblAnZulKCL" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"></TD>
											<TD><asp:label id="lbl1Bereifung" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lbl2ReBereif" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									&nbsp;
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label34" runat="server" Font-Bold="True" Width="382px">Zulassung</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label33" runat="server" Width="157px">gew. Zulassungsdatum:</asp:label><asp:label id="lblZulDatum" runat="server"></asp:label></TD>
											<TD><asp:label id="Label27" runat="server" Width="145px">1. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen1" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label29" runat="server" Width="157px">Haltername:</asp:label><asp:label id="lblHaltername" runat="server" Width="267px"></asp:label></TD>
											<TD><asp:label id="Label28" runat="server" Width="145px">2. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen2" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD><asp:label id="Label30" runat="server" Width="145px">3. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen3" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 38px"><asp:label id="lblZulassungsdienst" runat="server" Font-Bold="True" Width="277px" Visible="False">In Kürze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:</asp:label></TD>
											<TD style="HEIGHT: 38px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 19px"><asp:label id="lbl2ZulName1" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 19px"><asp:label id="lbl2ZulStrasse" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 18px"><asp:label id="lbl2ZulName2" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 18px"><asp:label id="lbl2ZulPlzOrt" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD><asp:label id="lbl2ZulTel" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl2ZulMail" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table11" style="WIDTH: 919px; HEIGHT: 54px" cellSpacing="1" cellPadding="1" width="919" border="0">
										<TR>
											<TD><asp:label id="lblStva" runat="server" Font-Bold="True" Width="692px" Font-Italic="True" Visible="False"></asp:label></TD>
										</TR>
										<TR>
											<TD>
												<P><asp:label id="lblUnterlagen" runat="server" Font-Bold="True" Width="513px" Visible="False">Bitte halten Sie folgende Unterlagen für die Zulassung bereit:</asp:label></P>
											</TD>
										</TR>
									</TABLE>
									<TABLE class="TableBanner" id="Table8" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffffff" border="0" runat="server">
										<TR>
											<TD class="TableBannerCell" vAlign="middle" width="19" colSpan="1" height="30" rowSpan="1"><U>Kategorie\Dokument</U>**</TD>
											<TD class="TableBannerCell" noWrap align="center" height="30">ZB1
											</TD>
											<TD class="TableBannerCell" height="30">ZB2</TD>
											<TD class="TableBannerCell" height="30">CoC
											</TD>
											<TD class="TableBannerCell" height="30">DK&nbsp;
											</TD>
											<TD class="TableBannerCell" height="30">VM
											</TD>
											<TD class="TableBannerCell" width="41" height="30">PA
											</TD>
											<TD class="TableBannerCell" height="30">GewA
											</TD>
											<TD class="TableBannerCell" height="30">HRA
											</TD>
											<TD class="TableBannerCell" height="30">LEV</TD>
											<TD class="TableBannerCell" height="30">Bemerkung
											</TD>
										</TR>
										<TR>
											<TD class="TableHeaderCell" vAlign="middle" colSpan="11">Privat</TD>
										</TR>
										<TR>
											<TD class="TableBannerCell" vAlign="middle" colSpan="1" rowSpan="1">Zulassung</TD>
											<TD class="TableBannerInnerCell" noWrap align="center" bgColor="#ffffff" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label00" runat="server" Width="50px"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label01" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label02" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label03" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label04" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label05" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label06" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label07" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label08" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label09" runat="server"></asp:label></P>
												</FONT>
											</TD>
										</TR>
										<TR>
											<TD class="TableHeaderCell" colSpan="11">Unternehmen</TD>
										</TR>
										<TR>
											<TD class="TableBannerCell" colSpan="1" height="25" rowSpan="1">Zulassung</TD>
											<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label40" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label41" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label42" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label43" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label44" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label45" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label46" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label47" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label48" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label49" runat="server"></asp:label></P>
												</FONT>
											</TD>
										</TR>
									</TABLE>
									<TABLE class="TableLegende" id="Table9" cellSpacing="1" cellPadding="2" bgColor="#ffffff" border="0" runat="server">
										<TR>
											<TD><FONT size="1"><U>**Legende:</U></FONT></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><FONT size="1">O=Original</FONT></TD>
											<TD><FONT size="1">K=Kopie</FONT></TD>
											<TD><FONT size="1">F=Formular Zulassungsstelle</FONT>
											</TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><FONT size="1">ZB1=Fahrzeugschein,</FONT></TD>
											<TD><FONT size="1">ZB2=Fahrzeugbrief,</FONT></TD>
											<TD noWrap><FONT size="1">CoC=Certificate of Conformity,</FONT></TD>
											<TD><FONT size="1">DK=Deckungskarte,</FONT></TD>
											<TD><FONT size="1">VM=Vollmacht,</FONT></TD>
											<TD><FONT size="1">PA=Personalausweis,</FONT></TD>
											<TD><FONT size="1">GewA=Gewerbeanmeldung,</FONT></TD>
											<TD><FONT size="1">HRA=Handelsregister,</FONT></TD>
											<TD><FONT size="1">LEV=Lastschrifteinzug</FONT></TD>
										</TR>
										<TR>
											<TD>&nbsp;</TD>
											<TD></TD>
											<TD noWrap></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD colSpan="9"><U>Wir weisen darauf hin, dass diese Angaben unverbindliche Auskünfte 
													der entsprechenden Zulassungskreise sind.</U></TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
						</TABLE>
						<TABLE id="Table2" style="WIDTH: 1040px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="1040" border="0">
							<TR>
								<TD style="WIDTH: 479px">
									<P align="right"><asp:linkbutton id="cmdNewOrder" runat="server" Visible="False" CssClass="StandardButton" Height="23px"> Neuer Auftrag</asp:linkbutton><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
								</TD>
								<TD style="WIDTH: 110px"><asp:button id="cmdSave" runat="server" Width="95px" Text="Beauftragen"></asp:button></TD>
								<TD style="WIDTH: 296px"><asp:linkbutton id="cmdNewOrderHoldData" runat="server" Width="280px" Visible="False" CssClass="StandardButton" Height="23px"> Neuer Auftrag mit identischen Adressen</asp:linkbutton></TD>
								<TD><asp:button id="cmdPrint" runat="server" Width="98px" Visible="False" Text="Druckansicht"></asp:button></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 479px"></TD>
								<TD style="WIDTH: 110px"></TD>
								<TD style="WIDTH: 296px"></TD>
							</TR>
						</TABLE>
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD style="WIDTH: 118px"></TD>
								<TD style="WIDTH: 612px"><asp:label id="lblError" runat="server" Width="802px" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 1033px" vAlign="top" width="1033"></TD>
					<TD style="WIDTH: 42px" vAlign="top"></TD>
				</TR>
			</table>
			</TD></TR></TABLE></form>
	</body>
</HTML>
