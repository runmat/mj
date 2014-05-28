<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change02.aspx.vb" Inherits="AppEC.Change02"%>
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
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Datenselektion)</asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;Bitte die Auswahlkriterien eingeben.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="100"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE class="BorderLeftBottom" id="Table1" cellSpacing="0" cellPadding="3" bgColor="white" border="0">
													<TR>
														<TD vAlign="center" colSpan="4"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;</TD>
													</TR>
													<TR>
														<TD class="" vAlign="center"></TD>
														<TD class="" vAlign="center">
															<P align="right">Hersteller:
															</P>
														</TD>
														<TD class="" vAlign="center"><asp:dropdownlist id="ddlHersteller" runat="server"></asp:dropdownlist></TD>
														<TD class="" vAlign="center">&nbsp;</TD>
													</TR>
													<TR>
														<TD class="" vAlign="center"></TD>
														<TD class="" vAlign="center">
															<P align="right">Model-Id:
															</P>
														</TD>
														<TD class="" vAlign="center"><asp:textbox id="txtModelId" runat="server" MaxLength="9"></asp:textbox></TD>
														<TD class="" vAlign="center"></TD>
													</TR>
													<TR>
														<TD class="" vAlign="center"></TD>
														<TD class="" vAlign="center">
															<P align="right">Batch-Id:
															</P>
														</TD>
														<TD class="" vAlign="center"><asp:textbox id="txtBatchid" runat="server" MaxLength="17"></asp:textbox></TD>
														<TD class="" vAlign="center"></TD>
													</TR>
													<TR>
														<TD vAlign="center"></TD>
														<TD class="" vAlign="center" noWrap>
															<P align="right">Unit Nummer:</P>
														</TD>
														<TD vAlign="center"><asp:textbox id="txtUnitNr" runat="server" MaxLength="17"></asp:textbox></TD>
														<TD vAlign="center"></TD>
													</TR>
													<TR>
														<TD vAlign="center"></TD>
														<TD class="" vAlign="center" noWrap>
															<P align="right">Vorselektion:</P>
														</TD>
														<TD vAlign="center"><asp:radiobuttonlist id="rblBatch" Runat="server">
																<asp:ListItem Value="alle">alle Unit Nummern/Batche *</asp:ListItem>
																<asp:ListItem Value="zugeteilte">zugeteilte UnitNummern/Batche *</asp:ListItem>
																<asp:ListItem Value="sperrbare/entsperrbare" Selected="True">sperrbare/entsperrbare Unit Nummern/Batche *</asp:ListItem>
																<asp:ListItem Value="sperrbare">sperrbare UnitNummern/Batche</asp:ListItem>
																<asp:ListItem Value="entsperrbare">entsperrbare UnitNummern/Batche</asp:ListItem>
															</asp:radiobuttonlist></TD>
														<TD vAlign="center"></TD>
													</TR>
													<TR>
														<TD class="" vAlign="center" noWrap></TD>
														<TD class="" vAlign="center" noWrap>
															<P align="right">
																Unit Nummern:</P>
														</TD>
														<TD class="" vAlign="center"><asp:checkbox id="cbxShowUnitNummern" runat="server"></asp:checkbox></TD>
														<TD class="" vAlign="center"></TD>
													</TR>
													<TR>
														<TD vAlign="center"></TD>
														<TD vAlign="center">&nbsp;</TD>
														<TD vAlign="center">* erwartet mindestens ein Selektionskriterum</TD>
														<TD vAlign="center"></TD>
													</TR>
													<TR id="trSelectDropdown" runat="server">
														<TD class="" vAlign="center"></TD>
														<TD class="" vAlign="center"></TD>
														<TD class="" vAlign="center"></TD>
														<TD class="" vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton" Width="100%"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
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
