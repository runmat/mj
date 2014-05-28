<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report20.aspx.vb" Inherits="AppEC.Report20"%>
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
								<TR>
									<TD class="TaskTitle" colSpan="2">Bitte wählen Sie eine lokal gespeicherte 
										Excel-Datei zur Übertragung aus.</TD>
								</TR>
								<tr>
									<TD vAlign="top" width="50" height="192">
									</TD>
									<TD vAlign="top" align="right">
										<DIV align="left">
											<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server" align="left">
												<tr>
													<td vAlign="top" align="left">
														<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
															<TR>
																<TD vAlign="top" align="left" width="100%">&nbsp;</TD>
															</TR>
															<TR>
																</STRONG>
																<TD class="" vAlign="top" align="left" width="100%">
																	<DIV align="center">
																		<TABLE id="Table3" cellSpacing="1" cellPadding="1" border="0" align="center">
																			<TR id="trDateiauswahl">
																				<TD><INPUT class="InfoBoxFlat" id="upFile" type="file" size="40" name="File1" runat="server">&nbsp;<A href="javascript:openinfo('Info02.htm');"><IMG src="/PortalORM/Images/fragezeichen.gif" border="0"></A>&nbsp;</TD>
																				<TD><asp:linkbutton id="cmdContinue" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Weiter&nbsp;&#187;</asp:linkbutton></TD>
																			</TR>
																		</TABLE>
																	</DIV>
																</TD>
															</TR>
														</TABLE>
														<P align="center">&nbsp;</P>
													</td>
												</tr>
											</TABLE>
										</DIV>
									</TD>
					</td>
				</tr>
				<TR>
					<TD vAlign="top" width="50">&nbsp;</TD>
					<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
						<asp:label id="lblExcelfile" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="top" width="50">&nbsp;</TD>
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
