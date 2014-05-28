<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberf03.aspx.vb" Inherits="AppUeberf.Ueberf03" %>
<%@ Register TagPrefix="cc1" Namespace="CKG.Portal.PageElements" Assembly="CKG.Portal"   %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
									<asp:label id="lblPageTitle" runat="server">Anschlussfahrt</asp:label>)</TD>
							</TR>
							<tr>
								<TD style="WIDTH: 103px" vAlign="top" width="103">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
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
								<TD vAlign="top">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
										<TR>
											<TD style="WIDTH: 405px" width="405" colSpan="2">
												<uc1:ProgressControl id="ProgressControl1" runat="server"></uc1:ProgressControl>
											</TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415">&nbsp;
											</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="lblFahrzeugdaten" runat="server" Font-Bold="True" Width="186px">Rücklieferung 2. Fahrzeug</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405" colSpan="2">
												<uc1:AddressSearchInputControl id="ctrlAddressSearchRueckliefer" runat="server"></uc1:AddressSearchInputControl></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label17" runat="server" Width="110px">Auswahl:</asp:label><asp:dropdownlist id="drpRetour" runat="server" Width="291px" AutoPostBack="True"></asp:dropdownlist></TD>
											<TD>
												<asp:CheckBox id="chkUbernahmeLeasingnehmerRueck" runat="server" Text="Leasingnehmerdaten übernehmen" AutoPostBack="True"></asp:CheckBox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label6" runat="server" Width="110px">Firma / Name*</asp:label><asp:textbox id="txtAbName" runat="server" Width="291px" Wrap="False"></asp:textbox></TD>
											<TD>
												<asp:Label id="lblAbKundennummer" runat="server" Visible="False"></asp:Label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415" height="21"><asp:label id="Label7" runat="server" Width="110px">Strasse*</asp:label><asp:textbox id="txtAbStrasse" runat="server" Width="291px" Wrap="False"></asp:textbox></TD>
											<TD height="21"><asp:label id="Label13" runat="server" Width="53px">Nr.*</asp:label><asp:textbox id="txtAbNr" runat="server" Width="73px" Wrap="False"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label8" runat="server" Width="110px">PLZ*</asp:label><asp:textbox id="txtAbPLZ" runat="server" Width="102px" MaxLength="5" Wrap="False"></asp:textbox></TD>
											<TD><asp:label id="Label15" runat="server" Width="53px">Ort*</asp:label><asp:textbox id="txtAbOrt" runat="server" Width="299px" Wrap="False"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label9" runat="server" Width="110px">Ansprechpartner*</asp:label><asp:textbox id="txtAbAnsprechpartner" runat="server" Width="291px" Wrap="False"></asp:textbox></TD>
											<TD><asp:label id="Label16" runat="server" Width="54px">Tel. 1:*</asp:label><asp:textbox id="txtAbTelefon1" runat="server" Width="299px" Wrap="False" MaxLength="16"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"></TD>
											<TD>
												<asp:label id="Label19" runat="server" Width="54px">Tel. 2:</asp:label>
												<asp:textbox id="txtAbTelefon2" runat="server" Width="299px" Wrap="False" MaxLength="16"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label5" runat="server" Font-Bold="True" Width="293px">Fahrzeugdaten 2. Fahrzeug</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ*</asp:label><asp:textbox id="txtHerstTyp" runat="server" Width="291px" MaxLength="25" Wrap="False"></asp:textbox></TD>
											<TD><asp:label id="Label11" runat="server" Font-Bold="True" Width="286px">Fahrzeug zugelassen und betriebsbereit?*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen*</asp:label><asp:textbox id="txtKennzeichen1" runat="server" Width="39px" Wrap="False" MaxLength="3"></asp:textbox><asp:label id="Label10" runat="server" Font-Bold="True" Width="11px"> -</asp:label><asp:textbox id="txtKennzeichen2" runat="server" Width="102px" Wrap="False" MaxLength="7"></asp:textbox></TD>
											<TD><asp:radiobuttonlist id="rdbZugelassen" runat="server" Width="109px" TextAlign="Left" RepeatDirection="Horizontal" Height="19px">
													<asp:ListItem>Ja</asp:ListItem>
													<asp:ListItem>Nein</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer</asp:label><asp:textbox id="txtVin" runat="server" Width="200px" Wrap="False"></asp:textbox></TD>
											<TD><asp:label id="Label12" runat="server" Font-Bold="True" Width="251px">Bereifung*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 415px" width="415"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr*</asp:label><asp:textbox id="txtRef" runat="server" Width="200px" Wrap="False"></asp:textbox></TD>
											<TD><asp:radiobuttonlist id="rdbBereifung" runat="server" Width="137px" TextAlign="Left" RepeatDirection="Horizontal" Height="19px">
													<asp:ListItem Value="Sommer">Sommer</asp:ListItem>
													<asp:ListItem Value="Winter">Winter</asp:ListItem>
													<asp:ListItem Value="Ganzjahresreifen">Ganzjahresreifen</asp:ListItem>
													<asp:ListItem Value="Unbekannt">Unbekannt</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
									</TABLE>
									&nbsp;</TD>
							</tr>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top">
									<asp:label id="Label18" runat="server" Width="103px">Bemerkung:</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top">
									<cc1:textareacontrol id="txtBemerkung" runat="server" Width="424px" MaxLength="256" Height="47px" TextMode="MultiLine"></cc1:textareacontrol></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top">&nbsp;</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top">
									<asp:label id="lblError" runat="server" Width="831px" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top">
									<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 330px">
												<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Customize/Ueberf/arrowUeberfLeft.gif"></asp:imagebutton></P>
											</TD>
											<TD style="WIDTH: 74px"></TD>
											<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Customize/Ueberf/arrowUeberfRight.gif"></asp:imagebutton></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 330px">&nbsp;</TD>
											<TD style="WIDTH: 74px"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 330px"></TD>
											<TD style="WIDTH: 74px"><asp:label id="Label14" runat="server" Font-Bold="True" Width="80px" ForeColor="Red">*=Pflichtfeld</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 330px"></TD>
											<TD style="WIDTH: 74px"></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
