<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change205.aspx.vb" Inherits="AppSIXTL.Change205" %>
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
												<TD vAlign="center" width="120"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
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
														<TR>
															<TD class="TextLarge" vAlign="top" noWrap align="left">
																<TABLE id="Table8" cellSpacing="1" cellPadding="1" border="0">
																	<TR>
																		<TD noWrap><asp:label id="Label1" runat="server">Leasingvertrags-Nr.</asp:label></TD>
																	</TR>
																</TABLE>
															</TD>
															<TD class="TextLarge"><FONT size="1">
																	<TABLE id="Table7" cellSpacing="1" cellPadding="1" border="0">
																		<TR>
																			<TD><asp:textbox id="txtOrdernummer" runat="server" MaxLength="10" Width="250px"></asp:textbox>&nbsp;<FONT size="1">(1234567)</FONT></TD>
																		</TR>
																	</TABLE>
																</FONT>
															</TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" noWrap align="left">
																<TABLE id="Table9" cellSpacing="1" cellPadding="1" border="0">
																	<TR>
																		<TD><asp:label id="Label2" runat="server">Kfz-Kennzeichen*</asp:label></TD>
																	</TR>
																</TABLE>
															</TD>
															<TD class="" height="37"><FONT size="1">
																	<TABLE id="Table5" cellSpacing="1" cellPadding="1" border="0">
																		<TR>
																			<TD><asp:textbox id="txtAmtlKennzeichen" runat="server" MaxLength="9" Width="250px"></asp:textbox>&nbsp;<FONT size="1">(XX-Y1234)</FONT></TD>
																		</TR>
																	</TABLE>
																</FONT>
															</TD>
														</TR>
														<TR>
															<TD class="TextLarge" noWrap align="left"></TD>
															<TD>*<FONT size="1">Eingabe von nachgestelltem Platzhalter möglich. Mindestens Kreis 
																	und ein Buchstabe (z.B. XX-Y*)</FONT></TD>
														</TR>
														<TR>
															<TD class="" noWrap align="left" colSpan="2"><INPUT id="cbxDatei" style="DISPLAY: none" type="checkbox" name="cbxDatei" runat="server"></TD>
														</TR>
														<TR>
															<TD class="" vAlign="top" noWrap align="left"><A class="StandardButtonTable" href="javascript:showhide()">•&nbsp;Dateiauswahl</A>&nbsp; 
																</STRONG><A href="javascript:openinfo('Info01.htm');"><IMG src="/Portal/Images/fragezeichen.gif" border="0"></A></TD>
															</STRONG>
															<TD class="" vAlign="top" align="left" width="100%">
																<TABLE id="Table3" cellSpacing="1" cellPadding="1" border="0">
																	<TR id="trDateiauswahl" style="DISPLAY: none">
																		<TD><INPUT class="InfoBoxFlat" id="upFile" type="file" size="40" name="File1" runat="server"></TD>
																		<TD><asp:linkbutton id="cmdContinue" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Weiter&nbsp;&#187;</asp:linkbutton></TD>
																	</TR>
																</TABLE>
																<asp:label id="lblExcelfile" runat="server"></asp:label></TD>
														</TR>
														<TR>
															<TD class="" noWrap align="left"></TD>
															<TD class="" width="100%"></TD>
														</TR>
													</TABLE>
												</td>
											</tr>
										</TABLE>
									</TD>
					</td>
				</tr>
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
								function showhide()
								{
									o = document.getElementById("trDateiauswahl").style;
									if (o.display != "none"){
											o.display = "none";
											document.forms[0].txtOrdernummer.disabled=false;
											document.forms[0].txtAmtlKennzeichen.disabled=false;
											document.forms[0].cbxDatei.checked=false;								
											window.document.Form1.txtOrdernummer.focus();
										} else {											
											o.display = "";
											document.forms[0].txtOrdernummer.disabled=true;
											document.forms[0].txtAmtlKennzeichen.disabled=true;
											document.forms[0].cbxDatei.checked=true;
											document.forms[0].upFile.focus();													
									}					
								}							
								function openinfo (url) {
										fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
										fenster.focus();
								}
						-->
				</SCRIPT>
			</table>
			</TD></TR></TBODY></TABLE>
		</form>
		<asp:literal id="Literal1" runat="server"></asp:literal>
	</body>
</HTML>
