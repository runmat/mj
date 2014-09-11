<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change07_2.aspx.vb" Inherits="AppSIXT.Change07_2" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Anlegen)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdNew" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Neuanlage</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;
												<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" Visible="False" NavigateUrl="javascript:window.close()">Fenster schließen</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD class="" vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="1" width="100%" bgColor="white" border="0">
													<TR>
														<TD vAlign="center" width="150" height="22"><STRONG>LI/LZ/SO:</STRONG>
														</TD>
														<TD class="TextLarge" vAlign="center" height="22"><asp:dropdownlist id="drpLizLim" runat="server" Width="104px">
																<asp:ListItem Value="Auswahl" Selected="True">Auswahl</asp:ListItem>
																<asp:ListItem Value="LI">Limousine</asp:ListItem>
																<asp:ListItem Value="LZ">Lizenz</asp:ListItem>
																<asp:ListItem Value="SO">Sonstige</asp:ListItem>
															</asp:dropdownlist>*</TD>
													</TR>
													<TR>
														<TD vAlign="center" width="150" height="22"><STRONG>Regelname:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="22">
															<asp:textbox id="txtRegelname" runat="server" Width="268px" MaxLength="40"></asp:textbox>*</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150"><STRONG>PDI:</STRONG></TD>
														<TD class="TextLarge" vAlign="center"><STRONG><asp:textbox id="txtPDI" runat="server"></asp:textbox></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150"><STRONG>Referenz: </STRONG>
														</TD>
														<TD class="TextLarge" vAlign="center"><STRONG><asp:textbox id="txtReferenz" runat="server"></asp:textbox></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="17"><STRONG>FIN:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="17"><STRONG><asp:textbox id="txtFIN" runat="server" MaxLength="17"></asp:textbox></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Handelsname:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:textbox id="txtHandelsname" runat="server" Width="269px"></asp:textbox></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Model-Bezeichnung:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:textbox id="txtModell" runat="server" Width="269px"></asp:textbox></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Antrieb:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG>
																<asp:dropdownlist id="drpAntrieb" runat="server" Width="157px"></asp:dropdownlist></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Hersteller:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:dropdownlist id="drpHersteller" runat="server" Width="244px"></asp:dropdownlist>*</STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Leist. in KW:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:textbox id="txtLeistKW" runat="server"></asp:textbox></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Navi:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:dropdownlist id="drpNavi" runat="server" Width="157px"></asp:dropdownlist></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Bereifung:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:dropdownlist id="drpBereifung" runat="server" Width="157px"></asp:dropdownlist></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Farbe:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:dropdownlist id="drpFarbe" runat="server" Width="157px"></asp:dropdownlist></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Ausführung:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:dropdownlist id="drpAusfuehrung" runat="server" Width="157px"></asp:dropdownlist></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Kst. LN:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:textbox id="txtKstLN" runat="server"></asp:textbox>**</STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Name LN:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG><asp:textbox id="txtNameLN" runat="server" Width="269px"></asp:textbox>**</STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Versicherung:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG>
																<asp:dropdownlist id="drpVersicherer" runat="server" Width="244px"></asp:dropdownlist></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>Anzahl:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><asp:textbox id="txtAnzahl" runat="server" Width="50px" MaxLength="4"></asp:textbox><STRONG>*</STRONG>(bei 
															gesetzter FIN&nbsp;oder Referenz tragen Sie bitte eine 1 ein.)</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG> Zula auf?</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><STRONG>
																<asp:dropdownlist id="drpHalter" runat="server" Width="244px"></asp:dropdownlist></STRONG></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"><STRONG>gepl. Station:</STRONG></TD>
														<TD class="TextLarge" vAlign="center" height="19"><asp:textbox id="txtGeplStation" runat="server"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19">&nbsp;</TD>
														<TD class="TextLarge" vAlign="center" height="19"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"></TD>
														<TD class="TextLarge" vAlign="center" height="19">*&nbsp;&nbsp; Pflichtfelder</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150" height="19"></TD>
														<TD class="TextLarge" vAlign="center" height="19">** Pflichtfelder bei 
															Lizenzfahrzeugen</TD>
													</TR>
												</TABLE>
												&nbsp;
											</TD>
										</TR>
									</TABLE>
									<asp:label id="lblErrMessage" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --> &nbsp;</TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
