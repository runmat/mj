<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change10.aspx.vb" Inherits="AppCSC.Change10" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" noWrap colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> - Vorgangssuche</asp:label>
								</td>
							</TR>
							<tr>
								<TD vAlign="top" width="82">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="120">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table4" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="">
												<asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" width="100%">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white">
													<TR>
														<TD class="TextLarge" width="150">Kennzeichen:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtKennzeichen" runat="server" Width="350px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" width="150">Kontonummer:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtVertragsnummer" runat="server" Width="350px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" width="150">
															Fahrgestellnummer:</TD>
														<TD class="TextLarge">
															<asp:textbox id="txtFahrgestellnummer" runat="server" Width="350px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" width="150"></TD>
														<TD class="TextLarge">Bitte geben mindestens ein Suchkriterium ein.<BR>
															Platzhalter sind in dieser Suche nicht zul�ssig.</TD>
													</TR>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
								</TD>
							</tr>
							<TR>
								<TD vAlign="top" width="82">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="82">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<script language="JavaScript">
<!-- //
 window.document.Form1.txtVertragsnummer.focus();
//-->
		</script>
	</body>
</HTML>
