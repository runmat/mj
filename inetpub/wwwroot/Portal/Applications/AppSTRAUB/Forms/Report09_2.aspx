<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report09_2.aspx.vb" Inherits="AppSTRAUB.Report09_2" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Briefdaten)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" width="100%"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="PageNavigation" NavigateUrl="Equipment.aspx" Visible="False">Abfragekriterien</asp:hyperlink>&nbsp;</TD>
										</TR>
										<TR>
											<TD class="" width="100%">
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False" DESIGNTIMEDRAGDROP="163"></asp:label></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge" width="100%">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2" width="100%">
															<TABLE id="Table10" class="BorderFull" cellSpacing="0" cellPadding="5" bgColor="white" border="0">
																<TR>
																	<TD class="PageNavigation" vAlign="center" align="left" colSpan="2">Status:&nbsp;
																		<asp:label id="lblStatus" runat="server"></asp:label></TD>
																	<TD class="PageNavigation" vAlign="center" align="right" colSpan="3">
																		<asp:hyperlink id="lnkSchluesselinformationen" runat="server" NavigateUrl="Report09_3.aspx?chassisnum=" Target="_blank">Schlüsselinformationen</asp:hyperlink>&nbsp;&nbsp;
																		<asp:HyperLink id="HyperLink2" runat="server" Target="_blank"> ABE-Daten</asp:HyperLink></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Kennzeichen:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblKennzeichen" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Briefnummer:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblBriefnummer" runat="server"></asp:label></TD>
																</TR>
																<TR id="Tr5" runat="server">
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Fahrgestellnummer:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblFahrgestellnummer" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Briefeingang:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblBriefeingang" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Hersteller:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblHersteller" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Fahrzeugmodell:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblFahrzeugmodell" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">Eingangsdatum 
																		PDI:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblEingangsdatum" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left"></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">Fahrzeughalter:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblFahrzeughalter" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="left">PDI-Nummer / -Name:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:label id="lblPDI" runat="server"></asp:label>
																		<asp:label id="lblPDIName" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left"></TD>
																	<TD class="TextLarge" vAlign="top" align="right"></TD>
																</TR>
																<tr>
																	<TD class="TextHeader" vAlign="top" colSpan="5">
																	</TD>
																</tr>
																<TR>
																	<TD class="PageNavigation" vAlign="center" align="left">Briefdaten
																	</TD>
																	<TD class="PageNavigation" vAlign="top" align="right" colSpan="4"></TD>
																</TR>
																<TR id="Tr1" runat="server">
																	<TD class="TextLargeDescription" vAlign="center" align="left">Erstzulassungsdatum:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblErstzulassungsdatum" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"><asp:label id="lblMindestlaufzeitDescription" runat="server" Visible="False">Mindestlaufzeit:</asp:label>Ordernummer:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblMindestlaufzeit" runat="server" Visible="False"></asp:label><asp:label id="lblOrdernummer" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Ehemaliges 
																		Kennzeichen:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblEhemaligesKennzeichen" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Umgemeldet 
																		am:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblUmgemeldetAm" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Ehemalige 
																		Briefnummer:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblEhemaligeBriefnummer" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Briefaufbietung / 
																		CoC:</TD>
																	<TD class="TextLarge" vAlign="top" align="right" noWrap><asp:checkbox id="chkBriefaufbietung" runat="server" Enabled="False"></asp:checkbox>/
																		<asp:checkbox id="cbxCOC" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" vAlign="top" colSpan="5">
																		Abmeldedaten&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Carport-Eingang:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblPDIEingang" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Kennzeicheneingang:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblKennzeicheneingang" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Check-In:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblCheckIn" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left"></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Fahrzeugschein:&nbsp;</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:checkbox id="chkFahzeugschein" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Beide Kennzeichen 
																		vorhanden:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="chkVorhandeneElemente" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Stillegung:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblStillegung" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" vAlign="top" colSpan="5">
																		Letzte Versanddaten
																	</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="left"><asp:label id="lblVersendetAmDescription" runat="server">Versendet am:</asp:label></TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblAngefordertAm" runat="server"></asp:label><asp:label id="lblVersendetAm" runat="server" Visible="False"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left"><asp:label id="lblVersandart" runat="server"> Versandart:</asp:label></TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="Label1" runat="server">   temporär</asp:label><asp:radiobutton id="rbTemporaer" runat="server" Enabled="False" GroupName="Versandart"></asp:radiobutton><BR>
																		<asp:label id="Label2" runat="server">   endgültig</asp:label><asp:radiobutton id="rbEndgueltig" runat="server" Enabled="False" GroupName="Versandart"></asp:radiobutton></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">Versandanschrift:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblVersandanschrift" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left"></TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
