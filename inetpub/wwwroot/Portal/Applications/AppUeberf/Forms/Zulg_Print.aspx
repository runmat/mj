<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Zulg_Print.aspx.vb" Inherits="AppUeberf.Zulg_Print" %>
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
								<TD style="WIDTH: 291px"></TD>
								<TD style="WIDTH: 213px"><asp:label id="lblSchritt" runat="server" Width="145px" Font-Bold="True"> Ausdruck</asp:label></TD>
								<TD></TD>
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
								<TD style="WIDTH: 403px"><asp:label id="lblKundeName1" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD><asp:label id="lblKundeStrasse" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px"><asp:label id="lblKundeAnsprechpartner" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD><asp:label id="lblKundePlzOrt" runat="server" Font-Bold="True" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px"><asp:label id="Label10" runat="server" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px; HEIGHT: 20px" noWrap>
									<asp:label id="Label25" runat="server" Width="145px">Haltername:</asp:label>
									<asp:label id="lblHaltername" runat="server" Width="215px" Font-Size="Smaller"></asp:label></TD>
								<TD style="HEIGHT: 20px" noWrap><asp:label id="Label2" runat="server" Width="156px">1. Wunschkennzeichen:</asp:label><asp:label id="lblKennzeichen1" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px" noWrap>
									<asp:label id="Label14" runat="server" Width="145px">gew. Zulassungsdatum:</asp:label>
									<asp:label id="lblDatumUeberf" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD noWrap>
									<asp:label id="Label11" runat="server" Width="156px">2. Wunschkennzeichen: </asp:label>
									<asp:label id="lblKennzeichen2" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px" noWrap><asp:label id="Label4" runat="server" Width="145px">Referenz-Nr</asp:label><asp:label id="lblRef" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD noWrap>
									<asp:label id="Label12" runat="server" Width="157px">3. Wunschkennzeichen: </asp:label>
									<asp:label id="lblKennzeichen3" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
						</TABLE>
						<P>
							<TABLE id="Table5" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
								<TR>
									<TD style="WIDTH: 405px; HEIGHT: 38px"><asp:label id="lblZulassungsdienst" runat="server" Width="272px" Font-Bold="True">In Kürze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:</asp:label></TD>
									<TD style="HEIGHT: 38px"></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 405px; HEIGHT: 19px"><asp:label id="lbl2ReName1" runat="server" Font-Size="Smaller"></asp:label></TD>
									<TD style="HEIGHT: 19px"><asp:label id="lbl2ReName2" runat="server" Font-Size="Smaller"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 405px; HEIGHT: 18px"><asp:label id="lbl2ReStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
									<TD style="HEIGHT: 18px"><asp:label id="lbl2RePlzOrt" runat="server" Font-Size="Smaller"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 405px; HEIGHT: 36px"><asp:label id="lbl2ReTelefon" runat="server" Font-Size="Smaller"></asp:label></TD>
									<TD style="HEIGHT: 36px"><asp:label id="lbl2ReE_Mail" runat="server" Font-Size="Smaller"></asp:label></TD>
								</TR>
							</TABLE>
							<hr style="WIDTH: 100%; HEIGHT: 2px" color="#000000" SIZE="2">
						<P></P>
						<P>
							<TABLE id="Table8" style="WIDTH: 919px; HEIGHT: 54px" cellSpacing="1" cellPadding="1" width="919" border="0">
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
							<TABLE class="TableBanner" id="Table6" style="WIDTH: 672px; HEIGHT: 128px" cellSpacing="1" cellPadding="1" width="672" bgColor="#ffffff" border="0" runat="server">
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
						<TABLE class="TableLegende" id="Table7" cellSpacing="1" cellPadding="2" bgColor="#ffffff" border="0" runat="server">
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
								<TD colSpan="9"><U><FONT size="2">Wir weisen darauf hin, dass diese Angaben unverbindliche 
											Auskünfte der entsprechenden Zulassungskreise sind.</FONT></U></TD>
							</TR>
						</TABLE>
						&nbsp;
					</TD>
				</tr>
			</TABLE>
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="WIDTH: 612px">
						<TABLE id="Table9" style="WIDTH: 633px; HEIGHT: 155px" cellSpacing="1" cellPadding="1" width="633" border="0">
							<TR>
								<TD style="WIDTH: 438px" noWrap></TD>
								<TD noWrap>
									<P align="right">Kroschke Car Logistic GmbH</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Zentraldisposition
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Bogenstr. 26
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">22926&nbsp;Ahrensburg</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Telefon&nbsp;04102 804-0
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">
										Telefax&nbsp;04102 804-266
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
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
					<TD style="WIDTH: 612px">
						<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
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
