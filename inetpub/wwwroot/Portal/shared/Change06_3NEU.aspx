<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change06_3NEU.aspx.vb" Inherits="CKG.Portal.Shared.Change06_3NEU" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (ABE-Daten Neu)</asp:label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="3">&nbsp;
									<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="javascript:window.close()">Fenster schließen</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><asp:label id="lblMessage" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="2">
															<TABLE id="Table2" cellSpacing="0" cellPadding="2" bgColor="white" border="0">
															    <TR>
																	<TD class="PageNavigation" vAlign="top" align="left" colSpan="6">Allgemein</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63" height="19">1</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="19">Kennzeichen:</TD>
																	<TD class="ABEDaten" noWrap colSpan="3" rowSpan="1"><asp:label id="lbl_Kennzeichen" runat="server"></asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%" height="19"><asp:label id="lbl_KennEmpty" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" width="63">2</TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Fahrgestellnummer:</TD>
																	<TD class="ABEDaten" colSpan="3"><asp:label id="lbl_FIN" runat="server"></asp:label></TD>
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="Label3" runat="server"></asp:label></TD>
																</TR>
																
																<TR>
																	<TD class="PageNavigation" vAlign="top" align="left" colSpan="6">Allgemeine 
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
																	<TD class="ABEDaten" align="left" width="100%"><asp:label id="lbl_55" runat="server" BorderColor="White" ForeColor="White" BackColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_91" runat="server" ForeColor="Black" BackColor="SaddleBrown" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_92" runat="server" ForeColor="Black" BackColor="DimGray" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_93" runat="server" ForeColor="Black" BackColor="Green" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_94" runat="server" ForeColor="Black" BackColor="RoyalBlue" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_95" runat="server" ForeColor="Black" BackColor="Magenta" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_96" runat="server" ForeColor="Black" BackColor="Red" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_97" runat="server" ForeColor="Black" BackColor="OrangeRed" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_98" runat="server" ForeColor="Black" BackColor="Yellow" BorderColor="Black" BorderWidth="1px">-</asp:label><asp:label id="lbl_99" runat="server" ForeColor="Black" BackColor="White" BorderColor="Black" BorderWidth="1px">-</asp:label>
																		<asp:label id="lbl_00" runat="server" BackColor="Transparent">-</asp:label></TD>
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
																	<TD class="PageNavigation" align="right" width="63" colSpan="6">Antriebsdaten</TD>
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
																	<TD class="PageNavigation" noWrap align="left" colSpan="6">Kraftstoff / Tank</TD>
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
																	<TD class="PageNavigation" align="left" colSpan="6">Achsen / Bereifung</TD>
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
																	<TD class="PageNavigation" align="left" width="63" colSpan="6">Bemerkungen</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="right">1</TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="ABEDaten" colSpan="4"><asp:label id="lbl_30" runat="server" Font-Size="XX-Small" Font-Names="Arial">-</asp:label></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<tr>
											<td colSpan="3"><!--#include File="../PageElements/Footer.html" --></td>
										</tr>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
