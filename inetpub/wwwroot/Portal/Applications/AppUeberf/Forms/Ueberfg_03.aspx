<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_03.aspx.vb" Inherits="AppUeberf.Ueberfg_03" %>
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
									<asp:label id="lblPageTitle" runat="server">Anschlussfahrt</asp:label>)</TD>
							</TR>
							<tr>
								<TD style="WIDTH: 103px" vAlign="top" width="103">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
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
								<TD vAlign="top">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
										<TR>
											<TD style="WIDTH: 405px" width="405">
												<P align="right"><STRONG>Schritt&nbsp;4 von 5</STRONG></P>
											</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="lblKundeName1" runat="server" Width="225px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundeStrasse" runat="server" Width="278px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="lblKundeAnsprechpartner" runat="server" Width="307px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
											<TD><asp:label id="lblKundePlzOrt" runat="server" Width="134px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405">&nbsp;
											</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="lblFahrzeugdaten" runat="server" Width="186px" Font-Bold="True">Rücklieferung 2. Fahrzeug</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405" colSpan="2"><uc1:addresssearchinputcontrol id="ctrlAddressSearchRueckliefer" runat="server"></uc1:addresssearchinputcontrol></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label17" runat="server" Width="110px">Auswahl:</asp:label><asp:dropdownlist id="drpRetour" runat="server" Width="216px" AutoPostBack="True"></asp:dropdownlist></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label6" runat="server" Width="110px">Firma / Name*</asp:label>
                                                <asp:textbox id="txtAbName" runat="server" Width="220px" Wrap="False" 
                                                    MaxLength="35"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405" height="21"><asp:label id="Label7" runat="server" Width="110px">Strasse*</asp:label>
                                                <asp:textbox id="txtAbStrasse" runat="server" Width="220px" Wrap="False" 
                                                    MaxLength="35"></asp:textbox></TD>
											<TD height="21"><asp:label id="Label13" runat="server" Width="50px">Nr.*</asp:label><asp:textbox id="txtAbNr" runat="server" Width="73px" Wrap="False"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label8" runat="server" Width="110px">PLZ*</asp:label><asp:textbox id="txtAbPLZ" runat="server" Width="102px" Wrap="False" MaxLength="5"></asp:textbox></TD>
											<TD><asp:label id="Label15" runat="server" Width="50px">Ort*</asp:label>
                                                <asp:textbox id="txtAbOrt" runat="server" Width="299px" Wrap="False" 
                                                    MaxLength="35"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label9" runat="server" Width="110px">Ansprechpartner*</asp:label><asp:textbox id="txtAbAnsprechpartner" runat="server" Width="223px" Wrap="False"></asp:textbox></TD>
											<TD><asp:label id="Label16" runat="server" Width="50px">1. Tel.:*</asp:label><asp:textbox id="txtAbTelefon" runat="server" Width="299px" Wrap="False" MaxLength="16"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label18" runat="server" Width="110px">Fax</asp:label><asp:textbox id="txtAbFax" runat="server" Width="223px" Wrap="False"></asp:textbox></TD>
											<TD><asp:label id="Label19" runat="server" Width="50px">2. Tel.:</asp:label><asp:textbox id="txtAbTelefon2" runat="server" Width="299px" Wrap="False" MaxLength="16"></asp:textbox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label5" runat="server" Width="293px" Font-Bold="True">Fahrzeugdaten 2. Fahrzeug</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ*</asp:label>
                                                <asp:textbox id="txtHerstTyp" runat="server" Width="200px" Wrap="False" 
                                                    MaxLength="24"></asp:textbox></TD>
											<TD><asp:label id="Label11" runat="server" Width="286px" Font-Bold="True">Fahrzeug zugelassen und betriebsbereit?*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen*</asp:label><asp:textbox id="txtKennzeichen1" runat="server" Width="39px" Wrap="False"></asp:textbox><asp:label id="Label10" runat="server" Width="11px" Font-Bold="True"> -</asp:label><asp:textbox id="txtKennzeichen2" runat="server" Width="102px" Wrap="False"></asp:textbox></TD>
											<TD><asp:radiobuttonlist id="rdbZugelassen" runat="server" Width="109px" Height="19px" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem>Ja</asp:ListItem>
													<asp:ListItem>Nein</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR id="TRllbAnZulKCL" runat="server" Visible="False">
											<TD style="WIDTH: 405px; HEIGHT: 25px" width="405"></TD>
											<TD style="HEIGHT: 25px">
												<asp:label id="Label21" runat="server" Font-Bold="True" Width="286px" Visible="False">Zulassung durch Kroschke?*</asp:label></TD>
										</TR>
										<TR id="TRrdbAnZulKCL" runat="server" Visible="False">
											<TD style="WIDTH: 405px; HEIGHT: 25px" width="405"></TD>
											<TD style="HEIGHT: 25px">
												<asp:radiobuttonlist id="rdbAnZulKCL" runat="server" Width="109px" TextAlign="Left" RepeatDirection="Horizontal" Height="19px" Visible="False">
													<asp:ListItem Value="Ja">Ja</asp:ListItem>
													<asp:ListItem Value="Nein">Nein</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px; HEIGHT: 25px" width="405"></TD>
											<TD style="HEIGHT: 25px"><asp:label id="Label20" runat="server" Width="192px" Font-Bold="True" Height="19px">Fahrzeugklasse in Tonnen*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer</asp:label><asp:textbox id="txtVin" runat="server" Width="200px" Wrap="False"></asp:textbox></TD>
											<TD><asp:radiobuttonlist id="rdbFahrzeugklasse" runat="server" Width="196px" Height="22px" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="P">&lt; 3,5</asp:ListItem>
													<asp:ListItem Value="G">3,5 - 7,5</asp:ListItem>
													<asp:ListItem Value="L">&gt; 7,5</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr</asp:label><asp:textbox id="txtRef" runat="server" Width="200px" Wrap="False"></asp:textbox></TD>
											<TD><asp:label id="Label12" runat="server" Width="251px" Font-Bold="True">Bereifung*</asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 405px" width="405"></TD>
											<TD><asp:radiobuttonlist id="rdbBereifung" runat="server" Width="137px" Height="19px" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="Sommer">Sommer</asp:ListItem>
													<asp:ListItem Value="Winter">Winter</asp:ListItem>
													<asp:ListItem Value="Ganzjahresreifen">Ganzjahresreifen</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
									</TABLE>
									&nbsp;</TD>
							</tr>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top">&nbsp;</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 103px" vAlign="top" width="103"></TD>
								<TD vAlign="top">
									<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 330px">
												<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
											</TD>
											<TD style="WIDTH: 74px"></TD>
											<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif"></asp:imagebutton></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 330px">&nbsp;</TD>
											<TD style="WIDTH: 74px"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 330px"></TD>
											<TD style="WIDTH: 74px"><asp:label id="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="Red">*=Pflichtfeld</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 330px"></TD>
											<TD style="WIDTH: 74px"></TD>
											<TD></TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" Width="289px" EnableViewState="False" CssClass="TextError"></asp:label></TD>
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
