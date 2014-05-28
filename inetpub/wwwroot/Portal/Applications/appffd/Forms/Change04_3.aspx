<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change04_3.aspx.vb" Inherits="AppFFD.Change04_3" %>
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
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" Width="3px" ImageUrl="/Portal/Images/empty.gif"></asp:imagebutton></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Adressauswahl)</asp:label>
								</td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx" CssClass="TaskTitle">Fahrzeugauswahl</asp:hyperlink></TD>
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
                                                        <td class="StandardTableAlternate" style="width: 100%">
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
														<TD class="TextLarge" width="350"><asp:radiobutton id="chkZweigstellen" runat="server" Text="Zweigstellen:" Checked="True" GroupName="grpVersand"></asp:radiobutton></TD>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="3" width="100%">
															<asp:dropdownlist id="cmbZweigstellen" runat="server"></asp:dropdownlist></TD>
													</TR>
													<TR id="ZeigeZULST" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" width="350"><asp:radiobutton id="chkZulassungsstellen" runat="server" Text="Zulassungsstellen:" GroupName="grpVersand"></asp:radiobutton></TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" colSpan="3" width="100%">Kennzeichen:
															<asp:textbox id="txtKennzeichen" runat="server" Width="70px"></asp:textbox>&nbsp;&nbsp;&nbsp; 
															Ort:
															<asp:textbox id="TextBox1" runat="server" Width="150px"></asp:textbox><BR>
															&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<BR>
															<asp:dropdownlist id="cmbZuslassungstellen" runat="server" Visible="False"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" width="350">&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="3" width="100%">&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="ZeigeTEXT50" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" noWrap width="350">Kunde für 
															Anforderungen mit<BR>
															erweitertem Zahlungsziel<BR>
															(Delayed Payment) endgültig</TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" colSpan="3" width="100%">
															<asp:TextBox id="txtTEXT50" runat="server" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;
															<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtTEXT50" ErrorMessage="Eingabe erforderlich"></asp:RequiredFieldValidator></TD>
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
		<script language="JavaScript">
			<!-- //
			window.document.Form1.elements[window.document.Form1.length-2].focus();
			//-->
		</script>
	</body>
</HTML>
