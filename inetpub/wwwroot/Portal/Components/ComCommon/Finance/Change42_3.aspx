<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change42_3.aspx.vb" Inherits="CKG.Components.ComCommon.Change42_3" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
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
				<TBODY>
					<tr>
						<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" Width="3px" ImageUrl="/Portal/Images/empty.gif"></asp:imagebutton></td>
					</tr>
					<TR>
						<TD vAlign="top" align="left" colSpan="3">
							<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
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
													<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TR>
													<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change42.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change42_2.aspx">Fahrzeugauswahl</asp:hyperlink></TD>
												</TR>
											</TABLE>
											<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TBODY>
													<tr>
														<td vAlign="top" align="left" colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
													</tr>
													<TR>
														<TD vAlign="top" align="left" colSpan="3"></TD>
													</TR>
													<TR>
														<TD vAlign="top" align="left" colSpan="3">&nbsp;<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</TR>
													<TR>
														<td class="TextLarge" vAlign="top" align="left" colSpan="3">
															<TABLE id="Table2" cellSpacing="0" cellPadding="5" with="100%" bgColor="white" border="0">
																<TR>
																	<TD class="StandardTableAlternate"><U>Zustellungsart:</U>
																	</TD>
																	<TD class="StandardTableAlternate" noWrap><asp:radiobutton id="rb_VersandStandard" runat="server" Text="rb_VersandStandard" Checked="True" GroupName="Versandart"></asp:radiobutton><BR>
																		&nbsp;&nbsp;<FONT color="red">(siehe Hinweis)</FONT>&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternate" noWrap><asp:radiobutton id="rb_0900" runat="server" Text="rb_0900" GroupName="Versandart"></asp:radiobutton>&nbsp;&nbsp;<BR>
																		&nbsp;&nbsp;
																		<asp:label id="lbl_0900" runat="server"> lbl_0900</asp:label>&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternate" noWrap><asp:radiobutton id="rb_1000" runat="server" Text="rb_1000" GroupName="Versandart"></asp:radiobutton>&nbsp;&nbsp;<br>
																		&nbsp;&nbsp;<asp:label id="lbl_1000" runat="server"> lbl_1000</asp:label>&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternate" noWrap><asp:radiobutton id="rb_1200" runat="server" Text="rb_1200" GroupName="Versandart"></asp:radiobutton>&nbsp;&nbsp;<BR>
																		&nbsp;&nbsp;
																		<asp:label id="lbl_1200" runat="server"> lbl_1200</asp:label>&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternate" noWrap>
                                                                        <asp:radiobutton id="rb_SendungsVerfolgt" 
                                                                            runat="server" Text="rb_SendungsVerfolgt" GroupName="Versandart"></asp:radiobutton>
                                                                        <br />
																		<asp:label id="lbl_SendungsVerfolgt" runat="server">lbl_SendungsVerfolgt</asp:label></TD>
																	<td class="StandardTableAlternate" width="100%"></td>
																</TR>
															</TABLE>
														</td>
													</TR>
													<tr>
														<td class="TextLarge" vAlign="top" align="left" colSpan="3">
															<TABLE id="Table2" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
																<TBODY>
																	<TR>
																	</TR>
																	<TR>
																		<TD class="StandardTableAlternate" width="450" colSpan="3"><EM>Achtung </EM>:&nbsp;Die 
																			Nettopreise verstehen sich pro Sendung.</TD>
																	</TR>
																	<TR>
																		<TD class="StandardTableAlternate" width="135"><U></U><asp:radiobutton id="rb_Zweigstellen" runat="server" Text="<u>Versandadresse:</u>" Checked="True" GroupName="grpVersand" AutoPostBack="True"></asp:radiobutton></TD>
																		<TD class="StandardTableAlternate" noWrap colSpan="2"><asp:dropdownlist id="cmbZweigstellen" runat="server"></asp:dropdownlist></TD>
																	</TR>
																	<TR>
																		<TD class="StandardTableAlternate" width="135"><asp:radiobutton id="rb_ZulStelle" runat="server" Text="<u>Zulassungsstelle:</u>" GroupName="grpVersand" AutoPostBack="True"></asp:radiobutton></TD>
																		<TD class="StandardTableAlternate" noWrap colSpan="2"><asp:dropdownlist id="ddl_ZulStelle" runat="server" Visible="False"></asp:dropdownlist></TD>
																	</TR>
																	<TR id="tr_Adresse" runat="server">
																		<TD class="StandardTableAlternate" vAlign="top" width="135"><asp:radiobutton id="rb_Manuell" runat="server" Text="<u> manuelle Eingabe:</u>" GroupName="grpVersand" AutoPostBack="True"></asp:radiobutton></TD>
																		<TD class="StandardTableAlternate" noWrap colSpan="2">
																			<table id="tbl_Adresse" visible="False" runat="server" align="left" border="0" bgColor="white" width="423" cellPadding="5" cellSpacing="0" height="155">
																				<TR>
																					<TD class="StandardTableAlternate">
																						<asp:label id="Label3" runat="server">Name:</asp:label></TD>
																					<TD class="StandardTableAlternate">
																						<asp:textbox id="txt_Name" runat="server" Width="255px"></asp:textbox></TD>
																					<TD class="StandardTableAlternate" width="188"></TD>
																				</TR>
																				<TR>
																					<TD class="StandardTableAlternate">
																						<asp:label id="lbl_Name2" runat="server">lbl_Name2</asp:label></TD>
																					<TD class="StandardTableAlternate">
																						<asp:textbox id="txt_Name2" runat="server" Width="255px"></asp:textbox></TD>
																					<TD class="StandardTableAlternate" width="188"></TD>
																				</TR>
																				<TR>
																					<TD class="StandardTableAlternate">
																						<asp:label id="Label2" runat="server">Strasse:</asp:label></TD>
																					<TD class="StandardTableAlternate">
																						<asp:textbox id="txt_Strasse" runat="server" Width="254px"></asp:textbox></TD>
																					<TD class="StandardTableAlternate" width="188">
																						<asp:label id="lbl_Nummer" runat="server">Nr.:</asp:label>&nbsp;
																						<asp:textbox id="txt_Nummer" runat="server" Width="45px"></asp:textbox></TD>
																				</TR>
																				<TR>
																					<TD class="StandardTableAlternate" height="27">
																						<asp:label id="lbl_PLZ" runat="server">PLZ:</asp:label></TD>
																					<TD class="StandardTableAlternate" height="27">
																						<asp:textbox id="txt_PLZ" runat="server" Width="99px"></asp:textbox></TD>
																					<TD class="StandardTableAlternate" height="27">
																					</TD>
																				</TR>
																				<TR>
																					<TD class="StandardTableAlternate">
																						<asp:label id="Label1" runat="server">Ort:</asp:label></TD>
																					<TD class="StandardTableAlternate">
																						<asp:textbox id="txt_Ort" runat="server" Width="255px"></asp:textbox></TD>
																					<TD class="StandardTableAlternate" width="188"></TD>
																				</TR>
																				<TR>
																					<TD class="StandardTableAlternate" height="27">
																						<asp:label id="lbl_Land" runat="server">Land:</asp:label></TD>
																					<TD class="StandardTableAlternate" height="27">
																						<asp:dropdownlist id="ddl_Land" Runat="server"></asp:dropdownlist>
																					<TD class="StandardTableAlternate" height="27">
																					</TD>
																				</TR>
																			</table>
																		</TD>
																	</TR>
																</TBODY></TABLE>
														</td>
													</tr>
												</TBODY></TABLE>
										</td>
									</tr>
								</TBODY></TABLE>
						</TD>
					</TR>
				</TBODY></table>
			</TD></TR></TBODY></TABLE>&nbsp; </TD></TR><TR>
				<TD colSpan="3" align="left" vAlign="top">
					<TABLE class="InfoText" id="Table4" cellSpacing="1" cellPadding="1" border="0">
						<TR>
							<TD><STRONG><U>Hinweis:<BR>
									</U></STRONG>Die Deutsche Post AG garantiert für&nbsp;Standardsendungen 
								keine Zustellzeiten<BR>
								und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
								<BR>
								<BR>
								&nbsp;&nbsp;&nbsp;-95% aller Sendungen werden dem Empfänger innerhalb von 24 
								Stunden zugestellt,<BR>
								&nbsp;&nbsp;&nbsp;-3% aller Sendungen benötigen zwischen 24 und 48 Stunden bis 
								zur Zustellung.<BR>
								<BR>
								Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post 
								AG.
							</TD>
						</TR>
					</TABLE>
				</TD>
			</TR>
			<TR>
				<TD colSpan="3" align="left" vAlign="top"></TD>
			</TR>
			<tr>
				<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><br>
				</td>
			</tr>
			</TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></form>
		<!--<script language="JavaScript">
			 			window.document.Form1.elements[window.document.Form1.length-2].focus();
			
		</script>-->
		</TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></FORM>
	</body>
</HTML>
