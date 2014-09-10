<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report41.aspx.vb" Inherits="AppSIXT.Report41" %>
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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server" Visible="False"> (Fahrzeugsuche)</asp:label></td>
								</TR>
								<tr>
									<TD vAlign="top" width="120" height="192">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
											<TR>
												<TD class="TaskTitle" width="120">&nbsp;</TD>
											</TR>
											<TR id="trcmdUpload" runat="server">
												<TD vAlign="center" width="120"></TD>
											</TR>
											<TR id="trcmdSearch" runat="server">
												<TD vAlign="center" width="120"></TD>
											</TR>
										</TABLE>
									</TD>
									<TD vAlign="top" align="right">
										<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" vAlign="top" align="right">&nbsp;</TD>
											</TR>
										</TABLE>
										<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
											<tr>
												<td vAlign="top" align="left">
													<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
														<TBODY>
															<TR>
																<TD vAlign="top" align="left" width="100%">Bitte Excel-Datei mit Fahrgestellnummern 
																	hochladen.</TD>
															</TR>
															<TR>
																</STRONG>
																<TD class="" vAlign="top" align="left" width="100%">
																	<TABLE id="Table3" cellSpacing="1" cellPadding="1" border="0">
																		<TR id="trDateiauswahl">
																			<TD><INPUT class="InfoBoxFlat" id="upFile" type="file" size="40" name="File1" runat="server"></TD>
																			<TD><asp:linkbutton id="cmdContinue" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Weiter&nbsp;&#187;</asp:linkbutton></TD>
																		</TR>
																	</TABLE>
																	<asp:label id="lblExcelfile" runat="server"></asp:label></TD>
															</TR>
															<TR>
																<TD class="" width="100%">
																	<P>Folgendes Format wird benötigt:&nbsp;
																		<TABLE id="Table4" height="123" cellSpacing="1" cellPadding="1" width="328" border="1">
																			<TBODY>
																				<TR>
																					<TD bgColor="#c0c0c0" colSpan="1" rowSpan="1">
																						<P align="center"><STRONG></STRONG>&nbsp;</P>
																					</TD>
																					<TD align="middle" width="224" bgColor="#c0c0c0"><STRONG>A</STRONG></TD>
																					<TD align="middle" bgColor="#c0c0c0"><STRONG>B</STRONG></TD>
																				</TR>
																				<TR>
																					<TD bgColor="#c0c0c0">
																						<P align="center"><STRONG>1</STRONG></P>
																					</TD>
																					<TD width="224">Fahrgestellnummer</TD>
																					<TD>&nbsp;</TD>
																				</TR>
																				<TR>
																					<TD bgColor="#c0c0c0">
																						<P align="center"><STRONG>2</STRONG></P>
																					</TD>
																					<TD width="224">ABCDEF12345P123456</TD>
																</TD>
																<TD>&nbsp;</TD>
															</TR>
															<TR>
																<TD bgColor="#c0c0c0">
																	<P align="center"><STRONG>3</STRONG></P>
																</TD>
																<TD width="224">ABCDEF12345P123456</TD>
												</td>
												<TD>&nbsp;</TD>
											</tr>
											<TR>
												<TD bgColor="#c0c0c0">
													<P align="center"><STRONG>4</STRONG></P>
												</TD>
												<TD width="224">...</TD>
												<TD>&nbsp;</TD>
											</TR>
										</TABLE>
										</P>
									</TD>
								</tr>
							</TBODY></TABLE>
					</td>
				</tr>
			</table>
			</TD></TD></TR>
			<TR>
				<TD vAlign="top" width="120">&nbsp;</TD>
				<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top" width="120">&nbsp;</TD>
				<TD align="right"><!--#include File="../../../PageElements/Footer.html" --></TD>
			</TR>
			<SCRIPT language="JavaScript">										
						<!--

						-->
			</SCRIPT>
			</TBODY></TABLE></TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
