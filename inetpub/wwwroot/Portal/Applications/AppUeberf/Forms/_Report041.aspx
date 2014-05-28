<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report041.aspx.vb" Inherits="AppUeberf._Report041" %>
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
											<td class="TaskTitle" vAlign="top" noWrap align="right" colSpan="1"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()" CssClass="TaskTitle">Fenster schließen</asp:hyperlink></td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="" vAlign="top" align="left"><STRONG>
																<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
																	<TR>
																		<TD><STRONG>Vorschau:</STRONG></TD>
																	</TR>
																</TABLE>
															</STRONG>
															<DIV style="OVERFLOW: auto; WIDTH: 130px; HEIGHT: 550px" align="left">
																<TABLE id="tblPreview" cellSpacing="1" cellPadding="1" border="0">
																	<TR>
																		<TD id="tCell" runat="server"></TD>
																	</TR>
																</TABLE>
															</DIV>
														</TD>
														<TD class="TextLarge" vAlign="top" align="left" width="100%" colSpan="2">
															<TABLE id="Table2" cellSpacing="0" cellPadding="2" bgColor="white" border="0">
																<TR>
																	<TD class="PageNavigation" vAlign="top" align="left" colSpan="10">Übersicht 
																		Auftragsdaten</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="19"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="19">Unsere&nbsp;Auftrags-Nr.:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" rowSpan="1" width="197"><strong><asp:label id="Label1" runat="server"></asp:label></strong></TD>
																	<TD class="StandardTableAlternateDescription" noWrap width="4"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap align="left" width="186">Ihre 
																		Referenz:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label3" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Kennzeichen:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" width="197"><asp:label id="Label4" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" noWrap width="4"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap align="left">Datum 
																		Terminvereinbarung /&nbsp;mit:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label11" runat="server" Font-Bold="True"></asp:label><asp:label id="Label18" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="22"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22">Fahrzeugmodell:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="22" width="197"><asp:label id="Label6" runat="server" Font-Bold="True" Width="188px"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" height="22" width="4"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="22">Vereinbarter Zeitraum:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="22"><asp:label id="Label12" runat="server" Visible="False" Font-Bold="True"></asp:label><asp:label id="Label5" runat="server" Visible="False" Font-Bold="True"></asp:label>&nbsp;</TD>
																</TR>
																<tr>
																	<td class="StandardTableAlternateDescription" align="right"></td>
																	<td class="StandardTableAlternateDescription" noWrap><asp:Label ID="lbl_TextLN" Runat="server">Leasingkunde</asp:Label></td>
																	<td class="ABEDaten" noWrap colSpan="4"><asp:Label ID="lbl_DataLN" Runat="server" Font-Bold="True"></asp:Label></td>
																	<TD class="StandardTableAlternateDescription" height="22" width="4"></TD>
																	<td class="StandardTableAlternateDescription" align="left" width="186"><asp:Label ID="lbl_TextLG" Runat="server">Leasinggesellschaft</asp:Label></td>
																	<td class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:Label ID="lbl_DataLG" Runat="server" Font-Bold="True"></asp:Label></td>
																</tr>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="22">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="22"><asp:label id="lblKfall" runat="server" Visible="False">Klärfall</asp:label></TD>
																	<TD class="ABEDaten" noWrap colSpan="8" height="22"><asp:label id="Label17" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" noWrap align="left" colSpan="10">Fahrzeugübernahme / 
																		Fahrzeugabgabe</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="23"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23">Abholort:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="23" width="197"><asp:label id="Label8" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23" width="4"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="23">Abgabeort:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="23"><asp:label id="Label9" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="23"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23">Abholung am:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="23" width="197"><asp:label id="Label7" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23" width="4"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="23">Abgabe 
																		am:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="23"><asp:label id="Label10" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="23">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23"></TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="23" width="197"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap height="23" width="4"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186" height="23"></TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2" height="23"></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" align="left" colSpan="10" height="22">Zusatzinformationen</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Kundenberater:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" width="197"><asp:label id="Label14" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" width="4"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186">Telefon-Nr.:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label15" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="ABEDaten" noWrap colSpan="4" width="197"></TD>
																	<TD class="StandardTableAlternateDescription" width="4"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186">eMail:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label16" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap>Gefahrene Km:</TD>
																	<TD class="ABEDaten" noWrap colSpan="4" width="197"><asp:label id="Label13" runat="server" Font-Bold="True"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" width="4"></TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186">Fahrt-Nr.:</TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"><asp:label id="Label2" runat="server" Font-Bold="True"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" noWrap></TD>
																	<TD class="ABEDaten" noWrap colSpan="4" width="197"></TD>
																	<TD class="StandardTableAlternateDescription" width="4">&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" align="left" width="186"></TD>
																	<TD class="ABEDaten" noWrap align="left" width="100%" colSpan="2"></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" noWrap align="left" colSpan="10">Archivierte Dokumente</TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="23"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap align="right" height="23"><asp:imagebutton id="btnShowProtokoll" runat="server" ImageUrl="/Portal/Images/document.gif"></asp:imagebutton></TD>
																	<TD class="ABEDaten" noWrap colSpan="4" height="23" width="197">Übergabeprotokolle</TD>
																	<TD class="ABEDaten" align="left" colSpan="4" height="23"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="20"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap align="right" height="20"><asp:imagebutton id="btnShowPics" runat="server" ImageUrl="/Portal/Images/camera.gif"></asp:imagebutton></TD>
																	<TD class="ABEDaten" colSpan="4" height="20" width="197">Digitalfotos</TD>
																	<TD class="ABEDaten" width="100%" colSpan="4" height="20"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" align="right" height="20"></TD>
																	<TD class="StandardTableAlternateDescription" noWrap align="right" height="20"><asp:imagebutton id="btnShowAbmeldung" runat="server" Visible="False" ImageUrl="/Portal/Images/pdf_grey.gif"></asp:imagebutton></TD>
																	<TD class="ABEDaten" colSpan="4" height="20" width="197"></TD>
																	<TD class="ABEDaten" width="100%" colSpan="4" height="20"></TD>
																</TR>
															</TABLE>
															<INPUT id="txtHidden" type="hidden" name="txtHidden" runat="server">
															<asp:label id="lblScript" runat="server"></asp:label><INPUT id="txtType" type="hidden" name="txtType" runat="server"></TD>
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
				<script language="JavaScript">
				<!-- //
				function SetKey(File) {					
					window.document.Form1.txtHidden.value = File;
				}
				//-->
				</script>
			</table>
		</form>
	</body>
</HTML>
