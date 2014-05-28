<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report11_3.aspx.vb" Inherits="AppHERTZ.Report11_3" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
											<TD class="TaskTitle" width="120">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle"><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx" CssClass="PageNavigation">Abfragekriterien</asp:hyperlink>&nbsp;</TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" colspan="2" align="left">
															<TABLE id="Table10" cellSpacing="0" cellPadding="5" border="0" bgColor="white">
																<TR>
																	<TD class="PageNavigation" vAlign="center" align="left">Status</TD>
																	<TD class="PageNavigation" vAlign="top" align="right">
																		<asp:label id="lblStatus" runat="server"></asp:label></TD>
																	<TD class="PageNavigation" vAlign="center" align="right" colSpan="3">
																		<asp:HyperLink id="lnkSchluesselinformationen" runat="server" NavigateUrl="Report38_2.aspx?chassisnum=" Target="_blank">Schlüsselinformationen</asp:HyperLink></TD>
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
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:label id="lblBriefeingang" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Hersteller:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:Label id="lblHersteller" runat="server"></asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Fahrzeugmodell:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:label id="lblFahrzeugmodell" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">Eingangsdatum 
																		PDI:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:Label id="lblEingangsdatum" runat="server"></asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left"></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">Fahrzeughalter:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:label id="lblFahrzeughalter" runat="server"></asp:label></TD>
																</TR>
																<tr>
																	<TD class="PageNavigation" vAlign="top" colspan="5">
																		Briefdaten
																	</TD>
																</tr>
																<TR id="Tr1" runat="server">
																	<TD class="TextLargeDescription" vAlign="center" align="left">
																		Erstzulassungsdatum:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:label id="lblErstzulassungsdatum" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">
																		<asp:Label id="lblMindestlaufzeitDescription" runat="server" Visible="False">Mindestlaufzeit:</asp:Label>Ordernummer:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:Label id="lblMindestlaufzeit" runat="server" Visible="False"></asp:Label>
																		<asp:label id="lblOrdernummer" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Ehemaliges 
																		Kennzeichen:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:label id="lblEhemaligesKennzeichen" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Umgemeldet 
																		am:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:Label id="lblUmgemeldetAm" runat="server"></asp:Label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Ehemalige 
																		Briefnummer:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:Label id="lblEhemaligeBriefnummer" runat="server"></asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Briefaufbietung:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:CheckBox id="chkBriefaufbietung" runat="server" Enabled="False"></asp:CheckBox></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" vAlign="top" colSpan="5">
																		Abmeldedaten&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Carport-Eingang:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:Label id="lblPDIEingang" runat="server"></asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Kennzeicheneingang:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:Label id="lblKennzeicheneingang" runat="server"></asp:Label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Check-In:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:Label id="lblCheckIn" runat="server"></asp:Label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left"></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Fahrzeugschein:&nbsp;</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:CheckBox id="chkFahzeugschein" runat="server" Enabled="False"></asp:CheckBox></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Beide Kennzeichen 
																		vorhanden:</TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:CheckBox id="chkVorhandeneElemente" runat="server" Enabled="False"></asp:CheckBox></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Stilllegung:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblStillegung" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" vAlign="top" colSpan="5">
																		Letzte Versanddaten
																	</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="left">
																		<asp:Label id="lblVersendetAmDescription" runat="server">Versendet am:</asp:Label></TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:label id="lblAngefordertAm" runat="server"></asp:label>
																		<asp:Label id="lblVersendetAm" runat="server" Visible="False"></asp:Label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left"><asp:Label id="lblVersandart" runat="server"> Versandart:</asp:Label></TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:Label id="Label1" runat="server">   temporär</asp:Label>
																		<asp:RadioButton id="rbTemporaer" runat="server" GroupName="Versandart" Enabled="False"></asp:RadioButton><BR>
																		<asp:Label id="Label2" runat="server">   endgültig</asp:Label>
																		<asp:RadioButton id="rbEndgueltig" runat="server" GroupName="Versandart" Enabled="False"></asp:RadioButton>
																	</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">
																		Versandanschrift:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:label id="lblVersandanschrift" runat="server"></asp:label>
																	</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left"></TD>
																	<TD class="" vAlign="top" align="right">
																		<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="Report11_3p.aspx" Target="_blank" CssClass="StandardButtonTable">&#149;&nbsp;Druckversion</asp:HyperLink></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD></TD>
								<TD>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
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
