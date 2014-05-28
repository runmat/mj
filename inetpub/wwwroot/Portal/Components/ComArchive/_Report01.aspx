<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report01.aspx.vb" Inherits="CKG.Components.ComArchive._Report01" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
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
		<DIV align="left">
			<form id="Form1" method="post" runat="server">
				<table width="300" align="left">
					<tr>
						<td><uc1:header id="ucHeader" runat="server" Visible="False"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" align="right" colSpan="2" height="2"></td>
								</TR>
								<TR>
									<TD class="StandardTableButtonFrame" vAlign="top"></TD>
									<TD vAlign="top">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()">Fenster schließen</asp:hyperlink></TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge">
													<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
														<TR>
															<TD vAlign="top" align="left" colSpan="2"><TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0" runat="server">
																	<TBODY>
																		<TR>
																			<TD vAlign="top" align="left" width="100%" bgColor="#ffffff">
																				<TABLE class="TableBackGround" id="Table4" borderColor="#cccccc" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffffff" border="0">
																					<TR>
																						<TD class="GridTableHead" height="30" rowSpan="1"><STRONG>Detailinformationen</STRONG></TD>
																						<TD class="GridTableHead" id="tdSearch" bgColor="#ffffff" runat="server"></TD>
																					</TR>
																					<TR>
																						<TD bgColor="#ffffff"><STRONG>&nbsp;</STRONG></TD>
																						<TD bgColor="#ffffff"></TD>
																					</TR>
																					<TR>
																						<TD bgColor="#ffffff">Länge in Bytes:</TD>
																						<TD bgColor="#ffffff"><asp:textbox id="txtLAenge" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD bgColor="#ffffff">Erstellungsdatum:</TD>
																						<TD bgColor="#ffffff"><asp:textbox id="txtErstellDatum" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" bgColor="#ffffff" colSpan="1" rowSpan="1">Letzte Änderung:
																						</TD>
																						<TD vAlign="top" width="100%" bgColor="#ffffff"><asp:textbox id="txtAenderDatum" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" bgColor="#ffffff">Titel:</TD>
																						<TD vAlign="top" width="100%" bgColor="#ffffff"><asp:textbox id="txtTitel" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" noWrap bgColor="#ffffff">Anzahl Felder gesamt:</TD>
																						<TD vAlign="top" width="100%" bgColor="#ffffff"><asp:textbox id="txtFelderGesamt" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" bgColor="#ffffff">Anzahl Textfelder:</TD>
																						<TD vAlign="top" width="100%" bgColor="#ffffff"><asp:textbox id="txtFelderText" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" bgColor="#ffffff">Anzahl Bildfelder:</TD>
																						<TD vAlign="top" width="100%" bgColor="#ffffff"><asp:textbox id="txtFelderBild" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" bgColor="#ffffff">Anzahl Binärfelder:</TD>
																						<TD vAlign="top" width="100%" bgColor="#ffffff"><asp:textbox id="txtFelderBlob" runat="server" Font-Bold="True" BackColor="Transparent" BorderWidth="0px"></asp:textbox></TD>
																					</TR>
																				</TABLE>
																			</TD>
																		</TR>
																	</TBODY></TABLE>
															</TD>
														</TR>
														<tr>
															<td><asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></td>
														</tr>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</td>
					</tr>
				</table>
			</form>
		</DIV>
	</body>
</HTML>
