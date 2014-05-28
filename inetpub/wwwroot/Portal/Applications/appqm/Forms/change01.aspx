<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="change01.aspx.vb" Inherits="appQM.change01" %>
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
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Erfassen)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="100" height="358">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdAbsenden" runat="server" CssClass="StandardButton" tabIndex="15">&#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:linkbutton id="cmdNew" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Neu</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" tabIndex="16">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" height="358">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="918" border="0">
													<TR>
														<TD class="TextLarge" height="13">
															<P align="right">* = 
																erforderlich&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</P>
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge">
															<P align="center"><STRONG><FONT size="5">Reklamation erfassen</FONT></STRONG></P>
														</TD>
													</TR>
												</TABLE>
												<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="918" border="0">
													<TR>
														<TD>
															<P align="center"><STRONG>Referenz*</STRONG></P>
														</TD>
														<TD>
															<P align="center"><STRONG>Anz.Pos</STRONG></P>
														</TD>
														<TD>
															<P align="center"><STRONG>Meldedatum(TTMMJJ)*</STRONG></P>
														</TD>
														<TD>
															<P align="center"><STRONG>Kundenreklamation*</STRONG></P>
														</TD>
													</TR>
													<TR>
														<TD>
															<P align="center"><asp:textbox id="txtReferenz" runat="server" Width="185px" MaxLength="25" tabIndex="1"></asp:textbox></P>
														</TD>
														<TD>
															<P align="center"><asp:textbox id="txtAnzPos" runat="server" Width="56px" MaxLength="3">1</asp:textbox></P>
														</TD>
														<TD>
															<P align="center"><asp:textbox id="txtMeldedatum" runat="server" Width="94px" MaxLength="6" tabIndex="2"></asp:textbox></P>
														</TD>
														<TD>
															<P align="center"><asp:radiobuttonlist id="rbKundenrekla" runat="server" RepeatDirection="Horizontal" tabIndex="3">
																	<asp:ListItem Value="1" Selected="True">Ja</asp:ListItem>
																	<asp:ListItem Value="0">Nein</asp:ListItem>
																</asp:radiobuttonlist></P>
														</TD>
													</TR>
												</TABLE>
												<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="918" border="0">
													<TR>
														<TD width="164">
															<P align="right"><STRONG>Kundennr.*</STRONG></P>
														</TD>
														<TD width="389">
															<P align="center"><STRONG>Kundenname</STRONG></P>
														</TD>
														<TD>
															<P align="center"><STRONG>Prozess</STRONG></P>
														</TD>
													</TR>
													<TR>
														<TD width="164"><STRONG>
																<P align="right"><asp:textbox id="txtKundennr" runat="server" Width="81px" MaxLength="10" tabIndex="4"></asp:textbox></P>
															</STRONG>
														</TD>
														<TD width="389">
															<P align="center"><asp:textbox id="txtKundenname" runat="server" Width="310px" MaxLength="40" tabIndex="5"></asp:textbox></P>
														</TD>
														<TD>
															<P align="center"><asp:dropdownlist id="ddProzess" runat="server" Width="300px" tabIndex="8"></asp:dropdownlist></P>
														</TD>
													</TR>
													<TR>
														<TD width="164">
															<P align="right"><STRONG>Ansprechpartner</STRONG></P>
														</TD>
														<TD width="389">
															<P align="center"><asp:textbox id="txtAnsprechpartner" runat="server" Width="310px" MaxLength="40" tabIndex="6"></asp:textbox></P>
														</TD>
														<TD>
															<P align="center"><STRONG>Fehler</STRONG></P>
														</TD>
													</TR>
													<TR>
														<TD width="164">
															<P align="right"><STRONG>Kontakt AP</STRONG></P>
														</TD>
														<TD width="389">
															<P align="center"><asp:textbox id="txtKontakdaten" runat="server" Width="310px" MaxLength="40" tabIndex="7"></asp:textbox></P>
														</TD>
														<TD>
															<P align="center"><asp:dropdownlist id="ddFehler" runat="server" Width="300px" tabIndex="9"></asp:dropdownlist></P>
														</TD>
													</TR>
												</TABLE>
												<P align="center"><STRONG>
														<TABLE id="Table8" cellSpacing="0" cellPadding="0" width="918" align="left" border="0">
															<TR>
																<TD>
																	<P align="center"><STRONG><FONT size="5">Fehlerbeschreibung</FONT></STRONG></P>
																</TD>
															</TR>
															<TR>
																<TD>
																	<P align="center"><asp:textbox id="txtFehlerbeschreibung" runat="server" Width="806px" MaxLength="255" Height="68px" tabIndex="10"></asp:textbox></P>
																</TD>
															</TR>
															<TR>
																<TD>
																	<P align="center"><STRONG><FONT size="5">Verursacher</FONT></STRONG></P>
																</TD>
															</TR>
														</TABLE>
													</STRONG>
													<br>
												</P>
											</td>
										</tr>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="918" border="0">
										<TR>
											<TD>
												<P align="center"><STRONG>Verursacher Firma</STRONG></P>
											</TD>
											<TD>
												<P align="center"><STRONG>Verursacher Name</STRONG></P>
											</TD>
										</TR>
										<TR>
											<TD>
												<P align="center"><asp:textbox id="txtVerursacherFirma" runat="server" Width="350px" MaxLength="40" tabIndex="11"></asp:textbox></P>
											</TD>
											<TD>
												<P align="center"><asp:textbox id="txtVerursacherName" runat="server" Width="350px" MaxLength="40" tabIndex="12"></asp:textbox></P>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table9" cellSpacing="0" cellPadding="0" width="918" border="0">
										<TR>
											<TD>
												<P align="center"><STRONG><FONT size="5">Klärung / Status</FONT></STRONG></P>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="918" border="0">
										<TR>
											<TD>
												<P align="center"><STRONG>Klärungsverantwortlicher</STRONG></P>
											</TD>
											<TD>
												<P align="center"><STRONG>Status</STRONG></P>
											</TD>
										</TR>
										<TR>
											<TD>
												<P align="center"><asp:textbox id="txtKlaerungsVerantwortlName" runat="server" Width="350px" MaxLength="40" tabIndex="13"></asp:textbox></P>
											</TD>
											<TD>
												<P align="center"><asp:dropdownlist id="ddStatus" runat="server" Width="350px" tabIndex="14"></asp:dropdownlist></P>
											</TD>
										</TR>
										<TR>
											<TD>&nbsp;&nbsp;
											</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD>
												<P align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
												</P>
											</TD>
											<TD></TD>
										</TR>
									</TABLE>
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="lblError" runat="server" Width="350px" Font-Bold="True" ForeColor="Red"></asp:label>
								</td>
							</tr>
							<TR>
								<TD vAlign="top" width="100">&nbsp;</TD>
								<td>
									<P align="left">&nbsp;</P> <!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<TR>
								<TD vAlign="top" width="100"></TD>
								<TD></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<script language="JavaScript">
<!-- //
 window.document.Form1.txtReferenz.focus();
//-->
		</script>
	</body>
</HTML>
