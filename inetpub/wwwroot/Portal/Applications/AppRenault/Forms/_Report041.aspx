<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report041.aspx.vb" Inherits="AppRenault._Report041" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server" Visible="False"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server" Visible="False"></asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" vAlign="top" align="left" width="100%" colSpan="1"><asp:label id="lblMessage" runat="server"></asp:label>&nbsp;</td>
											<td class="TaskTitle" vAlign="top" noWrap align="right" colSpan="1"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()">Fenster schließen</asp:hyperlink></td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="" vAlign="top" align="left"><STRONG>
																<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
																	<TR>
																		<TD><STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</STRONG></TD>
																	</TR>
																</TABLE>
															</STRONG>
														</TD>
														<TD class="TextLarge" vAlign="top" align="left" width="100%" colSpan="2">
															<TABLE id="Table2" cellSpacing="0" cellPadding="2" bgColor="white" border="0">
																<TR>
																	<TD class="PageNavigation" vAlign="top" align="left" colSpan="10">Übersicht 
																		Auftragsdaten</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="19"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="19">DAD Auftrags-Nr.:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" rowSpan="1"><strong><asp:label id="Label1" runat="server"></asp:label></strong></TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="StandardTableAlternateDescription" noWrap align="left" width="186">Leasingvertrags-Nr.:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label3" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Kennzeichen:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4"><asp:label id="Label4" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="StandardTableAlternateDescription" align="left" noWrap>Datum 
																		Terminvereinbarung /&nbsp;mit:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label11" runat="server" Font-Bold="True"></asp:label>
																		<asp:label id="Label18" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="22"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22">Fahrzeugmodell:
																	</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="22"><asp:label id="Label6" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" height="22"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="22">Vereinbarter 
																		Zeitraum:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="22"><asp:label id="Label12" runat="server" Font-Bold="True" Visible="False"></asp:label>
																		<asp:label id="Label5" runat="server" Font-Bold="True" Visible="False"></asp:label>&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="22">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22">
																		<asp:Label id="lblKfall" runat="server" Visible="False">Klärfall</asp:Label></TD>
																	<TD class="ABEDaten" noWrap colSpan="8" height="22">
																		<asp:label id="Label17" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" noWrap align="left" colSpan="10">Fahrzeugübernahme / 
																		Fahrzeugabgabe</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="23"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23">Abholort:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="23"><asp:label id="Label8" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="23">Abgabeort:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="23"><asp:label id="Label9" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="23"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23">Abholung am:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="23"><asp:label id="Label7" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="23">Abgabe 
																		am:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="23"><asp:label id="Label10" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="23">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23"></TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="23"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="23"></TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="23"></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" align="left" colSpan="10" height="22">Zusatzinformationen</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Kundenberater:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4"><asp:label id="Label14" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186">Telefon-Nr.:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label15" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="ABEDaten" noWrap colSpan="4"></TD>
																	<TD class="StandardTableAlternateDescription"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186">eMail:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label16" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Gefahrene Km:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4"><asp:label id="Label13" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186">Fahrt-Nr.:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label2" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="ABEDaten" noWrap colSpan="4"></TD>
																	<TD class="StandardTableAlternateDescription">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186"></TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" noWrap align="left" colSpan="10"></TD>
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
											<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><asp:literal id="litScript" runat="server"></asp:literal></td>
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
