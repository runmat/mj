<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07_4.aspx.vb" Inherits="AppFFE.Change07_4" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" ImageUrl="/Portal/Images/empty.gif" Width="3px"></asp:imagebutton></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Adressauswahl)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSearch" runat="server" 
                                                    CssClass="StandardButton" Visible="False">&#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top" align="left" colSpan="3">
												<table id="Table2" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<tr>
														<td class="StandardTableAlternate">Zustellung:<BR>
															&nbsp;&nbsp;</td>
														<td class="StandardTableAlternate" noWrap>
															<asp:radiobutton id="rb_VersandStandard" runat="server" Text="rb_VersandStandard" Checked="True" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;<FONT color="#000000">(siehe Hinweis)</FONT></td>
														<td class="StandardTableAlternate" noWrap>
															<asp:radiobutton id="rb_0900" runat="server" Text="rb_0900" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:label id="lbl_0900" runat="server"> lbl_0900</asp:label></td>
														<td class="StandardTableAlternate" noWrap>
															<asp:radiobutton id="rb_1000" runat="server" Text="rb_1000" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:label id="lbl_1000" runat="server"> lbl_1000</asp:label></td>
														<td class="StandardTableAlternate" width="100%">
															<asp:radiobutton id="rb_1200" runat="server" Text="rb_1200" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;&nbsp;&nbsp;
															<asp:label id="lbl_1200" runat="server"> lbl_1200</asp:label></td>
													</tr>
													<tr>
														<td class="StandardTableAlternate" colSpan="5">Achtung: Auslieferungen erfolgen 
															täglich bei Beauftragung vor 16 Uhr. 
															Die Nettopreise verstehen sich pro Sendung.</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<table id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<tr>
														<td class="StandardTableAlternate" width="170">&nbsp;<asp:radiobutton id="chkZweigstellen" runat="server" GroupName="grpVersand" Checked="True" Text="Versandadressen:" AutoPostBack="True"></asp:radiobutton>&nbsp;</td>
														<td class="StandardTableAlternate" vAlign="top" align="left" width="100%"><asp:dropdownlist id="cmbZweigstellen" runat="server"></asp:dropdownlist></td>
													</tr>
													<tr id="ZeigeZULST" runat="server">
														<td class="StandardTableAlternate" vAlign="top" width="170">&nbsp;<asp:radiobutton id="chkZulassungsstellen" runat="server" GroupName="grpVersand" Text="Zulassungsstellen:" AutoPostBack="True"></asp:radiobutton>&nbsp;</td>
														<td class="StandardTableAlternate" vAlign="top" align="left" width="100%"><asp:dropdownlist id="cmbZuslassungstellen" Visible="False" runat="server"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;
														</td>
													</tr>
													<tr id="tr_Adresse" runat="server">
														<td class="StandardTableAlternate" width="17">
															<table id="Table22" height="155" cellSpacing="0" cellPadding="5" width="147" align="left" bgColor="white" border="0">
																<tr>
																	<td class="StandardTableAlternate" align="left" width="173" height="29">
																		<asp:radiobutton id="rb_Manuell" runat="server" GroupName="grpVersand" Text=" manuelle Eingabe:" AutoPostBack="True"></asp:radiobutton>
																	</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																</tr>
															</table>
														</td>
														<td class="StandardTableAlternate" noWrap>
															<table id="tbl_Adresse" visible="False" runat="server" height="155" 
                                                                cellSpacing="0" cellPadding="0" align="left" bgColor="white" border="0">
																<tr>
																	<td class="StandardTableAlternate" width="200"><asp:label id="Label3" runat="server">Name:</asp:label>&nbsp;</td>
																	<td class="StandardTableAlternate"><asp:textbox id="txt_Name" runat="server" Width="255px"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr id="tr_Name2">
																	<td class="StandardTableAlternate" width="200"><asp:label id="lbl_Name2" 
                                                                            runat="server">lbl_Name2</asp:label></td>
																	<td class="StandardTableAlternate"><asp:textbox id="txt_Name2" runat="server" 
                                                                            Width="255px"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188">&nbsp;</td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="200"><asp:label id="Label2" runat="server">Strasse:</asp:label>&nbsp;</td>
																	<td class="StandardTableAlternate" width="188"><asp:textbox id="txt_Strasse" runat="server" Width="255px"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188">&nbsp;&nbsp;<asp:label id="lbl_Nummer" runat="server">Nr.:</asp:label>&nbsp;
																		<asp:textbox id="txt_Nummer" runat="server" Width="45px" MaxLength="10"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate"  width="200"><asp:label id="lbl_PLZ" runat="server">PLZ:</asp:label>&nbsp;</td>
																	<td class="StandardTableAlternate"><asp:textbox id="txt_PLZ" runat="server" Width="99px"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="200"><asp:label id="Label1" runat="server">Ort:</asp:label></td>
																	<td class="StandardTableAlternate"><asp:textbox id="txt_Ort" runat="server" Width="255px"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="200">
																		<asp:label id="lbl_Land" runat="server">Land:</asp:label></td>
																	<td class="StandardTableAlternate">
																		<asp:dropdownlist id="ddl_Land" Runat="server" Enabled="False">
																			<asp:ListItem Value="0" Selected="True">DE</asp:ListItem>
																		</asp:dropdownlist></td>
																	<td class="StandardTableAlternate" width="188"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td class="TextLarge" vAlign="top" width="173">&nbsp;&nbsp;
														</td>
														<td class="TextLarge" vAlign="top" align="left" width="100%">&nbsp;&nbsp;
														</td>
													</tr>
													<tr id="ZeigeTEXT50" runat="server">
														<td class="StandardTableAlternate" vAlign="top" noWrap width="173">Kunde für 
															Anforderungen mit<BR>
															erweitertem Zahlungsziel<BR>
															(Delayed Payment) endgültig</td>
														<td class="StandardTableAlternate" vAlign="top" align="left" width="100%"><asp:textbox id="txtTEXT50" runat="server" MaxLength="50"></asp:textbox>&nbsp;&nbsp;
															<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Eingabe erforderlich" ControlToValidate="txtTEXT50"></asp:requiredfieldvalidator></td>
													</tr>
													<tr>
														<td class="InfoText" vAlign="top" noWrap width="553" colSpan="2"><STRONG><U>Hinweis:<BR>
																</U></STRONG>Die Deutsche Post AG garantiert für diese Sendungen keine 
															Lauf- und Zustellungszeiten<BR>
															und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
															<BR>
															<BR>
															<STRONG>&nbsp;&nbsp;&nbsp;-95% aller Sendungen werden dem Empfänger innerhalb von 
																24 Stunden zugestellt,</STRONG><BR>
															<STRONG>&nbsp;&nbsp;&nbsp;-3% aller Sendungen benötigen zwischen 24 und 48 Stunden 
																bis zur Zustellung.</STRONG><BR>
															<BR>
															Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post 
															AG.
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<tr>
											<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><br>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
		<!--<script language="JavaScript">
			 //
			window.document.Form1.elements[window.document.Form1.length-1].focus();
			//
		</script>-->
	</body>
</HTML>
