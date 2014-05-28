<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report01.aspx.vb" Inherits="AppEC.Report01" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2" height="19">
									<asp:Label id="lblHead" runat="server"></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2" height="19">Bitte Hersteller auswählen.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<P>&nbsp;</P>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE class="BorderLeftBottom" id="Table2" cellSpacing="1" cellPadding="1" border="0">
													<TR>
														<TD>&nbsp;</TD>
														<TD colSpan="3">
															<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Hersteller:</TD>
														<TD>
															<asp:DropDownList id="ddlHersteller" runat="server" CssClass="DropDownStyle"></asp:DropDownList></TD>
														<TD></TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD></TD>
														<TD></TD>
														<TD></TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD></TD>
														<TD></TD>
														<TD>
															<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter&nbsp;&#187</asp:LinkButton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
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
