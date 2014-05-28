<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report01.aspx.vb" Inherits="AppVFS.Report01"%>
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
									<TD class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
								</TR>
								<TR>
					</TD>
					<TD vAlign="top" width="100%">
						<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;Kennzeichenbestand:</TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD class="PageNavigation" vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>&nbsp;</TD>
							</TR>
						</TABLE>
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" align="middle" width="100%">
									<TABLE class="BorderLeftBottom" id="Table1" height="142" cellSpacing="0" cellPadding="5" width="473" bgColor="white" border="0">
										<TR>
											<TD class="TextLarge" vAlign="center" noWrap width="173">Versicherungsjahr</TD>
											<TD class="" vAlign="center" noWrap width="541">
												<asp:dropdownlist id="drpVJahr" runat="server" Width="98px"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="center" noWrap width="173"><span lang="de">VD-Bezirk</span>:</TD>
											<TD vAlign="center" noWrap width="541"><asp:textbox id="txtOrgNr" runat="server" Width="129px"></asp:textbox>
												<asp:Label id="lblPlatzhaltersuche" runat="server" Font-Size="Smaller" Visible="False">*(mit Platzhaltersuche)</asp:Label></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="center" noWrap width="173"></TD>
											<TD vAlign="center" noWrap width="541"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" noWrap width="173"></TD>
											<TD class="TextLarge" vAlign="center" width="541">
												<P align="left">
													<TABLE class="TableBackGround" id="Table2" cellSpacing="0" cellPadding="0" border="0">
														<TR>
															<TD></TD>
														</TR>
													</TABLE>
												</P>
											</TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="center" noWrap width="173"></TD>
											<TD class="" vAlign="center" width="541"><P align="right">
                                                <asp:linkbutton id="btnConfirm" runat="server" CssClass="StandardButton" 
                                                    Width="150px">Weiter&nbsp;&#187;</asp:linkbutton></P>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
						<P align="right">&nbsp;</P>
					</TD>
				</TR>
				<TR>
					<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
