<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_Print.aspx.vb" Inherits="AppUeberf.Ueberfg_Print" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript" language="JavaScript">

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
								<TD style="WIDTH: 204px">
									<P align="right"><asp:label id="lblSchritt" runat="server" Font-Bold="True" Width="88px"> Ausdruck</asp:label></P>
								</TD>
								<TD style="WIDTH: 58px"></TD>
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
						<TABLE id="Table1" style="WIDTH:100%; HEIGHT:67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
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
								<TD style="WIDTH: 318px" height="21"><asp:label id="Label7" runat="server" Width="110px">Straﬂe, Nr:</asp:label><asp:label id="lblAbStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Straﬂe, Nr:</asp:label><asp:label id="lblAnStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
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
								<TD style="WIDTH: 318px"><asp:label id="Label14" runat="server" Width="125px">‹berf¸hrung bis:</asp:label><asp:label id="lblDatumUeberf" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label22" runat="server" Width="128px">Wagenw‰sche:</asp:label><asp:label id="lblWW" runat="server" Font-Size="Smaller"></asp:label></TD>
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
							<hr color="#000000" style="WIDTH: 100%; HEIGHT: 2px" SIZE="2">
						<P></P>
						<TABLE id="Table5" style="WIDTH:100%; HEIGHT:67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Anschluss" runat="server" Font-Bold="True">Anschlussfahrt</asp:label></TD>
								<TD><asp:label id="lbl1FzgDaten" runat="server" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Name" runat="server" Width="107px">Name:</asp:label><asp:label id="lbl2ReName" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Herst" runat="server" Width="95px">Hersteller:</asp:label><asp:label id="lbl2ReHerst" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1StrNr" runat="server" Width="107px">Straﬂe, Nr.:</asp:label><asp:label id="lbl2ReStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Kenn" runat="server" Width="95px">Kennzeichen:</asp:label><asp:label id="lbl2ReKenn" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">
									<P><asp:label id="lbl1PLZOrt" runat="server" Width="107px">PLZ Ort:</asp:label>
										<asp:label id="lbl2RePlzOrt" runat="server" Font-Size="Smaller"></asp:label></P>
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
								<TD><asp:label id="lbl1FzgZugelassen" runat="server" Width="242px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lbl2ReZugelassen" runat="server" Font-Size="Smaller"></asp:label></TD>
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
						&nbsp;</TD>
				</tr>
			</TABLE>
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="WIDTH: 612px"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 612px"></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="WIDTH: 1040px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="1040" border="0">
				<TR>
					<TD style="WIDTH: 384px">
						<P align="right">&nbsp;</P>
					</TD>
					<TD><a href="javascript:window.print()">Seite drucken</a></TD>
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
