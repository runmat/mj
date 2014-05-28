<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change07_4.aspx.vb" Inherits="AppFFD.Change07_4"%>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top" align="left" colSpan="3">
												<TABLE id="Table2" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="StandardTableAlternate"><asp:Label runat="server" ID="lblZustellung" Text="Zustellung: "></asp:Label></TD>
														<td class="StandardTableAlternate" style="width: 100%" valign="top">
                                                            <asp:DropDownList runat="server" ID="drpVersand">
                                                                <asp:ListItem Value="1391">Standardversand (Deutsche Post - siehe Hinweis*)</asp:ListItem>
                                                                <asp:ListItem Value="5530">sendungsverfolgter Versand (4,95€ netto)</asp:ListItem>
                                                                <asp:ListItem Value="1390">Expressversand vor 12:00 Uhr (17,80€ netto)</asp:ListItem>
                                                                <asp:ListItem Value="1389">Expressversand vor 10:00 Uhr (23,00€ netto)</asp:ListItem>
                                                                <asp:ListItem Value="1385">Expressversand vor 09:00 Uhr (28,20€ netto)</asp:ListItem>
                                                                
                                                            </asp:DropDownList>

                                                        </td>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" colSpan="6">Achtung: Auslieferungen erfolgen 
															täglich bei Beauftragung vor 15 Uhr unter Berücksichtigung freier Kontingente. 
															Die Nettopreise verstehen sich pro Sendung.</TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" width="173">&nbsp;<asp:radiobutton id="chkZweigstellen" runat="server" GroupName="grpVersand" Checked="True" Text="Zweigstellen:"></asp:radiobutton>&nbsp;</TD>
														<TD class="TextLarge" vAlign="top" align="left" width="100%" colSpan="3"><asp:dropdownlist id="cmbZweigstellen" runat="server"></asp:dropdownlist></TD>
													</TR>
													<TR id="ZeigeZULST" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" width="173">&nbsp;<asp:radiobutton id="chkZulassungsstellen" runat="server" GroupName="grpVersand" Text="Zulassungsstellen:"></asp:radiobutton>&nbsp;</TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" width="100%" colSpan="3"><asp:dropdownlist id="cmbZuslassungstellen" runat="server"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="tr_Adresse" runat="server">
														<TD class="StandardTableAlternate" width="173">
															<TABLE id="Table22" height="155" cellSpacing="0" cellPadding="5" width="147" align="left" bgColor="white" border="0">
																<tr>
																	<td class="StandardTableAlternate" align="left" width="161" height="29">
																		<asp:radiobutton id="rb_Manuell" runat="server" GroupName="grpVersand" Text=" manuelle Eingabe:"></asp:radiobutton>
																	</td>
																</tr>
																<TR>
																	<TD class="StandardTableAlternate" width="188" height="27"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate" width="188" height="27"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate" width="188" height="27"></TD>
																</TR>
															</TABLE>
														</TD>
														<TD class="StandardTableAlternate" noWrap colSpan="2">
															<TABLE id="Table9" height="155" cellSpacing="0" cellPadding="5" width="423" align="left" bgColor="white" border="0">
																<TR>
																	<TD class="StandardTableAlternate"><asp:label id="Label3" runat="server">Name:</asp:label></TD>
																	<TD class="StandardTableAlternate"><asp:textbox id="txt_Name" runat="server" Width="255px"></asp:textbox></TD>
																	<TD class="StandardTableAlternate" width="188"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate"><asp:label id="Label2" runat="server">Strasse:</asp:label></TD>
																	<TD class="StandardTableAlternate"><asp:textbox id="txt_Strasse" runat="server" Width="254px"></asp:textbox></TD>
																	<TD class="StandardTableAlternate" width="188"><asp:label id="lbl_Nummer" runat="server">Nr.:</asp:label>&nbsp;
																		<asp:textbox id="txt_Nummer" runat="server" Width="45px" MaxLength="5"></asp:textbox></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate" height="27"><asp:label id="lbl_PLZ" runat="server">PLZ:</asp:label></TD>
																	<TD class="StandardTableAlternate" height="27"><asp:textbox id="txt_PLZ" runat="server" Width="99px"></asp:textbox></TD>
																	<TD class="StandardTableAlternate" width="188" height="27"></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate"><asp:label id="Label1" runat="server">Ort:</asp:label></TD>
																	<TD class="StandardTableAlternate"><asp:textbox id="txt_Ort" runat="server" Width="255px"></asp:textbox></TD>
																	<TD class="StandardTableAlternate" width="188"></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" width="173">&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" vAlign="top" align="left" width="100%" colSpan="3">&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="ZeigeTEXT50" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" noWrap width="173">Kunde für 
															Anforderungen mit<BR>
															erweitertem Zahlungsziel<BR>
															(Delayed Payment) endgültig</TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" width="100%" colSpan="3"><asp:textbox id="txtTEXT50" runat="server" MaxLength="50"></asp:textbox>&nbsp;&nbsp;
															<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Eingabe erforderlich" ControlToValidate="txtTEXT50"></asp:requiredfieldvalidator></TD>
													</TR>
													<TR>
														<TD class="InfoText" vAlign="top" noWrap colSpan="3" width="553"><STRONG><U>*Wichtiger Hinweis:<BR>
																</U></STRONG>Die Deutsche Post AG garantiert für diese Sendungen keine 
															Lauf- und Zustellungszeiten!<BR>
				                                            Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post AG.
														</TD>
														<TD class="InfoText" vAlign="top" noWrap width="100%" colSpan="2"></TD>
													</TR>
												</TABLE>
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
