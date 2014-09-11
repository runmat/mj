<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report_002_02.aspx.vb" Inherits="AppARVAL.Report_002_02" %>
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
					<td><uc1:header id="ucHeader" runat="server" Visible="False"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server"> Detaildaten</asp:label>)</td>
							</TR>
							<TR>
								<TD class="TaskTitle" align="right" colSpan="2">&nbsp;
									<asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()" CssClass="TaskTitle">Fenster schließen</asp:hyperlink></TD>
							</TR>
							<tr>
								<TD vAlign="top"></TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="5" width="100%" border="0">
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD>
												<TABLE id="Table10" cellSpacing="0" cellPadding="5" bgColor="white" border="0">
													<TR>
														<TD class="" vAlign="top" noWrap align="left"><asp:label id="Label8" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">LV-Nr. / Status</asp:label>:</TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblLVNr" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label>&nbsp;/&nbsp;
															<asp:label id="lblStatus" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91"><asp:label id="Label17" runat="server" CssClass="DetailTableFont" Font-Size="">Angelegt am:</asp:label></TD>
														<TD vAlign="top" align="left"><asp:label id="lblAntrag" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label25" runat="server" CssClass="DetailTableFont" Font-Size="">Leasingdauer:</asp:label></TD>
														<TD class="" vAlign="top" align="left" colSpan="3"><asp:label id="lblLBeginn" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label>&nbsp;-
															<asp:label id="lblLEnde" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR class="PageNavigation">
														<TD vAlign="top" noWrap align="left"><FONT size="3"><asp:label id="Label6" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Leasingnehmer</asp:label></FONT></TD>
														<TD vAlign="top" noWrap align="left" colSpan="3"><FONT size="3"><FONT size="2"></FONT></FONT></TD>
														<TD vAlign="top" noWrap align="left" width="91"><FONT size="3"><FONT size="3"><asp:label id="Label35" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Halter</asp:label></FONT></FONT></TD>
														<TD vAlign="top" noWrap align="left"><FONT size="3"><FONT size="2"></FONT></FONT></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label7" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Versicherungsgeber</asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="100%" colSpan="2"></TD>
													</TR>
													<TR id="Tr5" runat="server">
														<TD class="" vAlign="top" noWrap align="left"><asp:label id="Label10" runat="server" CssClass="DetailTableFont">Name:</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblNameLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91"><asp:label id="Label28" runat="server" CssClass="DetailTableFont">Name:</asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="lblName1" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left"><asp:label id="Label13" runat="server" CssClass="DetailTableFont" Font-Size="">Versich.Schein-Nr.:</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="2"><asp:label id="lblVschein" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label37" runat="server" CssClass="DetailTableFont">Name 2:</asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblName2LN" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91"><asp:label id="Label29" runat="server" CssClass="DetailTableFont">Name 2:</asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="lblName2" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label14" runat="server" CssClass="DetailTableFont" Font-Size="">Versicherungsdauer:</asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="2"><asp:label id="lblVBeginn" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label>&nbsp;-&nbsp;
															<asp:label id="lblVEnde" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label38" runat="server" CssClass="DetailTableFont">Name 3:</asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblName3LN" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91"><asp:label id="Label30" runat="server" CssClass="DetailTableFont">Name 3:</asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="lblName3" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label9" runat="server" CssClass="DetailTableFont" Font-Size="">Name:</asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="2"><asp:label id="lblNameVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label22" runat="server" CssClass="DetailTableFont" Font-Size="">Straße:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblStrLN" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91"><asp:label id="Label31" runat="server" CssClass="DetailTableFont">Straße:</asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="lblStras_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left"><asp:label id="Label11" runat="server" CssClass="DetailTableFont" Font-Size="">Straße:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="2"><asp:label id="lblStrVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR>
														<TD class="" vAlign="top" noWrap align="left" height="26"><asp:label id="Label21" runat="server" CssClass="DetailTableFont" Font-Size=""> Postleitzahl:</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblPLZLN" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91"><asp:label id="Label32" runat="server" CssClass="DetailTableFont">Plz:</asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="lblPstlz_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label12" runat="server" CssClass="DetailTableFont" Font-Size=""> Postleitzahl:</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="2" height="26"><asp:label id="lblPLZVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR id="Tr1" runat="server">
														<TD class="" vAlign="top" noWrap align="left"><asp:label id="Label23" runat="server" CssClass="DetailTableFont" Font-Size="">Ort:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblOrtLN" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91"><asp:label id="Label33" runat="server" CssClass="DetailTableFont">Ort:</asp:label></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="lblOrt_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left"><asp:label id="Label39" runat="server" CssClass="DetailTableFont" Font-Size="">Ort:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="2"><asp:label id="lblOrtVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR>
														<TD class="" vAlign="top" noWrap align="left" height="30"><asp:label id="Label24" runat="server" CssClass="DetailTableFont" Font-Size=""> Kunden-Nr.:</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="3" height="30"><asp:label id="lblKonzernID" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" width="91" height="30"><asp:label id="Label34" runat="server" CssClass="DetailTableFont">Kunden-Nr:</asp:label></TD>
														<TD vAlign="top" noWrap align="left" height="30"><asp:label id="lblKonzs_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left"></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="2"></TD>
													</TR>
													<TR>
														<TD class="" vAlign="top" noWrap align="left" height="27"><asp:label id="Label18" runat="server" CssClass="DetailTableFont" Font-Size="">Versand:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3" height="27"><asp:label id="lblVersandLN" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left" height="27"></TD>
														<TD vAlign="top" noWrap align="left" height="27"></TD>
														<TD class="" vAlign="top" noWrap align="left" height="27"><asp:label id="Label15" runat="server" CssClass="DetailTableFont" Font-Size="">Versand:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="2" height="27"><asp:label id="lblVersandVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label42" runat="server" CssClass="DetailTableFont" Font-Size="">Rückgabe:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblRueckLN" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label43" runat="server" CssClass="DetailTableFont" Font-Size="">Rückgabe:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="2" height="28"><asp:label id="lblRueckVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR>
														<TD class="" vAlign="top" noWrap align="left" width="154"><asp:label id="Label19" runat="server" CssClass="DetailTableFont" Font-Size=""> Eingang unvollständig:</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="3"><asp:dropdownlist id="lblUnvLN" runat="server" CssClass="DetailTableFont" Width="200px"></asp:dropdownlist></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD class="" vAlign="top" noWrap align="left"><asp:label id="Label16" runat="server" CssClass="DetailTableFont" Font-Size="">Eingang unvollständig:</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="2"><asp:dropdownlist id="lblUnvVG" runat="server" CssClass="DetailTableFont" Width="200px"></asp:dropdownlist></TD>
													</TR>
													<TR class="PageNavigation">
														<TD vAlign="top" noWrap align="left" width="154" height="2"><STRONG><asp:label id="Label1" runat="server" CssClass="DetailTableFontBold" Font-Size="">Mahndaten</asp:label></STRONG></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="3" height="2"></TD>
														<TD vAlign="top" noWrap align="left" height="2"></TD>
														<TD vAlign="top" noWrap align="left" height="2"></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="2" height="2">
															<P>&nbsp;</P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label26" runat="server" CssClass="DetailTableFont" Font-Size="">Zuletzt gemahnt / Stufe:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblMadatLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label>&nbsp;/&nbsp;<asp:label id="lblMahnsLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label27" runat="server" CssClass="DetailTableFont" Font-Size="">Zuletzt gemahnt / Stufe:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="2"><asp:label id="lblMadatVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label>&nbsp;/&nbsp;<asp:label id="lblMahnsVG" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></TD>
													</TR>
													<TR class="PageNavigation">
														<TD vAlign="top" noWrap align="left"><asp:label id="Label20" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Versicherungsumfang</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="8"></TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="6"><asp:label id="lblVersUmf" runat="server" Font-Bold="True"></asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left"></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left"></TD>
													</TR>
													<TR class="PageNavigation">
														<TD vAlign="top" noWrap align="left" width="154"><STRONG><asp:label id="Label5" runat="server" CssClass="DetailTableFontBold" Font-Size="">Fahrzeugdaten</asp:label></STRONG></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="3"></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="lblVersumfang" runat="server" CssClass="DetailTableFontBold" Font-Size=""> Bemerkungen</asp:label></TD>
														<TD vAlign="top" noWrap align="left"></TD>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label36" runat="server" CssClass="DetailTableFont" Font-Size="">Kundenbetreuer(in):</asp:label></TD>
														<TD class="" vAlign="top" noWrap align="left" colSpan="3"><STRONG>
																<P align="left"><asp:label id="lblKonzs_ZK" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label>&nbsp;
																	<asp:label id="lblName1_ZK" runat="server" CssClass="DetailTableFont" Font-Bold="True" Font-Size=""></asp:label></P>
															</STRONG>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label4" runat="server" CssClass="DetailTableFont" Font-Size=""> Erstzulassung:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblEz" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label></TD>
														<TD vAlign="top" noWrap align="left">
															<P align="left"><asp:label id="lblInfo" runat="server" CssClass="TextError" Font-Bold="True" Font-Size=""></asp:label></P>
														</TD>
														<TD vAlign="top" noWrap align="left" colSpan="5">
															<P align="left">
																<TABLE id="tblBemerkungen" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server">
																	<TR>
																		<TD noWrap>
																			<asp:label id="lblB1" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:label></TD>
																		<TD noWrap>
																			<asp:label id="lblB2" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:label></TD>
																		<TD noWrap>
																			<asp:label id="lblB3" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:label></TD>
																		<TD noWrap width="100%">
																			<asp:label id="lblB4" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:label></TD>
																	</TR>
																</TABLE>
															</P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label40" runat="server" CssClass="DetailTableFont" Font-Size="">Hersteller / Typ:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblHerst" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="6">
															<P align="left"><asp:dropdownlist id="ddl1" runat="server" CssClass="DetailTableFont"></asp:dropdownlist></P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label3" runat="server" CssClass="DetailTableFont" Font-Size=""> Kennzeichen:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblKennz" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="5" height="28">
															<P align="left"><asp:label id="lblBemerkungen" runat="server" CssClass="" Font-Size=""><u>Bemerkungen 
																		erfassen:</u></asp:label></P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left"><asp:label id="Label41" runat="server" CssClass="DetailTableFont" Font-Size="">Fahrzeugart:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3"><asp:label id="lblFzArt" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="5">
															<TABLE id="tblBem1" cellSpacing="1" cellPadding="1" border="0" runat="server">
																<TR>
																	<TD noWrap>1:</TD>
																	<TD><asp:textbox id="txtBem1" runat="server" MaxLength="25"></asp:textbox></TD>
																	<TD noWrap>2:</TD>
																	<TD><asp:textbox id="txtBem2" runat="server" MaxLength="25"></asp:textbox></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left" height="17"><asp:label id="Label2" runat="server" CssClass="DetailTableFont" Font-Size="">Fahrgestellnr.:</asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="3" height="17"><asp:label id="lblFGNr" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:label></TD>
														<TD vAlign="top" noWrap align="left" colSpan="5">
															<TABLE id="tblBem2" cellSpacing="1" cellPadding="1" border="0" runat="server">
																<TR>
																	<TD noWrap>3:</TD>
																	<TD><asp:textbox id="txtBem3" runat="server" MaxLength="25"></asp:textbox></TD>
																	<TD noWrap>4:</TD>
																	<TD><asp:textbox id="txtBem4" runat="server" MaxLength="25"></asp:textbox>&nbsp;<asp:linkbutton id="btnSave" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
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
								<TD><asp:label id="lblError" runat="server" CssClass="TextError" Font-Size="" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<SCRIPT language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
						return (Check);
						}
						//-->
						</SCRIPT>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
