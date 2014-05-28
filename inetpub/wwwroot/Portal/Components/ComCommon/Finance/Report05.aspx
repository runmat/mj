<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report05.aspx.vb" Inherits="CKG.Components.ComCommon.Report05"%>
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
									<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
								</TR>
								<TR>
									<TD class="TaskTitle" vAlign="top" colSpan="2">&nbsp;</TD>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="50">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
							<TR id="trCreate" runat="server">
								<TD vAlign="middle" width="150">
									<P><asp:linkbutton id="lb_Suche" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suche</asp:linkbutton><br>
									</P>
								</TD>
							</TR>
						</TABLE>
					</TD>
					<TD vAlign="top">
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<TD vAlign="top" align="left">
										<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
											<tr>
												<td class="TextLarge" width="137" height="33">
													<P align="left"><asp:Label Runat="server" ID="lbl_Haendlernummer"></asp:Label>
													</P>
												</td>
												<td height="33"><asp:textbox id="txt_Personennummer" runat="server" Width="280px"></asp:textbox></td>
											</tr>
											<tr>
												<td class="StandardTableAlternate" width="137" height="33">
													<P align="left"><asp:Label Runat="server" ID="lbl_Haendler">lbl_Haendler</asp:Label>
													</P>
												</td>
												<td class="StandardTableAlternate" height="33"><asp:DropDownList ID="ddl_Haendler" Runat="server"></asp:DropDownList></td>
											</tr>
										</TABLE>
										&nbsp;
									</TD>
								</TR>
							</TBODY></TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="50">&nbsp;</TD>
					<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="top" width="50">&nbsp;</TD>
					<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
