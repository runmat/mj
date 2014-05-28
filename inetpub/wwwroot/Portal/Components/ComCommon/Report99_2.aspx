<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report99_2.aspx.vb" Inherits="CKG.Components.ComCommon.Report99_2" %>
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
							<TBODY>
								<TR>
									<TD class="PageNavigation"><asp:label id="lblHead" runat="server"></asp:label></TD>
								</TR>
								<TR>
									<TD vAlign="top">
										<DIV align="center">
											<table id="tblContent" style="WIDTH: 100%; BORDER-COLLAPSE: collapse; BACKGROUND-COLOR: white" cellSpacing="0" align="center" border="1" runat="server">
												<tr nowrap="nowrap">
													<td width="150">&nbsp;</td>
													<td align="left">&nbsp;</td>
												</tr>
												<tr nowrap="nowrap">
													<td width="150">&nbsp;</td>
													<TD align="left"><asp:hyperlink id="lnkBack" runat="server" NavigateUrl="Report02.aspx">zurück zur Übersichtsseite</asp:hyperlink></TD>
												</tr>
												<tr nowrap="nowrap">
													<td width="150">&nbsp;</td>
													<td align="left">&nbsp;</td>
												</tr>
												<tr class="GridTableHead" nowrap="nowrap">
													<td noWrap align="middle" width="200">Bundesland</td>
													<td noWrap align="left">&nbsp;Formulare für Lastschrifteinzug</td>
												</tr>
											</table>
										</DIV>
										<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
											<TR>
												<TD><asp:label id="lblContent" runat="server" BorderWidth="0px">Label</asp:label></TD>
											</TR>
										</TABLE>
									</TD>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
				</TR>
				<TR>
					<TD><!--#include File="../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
