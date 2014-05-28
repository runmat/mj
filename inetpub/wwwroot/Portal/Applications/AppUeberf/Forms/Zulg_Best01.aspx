<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Zulg_Best01.aspx.vb" Inherits="AppUeberf.Zulg_Best01" %>
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
											<TD style="WIDTH: 283px"><asp:label id="lblSchritt" runat="server" Width="274px" Font-Bold="True">Schritt 3 von 3</asp:label></TD>
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
											<TD style="WIDTH: 437px" width="437"><asp:label id="lblKundeName1" runat="server" Width="225px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundeStrasse" runat="server" Width="442px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="lblKundeAnsprechpartner" runat="server" Width="307px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundePlzOrt" runat="server" Width="405px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label10" runat="server" Width="188px" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label14" runat="server" Width="157px">gew. Zulassungsdatum:</asp:label><asp:label id="lblDatumUeberf" runat="server"></asp:label></TD>
											<TD><asp:label id="Label2" runat="server" Width="145px">1. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen1" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label25" runat="server" Width="157px">Haltername:</asp:label><asp:label id="lblHaltername" runat="server" Width="267px"></asp:label></TD>
											<TD><asp:label id="Label11" runat="server" Width="145px">2. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen2" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 437px" width="437"><asp:label id="Label4" runat="server" Width="157px">Referenz-Nr</asp:label><asp:label id="lblRef" runat="server"></asp:label></TD>
											<TD><asp:label id="Label12" runat="server" Width="145px">3. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen3" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<P>
										<hr color="#000000">
									<P></P>
									<TABLE id="Table5" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
										<TR>
											<TD style="WIDTH: 438px; HEIGHT: 38px"><asp:label id="lblZulassungsdienst" runat="server" Width="277px" Font-Bold="True" Visible="False">In Kürze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:</asp:label></TD>
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
									<P align="center">&nbsp;</P>
									<P>
										<TABLE id="Table8" style="WIDTH: 919px; HEIGHT: 54px" cellSpacing="1" cellPadding="1" width="919" border="0">
											<TR>
												<TD><asp:label id="lblStva" runat="server" Width="692px" Font-Bold="True" Font-Italic="True" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD>
													<P><asp:label id="lblUnterlagen" runat="server" Width="513px" Font-Bold="True" Visible="False">Bitte halten Sie folgende Unterlagen für die Zulassung bereit:</asp:label></P>
												</TD>
											</TR>
										</TABLE>
									</P>
									<TABLE class="TableBanner" id="Table6" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffffff" border="0" runat="server">
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
													<P align="center"><asp:label id="Label00" runat="server" Width="100%"></asp:label></P>
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
							<TR>
								<TD style="WIDTH: 118px"></TD>
								<TD style="WIDTH: 612px"><asp:label id="lblError" runat="server" Width="802px" EnableViewState="False" CssClass="TextError"></asp:label></TD>
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
