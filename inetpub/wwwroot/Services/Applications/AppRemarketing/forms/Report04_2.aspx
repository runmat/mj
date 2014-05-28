<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report04_2.aspx.cs" Inherits="AppRemarketing.forms.Report04_2" MasterPageFile="../Master/AppMaster.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="innerContentRightHeading">
                                <h1>
                                    <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                                </h1>
                                <span style="float: right; padding-right: 15px"><span style="margin-right: 5px">
                                    <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" height="11px" style="display:none" />
                                </span><span>
                                    <asp:LinkButton ID="lbCreatePDF" runat="server" Text="PDF herunterladen" ForeColor="White" Visible="false"></asp:LinkButton>
                                </span></span>
                            </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lbCreatePDF" />
                            </Triggers>
                        </asp:UpdatePanel>                            
                            <div>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                            </div>
                            <div style="padding-top: 20px; padding-left: 5px; margin-top: 10px">
                                <asp:Label ID="lblFahrgestellnummer" runat="server" Font-Bold="True">Fahrgestellnummer</asp:Label>
                                <asp:Label ID="lblFahrgestellnummerShow" runat="server"></asp:Label>
                                <asp:Label ID="lblKennzeichen" runat="server" Font-Bold="True">Kennzeichen</asp:Label>
                                <asp:Label ID="lblKennzeichenShow" runat="server"></asp:Label>
                                <asp:Label ID="lblBriefnummer" runat="server" Font-Bold="True">ZBII-Nr.</asp:Label>
                                <asp:Label ID="lblBriefnummerShow" runat="server"></asp:Label>
                                <asp:Label ID="lblLizNr" runat="server" Font-Bold="True">Leasingvertragsnummer</asp:Label>
                                <asp:Label ID="lblLizNrShow" runat="server"></asp:Label>
                            </div>
                            
                    <div class="DivHistTabContainer">
                        <ul class="HistTabContainer">
                            <li id="First" onclick="javascript:clickFirst()" class="HistbuttonFirst">
                                Übersicht </li>
                            <li id="Second" onclick="javascript:clickSecond()" class="Histbutton">
                                Brief/Schlüssel</li>
                            <li id="Last" onclick="javascript:clickLast()" class="HistButtonLast">
                                Typdaten </li>
                        </ul>
                    </div>
                    <div id="HistTabPanel1" class="HistTabPanel">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td colspan="6">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6"  style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="True" ForeColor="White" ID="lbl_Fahrzeugdaten" 
                                                runat="server">Übersicht allgemeine Daten</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="First">
                                            <asp:Label ID="lblModell" runat="server">Modell</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblModellShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblFarbe" runat="server">Farbe</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFarbeShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblInnenausstattung" runat="server">Innenausstattung</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblInnenausstattungShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblPrNr" runat="server">Pr.-Nr.</asp:Label>
                                        </td>
                                        <td colspan="5" >
                                            <asp:Label ID="lblPrNrShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblInventar" runat="server">Inventar</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblInventarShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblAuslieferungsdatum" runat="server">Auslieferungsdatum</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblAuslieferungsdatumShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblEingangsdatum" runat="server">Eingangsdatum</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblEingangsdatumShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblAvID" runat="server">Autovermieter-ID</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblAvIDShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblZulassungsdatum" runat="server">Zulassungsdatum</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblZulassungsdatumShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblZulassungAnAG" runat="server">Zulassung an AG</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblZulassungAnAGShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblVerlustdatum" runat="server">Verlust/Selbstvermarktung</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblVerlustdatumShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblHcEingang" runat="server">HC-Eingang</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHcEingangShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblHereinnahme" runat="server">Hereinnahmeort</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHereinnahmeOrtShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblKmStand" runat="server">KM-Stand</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblKmStandShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblSilllegung" runat="server">Stilllegung</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblStilllegungShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblImportSchaeden" runat="server">Importdatum Schäden</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblImportSchaedenShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblEreignisart" runat="server">Ereignisart</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblEreignisartShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblSchadbetragSelbstVer" runat="server">Schadensbetrag Selbstvermarktung</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblSchadbetragSelbstVerShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblRechnungUeberm" runat="server">Rechnungsübermittlung</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblRechnungUebermShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblRechnungsbetrag" runat="server">Rechnungsbetrag</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblRechnungsbetragShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblRechnungsnummer" runat="server">Rechnungsnummer</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblRechnungsnummerShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblRechnungsdatum" runat="server">Rechnungsdatum</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblRechnungsdatumShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblSchadenVorh" runat="server">Schaden vorhanden</asp:Label>
                                        </td>
                                        <td >
                                                <asp:CheckBox ID="cbxSchaden" runat="server" Enabled="False" 
                                                TextAlign="Left" />
                                            </td>
                                        <td class="First">
                                            <asp:Label ID="lblErfassungNavi" runat="server">Erfassung Navi CD</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblErfassungNaviShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblUebermSelbstverm" runat="server">Übermittlung Selbstvermarktung</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblUebermSelbstvermShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblUebermVerlust" runat="server">Übermittlung Verlust</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblUebermVerlustShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="lblSchadensmeldungAudi" runat="server">Schadensmeldung an Audi</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblSchadensmeldungAudiShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="lblMeldungAnAudi" runat="server">Meldedatum an Audi</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblMeldungAnAudiShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblBemerkung1" runat="server">Schäden 1</asp:Label>
                                        </td>
                                        <td colspan="5" >
                                            <asp:Label ID="lblBemerkung1Show" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="lblBemerkung2" runat="server">Schäden 2</asp:Label>
                                        </td>
                                        <td colspan="5" >
                                            <asp:Label ID="lblBemerkung2Show" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="True" ForeColor="White" ID="lbl_Briefdaten" 
                                                runat="server">Gutachtendaten</asp:Label>
                            
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First">
                                                <asp:Label ID="lblLaufNummer" runat="server">Laufende Nr.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLaufNummerShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblGutEingangsdatum" runat="server">Eingangsdatum</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGutEingangsdatumShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblGutEingangszeit" runat="server">Eingangszeit</asp:Label>
                                                &nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblGutEingangszeitShow" runat="server"></asp:Label>
                                                </td>
                                        </tr>
                                        <tr>
                                            <td class="First">
                                                <asp:Label ID="lblGutachter" runat="server">Gutachter</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGutachterShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblGutachtenID" runat="server">Gutachten-ID</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGutachtenIDShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblGutKMStand" runat="server">KM-Stand</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGutKMStandShow" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First">
                                                <asp:Label ID="lblGutachter0" runat="server">Gutachtendatum</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGutachtendatumShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblSchadenskennzeichen" runat="server">Schadenskennzeichen</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSchadenskennzeichenShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblSchadensbetrag" runat="server">Schadensbetrag</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSchadensbetragShow" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First">
                                                <asp:Label ID="lblRepKennzeichen" runat="server">Reparatur-Kennzeichen</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRepKennzeichenShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblWertminderungBetrag" runat="server">Wertminderungsbetrag AV</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblWertminderungBetragShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                <asp:Label ID="lblFehlbetrag" runat="server">Fehlbetrag</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFehlbetragShow" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First">
                                                <asp:Label ID="lblFehlbetragAV" runat="server">Fehlbetrag AV</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFehlbetragAVShow" runat="server"></asp:Label>
                                            </td>
                                            <td class="First">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td class="First">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        
                                    </table>
                                </div>
                                
                                <div id="HistTabPanel2"  class="HistTabPanel"   style="display: none" >
                                    <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td colspan="6">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6"  style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="True" ForeColor="White" ID="Label1" 
                                                runat="server">Rechnungsdaten</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="First">
                                            <asp:Label ID="Label2" runat="server">Externe Belegnummer</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblExBelegNrShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label4" runat="server">Rechnungsbetrag</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRechBetragShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="Label6" runat="server">Belegdatum</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBelegdatumShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label10" runat="server">Valuta-Fix-Datum</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblValutaShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label12" runat="server">Freigabedatum</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblFreigabedatumShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="Label14" runat="server">Zahlungsart</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblZahlungsartShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6"  style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="True" ForeColor="White" ID="Label3" 
                                                runat="server">Ein- und Ausgänge(Brief und Schlüssel)</asp:Label>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label15" runat="server">Eingang ZBII</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblEingangBriefShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label19" runat="server">Versand ZBII</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblVersandBriefShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            &nbsp;</td>
                                        <td >
                                            &nbsp;</td>
                                    </tr>
                                      <td  class="First">
                                            <asp:Label ID="Label18" runat="server">Eingang Schlüssel</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblEingangSchlueShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label21" runat="server">Versand Schlüssel</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblVersandSchlueShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            &nbsp;</td>
                                        <td >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="6"  style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="True" ForeColor="White" ID="Label23" 
                                                runat="server">Versandadresse Händler</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label5" runat="server">Händler</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaendlerShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label8" runat="server">Name1</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaName1Show" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="Label11" runat="server">Name2</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaName2Show" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label7" runat="server">Name3</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaName3Show" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label13" runat="server">Strasse</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaStrasseShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="Label16" runat="server">PLZ</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaPLZShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label9" runat="server">Ort</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaOrtShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label17" runat="server">Land</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblHaLandShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            &nbsp;</td>
                                        <td >
                                            &nbsp;</td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td colspan="6"  style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="True" ForeColor="White" ID="Label20" 
                                                runat="server">Versandadresse Bank</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label22" runat="server">Name1</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBaName1Show" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label25" runat="server">Name2</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBaName2Show" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="Label27" runat="server">Name3</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBaName3Show" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label29" runat="server">Strasse</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBaStrasseShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            <asp:Label ID="Label31" runat="server">PLZ</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBaPLZShow" runat="server"></asp:Label>
                                        </td>
                                        <td  class="First">
                                            <asp:Label ID="Label33" runat="server">Ort</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBaOrtShow" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="First">
                                            <asp:Label ID="Label35" runat="server">Land</asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblBaLandShow" runat="server"></asp:Label>
                                        </td>
                                        <td class="First">
                                            &nbsp;</td>
                                        <td >
                                            &nbsp;</td>
                                        <td  class="First">
                                            &nbsp;</td>
                                        <td >
                                            &nbsp;</td>
                                    </tr>
                                    </table>
                                    
                                    
                                   
                                </div>
                                <div id="HistTabPanel3"  class="HistTabPanel"  style="display: none">
                                               <table id="Table4" width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                        <td colspan="6">
                                            &nbsp;
                                        </td>                                            
                                        </tr>                                    
                                        <tr>
                                        <td colspan="6"  style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblAllgFahrzeugdaten" runat="server"> Allgemeine Fahrzeugdaten</asp:Label>
                                        </td>                                            
                                        </tr>
                                        <tr>
                                            <td class="First" align="right" width="63" >
                                                1
                                            </td>
                                            <td  class="First"  nowrap>
                                                Fahrzeugklasse&nbsp;/ Aufbau:
                                            </td>
                                            <td class="ABEDaten" nowrap colspan="3" rowspan="1">
                                                <asp:Label ID="lbl_6" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%" height="19">
                                                <asp:Label ID="lbl_7" runat="server">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                2
                                            </td>
                                            <td  class="First" nowrap>
                                                Hersteller / Schlüssel:
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_1" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                                <asp:Label ID="lbl_2" runat="server">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                3
                                            </td>
                                            <td  class="First" nowrap>
                                                Handelsname / Farbe:
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_3" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                                <asp:Label ID="lbl_55" runat="server" BorderWidth="1px" BackColor="Black" ForeColor="White"
                                                    BorderColor="White">-</asp:Label><asp:Label ID="lbl_91" runat="server" BorderWidth="1px"
                                                        BackColor="SaddleBrown" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label
                                                            ID="lbl_92" runat="server" BorderWidth="1px" BackColor="DimGray" ForeColor="Black"
                                                            BorderColor="Black">-</asp:Label><asp:Label ID="lbl_93" runat="server" BorderWidth="1px"
                                                                BackColor="Green" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label ID="lbl_94"
                                                                    runat="server" BorderWidth="1px" BackColor="RoyalBlue" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label
                                                                        ID="lbl_95" runat="server" BorderWidth="1px" BackColor="Magenta" ForeColor="Black"
                                                                        BorderColor="Black">-</asp:Label><asp:Label ID="lbl_96" runat="server" BorderWidth="1px"
                                                                            BackColor="Red" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label ID="lbl_97"
                                                                                runat="server" BorderWidth="1px" BackColor="OrangeRed" ForeColor="Black" BorderColor="Black">-</asp:Label><asp:Label
                                                                                    ID="lbl_98" runat="server" BorderWidth="1px" BackColor="Yellow" ForeColor="Black"
                                                                                    BorderColor="Black">-</asp:Label><asp:Label ID="lbl_99" runat="server" BorderWidth="1px"
                                                                                        BackColor="White" ForeColor="Black" BorderColor="Black">-</asp:Label>&nbsp;<asp:Label
                                                                                            ID="lbl_00" runat="server" BackColor="Transparent">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                4
                                            </td>
                                            <td  class="First" nowrap>
                                                Genehmigungs-Datum / Nr.&nbsp;
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_5" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                                <asp:Label ID="lbl_4" runat="server">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63" height="23">
                                                5
                                            </td>
                                            <td  class="First" nowrap height="23">
                                                Typ / Schlüssel
                                            </td>
                                            <td class="ABEDaten" colspan="3" height="23">
                                                <asp:Label ID="lbl_0" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%" height="23">
                                                <asp:Label ID="lbl_29" runat="server">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63" height="22">
                                                6
                                            </td>
                                            <td  class="First" nowrap height="22">
                                                Fabrikname:
                                            </td>
                                            <td class="ABEDaten" colspan="3" height="22">
                                                <asp:Label ID="lbl_8" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                7
                                            </td>
                                            <td  class="First" nowrap>
                                                Variante / Version:
                                            </td>
                                            <td  class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_9" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                                <asp:Label ID="lbl_10" runat="server">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                8
                                            </td>
                                            <td  class="First" nowrap>
                                                Anzahl Sitze:
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_26" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                9
                                            </td>
                                            <td  class="First" nowrap>
                                                Zul. Gesamtgewicht (kg):
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_28" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                10
                                            </td>
                                            <td class="First"  nowrap>
                                                Länge min. (mm)
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_31" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                11
                                            </td>
                                            <td  class="First" nowrap>
                                                Breite min. (mm)
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_32" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                12
                                            </td>
                                            <td class="First" nowrap>
                                                Höhe min. (mm)
                                            </td>
                                            <td  class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_33" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;
                                            </td>
                                        </tr>
<tr>
                                        <td colspan="6"  style="background-color: #9C9C9C">
                                            <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblAntriebsdaten" runat="server">Antriebsdaten</asp:Label>
                                        </td>                                                   
  
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                1
                                            </td>
                                            <td class="First"  nowrap>
                                                Hubraum (cm³)
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_11" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                2
                                            </td>
                                            <td  class="First" nowrap>
                                                Nennleistung (KW) bei U/Min:
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_13" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td  class="ABEDaten" align="left" width="100%">
                                                <asp:Label ID="lbl_14" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                3
                                            </td>
                                            <td  class="First" nowrap>
                                                Höchstgeschwindigkeit (km/h):
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_12" runat="server">-&nbsp;</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                4
                                            </td>
                                            <td  class="First" nowrap>
                                                Stand- / Fahrgeräusch (db):
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_19" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                                <asp:Label ID="lbl_20" runat="server">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6"  style="background-color: #9C9C9C">
                                                <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblKraftstoff" runat="server"> Kraftstoff / Tank</asp:Label>
                                                   
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                1
                                            </td>
                                            <td class="First"  nowrap>
                                                Art / Code:
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_15" runat="server">-</asp:Label>
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                                <asp:Label ID="lbl_16" runat="server">-</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                2
                                            </td>
                                            <td class="First"  nowrap>
                                                Fassungsvermögen Tank (m³):
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_21" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63" height="22">
                                                3
                                            </td>
                                            <td class="First"  nowrap height="22">
                                                Co2-Gehalt (g/km):
                                            </td>
                                            <td class="ABEDaten" colspan="3" height="22">
                                                <asp:Label ID="lbl_17" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63" height="22">
                                                4
                                            </td>
                                            <td  class="First" nowrap height="22">
                                                Nat. Emissionsklasse:
                                            </td>
                                            <td class="ABEDaten" colspan="3" height="22">
                                                <asp:Label ID="lbl_18" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63" height="22">
                                                5
                                            </td>
                                            <td  class="First" nowrap height="22">
                                                Abgasrichtlinie:
                                            </td>
                                            <td class="ABEDaten" colspan="3" height="22">
                                                <asp:Label ID="lbl_22" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="6"  style="background-color: #9C9C9C">
                                                                                                  
                                               <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblAchsen" runat="server"> Achsen / Bereifung</asp:Label>
                                                  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                1
                                            </td>
                                            <td  class="First" nowrap>
                                                Anzahl Achsen:
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_23" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td  class="First" align="right" width="63">
                                                2
                                            </td>
                                            <td  class="First" nowrap>
                                                Anzahl Antriebsachsen:
                                            </td>
                                            <td class="ABEDaten" colspan="3">
                                                <asp:Label ID="lbl_24" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            <td class="ABEDaten" align="left" width="100%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                3
                                            </td>
                                            <td class="First"  nowrap>
                                                Max. Achslast Achse 1,2,3 (kg):
                                            </td>
                                            <td class="ABEDaten" colspan="4">
                                                <asp:Label ID="lbl_25" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td class="First"  align="right" width="63">
                                                4
                                            </td>
                                            <td class="First"  nowrap>
                                                Bereifung Achse 1,2,3:
                                            </td>
                                            <td class="ABEDaten" colspan="4">
                                                <asp:Label ID="lbl_27" runat="server">-</asp:Label>&nbsp;
                                            </td>
                                          
                                        </tr>
                                        <tr>
           
                                            <td colspan="6"  style="background-color: #9C9C9C">
                                                                                                  
                                               <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblBemerkungen" runat="server"> Bemerkungen</asp:Label>
                                                  
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td class="First"  valign="top" align="right">
                                               <asp:Label ID="lbl_AnzBemerkungen2" runat="server"></asp:Label>
                                            </td>

                                            <td class="ABEDaten" colspan="5">
                                                <asp:Label ID="lbl_30" runat="server" Font-Names="Arial" Font-Size="XX-Small">-</asp:Label>
                                            </td>
                                        </tr>
                                    </table>                      
                                 
                                </div>

                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" >

        function clickFirst() 
        {
            document.getElementById("First").className = "HistbuttonFirst";
            document.getElementById("Second").className = "Histbutton";
            document.getElementById("Last").className = "HistButtonLast";
            document.getElementById("HistTabPanel1").style.display = "block";
            document.getElementById("HistTabPanel2").style.display = "none";
            document.getElementById("HistTabPanel3").style.display = "none";
        }
        function clickSecond() {
            document.getElementById("First").className = "HistButtonBeforActive";
            document.getElementById("Second").className = "HistButtonMiddleActive";
            document.getElementById("Last").className = "HistButtonLast";
            document.getElementById("HistTabPanel1").style.display = "none";
            document.getElementById("HistTabPanel2").style.display = "block";
            document.getElementById("HistTabPanel3").style.display = "none";
        }


        function clickLast() {
            document.getElementById("First").className = "Histbutton";
            document.getElementById("Second").className = "HistButtonBeforActive";
            document.getElementById("Last").className = "HistButtonLastActive";
            document.getElementById("HistTabPanel1").style.display = "none";
            document.getElementById("HistTabPanel2").style.display = "none";
            document.getElementById("HistTabPanel3").style.display = "block";
        }              
    </script>
</asp:Content>