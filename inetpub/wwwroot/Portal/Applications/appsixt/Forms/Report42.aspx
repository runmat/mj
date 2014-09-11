<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report42.aspx.vb" Inherits="AppSIXT.Report42" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
							<TBODY>
								<TR>
									<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server" Visible="False"></asp:label></td>
								</TR>
								<tr>
									<TD vAlign="top" width="120" height="192">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
											<TR>
												<TD class="TaskTitle" width="120">&nbsp;</TD>
											</TR>
											<TR id="trcmdUpload" runat="server">
												<TD vAlign="center" width="120">
													<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton></TD>
											</TR>
											<TR id="trcmdSearch" runat="server">
												<TD vAlign="center" width="120"></TD>
											</TR>
										</TABLE>
									</TD>
									<TD vAlign="top" align="right">
										<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" vAlign="top" align="right">&nbsp;</TD>
											</TR>
										</TABLE>
										<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
											<tr>
												<td vAlign="top" align="left">
												</td>
											</tr>
										</TABLE>
									</TD>
					</td>
				</tr>
				<TR>
					<TD vAlign="top" width="120">&nbsp;</TD>
					<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="top" width="120">&nbsp;</TD>
					<TD align="right"><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
				<SCRIPT language="JavaScript">										
						<!--

						-->
				</SCRIPT>
			</table>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
