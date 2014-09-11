<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change06.aspx.vb" Inherits="AppSIXT.Change06" %>
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
					<td><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" ImageUrl="/Portal/Images/empty.gif"></asp:imagebutton></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (PDI - Suche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">Bitte Suchkriterien und Aufgabe wählen.</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<tr>
											<td vAlign="top" align="left">
												<TABLE class="TableColors" id="Table1" cellSpacing="0" cellPadding="2" width="50%" bgColor="white">
													<TR class="BannerBackground_1">
														<TD class="TextLarge" noWrap><STRONG>Suchkriterien</STRONG></TD>
														<TD class="TextLarge"></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR id="trPDI" runat="server">
														<TD id="tdPDI" noWrap runat="server">PDI-Nummer:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:radiobutton id="rbSuchePDI" runat="server" GroupName="rbSearch"></asp:radiobutton></TD>
														<TD class="TextLarge">
															<TABLE class="TableColors" id="Table5" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server">
																<TR>
																	<TD noWrap width="100%"><asp:textbox id="txtPDINummer" runat="server" BackColor="Transparent" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Width="100px" MaxLength="10"></asp:textbox></TD>
																	<TD>Farbe:</TD>
																	<TD>
																		<P align="center"><INPUT id="idWeiss" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idGelb" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idOrange" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idRot" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idViolett" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idBlau" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idGruen" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idGrau" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idBraun" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD>
																		<P align="center"><INPUT id="idSchwarz" type="radio" name="Farbe" runat="server"></P>
																	</TD>
																	<TD><INPUT id="idAlle" type="radio" name="Farbe" runat="server"></TD>
																</TR>
																<TR>
																	<TD noWrap><FONT size="1"><FONT face="Arial"><FONT size="2"><FONT size="1"></FONT></FONT></FONT></FONT></TD>
																	<TD><FONT face="Arial" size="1"></FONT></TD>
																	<TD>
																		<P align="center"><FONT face="Arial" size="1">0</FONT></P>
																	</TD>
																	<TD>
																		<P align="center"><FONT face="Arial" size="1">1</FONT></P>
																	</TD>
																	<TD>
																		<P align="center"><FONT face="Arial" size="1">2</FONT></P>
																	</TD>
																	<TD colSpan="1" rowSpan="1">
																		<P align="center"><FONT face="Arial" size="1">3</FONT></P>
																	</TD>
																	<TD colSpan="1" rowSpan="1">
																		<P align="center"><FONT face="Arial" size="1">4</FONT></P>
																	</TD>
																	<TD colSpan="1" rowSpan="1">
																		<P align="center"><FONT face="Arial" size="1">5</FONT></P>
																	</TD>
																	<TD colSpan="1" rowSpan="1">
																		<P align="center"><FONT face="Arial" size="1">6</FONT></P>
																	</TD>
																	<TD colSpan="1" rowSpan="1">
																		<P align="center"><FONT face="Arial" size="1">7</FONT></P>
																	</TD>
																	<TD colSpan="1" rowSpan="1">
																		<P align="center"><FONT face="Arial" size="1">8</FONT></P>
																	</TD>
																	<TD colSpan="1" rowSpan="1">
																		<P align="center"><FONT face="Arial" size="1">9</FONT></P>
																	</TD>
																	<TD><FONT size="1"><FONT face="Arial"><FONT size="2"><FONT size="1">alle</FONT></FONT></FONT></FONT></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR id="trFahrgestell">
														<TD id="tdFahrg" vAlign="center" noWrap runat="server">Fahrgestell-Nr.</TD>
														<TD class="TextLarge"><asp:radiobutton id="rbSucheFahrg" runat="server" GroupName="rbSearch"></asp:radiobutton></TD>
														<TD class="TextLarge">
															<TABLE class="TableColors" id="Table7" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server">
																<TR id="trEinzel">
																	<TD height="30"><asp:radiobutton id="rbEinzel" runat="server" GroupName="rbFahrgestell"></asp:radiobutton></TD>
																	<TD height="30">Einzelauswahl</TD>
																	<TD width="100%" height="30"><asp:textbox id="txtEinzel" runat="server" BackColor="Transparent" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Width="100%" MaxLength="17"></asp:textbox></TD>
																</TR>
																<TR id="trMehrfach">
																	<TD height="30"><asp:radiobutton id="rbMehrfach" runat="server" GroupName="rbFahrgestell"></asp:radiobutton></TD>
																	<TD height="30"><A href="javascript:openinfo('info.htm');">Aus Datei</A></TD>
																	<TD vAlign="center" noWrap height="30"><INPUT class="InfoBoxFlat" id="txtMehrfach" type="file" name="File1" runat="server"></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR class="BannerBackground_1">
														<TD vAlign="center" width="100"><STRONG>Aufgabe</STRONG></TD>
														<TD></TD>
														<TD colSpan="1" rowSpan="1">
															<TABLE id="Table8" cellSpacing="1" cellPadding="1">
																<TR>
																	<TD noWrap><asp:radiobutton id="chkZulassen" runat="server" GroupName="grpTask"></asp:radiobutton>&nbsp;Zulassen</TD>
																	<TD noWrap><asp:radiobutton id="chkSperren" runat="server" GroupName="grpTask"></asp:radiobutton>&nbsp;Sperren</TD>
																	<TD noWrap><asp:radiobutton id="chkEntsperren" runat="server" GroupName="grpTask"></asp:radiobutton>&nbsp;Entsperren</TD>
																	<TD noWrap><asp:radiobutton id="chkVerschieben" runat="server" GroupName="grpTask"></asp:radiobutton>&nbsp;Verschieben</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD class="" vAlign="top" width="100" colSpan="1" rowSpan="1"></TD>
														<TD noWrap></TD>
														<TD class="" noWrap></TD>
													</TR>
													<TR>
														<TD vAlign="top" width="100">&nbsp;</TD>
														<TD noWrap></TD>
														<TD noWrap>
															<P align="right"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Suchen</asp:linkbutton></P>
														</TD>
													</TR>
												</TABLE>
												&nbsp;
											</td>
										</tr>
										<TR id="trNoMatch" runat="server">
											<TD vAlign="top" align="left">
												<table id="tblHeader" class="TableColorsTop" runat="server" width="300">
													<TR>
														<TD class="TaskTitle" noWrap>Die nacholgenden Fahrgestellnummern konnten<BR>
															im System nicht zugeordnet werden!</TD>
													</TR>
												</table>
												<DIV style="OVERFLOW: auto; WIDTH: 300px; HEIGHT: 200px" align="left" class="TableColorsLeftBottom">
													<TABLE class="" id="Table4" cellSpacing="1" cellPadding="1" bgColor="#ffffff" runat="server" width="100%">
														<TR>
															<TD>&nbsp;</TD>
														</TR>
														<TR>
															<TD>
																<P align="left">
																	<asp:label id="lblNoMatch" runat="server"></asp:label></P>
															</TD>
														</TR>
														<TR>
															<TD>&nbsp;</TD>
														</TR>
													</TABLE>
												</DIV>
												<br>
												<asp:linkbutton id="Linkbutton1" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Weiter</asp:linkbutton>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top">&nbsp;
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<SCRIPT language="JavaScript">										
						<!--	
								function switchInput(key)
								{	
									if (key == 0){
																		
										document.getElementById("rbSucheFahrg").style.background = "#ffffff";
										document.getElementById("rbSuchePDI").style.background = "#ff6600";
										document.getElementById("txtPDINummer").focus();										
									}
									if (key == 1){
														
										document.getElementById("rbSucheFahrg").style.background = "#ff6600";
										document.getElementById("rbSuchePDI").style.background = "#ffffff";
										
										if (document.getElementById("rbEinzel").checked == true){
											document.getElementById("rbEinzel").style.background = "#ff6600";										
											document.getElementById("rbMehrfach").style.background = "ffffff";	
											document.getElementById("txtEinzel").focus();
										}else{												
											document.getElementById("rbMehrfach").style.background = "#ff6600";	
											document.getElementById("rbEinzel").style.background = "ffffff";
											document.Form1.txtMehrfach.focus();
										}										
									}												
								}	
								
								function setInputFocus(key)
								{
									document.getElementById("rbSucheFahrg").style.background = "#ff6600";
									document.getElementById("rbSuchePDI").style.background = "#ffffff";
									document.getElementById("rbSucheFahrg").checked = true;
									if (key == 0){
										document.getElementById("rbEinzel").style.background = "#ff6600";										
										document.getElementById("rbMehrfach").style.background = "ffffff";
										document.getElementById("txtEinzel").focus();
										
									}else{
										document.Form1.txtMehrfach.focus();
										document.getElementById("rbMehrfach").style.background = "#ff6600";	
										document.getElementById("rbEinzel").style.background = "ffffff";
									}				
								}
								
								function switchTask(key)
								{
									document.getElementById("chkZulassen").style.background = "#eeeeee";
									document.getElementById("chkSperren").style.background = "#eeeeee";
									document.getElementById("chkEntsperren").style.background = "#eeeeee";
									document.getElementById("chkVerschieben").style.background = "#eeeeee";
									
									if (key == 0){
										document.getElementById("chkZulassen").style.background = "#ff6600";										
									}
									if (key == 1){
										document.getElementById("chkSperren").style.background = "#ff6600";
									}
									if (key == 2){
										document.getElementById("chkEntsperren").style.background = "#ff6600";
									}
									if (key == 3){
										document.getElementById("chkVerschieben").style.background = "#ff6600";
									}
								}
								
								function openinfo (url) {										
										fenster=window.open(url, "SIXT", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=500,height=200");
										fenster.focus();
								}
							
								document.getElementById("idWeiss").style.background = "#FFFFFF";					
								document.getElementById("idGelb").style.background = "#FFFF00";					
								document.getElementById("idOrange").style.background = "#FF8800";					
								document.getElementById("idRot").style.background = "#FF0000";					
								document.getElementById("idViolett").style.background = "#FF00FF";					
								document.getElementById("idBlau").style.background = "#0088FF";					
								document.getElementById("idGruen").style.background = "#008800";					
								document.getElementById("idGrau").style.background = "#888888";					
								document.getElementById("idBraun").style.background = "#804000";					
								document.getElementById("idSchwarz").style.background = "#000000";					
						-->
		</SCRIPT>
	</body>
</HTML>
