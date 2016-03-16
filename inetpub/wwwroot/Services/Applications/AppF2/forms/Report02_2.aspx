<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02_2.aspx.vb" Inherits="AppF2.Report02_2"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
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
                                    <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" height="11px" />
                                </span><span>
                                    <asp:LinkButton ID="lbCreatePDF" runat="server" Text="PDF herunterladen" ForeColor="White"></asp:LinkButton>
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
                    <div style="padding-top: 10px; margin-bottom: 10px; margin-top: 10px">
                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server" Font-Bold="True">lbl_Fahrgestellnummer</asp:Label>
                        <asp:Label ID="lblFahrgestellnummerShow" runat="server"></asp:Label>
                        <asp:Label ID="lbl_Kennzeichen" runat="server" Font-Bold="True">lbl_Kennzeichen </asp:Label>
                        <asp:Label ID="lblKennzeichenShow" runat="server"></asp:Label>
                        <asp:Label ID="lbl_Status" runat="server" Font-Bold="True">lbl_Status </asp:Label>
                        <asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                        <asp:Label ID="lbl_Lagerort" runat="server" Font-Bold="True">lbl_Lagerort</asp:Label>
                        <asp:Label ID="lblLagerortShow" runat="server"></asp:Label>
                    </div>
                            
                    <div class="DivHistTabContainer">
                        <input type="hidden" id="ihShowZweitschluessel" runat="server"/>                               
                        <ul class="HistTabContainer" >
                            <li id="HistTab1" onclick="javascript:clickHistTab(1);" class="HistbuttonFirst" >
                                Übersicht
                            </li>
                            <li id="HistTab2" onclick="javascript:clickHistTab(2);" class="Histbutton">
                                Typdaten
                            </li>
                            <li id="HistTab3" onclick="javascript:clickHistTab(3);" class="Histbutton">
                                Lebenslauf
                            </li>
                            <li id="HistTab4" onclick="javascript:clickHistTab(4);" class="Histbutton">
                                Übermittlung
                            </li>
                            <li id="HistTab5" onclick="javascript:clickHistTab(5);" class="Histbutton">
                                Händlerdaten
                            </li>
                            <li id="HistTab6" onclick="javascript:clickHistTab(6);" class="HistButtonLast">
                                Zweitschlüssel
                            </li>
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
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_Fahrzeugdaten" runat="server">lbl_Fahrzeugdaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Hersteller" runat="server">lbl_Hersteller</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblHerstellerShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Fahrzeugmodell" runat="server">lbl_Fahrzeugmodell</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFahrzeugmodellShow" runat="server"></asp:Label>
                                </td>
                                <td  class="First">
                                    <asp:Label ID="lbl_Farbe" runat="server">lbl_Farbe</asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="lbl_155" runat="server" BackColor="Black" BorderColor="White" BorderWidth="1px" ForeColor="White">-</asp:Label>
                                    <asp:Label ID="lbl_191" runat="server" BackColor="SaddleBrown" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_192" runat="server" BackColor="DimGray" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_193" runat="server" BackColor="Green" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_194" runat="server" BackColor="RoyalBlue" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_195" runat="server" BackColor="Magenta" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_196" runat="server" BackColor="Red" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_197" runat="server" BackColor="OrangeRed" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_198" runat="server" BackColor="Yellow" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_199" runat="server" BackColor="White" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label>&nbsp;
                                    <asp:Label ID="lbl_200" runat="server" BackColor="Transparent">-</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  class="First">
                                    <asp:Label ID="lbl_HerstellerSchluessel" runat="server">lbl_HerstellerSchluessel</asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="lblHerstellerSchluesselShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Typschluessel" runat="server">lbl_Typschluessel</asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="lblTypschluesselShow" runat="server"></asp:Label>
                                </td>
                                <td  class="First">
                                    <asp:Label ID="lbl_VarianteVersion" runat="server">lbl_VarianteVersion</asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="lblVarianteVersionShow" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_Briefdaten" runat="server">lbl_Briefdaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Briefnummer" runat="server">lbl_Briefnummer</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblBriefnummerShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Erstzulassungsdatum" runat="server">lbl_Erstzulassungsdatum</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblErstzulassungsdatumShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Abmeldedatum" runat="server">lbl_Abmeldedatum</asp:Label>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="lblAbmeldedatumShow" runat="server"></asp:Label>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_CoC" runat="server">lbl_CoC</asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbxCOC" runat="server" Enabled="False" TextAlign="Left" />
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Fahrzeughalter" runat="server">lbl_Fahrzeughalter</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFahrzeughalterShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Standort" runat="server">lbl_Standort</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblStandortShow" runat="server"></asp:Label>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Versandgrund" runat="server">lbl_Versandgrund</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblVersandgrundShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="First">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_Abmeldedaten" runat="server">lbl_Abmeldedaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_CarportEingang" runat="server">lbl_CarportEingang</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCarportEingangShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_KennzeichenEingang" runat="server">lbl_KennzeichenEingang</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblKennzeichenEingangShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_CheckIn" runat="server">lbl_CheckIn</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCheckInShow" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Fahrzeugschein" runat="server">lbl_Fahrzeugschein</asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbxFahrzeugschein" runat="server" Enabled="False" TextAlign="Left" />
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_BeideKennzVorhanden" runat="server">lbl_BeideKennzVorhanden</asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbxBeideKennzVorhanden" runat="server" Enabled="False" TextAlign="Left" />
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Stilllegung" runat="server">lbl_Stilllegung</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblStilllegungShow" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>           
                                <td colspan="6" style="background-color:#9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_Aenderungsdaten" runat="server">lbl_Aenderungsdaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_UmgemeldetAm" runat="server">lbl_UmgemeldetAm</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblUmgemeldetAmShow" runat="server"></asp:Label>
                                    &nbsp;</td>
                                <td class="First">
                                    <asp:Label ID="lbl_EhemaligesKennzeichen" runat="server">lbl_EhemaligesKennzeichen</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEhemaligesKennzeichenShow" runat="server"></asp:Label>
                                    &nbsp;
                                </td>
                                <td class="First">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Briefaufbietung" runat="server">lbl_Briefaufbietung</asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkBriefaufbietung" runat="server" Enabled="False" />
                                    &nbsp;</td>
                                <td class="First">
                                    <asp:Label ID="lbl_EhemaligeBriefnummer" runat="server">lbl_EhemaligeBriefnummer</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEhemaligeBriefnummerShow" runat="server"></asp:Label>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color:#9C9C9C">
                                               
                                        <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_Referenzen" runat="server">lbl_Referenzen</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Ordernummer" runat="server">lbl_Ordernummer</asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:Label ID="lblOrdernummerShow" runat="server"></asp:Label>
                                </td>
                            </tr>                                        
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Ref1" runat="server">lbl_Ref1</asp:Label>&nbsp;</td>
                                <td colspan="5">
                                    <asp:Label ID="lblRef1Show" runat="server"></asp:Label>
                                    &nbsp;</td>
                            </tr>                                        
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Ref2" runat="server">lbl_Ref2</asp:Label>
                                &nbsp;</td>
                                <td colspan="5">
                                    <asp:Label ID="lblRef2Show" runat="server"></asp:Label>
                                    &nbsp;</td>
                            </tr>                                        
                            <tr>
                                <td colspan="6" style="background-color:#9C9C9C">
                                               
                                        <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_UebBemerkungen" runat="server">lbl_UebBemerkungen</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_AnzBemerkungen" runat="server">lbl_AnzBemerkungen</asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:Label ID="lblAnzBemerkungenShow" runat="server"></asp:Label>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </div>
                    <div id="HistTabPanel2"  class="HistTabPanel" style="display: none" >
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
                                    <asp:Label ID="lbl_55" runat="server" BorderWidth="1px" BackColor="Black" ForeColor="White" BorderColor="White">-</asp:Label>
                                    <asp:Label ID="lbl_91" runat="server" BorderWidth="1px" BackColor="SaddleBrown" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_92" runat="server" BorderWidth="1px" BackColor="DimGray" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_93" runat="server" BorderWidth="1px" BackColor="Green" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_94" runat="server" BorderWidth="1px" BackColor="RoyalBlue" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_95" runat="server" BorderWidth="1px" BackColor="Magenta" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_96" runat="server" BorderWidth="1px" BackColor="Red" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_97" runat="server" BorderWidth="1px" BackColor="OrangeRed" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_98" runat="server" BorderWidth="1px" BackColor="Yellow" ForeColor="Black" BorderColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_99" runat="server" BorderWidth="1px" BackColor="White" ForeColor="Black" BorderColor="Black">-</asp:Label>&nbsp;
                                    <asp:Label ID="lbl_00" runat="server" BackColor="Transparent">-</asp:Label>
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
                                                                                                  
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblAchsen" runat="server">  Achsen / Bereifung</asp:Label>
                                                  
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
                                                                                                  
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblBemerkungen" runat="server">  Bemerkungen</asp:Label>
                                                  
                                </td>                                            
                            </tr>
                            <tr>
                                <td class="First"  valign="top" align="right">
                                    <asp:Label ID="lbl_AnzBemerkungen2" runat="server">lbl_AnzBemerkungen2</asp:Label>
                                </td>

                                <td class="ABEDaten" colspan="5">
                                    <asp:Label ID="lbl_30" runat="server" Font-Names="Arial" Font-Size="XX-Small">-</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="HistTabPanel3"  class="HistTabPanel"  style="display: none">
                        <div style="height:30px"></div>
                        <asp:DataGrid ID="Datagrid2" runat="server" BackColor="White" AutoGenerateColumns="False"
                            Width="100%" AllowSorting="True" bodyHeight="300" CssClass="GridView" PageSize="50">
                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                            <ItemStyle CssClass="ItemStyle" />
                            <Columns>
                                <asp:BoundColumn DataField="KURZTEXT" SortExpression="KURZTEXT" HeaderText="Vorgang">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="STRMN" SortExpression="STRMN" HeaderText="Durchf&#252;hrungs-&lt;br&gt;datum"
                                    DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Versandadresse">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") & " " & DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
                                        </asp:Label><asp:Literal ID="Literal1" runat="server" Text="<br>"></asp:Literal><asp:Label
                                            ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") & " " & DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
                                        </asp:Label><asp:Literal ID="Literal2" runat="server" Text="<br>"></asp:Literal><asp:Label
                                            ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") & " " & DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
                                        </asp:Label><asp:Literal ID="Literal3" runat="server" Text="<br>"></asp:Literal><asp:Label
                                            ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LANDX_Z5") %>'>
                                        </asp:Label></ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn Visible="False" DataField="ERDAT" SortExpression="ERDAT" HeaderText="Erfassungs-&lt;br&gt;datum"
                                    DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ZZDIEN1" SortExpression="ZZDIEN1" HeaderText="Versandart">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="QMNAM" SortExpression="QMNAM" HeaderText="Beauftragt&lt;br&gt;durch">
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="QMNUM" SortExpression="QMNUM" HeaderText="Meldungsnummer">
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                    <div id="HistTabPanel4" class="HistTabPanel"  style="display: none">
                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False"
                            Width="100%"  AllowSorting="True" bodyHeight="300" CssClass="GridView" PageSize="50">
                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                            <ItemStyle CssClass="ItemStyle" />
                            <Columns>
                                <asp:BoundColumn DataField="MNCOD" SortExpression="MNCOD" HeaderText="Aktionscode">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="MATXT" SortExpression="MATXT" HeaderText="Vorgang"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PSTER" SortExpression="PSTER" HeaderText="Statusdatum">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ZZUEBER" SortExpression="ZZUEBER" HeaderText="&#220;bermittlungs-&lt;br&gt;datum"
                                    DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="AEZEIT" SortExpression="AEZEIT" HeaderText="&#196;nderungs-&lt;br&gt;Zeit"
                                    DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                        <div id="DivPlaceholder" runat="server" style="height:500px; border-bottom:solid 1px #dfdfdf" visible="false"></div>
                    </div>
                    <div id="HistTabPanel5"  class="HistTabPanel"  style="display: none">
                        <table  width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Haendlernr" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblHaendlernrShow" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  class="First" valign="top">
                                    <asp:Label ID="lbl_Haendleradresse" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblHaendleradresseShow" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr class="First">
                                <td class="First">
                                    <asp:Label ID="lbl_Finanzierungsart" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFinanzierungsartShow" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="HistTabPanel6" class="HistTabPanel"  style="display: none">
                        <div style="height:30px"></div>
                        <asp:DataGrid ID="Datagrid3" runat="server" BackColor="White" AutoGenerateColumns="False"
                            Width="100%" AllowSorting="True" bodyHeight="300" CssClass="GridView" PageSize="50">
                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                            <ItemStyle CssClass="ItemStyle" />
                            <Columns>
                                <asp:BoundColumn DataField="KURZTEXT" SortExpression="KURZTEXT" HeaderText="Vorgang">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="STRMN" SortExpression="STRMN" HeaderText="Durchf&#252;hrungs-&lt;br&gt;datum"
                                    DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Versandadresse">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") & " " & DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
                                        </asp:Label><asp:Literal ID="Literal1" runat="server" Text="<br>"></asp:Literal><asp:Label
                                            ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") & " " & DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
                                        </asp:Label><asp:Literal ID="Literal2" runat="server" Text="<br>"></asp:Literal><asp:Label
                                            ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") & " " & DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
                                        </asp:Label><asp:Literal ID="Literal3" runat="server" Text="<br>"></asp:Literal><asp:Label
                                            ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LANDX_Z5") %>'>
                                        </asp:Label></ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn Visible="False" DataField="ERDAT" SortExpression="ERDAT" HeaderText="Erfassungs-&lt;br&gt;datum"
                                    DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ZZDIEN1" SortExpression="ZZDIEN1" HeaderText="Versandart">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="QMNAM" SortExpression="QMNAM" HeaderText="Beauftragt&lt;br&gt;durch">
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="QMNUM" SortExpression="QMNUM" HeaderText="Meldungsnummer">
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" >

        InitializeHistTabs();

        function InitializeHistTabs() {
            var showZweitschluessel = (document.getElementById("<%=ihShowZweitschluessel.ClientID %>").value == 'true');

            if (showZweitschluessel) {
                document.getElementById("HistTab5").className = "Histbutton";
                document.getElementById("HistTab6").style.display = "block";
            } else {
                document.getElementById("HistTab5").className = "HistButtonLast";
                document.getElementById("HistTab6").style.display = "none";
            }
        }

        function clickHistTab(tabid) {
            var showZweitschluessel = (document.getElementById("<%=ihShowZweitschluessel.ClientID %>").value == 'true');

            var classHistTab5 = "HistButtonLast";
            var classHistTab5Active = "HistButtonLastActive";
            if (showZweitschluessel) {
                classHistTab5 = "Histbutton";
                classHistTab5Active = "HistButtonMiddleActive";
            }

            if (tabid == 1)
                document.getElementById("HistTab1").className = "HistbuttonFirst";
            else if (tabid == 2)
                document.getElementById("HistTab1").className = "HistButtonBeforActive";
            else
                document.getElementById("HistTab1").className = "Histbutton";

            if (tabid == 2)
                document.getElementById("HistTab2").className = "HistButtonMiddleActive";
            else if (tabid == 3)
                document.getElementById("HistTab2").className = "HistButtonBeforActive";
            else
                document.getElementById("HistTab2").className = "Histbutton";

            if (tabid == 3)
                document.getElementById("HistTab3").className = "HistButtonMiddleActive";
            else if (tabid == 4)
                document.getElementById("HistTab3").className = "HistButtonBeforActive";
            else
                document.getElementById("HistTab3").className = "Histbutton";

            if (tabid == 4)
                document.getElementById("HistTab4").className = "HistButtonMiddleActive";
            else if (tabid == 5)
                document.getElementById("HistTab4").className = "HistButtonBeforActive";
            else
                document.getElementById("HistTab4").className = "Histbutton";

            if (tabid == 5)
                document.getElementById("HistTab5").className = classHistTab5Active;
            else if (tabid == 6)
                document.getElementById("HistTab5").className = "HistButtonBeforActive";
            else
                document.getElementById("HistTab5").className = classHistTab5;

            if (tabid == 6)
                document.getElementById("HistTab6").className = "HistButtonLastActive";
            else
                document.getElementById("HistTab6").className = "HistButtonLast";

            for (var i = 1; i < 7; i++) {
                var tabpanelid = "HistTabPanel" + i.toString();

                if (i == tabid)
                    document.getElementById(tabpanelid).style.display = "block";
                else
                    document.getElementById(tabpanelid).style.display = "none";
            }
        }
                     
    </script>
</asp:Content>
