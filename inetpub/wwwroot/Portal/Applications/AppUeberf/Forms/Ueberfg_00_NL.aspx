<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Ueberfg_00_NL.aspx.vb" Inherits="AppUeberf.Ueberfg_00_NL" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
				<TBODY>
					<TR>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</TR>
					<TR>
						<TD>
							<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<TD class="PageNavigation"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
									</TR>
									<TR>
										<TD>&nbsp;</TD>
									</TR>
									<TR>
										<TD class="TaskTitle">&nbsp;
											<asp:label id="lblNoData" runat="server" Font-Bold="True"></asp:label></TD>
									</TR>
									<TR>
										<TD vAlign="top">
											<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TBODY>
													<TR>
														<TD vAlign="top" align="left">
															<table>
																<TBODY>
																	<tr>
																		<TD class="TextLarge" vAlign="top" noWrap width="120"><asp:linkbutton id="cmdCreate" tabIndex="12" runat="server" CssClass="StandardButtonTable" Width="100px"> •&nbsp;Weiter</asp:linkbutton>
																			</TD>
																		<td vAlign="top">
																			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
																				<TBODY>
																					<tr id="trKUNNR" runat="server">
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lbl_KundenNr" runat="server">Kunde</asp:label></TD>
																						<TD><asp:dropdownlist id="ddlKunde" runat="server" Width="485px"></asp:dropdownlist></TD>
																					</tr>
																				</TBODY></TABLE>
																			</td>
																	</tr>
																	<tr>
																		<TD class="TextLarge" vAlign="top" noWrap width="120"><asp:linkbutton id="cmdBack" 
                                                                                tabIndex="12" runat="server" CssClass="StandardButtonTable" Width="100px"> •&nbsp;Zurück</asp:linkbutton>
																			</TD>
																		<td vAlign="top">
																			&nbsp;</td>
																	</tr>
																</TBODY></table>
														</TD>
													</TR>
												</TBODY></TABLE>
											</TD>
									</TR>
									<TR>
										<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
									</TR>
									<TR>
										<TD vAlign="top"><!--#include File="../../../PageElements/Footer.html" --></TD>
									</TR>
								</TBODY></TABLE>
						</TD>
					</TR>
				</TBODY></TABLE>
		</form>
		</TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></FORM>
	</body>
</HTML>
