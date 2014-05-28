<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADSave.aspx.vb" Inherits="AppUeberf.UeberfDADSave" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            #Table12
            {}
            .style1
            {
                width: 297px;
                font-weight: bold;
                text-decoration: underline;
            }
            #Table2
            {
                width: 99%;
            }
            .style2
            {
                width: 466px;
            }
            </style>
	    </HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" cellSpacing="0" cellPadding="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td vAlign="top" align="left" width="100%" colSpan="3">
						<table id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Übersicht</asp:label>)</td>
							</tr>
							<tr>
								<td style="WIDTH: 144px" vAlign="top" width="174">
									<table id="Table12" cellSpacing="0" cellPadding="0" border="0">
										<tr>
											<td width="150">
												<asp:linkbutton id="lbtWeiter" tabIndex="12" runat="server" 
                                                    CssClass="StandardButtonTable" Width="100px"> •&nbsp;Speichern</asp:linkbutton>
												</td>
										</tr>
										<tr>
											<td vAlign="middle" width="150">
                                                <asp:linkbutton id="lbtBack" tabIndex="12" 
                                                    runat="server" CssClass="StandardButtonTable" Width="100px" 
                                                    BorderStyle="None" Height="19px"> •&nbsp;Zurück</asp:linkbutton></td>
										</tr>
										<tr>
											<td vAlign="middle" width="150">
                                                <asp:linkbutton id="lbtPrint" tabIndex="12" 
                                                    runat="server" CssClass="StandardButtonTable" Width="100px" 
                                                    BorderStyle="None" Height="19px" Visible="False"> •&nbsp;Drucken</asp:linkbutton></td>
										</tr>
									</table>
								</td>
								<td style="WIDTH: 917px" vAlign="top">
									<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td style="WIDTH: 437px" width="437">
												<asp:Literal ID="ltAnzeige" runat="server"></asp:Literal>
                                            </td>
										</tr>
										<tr>
											<td style="WIDTH: 437px" width="437">
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000" 
                                                    style="font-weight: 700"></asp:Label>
                                            </td>
										</tr>
										<tr><td>
                                            <asp:Panel ID="pnlZulassung" runat="server" Height="268px" Visible="False">
                                               				<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD class="style1">
                                                Zulassung:</TD>
                                                <td>
                                                    Buchungscode: <asp:Label ID="lblBuchungscode" runat="server"></asp:Label>
                                                </td>
											
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label5" runat="server" Width="180px" 
                                                    Font-Bold="True" ForeColor="#009900">Leasingnehmer</asp:label></TD>
											<TD><asp:label id="Label15" runat="server" Width="146px" Font-Bold="True" 
                                                    ForeColor="#009900">Halter</asp:label></TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label40" runat="server" Width="110px">Nr:</asp:Label>
                                                                                        <asp:Label ID="lblLnNummer" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
										<TR>
											<TD class="style2"><asp:label id="Label6" runat="server" Width="110px"> Name:</asp:label>
                                                <asp:label id="lblLnName" runat="server"></asp:label></TD>
											<TD>
                                                <asp:Label ID="Label73" runat="server" Height="16px" Width="106px">Name:</asp:Label>
                                                <asp:Label ID="lblShName" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD height="21" class="style2"><asp:label id="Label7" runat="server" Width="110px">Straße, Nr:</asp:label>
                                                <asp:label id="lblLnStrasse" runat="server"></asp:label></TD>
											<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Straße, Nr:</asp:label>
                                                <asp:label id="lblShStrasse" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label8" runat="server" Width="110px">PLZ Ort:</asp:label>
                                                <asp:label id="lblLnOrt" runat="server"></asp:label></TD>
											<TD><asp:label id="Label18" runat="server" Width="106px">PLZ Ort:</asp:label>
                                                <asp:label id="lblShOrt" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="style2">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD class="style2"><asp:label id="lblFahrzeugdaten" runat="server" Width="186px" 
                                                    Font-Bold="True" ForeColor="#009900">Zulassungsdaten</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ:</asp:label><asp:label id="lblHerst" runat="server"></asp:label></TD>
											<TD><asp:label id="Label11" runat="server" Width="150px">Wunschkennzeichen:</asp:label>
                                                <asp:label id="lblWunschkenn" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label42" runat="server" Width="110px">Fgst.-Nummer:</asp:Label>
                                                <asp:Label ID="lblVin" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label54" runat="server" Width="150px">Res.-Nummer:</asp:Label>
                                                <asp:Label ID="lblResNr" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label43" runat="server" Width="110px">Referenz-Nr.:</asp:Label>
                                                <asp:Label ID="lblRef" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label55" runat="server" Width="150px">Res.-Name:</asp:Label>
                                                <asp:Label ID="lblResName" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label44" runat="server" Width="110px">Briefnummer:</asp:Label>
                                                <asp:Label ID="lblBriefnr" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label53" runat="server" Width="150px">Feinstaubplakette:</asp:Label>
                                                <asp:Label ID="lblFeinstaub" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">&nbsp;</TD>
											<TD>
                                                <asp:Label ID="Label52" runat="server" Width="150px">Zulassungsdatum:</asp:Label>
                                                <asp:Label ID="lblZulDat" runat="server"></asp:Label>
                                            </TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        &nbsp;</td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label51" runat="server" Width="150px">Terminart:</asp:Label>
                                                                                        <asp:Label ID="lblTerminart" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style2">
                                                                                        &nbsp;</td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label50" runat="server" Width="150px">KFZ-Steuer:</asp:Label>
                                                                                        <asp:Label ID="lblSteuer" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
										<TR>
											<TD class="style2">&nbsp;</TD>
											<TD>&nbsp;</TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label57" runat="server" Font-Bold="True" Width="188px" 
                                                                                            ForeColor="#009900">Versicherungsnehmer</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label10" runat="server" Font-Bold="True" Width="188px" 
                                                                                            ForeColor="#009900">Versicherungsdaten</asp:Label>
                                                                                    </td>
                                                                                </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label2" runat="server" Width="110px">Name:</asp:Label>
                                                <asp:Label ID="lblVersName" runat="server"></asp:Label>
                                            </TD>
                                               
                                            <td><asp:Label ID="Label102" runat="server" Width="135px">Versicherung:</asp:Label>
                                            <asp:Label ID="lblVersGesellschaft" runat="server"></asp:Label></td>
										    
										</TR>
										                        <tr>
                                                                    <td class="style2">
                                                                        <asp:Label ID="Label64" runat="server" Width="110px">Straße, Nr:</asp:Label>
                                                                        <asp:Label ID="lblVersStrasse" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label59" runat="server" Width="135px">eVB-Nummer:</asp:Label>
                                                                        <asp:Label ID="lblEvbNr" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label65" runat="server" Width="110px">PLZ Ort:</asp:Label>
                                                <asp:Label ID="lblVersOrt" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label60" runat="server" Width="135px">eVB-von:</asp:Label>
                                                <asp:Label ID="lblEvbVon" runat="server"></asp:Label>
                                            </TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        &nbsp;</td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label56" runat="server" Width="135px">eVB-bis:</asp:Label>
                                                                                        <asp:Label ID="lblEvbBis" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
										<TR>
											<TD class="style2">
                                                &nbsp;</TD>
											<TD></TD>
										</TR>
									                                            <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label69" runat="server" Font-Bold="True" Width="203px" 
                                                                                            ForeColor="#009900">Versand 
                                                                                        Schein und Schilder</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label70" runat="server" Width="110px"> Name:</asp:Label>
                                                                                        <asp:Label ID="lblVssName" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label71" runat="server" Width="110px">Straße, Nr:</asp:Label>
                                                                                        <asp:Label ID="lblVssStrasse" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label72" runat="server" Width="110px">PLZ Ort:</asp:Label>
                                                                                        <asp:Label ID="lblVssOrt" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
									</TABLE>
									        </asp:Panel>
									        </TD>
                                            </td></tr>
                                            <tr><td>
                                                <asp:Panel ID="pnlAuslieferung" runat="server" Visible="False">
                                                    <TABLE id="Table4" cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD class="style1" colspan="2">
                                                Auslieferung:</TD>
											
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label3" runat="server" Width="180px" 
                                                    Font-Bold="True" ForeColor="#009900">Leasingnehmer</asp:label></TD>
											<TD><asp:label id="Label4" runat="server" Width="146px" Font-Bold="True" 
                                                    ForeColor="#009900">Fahrzeugnutzer</asp:label></TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label9" runat="server" Width="110px">Nr:</asp:Label>
                                                                                        <asp:Label ID="lblAusLnNummer" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
										<TR>
											<TD class="style2"><asp:label id="Label13" runat="server" Width="110px"> Name:</asp:label>
                                                <asp:label id="lblAusLnName" runat="server"></asp:label></TD>
											<TD>
                                                <asp:Label ID="Label16" runat="server" Height="16px" Width="106px">Name:</asp:Label>
                                                <asp:Label ID="lblFnName" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD height="21" class="style2"><asp:label id="Label20" runat="server" Width="110px">Straße, Nr:</asp:label>
                                                <asp:label id="lblAusLnStrasse" runat="server"></asp:label></TD>
											<TD height="21"><asp:label id="Label22" runat="server" Width="106px">Telefon:</asp:label>
                                                <asp:label id="lblFnTelefon" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label24" runat="server" Width="110px">PLZ Ort:</asp:label>
                                                <asp:label id="lblAusLnOrt" runat="server"></asp:label></TD>
											<TD><asp:label id="Label26" runat="server" Width="106px">E-Mail:</asp:label>
                                                <asp:label id="lblFnMail" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="style2">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label28" runat="server" Width="186px" 
                                                    Font-Bold="True" ForeColor="#009900">Fahrzeugdaten</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label29" runat="server" Width="110px">Hersteller / Typ:</asp:label>
                                                <asp:label id="lblAusFahrzeugtyp" runat="server"></asp:label></TD>
											<TD><asp:label id="Label31" runat="server" Width="150px">Kennzeichen:</asp:label>
                                                <asp:label id="lblAusKennzeichen" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label33" runat="server" Width="110px">Fgst.-Nummer:</asp:Label>
                                                <asp:Label ID="lblAusFin" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label35" runat="server" Width="150px">Bereifung:</asp:Label>
                                                <asp:Label ID="lblAusBereifung" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label45" runat="server" Width="110px">Briefnummer:</asp:Label>
                                                <asp:Label ID="lblAusBriefnummer" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label39" runat="server" Width="150px">Fahrzeugklasse:</asp:Label>
                                                <asp:Label ID="lblAusFzgKlasse" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">&nbsp;</TD>
											<TD>&nbsp;</TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label76" runat="server" Font-Bold="True" Width="188px" 
                                                                                            ForeColor="#009900">Händler</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label77" runat="server" Font-Bold="True" Width="188px" 
                                                                                            ForeColor="#009900">Fahrzeugempfänger</asp:Label>
                                                                                    </td>
                                                                                </tr>
										                        <tr>
                                                                    <td class="style2">
                                                                        <asp:Label ID="Label104" runat="server" Width="110px">Name:</asp:Label>
                                                                        <asp:Label ID="lblHdName" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label103" runat="server" Width="110px">Name:</asp:Label>
                                                                        <asp:Label ID="lblFeName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label82" runat="server" Width="110px">Straße, Nr:</asp:Label>
                                                <asp:Label ID="lblHdStrasse" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label88" runat="server" Width="110px">Straße, Nr:</asp:Label>
                                                <asp:Label ID="lblFeStrasse" runat="server"></asp:Label>
                                            </TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label86" runat="server" Width="110px">PLZ Ort:</asp:Label>
                                                                                        <asp:Label ID="lblHdOrt" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label89" runat="server" Width="110px">PLZ Ort:</asp:Label>
                                                                                        <asp:Label ID="lblFeOrt" runat="server"></asp:Label>
                                                                                    </td>
                                                        </tr>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label90" runat="server" Width="110px">Ansprechpartner:</asp:Label>
                                                                                        <asp:Label ID="lblHdAnsprech" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label93" runat="server" Width="110px">Ansprechpartner:</asp:Label>
                                                                                        <asp:Label ID="lblFeAnsprech" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label91" runat="server" Width="110px">Telefon1:</asp:Label>
                                                <asp:Label ID="lblHdTelefon" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label94" runat="server" Width="110px">Telefon1:</asp:Label>
                                                <asp:Label ID="lblFeTelefon" runat="server"></asp:Label>
                                            </TD>
										</TR>
										                <tr>
                                                            <td class="style2">
                                                                <asp:Label ID="Label162" runat="server" Width="110px">Telefon2:</asp:Label>
                                                                <asp:Label ID="lblHdTelefon2" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label163" runat="server" Width="110px">Telefon2:</asp:Label>
                                                                <asp:Label ID="lblFeTelefon2" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label92" runat="server" Width="110px">e-Mail:</asp:Label>
                                                <asp:Label ID="lblHdMail" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label95" runat="server" Width="110px">e-Mail:</asp:Label>
                                                <asp:Label ID="lblFeMail" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">
												<P>&nbsp;</P>
											</TD>
											<TD></TD>
										</TR>
									                                            <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label96" runat="server" Width="110px">Auslieferung:</asp:Label>
                                                                                        <asp:Label ID="lblAusTerminhinweis" runat="server"></asp:Label>
                                                                                        &nbsp;<asp:Label ID="lblAusDatum" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style2">
                                                                                        &nbsp;</td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label154" runat="server" Font-Bold="True" ForeColor="#009900" 
                                                                                            Width="188px">Details</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style2">
                                                                <asp:Label ID="Label155" runat="server" Width="135px">Wagenwäsche:</asp:Label>
                                                                <asp:Label ID="lblWaesche" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label159" runat="server" Width="110px">Servicekarte:</asp:Label>
                                                                <asp:Label ID="lblAusServicekarte" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style2">
                                                                <asp:Label ID="Label156" runat="server" Width="135px">Volltanken:</asp:Label>
                                                                <asp:Label ID="lblTanken" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label160" runat="server" Width="110px">Tankkarten:</asp:Label>
                                                                <asp:Label ID="lblAusTankkarten" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style2">
                                                                <asp:Label ID="Label157" runat="server" Width="135px">Fahrzeugeinweisung:</asp:Label>
                                                                <asp:Label ID="lblEinweisung" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style2">
                                                                <asp:Label ID="Label161" runat="server" Width="135px">Weiteres:</asp:Label>
                                                                <asp:Label ID="lblWeiteres" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                                                <tr>
                                                                                    <td class="style2">
                                                                                        &nbsp;</td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                        </tr>
                                                                                <tr><td colspan="2">
                                                                                    <asp:Label ID="lblWinterAnzeige" Visible="false" runat="server" Height="43px" Width="135px">Winterreifen:</asp:Label>
                                                                                    <asp:Label ID="lblWinterText" Visible="false" runat="server" Height="41px" Width="683px"></asp:Label>
                                                                                    </td></tr>
                                                                                      <tr><td colspan="2">
                                                                                    <asp:Label ID="Label12" runat="server" Height="43px" Width="135px">Bemerkung:</asp:Label>
                                                                                    <asp:Label ID="lblAusBemerkung" runat="server" Height="41px" Width="683px"></asp:Label>
                                                                                    </td></tr>
                                                                                    <caption>
                                                                                        &nbsp;<tr>
                                                                                            <td>
                                                                                                &nbsp;</td>
                                                                                            <td>
                                                                                                &nbsp;</td>
                                                                                        </tr>
                                                        </caption>
                                                        </tr>
                                                                                    
									</TABLE>
                                                </asp:Panel>
                                                </td></tr>
                                                <tr><td>
                                                <asp:Panel ID="pnlRueck" runat="server" Visible="False">
                                                    <TABLE id="Table13" cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD class="style1" colspan="2">
                                                Rückführung/Anschlußfahrt:</TD>
											
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:label id="Label105" runat="server" Width="180px" 
                                                    Font-Bold="True" ForeColor="#009900">Leasingnehmer</asp:label></TD>
											<TD><asp:label id="Label106" runat="server" Width="146px" Font-Bold="True" 
                                                    ForeColor="#009900">Fahrzeugnutzer</asp:label></TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label107" runat="server" Width="110px">Nr:</asp:Label>
                                                                                        <asp:Label ID="lblRLnNr" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label142" runat="server" Height="16px" Width="106px">Name:</asp:Label>
                                                                                        <asp:Label ID="lblRFnName" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
										<TR>
											<TD class="style2"><asp:label id="Label108" runat="server" Width="110px">Referenz:</asp:label>
                                                <asp:label id="lblRReferenz" runat="server"></asp:label></TD>
											<TD>
                                                <asp:Label ID="Label143" runat="server" Width="106px">Telefon:</asp:Label>
                                                <asp:Label ID="lblRFnTelefon" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD height="21" class="style2">
                                                <asp:Label ID="Label112" runat="server" Width="110px">Ansprechpartner Leasing:</asp:Label>
                                                <asp:Label ID="lblRAnsprechLeasing" runat="server"></asp:Label>
                                            </TD>
											<TD height="21">
                                                <asp:Label ID="Label144" runat="server" Width="106px">E-Mail:</asp:Label>
                                                <asp:Label ID="lblRFnMail" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">&nbsp;</TD>
											<TD>&nbsp;</TD>
										</TR>
										<TR>
											<TD class="style2">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:label id="Label114" runat="server" Width="186px" 
                                                    Font-Bold="True" ForeColor="#009900">Fahrzeugdaten</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD class="style2"><asp:label id="Label115" runat="server" Width="110px">Hersteller / Typ:</asp:label>
                                                <asp:label id="lblRFahrzeugtyp" runat="server"></asp:label></TD>
											<TD>
                                                <asp:Label ID="Label152" runat="server" Width="150px">Kennzeichen:</asp:Label>
                                                <asp:Label ID="lblRKennzeichen" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label117" runat="server" Width="110px">Fgst.-Nummer:</asp:Label>
                                                <asp:Label ID="lblRFahrgestellnummer" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label153" runat="server" Width="150px">Bereifung:</asp:Label>
                                                <asp:Label ID="lblRBereifung" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label119" runat="server" Width="110px">Fahrzeugstatus:</asp:Label>
                                                <asp:Label ID="lblRStatus" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label145" runat="server" Width="150px">Fahrzeugklasse:</asp:Label>
                                                <asp:Label ID="lblRFahrzeugKlasse" runat="server"></asp:Label>
                                            </TD>
										</TR>
										                <tr>
                                                            <td class="style2">
                                                                &nbsp;</td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label149" runat="server" Width="110px">Wunschtermin:</asp:Label>
                                                <asp:Label ID="lblRWunschtermin" runat="server"></asp:Label>
                                            </TD>
											<TD>&nbsp;</TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        &nbsp;</td>
                                                                                    <td>
                                                                                        &nbsp;</td>
                                                        </tr>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label123" runat="server" Font-Bold="True" Width="188px" 
                                                                                            ForeColor="#009900">Abholadresse</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label124" runat="server" Font-Bold="True" Width="188px" 
                                                                                            ForeColor="#009900">Anlieferadresse</asp:Label>
                                                                                    </td>
                                                                                </tr>
										                        <tr>
                                                                    <td class="style2">
                                                                        <asp:Label ID="Label125" runat="server" Width="110px">Name:</asp:Label>
                                                                        <asp:Label ID="lblRAbName" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label126" runat="server" Width="110px">Name:</asp:Label>
                                                                        <asp:Label ID="lblRAnName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label127" runat="server" Width="110px">Straße, Nr:</asp:Label>
                                                <asp:Label ID="lblRAbStrasse" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label128" runat="server" Width="110px">Straße, Nr:</asp:Label>
                                                <asp:Label ID="lblRAnStrasse" runat="server"></asp:Label>
                                            </TD>
										</TR>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label129" runat="server" Width="110px">PLZ Ort:</asp:Label>
                                                                                        <asp:Label ID="lblRAbOrt" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label130" runat="server" Width="110px">PLZ Ort:</asp:Label>
                                                                                        <asp:Label ID="lblRAnOrt" runat="server"></asp:Label>
                                                                                    </td>
                                                        </tr>
										                                        <tr>
                                                                                    <td class="style2">
                                                                                        <asp:Label ID="Label131" runat="server" Width="110px">Ansprechpartner:</asp:Label>
                                                                                        <asp:Label ID="lblRAbAnsprech" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label132" runat="server" Width="110px">Ansprechpartner:</asp:Label>
                                                                                        <asp:Label ID="lblRAnAnsprech" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label133" runat="server" Width="110px">Telefon:</asp:Label>
                                                <asp:Label ID="lblRAbTelefon" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label134" runat="server" Width="110px">Telefon:</asp:Label>
                                                <asp:Label ID="lblRAnTelefon" runat="server"></asp:Label>
                                            </TD>
										</TR>
										                <tr>
                                                            <td class="style2">
                                                                <asp:Label ID="Label150" runat="server" Width="110px">Handy:</asp:Label>
                                                                <asp:Label ID="lblRAbHandy" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label151" runat="server" Width="110px">Handy:</asp:Label>
                                                                <asp:Label ID="lblRAnHandy" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
										<TR>
											<TD class="style2">
                                                <asp:Label ID="Label135" runat="server" Width="110px">e-Mail:</asp:Label>
                                                <asp:Label ID="lblRAbMail" runat="server"></asp:Label>
                                            </TD>
											<TD>
                                                <asp:Label ID="Label136" runat="server" Width="110px">e-Mail:</asp:Label>
                                                <asp:Label ID="lblRAnMail" runat="server"></asp:Label>
                                            </TD>
										</TR>
										<TR>
											<TD class="style2">
												<P>&nbsp;</P>
											</TD>
											<TD></TD>
										</TR>
                                                                                      <tr><td colspan="2">
                                                                                    <asp:Label ID="Label139" runat="server" Height="43px" Width="110px">Bemerkung:</asp:Label>
                                                                                    <asp:Label ID="lblRBemerkung" runat="server" Height="41px" Width="683px"></asp:Label>
                                                                                    </td></tr>
                                                                                    <tr><td><P>&nbsp;</P></td></tr>
                                                                                    <caption>
                                                                                        &nbsp;</caption>
                                                        </tr>
                                                                                    
									</TABLE>
                                                </asp:Panel>
                                                    </td></tr>
									</table>
								</table>
									
                                </td>
									</td>
							</tr>
						</table>
			</form>
	</body>
</HTML>
