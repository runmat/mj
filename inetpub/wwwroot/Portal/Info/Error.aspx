<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Error.aspx.vb" Inherits="CKG.Portal.Info.ErrorPage" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
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
									<td class="PageNavigation" width="150">&nbsp;</td>
									<td class="PageNavigation">&nbsp;&nbsp;&nbsp;</td>
								</TR>
								<tr>
									<TD vAlign="top" width="150">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="1" cellPadding="0" width="150" border="1">
											<TR>
												<TD class="TextHeader" width="150">Fehler</TD>
											</TR>
										</TABLE>
									</TD>
									<td vAlign="top">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td align="left" height="25">
												</td>
											</tr>
											<TR>
												<TD align="left" height="74">
													<asp:Label id="lblErrorMessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label><BR>
													<BR>
													<span lang="de">Die Administration Ihres Portals wurde soeben über das Problem 
                                                    informiert </span>!<BR>
													<BR>
													<asp:Label id="lblCName" runat="server" Font-Bold="True">DAD Deutscher Auto Dienst GmbH</asp:Label><BR>
													<asp:Label id="lblCAddress" runat="server">Bogenstraße 26, 22926 Ahrensburg<br>Hotline: +49 04102 804-109</asp:Label><BR>
													<asp:panel id="pnlLinks" runat="server">
														<asp:HyperLink id="lnkMail" runat="server" NavigateUrl="mailto:info@dad.de">info@dad.de</asp:HyperLink>
														<BR>
														<asp:HyperLink id="lnkWeb" runat="server" NavigateUrl="http://www.dad.de">www.dad.de</asp:HyperLink>
													</asp:panel>
												</TD>
											</TR>
											<TR>
												<td align="left" height="25">
												</td>
											</TR>
										</TABLE>
									</td>
								</tr>
								<tr>
									<td></td>
									<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
								</tr>
								<tr>
									<td></td>
									<td><!--#include File="../PageElements/Footer.html" --></td>
								</tr>
							</TABLE>
						</td>
					</tr>
				</TBODY></table>
		</form>
	</body>
</HTML>
