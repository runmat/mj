<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report21.aspx.vb" Inherits="AppEC.Report21"%>
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
								<TD class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2" height="19">Bitte geben Sie die Auswahlkriterien 
									ein.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120"></TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="" vAlign="top">
												<TABLE class="BorderLeftBottom" id="Table2" cellSpacing="1" cellPadding="1" border="0">
													<TR>
														<TD></TD>
														<TD colSpan="3"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;</TD>
													</TR>
													<TR>
														<TD>&nbsp;&nbsp;</TD>
														<TD></TD>
														<TD></TD>
														<TD></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Hersteller:
														</TD>
														<TD><asp:dropdownlist id="ddl_Hersteller" runat="server"></asp:dropdownlist></TD>
														<TD noWrap></TD>
													</TR>
													<TR>
														<TD>&nbsp;&nbsp;</TD>
														<TD></TD>
														<TD></TD>
														<TD></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD></TD>
														<TD><asp:label id="lblInfo" runat="server" EnableViewState="False" CssClass="TextInfo">Nur Fahrzeuge von Securiti Fleet</asp:label></TD>
														<TD><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
