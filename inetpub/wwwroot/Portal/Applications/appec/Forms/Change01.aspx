<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01.aspx.vb" Inherits="AppEC.Change01" %>
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
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;&nbsp;&nbsp;
								</TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;Bitte Daten eingeben.</TD>
							</TR>
							<TR>
								<TD class="" vAlign="top" width="100">&nbsp;</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="" vAlign="top"></TD>
										</TR>
									</TABLE>
									<TABLE class="BorderLeftBottom" id="Table3" cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="2" bgColor="white" border="0">
													<TR>
														<TD vAlign="middle" noWrap colSpan="7"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;<asp:label 
                                                                id="lblSuccess" runat="server" EnableViewState="False" ForeColor="Blue"></asp:label></TD>
													</TR>
													<TR>
														<TD vAlign="middle" noWrap></TD>
														<TD class="" vAlign="middle" noWrap>Model-Id:
														</TD>
														<TD class="" vAlign="middle" noWrap><asp:textbox id="txtModelId" runat="server" CssClass="ButtonRowBackground" MaxLength="7"></asp:textbox></TD>
														<TD vAlign="top" noWrap colSpan="4">
															<P><FONT color="red"><asp:image id="Image1" runat="server" ToolTip="Alphanumerisch, max. 7 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="middle" noWrap></TD>
														<TD vAlign="middle" noWrap><asp:label id="Label1" runat="server" Enabled="False">Modellbezeichnung:</asp:label></TD>

														<TD vAlign="middle" noWrap><asp:textbox id="txtModell" runat="server"  CssClass="InputDisableStyle" Width="100%" ReadOnly="False"></asp:textbox></TD><TD vAlign="middle" noWrap>
														<TD vAlign="top" noWrap colSpan="3"></TD>
													</TR>
													<TR>
														<TD vAlign="middle" noWrap></TD>
														<TD class="" vAlign="middle" noWrap>SIPP-Code:</TD>
														<TD class="" vAlign="middle"><asp:textbox id="txtSippcode" runat="server" CssClass="ButtonRowBackground" MaxLength="4"></asp:textbox></TD>
														<TD vAlign="top" colSpan="4"><FONT color="red"><asp:image id="Image2" runat="server" ToolTip="Alphanumerisch, max. 4 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle">Hersteller:</TD>
														<TD class="" vAlign="middle" colSpan="4"><asp:dropdownlist id="ddlHersteller" runat="server" CssClass="DropDownStyle" Enabled="False" Width="100%"></asp:dropdownlist></TD>
														<td></td>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle">Batch-Id:</TD>
														<TD class="" vAlign="middle"><asp:textbox id="txtBatchId" runat="server" CssClass="ButtonRowBackground" MaxLength="8"></asp:textbox></TD>
														<TD vAlign="top" colSpan="4"><FONT color="red"><asp:image id="Image4" runat="server" ToolTip="Numerisch, max. 8 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle">Geplanter Liefermonat:</TD>
														<TD class="" vAlign="middle"><asp:textbox id="txtDatEinsteuerung" runat="server" CssClass="ButtonRowBackground" MaxLength="7" ToolTip="Datum, Format: MM.JJJJ"></asp:textbox></TD>
														<TD vAlign="top" colSpan="4"><FONT color="red"><asp:image id="Image6" runat="server" ToolTip="Datum, Format: MM.JJJJ" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle">Anzahl Fahrzeuge:</TD>
														<TD class="" vAlign="middle"><asp:textbox id="txtAnzahlFahrzeuge" runat="server" CssClass="ButtonRowBackground" MaxLength="5"></asp:textbox></TD>
														<TD vAlign="top" colSpan="4"><FONT color="red"><asp:image id="Image7" runat="server" ToolTip="Numerisch, max. 5 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle">Unit-Nr. von -&nbsp;bis:</TD>
														<TD class="" vAlign="middle"><asp:textbox id="txtUnitNrVon" runat="server" CssClass="ButtonRowBackground" MaxLength="8"></asp:textbox></TD>
														<TD vAlign="middle" noWrap align="left" colSpan="2"><asp:textbox id="txtUnitNrBis" runat="server" CssClass="ButtonRowBackground" MaxLength="8"></asp:textbox></TD>
														<TD class="" vAlign="top" colSpan="2"><FONT color="red"><asp:image id="Image8" runat="server" ToolTip="Numerisch, 8 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></TD>
													</TR>
                                                    <tr>
                                                        <TD vAlign="middle"></TD>
														<TD vAlign="middle">Unit-Nr. Upload:</TD>
														<TD vAlign="middle" noWrap colSpan="3"><input class="InfoBoxFlat" id="upFileUnitNr" type="file" size="40" name="File1" runat="server" /></TD>
														<TD vAlign="top" colSpan="2"><asp:image id="Image10" runat="server" ToolTip="alternativ zu (von..bis): Datei im Excel-Format, Unit-Nummern in Spalte A (ohne Überschrift)" ImageUrl="/Portal/Images/info.gif"></asp:image></TD>
                                                    </tr>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle">Laufzeit in Tagen:</TD>
														<TD class="" vAlign="middle"><asp:textbox id="txtLaufzeit" runat="server" CssClass="ButtonRowBackground" MaxLength="4"></asp:textbox></TD>
														<TD vAlign="top" noWrap colSpan="4">
															<P align="left"><FONT color="red"><asp:image id="Image9" runat="server" ToolTip="Numerisch, max. 4 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*&nbsp;</FONT>
																<asp:checkbox id="cbxLaufz" runat="server" TextAlign="Left" Text="Laufzeitbindung"></asp:checkbox></P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle">Bemerkungen:</TD>
														<TD class="" vAlign="middle" noWrap colSpan="3">
															<P><asp:textbox id="txtBemerkung" runat="server" CssClass="ButtonRowBackground" MaxLength="60" Width="100%"></asp:textbox></P>
														</TD>
														<TD class="" vAlign="top" colSpan="2"><asp:image id="Image12" runat="server" ToolTip="Alphanumerisch, max. 60 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD vAlign="middle">Auftragsnummer von - bis:</TD>
														<TD vAlign="middle" noWrap><asp:textbox id="txtAuftragsnummerVon" runat="server" CssClass="ButtonRowBackground" MaxLength="20"></asp:textbox></TD>
														<TD vAlign="top" align="left" colSpan="2"><asp:textbox id="txtAuftragsnummerBis" runat="server" CssClass="ButtonRowBackground" MaxLength="20"></asp:textbox></TD>
														<TD vAlign="top" colSpan="2"><asp:image id="Image3" runat="server" ToolTip="Alphanumerisch, max. 20 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD vAlign="middle">Verwendungszweck:</TD>
														<TD vAlign="middle" noWrap colSpan="3"><asp:dropdownlist id="ddlVerwendung" runat="server" CssClass="DropDownStyle" Width="100%"></asp:dropdownlist></TD>
														<TD vAlign="top" colSpan="2"></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD vAlign="middle">Kennzeichenserie:</TD>
														<TD vAlign="middle" noWrap colSpan="3"><asp:dropdownlist id="ddlKennzeichenserie1" runat="server" CssClass="DropDownStyle"></asp:dropdownlist></TD>
														<TD vAlign="top" colSpan="2"></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD vAlign="middle">Fahrzeuggruppe:</TD>
														<TD vAlign="top">
															<TABLE id="Table2" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD noWrap><asp:radiobutton id="rbLKW" runat="server" Width="60px" Text="LKW" GroupName="rbFahrzeuggruppe"></asp:radiobutton></TD>
																	<TD noWrap><asp:radiobutton id="rbPKW" runat="server" Width="60px" Text="PKW" GroupName="rbFahrzeuggruppe" Checked="True"></asp:radiobutton></TD>
																</TR>
															</TABLE>
														</TD>
														<TD vAlign="middle" colSpan="4">&nbsp;</TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle" noWrap>Wintertaugliche Bereifung:</TD>
														<TD class="" vAlign="middle">
															<TABLE id="Table5" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD noWrap><asp:radiobutton id="rbJ1" runat="server" Width="60px" Text="Ja" GroupName="rbWinterreifen"></asp:radiobutton></TD>
																	<TD noWrap><asp:radiobutton id="rbN1" runat="server" Width="60px" Text="Prüfen" GroupName="rbWinterreifen" Checked="True"></asp:radiobutton></TD>
																</TR>
															</TABLE>
														</TD>
														<TD vAlign="middle" colSpan="4"><asp:dropdownlist id="ddlModellZuHersteller" runat="server" Enabled="False" Width="0px"></asp:dropdownlist><asp:dropdownlist id="ddlModellZuSipp" runat="server" Enabled="False" Width="0px"></asp:dropdownlist>
														    <asp:dropdownlist id="ddlModellZuLaufzeit" runat="server" Enabled="False" Width="0px"></asp:dropdownlist><asp:dropdownlist id="ddlModellZuLaufzeitbindung" runat="server" Enabled="False" Width="0px"></asp:dropdownlist>
														</TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle" noWrap>Anhängerkupplung:</TD>
														<TD class="" vAlign="middle">
															
															<TABLE id="Table9" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD noWrap><asp:radiobutton id="rbJAnhaenger" runat="server" Width="60px" Text="Ja" 
                                                                GroupName="rbAnhaenger"></asp:radiobutton></asp:radiobutton></TD>
																	<TD noWrap><asp:radiobutton id="rbNAnhaenger" runat="server" Width="60px" Text="Prüfen" 
                                                                GroupName="rbAnhaenger" Checked="True"></asp:radiobutton></asp:radiobutton></TD>
																</TR>
															</TABLE>
															
                                                            
														</TD>
														<TD vAlign="middle" colSpan="4">&nbsp;</TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD class="" vAlign="middle" noWrap>Navigationssystem:</TD>
														<TD class="" vAlign="middle">
															<TABLE id="Table5" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD noWrap><asp:radiobutton id="rb_NaviJa" runat="server" Width="60px" Text="Ja" GroupName="rbNavi"></asp:radiobutton></TD>
																	<TD noWrap><asp:radiobutton id="rb_NaviNein" runat="server" Width="60px" Text="Nein" GroupName="rbNavi" Checked="True"></asp:radiobutton></TD>
																</TR>
															</TABLE>
														</TD>
														<TD vAlign="middle" colSpan="4"></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD vAlign="middle">Securiti Fleet:</TD>
														<TD vAlign="middle">
															<TABLE id="Table7" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD noWrap><asp:radiobutton id="rbJ2" runat="server" Width="60px" Text="Ja" GroupName="rbSecurFleet"></asp:radiobutton></TD>
																	<TD noWrap><asp:radiobutton id="rbN2" runat="server" Width="60px" Text="Nein" GroupName="rbSecurFleet" Checked="True"></asp:radiobutton></TD>
																</TR>
															</TABLE>
														</TD>
														<TD vAlign="middle" colSpan="4"><asp:textbox id="txtTest" runat="server" Width="0px" ReadOnly="True"></asp:textbox></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD vAlign="middle">Leasing</TD>
														<TD vAlign="middle">
															<TABLE id="Table8" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD noWrap>
																		<asp:radiobutton id="rbLeasingJa" runat="server" Width="60px" Text="Ja" GroupName="rbLeasing"></asp:radiobutton></TD>
																	<TD noWrap>
																		<asp:radiobutton id="rbLeasingNein" runat="server" Width="60px" Text="Nein" GroupName="rbLeasing" Checked="True"></asp:radiobutton></TD>
																</TR>
															</TABLE>
														</TD>
														<TD vAlign="middle" colSpan="4"></TD>
													</TR>
													<TR>
														<TD class="ButtonRowBackground" vAlign="middle"></TD>
														<TD class="ButtonRowBackground" vAlign="middle">&nbsp;<FONT color="#ff0000"><FONT face="Arial">*<FONT size="1">Eingabe 
																		erforderlich</FONT></FONT></FONT></TD>
														<TD class="ButtonRowBackground" vAlign="middle"></TD>
														<TD class="ButtonRowBackground" vAlign="middle" colSpan="4"></TD>
													</TR>
													<TR>
														<TD vAlign="middle"></TD>
														<TD vAlign="middle"><asp:dropdownlist id="ddlModellHidden" runat="server" EnableViewState="True" Enabled="False" Width="0px"></asp:dropdownlist></TD>
														<TD vAlign="middle" colSpan="1">
															<P align="right"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton" Width="150px"> &#149;&nbsp;Speichern</asp:linkbutton></P>
														</TD>
                                                        <TD vAlign="middle" colSpan="1">
															<P align="right"><asp:linkbutton id="cmdReset" runat="server" 
                                                                    CssClass="StandardButton" Width="150px"> &#149;&nbsp;Zurücksetzen</asp:linkbutton></P>
														</TD>
														<TD vAlign="middle" colSpan="2">
															<P align="right"><asp:linkbutton id="btnFinished" runat="server" 
                                                                    CssClass="StandardButton" Width="150px" Visible="False"> &#149;&nbsp;Zur Übersicht</asp:linkbutton></P>
														</TD>
														<td><asp:image id="Image5" runat="server" ToolTip="Alphanumerisch, max. 20 Stellen" ImageUrl="/Portal/Images/info.gif" Visible="False"></asp:image></td>
													</TR>
												</TABLE>
												<INPUT id="txtHerstellerHidden" type="hidden" size="1" runat="server"> <INPUT id="txtHerstellerBezeichnungHidden" type="hidden" size="1" runat="server"></TD>
										</TR>
									</TABLE>
									&nbsp;
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
						</TABLE> <!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
				<SCRIPT language="JavaScript">										
							<!--													
								function SetHersteller()
								{
									document.Form1.txtHerstellerHidden.value = document.Form1.ddlHersteller.value;																	
									for (var i = 0; i < document.Form1.ddlHersteller.length; i++)
									{
										if (document.Form1.ddlHersteller.options[i].value == document.Form1.txtHerstellerHidden.value)
										{												
											document.Form1.txtHerstellerBezeichnungHidden.value = document.Form1.ddlHersteller.options[i].text;
											break;
										}
									}																				
								}
								
								function setFocus()
								{
							        document.Form1.txtModelId.focus();	
								}
								
								function SetModell()
								{
									var ok;
									document.Form1.txtModell.value = "";
									ok=0;
									for (var i = 0; i < document.Form1.ddlModellHidden.length; i++)
									{
										if (document.Form1.ddlModellHidden.options[i].value == document.Form1.txtModelId.value)
										{
											document.Form1.txtModell.value = document.Form1.ddlModellHidden.options[i].text;
											ok=1;
											break;
										}
									}
									
									if ((document.Form1.txtModelId.value != "") && (ok==0)){	
										alert("Model-Id unbekannt!");
										document.Form1.txtModelId.value = "";
										document.Form1.txtModelId.focus();												
									}										
									else{								
									document.Form1.ddlModellZuHersteller.selectedIndex = 0;
									var id;
									for (var i = 0; i < document.Form1.ddlModellZuHersteller.length-1; i++)
									{
										
										if (document.Form1.ddlModellZuHersteller.options[i].value == document.Form1.txtModelId.value)
										{
											id = document.Form1.ddlModellZuHersteller.options[i].text;
											
											for (var j = 0; j < document.Form1.ddlHersteller.length-1; j++)
											{
												if (document.Form1.ddlHersteller.options[j].value == id)
												{
													document.Form1.ddlHersteller.selectedIndex = j;
													document.Form1.txtHerstellerHidden.value=id;
													document.Form1.txtHerstellerBezeichnungHidden.value=document.Form1.ddlHersteller.options[j].text;
												}
											}											
											break;
										}										
									}
									
									document.Form1.ddlModellZuSipp.selectedIndex = 0;
									var sipp;
									for (var i = 0; i < document.Form1.ddlModellZuSipp.length-1; i++)
									{
										if (document.Form1.ddlModellZuSipp.options[i].value == document.Form1.txtModelId.value)
										{											
											document.Form1.txtSippcode.value = document.Form1.ddlModellZuSipp.options[i].text;
											break;
										}										
									}

									document.Form1.ddlModellZuLaufzeit.selectedIndex = 0;
									//var Laufzeit;
									for (var i = 0; i < document.Form1.ddlModellZuLaufzeit.length - 1; i++) {
									    if (document.Form1.ddlModellZuLaufzeit.options[i].value == document.Form1.txtModelId.value) {
									        document.Form1.txtLaufzeit.value = document.Form1.ddlModellZuLaufzeit.options[i].text;
									        break;
									    }
									}

									document.Form1.ddlModellZuLaufzeitbindung.selectedIndex = 0;
									//var Laufzeitbindung;
									for (var i = 0; i < document.Form1.ddlModellZuLaufzeitbindung.length - 1; i++) {
									    if (document.Form1.ddlModellZuLaufzeitbindung.options[i].value == document.Form1.txtModelId.value) {

									        if (document.Form1.ddlModellZuLaufzeitbindung.options[i].text == 'X') {
									            document.Form1.cbxLaufz.checked = true;
									        }
									        
									        break;
									    }
									}
									
									
									document.Form1.txtBatchId.focus();											
								}
								}
							-->
				</SCRIPT>
			</TABLE>
		</form>
	</body>
</HTML>
