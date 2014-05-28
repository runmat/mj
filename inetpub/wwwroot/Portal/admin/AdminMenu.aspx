<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdminMenu.aspx.vb" Inherits="CKG.Admin.AdminMenu" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" cellpadding="0" cellspacing="0">
				<TBODY>
					<tr>
						<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2">Administration - bitte wählen...</td>
								</TR>
								<tr>
									<TD vAlign="top" width="120">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
											<TR>
												<TD class="TaskTitle">&nbsp;</TD>
											</TR>
										</TABLE>
									</TD>
									<td vAlign="top">
										<table width="100%" align="left" cellSpacing="0" cellPadding="0" border="0">
											<TBODY>
												<tr>
													<TD class="TaskTitle">
														<asp:HyperLink id="lnkUserManagement" runat="server" CssClass="TaskTitle" NavigateUrl="UserManagement.aspx" Visible="False">Benutzerverwaltung</asp:HyperLink>
														<asp:hyperlink id="lnkGroupManagement" runat="server" CssClass="TaskTitle" NavigateUrl="GroupManagement.aspx" Visible="False">Gruppenverwaltung</asp:hyperlink>
														<asp:hyperlink id="lnkOrganizationManagement" runat="server" CssClass="TaskTitle" NavigateUrl="OrganizationManagement.aspx" Visible="False">Organisationsverwaltung</asp:hyperlink>
														<asp:HyperLink id="lnkCustomerManagement" runat="server" CssClass="TaskTitle" NavigateUrl="CustomerManagement.aspx" Visible="False">Kundenverwaltung</asp:HyperLink>
														<asp:HyperLink id="lnkAppManagement" runat="server" CssClass="TaskTitle" NavigateUrl="AppManagement.aspx" Visible="False">Anwendungsverwaltung</asp:HyperLink>
														<asp:hyperlink id="lnkArchivManagement" runat="server" NavigateUrl="ArchivManagement.aspx" CssClass="TaskTitle" Visible="False">Archivverwaltung</asp:hyperlink>&nbsp;</TD>
												</tr>
								</tr>
								<tr>
									<td vAlign="top" align="left" class="TextLarge">
										&nbsp;&nbsp;&nbsp;</td>
								</tr>
								<TR>
									<TD height="25">
									</TD>
								</TR>
							</TABLE>
						</td>
					</tr>
					<tr>
						<td></td>
						<td><asp:label id="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:label>
							<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></td>
					</tr>
					<tr>
						<td></td>
						<td><!--#include File="../PageElements/Footer.html" --></td>
					</tr>
				</TBODY></table>
			</TD></TR></TBODY></TABLE>
		</form>
	</body>
</HTML>
