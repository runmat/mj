<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report00.aspx.vb" Inherits="AppDCL._Report00" %>
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
					<td height="25"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Abfragekriterien)</asp:label><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx" CssClass="PageNavigation">Abfragekriterien</asp:hyperlink></td>
							</TR>
							<tr>
								<TD class="StandardTableButtonFrame" vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">
												<asp:label id="lblFahrernrtext" runat="server" Font-Bold="True">Bitte Fahrer-Nummer eingeben:&nbsp;</asp:label>
												<asp:textbox id="txtFahrernr" tabIndex="1" runat="server"></asp:textbox>&nbsp;
												<asp:linkbutton id="btnFahrernr" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Aufträge suchen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2">
														</TD>
													</TR>
												</TABLE>
												<asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
