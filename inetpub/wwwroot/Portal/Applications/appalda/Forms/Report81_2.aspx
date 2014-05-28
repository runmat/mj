<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report81_2.aspx.vb" Inherits="AppALDA.Report81_2" %>
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
									<asp:label id="lblPageTitle" runat="server"> (Briefdaten)</asp:label>
									<asp:hyperlink id="lnkSchluesselinformationen" runat="server" NavigateUrl="Report38.aspx?chassisnum=" Visible="False" Target="_blank">Schlüsselinformationen</asp:hyperlink></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;
												<asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Equipment.aspx">Abfragekriterien</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="middle" colSpan="2">
															<TABLE id="Table10" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																<TR>
																	<TD class="" vAlign="center" align="left" colSpan="2"><STRONG>Status:</STRONG>&nbsp;
																		<asp:label id="lblStatus" runat="server"></asp:label></TD>
																	<TD class="" vAlign="center" align="left"></TD>
																	<TD class="" vAlign="center" align="right" colSpan="2" noWrap></TD>
																</TR>
																<TR>
																	<TD class="GridTableHead" vAlign="center" align="left" width="152"><STRONG>Fahrzeugdaten</STRONG></TD>
																	<TD class="GridTableHead" vAlign="top" align="right">&nbsp;</TD>
																	<TD class="GridTableHead" vAlign="center" align="left">&nbsp;</TD>
																	<TD class="GridTableHead" vAlign="center" align="left">&nbsp;</TD>
																	<TD class="GridTableHead" vAlign="top" align="right">
																		<asp:hyperlink id="HyperLink1" runat="server" CssClass="" Target="_blank">ABE-Daten</asp:hyperlink></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left" width="152">Kfz-Kennzeichen:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblKennzeichen" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Kfz-Briefnummer:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblBriefnummer" runat="server"></asp:label></TD>
																</TR>
																<TR id="Tr5" runat="server">
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left" width="152">Fahrgestellnummer:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblFahrgestellnummer" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Briefeingang:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblBriefeingang" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left" width="152">Hersteller:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblHersteller" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Fahrzeugmodell:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblFahrzeugmodell" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left" width="152"></TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblEingangsdatum" runat="server" Visible="False"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left"></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left"></TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"></TD>
																</TR>
																<TR>
																	<TD class="GridTableHead" vAlign="center" align="left" width="152"><STRONG>Briefdaten</STRONG></TD>
																	<TD class="GridTableHead" vAlign="top" align="right">&nbsp;</TD>
																	<TD class="GridTableHead" vAlign="center" align="left">&nbsp;</TD>
																	<TD class="GridTableHead" vAlign="center" align="left">&nbsp;</TD>
																	<TD class="GridTableHead" vAlign="top" align="right"></TD>
																</TR>
																<TR>
																	<TD class="" vAlign="center" align="left" width="152"></TD>
																	<TD class="TextLarge" vAlign="top" align="right"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLarge" vAlign="top" align="right"></TD>
																</TR>
																<TR id="Tr1" runat="server">
																	<TD class="TextLargeDescription" vAlign="center" align="left" width="152">Erstzulassungsdatum:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblErstzulassungsdatum" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Leasingvertrags-Nr.:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblOrdernummer" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left" width="152">
																		<asp:label id="Label3" runat="server">CoC</asp:label></TD>
																	<TD class="TextLarge" vAlign="top" align="right">
																		<asp:checkbox id="cbxCOC" runat="server" TextAlign="Left" Enabled="False"></asp:checkbox></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Fahrzeughalter:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblFahrzeughalter" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="GridTableHead" vAlign="top" colSpan="5">
																		<STRONG>Abmeldedaten</STRONG>&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left" width="152">Carport-Eingang:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblPDIEingang" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Kennzeicheneingang:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblKennzeicheneingang" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left" width="152">Check-In:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lblCheckIn" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left"></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="center" align="left">Fahrzeugschein:&nbsp;</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:checkbox id="chkFahzeugschein" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="center" align="left" width="152">Beide 
																		Kennzeichen vorhanden:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="chkVorhandeneElemente" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="center" align="left">Stillegung:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblStillegung" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="GridTableHead" vAlign="top" colSpan="5">
																		<STRONG>Letzte Versanddaten</STRONG>
																	</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="left" colspan="2">
																		<table border="0" width="100%" cellpadding="0" cellspacing="0">
																			<tr>
																				<td><asp:label id="lblVersendetAmDescription" runat="server">Versendet am:</asp:label></td>
																				<td class="TextLarge" align="right"><asp:label id="lblAngefordertAm" runat="server"></asp:label><asp:label id="lblVersendetAm" runat="server" Visible="False"></asp:label></td>
																			</tr>
																			<tr>
																				<td><asp:label id="lblVersandart" runat="server" DESIGNTIMEDRAGDROP="45">Versandart:&nbsp;</asp:label></td>
																				<td class="TextLarge" noWrap align="right"><asp:label id="Label1" runat="server" DESIGNTIMEDRAGDROP="46">   temporär</asp:label>
																					<asp:radiobutton id="rbTemporaer" runat="server" GroupName="Versandart" Enabled="False"></asp:radiobutton>
																					<asp:label id="Label2" runat="server">   endgültig</asp:label>
																					<asp:radiobutton id="rbEndgueltig" runat="server" GroupName="Versandart" Enabled="False"></asp:radiobutton></td>
																			</tr>
																		</table>
																	</TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left">Versandanschrift:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblVersandanschrift" runat="server"></asp:label></TD>
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
								<TD width="120"></TD>
								<TD>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD width="120"></TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
