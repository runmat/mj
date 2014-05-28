<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ZulUebBest.aspx.vb" Inherits="AppUeberf.ZulUebBest" %>
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
							<tr>
								<TD style="WIDTH: 114px" vAlign="top" width="114">
									<TABLE id="Table12" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD width="150">
												<asp:Panel id="pnlPlaceholder" runat="server" Width="144px"></asp:Panel></TD>
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
											<TD style="WIDTH: 437px" width="437"><uc1:progresscontrol id="ProgressControl1" runat="server"></uc1:progresscontrol></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD></TD>
										</TR>
									</TABLE>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label34" runat="server" Font-Bold="True" Width="382px">Zulassung</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label31" runat="server" Width="170px">Leasingnehmer:</asp:label><asp:label id="lblLeasingnehmer" runat="server"></asp:label></TD>
											<TD><asp:label id="Label27" runat="server" Width="145px">1. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen1" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 437px; BORDER-BOTTOM: black thin solid" width="437"><asp:label id="Label33" runat="server" Width="170px">gew. Zulassungsdatum:</asp:label><asp:label id="lblZulDatum" runat="server"></asp:label></TD>
											<TD><asp:label id="Label28" runat="server" Width="145px">2. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen2" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label29" runat="server" Width="170px">Haltername:</asp:label><asp:label id="lblHaltername" runat="server" Width="226px"></asp:label></TD>
											<TD><asp:label id="Label30" runat="server" Width="145px">3. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen3" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label32" runat="server" Width="170px">Versicherungsnehmer:</asp:label><asp:label id="lblVersicherungsnehmer" runat="server" Width="198px"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label35" runat="server" Width="170px">Versicherer:</asp:label><asp:label id="lblVersicherer" runat="server" Width="198px"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label36" runat="server" Width="170px" Height="21px">KFZ-Steuer-Zahlung durch:</asp:label><asp:label id="lblKfzSteuer" runat="server" Width="222px"></asp:label></TD>
											<TD><asp:label id="Label37" runat="server" Font-Bold="True" Width="157px">Bemerkung:</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD><asp:label id="lblBemerkung" runat="server" Width="353px"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 38px"><asp:label id="lblZulassungsdienst" runat="server" Font-Bold="True" Width="277px" Visible="False">In K�rze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:</asp:label></TD>
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
											<TD style="WIDTH: 438px; HEIGHT: 36px"><asp:label id="lbl2ZulTel" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 36px"><asp:label id="lbl2ZulMail" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table11" style="WIDTH: 919px; HEIGHT: 54px" cellSpacing="1" cellPadding="1" width="919" border="0">
										<TR>
											<TD><asp:label id="lblStva" runat="server" Font-Italic="True" Font-Bold="True" Width="692px" Visible="False"></asp:label></TD>
										</TR>
										<TR>
											<TD>
												<P><asp:label id="lblUnterlagen" runat="server" Font-Bold="True" Width="513px" Visible="False">Bitte halten Sie folgende Unterlagen f�r die Zulassung bereit:</asp:label></P>
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
													<P align="center"><asp:label id="Label01" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label00" runat="server" Width="100%"></asp:label></P>
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
													<P align="center"><asp:label id="Label41" runat="server"></asp:label></P>
												</FONT>
											</TD>
											<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
													<P align="center"><asp:label id="Label40" runat="server"></asp:label></P>
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
											<TD colSpan="9">
												<P><U>Wir weisen darauf hin, dass diese Angaben unverbindliche Ausk�nfte der 
														entsprechenden Zulassungskreise sind.</U></P>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD colSpan="2">
												<P>
													<HR style="WIDTH: 100%; HEIGHT: 2px" color="#000000" SIZE="2">
												<P></P>
											</TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
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
											<TD style="WIDTH: 437px" width="437" height="21"><asp:label id="Label7" runat="server" Width="110px">Stra�e, Nr:</asp:label><asp:label id="lblAbStrasse" runat="server"></asp:label></TD>
											<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Stra�e, Nr:</asp:label><asp:label id="lblAnStrasse" runat="server"></asp:label></TD>
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
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label16" runat="server" Width="110px">Telefon 1:</asp:label><asp:label id="lblAbTelefon" runat="server"></asp:label></TD>
											<TD><asp:label id="Label20" runat="server" Width="106px">Telefon 1:</asp:label><asp:label id="lblAnTelefon" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label50" runat="server" Width="110px">Telefon 2:</asp:label><asp:label id="lblAbTelefon2" runat="server"></asp:label></TD>
											<TD><asp:label id="Label51" runat="server" Width="105px">Telefon 2:</asp:label><asp:label id="lblAnTelefon2" runat="server"></asp:label></TD>
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
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen:</asp:label><asp:label id="lblKennzeichen" runat="server"></asp:label></TD>
											<TD><asp:label id="Label26" runat="server" Width="227px">Fahrzeugwert:</asp:label><asp:label id="lblFahrzeugwert" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer</asp:label><asp:label id="lblVin" runat="server"></asp:label></TD>
											<TD><asp:label id="Label12" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lblBereifung" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr</asp:label><asp:label id="lblRef" runat="server"></asp:label></TD>
											<TD></TD>
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
											<TD style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 437px; BORDER-BOTTOM: black thin solid" width="437"><asp:label id="Label14" runat="server" Width="157px">�berf�hrung bis:</asp:label><asp:label id="lblDatumUeberf" runat="server"></asp:label>&nbsp;
												<asp:label id="lblUeberfDatumFix" runat="server" Font-Size="Smaller"></asp:label></TD>
											<TD><asp:label id="Label22" runat="server" Width="128px">Wagenw�sche:</asp:label><asp:label id="lblWW" runat="server" Width="66px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label21" runat="server" Width="157px">Wagen volltanken:</asp:label><asp:label id="lblTanken" runat="server"></asp:label></TD>
											<TD><asp:label id="Label23" runat="server" Width="128px">Fahrzeugeinweisung:</asp:label><asp:label id="lblEinw" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px; HEIGHT: 21px" width="437"><asp:label id="Label25" runat="server" Width="157px">Rotes Kennzeichen:</asp:label><asp:label id="lblRotKenn" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 21px"></TD>
										</TR>
										<TR>
											<TD colSpan="2">
												<asp:label id="Label39" runat="server" Width="157px" Height="48px">Winterreifen:</asp:label><asp:label id="lblWinterText" runat="server" Width="627px" Height="47px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label24" runat="server" Font-Bold="True" Width="149px">Bemerkung:</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437" colSpan="2">
												<P>
													<asp:label id="lblBem" runat="server" Width="741px"></asp:label></P>
											</TD>
										</TR>
									</TABLE>
									<P>
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
											<TD style="WIDTH: 436px"><asp:label id="lbl1StrNr" runat="server" Width="107px">Stra�e, Nr.:</asp:label><asp:label id="lbl2ReStrasse" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl1Kenn" runat="server" Width="95px">Kennzeichen:</asp:label><asp:label id="lbl2ReKenn" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1PLZOrt" runat="server" Width="107px">PLZ Ort:</asp:label><asp:label id="lbl2RePlzOrt" runat="server"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1Ansprech" runat="server" Width="107px">Ansprechpartner:</asp:label><asp:label id="lbl2ReAnsprech" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl1Vin" runat="server" Width="95px">Fgst.-Nummer:</asp:label><asp:label id="lbl2ReVin" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1Telefon" runat="server" Width="107px">Telefon 1:</asp:label><asp:label id="lbl2ReTelefon1" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl1Ref" runat="server" Width="95px">Referenz-Nr.:</asp:label><asp:label id="lbl2ReRef" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="Label52" runat="server" Width="107px">Telefon 2:</asp:label><asp:label id="lbl2ReTelefon2" runat="server"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"></TD>
											<TD><asp:label id="lbl1FzgZugelassen" runat="server" Width="245px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lbl2ReZugelassen" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px">&nbsp;</TD>
											<TD><asp:label id="lbl1Bereifung" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lbl2ReBereif" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="Label38" runat="server" Font-Bold="True" Width="149px">Bemerkung:</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px" colSpan="2">
												<asp:label id="lblReBemerkung" runat="server" Width="656px"></asp:label></TD>
										</TR>
									</TABLE>
									&nbsp;
								</TD>
							</tr>
							<TR>
								<TD style="WIDTH: 113px" vAlign="top" width="113"></TD>
								<TD style="WIDTH: 917px" vAlign="top"><asp:label id="lblError" runat="server" Width="802px" EnableViewState="False" CssClass="TextError"></asp:label></TD>
							</TR>
						</TABLE>
						<TABLE id="Table2" style="WIDTH: 1040px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="1040" border="0">
							<TR>
								<TD style="WIDTH: 479px">
									<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
								</TD>
								<TD style="WIDTH: 110px"><asp:button id="cmdSave" runat="server" Width="95px" Text="Beauftragen"></asp:button></TD>
								<TD><asp:button id="cmdPrint" runat="server" Width="98px" Visible="False" Text="Druckansicht"></asp:button></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 479px"></TD>
								<TD style="WIDTH: 110px"></TD>
								<TD></TD>
							</TR>
						</TABLE>
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 1033px" vAlign="top" width="1033"></TD>
					<TD vAlign="top" style="WIDTH: 42px"></TD>
				</TR>
			</table>
			</TD></TR></TABLE></form>
	</body>
</HTML>
