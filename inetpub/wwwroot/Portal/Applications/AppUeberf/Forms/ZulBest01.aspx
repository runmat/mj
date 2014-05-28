<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ZulBest01.aspx.vb" Inherits="AppUeberf.ZulBest01" %>
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
								<TD style="WIDTH: 174px" vAlign="top" width="174">
									<TABLE id="Table12" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD width="150">
												<asp:Panel id="pnlPlaceholder" runat="server" Width="144px"></asp:Panel></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<TD style="WIDTH: 917px" vAlign="top">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 437px" width="437" colspan="2">
												<uc1:ProgressControl id="ProgressControl1" runat="server"></uc1:ProgressControl></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">
												<asp:Panel id="pnlPlaceholder3" runat="server" Width="429px"></asp:Panel></TD>
											<TD>
												<asp:Panel id="pnlPlaceHolder2" runat="server" Width="475px"></asp:Panel></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px; HEIGHT: 19px" width="437">&nbsp;</TD>
											<TD style="HEIGHT: 19px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label10" runat="server" Font-Bold="True" Width="188px">Dienstleistungsdetails</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label13" runat="server" Width="157px">Leasingnehmer:</asp:label><asp:label id="lblLeasingnehmer" runat="server"></asp:label></TD>
											<TD><asp:label id="Label2" runat="server" Width="145px">1. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen1" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label4" runat="server" Width="157px">Referenz-Nr:</asp:label><asp:label id="lblRef" runat="server"></asp:label></TD>
											<TD><asp:label id="Label11" runat="server" Width="145px">2. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen2" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 437px; BORDER-BOTTOM: black thin solid; HEIGHT: 15px" width="437"><asp:label id="Label14" runat="server" Width="157px">gew. Zulassungsdatum:</asp:label><asp:label id="lblDatumUeberf" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 15px"><asp:label id="Label12" runat="server" Width="145px">3. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen3" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label25" runat="server" Width="157px">Haltername:</asp:label><asp:label id="lblHaltername" runat="server" Width="267px"></asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD>
												<asp:label id="Label19" runat="server" Width="188px" Font-Bold="True">Schilderversand an:</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label15" runat="server" Width="170px">Versicherungsnehmer:</asp:label><asp:label id="lblVersicherungsnehmer" runat="server" Width="240px"></asp:label></TD>
											<TD>
												<asp:Label id="lblSchildversandName" runat="server" Width="151px"></asp:Label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label16" runat="server" Width="170px">Versicherer:</asp:label><asp:label id="lblVersicherer" runat="server" Width="236px"></asp:label></TD>
											<TD>
												<asp:Label id="lblSchildversandStrasseHausnr" runat="server" Width="165px"></asp:Label>&nbsp;</TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px; HEIGHT: 22px" width="437"><asp:label id="Label17" runat="server" Width="170px" Height="21px">KFZ-Steuer-Zahlung durch:</asp:label><asp:label id="lblKfzSteuer" runat="server" Width="198px" Height="6px"></asp:label></TD>
											<TD style="HEIGHT: 22px">
												<asp:Label id="lblSchildversandPLZOrt" runat="server" Width="181px"></asp:Label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437" colSpan="2">
												<asp:label id="Label18" runat="server" Width="157px" Font-Bold="True">Bemerkung:</asp:label>
												<asp:label id="lblBemerkung" runat="server" Width="598px" BorderStyle="None"></asp:label></TD>
										</TR>
									</TABLE>
									<P>
										<hr color="#000000">
									<P></P>
									<TABLE id="Table5" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 38px"><asp:label id="lblZulassungsdienst" runat="server" Font-Bold="True" Width="277px" Visible="False">In Kürze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:</asp:label></TD>
											<TD style="HEIGHT: 38px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 19px"><asp:label id="lbl2ReName1" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 19px"><asp:label id="lbl2ReStrasse" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 18px"><asp:label id="lbl2ReName2" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 18px"><asp:label id="lbl2RePlzOrt" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 36px"><asp:label id="lbl2ReTelefon" runat="server"></asp:label></TD>
											<TD style="HEIGHT: 36px"><asp:label id="lbl2ReE_Mail" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<P>
										<TABLE id="Table8" style="WIDTH: 919px; HEIGHT: 54px" cellSpacing="1" cellPadding="1" width="919" border="0">
											<TR>
												<TD><asp:label id="lblStva" runat="server" Font-Bold="True" Width="692px" Font-Italic="True" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD>
													<P><asp:label id="lblUnterlagen" runat="server" Font-Bold="True" Width="513px" Visible="False">Bitte halten Sie folgende Unterlagen für die Zulassung bereit:</asp:label></P>
												</TD>
											</TR>
										</TABLE>
									</P>
									<TABLE class="TableBanner" id="Table6" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffffff" border="0" runat="server">
										<TR>
											<TD class="TableBannerCell" vAlign="center" width="19" colSpan="1" height="30" rowSpan="1"><U>Kategorie\Dokument</U>**</TD>
											<TD class="TableBannerCell" noWrap align="middle" height="30">ZB1
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
											<TD class="TableHeaderCell" vAlign="center" colSpan="11">Privat</TD>
										</TR>
										<TR>
											<TD class="TableBannerCell" vAlign="center" colSpan="1" rowSpan="1">Zulassung</TD>
											<TD class="TableBannerInnerCell" noWrap align="middle" bgColor="#ffffff" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
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
									<P>
										<TABLE class="TableLegende" id="Table7" cellSpacing="1" cellPadding="2" bgColor="#ffffff" border="0" runat="server">
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
									</P>
									<P>&nbsp;</P>
								</TD>
							</tr>
							<TR>
								<TD style="WIDTH: 174px" vAlign="top" width="174"></TD>
								<TD style="WIDTH: 917px" vAlign="top"><asp:label id="lblError" runat="server" Width="802px" EnableViewState="False" CssClass="TextError"></asp:label></TD>
							</TR>
						</TABLE>
						<TABLE id="Table2" style="WIDTH: 1040px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="1040" border="0">
							<TR>
								<TD style="WIDTH: 479px">
									<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" ImageUrl="../../../Images/arrowUeberfLeft.gif" Height="34px"></asp:imagebutton></P>
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
					<TD style="WIDTH: 42px" vAlign="top"></TD>
				</TR>
			</table>
			</TD></TR></TABLE></form>
	</body>
</HTML>
