<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report05_2.aspx.vb" Inherits="AppServicesCarRent.Report05_2"
    MasterPageFile="../MasterPage/App.Master" %>

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
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../../Images/queryArrowUp.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:Panel ID="divTrenn" runat="server" Visible="false">
                                    <div id="PlaceHolderDiv">
                                    </div>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbCreatePDF" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="TextError">&nbsp;</asp:Label>
                    </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="border: none;
                            width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Fahrgestellnummer" runat="server" Font-Bold="True">lbl_Fahrgestellnummer</asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbFahrgestellnummerShow" runat="server" OnClick="OnFahrgestellnummerClick" />
                                </td>
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Kennzeichen" runat="server" Font-Bold="True">lbl_Kennzeichen</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblKennzeichenShow" runat="server"></asp:Label>
                                </td>
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Status" runat="server" Font-Bold="True">lbl_Kennzeichen</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                                </td>
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Lagerort" runat="server" Font-Bold="True">lbl_Kennzeichen</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLagerortShow" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <div class="DivHistTabContainer">
                        <ul class="HistTabContainer">
                            <li id="First" onclick="javascript:clickFirst()" class="HistbuttonFirst">Übersicht</li>
                            <li id="Second" onclick="javascript:clickSecond()" class="Histbutton">Typdaten </li>
                            <li id="Third" onclick="javascript:clickThird()" class="Histbutton">Lebenslauf </li>
                            <li id="Fourth" onclick="javascript:clickFourth()" class="Histbutton">Opt. Archiv</li>
                            <li id="Last" runat="server" onclick="javascript:clickLast()" class="HistButtonLast">
                                Schlüsselinfo</li>
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
                                <td colspan="6" style="background-color: #9C9C9C">
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
                                <td class="First">
                                    <asp:Label ID="lbl_Farbe" runat="server">lbl_Farbe</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_155" runat="server" BackColor="Black" BorderColor="White" BorderWidth="1px"
                                        ForeColor="White">-</asp:Label>
                                    <asp:Label ID="lbl_191" runat="server" BackColor="SaddleBrown" BorderColor="Black"
                                        BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_192" runat="server" BackColor="DimGray" BorderColor="Black" BorderWidth="1px"
                                        ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_193" runat="server" BackColor="Green" BorderColor="Black" BorderWidth="1px"
                                        ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_194" runat="server" BackColor="RoyalBlue" BorderColor="Black"
                                        BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_195" runat="server" BackColor="Magenta" BorderColor="Black" BorderWidth="1px"
                                        ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_196" runat="server" BackColor="Red" BorderColor="Black" BorderWidth="1px"
                                        ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_197" runat="server" BackColor="OrangeRed" BorderColor="Black"
                                        BorderWidth="1px" ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_198" runat="server" BackColor="Yellow" BorderColor="Black" BorderWidth="1px"
                                        ForeColor="Black">-</asp:Label>
                                    <asp:Label ID="lbl_199" runat="server" BackColor="White" BorderColor="Black" BorderWidth="1px"
                                        ForeColor="Black">-</asp:Label>&nbsp;<asp:Label ID="lbl_200" runat="server" BackColor="Transparent">-</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_HerstellerSchluessel" runat="server">lbl_HerstellerSchluessel</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblHerstellerSchluesselShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Typschluessel" runat="server">lbl_Typschluessel</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTypschluesselShow" runat="server"></asp:Label>
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_VarianteVersion" runat="server">lbl_VarianteVersion</asp:Label>
                                </td>
                                <td>
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
                                    <asp:Label ID="lblErstzulassungsdatumShow" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Abmeldedatum" runat="server">lbl_Abmeldedatum</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAbmeldedatumShow" runat="server"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Ordernummer" runat="server">lbl_Ordernummer</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOrdernummerShow" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Fahrzeughalter" runat="server">lbl_Fahrzeughalter</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFahrzeughalterShow" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Standort" runat="server">lbl_Standort</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblStandortShow" runat="server"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_CoC" runat="server">lbl_CoC</asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbxCOC" runat="server" Enabled="False" TextAlign="Left" />&nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_Versandgrund" runat="server">lbl_Versandgrund</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblVersandgrundShow" runat="server"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_Aenderungsdaten" runat="server">lbl_Aenderungsdaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_UmgemeldetAm" runat="server">lbl_UmgemeldetAm</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblUmgemeldetAmShow" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_EhemaligesKennzeichen" runat="server">lbl_EhemaligesKennzeichen</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEhemaligesKennzeichenShow" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Briefaufbietung" runat="server">lbl_Briefaufbietung</asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkBriefaufbietung" runat="server" Enabled="False" />
                                </td>
                                <td class="First">
                                    <asp:Label ID="lbl_EhemaligeBriefnummer" runat="server">lbl_EhemaligeBriefnummer</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEhemaligeBriefnummerShow" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_UebBemerkungen" runat="server">lbl_UebBemerkungen</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_AnzBemerkungen" runat="server">lbl_AnzBemerkungen</asp:Label>
                                </td>
                                <td colspan="5" style="width: 100%">
                                    <asp:Label ID="lblAnzBemerkungenShow" runat="server"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <%--auskommentiert weil Archivzugriff nicht sauber ITA 5680 --%>
                            <%--
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lbl_UebDokumente" runat="server">lbl_UebDokumente</asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="First">
                                    <asp:Label ID="lbl_Dokumente" runat="server">lbl_Dokumente</asp:Label>
                                </td>
                                <td colspan="5" style="width: 100%">
                                    
                                    <asp:ImageButton ID="ibtnShowDokument" runat="server" ImageUrl="/services/images/pdf-logo.png" Height="20" Width="20" Visible='<%# Links.HasDokument %>' /> 
                                    &nbsp;
                                </td>
                            </tr>--%>
                        </table>
                        <asp:Literal runat="server" ID="ShowReportHelper" Visible="false" />
                    </div>
                    <div id="HistTabPanel2" class="HistTabPanel" style="display: none">
                        <table id="Table4" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblAllgFahrzeugdaten" runat="server"> Allgemeine Fahrzeugdaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    1
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Fahrzeugklasse&nbsp;/ Aufbau:
                                </td>
                                <td class="ABEDaten" nowrap="nowrap" colspan="3" rowspan="1">
                                    <asp:Label ID="lbl_6" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%" height="19">
                                    <asp:Label ID="lbl_7" runat="server">-</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    2
                                </td>
                                <td class="First" nowrap="nowrap">
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
                                <td class="First" align="right" width="63">
                                    3
                                </td>
                                <td class="First" nowrap="nowrap">
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
                                <td class="First" align="right" width="63">
                                    4
                                </td>
                                <td class="First" nowrap="nowrap">
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
                                <td class="First" align="right" width="63" height="23">
                                    5
                                </td>
                                <td class="First" nowrap="nowrap" height="23">
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
                                <td class="First" align="right" width="63" height="22">
                                    6
                                </td>
                                <td class="First" nowrap="nowrap" height="22">
                                    Fabrikname:
                                </td>
                                <td class="ABEDaten" colspan="3" height="22">
                                    <asp:Label ID="lbl_8" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%" height="22">
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    7
                                </td>
                                <td class="First" nowrap="nowrap">
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
                                <td class="First" align="right" width="63">
                                    8
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Anzahl Sitze:
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_26" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    9
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Zul. Gesamtgewicht (kg):
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_28" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    10
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Länge min. (mm)
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_31" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    11
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Breite min. (mm)
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_32" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    12
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Höhe min. (mm)
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_33" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblAntriebsdaten" runat="server">Antriebsdaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    1
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Hubraum (cm³)
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_11" runat="server">-</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    2
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Nennleistung (KW) bei U/Min:
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_13" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    <asp:Label ID="lbl_14" runat="server">-</asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    3
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Höchstgeschwindigkeit (km/h):
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_12" runat="server">-&nbsp;</asp:Label>
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    4
                                </td>
                                <td class="First" nowrap="nowrap">
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
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblKraftstoff" runat="server"> Kraftstoff / Tank</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    1
                                </td>
                                <td class="First" nowrap="nowrap">
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
                                <td class="First" align="right" width="63">
                                    2
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Fassungsvermögen Tank (m³):
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_21" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63" height="22">
                                    3
                                </td>
                                <td class="First" nowrap="nowrap" height="22">
                                    Co2-Gehalt (g/km):
                                </td>
                                <td class="ABEDaten" colspan="3" height="22">
                                    <asp:Label ID="lbl_17" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%" height="22">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63" height="22">
                                    4
                                </td>
                                <td class="First" nowrap="nowrap" height="22">
                                    Nat. Emissionsklasse:
                                </td>
                                <td class="ABEDaten" colspan="3" height="22">
                                    <asp:Label ID="lbl_18" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%" height="22">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63" height="22">
                                    5
                                </td>
                                <td class="First" nowrap="nowrap" height="22">
                                    Abgasrichtlinie:
                                </td>
                                <td class="ABEDaten" colspan="3" height="22">
                                    <asp:Label ID="lbl_22" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%" height="22">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblAchsen" runat="server">  Achsen / Bereifung</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    1
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Anzahl Achsen:
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_23" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    2
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Anzahl Antriebsachsen:
                                </td>
                                <td class="ABEDaten" colspan="3">
                                    <asp:Label ID="lbl_24" runat="server">-</asp:Label>&nbsp;
                                </td>
                                <td class="ABEDaten" align="left" width="100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    3
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Max. Achslast Achse 1,2,3 (kg):
                                </td>
                                <td class="ABEDaten" colspan="4">
                                    <asp:Label ID="lbl_25" runat="server">-</asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First" align="right" width="63">
                                    4
                                </td>
                                <td class="First" nowrap="nowrap">
                                    Bereifung Achse 1,2,3:
                                </td>
                                <td class="ABEDaten" colspan="4">
                                    <asp:Label ID="lbl_27" runat="server">-</asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="lblBemerkungen" runat="server">  Bemerkungen</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First" valign="top" align="right">
                                    <asp:Label ID="lbl_AnzBemerkungen2" runat="server">lbl_AnzBemerkungen2</asp:Label>
                                </td>
                                <td class="ABEDaten" colspan="5">
                                    <asp:Label ID="lbl_30" runat="server" Font-Names="Arial" Font-Size="XX-Small">-</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="HistTabPanel3" class="HistTabPanel" style="display: none">
                        <div style="height: 30px">
                        </div>
                        <div id="data" style="width: 100%">
                            <asp:GridView ID="Datagrid2" AutoGenerateColumns="False" BackColor="White" runat="server"
                                CssClass="GridView" Width="100%" GridLines="None" PageSize="20" AllowPaging="True"
                                AllowSorting="True">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <EditRowStyle></EditRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="KURZTEXT" SortExpression="KURZTEXT" HeaderText="Vorgang">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MAKTX" SortExpression="MAKTX" HeaderText="Komponentenbezeichnung" />
                                    <asp:BoundField DataField="STRMN" SortExpression="STRMN" HeaderText="Durchf&#252;hrung"
                                        DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Versandadresse">
                                        <HeaderStyle ForeColor="#ffffff" />
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") & " " & DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
                                            </asp:Label><asp:Literal ID="Literal1" runat="server" Text="<br>"></asp:Literal><asp:Label
                                                ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") & " " & DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
                                            </asp:Label><asp:Literal ID="Literal2" runat="server" Text="<br>"></asp:Literal><asp:Label
                                                ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") & " " & DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
                                            </asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField Visible="False" DataField="ERDAT" SortExpression="ERDAT" HeaderText="Erfassungsdatum"
                                        DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                    <asp:BoundField DataField="ZZDIEN1" SortExpression="ZZDIEN1" HeaderText="Versandart">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QMNAM" SortExpression="QMNAM" HeaderText="Beauftragt durch">
                                    </asp:BoundField>
                                    <asp:BoundField Visible="False" DataField="QMNUM" SortExpression="QMNUM" HeaderText="Meldungsnummer">
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div id="HistTabPanel4" runat="server" class="HistTabPanel">
                        <div style="height: 30px">
                        </div>
                        <div style="width: 100%">
                            <asp:GridView Width="100%" AutoGenerateColumns="False" BackColor="White" AllowSorting="True"
                                runat="server" ID="gvArchiv" CssClass="GridView" GridLines="None" EnableModelValidation="True">
                                <PagerSettings Visible="False" />
                                <HeaderStyle HorizontalAlign="Left" CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <EditRowStyle></EditRowStyle>
                                <Columns>
                                    <asp:TemplateField Visible="true" HeaderStyle-Width="5%" SortExpression="Download">
                                        <ItemTemplate>
                                            <asp:ImageButton name="ibDownload" ID="ibDownload" runat="server" CommandName="download"
                                                ImageUrl="../../../Images/iconPDF.gif" CommandArgument="<%# Container.DataItemIndex %>"
                                                ToolTip="Dokument vom Server laden"></asp:ImageButton>
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderStyle-Width="20%" DataField="ARCHNAME" SortExpression="ARCHNAME"
                                        HeaderText="Archivname">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-Width="20%" DataField="DOKUMENTENART" SortExpression="DOKUMENTENART"
                                        HeaderText="Dokumentart">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-Width="15%" DataField="ERDAT" SortExpression="ERDAT"
                                        HeaderText="Erstelldatum" DataFormatString="{0:dd.MM.yyyy}">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-Width="20%" DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM"
                                        HeaderText="Fahrgestellnummer">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-Width="20%" DataField="EQFNR" SortExpression="EQFNR"
                                        HeaderText="Sortierfeld">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </asp:BoundField>
                                    <%--            
                       <asp:TemplateField SortExpression="Dokumentart" HeaderText="Dokumentart" >
                                    <HeaderStyle />
                                    <HeaderTemplate>
                                        <asp:LinkButton HeaderText="Dokumentart" ID="col_DOKUMENTENART" runat="server" CommandArgument="DOKUMENTENART"></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DOKUMENTENART") %>'
                                            ID="lblDOKUMENTENART" Visible="true"> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Label CssClass="First" runat="server" ID="lblNoData" Text="" />
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                    <div id="HistTabPanel5" runat="server" class="HistTabPanel" style="display: none">
                        <table id="Table6" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    Fahrgestellnummer:
                                </td>
                                <td>
                                    <asp:Label ID="lblFahrgestellnummer" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="First">
                                    Eingang Schlüssel:&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblEingangSchluessel" runat="server"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="Label4" runat="server">Letzte Versanddaten</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    <asp:Label ID="lblVersendetAmDescription" runat="server">Versendet am:</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAngefordertAm" runat="server"></asp:Label><asp:Label ID="lblVersendetAm"
                                        runat="server" Visible="False"></asp:Label>&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="First">
                                    <asp:Label ID="Label1" runat="server">Versandart:</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server">   temporär</asp:Label>
                                    <asp:RadioButton ID="rbTemporaer" runat="server" Enabled="False" GroupName="Versandart">
                                    </asp:RadioButton><br />
                                    <asp:Label ID="Label2" runat="server">   endgültig</asp:Label>
                                    <asp:RadioButton ID="rbEndgueltig" runat="server" Enabled="False" GroupName="Versandart">
                                    </asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="First">
                                    Versandanschrift:
                                </td>
                                <td>
                                    <asp:Label ID="lblVersandanschrift" runat="server"></asp:Label>&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="background-color: #9C9C9C">
                                    <asp:Label Font-Bold="true" ForeColor="#FFFFFF" ID="Label5" runat="server">Lebenslauf</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:GridView ID="gv_LebelaufTuete" Width="100%" AutoGenerateColumns="False" BackColor="White"
                                        runat="server" CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True"
                                        AllowSorting="True">
                                        <PagerSettings Visible="False" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <EditRowStyle></EditRowStyle>
                                        <Columns>
                                            <asp:BoundField DataField="KURZTEXT" SortExpression="KURZTEXT" HeaderText="Vorgang">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MAKTX" SortExpression="MAKTX" HeaderText="Komponente">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STRMN" SortExpression="STRMN" HeaderText="Durchf&#252;hrung"
                                                DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Versandadresse">
                                                <HeaderStyle ForeColor="#ffffff" />
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") & " " & DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
                                                    </asp:Label><asp:Literal ID="Literal1" runat="server" Text="<br>"></asp:Literal><asp:Label
                                                        ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") & " " & DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
                                                    </asp:Label><asp:Literal ID="Literal2" runat="server" Text="<br>"></asp:Literal><asp:Label
                                                        ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") & " " & DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
                                                    </asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField Visible="False" DataField="ERDAT" SortExpression="ERDAT" HeaderText="Erfassungsdatum"
                                                DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="ZZDIEN1" SortExpression="ZZDIEN1" HeaderText="Versandart">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QMNAM" SortExpression="QMNAM" HeaderText="Beauftragt durch">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="QMNUM" SortExpression="QMNUM" HeaderText="Meldungsnummer">
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataQueryFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function getNextElementSibling(node) {
            try {
                if (node.nextElementSibling) {
                    return node.nextElementSibling;
                }

                var s = node.nextSibling;

                while (s && s.nodeType !== 1) {
                    s = s.nextSibling;
                }

                return s;

            } catch (e) {
                return null;
            }

        }





        var temp;
        var first;
        var second;
        var third;
        var fourth;
        var last;

        var panel1;
        var panel2;
        var panel3;
        var panel4;
        var panel5;



        initTabPanel()

        if (lastTab == "second") {
            clickSecond()
        }
        else if (lastTab == "third") {
            clickThird()
        }
        else if (lastTab == "fourth") {
            clickFourth()
        }
        else if (lastTab == "last") {
            clickLast()
        }
        else {
            clickFirst()
        }

        function initTabPanel() {

            first = document.getElementById("First");
            second = document.getElementById("Second");
            third = document.getElementById("Third");
            fourth = document.getElementById("Fourth");
            last = getNextElementSibling(fourth);
            panel1 = document.getElementById("HistTabPanel1");
            panel2 = getNextElementSibling(panel1);
            panel3 = getNextElementSibling(panel2);
            panel4 = getNextElementSibling(panel3);
            panel5 = getNextElementSibling(panel4);


            if (panel4.disabled) {
                fourth.style.display = "none";

                if (last) {
                    last.className = "HistButtonLast";
                }
                else {
                    third.className = "HistButtonLast";
                }
            }
            else {
                fourth.style.display = "";
                if (last) {
                    fourth.className = "Histbutton";
                    last.className = "HistButtonLast";
                }
                else {
                    fourth.className = "HistButtonLast";
                }
            }


        }



        function clickFirst() {

            first.className = "HistbuttonFirst";
            second.className = "Histbutton";
            third.className = "Histbutton";


            if (panel4.disabled) {
                fourth.style.display = "none";

                if (last) {
                    last.className = "HistButtonLast";
                }
                else {
                    third.className = "HistButtonLast";
                }
            }
            else {
                fourth.style.display = "";
                if (last) {
                    fourth.className = "Histbutton";
                    last.className = "HistButtonLast";
                }
                else {
                    fourth.className = "HistButtonLast";
                }
            }



            if (panel1) { panel1.style.display = "block"; }
            if (panel2) { panel2.style.display = "none"; }
            if (panel3) { panel3.style.display = "none"; }
            if (panel4) { panel4.style.display = "none"; }
            if (panel5) { panel5.style.display = "none"; }

        }


        function clickSecond() {

            first.className = "HistButtonBeforActive";
            second.className = "HistButtonMiddleActive";
            third.className = "Histbutton";

            if (panel4.disabled) {
                fourth.style.display = "none";

                if (last) {
                    last.className = "HistButtonLast";
                }
                else {
                    third.className = "HistButtonLast";
                }
            }
            else {
                fourth.style.display = "";
                if (last) {
                    fourth.className = "Histbutton";
                    last.className = "HistButtonLast";
                }
                else {
                    fourth.className = "HistButtonLast";
                }
            }


            if (panel1) { panel1.style.display = "none"; }
            if (panel2) { panel2.style.display = "block"; }
            if (panel3) { panel3.style.display = "none"; }
            if (panel4) { panel4.style.display = "none"; }
            if (panel5) { panel5.style.display = "none"; }

        }

        function clickThird() {

            first.className = "Histbutton";
            second.className = "HistButtonBeforActive";


            if (panel4.disabled) {
                fourth.style.display = "none";

                if (last) {
                    third.className = "HistButtonMiddleActive";
                    last.className = "HistButtonLast";
                }
                else {
                    third.className = "HistButtonLastActive";
                }
            }
            else {
                fourth.style.display = "";
                if (last) {
                    third.className = "HistButtonMiddleActive";
                    fourth.className = "Histbutton";
                    last.className = "HistButtonLast";
                }
                else {
                    third.className = "HistButtonMiddleActive";
                    fourth.className = "HistButtonLast";
                }
            }

            if (panel1) { panel1.style.display = "none"; }
            if (panel2) { panel2.style.display = "none"; }
            if (panel3) { panel3.style.display = "block"; }
            if (panel4) { panel4.style.display = "none"; }
            if (panel5) { panel5.style.display = "none"; }
        }


        function clickFourth() {

            first.className = "Histbutton";
            second.className = "Histbutton";
            third.className = "HistButtonBeforActive";


            if (panel4.disabled) {
                fourth.style.display = "none";

                if (last) {
                    last.className = "HistButtonLast";
                }
                else {
                    third.className = "HistButtonLast";
                }
            }
            else {
                fourth.style.display = "";
                if (last) {
                    fourth.className = "HistButtonMiddleActive";
                    last.className = "HistButtonLast";
                }
                else {
                    fourth.className = "HistButtonLastActive";
                }
            }


            if (panel1) { panel1.style.display = "none"; }
            if (panel2) { panel2.style.display = "none"; }
            if (panel3) { panel3.style.display = "none"; }
            if (panel4) { panel4.style.display = "block"; }
            if (panel5) { panel5.style.display = "none"; }
        }

        function clickLast() {


            first.className = "Histbutton";
            second.className = "Histbutton";

            if (panel4.disabled) {
                fourth.style.display = "none";

                if (last) {
                    last.className = "HistButtonLastActive";
                    third.className = "HistButtonBeforActive";
                }
                else {
                    third.className = "HistButtonLast";
                }
            }
            else {
                fourth.style.display = "";
                if (last) {
                    third.className = "Histbutton";
                    fourth.className = "HistButtonBeforActive";
                    last.className = "HistButtonLastActive";
                }
                else {
                    fourth.className = "HistButtonLast";
                }
            }


            if (panel1) { panel1.style.display = "none"; }
            if (panel2) { panel2.style.display = "none"; }
            if (panel3) { panel3.style.display = "none"; }
            if (panel4) { panel4.style.display = "none"; }
            if (panel5) { panel5.style.display = "block"; }

        }


    </script>
</asp:Content>
