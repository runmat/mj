<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report14_2.aspx.cs" Inherits="AppRemarketing.forms.Report14_2" MasterPageFile="../Master/AppMaster.Master" %>

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
                                &nbsp;
                                <asp:Label ID="lblEreignisartShow" runat="server"></asp:Label>
                                <%--<asp:Label ID="lblLizNr" runat="server" Font-Bold="True">Leasingvertragsnummer</asp:Label>
                                <asp:Label ID="lblLizNrShow" runat="server"></asp:Label>--%>
                            </div>
                            
                    <div class="DivHistTabContainer">
                        <ul class="HistTabContainer">
                            <li id="Tab1" onclick="javascript:click1()" class="HistbuttonFirst">
                                Übersicht </li>
                            <li id="Tab2" onclick="javascript:click2()" class="Histbutton">
                                Brief/Schlüssel</li>
                            <li id="Tab3" onclick="javascript:click3()" class="Histbutton">
                                Typdaten </li>
                            <li id="Tab4" onclick="javascript:click4()" class="Histbutton">
                                Lebenslauf </li>
                            <li id="Tab5" onclick="javascript:click5()" class="Histbutton">
                                Vorschäden </li>
                            <li id="Tab6" onclick="javascript:click6()" class="HistButtonLast">
                                Modell/Ausstatt.&nbsp;&nbsp;&nbsp; </li>
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
                                <td class="First">Autovermieter</td>
                                <td><%# Uebersicht.Vermieter %></td>
                                <td class="First">Auslieferungsdatum</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.Auslieferung) %>&nbsp;</td>
                                <td class="First">Zulassungsdatum</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.Zulassung) %>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="First">Verlust/Selbstvermarktung</td>
                                <td><%# Uebersicht.Vermarktung?"Ja":"Nein" %></td>
                                <td class="First">Vorschäden</td>
                                <td><%# Uebersicht.Vorschaeden?"Ja":"Nein" %></td>
                                <td class="First">UPE-Preis</td>
                                <td ><%# Uebersicht.UPEPreis != null ? string.Format("{0:c} &#8364;", Uebersicht.UPEPreis) : "&nbsp;"%></td>
                            </tr>
                            <tr>
                                <td class="First">HC-Eingang</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.HCEingang) %>&nbsp;</td>
                                <td class="First">Hereinnahmecenter</td>
                                <td><%# Uebersicht.HC %>&nbsp;</td>
                                <td class="First">KM-Stand</td>
                                <td><%# string.Format("{0:#,###}", Uebersicht.KM) %>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="First">ZB II Eingang</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.ZBEingang) %>&nbsp;</td>
                                <td class="First">Schlüsseleingang</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.SchlEingang) %>&nbsp;</td>
                                <td class="First">Vertragsjahr</td>
                                <td><%# string.Format("{0:####}", Uebersicht.Vertragsjahr) %>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="First">Datum Vertragswidrigkeit</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.VertragswidrigkeitDate) %>&nbsp;</td>
                                <td class="First">Art Vertragswidrigkeit</td>
                                <td><%# Uebersicht.VertragswidrigkeitArt %>&nbsp;</td>
                                <td class="First">TÜV Rückmeldung</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.TuevRueckmeldung) %>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="First">Mietfahrzeug Abrechnungsdatum</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.MietfzgAbrechnungsdatum) %>&nbsp;</td>
                                <td class="First">Mietfahrzeug Rückkaufrechnung erstellt</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.MietfzgRueckkaufrechnung) %>&nbsp;</td>
                                <td class="First">TÜV manuell beauftragt</td>
                                <td><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.TuevManuellBeauftragt) %>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="True" ForeColor="White" ID="Label24" 
                                        runat="server">Belastungsanzeige</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">Laufende Nr.</td>
                                <td><%# Belastungsanzeige!=null?Belastungsanzeige.LfdNo:"&nbsp;" %></td>
                                <td class="First">Erstellungsdatum</td>
                                <td><%# Belastungsanzeige != null ? string.Format("{0:dd.MM.yyyy}", Belastungsanzeige.Date) : "&nbsp;"%></td>
                                <td  class="First">Summe</td>
                                <td><%# Belastungsanzeige != null ? string.Format("{0:c}", Belastungsanzeige.Sum) : "&nbsp;"%></td>
                            </tr>
                            <tr>
                                <td class="First">Gutachter</td>
                                <td><%# Belastungsanzeige != null ? Belastungsanzeige.Gutachter : "&nbsp;"%></td>
                                <td class="First">Gutachten-ID</td>
                                <td><%# Belastungsanzeige != null ? Belastungsanzeige.GutachtenId : "&nbsp;"%></td>
                                <td  class="First">KM-Stand</td>
                                <td><%# Belastungsanzeige != null ? string.Format("{0:#,###}", Belastungsanzeige.KM) : "&nbsp;"%></td>
                            </tr>
                            <tr>
                                <td class="First">Status Belastungsanzeige</td>
                                <td><%# Belastungsanzeige != null ? Belastungsanzeige.Status : "&nbsp;"%></td>
                                <td class="First">Schadenrechnungsnr.</td>
                                <td><%# Belastungsanzeige != null ? Belastungsanzeige.SchadRechNo : "&nbsp;"%></td>
                                <td  class="First">Datum Schadenrg.</td>
                                <td><%# Belastungsanzeige != null ? string.Format("{0:dd.MM.yyyy}", Belastungsanzeige.SchadRechDate) : "&nbsp;"%></td>
                            </tr>
                            <tr>
                                <td class="First">Widerspruch-Text</td>
                                <td><%# Belastungsanzeige != null ? Belastungsanzeige.WiderspruchText : "&nbsp;"%></td>
                                <td class="First">Widerspruch-Datum</td>
                                <td><%# Belastungsanzeige != null ? string.Format("{0:dd.MM.yyyy}", Belastungsanzeige.WiderspruchDate) : "&nbsp;"%></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="First">Blockade-Text</td>
                                <td ><%# Belastungsanzeige != null ? Belastungsanzeige.BlockadeText : "&nbsp;"%></td>
                                <td class="First">Blockade-Datum</td>
                                <td ><%# Belastungsanzeige != null ? string.Format("{0:dd.MM.yyyy}", Belastungsanzeige.BlockadeDate) : "&nbsp;"%></td>
                                <td  class="First">Blockade-User</td>
                                <td ><%# Belastungsanzeige != null ? Belastungsanzeige.BlockadeUser : "&nbsp;"%></td>
                            </tr>
                             <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="True" ForeColor="White" ID="Label26" 
                                        runat="server">Weitervermarktung</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">HC-Ausgang</td>
                                <td ><%# string.Format("{0:dd.MM.yyyy}",Uebersicht.HCAusgang) %>&nbsp;</td>
                                <td class="First">ZB II Ausgang</td>
                                <td ><%# string.Format("{0:dd.MM.yyyy}",Uebersicht.ZBAusgang) %>&nbsp;</td>
                                <td  class="First">Schlüsselausgang</td>
                                <td ><%# string.Format("{0:dd.MM.yyyy}", Uebersicht.SchlAusgang) %>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="True" ForeColor="White" ID="Label28" 
                                        runat="server">Dokumente zum Fahrzeug</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <%# Links.HasBelastungsanzeige ? "Belastungsanzeige" : "Keine Belastungsanzeige"%>
                                    &nbsp;
                                    <asp:ImageButton runat="server" ImageUrl="/services/images/pdf-logo.png" Height="20" Width="20" Visible='<%# Links.HasBelastungsanzeige %>' OnClick="ShowBelastungsanzeige" style="vertical-align: middle"/>&nbsp;
                                </td>
                                <td class="First">
                                    <%# Links.HasSchadensgutachten ? "Schadensgutachten" : "Kein Schadensgutachten"%>
                                    &nbsp;
                                    <asp:ImageButton runat="server" ImageUrl="/services/images/pdf-logo.png" Height="20" Width="20" Visible='<%# Links.HasSchadensgutachten %>' OnClick="ShowSchadensgutachten" style="vertical-align: middle" />&nbsp;
                                </td>
                                <td class="First">
                                    <%# Links.HasTuevGutachten ? "Gutachten" : "Kein Gutachten"%>
                                    &nbsp;
                                    <asp:HyperLink runat="server" ImageUrl="/services/images/TUEV.png" Height="20" Width="20" Visible='<%# Links.HasTuevGutachten %>' NavigateUrl='<%# Links.TuevGutachtenUrl %>' Target="_blank" style="vertical-align: middle" />&nbsp;
                                </td>
                                <td class="First">
                                    <%# Links.HasRepKalk ? "RepKalk" : "Keine RepKalk"%>
                                    &nbsp;
                                    <asp:ImageButton runat="server" ImageUrl="/services/images/Tool.png" Height="20" Width="20" Visible='<%# Links.HasRepKalk %>' OnClick="ShowRepKalk" style="vertical-align: middle" ToolTip="Reparaturkostenkalkulation" />&nbsp;
                                </td>
                                <td class="First">                                    
                                    <%# Links.HasRechnung ? "Rechnung" : "Keine Rechnung"%>
                                    &nbsp;
                                    <asp:ImageButton runat="server" ImageUrl="/services/images/pdf-logo.png" Height="20" Width="20" Visible='<%# Links.HasRechnung %>' OnClick="ShowRechnung" style="vertical-align: middle" />&nbsp;
                                </td>
                                <td>
                                </td>
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
                            <tr>
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
                    <div id="HistTabPanel4"  class="HistTabPanel"  style="display: none">
                        <table id="Lebenslauf" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>                                            
                            </tr>                                    
                            <tr>
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Datum</asp:Label>
                                </td>                                            
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Ereignis</asp:Label>
                                </td>                                            
                            </tr>
                            <asp:Repeater id="LebenslaufRepeater" runat="server">
                                <ItemTemplate>
                                <tr>
                                    <td><%# DataBinder.Eval(Container.DataItem,"Date", "{0:dd.MM.yyyy}") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem,"Description") %></td>
                                </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                <tr style="background-color:rgb(239, 239, 239)">
                                    <td><%# DataBinder.Eval(Container.DataItem,"Date", "{0:dd.MM.yyyy}") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem,"Description") %></td>
                                </tr>
                                </AlternatingItemTemplate> 
                            </asp:Repeater>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>                                            
                            </tr>           
                        </table>
                    </div>
                    
                    <div id="HistTabPanel5"  class="HistTabPanel"  style="display: none">
                        <table id="Schaden" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="9">
                                    &nbsp;
                                </td>                                            
                            </tr>                                    
                            <tr>
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Fahrgestellnummer</asp:Label>
                                </td>                                            
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Kennzeichen</asp:Label>
                                </td>
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Anlagedatum</asp:Label>
                                </td>
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Schadensbetrag</asp:Label>
                                </td>
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Schadensdatum</asp:Label>
                                </td>
                                 <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Beschreibung</asp:Label>
                                </td>
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Datum Update</asp:Label>
                                </td>
                                 <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Repariert</asp:Label>
                                </td>
                                <td style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" runat="server">Wertminderungsbetrag</asp:Label>
                                </td>                                               
                            </tr>
                            <asp:Repeater id="VorschadenRepeater" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# DataBinder.Eval(Container.DataItem,"FAHRGNR") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"KENNZ") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"ERDAT", "{0:dd.MM.yyyy}") %></td>
                                        <td align="right"><%# DataBinder.Eval(Container.DataItem,"PREIS", "{0:c}") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"SCHAD_DAT", "{0:dd.MM.yyyy}") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"BESCHREIBUNG") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"DAT_UPD_VORSCH", "{0:dd.MM.yyyy}") %></td>  
                                        <td><%# DataBinder.Eval(Container.DataItem,"REPARIERT") %></td>
                                        <td align="right"><%# DataBinder.Eval(Container.DataItem,"WRTMBETR", "{0:c}") %></td>                                   
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr style="background-color:rgb(239, 239, 239)">
                                        <td><%# DataBinder.Eval(Container.DataItem,"FAHRGNR") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"KENNZ") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"ERDAT", "{0:dd.MM.yyyy}") %></td>
                                        <td align="right"><%# DataBinder.Eval(Container.DataItem,"PREIS", "{0:c}") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"SCHAD_DAT", "{0:dd.MM.yyyy}") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"BESCHREIBUNG") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem,"DAT_UPD_VORSCH", "{0:dd.MM.yyyy}") %></td>  
                                        <td><%# DataBinder.Eval(Container.DataItem,"REPARIERT") %></td>
                                        <td align="right"><%# DataBinder.Eval(Container.DataItem,"WRTMBETR", "{0:c}") %></td>                                     
                                    </tr>
                                </AlternatingItemTemplate> 
                            </asp:Repeater>
                            <tr>
                                <td colspan="9">
                                    &nbsp;
                                </td>                                            
                            </tr>           
                        </table>
                    </div>
                    
                    <div id="HistTabPanel6"  class="HistTabPanel"  style="display: none">
                                    
                        <table id="Table1" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>                                            
                                </tr>                                    
                                <tr>
                                <td colspan="6"  style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="Label30" runat="server"> Modell</asp:Label>
                                </td>                                            
                            </tr>
                            <tr>
                                <td class="First" nowrap align="right" width="20">
                                    1
                                </td>
                                <td  class="First" nowrap width="220">
                                    Modell Code / Bezeichnung
                                </td>
                                <td class="ABEDaten" colspan="3" nowrap width="100">
                                    <asp:Label ID="lbl_ModelCode" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" nowrap width="100%">
                                    <asp:Label ID="lbl_ModelBezeichnung" runat="server">-</asp:Label>
                                </td>
                            </tr>
                        </table>                              
                                              
                        <table id="Table6" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>                                            
                                </tr>                                    
                                <tr>
                                <td colspan="6"  style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="Label52" runat="server"> Außenfarbe</asp:Label>
                                </td>                                            
                            </tr>
                            <tr>
                                <td class="First" nowrap align="right" width="20">
                                    1
                                </td>
                                <td  class="First" nowrap width="220">
                                    Farbcode / Bezeichnung
                                </td>
                                <td class="ABEDaten" colspan="3" nowrap width="100">
                                    <asp:Label ID="lbl_FarbCode_Aussen" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" nowrap width="100%">
                                    <asp:Label ID="lbl_FarbBezeichnung_Aussen" runat="server">-</asp:Label>
                                </td>
                            </tr>
                        </table>                                                 
                        <table id="Table2" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>                                            
                                </tr>                                    
                                <tr>
                                <td colspan="6"  style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="Label62" runat="server"> Innenfarbe</asp:Label>
                                </td>                                            
                            </tr>
                            <tr>
                                <td class="First" nowrap align="right" width="20">
                                    1
                                </td>
                                <td  class="First" nowrap width="220">
                                    Farbcode / Bezeichnung
                                </td>
                                <td class="ABEDaten" colspan="3" nowrap width="100">
                                    <asp:Label ID="lbl_FarbCode_Innen" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" nowrap width="100%">
                                    <asp:Label ID="lbl_FarbBezeichnung_Innen" runat="server">-</asp:Label>
                                </td>
                            </tr>
                        </table>                      
                                                                      
                        <table id="Table3" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>                                            
                                </tr>                                    
                                <tr>
                                <td colspan="6"  style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="Label64" runat="server"> Ausstattungen</asp:Label>
                                </td>                                            
                            </tr>
                            <asp:Repeater id="AusstattungRepeater" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="First" nowrap align="right" width="20">
                                            <%# DataBinder.Eval(Container.DataItem, "Pos") %>
                                        </td>
                                        <td  class="First" nowrap width="220">
                                            Ausstattung&nbsp;Code / Bezeichnung
                                        </td>
                                        <td class="ABEDaten" colspan="3" nowrap width="100">
                                            <%# DataBinder.Eval(Container.DataItem, "PACKIDENT") %>
                                        </td>
                                        <td class="ABEDaten" align="left" nowrap width="100%">
                                            <%# DataBinder.Eval(Container.DataItem, "BEZ_PRNR") %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>   
                                 
                </div>
                    

                </div>
            </div>
        </div>
    </div>
    <asp:Literal runat="server" ID="ShowReportHelper" Visible="false" />
    <script type="text/javascript" >

        click1 = function () { click(0); };
        click2 = function () { click(1); };
        click3 = function () { click(2); };
        click4 = function () { click(3); };
        click5 = function () { click(4); };
        click6 = function () { click(5); };

        click = function (i) {
            var numTabs = 6;
            for (var t = 0; t < numTabs; t++) {
                var tab = document.getElementById("Tab" + (t + 1));
                var panel = document.getElementById("HistTabPanel" + (t + 1));

                if (t == i) {
                    // tab is active
                    panel.style.display = "block";
                    if (t == 0) { tab.className = "HistbuttonFirst"; }
                    else if ((t + 1) == numTabs) { tab.className = "HistButtonLastActive"; }
                    else { tab.className = "HistButtonMiddleActive"; }
                }
                else {
                    // tab not active
                    panel.style.display = "none";
                    if ((t + 1) == i) { tab.className = "HistButtonBeforActive"; }
                    else if ((t + 1) == numTabs) { tab.className = "HistButtonLast"; }
                    else { tab.className = "Histbutton"; }
                }
            }
        };            
    </script>
</asp:Content>