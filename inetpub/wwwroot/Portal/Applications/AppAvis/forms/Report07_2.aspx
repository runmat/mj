<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report07_2.aspx.vb" Inherits="AppAvis.Report07_2" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	   
        </style>
	    <style type="text/css">
            .StandardButton
            {}
            #Table5
            {
                width: 100%;
            }
            #Table6
            {
                width: 133%;
            }
            .style1
            {
                width: 100px;
            }
            .style2
            {
                width: 100px;
            }
            .style3
            {
                width: 170px;
            }
            </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Briefdaten)</asp:label><asp:hyperlink id="lnkSchluesselinformationen" runat="server" Target="_blank" Visible="False" NavigateUrl="Report38.aspx?chassisnum=">Schlüsselinformationen</asp:hyperlink></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"  Height="15px" Width="100px"> •&nbsp;Zurück</asp:LinkButton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
                                        
                                                &nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">&nbsp;</TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="5" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;
												<asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx" CssClass="TaskTitle">Abfragekriterien</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left">
															<table border="0" cellpadding="0" cellspacing="0" width="800">
																<tr>
																	<td align="middle">
																		<TABLE id="Table110" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Fahrgestellnummer" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Fahrgestellnummer" runat="server">Fahrgestellnummer:</asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblFahrgestellnummerShow" runat="server"></asp:label></td>
																			</tr>
																		</TABLE>
																	</td>
																	<td align="middle">
																		<TABLE id="Table111" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Kennzeichen" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Kennzeichen" runat="server">Kennzeichen:</asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblKennzeichenShow" runat="server"></asp:label></td>
																			</tr>
																		</TABLE>
																	</td>
																	<td align="middle">
																		<TABLE id="Table112" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Status" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Status" runat="server">Status :</asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblStatusShow" runat="server"></asp:label></td>
																			</tr>
																		</TABLE>
																	</td>
																	<td align="middle">
																		<TABLE id="Table113" cellSpacing="0" cellPadding="5" align="left" bgColor="white" border="0">
																			<tr id="tr_Lagerort" runat="server">
																				<td class="TextLarge" noWrap align="right"><asp:Label id="lbl_Lagerort" runat="server">Lagerort:</asp:Label>&nbsp;</td>
																				<td class="TextLargeDescription" noWrap align="left">&nbsp;<asp:label id="lblLagerortShow" runat="server"></asp:label></td>
																				<td class="TextLargeDescription" noWrap align="center">&nbsp;</td>
																			</tr>
																		</TABLE>
																	</td>
																</tr>
															</table>
														</TD>
													</TR>
                                                    <tr id="trUebersicht2" runat="server">
                                                        <td width="800">
                                                            <table id="Table5" cellspacing="0" cellpadding="5" bgcolor="white" border="0">
                                                                <tr>
                                                                    <td align="left"width="20%"  >
                                                                        <asp:ImageButton
                                                                    ID="lnkCreateExcel" ImageUrl="~/Images/iconPDF.gif" runat="server" Height="14px"
                                                                    Width="14px" ImageAlign="Top" Visible="False"></asp:ImageButton>
                                                                        <asp:LinkButton ID="cmdPrint" class="TextLarge" Visible="False" runat="server">PDF-Format</asp:LinkButton>
                                                                    </td>
                                                                    <td colspan="0" align="right" width="80%" >
                                                                        <asp:LinkButton ID="lb_Uebersicht" runat="server" CssClass="ButtonActive">Übersicht</asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_Typdaten" runat="server" CssClass="StandardButton">Typdaten</asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_Lebenslauf" runat="server" CssClass="StandardButton">Lebenslauf</asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_Uebermittlung" runat="server" CssClass="StandardButton">Übermittlung</asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_Schluessel" runat="server" CssClass="StandardButton">Schlüsselinformationen</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="TaskTitle">
                                                                        &nbsp;</td>
                                                                    <td class="TaskTitle">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="trUebersicht" runat="server" >
                                                        <td valign="top" align="left" width="800">
                                                            <table id="Table10" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_Hersteller" runat="server">ID / Hersteller</asp:Label>
                                                                        :</td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblHerstellerShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_Fahrzeugmodell" runat="server">TypID / Modell</asp:Label>
                                                                        :</td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblFahrzeugmodellShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_Farbe" runat="server">Farbe:</asp:Label>
                                                                    </td>
                                                                    <td valign="top" nowrap align="left">
                                                                        <asp:Label ID="lbl_155" runat="server" BorderWidth="1px" BackColor="Black" ForeColor="White"
                                                                            BorderColor="White">-</asp:Label><asp:Label ID="lbl_191" runat="server" BorderWidth="1px"
                                                                                BackColor="SaddleBrown" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label
                                                                                    ID="lbl_192" runat="server" BorderWidth="1px" BackColor="DimGray" ForeColor="Black"
                                                                                    BorderColor="Black">-</asp:Label><asp:Label ID="lbl_193" runat="server" BorderWidth="1px"
                                                                                        BackColor="Green" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label ID="lbl_194"
                                                                                            runat="server" BorderWidth="1px" BackColor="RoyalBlue" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label
                                                                                                ID="lbl_195" runat="server" BorderWidth="1px" BackColor="Magenta" ForeColor="Black"
                                                                                                BorderColor="Black">-</asp:Label><asp:Label ID="lbl_196" runat="server" BorderWidth="1px"
                                                                                                    BackColor="Red" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label ID="lbl_197"
                                                                                                        runat="server" BorderWidth="1px" BackColor="OrangeRed" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label
                                                                                                            ID="lbl_198" runat="server" BorderWidth="1px" BackColor="Yellow" ForeColor="Black"
                                                                                                            BorderColor="Black">-</asp:Label><asp:Label ID="lbl_199" runat="server" BorderWidth="1px"
                                                                                                                BackColor="White" ForeColor="Black" BorderColor="Black">-</asp:Label>&nbsp;<asp:Label
                                                                                                                    ID="lbl_200" runat="server" BackColor="Transparent">-</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="StandardTableAlternateDescription" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_HerstellerSchluessel" runat="server">Herstellerschlüssel</asp:Label>
                                                                        :</td>
                                                                    <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblHerstellerSchluesselShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternateDescription" valign="center" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternateDescription" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_Typschluessel" runat="server">Typschlüssel</asp:Label>
                                                                        :</td>
                                                                    <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblTypschluesselShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternateDescription" valign="center" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternateDescription" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_VarianteVersion" runat="server">Variante/Version:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblVarianteVersionShow" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="top" align="left">
                                                                        Eingang :
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" align="right">
                                                                        <asp:Label ID="lblEingangsdatum" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" align="left">
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" align="left">
                                                                        Carport Nr./ -Name:
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" align="right">
                                                                        <strong>
                                                                            <asp:Label ID="lblPDI" runat="server"></asp:Label>
                                                                            <asp:Label ID="lblPDIName" runat="server"></asp:Label></strong>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" nowrap valign="top" align="left">
                                                                        Bereitdatum:
                                                                    </td>
                                                                    <td class="StandardTableAlternate" nowrap align="left">
                                                                        <strong>
                                                                            <asp:Label ID="lblBereitdatum" runat="server"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="TaskTitle" colspan="8">
                                                                        <asp:Label ID="lbl_Briefdaten" runat="server">ZBII-Daten</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_Briefnummer" runat="server">ZBII Nummer:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblBriefnummerShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left" width="152">
                                                                        <asp:Label ID="lbl_Erstzulassungsdatum" runat="server">Erstzulassungsdatum:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblErstzulassungsdatumShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_Abmeldedatum" runat="server">Abmeldedatum:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblAbmeldedatumShow" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="" valign="top" nowrap align="left">
                                                                        <asp:Label ID="lbl_Ordernummer" runat="server">MVA-Nr.:</asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblOrdernummerShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="left" width="152">
                                                                        <asp:Label ID="lbl_AktZulassungsdatum" runat="server">Aktuelle Zulassung:</asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblAktZulassungsdatumShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="left">
                                                                        <asp:Label ID="lbl_Produktionskennziffer" runat="server" 
                                                                            Text="Produktionskennziffer:"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblProduktionskennzifferShow" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        <asp:Label ID="lbl_CoC" runat="server">COC:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:CheckBox ID="cbxCOC" runat="server" TextAlign="Left" Enabled="False"></asp:CheckBox>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left" width="152">
                                                                        &nbsp;
                                                                        <asp:Label ID="lbl_Fahrzeughalter" runat="server">Fahrzeughalter:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblFahrzeughalterShow" runat="server"></asp:Label>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="TaskTitle" colspan="8">
                                                                        <asp:Label ID="lbl_Aenderungsdaten" runat="server">Änderungsdaten</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="" valign="center" nowrap align="left" width="152">
                                                                        <asp:Label ID="lbl_UmgemeldetAm" runat="server">Umgemeldet am:</asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblUmgemeldetAmShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                    </td>
                                                                    <td class="" valign="center"  align="left">
                                                                        <asp:Label ID="lbl_EhemaligesKennzeichen" runat="server">Ehemaliges Kennzeichen:</asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top"  align="right">
                                                                        <asp:Label ID="lblEhemaligesKennzeichenShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left" width="152">
                                                                        <asp:Label ID="lbl_Briefaufbietung" runat="server">ZBII-Aufbietung:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:CheckBox ID="chkBriefaufbietung" runat="server" Enabled="False"></asp:CheckBox>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center"  align="left">
                                                                        <asp:Label ID="lbl_EhemaligeBriefnummer" runat="server">Ehemalige ZBII Nummer:</asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblEhemaligeBriefnummerShow" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="" valign="center" nowrap align="left" width="152">
                                                                        Datum Sperre bis</td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblSperrebis" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="TaskTitle" colspan="8">
                                                                        <asp:Label ID="Label6" runat="server">Abmeldedaten</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="" valign="center" nowrap align="left" width="152">
                                                                        Carport-Eingang:
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblPDIEingang" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        Kennzeicheneingang:
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblKennzeicheneingang" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        Check-In:
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblCheckIn" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="left" width="152">
                                                                        Fahrzeugschein:
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:CheckBox ID="chkFahzeugschein" runat="server" Enabled="False" />
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top"  align="left">
                                                                        Beide Kennzeichen vorhanden:
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:CheckBox ID="chkVorhandeneElemente" runat="server" Enabled="False" />
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="left">
                                                                        Stilllegung:
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblStillegung" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="TaskTitle" colspan="8">
                                                                        <asp:Label ID="Label7" runat="server">Letzte Versandaten</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="" valign="center" nowrap align="left" width="152">
                                                                        <asp:Label ID="lblVersendetAmDescription" runat="server">Versendet am:</asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblAngefordertAm" runat="server"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblVersendetAm" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        Versandanschrift:
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblStandortShow" runat="server"></asp:Label>
                                                                        <br />
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        &nbsp;</td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="TaskTitle" colspan="8">
                                                                        <asp:Label ID="Label4" runat="server">Treuhanddaten</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="" valign="center" nowrap align="left" width="152">
                                                                        <asp:Label ID="Label5" runat="server">Treuhandsperre:</asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:CheckBox ID="cbTreuhandsperre" runat="server" Enabled="False" />
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        Treugeber:
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lbTreugeber" runat="server"></asp:Label><br />
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
													<TR id="trTypdaten" runat="server">
														<TD vAlign="top" align="left" width="800"><TABLE id="Table4" cellSpacing="0" 
                                                                cellPadding="2" bgColor="white" border="0" >
																<TR>
																	<TD align="right" height="19">1</TD>
																	<TD  noWrap height="19">Fahrzeugklasse&nbsp;/ 
																		Aufbau:</TD>
																	<TD  noWrap rowSpan="1"><asp:label id="lbl_6" runat="server">-</asp:label></TD>
																	<TD  align="left" height="19"><asp:label id="lbl_7" runat="server">-</asp:label></TD>
																	<TD  align="left" height="19">&nbsp;</TD>
																</TR>
																<TR>
																	<TD  align="right">2</TD>
																	<TD  noWrap>Hersteller / Schlüssel:</TD>
																	<TD ><asp:label id="lbl_1" runat="server">-</asp:label></TD>
																	<TD  align="left"><asp:label id="lbl_2" runat="server">-</asp:label></TD>
																	<TD  align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD  align="right">3</TD>
																	<TD  noWrap>Handelsname / Farbe:
																	</TD>
																	<TD class="" noWrap><asp:label id="lbl_3" runat="server">-</asp:label></TD>
																	<TD class="" noWrap align="left"><asp:label id="lbl_55" runat="server" BorderWidth="1px" BackColor="Black" ForeColor="White" BorderColor="White">-</asp:label><asp:label id="lbl_91" runat="server" BorderWidth="1px" BackColor="SaddleBrown" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_92" runat="server" BorderWidth="1px" BackColor="DimGray" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_93" runat="server" BorderWidth="1px" BackColor="Green" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_94" runat="server" BorderWidth="1px" BackColor="RoyalBlue" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_95" runat="server" BorderWidth="1px" BackColor="Magenta" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_96" runat="server" BorderWidth="1px" BackColor="Red" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_97" runat="server" BorderWidth="1px" BackColor="OrangeRed" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_98" runat="server" BorderWidth="1px" BackColor="Yellow" ForeColor="Black" BorderColor="Black">-</asp:label><asp:label id="lbl_99" runat="server" BorderWidth="1px" BackColor="White" ForeColor="Black" BorderColor="Black">-</asp:label>&nbsp;<asp:label id="lbl_00" runat="server" BackColor="Transparent">-</asp:label></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">4</TD>
																	<TD class="" noWrap>Genehmigungs-Datum / Nr.&nbsp;</TD>
																	<TD class=""><asp:label id="lbl_5" runat="server">-</asp:label></TD>
																	<TD class="" align="left"><asp:label id="lbl_4" runat="server">-</asp:label></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right" height="23">5</TD>
																	<TD class="" noWrap height="23">Typ / Schlüssel</TD>
																	<TD class="" height="23"><asp:label id="lbl_0" runat="server">-</asp:label></TD>
																	<TD class="" align="left" height="23"><asp:label id="lbl_29" runat="server">-</asp:label></TD>
																	<TD class="" align="left" height="23">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right" height="22">6</TD>
																	<TD class="" noWrap height="22">Fabrikname:</TD>
																	<TD class="" height="22"><asp:label id="lbl_8" runat="server">-</asp:label></TD>
																	<TD class="" align="left" height="22"></TD>
																	<TD class="" align="left" height="22">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">7</TD>
																	<TD class="" noWrap>Variante / Version:</TD>
																	<TD class=""><asp:label id="lbl_9" runat="server">-</asp:label></TD>
																	<TD class="" align="left"><asp:label id="lbl_10" runat="server">-</asp:label></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">8</TD>
																	<TD class="" noWrap>Anzahl Sitze:</TD>
																	<TD class=""><asp:label id="lbl_26" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">9</TD>
																	<TD class="" noWrap>Zul. Gesamtgewicht (kg):</TD>
																	<TD class=""><asp:label id="lbl_28" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">10</TD>
																	<TD class="" noWrap>Länge min. (mm)</TD>
																	<TD class=""><asp:label id="lbl_31" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">11</TD>
																	<TD class="" noWrap>Breite min. (mm)</TD>
																	<TD class=""><asp:label id="lbl_32" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" noWrap>12</TD>
																	<TD class="">Höhe min. (mm)</TD>
																	<TD class="" align="left"><asp:label id="lbl_33" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>																	
																</TR>
																<TR>
																	<TD class="TaskTitle" align="left" colSpan="4">Antriebsdaten</TD>
																	<TD class="TaskTitle" align="left" width="100%">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">1</TD>
																	<TD class="" noWrap>Hubraum (cm³)</TD>
																	<TD class=""><asp:label id="lbl_11" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">2</TD>
																	<TD class="" noWrap>Nennleistung (KW) bei U/Min:</TD>
																	<TD class=""><asp:label id="lbl_13" runat="server">-</asp:label></TD>
																	<TD class="" align="left"><asp:label id="lbl_14" runat="server">-</asp:label></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">3</TD>
																	<TD class="" noWrap>Höchstgeschwindigkeit (km/h):</TD>
																	<TD class=""><asp:label id="lbl_12" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">4</TD>
																	<TD class="" noWrap>Stand- / Fahrgeräusch (db):</TD>
																	<TD class=""><asp:label id="lbl_19" runat="server">-</asp:label></TD>
																	<TD class="" align="left"><asp:label id="lbl_20" runat="server">-</asp:label></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="TaskTitle" noWrap align="left" colSpan="4">Kraftstoff / Tank</TD>
																	<TD class="TaskTitle" noWrap align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">1</TD>
																	<TD class="" noWrap>Art / Code:</TD>
																	<TD class=""><asp:label id="lbl_15" runat="server">-</asp:label></TD>
																	<TD class="" align="left"><asp:label id="lbl_16" runat="server">-</asp:label></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">2</TD>
																	<TD class="" noWrap>Fassungsvermögen Tank (m³):</TD>
																	<TD class=""><asp:label id="lbl_21" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right" height="22">3</TD>
																	<TD class="" noWrap height="22">Co2-Gehalt (g/km):</TD>
																	<TD class="" height="22"><asp:label id="lbl_17" runat="server">-</asp:label></TD>
																	<TD class="" align="left" height="22"></TD>
																	<TD class="" align="left" height="22">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right" height="22">4</TD>
																	<TD class="" noWrap height="22">Nat. 
																		Emissionsklasse:</TD>
																	<TD class="" height="22"><asp:label id="lbl_18" runat="server">-</asp:label></TD>
																	<TD class="" align="left" height="22"></TD>
																	<TD class="" align="left" height="22">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right" height="22">5</TD>
																	<TD class="" noWrap height="22">Abgasrichtlinie:</TD>
																	<TD class="" height="22"><asp:label id="lbl_22" runat="server">-</asp:label></TD>
																	<TD class="" align="left" height="22"></TD>
																	<TD class="" align="left" height="22">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="left" colSpan="4">Achsen / Bereifung</TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">1</TD>
																	<TD class="" noWrap>Anzahl Achsen:</TD>
																	<TD class=""><asp:label id="lbl_23" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">2</TD>
																	<TD class="" noWrap>Anzahl Antriebsachsen:</TD>
																	<TD class=""><asp:label id="lbl_24" runat="server">-</asp:label></TD>
																	<TD class="" align="left"></TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">3</TD>
																	<TD class="" noWrap>Max. Achslast Achse 1,2,3 
																		(kg):</TD>
																	<TD class="" colSpan="2"><asp:label id="lbl_25" runat="server">-</asp:label></TD>
																	<TD class="">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="right">4</TD>
																	<TD class="" noWrap>Bereifung Achse 1,2,3:</TD>
																	<TD class="" colSpan="2"><asp:label id="lbl_27" runat="server">-</asp:label></TD>
																	<TD class="">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" align="left" colSpan="4">Bemerkungen</TD>
																	<TD class="" align="left">&nbsp;</TD>
																</TR>
																<TR>
																	<TD class="" vAlign="top" align="right">1</TD>
																	<TD class="" noWrap></TD>
																	<TD class="" colSpan="2"><asp:label id="lbl_30" runat="server" Font-Names="Arial" Font-Size="XX-Small">-</asp:label></TD>
																	<TD class="">&nbsp;</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR id="trLebenslauf" runat="server">
														<TD vAlign="top" align="left"><asp:datagrid id="Datagrid2" runat="server" BackColor="White" AutoGenerateColumns="False" Width="800px" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<Columns>
																	<asp:BoundColumn DataField="KURZTEXT" SortExpression="KURZTEXT" HeaderText="Vorgang"></asp:BoundColumn>
																	<asp:BoundColumn DataField="STRMN" SortExpression="STRMN" HeaderText="Durchf&#252;hrungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Versandadresse">
																		<ItemTemplate>
																			<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
																			</asp:Label>
																			<asp:Literal id="Literal1" runat="server" Text="<br>"></asp:Literal>
																			<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
																			</asp:Label>
																			<asp:Literal id="Literal2" runat="server" Text="<br>"></asp:Literal>
																			<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn Visible="False" DataField="ERDAT" SortExpression="ERDAT" HeaderText="Erfassungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ZZDIEN1" SortExpression="ZZDIEN1" HeaderText="Versandart"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ERNAM" SortExpression="ERNAM" HeaderText="Beauftragt&lt;br&gt;durch"></asp:BoundColumn>
																	<asp:BoundColumn Visible="False" DataField="QMNUM" SortExpression="QMNUM" HeaderText="Meldungsnummer"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
													<TR id="trUebermittlung" runat="server">
														<TD vAlign="top" align="left"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" Width="800px" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="TaskTitle"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="MNCOD" SortExpression="MNCOD" HeaderText="Aktionscode"></asp:BoundColumn>
																	<asp:BoundColumn DataField="MATXT" SortExpression="MATXT" HeaderText="Vorgang"></asp:BoundColumn>
																	<asp:BoundColumn DataField="PSTER" SortExpression="PSTER" HeaderText="Statusdatum"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ZZUEBER" SortExpression="ZZUEBER" HeaderText="&#220;bermittlungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:BoundColumn Visible="False" DataField="AEZEIT" SortExpression="AEZEIT" HeaderText="&#196;nderungs-&lt;br&gt;Zeit" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
													</TR>

													<tr id="trZweit" runat="server" >
                                                        <td valign="top" align="left" width="760px">
                                                            <table id="Table6" cellspacing="0" cellpadding="5" width="760px" bgcolor="white" border="0"  style="width:100%">
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left" style="width:100px">
                                                                        <asp:Label ID="Label8" runat="server">Fahrgestellnummer</asp:Label>
                                                                        :</td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right" style="width:100px">
                                                                        <asp:Label ID="lblFahrgestellnummer" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left" style="width:20px">
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left" style="width:100px">
                                                                        <asp:Label ID="Label10" runat="server" CssClass="StandardTableAlternate">Eingang Schlüssel</asp:Label>
                                                                        :</td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right" style="width:100px">
                                                                        <asp:Label ID="lblEingangSchluessel" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="TaskTitle" valign="center" align="left" >
                                                                        &nbsp;</td>
                                                                    <td class="TaskTitle" valign="top" nowrap align="right">
                                                                        &nbsp;</td>
                                                                    <td class="TaskTitle" valign="center" nowrap align="left" colspan="0">
                                                                        &nbsp;</td>
                                                                    <td class="TaskTitle" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="TaskTitle" valign="top" nowrap align="right">
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="style2" valign="center" align="left" >
                                                                        <asp:Label ID="Label11" runat="server">Versendet am:</asp:Label>
                                                                    </td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblAngefordertAm0" runat="server"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="Label13" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                    <td class="" valign="center" nowrap align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="style1" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="" valign="top" nowrap align="right">
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="StandardTableAlternate" valign="top" align="left" >
                                                                        Versandanschrift:
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <asp:Label ID="lblVersandanschrift0" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="left">
                                                                        &nbsp;</td>
                                                                    <td class="StandardTableAlternate" valign="top" nowrap align="right">
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
													</tr>

													</TABLE>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="120"></TD>
								<TD><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
			<!--#include File="../../../PageElements/Footer.html" --></form>
	</body>
</HTML>
