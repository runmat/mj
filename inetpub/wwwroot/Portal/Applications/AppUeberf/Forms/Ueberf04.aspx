<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberf04.aspx.vb" Inherits="AppUeberf.Ueberf04" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
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
			<table width="100%" align="center" cellSpacing="0" cellPadding="0">
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
								<TD style="WIDTH: 144px" vAlign="top" width="174">
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
											<TD style="WIDTH: 437px" width="437" colspan="2">
												<uc1:progresscontrol id="ProgressControl1" runat="server"></uc1:progresscontrol></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379">
												<asp:label id="lbl_Auftragsnr" runat="server" Width="462px" Font-Bold="True" Visible="False"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label5" runat="server" Width="180px" Font-Bold="True">Abholung</asp:label></TD>
											<TD><asp:label id="Label15" runat="server" Width="146px" Font-Bold="True">Anlieferung</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label6" runat="server" Width="110px"> Name:</asp:label><asp:label id="lblAbName" runat="server"></asp:label></TD>
											<TD><asp:label id="Label13" runat="server" Width="106px">Name:</asp:label><asp:label id="lblAnName" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379" height="21"><asp:label id="Label7" runat="server" Width="110px">Straﬂe, Nr:</asp:label><asp:label id="lblAbStrasse" runat="server"></asp:label></TD>
											<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Straﬂe, Nr:</asp:label><asp:label id="lblAnStrasse" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label8" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblAbOrt" runat="server"></asp:label></TD>
											<TD><asp:label id="Label18" runat="server" Width="106px">PLZ Ort:</asp:label><asp:label id="lblAnOrt" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label9" runat="server" Width="110px">Ansprechpartner:</asp:label><asp:label id="lblAbAnsprechpartner" runat="server"></asp:label></TD>
											<TD><asp:label id="Label19" runat="server" Width="106px">Ansprechpartner:</asp:label><asp:label id="lblAnAnspechpartner" runat="server" Width="265px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label16" runat="server" Width="110px">Telefon 1:</asp:label><asp:label id="lblAbTelefon" runat="server"></asp:label></TD>
											<TD><asp:label id="Label20" runat="server" Width="106px">Telefon 1:</asp:label><asp:label id="lblAnTelefon" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label30" runat="server" Width="110px">Telefon 2:</asp:label><asp:label id="lblAbTelefon2" runat="server"></asp:label></TD>
											<TD><asp:label id="Label29" runat="server" Width="106px">Telefon 2:</asp:label><asp:label id="lblAnTelefon2" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="lblFahrzeugdaten" runat="server" Width="186px" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ:</asp:label><asp:label id="lblHerst" runat="server"></asp:label></TD>
											<TD><asp:label id="Label11" runat="server" Width="243px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lblZugelassen" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen:</asp:label><asp:label id="lblKennzeichen" runat="server"></asp:label></TD>
											<TD><asp:label id="Label26" runat="server" Width="243px">Fahrzeugwert:</asp:label><asp:label id="lblFahrzeugwert" runat="server" Width="90px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer</asp:label><asp:label id="lblVin" runat="server"></asp:label></TD>
											<TD><asp:label id="Label12" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lblBereifung" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr</asp:label><asp:label id="lblRef" runat="server"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label10" runat="server" Width="188px" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 379px; BORDER-BOTTOM: black thin solid" width="379"><asp:label id="Label14" runat="server" Width="135px">‹berf¸hrung bis:</asp:label><asp:label id="lblDatumUeberf" runat="server"></asp:label>&nbsp;
												<asp:label id="lblUeberfDatumFix" runat="server"></asp:label></TD>
											<TD><asp:label id="Label22" runat="server" Width="128px">Wagenw‰sche:</asp:label><asp:label id="lblWW" runat="server" Width="66px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label21" runat="server" Width="135px">Wagen volltanken:</asp:label><asp:label id="lblTanken" runat="server"></asp:label></TD>
											<TD><asp:label id="Label23" runat="server" Width="128px">Fahrzeugeinweisung:</asp:label><asp:label id="lblEinw" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label25" runat="server" Width="135px">Rotes Kennzeichen:</asp:label><asp:label id="lblRotKenn" runat="server"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD colSpan="2">
												<asp:label id="Label39" runat="server" Width="135px" Height="43px">Winterreifen:</asp:label><asp:label id="lblWinterText" runat="server" Width="683px" Height="41px"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379"><asp:label id="Label24" runat="server" Width="149px" Font-Bold="True">Bemerkung:</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 379px" width="379">
												<P><asp:label id="lblBem" runat="server" Width="340px"></asp:label></P>
											</TD>
											<TD></TD>
										</TR>
									</TABLE>
									<P>
										<hr color="#000000">
									<P></P>
									<TABLE id="Table5" style="WIDTH: 918px; HEIGHT: 67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1Anschluss" runat="server" Width="229px" Font-Bold="True">Anschlussfahrt</asp:label></TD>
											<TD><asp:label id="lbl1FzgDaten" runat="server" Width="202px" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1Name" runat="server" Width="107px">Name:</asp:label><asp:label id="lbl2ReName" runat="server"></asp:label></TD>
											<TD><asp:label id="lbl1Herst" runat="server" Width="95px">Hersteller:</asp:label><asp:label id="lbl2ReHerst" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lbl1StrNr" runat="server" Width="107px">Straﬂe, Nr.:</asp:label><asp:label id="lbl2ReStrasse" runat="server"></asp:label></TD>
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
											<TD style="WIDTH: 436px"><asp:label id="Label31" runat="server" Width="107px">Telefon 2:</asp:label><asp:label id="lbl2ReTelefon2" runat="server"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"></TD>
											<TD><asp:label id="lbl1FzgZugelassen" runat="server" Width="243px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lbl2ReZugelassen" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="Label27" runat="server" Width="149px" Font-Bold="True">Bemerkung:</asp:label></TD>
											<TD><asp:label id="lbl1Bereifung" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lbl2ReBereif" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"><asp:label id="lblReBemerkung" runat="server" Width="356px"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 436px"></TD>
											<TD></TD>
										</TR>
									</TABLE>
									&nbsp;<asp:label id="lblError" runat="server" Width="821px" EnableViewState="False" CssClass="TextError"></asp:label></TD>
							</tr>
						</TABLE>
						<TABLE id="Table2" style="WIDTH: 1040px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="1040" border="0">
							<TR>
								<TD style="WIDTH: 479px">
									<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" ImageUrl="/Portal/Customize/Ueberf/arrowUeberfLeft.gif" Height="34px"></asp:imagebutton></P>
								</TD>
								<TD style="WIDTH: 110px"><asp:button id="cmdSave" runat="server" Width="95px" Text="Beauftragen"></asp:button></TD>
								<TD><asp:button id="cmdPrint" runat="server" Width="98px" Text="Druckansicht" Visible="False"></asp:button></TD>
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
