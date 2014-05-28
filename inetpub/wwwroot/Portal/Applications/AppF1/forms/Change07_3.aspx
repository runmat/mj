<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07_3.aspx.vb" Inherits="AppF1.Change07_3" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
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
	    <style type="text/css">
            .style1
            {
                color: #CC0000;
            }
            .style2
            {
                height: 25px;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" ImageUrl="/Portal/Images/empty.gif" Width="3px"></asp:imagebutton></td>
				</tr>
				<tr>
					<td vAlign="top" align="left" colSpan="3">
						<table id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Adressauswahl)</asp:label></td>
							</tr>
							<tr>
								<td vAlign="top" width="120">
									<table id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<tr>
											<td class="TaskTitle">&nbsp;</td>
										</tr>
										<tr>
											<td vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></td>
										</tr>
										<tr>
											<td vAlign="middle" width="150"><asp:linkbutton id="cmdSearch" runat="server" 
                                                    CssClass="StandardButton" Visible="False">&#149;&nbsp;Suchen</asp:linkbutton></td>
										</tr>
									</table>
								</td>
								<td vAlign="top">
									<table id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" 
                                                    runat="server" CssClass="TaskTitle" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink 
                                                    id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" 
                                                    NavigateUrl="Change01_2.aspx">Fahrzeugauswahl</asp:hyperlink></td>
										</tr>
									</table>
									<table id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top" align="left" colSpan="3">
												<table id="Table2" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<tr>
														<td class="StandardTableAlternate">Zustellart: </td>
														<td class="StandardTableAlternate" noWrap>
															<asp:radiobutton id="rb_VersandStandard" runat="server" 
                                                                Text="rb_VersandStandard" Checked="True" GroupName="Versandart" 
                                                                AutoPostBack="True"></asp:radiobutton></td>
														
														<td noWrap class="StandardTableAlternate">
															&nbsp;</td>
														
														
														<td noWrap>
															
															
															&nbsp;</td>
														
														
														<td class="StandardTableAlternate" noWrap>
															
															&nbsp;</td>
														<td class="StandardTableAlternate" width="100%">
															
															&nbsp;</td>
															
													</tr>
													<tr>
														<td class="StandardTableAlternate">&nbsp;</td>
														<td class="StandardTableAlternate" noWrap colspan="5">
															keine Sendungsverfolgung möglich, keine Laufzeitgarantie</td>
														
													</tr>
													
													<tr>
														<td class="StandardTableAlternate">&nbsp;</td>
														<td class="StandardTableAlternate" noWrap>
															<asp:radiobutton id="rb_VersandExpress" runat="server" Text="rb_VersandExpress" 
                                                                AutoPostBack="True"></asp:radiobutton></td>
														
														<td noWrap class="StandardTableAlternate">
															&nbsp;</td>
														
														
														<td noWrap>
															&nbsp;</td>
														
														
														<td class="StandardTableAlternate" noWrap>
															&nbsp;</td>
														<td class="StandardTableAlternate" width="100%">
															&nbsp;</td>
															
													</tr>
													<tr id="trDHL" runat="server" visible="false">
														<td class="StandardTableAlternate">&nbsp;</td>
														<td class="StandardTableAlternate" noWrap>
															<asp:Image ID="imgDHL" runat="server" ImageUrl="/Portal/Images/DHL.png" />
                                                        </td>
														
														<td class="StandardTableAlternate" noWrap colspan="4">
															<table>
															    <tr>
															        <td rowspan="0">
															            
															            DHL-Express: Rechnungsstellung erfolgt monatlich direkt an den Anforderer durch 
                                                                        den DAD</td>
															    </tr>
															
															    <tr>
															        <td rowspan="0">
															            
															<asp:radiobutton id="rb_0900" runat="server" Text="rb_0900" GroupName="Versandart"></asp:radiobutton>
															            
															        &nbsp;<asp:label id="lbl_0900" runat="server"> lbl_0900</asp:label>
															            
															        </td>
															    </tr>
															
															    <tr>
															        <td rowspan="0">
															            
															<asp:radiobutton id="rb_1000" runat="server" Text="rb_1000" GroupName="Versandart"></asp:radiobutton>
															            
															        &nbsp;<asp:label id="lbl_1000" runat="server"> lbl_1000</asp:label>
															            
															        </td>
															    </tr>
															
															    <tr>
															        <td rowspan="0">
															            
															<asp:radiobutton id="rb_1200" runat="server" Text="rb_1200" GroupName="Versandart"></asp:radiobutton>
															            
															        &nbsp;<asp:label id="lbl_1200" runat="server"> lbl_1200</asp:label>
															            
															        </td>
															    </tr>
															
															    <tr>
															        <td>
															            
															            Alle Kosten verstehen sich netto zzgl. Mwst. (auch Samstags Auslieferungen)<br />
															            <br />
															            </td>
															    </tr>
															
															</table>
															
															</td>
														
														
													</tr>
													<tr id="trTNT" runat="server" visible="false">
														<td class="StandardTableAlternate">&nbsp;</td>
														<td class="StandardTableAlternate" noWrap>
															<asp:Image ID="imgTNT" runat="server" ImageUrl="/Portal/Images/TNT.png" />
                                                        </td>
														
														<td class="StandardTableAlternate" noWrap colspan="4">
															
															<table>
															    <tr>
															        <td rowspan="0">
															            
															            TNT-Express: Rechnungsstellung erfolgt direkt an den Anforderer durch 
                                                                        TNT</td>
															    </tr>
															
															    <tr>
															        <td class="style2">
															            
															<asp:radiobutton id="rb_Sendungsverfolgt" runat="server" Text="rb_Sendungsverfolgt" 
                                                                            GroupName="Versandart"></asp:radiobutton>
															&nbsp;<asp:label id="lbl_Sendungsverfolgt" runat="server" Visible="False"> lbl_Sendungsverfolgt</asp:label>
															            
															        </td>
															    </tr>
															
															    <tr>
															        <td rowspan="0" class="style2">
															            
															<asp:radiobutton id="rb_0900TNT" runat="server" Text="rb_0900TNT" GroupName="Versandart"></asp:radiobutton>
															            
															        &nbsp;<asp:label id="lbl_900TNT" runat="server">lbl_900TNT</asp:label>
															            
															        </td>
															    </tr>
															
															    <tr>
															        <td rowspan="0">
															            
															<asp:radiobutton id="rb_1000TNT" runat="server" Text="rb_1000TNT" GroupName="Versandart"></asp:radiobutton>
															            
															        &nbsp;<asp:label id="lbl_1000TNT" runat="server">lbl_1000TNT</asp:label>
															            
															        </td>
															    </tr>
															
															    <tr>
															        <td rowspan="0">
															            
															<asp:radiobutton id="rb_1200TNT" runat="server" Text="rb_1200TNT" GroupName="Versandart"></asp:radiobutton>
															            
															        &nbsp;<asp:label id="lbl_1200TNT" runat="server">lbl_1200TNT</asp:label>
															            
															        </td>
															    </tr>
															
															    <tr>
															        <td>
															            
															            Alle Kosten verstehen sich netto zzgl. Mwst. (nur Auslieferungen Montag - Freitag)</td>
															    </tr>
															
															</table>
															</td>
														
														
													</tr>
                                                    
													<tr id="trHinweis" runat="server" visible="false">
														<td class="StandardTableAlternate">&nbsp;</td>
														<td class="StandardTableAlternate" noWrap valign="top">
															Hinweis:</td>
														
														<td class="StandardTableAlternate" colspan="4">
															
															Der Expressversand erfolgt&nbsp; auf Kosten des Anforderers. Bitte beachten Sie 
                                                            hierzu die Beförderungsbedingungen <br /> des jeweiligen Dienstleisters. </td>
														
														
													</tr>
                                                    
                                                    <tr>
                                                        <td class="StandardTableAlternate" colspan="6">
                                                            Achtung: Auslieferungen erfolgen täglich bei Beauftragung vor
                                                            <asp:Label ID="lbl_Uhrzeit" runat="server" Text="Uhrzeit"></asp:Label>.
                                                            <asp:Label ID="lbl_Hinweistext" runat="server" Text="lbl_Hinweistext"></asp:Label>
                                                        </td>
                                                    </tr>
												</table>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<table id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<tr id="trZeigeVorgegebeneAdressen" runat="server">
														<td class="StandardTableAlternate" valign="middle"  width="170">&nbsp;<asp:radiobutton id="chkZweigstellen" runat="server" GroupName="grpVersand" Checked="True" Text="Versandadressen:" AutoPostBack="True"></asp:radiobutton>&nbsp;</td>
														<td class="StandardTableAlternate"  align="left" valign="bottom" width="100%"><asp:dropdownlist id="cmbZweigstellen" runat="server"></asp:dropdownlist></td>
													</tr>
													<tr id="trZeigeZULST" runat="server">
														<td class="StandardTableAlternate" valign="middle"   width="170">&nbsp;<asp:radiobutton id="chkZulassungsstellen" runat="server" GroupName="grpVersand" Text="Zulassungsstellen:" AutoPostBack="True"></asp:radiobutton>&nbsp;</td>
														<td class="StandardTableAlternate" align="left" valign="bottom" width="100%"><asp:dropdownlist id="cmbZuslassungstellen" Visible="False" runat="server"></asp:dropdownlist>
														</td>
													</tr>
													<tr id="trZeigeManuelleAdresse" runat="server">
														<td class="StandardTableAlternate" width="17">
															<table id="Table22" height="155" cellSpacing="0" cellPadding="5" width="147" align="left" bgColor="white" border="0">
																<tr>
																	<td class="StandardTableAlternate" align="left" width="173" height="29">
																		<asp:radiobutton id="rb_Manuell" runat="server" GroupName="grpVersand" Text=" manuelle Eingabe:" AutoPostBack="True"></asp:radiobutton>
																	</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																</tr>
															</table>
														</td>
														<td class="StandardTableAlternate" noWrap>
															<table id="tbl_Adresse" visible="False" runat="server" height="155" 
                                                                cellSpacing="0" cellPadding="0" align="left" bgColor="white" border="0">
																<tr>
																	<td class="StandardTableAlternate" width="200"><asp:label id="Label3" runat="server">Name:</asp:label>&nbsp;</td>
																	<td class="StandardTableAlternate"><asp:textbox id="txt_Name" runat="server" 
                                                                            Width="255px" MaxLength="40"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr >
																	<td class="StandardTableAlternate" width="200"><asp:label id="lbl_Name2" 
                                                                            runat="server">lbl_Name2</asp:label></td>
																	<td class="StandardTableAlternate">
                                                                        <asp:textbox id="txt_Name2" runat="server" 
                                                                            Width="255px" MaxLength="40"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188">&nbsp;</td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="200"><asp:label id="Label2" runat="server">Strasse:</asp:label>&nbsp;</td>
																	<td class="StandardTableAlternate" width="188"><asp:textbox id="txt_Strasse" 
                                                                            runat="server" Width="255px" MaxLength="60"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188">&nbsp;&nbsp;<asp:label id="lbl_Nummer" runat="server">Nr.:</asp:label>&nbsp;
																		<asp:textbox id="txt_Nummer" runat="server" Width="45px" MaxLength="10"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate"  width="200"><asp:label id="lbl_PLZ" runat="server">PLZ:</asp:label>&nbsp;</td>
																	<td class="StandardTableAlternate"><asp:textbox id="txt_PLZ" runat="server" 
                                                                            Width="99px" MaxLength="10"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188" height="27"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="200"><asp:label id="lbl_Ort" runat="server">Ort:</asp:label></td>
																	<td class="StandardTableAlternate"><asp:textbox id="txt_Ort" runat="server" 
                                                                            Width="255px" MaxLength="40"></asp:textbox></td>
																	<td class="StandardTableAlternate" width="188"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
																<tr>
																	<td class="StandardTableAlternate" width="200">
																		<asp:label id="lbl_Land" runat="server">Land:</asp:label></td>
																	<td class="StandardTableAlternate">
																		<asp:dropdownlist id="ddl_Land" Runat="server" Enabled="False">
																			<asp:ListItem Value="0" Selected="True">DE</asp:ListItem>
																		</asp:dropdownlist></td>
																	<td class="StandardTableAlternate" width="188"></td>
																	<td class="StandardTableAlternate" width="133%">&nbsp;</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td class="TextLarge" vAlign="top" width="173">&nbsp;&nbsp;
														</td>
														<td class="TextLarge" vAlign="top" align="left" width="100%">&nbsp;&nbsp;
														</td>
													</tr>
													<tr id="ZeigeTEXT50" runat="server">
														<td class="StandardTableAlternate" vAlign="top" noWrap width="173">Kunde für 
															Anforderungen mit<BR>
															erweitertem Zahlungsziel<BR>
															(Delayed Payment) endgültig</td>
														<td class="StandardTableAlternate" vAlign="top" align="left" width="100%"><asp:textbox id="txtTEXT50" runat="server" MaxLength="50"></asp:textbox>&nbsp;&nbsp;
															<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Eingabe erforderlich" ControlToValidate="txtTEXT50"></asp:requiredfieldvalidator></td>
													</tr>
													<tr id="trInfTxt" runat="server">
													<td class="InfoText" vAlign="top" noWrap width="553" colSpan="2"><STRONG><U>
                                                            <span class="style1">Hinweis:</span><BR class="style1">
																</U></STRONG><span class="style1">Die Deutsche Post AG garantiert für diese Sendungen keine 
															Lauf- und Zustellungszeiten</span><BR class="style1">
															<span class="style1">und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
															
                                                            
                                                            </span>
                                                            <br class="style1">
                                                            <br class="style1">
                                                            <span class="style1"><strong>&nbsp;&nbsp;&nbsp;-95% aller Sendungen werden dem Empfänger
                                                                innerhalb von 24 Stunden zugestellt,</strong></span><br class="style1">
                                                            <span class="style1"><strong>&nbsp;&nbsp;&nbsp;-3% aller Sendungen benötigen zwischen
                                                                24 und 48 Stunden bis zur Zustellung.</strong></span><br class="style1">
                                                            <br class="style1">
                                                            <span class="style1">Bitte beachten Sie hierzu auch die Beförderungsbedingungen der
                                                                Deutschen Post AG.</span>
                                                        </td>
                                                    </tr>
                                                </table>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
										</tr>
										<tr>
											<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><br>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
		<!--<script language="JavaScript">
			 //
			window.document.Form1.elements[window.document.Form1.length-1].focus();
			//
		</script>-->
	</body>
</HTML>