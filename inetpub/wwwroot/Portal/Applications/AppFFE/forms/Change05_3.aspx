<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change05_3.aspx.vb" Inherits="AppFFE.Change05_3" %>
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
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx" CssClass="TaskTitle">Fahrzeugauswahl</asp:hyperlink>&nbsp;</TD>
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
														<TD class="StandardTableAlternate">
															<P>Halteranschrift:<br>
																<asp:dropdownlist id="ddlHalter" runat="server"></asp:dropdownlist></P>
															<P>Empfänger Kfz - Schein &amp; - Schilder:<br>
																<asp:dropdownlist id="ddlEmpf" runat="server"></asp:dropdownlist>&nbsp;</P>
														</TD>
														<TD class="StandardTableAlternate"><BR>
															&nbsp;</TD>
														<TD class="StandardTableAlternate"><BR>
															&nbsp;&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><BR>
															&nbsp;&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><BR>
															&nbsp;&nbsp;&nbsp;&nbsp;</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" colSpan="5"><asp:radiobutton id="chkVersandStandard" runat="server" Visible="False" Text="innerhalb von 24 bis 48 h" Checked="True" GroupName="Versandart"></asp:radiobutton><asp:radiobutton id="chk0900" runat="server" Visible="False" Text="vor 9:00 Uhr" GroupName="Versandart"></asp:radiobutton><asp:radiobutton id="chk1000" runat="server" Visible="False" Text="vor 10:00 Uhr" GroupName="Versandart"></asp:radiobutton><asp:radiobutton id="chk1200" runat="server" Visible="False" Text="vor 12:00 Uhr" GroupName="Versandart"></asp:radiobutton><asp:radiobutton id="chkZweigstellen" runat="server" Visible="False" Text="Zweigstellen:" Checked="True" GroupName="grpVersand"></asp:radiobutton><asp:radiobutton id="chkZulassungsstellen" runat="server" Visible="False" Text="Zulassungsstellen:" GroupName="grpVersand"></asp:radiobutton></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge"></TD>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="2"></TD>
													</TR>
													<TR id="ZeigeZULST" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"></TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" colSpan="2">&nbsp;<asp:textbox id="txtKennzeichen" runat="server" Width="70px" Visible="False"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:textbox id="TextBox1" runat="server" Width="150px" Visible="False"></asp:textbox><asp:dropdownlist id="cmbZweigstellen" runat="server" Visible="False"></asp:dropdownlist><asp:textbox id="txtTEXT50" runat="server" Visible="False" MaxLength="50"></asp:textbox><asp:dropdownlist id="cmbZuslassungstellen" runat="server" Visible="False"></asp:dropdownlist>
															<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Visible="False" ControlToValidate="txtTEXT50" ErrorMessage="Eingabe erforderlich"></asp:RequiredFieldValidator></TD>
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
