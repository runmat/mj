<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report30_3.aspx.vb" Inherits="AppRDZ.Report30_3" %>
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
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server" Visible="False"> (ABE-Daten)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()" CssClass="TaskTitle">Fenster schlieﬂen</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkPrint" runat="server" NavigateUrl="javascript:window.print()" CssClass="TaskTitle">Drucken</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"></td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" vAlign="top" align="left"><asp:label id="lblMessage" runat="server"></asp:label>&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="2">
															<TABLE id="Table2" cellSpacing="0" cellPadding="2" border="0" width="100%" bgColor="#ffffff">
																<TR>
																	<TD class="TextHeader" vAlign="top" colspan="2"><asp:label id="Label1" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" width="120">
																		&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="GridTableAlternate" vAlign="top" width="120">Post-Anschrift:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" width="100%"><asp:label id="Label3" runat="server">-</asp:label><BR>
																		<asp:Label id="Label4" runat="server"></asp:Label><BR>
																		<asp:Label id="Label5" runat="server"></asp:Label>&nbsp;
																		<asp:Label id="Label6" runat="server"></asp:Label><BR>
																		<asp:Label id="Label7" runat="server"></asp:Label>&nbsp;
																		<asp:Label id="Label8" runat="server"></asp:Label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription">
																		&nbsp;</TD>
																	<TD class="TextLarge">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription">Ansprechpartner:</TD>
																	<TD class="TextLarge">
																		<asp:label id="Label2" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription">&nbsp;</TD>
																	<TD class="TextLarge">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="GridTableAlternate" width="120">Telefon 1:</TD>
																	<TD class="StandardTableAlternate"><asp:label id="Label9" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="GridTableAlternate" width="120">Telefon 2:</TD>
																	<TD class="StandardTableAlternate">
																		<asp:label id="Label10" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="GridTableAlternate" vAlign="top" width="120">Telefon 3:</TD>
																	<TD class="StandardTableAlternate" vAlign="top"><asp:label id="Label11" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" width="120">Fax:</TD>
																	<TD class="TextLarge" vAlign="top"><asp:label id="Label12" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="GridTableAlternate" vAlign="top" width="120">
																		eMail:</TD>
																	<TD class="StandardTableAlternate" vAlign="top"><asp:label id="Label13" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" width="120">&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="GridTableAlternate" vAlign="top">Anforderungen:</TD>
																	<TD class="StandardTableAlternate" vAlign="top"><asp:label id="Label14" runat="server">-</asp:label><BR>
																		<asp:label id="Label15" runat="server">-</asp:label><BR>
																		<asp:label id="Label16" runat="server">-</asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" width="120">&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="GridTableAlternate" vAlign="top">Bemerkungen:</TD>
																	<TD class="StandardTableAlternate" vAlign="top"><asp:label id="Label17" runat="server">-</asp:label>&nbsp;</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<tr>
											<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --></td>
										</tr>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
