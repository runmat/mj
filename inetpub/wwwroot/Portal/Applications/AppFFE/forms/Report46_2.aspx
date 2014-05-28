<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report46_2.aspx.vb" Inherits="AppFFE.Report46_2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> 
                                    (Daten)</asp:label><asp:hyperlink id="lnkSchluesselinformationen" runat="server" Target="_blank" Visible="False" NavigateUrl="Report38.aspx?chassisnum=">Schlüsselinformationen</asp:hyperlink></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="lb_Uebersicht" runat="server" CssClass="StandardButton"> lb_Uebersicht</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="lb_Typdaten" runat="server" CssClass="StandardButton"> lb_Typdaten</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="lb_Lebenslauf" runat="server" CssClass="StandardButton"> lb_Lebenslauf</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="lb_Uebermittlung" runat="server" CssClass="StandardButton"> lb_Uebermittlung</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="lb_Drucken" runat="server" CssClass="StandardButton"> lb_Drucken</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;
												<asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx" CssClass="TaskTitle">Abfragekriterien</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2">
															<table border="0" cellpadding="0" cellspacing="0" width="800">
																<tr>
																	<td align="center">
																		<TABLE id="Table110" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Fahrgestellnummer" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblFahrgestellnummerShow" runat="server"></asp:label></td>
																			</tr>
																		</TABLE>
																	</td>
																	<td align="center">
																		<TABLE id="Table111" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Kennzeichen" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Kennzeichen" runat="server">lbl_Kennzeichen </asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblKennzeichenShow" runat="server"></asp:label></td>
																			</tr>
																		</TABLE>
																	</td>
																	<td align="center">
																		<TABLE id="Table112" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Status" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Status" runat="server">lbl_Status </asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblStatusShow" runat="server"></asp:label></td>
																			</tr>
																		</TABLE>
																	</td>
																	<td align="center">
																		<TABLE id="Table113" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Lagerort" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Lagerort" runat="server">lbl_Lagerort</asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblLagerortShow" runat="server"></asp:label></td>
																			</tr>
																		</TABLE>
																	</td>
																</tr>
															</table>
														</TD>
													</TR>
													<TR id="trUebersicht" runat="server">
														<TD vAlign="top" align="left" colSpan="2"><TABLE id="Table10" cellSpacing="0" cellPadding="5" width="800" bgColor="white" border="0">
																<TR>
																	<TD class="TaskTitle" colSpan="8">
																		<asp:Label id="lbl_Fahrzeugdaten" runat="server">lbl_Fahrzeugdaten</asp:Label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_Hersteller" runat="server">lbl_Hersteller</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lblHerstellerShow" runat="server"></asp:label></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_Fahrzeugmodell" runat="server">lbl_Fahrzeugmodell</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lblFahrzeugmodellShow" runat="server"></asp:label></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_Farbe" runat="server">lbl_Farbe</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lbl_155" runat="server" BorderWidth="1px" BackColor="Black" ForeColor="White" BorderColor="White">-</asp:label><asp:label id="lbl_191" runat="server" BorderWidth="1px" BackColor="SaddleBrown" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_192" runat="server" BorderWidth="1px" BackColor="DimGray" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_193" runat="server" BorderWidth="1px" BackColor="Green" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_194" runat="server" BorderWidth="1px" BackColor="RoyalBlue" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_195" runat="server" BorderWidth="1px" BackColor="Magenta" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_196" runat="server" BorderWidth="1px" BackColor="Red" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_197" runat="server" BorderWidth="1px" BackColor="OrangeRed" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_198" runat="server" BorderWidth="1px" BackColor="Yellow" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_199" runat="server" BorderWidth="1px" BackColor="White" ForeColor="Black" BorderColor="Black">-</asp:label>&nbsp;<asp:label id="lbl_200" runat="server" BackColor="Transparent">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_HerstellerSchluessel" runat="server">lbl_HerstellerSchluessel</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right"><asp:label id="lblHerstellerSchluesselShow" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_Typschluessel" runat="server">lbl_Typschluessel</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right"><asp:label id="lblTypschluesselShow" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_VarianteVersion" runat="server">lbl_VarianteVersion</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right"><asp:label id="lblVarianteVersionShow" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TaskTitle" colSpan="8">
																		<asp:Label id="lbl_Briefdaten" runat="server">lbl_Briefdaten</asp:Label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_Briefnummer" runat="server">lbl_Briefnummer</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lblBriefnummerShow" runat="server"></asp:label></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left" width="152">
																		<asp:Label id="lbl_Erstzulassungsdatum" runat="server">lbl_Erstzulassungsdatum</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lblErstzulassungsdatumShow" runat="server"></asp:label></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_Abmeldedatum" runat="server">lbl_Abmeldedatum</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lblAbmeldedatumShow" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_Ordernummer" runat="server">lbl_Ordernummer</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right"><asp:label id="lblOrdernummerShow" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="middle" noWrap align="left"></TD>
																	<TD class="StandardTableAlternate" vAlign="top" noWrap align="left" rowSpan="2">
																		<asp:Label id="lbl_Fahrzeughalter" runat="server">lbl_Fahrzeughalter</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right" rowSpan="2"><asp:label id="lblFahrzeughalterShow" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left" rowSpan="2">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternate" vAlign="top" noWrap align="left" rowSpan="2">
																		<asp:Label id="lbl_Standort" runat="server">lbl_Standort</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right" rowSpan="2"><asp:label id="lblStandortShow" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_CoC" runat="server">lbl_CoC</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:checkbox id="cbxCOC" runat="server" TextAlign="Left" Enabled="False"></asp:checkbox></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">&nbsp;&nbsp;&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="TaskTitle" colSpan="8">
																		<asp:Label id="lbl_Aenderungsdaten" runat="server">lbl_Aenderungsdaten</asp:Label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left" width="152">
																		<asp:Label id="lbl_UmgemeldetAm" runat="server">lbl_UmgemeldetAm</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lblUmgemeldetAmShow" runat="server"></asp:label></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left"></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_EhemaligesKennzeichen" runat="server">lbl_EhemaligesKennzeichen</asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right"><asp:label id="lblEhemaligesKennzeichenShow" runat="server"></asp:label></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left"></TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left">&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="right">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left" width="152">
																		<asp:Label id="lbl_Briefaufbietung" runat="server">lbl_Briefaufbietung</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right"><asp:checkbox id="chkBriefaufbietung" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left"></TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">
																		<asp:Label id="lbl_EhemaligeBriefnummer" runat="server">lbl_EhemaligeBriefnummer</asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right"><asp:label id="lblEhemaligeBriefnummerShow" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left"></TD>
																	<TD class="StandardTableAlternate" vAlign="middle" noWrap align="left">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" noWrap align="right">&nbsp;</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR id="trTypdaten" runat="server">
														<TD vAlign="top" align="left" colSpan="2"><TABLE id="Table4" cellSpacing="0" cellPadding="2" width="800" bgColor="white" border="0">
																<TR>
																	<TD class="TaskTitle" vAlign="top" align="left" colSpan="6">Allgemeine 
																		Fahrzeugdaten</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63" height="19">1</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="19">Fahrzeugklasse&nbsp;/ 
																		Aufbau:</TD>
																	<TD class="ABEDaten" noWrap colSpan="3" rowSpan="1"><asp:label id="lbl_6" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%" height="19"><asp:label id="lbl_7" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">2</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Hersteller / Schlüssel:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_1" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">3</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Handelsname / Farbe:
																	</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_3" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_55" runat="server" BorderWidth="1px" BackColor="Black" ForeColor="White" BorderColor="White">-</asp:label><asp:label id="lbl_91" runat="server" BorderWidth="1px" BackColor="SaddleBrown" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_92" runat="server" BorderWidth="1px" BackColor="DimGray" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_93" runat="server" BorderWidth="1px" BackColor="Green" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_94" runat="server" BorderWidth="1px" BackColor="RoyalBlue" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_95" runat="server" BorderWidth="1px" BackColor="Magenta" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_96" runat="server" BorderWidth="1px" BackColor="Red" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_97" runat="server" BorderWidth="1px" BackColor="OrangeRed" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_98" runat="server" BorderWidth="1px" BackColor="Yellow" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_99" runat="server" BorderWidth="1px" BackColor="White" ForeColor="Black" BorderColor="Black">-</asp:label>&nbsp;<asp:label id="lbl_00" runat="server" BackColor="Transparent">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">4</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Genehmigungs-Datum / Nr.&nbsp;</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_5" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_4" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63" height="23">5</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23">Typ / Schlüssel</TD>
																	<TD class="ABEDaten" colSpan="3" height="23"><asp:label id="lbl_0" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%" height="23"><asp:label id="lbl_29" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63" height="22">6</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22">Fabrikname:</TD>
																	<TD class="ABEDaten" colSpan="3" height="22"><asp:label id="lbl_8" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%" height="22"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">7</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Variante / Version:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_9" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_10" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">8</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Anzahl Sitze:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_26" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">9</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Zul. Gesamtgewicht (kg):</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_28" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">10</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Länge min. (mm)</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_31" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">11</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Breite min. (mm)</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_32" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">12</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Höhe min. (mm)</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_33" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="TaskTitle" align="right" width="63" colSpan="6">Antriebsdaten</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">1</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Hubraum (cm³)</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_11" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">2</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Nennleistung (KW) bei U/Min:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_13" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_14" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">3</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Höchstgeschwindigkeit (km/h):</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_12" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">4</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Stand- / Fahrgeräusch (db):</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_19" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_20" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TaskTitle" noWrap align="left" colSpan="6">Kraftstoff / Tank</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">1</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Art / Code:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_15" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_16" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">2</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Fassungsvermögen Tank (m³):</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_21" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63" height="22">3</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22">Co2-Gehalt (g/km):</TD>
																	<TD class="ABEDaten" colSpan="3" height="22"><asp:label id="lbl_17" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%" height="22"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63" height="22">4</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22">Nat. 
																		Emissionsklasse:</TD>
																	<TD class="ABEDaten" colSpan="3" height="22"><asp:label id="lbl_18" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%" height="22"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63" height="22">5</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22">Abgasrichtlinie:</TD>
																	<TD class="ABEDaten" colSpan="3" height="22"><asp:label id="lbl_22" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%" height="22"></TD>
																</TR>
																<TR>
																	<TD class="TaskTitle" align="left" colSpan="6">Achsen / Bereifung</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">1</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Anzahl Achsen:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_23" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">2</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Anzahl Antriebsachsen:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_24" runat="server">-</asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">3</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Max. Achslast Achse 1,2,3 
																		(kg):</TD>
																	<TD class="ABEDaten" colSpan="4"><asp:label id="lbl_25" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">4</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Bereifung Achse 1,2,3:</TD>
																	<TD class="ABEDaten" colSpan="4"><asp:label id="lbl_27" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TaskTitle" align="left" width="63" colSpan="6">Bemerkungen</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="right">1</TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="ABEDaten" colSpan="4"><asp:label id="lbl_30" runat="server" Font-Names="Arial" Font-Size="XX-Small">-</asp:label></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR id="trLebenslauf" runat="server">
														<TD vAlign="top" align="left" colSpan="2"><asp:datagrid id="Datagrid2" runat="server" BackColor="White" AutoGenerateColumns="False" Width="800px" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="TaskTitle"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="KURZTEXT" SortExpression="KURZTEXT" HeaderText="Vorgang"></asp:BoundColumn>
																	<asp:BoundColumn DataField="STRMN" SortExpression="STRMN" HeaderText="Durchf&#252;hrungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Versandadresse">
																		<ItemTemplate>
																			<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
																			</asp:Label>
																			<asp:Literal id="Literal1" runat="server" Text="<br>"></asp:Literal>
																			<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
																			</asp:Label>
																			<asp:Literal id="Literal2" runat="server" Text="<br>"></asp:Literal>
																			<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn Visible="False" DataField="ERDAT" SortExpression="ERDAT" HeaderText="Erfassungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ZZDIEN1" SortExpression="ZZDIEN1" HeaderText="Versandart"></asp:BoundColumn>
																	<asp:BoundColumn DataField="QMNAM" SortExpression="QMNAM" HeaderText="Beauftragt&lt;br&gt;durch"></asp:BoundColumn>
																	<asp:BoundColumn Visible="False" DataField="QMNUM" SortExpression="QMNUM" HeaderText="Meldungsnummer"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
													<TR id="trUebermittlung" runat="server">
														<TD vAlign="top" align="left" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" Width="800px" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="TaskTitle"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="MNCOD" SortExpression="MNCOD" HeaderText="Aktionscode"></asp:BoundColumn>
																	<asp:BoundColumn DataField="MATXT" SortExpression="MATXT" HeaderText="Vorgang"></asp:BoundColumn>
																	<asp:BoundColumn DataField="PSTER" SortExpression="PSTER" HeaderText="Statusdatum"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ZZUEBER" SortExpression="ZZUEBER" HeaderText="&#220;bermittlungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:BoundColumn Visible="False" DataField="AEZEIT" SortExpression="AEZEIT" HeaderText="&#196;nderungs-&lt;br&gt;Zeit" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
													<!-- jj --></TABLE>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="120"></TD>
								<TD><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
			<!--#include File="../../../PageElements/Footer.html" --></form>
	</body>
</HTML>
