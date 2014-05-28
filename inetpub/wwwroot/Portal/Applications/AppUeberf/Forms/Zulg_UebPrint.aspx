<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Zulg_UebPrint.aspx.vb" Inherits="AppUeberf.Zulg_UebPrint" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" type="text/javascript">

function printPage() {
if (window.print) {
jetztdrucken = confirm('Seite drucken ?');
if (jetztdrucken) window.print();
   }
}
		</script>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="WIDTH: 1058px; HEIGHT: 30px" colSpan="2">
						<TABLE id="Table3" style="WIDTH: 1027px; HEIGHT: 27px" cellSpacing="1" cellPadding="1" width="1027" border="0">
							<TR>
								<TD style="WIDTH: 234px">
									<P align="right"><asp:label id="lblSchritt" runat="server" Width="106px" Font-Bold="True"> Ausdruck</asp:label></P>
								</TD>
								<TD style="WIDTH: 55px"></TD>
								<TD>
									<asp:Image id="imgLogo" Runat="server"></asp:Image></TD>
							</TR>
						</TABLE>
						&nbsp;</TD>
				</TR>
				<TR>
					<TD class="PageNavigation" style="WIDTH: 1058px" colSpan="2"></TD>
				</TR>
				<tr>
					<TD style="WIDTH: 100%" vAlign="top">
						<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="lblKundeName1" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD><asp:label id="lblKundeStrasse" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="lblKundeAnsprechpartner" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD><asp:label id="lblKundePlzOrt" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label5" runat="server" Font-Bold="True">Abholung</asp:label></TD>
								<TD><asp:label id="Label15" runat="server" Font-Bold="True">Anlieferung</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label6" runat="server" Width="110px"> Name:</asp:label><asp:label id="lblAbName" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label13" runat="server" Width="106px">Name:</asp:label><asp:label id="lblAnName" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px" height="21"><asp:label id="Label7" runat="server" Width="110px">Straße, Nr:</asp:label><asp:label id="lblAbStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Straße, Nr:</asp:label><asp:label id="lblAnStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label8" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblAbOrt" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label18" runat="server" Width="106px">PLZ Ort:</asp:label><asp:label id="lblAnOrt" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label9" runat="server" Width="110px">Ansprechpartner:</asp:label><asp:label id="lblAbAnsprechpartner" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label19" runat="server" Width="106px">Ansprechpartner:</asp:label><asp:label id="lblAnAnspechpartner" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label16" runat="server" Width="110px">Telefon:</asp:label><asp:label id="lblAbTelefon" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label20" runat="server" Width="106px">Telefon:</asp:label><asp:label id="lblAnTelefon" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="lblFahrzeugdaten" runat="server" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ:</asp:label><asp:label id="lblHerst" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label11" runat="server" Width="242px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lblZugelassen" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen:</asp:label><asp:label id="lblKennzeichen" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label26" runat="server" Width="241px">Fahrzeugwert:</asp:label><asp:label id="lblFahrzeugwert" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer:</asp:label><asp:label id="lblVin" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label12" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lblBereifung" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr:</asp:label><asp:label id="lblRef" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label10" runat="server" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label14" runat="server" Width="125px">Überführung bis:</asp:label><asp:label id="lblDatumUeberf" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label22" runat="server" Width="128px">Wagenwäsche:</asp:label><asp:label id="lblWW" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label21" runat="server" Width="125px">Wagen volltanken:</asp:label><asp:label id="lblTanken" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label23" runat="server" Width="128px">Fahrzeugeinweisung:</asp:label><asp:label id="lblEinw" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 318px"><asp:label id="Label25" runat="server" Width="125px">Rotes Kennzeichen:</asp:label><asp:label id="lblRotKenn" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD></TD>
							</TR>
						</TABLE>
						<P><asp:label id="Label24" runat="server" Width="125px">Bemerkung:</asp:label><asp:label id="lblBem" runat="server" Font-Size="Smaller"></asp:label>
							<hr style="WIDTH: 100%; HEIGHT: 2px" color="#000000" SIZE="2">
						<P></P>
						<TABLE id="Table5" style="WIDTH: 100%; HEIGHT: 67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Anschluss" runat="server" Font-Bold="True">Anschlussfahrt</asp:label></TD>
								<TD><asp:label id="lbl1FzgDaten" runat="server" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Name" runat="server" Width="107px">Name:</asp:label><asp:label id="lbl2ReName" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Herst" runat="server" Width="95px">Hersteller:</asp:label><asp:label id="lbl2ReHerst" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1StrNr" runat="server" Width="107px">Straße, Nr.:</asp:label><asp:label id="lbl2ReStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Kenn" runat="server" Width="95px">Kennzeichen:</asp:label><asp:label id="lbl2ReKenn" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">
									<P><asp:label id="lbl1PLZOrt" runat="server" Width="107px">PLZ Ort:</asp:label><asp:label id="lbl2RePlzOrt" runat="server" Font-Size="Smaller"></asp:label></P>
								</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Ansprech" runat="server" Width="107px">Ansprechpartner:</asp:label><asp:label id="lbl2ReAnsprech" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Vin" runat="server" Width="95px">Fgst.-Nummer:</asp:label><asp:label id="lbl2ReVin" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Telefon" runat="server" Width="107px">Telefon:</asp:label><asp:label id="lbl2ReTelefon" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Ref" runat="server" Width="95px">Referenz-Nr.:</asp:label><asp:label id="lbl2ReRef" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"></TD>
								<TD><asp:label id="lbl1FzgZugelassen" runat="server" Width="221px">Fahrzeug zugelassen und fahrbereit?</asp:label><asp:label id="lbl2ReZugelassen" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">&nbsp;</TD>
								<TD><asp:label id="lbl1Bereifung" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lbl2ReBereif" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
						</TABLE>
						&nbsp;
						<TABLE id="Table12" style="WIDTH: 1035px; HEIGHT: 67px" cellSpacing="0" cellPadding="0" width="1035" border="0">
							<TR>
								<TD style="WIDTH: 317px" noWrap>
									<asp:label id="Label34" runat="server" Font-Bold="True" Width="203px">Zulassung</asp:label></TD>
								<TD noWrap></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px" noWrap>
									<asp:label id="Label33" runat="server" Width="141px">gew. Zulassungsdatum:</asp:label>
									<asp:label id="lblZulDatum" runat="server"></asp:label></TD>
								<TD noWrap>
									<asp:label id="Label27" runat="server" Width="141px">1. Wunschkennzeichen:</asp:label>
									<asp:label id="lblKennzeichen1" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px" noWrap>
									<asp:label id="Label29" runat="server" Width="81px">Haltername:</asp:label>
									<asp:label id="lblHaltername" runat="server" Width="191px"></asp:label></TD>
								<TD noWrap>
									<asp:label id="Label28" runat="server" Width="141px">2. Wunschkennzeichen: </asp:label>
									<asp:label id="lblKennzeichen2" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px" noWrap></TD>
								<TD noWrap>
									<asp:label id="Label30" runat="server" Width="141px">3. Wunschkennzeichen: </asp:label>
									<asp:label id="lblKennzeichen3" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</tr>
			</TABLE>
			<P>
				<TABLE id="Table6" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
					<TR>
						<TD style="WIDTH: 320px; HEIGHT: 38px"><asp:label id="lblZulassungsdienst" runat="server" Width="272px" Font-Bold="True">In Kürze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:</asp:label></TD>
						<TD style="HEIGHT: 38px"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 320px; HEIGHT: 19px"><asp:label id="lblZulName1" runat="server" Font-Size="Smaller"></asp:label></TD>
						<TD style="HEIGHT: 19px"><asp:label id="lblZulName2" runat="server" Font-Size="Smaller"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 320px; HEIGHT: 18px"><asp:label id="lblZulStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
						<TD style="HEIGHT: 18px"><asp:label id="lblZulPlzOrt" runat="server" Font-Size="Smaller"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 320px; HEIGHT: 36px"><asp:label id="lblZulTel" runat="server" Font-Size="Smaller"></asp:label></TD>
						<TD style="HEIGHT: 36px"><asp:label id="lblZulMail" runat="server" Font-Size="Smaller"></asp:label></TD>
					</TR>
				</TABLE>
			<P></P>
			<P>
				<TABLE id="Table11" style="WIDTH: 919px; HEIGHT: 54px" cellSpacing="1" cellPadding="1" width="919" border="0">
					<TR>
						<TD>
							<asp:label id="lblStva" runat="server" Font-Bold="True" Width="692px" Font-Italic="True"></asp:label></TD>
					</TR>
					<TR>
						<TD>
							<P>
								<asp:label id="lblUnterlagen" runat="server" Font-Bold="True" Width="513px">Bitte halten Sie folgende Unterlagen für die Zulassung bereit:</asp:label></P>
						</TD>
					</TR>
				</TABLE>
			</P>
			<P>
				<TABLE class="TableBanner" id="Table7" style="WIDTH: 672px; HEIGHT: 128px" cellSpacing="1" cellPadding="1" width="672" bgColor="#ffffff" border="0" runat="server">
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
			</P>
			<TABLE class="TableLegende" id="Table8" cellSpacing="1" cellPadding="2" bgColor="#ffffff" border="0" runat="server">
				<TR>
					<TD><FONT size="1"><U>**Legende:</U></FONT></TD>
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
				</TR>
				<TR>
					<TD><FONT size="1">ZB1=Fahrzeugschein,</FONT></TD>
					<TD><FONT size="1">ZB2=Fahrzeugbrief,</FONT></TD>
					<TD noWrap><FONT size="1">CoC=Certificate of Conformity,</FONT></TD>
					<TD><FONT size="1">DK=Deckungskarte,</FONT></TD>
					<TD><FONT size="1">VM=Vollmacht,</FONT></TD>
				</TR>
				<TR>
					<TD>&nbsp;<FONT size="1">PA=Personalausweis,</FONT></TD>
					<TD><FONT size="1">GewA=Gewerbeanmeldung,</FONT></TD>
					<TD noWrap><FONT size="1">HRA=Handelsregister,</FONT></TD>
					<TD><FONT size="1">LEV=Lastschrifteinzug</FONT></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD colSpan="9"><U>Wir weisen darauf hin, dass diese Angaben unverbindliche Auskünfte 
							der entsprechenden Zulassungskreise sind.</U></TD>
				</TR>
			</TABLE>
			</TD></TR></TABLE>
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="WIDTH: 612px">&nbsp;</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 612px">
						<TABLE id="Table9" style="WIDTH: 641px; HEIGHT: 155px" cellSpacing="1" cellPadding="1" width="641" border="0">
							<TR>
								<TD style="WIDTH: 448px" noWrap></TD>
								<TD noWrap>
									<P align="right">Kroschke Car Logistic GmbH</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 448px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Zentraldisposition
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 448px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Bogenstr. 26
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 448px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">22926 Ahrensburg</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 448px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Telefon&nbsp;04102&nbsp;804-0
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 448px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Telefax&nbsp;04102 804-266
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 448px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right"><A href="mailto:kcl-dispo@kroschke.de">kcl-dispo@kroschke.de</A></P>
								</TD>
							</TR>
						</TABLE>
						&nbsp;</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 612px"></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 612px"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="WIDTH: 1040px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="1040" border="0">
				<TR>
					<TD style="WIDTH: 384px">
						<P align="right">&nbsp;</P>
					</TD>
					<TD><A href="javascript:window.print()">Seite drucken</A></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 384px"></TD>
					<TD style="WIDTH: 110px">&nbsp;</TD>
					<TD></TD>
				</TR>
			</TABLE>
			</TD></TR></TABLE></form>
	</body>
</HTML>
