<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ZulPrint.aspx.vb" Inherits="AppUeberf.ZulPrint" %>
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
					<TD style="WIDTH: 954px; HEIGHT: 30px" colSpan="2">
						<TABLE id="Table3" style="WIDTH: 1027px; HEIGHT: 27px" cellSpacing="1" cellPadding="1" width="1027" border="0">
							<TR>
								<TD style="WIDTH: 308px">
									<P align="right">
										<asp:label id="lblSchritt" runat="server" Width="145px" Font-Bold="True"> Ausdruck</asp:label></P>
								</TD>
								<TD style="WIDTH: 27px"></TD>
								<TD>
									<P align="left">
										<asp:Image id="imgLogo" Runat="server"></asp:Image></P>
								</TD>
							</TR>
						</TABLE>
						&nbsp;</TD>
				</TR>
				<TR>
					<TD class="PageNavigation" style="WIDTH: 954px" colSpan="2"></TD>
				</TR>
				<tr>
					<TD style="WIDTH: 87.84%" vAlign="top">
						<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
							<TR>
								<TD style="WIDTH: 403px">
									<asp:label id="lblRefLabel" runat="server" Font-Bold="True" Width="96px" Font-Italic="True">Referenz:</asp:label>
									<asp:label id="lblReferenz" runat="server" Font-Bold="True" Width="225px" Font-Italic="True"></asp:label></TD>
								<TD>
									<asp:label id="lblNameLabel" runat="server" Font-Bold="True" Width="96px" Font-Italic="True">Name:</asp:label>
									<asp:label id="lblLeasingnehmerName" runat="server" Font-Bold="True" Width="242px" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px">
									<asp:label id="lblTypLabel" runat="server" Font-Bold="True" Width="96px" Font-Italic="True">Fahrzeugtyp:</asp:label>
									<asp:label id="lblFahrzeugtyp" runat="server" Font-Bold="True" Width="296px" Font-Italic="True"></asp:label></TD>
								<TD>
									<asp:label id="lblOrtLabel" runat="server" Font-Bold="True" Width="96px" Font-Italic="True">Ort:</asp:label>
									<asp:label id="lblLeasingnehmerOrt" runat="server" Font-Bold="True" Width="278px" Font-Italic="True"></asp:label></TD>
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
								<TD style="WIDTH: 403px"><asp:label id="Label17" runat="server" Width="145px">Leasingnehmer:</asp:label><asp:label id="lblLeasingnehmer" runat="server" Width="215px" Font-Size="Smaller"></asp:label></TD>
								<TD>
									<asp:label id="Label2" runat="server" Width="156px">1. Wunschkennzeichen:</asp:label>
									<asp:label id="lblKennzeichen1" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px">
									<asp:label id="Label4" runat="server" Width="145px">Referenz-Nr</asp:label>
									<asp:label id="lblRef" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD>
									<asp:label id="Label11" runat="server" Width="156px">2. Wunschkennzeichen: </asp:label>
									<asp:label id="lblKennzeichen2" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px; HEIGHT: 20px" noWrap><asp:label id="Label25" runat="server" Width="145px">Haltername:</asp:label><asp:label id="lblHaltername" runat="server" Width="215px" Font-Size="Smaller"></asp:label></TD>
								<TD style="HEIGHT: 20px" noWrap>
									<asp:label id="Label12" runat="server" Width="157px">3. Wunschkennzeichen: </asp:label>
									<asp:label id="lblKennzeichen3" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 403px; BORDER-BOTTOM: black thin solid" noWrap><asp:label id="Label14" runat="server" Width="145px">gew. Zulassungsdatum:</asp:label><asp:label id="lblDatumUeberf" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD noWrap></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px" noWrap><asp:label id="Label13" runat="server" Width="145px">Versicherungsnehmer:</asp:label><asp:label id="lblVersicherungsnehmer" runat="server" Width="215px" Font-Size="Smaller"></asp:label></TD>
								<TD noWrap>
									<asp:label id="Label19" runat="server" Font-Bold="True" Width="188px">Schilderversand an:</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px" noWrap><asp:label id="Label15" runat="server" Width="145px">Versicherer:</asp:label><asp:label id="lblVersicherer" runat="server" Width="215px" Font-Size="Smaller"></asp:label></TD>
								<TD noWrap>
									<asp:Label id="lblSchildversandName" runat="server" Width="151px"></asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px" noWrap><asp:label id="Label16" runat="server" Width="145px">KFZ-Steuer-Zahlung durch:</asp:label>
									<asp:label id="lblKfzSteuer" runat="server" Width="215px" Font-Size="Smaller"></asp:label></TD>
								<TD noWrap>
									<asp:Label id="lblSchildversandStrasseHausnr" runat="server" Width="165px"></asp:Label><br>
									<asp:Label id="lblSchildversandPLZOrt" runat="server" Width="181px"></asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 403px" noWrap></TD>
								<TD noWrap></TD>
							</TR>
							<TR>
								<TD colSpan="2">
									<asp:label id="Label18" runat="server" Font-Bold="True" Width="145px">Bemerkung:</asp:label><asp:label id="lblBemerkung" runat="server" Width="524px" Font-Size="Smaller"></asp:label></TD>
							</TR>
						</TABLE>
						<P>
							<TABLE id="Table5" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
								<TR>
									<TD style="WIDTH: 405px; HEIGHT: 38px"><asp:label id="lblZulassungsdienst" runat="server" Font-Bold="True" Width="272px">In Kürze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:</asp:label></TD>
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
									<TD style="WIDTH: 405px; HEIGHT: 20px"><asp:label id="lbl2ReTelefon" runat="server" Font-Size="Smaller"></asp:label></TD>
									<TD style="HEIGHT: 20px"><asp:label id="lbl2ReE_Mail" runat="server" Font-Size="Smaller"></asp:label></TD>
								</TR>
							</TABLE>
							<hr style="WIDTH: 100%; HEIGHT: 2px" color="#000000" SIZE="2">
						<P></P>
						<P>
							<TABLE id="Table8" style="WIDTH: 919px; HEIGHT: 54px" cellSpacing="1" cellPadding="1" width="919" border="0">
								<TR>
									<TD vAlign="top"><asp:label id="lblStva" runat="server" Font-Bold="True" Width="692px" Font-Italic="True"></asp:label></TD>
								</TR>
							</TABLE>
						</P>
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
									<P align="right">Zentraldisposition
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">Bogenstr. 26
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
									<P align="right">Telefon&nbsp;04102 804-0
									</P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 438px" noWrap>
									<P align="right">&nbsp;</P>
								</TD>
								<TD noWrap>
									<P align="right">Telefax&nbsp;04102&nbsp;804-266
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
					<TD style="WIDTH: 612px">
						<asp:label id="lblUser" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
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
