<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report41_03.aspx.vb" Inherits="AppFFE.Report41_03" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="923" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server">Händlerübersicht</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="145">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="145"><asp:linkbutton id="cmdSelect" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Versandadressen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="145">
												<asp:linkbutton id="cmdBack" runat="server" Visible="True" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="145">
												<asp:hyperlink id="HyperLink1" runat="server" CssClass="StandardButton" Target="_blank" NavigateUrl="Report41_03Print.aspx">&#149;&nbsp;Druckversion</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="143"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="800" border="0" height="19">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" border="0">
										<tr>
											<td vAlign="top" align="left" width="782">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" border="0" height="92">
													<TR id="trHaendlernummer" runat="server">
														<TD class="TextLarge" width="190">Händlernummer:&nbsp;</TD>
														<TD class="TextLarge" width="654"><asp:label id="lblHDNummer" runat="server"></asp:label></TD>
													</TR>
													<TR id="trName" runat="server">
														<TD class="TextLarge" width="190">Name:&nbsp;</TD>
														<TD class="TextLarge" width="654"><asp:label id="lblName" runat="server"></asp:label></TD>
													</TR>
													<TR id="trOrt" runat="server">
														<TD class="TextLarge" width="190">Adresse:&nbsp;</TD>
														<TD class="TextLarge" width="654"><asp:label id="lblAdresse" runat="server"></asp:label></TD>
													</TR>
													<TR id="trHdAuswahl" runat="server">
														<TD class="TaskTitle" vAlign="top" width="190">Zusätzliche Versandadressen</TD>
														<TD class="TaskTitle" vAlign="top"></TD>
													</TR>
												</TABLE>
												<TABLE id="AddressTable" runat="server" cellSpacing="0" cellPadding="5" width="100%" border="0">
												</TABLE>
											</td>
										</tr>
									</TABLE>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
									<asp:label id="lblMessage" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR>
					<TD vAlign="top"><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
			</table>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
