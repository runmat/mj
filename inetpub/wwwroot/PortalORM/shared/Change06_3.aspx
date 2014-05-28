<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change06_3.aspx.vb" Inherits="CKG.Portal.Shared.Change06_3" %>
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
								<td class="PageNavigation" colSpan="3">
									<asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (ABE-Daten)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">
												<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="javascript:window.close()">Fenster schließen</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"></td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" vAlign="top" align="left"><asp:label id="lblMessage" runat="server"></asp:label>&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="2">
															<TABLE id="Table2" cellSpacing="0" cellPadding="2" border="0" bgColor="white">
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="right" rowSpan="2">1</TD>
																	<TD class="TextLargeDescription" vAlign="top" rowSpan="2" width="200">Fahrzeug- und 
																		Aufbauart</TD>
																	<TD class="TextLarge" colSpan="3"><asp:label id="Label1_1" runat="server">-</asp:label></TD>
																	<TD class="TextLarge" align="right"><asp:label id="Label1_2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" colSpan="3"><asp:label id="Label1_3" runat="server">-</asp:label></TD>
																	<TD class="TextLarge" align="right"><asp:label id="Label1_4" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">2</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Fahrzeughersteller</TD>
																	<TD class="StandardTableAlternate" colSpan="3"><asp:label id="Label2_1" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternate" align="right"><asp:label id="Label2_2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" align="right">3</TD>
																	<TD class="TextLargeDescription" width="200">Typ und Ausführung</TD>
																	<TD class="TextLarge" colSpan="3"><asp:label id="Label3_1" runat="server">-</asp:label></TD>
																	<TD class="TextLarge" align="right"><asp:label id="Label3_2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">4</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Fahrzeug-Ident-Nr.</TD>
																	<TD class="StandardTableAlternate" colSpan="3"><asp:label id="Label4_1" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternate" align="right"><asp:label id="Label4_2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="right">5</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Antriebsart</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label5" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		6</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Höchst geschwindigkeit 
																		km/h</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="Label6" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="right">7</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" width="200">Leistung<BR>
																		kW bei Umdreh. je Minute</TD>
																	<TD class="StandardTableAlternate" vAlign="top"><asp:label id="Label7" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		8</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" width="200">Hubraum ccm</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="Label8" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="right">9</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Nutz- oder Aufliegelast 
																		kg</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label9" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		10</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Rauminhalt des Tankes<BR>
																		in Kubikmeter</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="Label10" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">11</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Steh-/Liegeplätze</TD>
																	<TD class="StandardTableAlternate"><asp:label id="Label11" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternateDescription" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		12</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Sitzplätze</TD>
																	<TD class="StandardTableAlternate" align="right"><asp:label id="Label12" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" align="right"></TD>
																	<TD class="TextLargeDescription">Maße in mm</TD>
																	<TD class="TextLarge"></TD>
																	<TD align="right"></TD>
																	<TD class="TextLarge"></TD>
																	<TD class="TextLarge" align="right"></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" align="right">13</TD>
																	<TD class="TextLargeDescription" width="200">Länge</TD>
																	<TD class="TextLarge"><asp:label id="Label13_1" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 13</TD>
																	<TD class="TextLargeDescription" width="200">Breite</TD>
																	<TD class="TextLarge" align="right"><asp:label id="Label13_2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" align="right">13</TD>
																	<TD class="TextLargeDescription" width="200">Höhe</TD>
																	<TD class="TextLarge"><asp:label id="Label13_3" runat="server">-</asp:label></TD>
																	<TD class="TextLarge" align="right"></TD>
																	<TD class="TextLarge"></TD>
																	<TD class="TextLarge" align="right"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">14</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Leergeweicht kg</TD>
																	<TD class="StandardTableAlternate"><asp:label id="Label14" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternateDescription" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		15</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Zul Gesamtgewicht in kg</TD>
																	<TD class="StandardTableAlternate" align="right"><asp:label id="Label15" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" align="right"></TD>
																	<TD class="TextLargeDescription" width="200">Zul. Achslast kg</TD>
																	<TD class="TextLarge"></TD>
																	<TD class="TextLarge" align="right"></TD>
																	<TD class="TextLarge"></TD>
																	<TD class="TextLarge" align="right"></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" align="right">16</TD>
																	<TD class="TextLargeDescription" width="200">vorn</TD>
																	<TD class="TextLarge"><asp:label id="Label16_1" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 16</TD>
																	<TD class="TextLargeDescription" width="200">mitten</TD>
																	<TD class="TextLarge" align="right"><asp:label id="Label16_2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="right">16</TD>
																	<TD class="TextLargeDescription" width="200" vAlign="top">hinten</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label16_3" runat="server">-</asp:label></TD>
																	<TD class="TextLarge" vAlign="top" align="right"></TD>
																	<TD class="TextLarge" vAlign="top"></TD>
																	<TD class="TextLarge" vAlign="top" align="right"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="right">17</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" width="200">Räder 
																		und/oder<BR>
																		Gleisketten</TD>
																	<TD class="StandardTableAlternate" vAlign="top"><asp:label id="Label17" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"></TD>
																	<TD class="StandardTableAlternate" vAlign="top"></TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="right">18</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Anzahl der Achsen</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label18" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		19</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">davon angetriebene Achsen</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="Label19" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" width="200">Größenbezeichnung der 
																		Bereifung</TD>
																	<TD class="StandardTableAlternate"></TD>
																	<TD class="StandardTableAlternate" align="right"></TD>
																	<TD class="StandardTableAlternate"></TD>
																	<TD class="StandardTableAlternate" align="right"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">20</TD>
																	<TD class="StandardTableAlternateDescription">vorn</TD>
																	<TD class="StandardTableAlternate" colSpan="4"><asp:label id="Label20" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">21</TD>
																	<TD class="StandardTableAlternateDescription" width="200">mitten und hinten</TD>
																	<TD class="StandardTableAlternate" colSpan="4"><asp:label id="Label21" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">22</TD>
																	<TD class="StandardTableAlternateDescription" width="200">oder vorn</TD>
																	<TD class="StandardTableAlternate" colSpan="4"><asp:label id="Label22" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">23</TD>
																	<TD class="StandardTableAlternateDescription" width="200">oder mitten und hinten</TD>
																	<TD class="StandardTableAlternate" colSpan="4"><asp:label id="Label23" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" align="right"></TD>
																	<TD class="TextLargeDescription" colSpan="5">Überdruck am Bremsanschluß in bar * 10</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="right">24</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Einleitungsbremse</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label24" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		25</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Zweileitungsbremse</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="Label25" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="right">26</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" width="200">Anhängekupplung<BR>
																		DIN 740, Form u. Größe</TD>
																	<TD class="StandardTableAlternate" vAlign="top"><asp:label id="Label26" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		27</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" width="200">Anhängekupplung<BR>
																		Prüfzeichen</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="Label27" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="right">28</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">Anhängelast kg bei<BR>
																		Anhänger mit Bremse</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label28" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		29</TD>
																	<TD class="TextLargeDescription" vAlign="top" width="200">bei Anhänger<BR>
																		ohne Bremse</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="Label29" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">30</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Standgeräusch dB(A)</TD>
																	<TD class="StandardTableAlternate"><asp:label id="Label30" runat="server">-</asp:label></TD>
																	<TD class="StandardTableAlternateDescription" align="right">&nbsp;&nbsp;&nbsp;&nbsp; 
																		31</TD>
																	<TD class="StandardTableAlternateDescription" width="200">Fahrgeräusch db((A)</TD>
																	<TD class="StandardTableAlternate" align="right"><asp:label id="Label31" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" align="right" vAlign="top">32</TD>
																	<TD class="TextLargeDescription" width="200" vAlign="top">
																		Datum der Erstzulassung</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label32_2" runat="server">-</asp:label></TD>
																	<TD class="TextLargeDescription" align="right" vAlign="top">&nbsp;&nbsp;&nbsp;&nbsp; 
																		32</TD>
																	<TD class="TextLargeDescription" width="200" vAlign="top">
																		Farbziffer</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="Label32" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" vAlign="top">33</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top">Bemerkungen</TD>
																	<TD class="StandardTableAlternate" colSpan="4" vAlign="top"><asp:label id="Label33" runat="server">-</asp:label></TD>
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
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
