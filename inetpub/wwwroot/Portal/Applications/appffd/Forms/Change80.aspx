<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80.aspx.vb" Inherits="AppFFD.Change80" %>
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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server" Visible="False"> (Fahrzeugsuche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120" height="192">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="120">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="120"><asp:linkbutton id="cmdContinue" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" height="192">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="tblProtocoll" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" border="0">
													<TR>
														<TD class="TextLarge" noWrap align="right">Dateiname:</TD>
														<TD class="TextLarge" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" align="right"><asp:label id="lblDateiname" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">Datum Programmlauf:</TD>
														<TD class="StandardTableAlternate" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" align="right"><asp:label id="lblVerarbeitung_Datum" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">Uhrzeit Programmlauf:</TD>
														<TD class="TextLarge" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" align="right"><asp:label id="lblVerarbeitung_Zeit" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">Sätze gelesen:</TD>
														<TD class="StandardTableAlternate" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" align="right"><asp:label id="lblGelesen" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">Sätze übernommen:</TD>
														<TD class="TextLarge" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" align="right"><asp:label id="lblGespeichert" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" colspan="3"><hr>
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">Sätze abgewiesen, weil</TD>
														<TD class="TextLarge" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" align="right">&nbsp;&nbsp;</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">schon angelegt:</TD>
														<TD class="StandardTableAlternate" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" align="right"><asp:label id="lblBereits_Angelegt" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">Kunde unbekannt:</TD>
														<TD class="TextLarge" align="right"></TD>
														<TD class="TextLarge" align="right">
															<asp:label id="lblKundeUnbekannt" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">Grund unbekannt:</TD>
														<TD class="StandardTableAlternate" align="right">&nbsp;&nbsp;&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" align="right"><asp:label id="lblUnbekannt_Abgewiesen" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
									<TABLE id="tblUploadSelection" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="tbl0001" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="TextLarge" noWrap align="right">Dateiauswahl:&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><INPUT id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">&nbsp;</TD>
														<TD class="TextLarge">&nbsp;
															<asp:label id="lblExcelfile" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
