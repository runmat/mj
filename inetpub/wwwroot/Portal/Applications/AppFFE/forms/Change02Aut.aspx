<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02Aut.aspx.vb" Inherits="AppFFE.Change02Aut" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD colSpan="3">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(<asp:label id="lblPageTitle" runat="server"></asp:label>)</td>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdAuthorize" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Autorisieren</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdDelete" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Löschen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE class="TableKontingent" id="Table3" cellSpacing="0" cellPadding="5" width="100%" bgColor="white">
										<tr>
											<td class="TextLarge" vAlign="top">Händlernummer:</td>
											<td class="TextLarge" vAlign="top" width="100%" colSpan="2"><asp:label id="lblHaendlerNummer" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="StandardTableAlternate" vAlign="top">Name:&nbsp;&nbsp;
											</td>
											<td class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:label id="lblHaendlerName" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top">Adresse:</td>
											<td class="TextLarge" vAlign="top" colSpan="2"><asp:label id="lblAdresse" runat="server"></asp:label></td>
										</tr>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<TR>
											<TD>
												<TABLE id="Table4" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="top">Vertragsnummer:</TD>
														<TD class="TextLarge" vAlign="top" colSpan="2"><asp:label id="lblVertragsnummer" runat="server"></asp:label></TD>
														<TD class="TextLarge" vAlign="top" noWrap>Angefordert am:</TD>
														<TD class="TextLarge" vAlign="top" width="100%" colSpan="2"><asp:label id="lblAngefordertAm" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="top">Fahrgestellnummer:&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:label id="lblFahrgestellnummer" runat="server"></asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="200">Storno:&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:radiobutton id="rdbStorno" runat="server" Enabled="False" GroupName="grpStorno"></asp:radiobutton></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top">ZBII Nummer:</TD>
														<TD class="TextLarge" vAlign="top" colSpan="2"><asp:label id="lblBriefnummer" runat="server"></asp:label></TD>
														<TD class="TextLarge" vAlign="top" width="200">Freigabe:&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="top" colSpan="2"><asp:radiobutton id="rdbFreigabe" runat="server" Enabled="False" GroupName="grpStorno"></asp:radiobutton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="top">Kontingentart:&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:label id="lblKontingentart" runat="server"></asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="200">Kunde:
														</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:label id="lblKunde" runat="server"></asp:label></TD>
													</TR>
													<TR id="TR_Betrag" runat="server" Visible="False">
														<TD class="StandardTableAlternate" vAlign="top"></TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="2"></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="200">Betrag:</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:label id="lblBetrag" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD class="TextLarge" vAlign="top" colSpan="2"></TD>
														<TD class="TextLarge" vAlign="top" width="200">Fälligkeit:&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="top" colSpan="2"><asp:label id="lblFaelligkeit" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table8" cellSpacing="0" cellPadding="5" width="100%" border="0">
										<TR id="ConfirmMessage" runat="server">
											<TD class="LabelExtraLarge"><asp:label id="lblInformation" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<tr>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
