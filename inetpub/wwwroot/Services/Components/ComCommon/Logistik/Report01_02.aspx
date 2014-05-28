<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01_02.aspx.vb" Inherits="CKG.Components.ComCommon.Report01_02"  MasterPageFile="../../../MasterPage/Services.Master" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                            <div>
                                <cc1:TabContainer ID="TabCon" runat="server" ActiveTabIndex="0">
                                    <cc1:TabPanel runat="server" HeaderText="&Uuml;bersicht" ID="TabPanel1">
                                        <HeaderTemplate>
                                            Übersicht</HeaderTemplate>
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="6">
                                                        <div class="formqueryHeader">
                                                            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">lbl_Fahrzeugdaten</asp:Label></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_Hersteller" runat="server">lbl_Hersteller</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHerstellerShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Fahrzeugmodell" runat="server">lbl_Fahrzeugmodell</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFahrzeugmodellShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Farbe" runat="server">lbl_Farbe</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_155" runat="server" BackColor="Black" BorderColor="White" BorderWidth="1px"
                                                            ForeColor="White">-</asp:Label><asp:Label ID="lbl_191" runat="server" BackColor="SaddleBrown"
                                                                BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label><asp:Label ID="lbl_192"
                                                                    runat="server" BackColor="DimGray" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label><asp:Label
                                                                        ID="lbl_193" runat="server" BackColor="Green" BorderColor="Black" BorderWidth="1px"
                                                                        ForeColor="Black">-</asp:Label><asp:Label ID="lbl_194" runat="server" BackColor="RoyalBlue"
                                                                            BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label><asp:Label ID="lbl_195"
                                                                                runat="server" BackColor="Magenta" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label><asp:Label
                                                                                    ID="lbl_196" runat="server" BackColor="Red" BorderColor="Black" BorderWidth="1px"
                                                                                    ForeColor="Black">-</asp:Label><asp:Label ID="lbl_197" runat="server" BackColor="OrangeRed"
                                                                                        BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label><asp:Label ID="lbl_198"
                                                                                            runat="server" BackColor="Yellow" BorderColor="Black" BorderWidth="1px" ForeColor="Black">-</asp:Label><asp:Label
                                                                                                ID="lbl_199" runat="server" BackColor="White" BorderColor="Black" BorderWidth="1px"
                                                                                                ForeColor="Black">-</asp:Label>&nbsp;<asp:Label ID="lbl_200" runat="server" BackColor="Transparent">-</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_HerstellerSchluessel" runat="server">lbl_HerstellerSchluessel</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHerstellerSchluesselShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Typschluessel" runat="server">lbl_Typschluessel</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTypschluesselShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_VarianteVersion" runat="server">lbl_VarianteVersion</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVarianteVersionShow" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <div class="formqueryHeader">
                                                            <asp:Label ID="lbl_Briefdaten" runat="server">lbl_Briefdaten</asp:Label></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_Briefnummer" runat="server">lbl_Briefnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBriefnummerShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Erstzulassungsdatum" runat="server">lbl_Erstzulassungsdatum</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblErstzulassungsdatumShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Abmeldedatum" runat="server">lbl_Abmeldedatum</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAbmeldedatumShow" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_Ordernummer" runat="server">lbl_Ordernummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOrdernummerShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Fahrzeughalter" runat="server">lbl_Fahrzeughalter</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFahrzeughalterShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Standort" runat="server">lbl_Standort</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStandortShow" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_CoC" runat="server">lbl_CoC</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="cbxCOC" runat="server" Enabled="False" TextAlign="Left" />
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                    
                                                    <td>
                                                        <asp:Label ID="lbl_Versandgrund" runat="server">lbl_Versandgrund</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVersandgrundShow" runat="server"></asp:Label>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <div class="formqueryHeader">
                                                            <asp:Label ID="lbl_Aenderungsdaten" runat="server">lbl_Aenderungsdaten</asp:Label></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_UmgemeldetAm" runat="server">lbl_UmgemeldetAm</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUmgemeldetAmShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_EhemaligesKennzeichen" runat="server">lbl_EhemaligesKennzeichen</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEhemaligesKennzeichenShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_Briefaufbietung" runat="server">lbl_Briefaufbietung</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkBriefaufbietung" runat="server" Enabled="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_EhemaligeBriefnummer" runat="server">lbl_EhemaligeBriefnummer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEhemaligeBriefnummerShow" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                  <tr>
                                                    <td colspan="6">
                                                        <div class="formqueryHeader">
                                                            <asp:Label ID="lbl_UebBemerkungen" runat="server">lbl_UebBemerkungen</asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_AnzBemerkungen" runat="server">lbl_AnzBemerkungen</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAnzBemerkungenShow" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                
                                                
                                            </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Typdaten">
                                        <ContentTemplate>
                                            <table id="Table4" cellspacing="0" cellpadding="2" width="100%" bgcolor="white" border="0">
                                                <tr>
                                                    <td valign="top" align="left" colspan="6">
                                                        <div class="formqueryHeader">
                                                            Allgemeine Fahrzeugdaten
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63" height="19">
                                                        1
                                                    </td>
                                                    <td nowrap height="19">
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
                                                    <td align="right" width="63">
                                                        2
                                                    </td>
                                                    <td nowrap>
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
                                                    <td align="right" width="63">
                                                        3
                                                    </td>
                                                    <td nowrap>
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
                                                    <td align="right" width="63">
                                                        4
                                                    </td>
                                                    <td nowrap>
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
                                                    <td align="right" width="63" height="23">
                                                        5
                                                    </td>
                                                    <td nowrap height="23">
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
                                                    <td align="right" width="63" height="22">
                                                        6
                                                    </td>
                                                    <td nowrap height="22">
                                                        Fabrikname:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3" height="22">
                                                        <asp:Label ID="lbl_8" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%" height="22">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        7
                                                    </td>
                                                    <td nowrap>
                                                        Variante / Version:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_9" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                        <asp:Label ID="lbl_10" runat="server">-</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        8
                                                    </td>
                                                    <td nowrap>
                                                        Anzahl Sitze:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_26" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        9
                                                    </td>
                                                    <td nowrap>
                                                        Zul. Gesamtgewicht (kg):
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_28" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        10
                                                    </td>
                                                    <td nowrap>
                                                        Länge min. (mm)
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_31" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        11
                                                    </td>
                                                    <td nowrap>
                                                        Breite min. (mm)
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_32" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        12
                                                    </td>
                                                    <td nowrap>
                                                        Höhe min. (mm)
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_33" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <div class="formqueryHeader">
                                                            Antriebsdaten
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        1
                                                    </td>
                                                    <td nowrap>
                                                        Hubraum (cm³)
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_11" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        2
                                                    </td>
                                                    <td nowrap>
                                                        Nennleistung (KW) bei U/Min:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_13" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                        <asp:Label ID="lbl_14" runat="server">-</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        3
                                                    </td>
                                                    <td nowrap>
                                                        Höchstgeschwindigkeit (km/h):
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_12" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        4
                                                    </td>
                                                    <td nowrap>
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
                                                    <td nowrap="nowrap" align="left" colspan="6">
                                                        <div class="formqueryHeader">
                                                            Kraftstoff / Tank
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        1
                                                    </td>
                                                    <td nowrap>
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
                                                    <td align="right" width="63">
                                                        2
                                                    </td>
                                                    <td nowrap>
                                                        Fassungsvermögen Tank (m³):
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_21" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63" height="22">
                                                        3
                                                    </td>
                                                    <td nowrap height="22">
                                                        Co2-Gehalt (g/km):
                                                    </td>
                                                    <td class="ABEDaten" colspan="3" height="22">
                                                        <asp:Label ID="lbl_17" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%" height="22">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63" height="22">
                                                        4
                                                    </td>
                                                    <td nowrap height="22">
                                                        Nat. Emissionsklasse:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3" height="22">
                                                        <asp:Label ID="lbl_18" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%" height="22">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63" height="22">
                                                        5
                                                    </td>
                                                    <td nowrap height="22">
                                                        Abgasrichtlinie:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3" height="22">
                                                        <asp:Label ID="lbl_22" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%" height="22">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6">
                                                        <div class="formqueryHeader">
                                                            Achsen / Bereifung
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        1
                                                    </td>
                                                    <td nowrap>
                                                        Anzahl Achsen:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_23" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        2
                                                    </td>
                                                    <td nowrap>
                                                        Anzahl Antriebsachsen:
                                                    </td>
                                                    <td class="ABEDaten" colspan="3">
                                                        <asp:Label ID="lbl_24" runat="server">-</asp:Label>
                                                    </td>
                                                    <td class="ABEDaten" align="left" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        3
                                                    </td>
                                                    <td nowrap>
                                                        Max. Achslast Achse 1,2,3 (kg):
                                                    </td>
                                                    <td class="ABEDaten" colspan="4">
                                                        <asp:Label ID="lbl_25" runat="server">-</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="63">
                                                        4
                                                    </td>
                                                    <td nowrap>
                                                        Bereifung Achse 1,2,3:
                                                    </td>
                                                    <td class="ABEDaten" colspan="4">
                                                        <asp:Label ID="lbl_27" runat="server">-</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6">
                                                        <div class="formqueryHeader">
                                                            Bemerkungen
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="right">
                                                        1
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td class="ABEDaten" colspan="4">
                                                        <asp:Label ID="lbl_30" runat="server" Font-Names="Arial" Font-Size="XX-Small">-</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Lebenslauf">
                                        <ContentTemplate>
                                            <asp:DataGrid ID="Datagrid2" runat="server" BackColor="White" AutoGenerateColumns="False"
                                                Width="800px" AllowSorting="True" bodyHeight="300" CssClass="GridView"
                                                PageSize="50">
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
                                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
                                                            </asp:Label><asp:Literal ID="Literal1" runat="server" Text="<br>"></asp:Literal><asp:Label
                                                                ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
                                                            </asp:Label><asp:Literal ID="Literal2" runat="server" Text="<br>"></asp:Literal><asp:Label
                                                                ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
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
                                            </asp:DataGrid></ContentTemplate>
                                    </cc1:TabPanel>
                                 </cc1:TabContainer>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbCreatePDF" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
