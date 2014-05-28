<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change06.aspx.vb" Inherits="AppPorsche.Change06"%>
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
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Selektionskriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;Bitte Händler auswählen.</TD>
							</TR>
							<TR>
								<TD vAlign="top" noWrap width="100"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" width="100">
												<TABLE class="BorderLeftBottom" id="Table1" cellSpacing="0" cellPadding="2" width="600" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap colSpan="5"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>&nbsp;&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" width="10" colSpan="1" rowSpan="1"></TD>
														<TD class="TextLarge" vAlign="center" noWrap colSpan="2"></TD>
														<TD class="TextLarge" vAlign="center" noWrap></TD>
														<TD class="TextLarge" vAlign="center" noWrap width="10"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center"></TD>
														<TD class="TextLarge" vAlign="center" noWrap colSpan="2"><asp:label id="lblTitel" runat="server">Aktueller Händlerbestand:</asp:label></TD>
														<TD class="TextLarge" vAlign="center" noWrap></TD>
														<TD class="TextLarge" vAlign="center" noWrap></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center"></TD>
														<TD class="TextLarge" vAlign="center" colSpan="3"><asp:listbox id="lstHaendler" runat="server" Width="100%" Height="100px" BackColor="White"></asp:listbox></TD>
														<TD class="TextLarge" vAlign="center"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center"></TD>
														<TD class="TextLarge" vAlign="center" noWrap colSpan="2"><asp:checkbox id="cbxAlle" runat="server" Text="Alle Händler anzeigen" Visible="False"></asp:checkbox></TD>
														<TD class="TextLarge" vAlign="center" width="11"></TD>
														<TD class="TextLarge" vAlign="center" width="11"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center"></TD>
														<TD class="TextLarge" vAlign="center" width="11" colSpan="2"></TD>
														<TD class="TextLarge" vAlign="center">
															<P align="right"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton" Width="150px"> &#149;&nbsp;Weiter</asp:linkbutton></P>
														</TD>
														<TD class="TextLarge" vAlign="center"></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top" width="100"></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD width="100%"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
