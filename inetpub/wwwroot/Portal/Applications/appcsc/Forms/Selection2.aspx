<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Selection2.aspx.vb" Inherits="AppCSC.Selection2" %>
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
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" noWrap colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>&nbsp;&nbsp;
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120"></TD>
								<TD vAlign="top">
									<asp:hyperlink id="lnkAdmin" runat="server" NavigateUrl="../Admin/AdminMenu.aspx" CssClass="PageNavigation" Visible="False">Administration</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD vAlign="top">
								</TD>
								<TD vAlign="top">
									<TABLE id="grdLinks" style="WIDTH: 100%; BORDER-COLLAPSE: collapse; BACKGROUND-COLOR: white" cellSpacing="0" border="0">
										<TBODY>
											<TR class="TaskTitle" style="HEIGHT: 26px">
												<TD>&nbsp;&nbsp;Dateneingabe</TD>
												<TD>&nbsp;</TD>
											</TR>
											<TR>
												<TD class="MainmenuItem" noWrap>&nbsp;
													<asp:linkbutton id="btnChange09" runat="server">Anforderung von Briefkopien</asp:linkbutton></TD>
												<TD class="MainmenuItemComment">&nbsp;&nbsp;
													<SPAN class="MainmenuItemComment" id="lblChange09"></SPAN></TD>
											</TR>
										</TBODY></TABLE>
									<TABLE id="grdReportLinks" style="WIDTH: 100%; BORDER-COLLAPSE: collapse; BACKGROUND-COLOR: white" cellSpacing="0" border="0">
										<TBODY>
											<TR class="TaskTitle" style="HEIGHT: 26px">
												<TD>&nbsp;&nbsp;Reports</TD>
												<TD>&nbsp;</TD>
											</TR>
											<TR>
												<TD class="MainmenuItem" noWrap>&nbsp;
													<asp:linkbutton id="btnReport41" runat="server">Report Inaktive Verträge</asp:linkbutton></TD>
												<TD class="MainmenuItemComment">&nbsp;&nbsp;
													<SPAN class="MainmenuItemComment" id="lblReport41"></SPAN></TD>
											</TR>
										</TBODY></TABLE>
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
