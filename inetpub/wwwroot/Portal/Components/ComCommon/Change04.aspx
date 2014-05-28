<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="CKG.Components.ComCommon.Change04" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
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
									<TD class="TaskTitle" vAlign="top" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="120">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
							<TR id="trCreate" runat="server">
								<TD vAlign="center" width="150">
									<P><asp:linkbutton id="lb_Suche" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suche</asp:linkbutton><br>
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
												<td class="TextLarge" width="162">
													<P align="right">Kundennummer:&nbsp;
													</P>
												</td>
												<td class="TextLarge"><asp:textbox id="txtSucheKundennr" runat="server" 
                                                        Width="155px"></asp:textbox></td>
										</tr>
																
										
										</TABLE>
										<TABLE id="TableXX" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR id="TR1" runat="server">
												<td><asp:linkbutton id="lb_Neu" runat="server" CssClass="StandardButton" 
                                                        Visible="False">Neu Anlegen</asp:linkbutton></td>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TBODY></TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="150">&nbsp;</TD>
					<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="top">&nbsp;</TD>
					<TD><!--#include File="../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>

