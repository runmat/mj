<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change05.aspx.vb" Inherits="AppEC.Change05" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<TD class="PageNavigation" colSpan="2" height="19">
										<asp:Label id="lblHead" runat="server"></asp:Label>
										<asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label>
									</TD>
								</TR>
								<TR>
					</TD>
					<TD vAlign="top" width="100%">
						<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;Bitte PDI - Vorauswahl 
									treffen.</TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD class="PageNavigation" vAlign="top">
									<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;</TD>
							</TR>
						</TABLE>
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" align="middle" width="100%">
									<TABLE class="BorderLeftBottom" id="Table1" cellSpacing="0" cellPadding="5" width="400" bgColor="white" border="0">
										<TR>
											<TD class="TextLarge" vAlign="center" noWrap>PDI:</TD>
											<TD class="" vAlign="center" width="100%" noWrap>
												<asp:DropDownList id="ddlPDIs" runat="server" CssClass="DropDownStyle"></asp:DropDownList>&nbsp;
												<asp:CheckBox id="cbxAlle" runat="server" Text="Alle"></asp:CheckBox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="center" noWrap></TD>
											<TD class="" vAlign="center" noWrap width="100%">
												<asp:CheckBox id="cbxPDI" runat="server" Text="Nur Fahrzeuge mit PDI Bereitmeldung" Checked="True"></asp:CheckBox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" noWrap>Aktion:</TD>
											<TD class="TextLarge" vAlign="center" width="100%">
												<P align="left">
													<TABLE class="TableBackGround" id="Table2" cellSpacing="0" cellPadding="0" border="0">
														<TR>
															<TD>
																<asp:RadioButtonList id="rbAktion" runat="server">
																	<asp:ListItem Value="Zulassen" Selected="True">Zulassen</asp:ListItem>
																	<asp:ListItem Value="Sperren">Sperren</asp:ListItem>
																	<asp:ListItem Value="Entsperren">Entsperren</asp:ListItem>
																	<asp:ListItem Value="Verschieben">Verschieben</asp:ListItem>
																</asp:RadioButtonList></TD>
														</TR>
													</TABLE>
												</P>
											</TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="center" noWrap></TD>
											<TD class="" vAlign="center" width="100%">
												<P align="right">
													<asp:linkbutton id="btnConfirm" runat="server" CssClass="StandardButtonTable" Width="150px">Weiter&nbsp;&#187;</asp:linkbutton></P>
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
			</TD></TR></TBODY></TABLE>
		</form>
	</body>
</HTML>
