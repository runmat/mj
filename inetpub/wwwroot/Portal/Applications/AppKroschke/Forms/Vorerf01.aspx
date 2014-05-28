<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Vorerf01.aspx.vb" Inherits="AppKroschke.Vorerf01" %>
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
			<input id="txtOrtsKzOld" type="hidden" name="txtOrtsKzOld" runat="server"> <INPUT id="txtFree2" type="hidden" name="txtFree2" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server" Font-Bold="True"></asp:label>)<asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx" CssClass="PageNavigation">Abfragekriterien</asp:hyperlink></td>
							</TR>
							<tr>
								<TD class="" vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="100" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD height="12">
												<P align="left"><asp:linkbutton id="btnListe" runat="server" CssClass="StandardButton"> &#149;&nbsp;Eingabeliste</asp:linkbutton></P>
											</TD>
										</TR>
										<TR>
											<TD>
												<P>&nbsp;</P>
											</TD>
										</TR>
									</TABLE>
									<asp:textbox id="txtDienstleistungsnr" runat="server" Font-Bold="True" Visible="False" Width="60px"></asp:textbox><asp:textbox id="txtURL" runat="server" Font-Bold="True" Visible="False" Width="31px"></asp:textbox><asp:textbox id="txtKundenname" runat="server" Font-Bold="True" Visible="False" Width="53px"></asp:textbox><asp:textbox id="txtSTVA" runat="server" Font-Bold="True" Visible="False" Width="42px" Height="22px"></asp:textbox><asp:dropdownlist id="ddlUrl" runat="server" Font-Bold="True" Visible="False" Width="50px" Font-Names="Courier New"></asp:dropdownlist><asp:textbox id="id_Record" runat="server" Font-Bold="True" Visible="False" Width="61px"></asp:textbox><asp:checkbox id="cbxShowColumn" runat="server" Visible="False"></asp:checkbox></TD>
								<TD vAlign="top" align="left">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" align="right">&nbsp;
												<asp:label id="lblFilter" runat="server" Font-Bold="True" Visible="False"></asp:label><asp:label id="lblDatensatz" runat="server" Font-Bold="True" Visible="False"></asp:label></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="1" cellPadding="1" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label><asp:label id="lblMsg" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
													</TR>
													<TR>
														<TD vAlign="top" align="left" colSpan="2"><TABLE id="Table10" cellSpacing="0" cellPadding="2" bgColor="white" border="0" runat="server">
																<TR id="Zeile0" runat="server">
																	<TD class="" vAlign="top" align="left" width="196"><asp:label id="lblStamm" runat="server" Font-Bold="True" Font-Underline="True">Stammdaten</asp:label></TD>
																	<TD vAlign="top" noWrap align="left" width="594">
																		<TABLE id="Table21" cellSpacing="1" cellPadding="1" width="100%" border="0">
																			<TR>
																				<TD width="100%">SAP-Id:
																					<asp:label id="txtSAPID" runat="server" Font-Underline="True" Enabled="False"></asp:label>&nbsp;-&nbsp;
																					<asp:label id="txtSQLId" runat="server" Font-Underline="True" Enabled="False"></asp:label></TD>
																				<TD></TD>
																				<TD><asp:textbox id="txtBarcode" runat="server" Width="100px" Visible="False" BackColor="Yellow"></asp:textbox></TD>
																				<TD noWrap><asp:linkbutton id="btnBarcode" runat="server" Visible="False" CssClass="StandardButtonTable">&#149;&nbsp;Daten holen</asp:linkbutton></TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
																<TR id="Zeile1" runat="server">
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="left" width="196">
																		<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
																			<TR>
																				<TD width="100%">Kunde<FONT color="red">*</FONT></TD>
																				<TD align="right"><asp:textbox id="txtKundennummer" runat="server" Width="62px" 
                                                                                        MaxLength="8"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD class="" vAlign="top" noWrap align="left" width="593">
																		<TABLE id="Table8" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD><asp:dropdownlist id="ddlKunnr" runat="server" Font-Bold="True" Font-Names="Courier New" EnableViewState="False"></asp:dropdownlist></TD>
																			</TR>
																		</TABLE>
																		<INPUT id="txtDummy" type="hidden" name="txtDummy" runat="server">&nbsp;
																		<asp:textbox id="txtAuart" runat="server" Font-Bold="True" Visible="False" Width="75px" MaxLength="10"></asp:textbox>
																		<asp:textbox id="txtVerVbeln" runat="server" Font-Bold="True" Visible="False" Width="75px" MaxLength="10"></asp:textbox>
																	</TD>
																</TR>
																<TR id="Zeile2" runat="server">
																	<TD class="TextLargeDescription" vAlign="top" align="left" width="196">
																		<TABLE id="Table6" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD>Referenzen</TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD class="TextLarge" vAlign="middle" noWrap align="left" width="593">
																		<TABLE id="Table16" cellSpacing="1" cellPadding="1" width="300" border="0">
																			<TR>
																				<TD noWrap><asp:textbox id="txtHalter" runat="server" Font-Bold="True" Width="175px" MaxLength="20"></asp:textbox>&nbsp;
																					<asp:textbox id="txtFahrgestellNr" runat="server" Font-Bold="True" Width="175px" MaxLength="20"></asp:textbox>&nbsp;
																					<asp:textbox id="txtTour" runat="server" Visible="False" Width="75px"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																		<asp:dropdownlist id="ddlKundeBakX" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:dropdownlist><asp:checkbox id="cbxSave" runat="server" Visible="False" Enabled="False" Text="saved"></asp:checkbox><asp:checkbox id="cbxListe" runat="server" Visible="False" Enabled="False" Text="liste"></asp:checkbox></TD>
																</TR>
																<TR id="Zeile3" runat="server">
																	<TD class="TextLargeDescription" vAlign="top" noWrap align="left" width="196">
																		<TABLE id="Table17" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD>Dienstleistung<FONT color="red">*</FONT></TD>
																			</TR>
																		</TABLE>
																		<INPUT id="txtDienstHidden" type="hidden" size="1" name="txtDienstHidden" runat="server">
																	</TD>
																	<TD class="TextLarge" vAlign="top" noWrap align="left" colSpan="1" width="593">
																		<TABLE id="Table11" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD><asp:dropdownlist id="ddlDienst" runat="server" Font-Bold="True" Font-Names="Courier New" EnableViewState="False"></asp:dropdownlist></TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
																<TR id="Zeile4" runat="server">
																	<TD class="" vAlign="top" noWrap align="left" width="197">
																		<TABLE id="Table13" cellSpacing="1" cellPadding="1" width="100%" border="0">
																			<TR>
																				<TD width="100%">StVA<FONT color="red">*</FONT></TD>
																				<TD align="right"><asp:textbox id="txtStVASel" runat="server" Width="40px" MaxLength="3"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD class="" vAlign="top" noWrap align="left" width="593">
																		<TABLE id="Table14" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD noWrap><asp:dropdownlist id="ddlSTVA" runat="server" Font-Bold="True" CssClass="TextHighlight" Font-Names="Courier New" EnableViewState="False"></asp:dropdownlist>&nbsp;
																					<asp:linkbutton id="LinkButton2" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Reservierung</asp:linkbutton></TD>
																			</TR>
																		</TABLE>
																		<asp:hyperlink id="hypUrl" runat="server" Font-Size="Smaller"></asp:hyperlink></TD>
																</TR>
																<TR id="Zeile5" runat="server">
																	<TD vAlign="top" noWrap align="left" width="196">
																		<TABLE id="Table18" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD>Datum Zulassung<FONT color="red">*</FONT></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD vAlign="top" align="left" width="593">
																		<TABLE id="Table15" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD noWrap><asp:textbox id="txtZulassungsdatum" runat="server" Font-Bold="True" Width="75px" MaxLength="10"></asp:textbox>&nbsp;(ttmmjj)</TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
																<TR id="Zeile8" runat="server">
																	<TD vAlign="top" noWrap align="left" width="196">
																		<TABLE id="Table19" cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD>Kennzeichen</TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD vAlign="top" noWrap align="left" width="594">
																		<P align="left">
																			<TABLE id="Table12" cellSpacing="1" cellPadding="1" width="609" align="left" border="0" height="64">
																				<TR>
																					<TD noWrap width="221" height="28"><asp:textbox id="txtOrtsKz" runat="server" Font-Bold="True" Width="30px" MaxLength="3"></asp:textbox>&nbsp;-&nbsp;
																						<asp:textbox id="txtWunschkennzeichenAbc" runat="server" Font-Bold="True" Width="100px" MaxLength="10"></asp:textbox>&nbsp;
																						<asp:textbox id="txtWunschkennzeichenNr" runat="server" Font-Bold="True" Visible="False" Width="50px" MaxLength="4"></asp:textbox>&nbsp;</TD>
																					<TD noWrap align="left" width="246" height="28"><asp:checkbox id="cbxEinKennz" runat="server" Text="Nur ein Kennzeichen" Width="149px"></asp:checkbox>
																						<asp:checkbox id="chk_krad" runat="server" Text="Krad"></asp:checkbox><asp:textbox id="txtWunschkennzeichen" runat="server" Font-Bold="True" Visible="False" Width="37px" MaxLength="2"></asp:textbox></TD>
																					<TD noWrap align="right" width="100%" height="28">Typ&nbsp;<asp:dropdownlist id="ddlKzTyp" runat="server" Font-Bold="True" Font-Names="Courier New"></asp:dropdownlist></TD>
																				</TR>
																				<TR>
																					<TD noWrap width="221">
																						<asp:checkbox id="chk_Feinstaub" runat="server" Text="Feinstaubplakette vom Amt"></asp:checkbox></TD>
																					<TD align="left" width="246"></TD>
																					<TD noWrap align="right" width="100%"></TD>
																				</TR>
																			</TABLE>
																		</P>
																	</TD>
																</TR>
																<TR id="Zeile6" runat="server">
																	<TD class="" vAlign="middle" noWrap align="left" width="196">
																		<P align="left">&nbsp;</P>
																	</TD>
																	<TD class="" vAlign="top" noWrap align="left" width="594">
																		<TABLE id="Table7" cellSpacing="1" cellPadding="1" width="100%" align="left" border="0">
																			<TR>
																				<TD noWrap width="100%"><asp:checkbox id="cbxWunschkennzFlag" runat="server" Text="Wunsch-Kennzeichen"></asp:checkbox></TD>
																				<TD noWrap align="right"><asp:checkbox id="cbxReserviert" runat="server" Text="Reserviert, Nr."></asp:checkbox>&nbsp;
																					<asp:textbox id="txtReservNr" runat="server" Font-Bold="True" Width="150px"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
																<TR id="trAdresse" style="DISPLAY: none" runat="server">
																	<TD vAlign="top" noWrap align="left" width="196">
																		<TABLE id="Table20" cellSpacing="1" cellPadding="1" border="0">
																			<TBODY>
																				<TR>
																					<TD><U><FONT color="black">Anlieferadresse</FONT></U></TD>
																				<tr>
																					<td><A class="StandardButtonTable" href="javascript:deleteAddress()">•&nbsp;Löschen</A></td>
																				</tr>
																	</TD>
																</TR>
															</TABLE>
														</TD>
														<TD vAlign="top" noWrap align="left" width="594">
															<TABLE class="TableFrame" id="tblAdresse" cellSpacing="1" cellPadding="1" width="100%" align="left" border="0" runat="server">
																<TR>
																	<TD>Name</TD>
																	<TD width="221"><asp:textbox id="txtName1" runat="server" Font-Bold="True" MaxLength="35"></asp:textbox></TD>
																	<TD align="right">Straße, Nr.
																		<asp:textbox id="txtStrasse" runat="server" Font-Bold="True" Width="150px" MaxLength="35"></asp:textbox></TD>
																</TR>
																<TR>
																	<TD>Plz</TD>
																	<TD align="left" width="243"><asp:textbox id="txtPLZ" runat="server" Font-Bold="True" Width="75px" MaxLength="10"></asp:textbox></TD>
																	<TD align="right">Ort&nbsp;
																		<asp:textbox id="txtOrt" runat="server" Font-Bold="True" Width="150px" MaxLength="25"></asp:textbox></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR id="Zeile7" runat="server">
														<TD vAlign="middle" noWrap align="left" width="196"></TD>
														<TD vAlign="top" noWrap align="left" width="594">
															<TABLE id="Table5" cellSpacing="1" cellPadding="1" width="100%" align="left" border="0" runat="server">
																<TR>
																	<TD vAlign="baseline" noWrap align="left">Bemerkung&nbsp;
																	</TD>
																	<TD vAlign="top" noWrap align="right" width="100%"><asp:textbox id="txtInterneBemerkung" runat="server" Font-Bold="True" Width="100%" MaxLength="40"></asp:textbox></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left" width="196"></TD>
														<TD vAlign="top" noWrap align="left" width="593">
															<TABLE class="" id="tblPreiseVA" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD></TD>
																</TR>
																<TR>
																	<TD>
																		<P><asp:textbox id="txtPreisSTVA" runat="server" Font-Bold="True" Visible="False" Width="75px"></asp:textbox></P>
																	</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left" height="1" width="197" bgColor="#0033cc"></TD>
														<TD vAlign="top" noWrap align="left" height="1" bgColor="#0033cc" width="593"></TD>
													</TR>
													<TR id="TRKundeInfo" runat="server">
														<TD vAlign="top" noWrap align="left" width="197" bgColor="#ededed" height="37">
															<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
																<TR>
																	<TD width="100%">Kundeninterne Informationen</TD>
																</TR>
															</TABLE>
														</TD>
														<TD vAlign="top" noWrap align="left" bgColor="#ededed" height="37" width="594">
															<TABLE id="tblKundeInfo" cellSpacing="1" cellPadding="1" width="100%" align="left" border="0" runat="server">
																<TR>
																	<TD vAlign="top" noWrap align="left" width="157">Verkäuferkürzel:</TD>
																	<TD vAlign="top" align="left" width="87"><asp:textbox id="txtVKkurz" runat="server" Font-Bold="True" Width="44px" MaxLength="4"></asp:textbox>&nbsp;</TD>
																</TR>
																<TR>
																	<TD vAlign="top" noWrap align="left" width="157" bgColor="#ededed">Kundeninterne 
																		Referenz:</TD>
																	<TD vAlign="top" noWrap align="left" width="100%" bgColor="#ededed">
																		<asp:textbox id="txtKIReferenz" runat="server" Font-Bold="True" Width="377px" MaxLength="40"></asp:textbox></TD>
																</TR>
																<TR>
																	<TD vAlign="top" noWrap align="left" width="157" bgColor="#ededed">Notiz:</TD>
																	<TD vAlign="top" noWrap align="left" width="100%" bgColor="#ededed"><asp:textbox id="txtNotiz" runat="server" Font-Bold="True" Width="441px" MaxLength="100" Font-Names="Arial"></asp:textbox></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap align="left" height="1" width="196" bgColor="#0033cc"></TD>
														<TD vAlign="top" noWrap align="left" height="1" bgColor="#0033cc" width="593"></TD>
													</TR>
													<TR id="trButtons" runat="server">
														<TD vAlign="baseline" noWrap align="left" colSpan="2" width="794">
															<asp:linkbutton id="cmdNew" runat="server" CssClass="StandardButtonTable" Width="191px">&#149;&nbsp;Speichern /  Neuer Datensatz</asp:linkbutton>&nbsp;&nbsp;
															<A class="StandardButtonTable" href="javascript:showhide()">•&nbsp;Anlieferadresse</A></TD>
													</TR>
												</TABLE>
												<FONT color="red">*</FONT><asp:label id="Label1" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Red">Eingabe erforderlich</asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR>
					<TD width="94">
						<P>&nbsp;</P>
					</TD>
					<TD><!--#include File="../../../PageElements/Footer.html" -->
						<P>&nbsp;</P>
					</TD>
				</TR>
				<SCRIPT language="JavaScript">										
							<!--
								function SetDienst()
								{
									document.Form1.txtDienstHidden.value = document.Form1.ddlDienst.value;
								}
							
								function SetOrtsKz()
								{
									if (document.Form1.txtOrtsKzOld.value == document.Form1.txtOrtsKz.value)
									{
										document.Form1.txtOrtsKzOld.value = document.Form1.ddlSTVA.value;
										document.Form1.txtOrtsKz.value = document.Form1.ddlSTVA.value;
										if (document.Form1.ddlSTVA.value == "HH1") {
										    document.Form1.txtOrtsKz.value = "HH";
										}
										if (document.Form1.ddlSTVA.value == "HH2") {
										    document.Form1.txtOrtsKz.value = "HH";
										}
										if (document.Form1.ddlSTVA.value == "HH4") {
										    document.Form1.txtOrtsKz.value = "HH";
										}
										if (document.Form1.ddlSTVA.value == "HH5") {
										    document.Form1.txtOrtsKz.value = "HH";
										}
										if (document.Form1.ddlSTVA.value == "HH6") {
										    document.Form1.txtOrtsKz.value = "HH";
										}											
									}
									/*if (document.Form1.ddlSTVA.value == "A")
									{
										document.Form1.txtStVASel.value = "";
									}
									else
									{*/
										document.Form1.txtStVASel.value = document.Form1.ddlSTVA.value;
									//}
									document.getElementById("hypUrl").href = "";
									document.getElementById("hypUrl").style.color = "#ffffff";
									document.getElementById("hypUrl").style.cursor = "default";
								}
								
								function keyListener(e){
								if(!e){
									//for IE
									e = window.event;
									}
									if(e.keyCode != 9){
										return 0;
   									}
   								}
								
								function SetStVA()
								{
									if (keyListener()==0){
										
									
									document.Form1.ddlSTVA.selectedIndex = 0;
									for (var i = 0; i < document.Form1.ddlSTVA.length-1; i++)
									{
										if (document.Form1.ddlSTVA.options[i].value.substr(0, document.Form1.txtStVASel.value.length) == document.Form1.txtStVASel.value.toUpperCase())
										{
											document.Form1.ddlSTVA.selectedIndex = i;
											break;
										}
									}
									SetOrtsKz();
									if (document.Form1.txtStVASel.value == "")
										{											
											document.Form1.ddlSTVA.value = "";
											SetOrtsKz();
										}		
									}															
								}
								function SetKnr()
								{	var found = 0;
									var s;
								
									for (var i = 0; i<=document.Form1.ddlKunnr.value.length-1; i++)
									{
										s = String(document.Form1.ddlKunnr.value);
										if (s.charAt(i) != '0')
										{
											found=i;
											break;
										}										
									}						
									s = String(document.Form1.ddlKunnr.value);
									document.Form1.txtKundennummer.value = String(s).substring(found,String(s).length)
									//20061102
									document.Form1.txtDummy.value = document.Form1.ddlKunnr.selectedIndex;
								}
								function SetKunnr()
								{		
									document.Form1.txtDummy.value = "";
									document.Form1.ddlKunnr.selectedIndex = 0;
									document.Form1.txtKundennummer.value = trimNumber(document.Form1.txtKundennummer.value);
									for (var i = 0; i <= document.Form1.ddlKunnr.length-1; i++)
									{
										if (String(parseInt(document.Form1.ddlKunnr.options[i].value,10)).substr(0, document.Form1.txtKundennummer.value.length) == document.Form1.txtKundennummer.value.toUpperCase())
										{
											document.Form1.ddlKunnr.selectedIndex = i;
											document.Form1.txtDummy.value = document.Form1.ddlKunnr.selectedIndex;
											//document.Form1.ddlKunnr.focus();
											break;
										}
									}
					                }

					                function trimNumber(s) {
					                    while (s.substr(0, 1) == '0' && s.length > 1) { s = s.substr(1, 9999); }
					                    return s;
					                }
								
								if (typeof window.event != 'undefined')
									document.onkeydown = function()
									{
										if (event.srcElement.tagName.toUpperCase() != 'INPUT')
										return (event.keyCode != 8);
									}
								else
									document.onkeypress = function(e)
									{
										if (e.target.nodeName.toUpperCase() != 'INPUT')
										return (e.keyCode != 8);
								}
								
								function showhide()
								{
									o = document.getElementById("trAdresse").style;
									
									if (o.display != "none"){
											o.display = "none";											
										} else {											
											o.display = "";											
									}					
								}
								
								function deleteAddress()
								{
									document.Form1.txtName1.value = "";
									document.Form1.txtStrasse.value = "";
									document.Form1.txtPLZ.value = "";
									document.Form1.txtOrt.value = "";
								}
								
								if ((document.Form1.txtName1.value != "") || (document.Form1.txtStrasse.value != "") || (document.Form1.txtPLZ.value != "") || (document.Form1.txtOrt.value != ""))  {
									o = document.getElementById("trAdresse").style;
									o.display = "";
								}																
							-->
				</SCRIPT>
			</table>
			<asp:label id="lblScript" runat="server"></asp:label></TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
