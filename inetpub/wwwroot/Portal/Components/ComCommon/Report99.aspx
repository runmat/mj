<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report99.aspx.vb" Inherits="CKG.Components.ComCommon.Report99" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML xmlns:o>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top" width="100%">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;Bitte Ortskennzeichen 
												eingeben und mit "Suchen" bestätigen.</TD>
										</TR>
										<TR>
											<TD vAlign="top" width="100%"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" width="100%">
												<TABLE id="Table1" cellSpacing="0" cellPadding="2" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="" vAlign="middle" noWrap>Ortskennzeichen*:
														</TD>
														<TD class="" vAlign="top" width="100%"><asp:textbox id="txtKennzeichen" runat="server" Width="100px" MaxLength="3" BackColor="Transparent"></asp:textbox>&nbsp;&nbsp;<asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Suchen</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="" vAlign="middle" noWrap colSpan="2"><FONT face="Arial" color="#ff0000" size="1">*Eingabe 
																erforderlich</FONT></TD>
													</TR>
													<TR>
														<TD class="" vAlign="middle" colSpan="2"><TABLE class="TableBanner" id="Table5" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffffff" border="0">
																<TR>
																	<TD class="TableBannerCell" vAlign="middle" width="19" colSpan="1" height="30" rowSpan="1"><U>Kategorie\Dokument</U>**</TD>
																	<TD class="TableBannerCell" noWrap align="center" height="30">ZB1
																	</TD>
																	<TD class="TableBannerCell" height="30">ZB2</TD>
																	<TD class="TableBannerCell" height="30">CoC
																	</TD>
																	<TD class="TableBannerCell" height="30">DK&nbsp;
																	</TD>
																	<TD class="TableBannerCell" height="30">VM
																	</TD>
																	<TD class="TableBannerCell" width="41" height="30">PA
																	</TD>
																	<TD class="TableBannerCell" height="30">GewA
																	</TD>
																	<TD class="TableBannerCell" height="30">HRA
																	</TD>
																	<TD class="TableBannerCell" height="30"><asp:hyperlink id="lnkEinzug" runat="server">LEV</asp:hyperlink></TD>
																	<TD class="TableBannerCell" height="30">Bemerkung
																	</TD>
																</TR>
																<TR>
																	<TD class="TableHeaderCell" vAlign="middle" colSpan="11">Privat</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" vAlign="middle" colSpan="1" rowSpan="1">Zulassung</TD>
																	<TD class="TableBannerInnerCell" noWrap align="center" bgColor="#ffffff" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label01" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label00" runat="server" Width="100%"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label02" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label03" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label04" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label05" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label06" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label07" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label08" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label09" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" colSpan="1" height="25" rowSpan="1">Umschreibung</TD>
																	<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label11" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label10" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label12" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label13" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label14" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label15" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label16" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label17" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label18" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label19" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" colSpan="1" height="25" rowSpan="1">Umkennzeichnung</TD>
																	<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label21" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label20" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label22" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label23" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label24" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label25" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label26" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label27" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label28" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label29" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" noWrap height="25">Ersatzfahrzeugschein</TD>
																	<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label31" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label30" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label32" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" align="center" height="25"><FONT face="Arial" size="1">
																			<P align="center"><strong><asp:label id="Label33" runat="server" Width="100%"></asp:label></strong></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label34" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label35" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label36" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label37" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label38" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label39" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
																<TR>
																	<TD class="TableHeaderCell" colSpan="11">Unternehmen</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" colSpan="1" height="25" rowSpan="1">Zulassung</TD>
																	<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label41" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label40" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label42" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label43" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label44" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label45" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label46" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label47" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label48" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label49" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" noWrap colSpan="1" height="25" rowSpan="1">Umschreibung</TD>
																	<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label51" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label50" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label52" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label53" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label54" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label55" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label56" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label57" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label58" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label59" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" noWrap colSpan="1" height="25" rowSpan="1">Umkennzeichnung</TD>
																	<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label61" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label60" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label62" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label63" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label64" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label65" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label66" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label67" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label68" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label69" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
																<TR>
																	<TD class="TableBannerCell" noWrap colSpan="1" height="25" rowSpan="1">Ersatzfahrzeugschein</TD>
																	<TD class="TableBannerInnerCell" colSpan="1" height="25" rowSpan="1"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label71" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label70" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label72" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label73" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label74" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label75" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label76" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label77" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label78" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																	<TD class="TableBannerInnerCell" height="25"><FONT face="Arial" size="1">
																			<P align="center"><asp:label id="Label79" runat="server"></asp:label></P>
																		</FONT>
																	</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
												<FONT face="Arial" size="1">
													<TABLE class="TableLegende" id="Table2" height="107" cellSpacing="1" cellPadding="2" bgColor="#ffffff" border="0">
														<TR>
															<TD><FONT size="1"><u>**Legende:</u></FONT></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
														</TR>
														<TR>
															<TD><FONT size="1">O=Original</FONT></TD>
															<TD><FONT size="1">K=Kopie</FONT></TD>
															<TD><FONT size="1">F=Formular Zulassungsstelle</FONT>
															</TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
														</TR>
														<TR>
															<TD><FONT size="1">ZB1=Fahrzeugschein,</FONT></TD>
															<TD><FONT size="1">ZB2=Fahrzeugbrief,</FONT></TD>
															<TD noWrap><FONT size="1">CoC=Certificate of Conformity,</FONT></TD>
															<TD><FONT size="1">DK=Deckungskarte,</FONT></TD>
															<TD><FONT size="1">VM=Vollmacht,</FONT></TD>
															<TD><FONT size="1">PA=Personalausweis,</FONT></TD>
															<TD><FONT size="1">GewA=Gewerbeanmeldung,</FONT></TD>
															<TD><FONT size="1">HRA=Handelsregister,</FONT></TD>
															<TD><FONT size="1">LEV=Lastschrifteinzug</FONT></TD>
														</TR>
														<TR>
															<TD>&nbsp;</TD>
															<TD></TD>
															<TD noWrap></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
															<TD></TD>
														</TR>
														<TR>
															<TD colSpan="9" height="18"><U><FONT size="1">Wir weisen darauf hin, dass diese Angaben 
																		unverbindliche Auskünfte der entsprechenden Zulassungskreise sind.</FONT></U></TD>
														</TR>
													</TABLE>
												</FONT>
												<TABLE class="TableLinks" id="Table2" cellSpacing="1" cellPadding="2" align="center" border="0">
													<TR>
														<TD>&nbsp;</TD>
													</TR>
													<TR>
														<TD><b>&nbsp;Links:</b></TD>
														<TD width="136">
															<P align="center"><asp:linkbutton id="cmdAmt" runat="server" CssClass="StandardButtonTable" Width="125" Enabled="False">Amt</asp:linkbutton></P>
														</TD>
														<TD>
															<P align="center"><asp:linkbutton id="cmdWunsch" runat="server" CssClass="StandardButtonTable" Width="125" Enabled="False">Wunschkennzeichen</asp:linkbutton></P>
														</TD>
														<TD>
															<P align="center"><asp:linkbutton id="cmdFormulare" runat="server" CssClass="StandardButtonTable" Width="125" Enabled="False">Formulare</asp:linkbutton></P>
														</TD>
														<TD>
															<P align="center"><asp:linkbutton id="cmdGebuehr" runat="server" CssClass="StandardButtonTable" Width="125" Enabled="False">Gebühren</asp:linkbutton></P>
														</TD>
													</TR>
													<TR>
													</TR>
												</TABLE>
												<table id="Table7" width="630" align="center">
													<TR>
														<TD><SPAN style="FONT-SIZE: 11pt; COLOR: #0066cc; FONT-FAMILY: Arial"><asp:label id="lbl_Message" runat="server"></asp:label>
															</SPAN></TD>
													</TR>
												</table>
											</TD>
										</TR>
									</TABLE>
									<P><SPAN class="text31"><B><SPAN style="FONT-SIZE: 9pt; COLOR: #4a63bc; FONT-FAMILY: Arial">Haftungsausschluss <o:p></o:p></SPAN></B></SPAN></P>
									<P><STRONG><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Haftungsausschluss für eigene 
            Seiten</SPAN></STRONG><SPAN style="FONT-SIZE: 8pt; COLOR: #27aeca; FONT-FAMILY: Arial"><BR>
										</SPAN><SPAN class="text21"><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">DAD 
            / Christoph Kroschke GmbH übernimmt keine Haftung oder Garantie 
            für die Aktualität, Richtigkeit oder Vollständigkeit der auf dieser 
            Website bereitgestellten Informationen. Die Inhalte dieser 
            Internet-Seiten basieren teilweise auf gesetzlichen Grundlagen und 
            werden regelmäßig überprüft. Es kann nicht garantiert werden, dass 
            nach einer gesetzlichen Änderung eine sofortige Anpassung der 
            Internet-Seiten erfolgt. DAD / Christoph Kroschke GmbH übernimmt 
            keine Haftung für direkte oder indirekte Schäden, die aus der 
            Benutzung diese Website entstehen können.</SPAN>
										</SPAN><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial"><BR><BR><STRONG>
												<SPAN style="FONT-FAMILY: Arial">Haftungsausschluss für fremde 
            Seiten</SPAN></STRONG><B><BR>
											</B>Mit dem Urteil vom 12.Mai 1998 - 
            312&nbsp;O&nbsp;85/98 - "Haftung für Links" hat das Landgericht 
            Hamburg entschieden, dass durch die Anbringung eines Links die 
            Inhalte der gelinkten Seite ggf. mit zu verantworten sind. Dies kann 
            nur dadurch verhindert werden, dass man sich ausdrücklich von diesen 
            Inhalten distanziert. Manche Verweise/Hyperlinks führen zu Angeboten 
            außerhalb dieser Website, welche nicht vom DAD / der Christoph Kroschke GmbH erstellt oder gepflegt werden. Dies wird insbesondere 
            dadurch deutlich, dass in der Regel der Verweis / Hyperlink mit 
            „www“ gekennzeichnet und nach dem Öffnen der Rahmen der Homepage der 
                                        Christoph Kroschke GmbH in dem neuen Fenster nicht mehr sichtbar 
            ist. Die Christoph Kroschke GmbH übernimmt keine Haftung oder 
            Garantie für die Aktualität, Richtigkeit oder Vollständigkeit der 
            auf der jeweiligen verzeichneten Websites veröffentlichten Inhalte, 
            für deren Rechtmäßigkeit oder für die Erfüllung von 
            Urheberrechtsbestimmungen im Zusammenhang mit den auf der jeweiligen 
            Website veröffentlichten Inhalten. DAD / Christoph Kroschke GmbH 
            übernimmt keine Haftung für direkte oder indirekte Schäden, die aus 
            der Benutzung dieser Websites entstehen können und distanziert sich 
            ausdrücklich von den Inhalten. <o:p></o:p></SPAN></P>
									<P><STRONG><SPAN style="FONT-SIZE: 8pt; COLOR: black; FONT-FAMILY: Arial">Verbindlichkeit</SPAN></STRONG><B><SPAN style="COLOR: #27aeca; FONT-FAMILY: Arial"><BR>
											</SPAN></B><SPAN class="text21"><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Die 
            in dieser Website gemachten Ausführungen dienen lediglich der 
            allgemeinen Information. Ein Rechtsanspruch auf eine bestimmte 
            Leistung des DAD / der Christoph Kroschke GmbH kann hierauf nicht 
            begründet werden. In jeder konkreten Angelegenheit ist daher eine 
            Einzelfallentscheidung nach dem vorgeschriebenen 
            Verwaltungsverfahren erforderlich. <o:p></o:p></SPAN>
										</SPAN></P>
									<SPAN class="text21">
										<B>
											<SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial; mso-fareast-font-family: 'Times New Roman'; mso-ansi-language: DE; mso-fareast-language: DE; mso-bidi-language: AR-SA"><SPAN style="mso-spacerun: yes">&nbsp;</SPAN>Urheberrechte</SPAN></B></SPAN><SPAN style="FONT-SIZE: 12pt; FONT-FAMILY: 'Times New Roman'; mso-fareast-font-family: 'Times New Roman'; mso-ansi-language: DE; mso-fareast-language: DE; mso-bidi-language: AR-SA"><BR>
									</SPAN><SPAN class="text21"><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial; mso-fareast-font-family: 'Times New Roman'; mso-ansi-language: DE; mso-fareast-language: DE; mso-bidi-language: AR-SA">Der gesamte Inhalt und die Abbildungen dieser 
            Website sowie die Gestaltung unterliegen dem Urheberrecht des DAD / 
            der Christoph Kroschke GmbH. Das Kopieren von Texten und Bildern 
            zum kommerziellen Gebrauch ist nur mit ausdrücklicher schriftlicher 
            Genehmigung des DAD / der Christoph Kroschke GmbH gestattet.</SPAN>
									</SPAN></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top">&nbsp;</TD>
					<TD vAlign="top"><u><FONT face="Arial" size="1"></FONT></u></TD>
				</TR>
				<TR>
					<TD vAlign="top">&nbsp;</TD>
					<TD><!--#include File="../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TABLE></form>
	</body>
</HTML>
