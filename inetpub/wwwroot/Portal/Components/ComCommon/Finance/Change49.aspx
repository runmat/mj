<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change49.aspx.vb" Inherits="CKG.Components.ComCommon.Change49" %>
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
			<table width="100%" align="center">
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<td class="PageNavigation" noWrap colSpan="2">
											<asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> - Fahrzeugsuche</asp:label>
										</td>
									</TR>
									<TR>
										<TD class="TaskTitle" noWrap colSpan="2">&nbsp;</TD>
									</TR>
									<tr>
										<TD vAlign="top">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="121" border="0">
												<TR>
													<TD class="" width="57">
														<asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TBODY>
													<tr>
														<td vAlign="top" align="left">
															<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white">
																<TBODY>
																	<TR id="tr_Kennzeichen">
																		<TD class="TextLarge" width="150"><asp:Label ID="lbl_Kennzeichen" Runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;</TD>
																		<TD class="TextLarge"><asp:textbox id="txtKennzeichen" runat="server" Width="200px" MaxLength="35"></asp:textbox></TD>
																	</TR>
																	<TR id="tr_Kontonummer">
																		<TD class="StandardTableAlternate" width="150"><asp:Label ID="lbl_Kontonummer" Runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;</TD>
																		<TD class="StandardTableAlternate"><asp:textbox id="txtVertragsnummer" runat="server" Width="200px" MaxLength="35"></asp:textbox></TD>
																	</TR>
																	<TR id="tr_Fahrgestellnummer">
																		<TD class="TextLarge" width="150">
																			<asp:Label ID="lbl_Fahrgestellnummer" Runat="server"></asp:Label></TD>
																		<TD class="TextLarge">
																			<asp:textbox id="txtFahrgestellnummer" runat="server" Width="200px" MaxLength="35"></asp:textbox></TD>
																	</TR>
																	<TR id="tr_zb2Nummer">
																		<TD class="StandardTableAlternate" width="150">
																			<asp:Label ID="lbl_zb2Nummer" Runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;</TD>
																		<TD class="StandardTableAlternate">
																			<asp:textbox id="txtZB2Nummer" runat="server" Width="200px" MaxLength="35"></asp:textbox></TD>
																	</TR>
																</TBODY>
															</TABLE>
															<br>
														</td>
													</tr>
												</TBODY>
											</TABLE>
										</td>
									</tr>
									<TR>
										<TD vAlign="top" width="120">&nbsp;</TD>
										<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
									</TR>
									<TR>
										<TD vAlign="top" width="120">&nbsp;</TD>
										<td><!--#include File="../../../PageElements/Footer.html" --></td>
									</TR>
								</TBODY>
							</TABLE>
						</td>
					</tr>
				</TBODY>
			</table>
		</form>
		<script language="JavaScript">
<!-- //
 window.document.Form1.txtVertragsnummer.focus();
//-->
		</script>
		</TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></FORM>
	</body>
</HTML>
