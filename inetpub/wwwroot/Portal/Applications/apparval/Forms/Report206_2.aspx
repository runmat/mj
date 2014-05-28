<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report206_2.aspx.vb" Inherits="AppARVAL.Report206_2" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Briefdaten)</asp:Label><asp:HyperLink
                                ID="lnkSchluesselinformationen" runat="server" Target="_blank" Visible="False"
                                NavigateUrl="Report38.aspx?chassisnum=">Schlüsselinformationen</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_Uebersicht" runat="server" CssClass="StandardButton"> lb_Uebersicht</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_Typdaten" runat="server" CssClass="StandardButton"> lb_Typdaten</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_Lebenslauf" runat="server" CssClass="StandardButton"> lb_Lebenslauf</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_Uebermittlung" runat="server" CssClass="StandardButton"> lb_Uebermittlung</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx"
                                            CssClass="TaskTitle">Abfragekriterien</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelExtraLarge">
                                        <table id="Table9" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="left" colspan="2">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="800">
                                                        <tr>
                                                            <td align="middle">
                                                                <table id="Table110" cellspacing="0" cellpadding="5" align="left" bgcolor="white"
                                                                    border="0">
                                                                    <tr id="tr_Fahrgestellnummer" runat="server">
                                                                        <td class="TextLarge" nowrap align="right">
                                                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>&nbsp;
                                                                        </td>
                                                                        <td class="TextLargeDescription" nowrap align="left">
                                                                            &nbsp;<asp:Label ID="lblFahrgestellnummerShow" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="middle">
                                                                <table id="Table111" cellspacing="0" cellpadding="5" align="left" bgcolor="white"
                                                                    border="0">
                                                                    <tr id="tr_Kennzeichen" runat="server">
                                                                        <td class="TextLarge" nowrap align="right">
                                                                            <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen </asp:Label>&nbsp;
                                                                        </td>
                                                                        <td class="TextLargeDescription" nowrap align="left">
                                                                            &nbsp;<asp:Label ID="lblKennzeichenShow" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="middle">
                                                                <table id="Table112" cellspacing="0" cellpadding="5" align="left" bgcolor="white"
                                                                    border="0">
                                                                    <tr id="tr_Status" runat="server">
                                                                        <td class="TextLarge" nowrap align="right">
                                                                            <asp:Label ID="lbl_Status" runat="server">lbl_Status </asp:Label>&nbsp;
                                                                        </td>
                                                                        <td class="TextLargeDescription" nowrap align="left">
                                                                            &nbsp;<asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="middle">
                                                                <table id="Table113" cellspacing="0" cellpadding="5" align="left" bgcolor="white"
                                                                    border="0">
                                                                    <tr id="tr_Lagerort" runat="server">
                                                                        <td class="TextLarge" nowrap align="right">
                                                                            <asp:Label ID="lbl_Lagerort" runat="server">lbl_Lagerort</asp:Label>&nbsp;
                                                                        </td>
                                                                        <td class="TextLargeDescription" nowrap align="left">
                                                                            &nbsp;<asp:Label ID="lblLagerortShow" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trUebersicht" runat="server">
                                                <td valign="top" align="left" colspan="2">
                                                    <table id="Table10" cellspacing="0" cellpadding="5" width="800" bgcolor="white" border="0">
                                                        <tr>
                                                            <td class="TaskTitle" colspan="8">
                                                                <asp:Label ID="lbl_Fahrzeugdaten" runat="server">lbl_Fahrzeugdaten</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Hersteller" runat="server">lbl_Hersteller</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblHerstellerShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Fahrzeugmodell" runat="server">lbl_Fahrzeugmodell</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblFahrzeugmodellShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Farbe" runat="server">lbl_Farbe</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
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
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_HerstellerSchluessel" runat="server">lbl_HerstellerSchluessel</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblHerstellerSchluesselShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Typschluessel" runat="server">lbl_Typschluessel</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblTypschluesselShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_VarianteVersion" runat="server">lbl_VarianteVersion</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblVarianteVersionShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TaskTitle" colspan="8">
                                                                <asp:Label ID="lbl_Briefdaten" runat="server">lbl_Briefdaten</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Briefnummer" runat="server">lbl_Briefnummer</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblBriefnummerShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left" width="152">
                                                                <asp:Label ID="lbl_Erstzulassungsdatum" runat="server">lbl_Erstzulassungsdatum</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblErstzulassungsdatumShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Abmeldedatum" runat="server">lbl_Abmeldedatum</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblAbmeldedatumShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Ordernummer" runat="server">lbl_Ordernummer</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblOrdernummerShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_Briefeingang" runat="server">lbl_Briefeingang</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblBriefeingangShow" runat="server"></asp:Label>
                                                                </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                <asp:Label ID="lbl_CoC" runat="server">lbl_CoC</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                <asp:CheckBox ID="cbxCOC" runat="server" TextAlign="Left" Enabled="False"></asp:CheckBox>
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternate" valign="top" nowrap align="left" rowspan="2">
                                                                <asp:Label ID="lbl_Fahrzeughalter" runat="server">lbl_Fahrzeughalter</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right" rowspan="2">
                                                                <asp:Label ID="lblFahrzeughalterShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left" rowspan="2">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="top" nowrap align="left" rowspan="2">
                                                                <asp:Label ID="lbl_Standort" runat="server">lbl_Standort</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right" rowspan="2">
                                                                <asp:Label ID="lblStandortShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternate" colspan="3">
                                                                &nbsp;</td>
                                                        </tr>
             <tr>
                                                            <td class="TaskTitle" valign="center" nowrap align="left" width="152" 
                                                                colspan="8">
                                                                <asp:Label ID="lbl_Abmeldedaten" runat="server">lbl_Abmeldedaten</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" valign="center" align="left" width="152">
                                                            <asp:Label ID="lbl_CarportEing" runat="server">lbl_CarportEing</asp:Label>

                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" align="right">
                                                                <asp:Label ID="lblPDIEingang" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="TextLarge" valign="center" align="left">
                                                                
                                                               <asp:Label ID="lbl_KennzVorhanden" runat="server">lbl_KennzVorhanden</asp:Label>
                                                                                                                            
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" align="right">
                                                                
                                                                <asp:CheckBox ID="chkVorhandeneElemente" runat="server" Enabled="False"></asp:CheckBox>
                                                                
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternate" valign="center" align="left" width="152">
                                                                <asp:Label ID="lbl_CheckIn" runat="server">lbl_CheckIn</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" align="right">
                                                                <asp:Label ID="lblCheckIn" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" align="left">
                                                                <asp:Label ID="lbl_Fahrzeugschein" runat="server">lbl_Fahrzeugschein</asp:Label>:
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" align="right">
                                                                <asp:CheckBox ID="chkFahzeugschein" runat="server" Enabled="False"></asp:CheckBox>
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="TaskTitle" valign="center" nowrap align="left" width="152" 
                                                                colspan="8">
                                                                <asp:Label ID="lbl_Aenderungsdaten" runat="server">lbl_Aenderungsdaten</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" valign="center" nowrap align="left" width="152">
                                                                <asp:Label ID="lbl_EhemaligesKennzeichen" runat="server">lbl_EhemaligesKennzeichen</asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblEhemaligesKennzeichenShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="TextLarge" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left" width="152">
                                                                <asp:Label ID="lbl_EhemaligeBriefnummer" runat="server">lbl_EhemaligeBriefnummer</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                <asp:Label ID="lblEhemaligeBriefnummerShow" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="center" nowrap align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" nowrap align="right">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trTypdaten" runat="server">
                                                <td valign="top" align="left" colspan="2">
                                                    <table id="Table4" cellspacing="0" cellpadding="2" width="800" bgcolor="white" border="0">
                                                        <tr>
                                                            <td class="TaskTitle" valign="top" align="left" colspan="6">
                                                                Allgemeine Fahrzeugdaten
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63" height="19">
                                                                1
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap height="19">
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                2
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                3
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                4
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63" height="23">
                                                                5
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap height="23">
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63" height="22">
                                                                6
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap height="22">
                                                                Fabrikname:
                                                            </td>
                                                            <td class="ABEDaten" colspan="3" height="22">
                                                                <asp:Label ID="lbl_8" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                7
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                8
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Anzahl Sitze:
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_26" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                9
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Zul. Gesamtgewicht (kg):
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_28" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                10
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Länge min. (mm)
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_31" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                11
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Breite min. (mm)
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_32" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                12
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Höhe min. (mm)
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_33" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TaskTitle" align="right" width="63" colspan="6">
                                                                Antriebsdaten
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                1
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Hubraum (cm³)
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_11" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                2
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                3
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Höchstgeschwindigkeit (km/h):
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_12" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                4
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
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
                                                            <td class="TaskTitle" nowrap align="left" colspan="6">
                                                                Kraftstoff / Tank
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                1
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
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
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                2
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Fassungsvermögen Tank (m³):
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_21" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63" height="22">
                                                                3
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap height="22">
                                                                Co2-Gehalt (g/km):
                                                            </td>
                                                            <td class="ABEDaten" colspan="3" height="22">
                                                                <asp:Label ID="lbl_17" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63" height="22">
                                                                4
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap height="22">
                                                                Nat. Emissionsklasse:
                                                            </td>
                                                            <td class="ABEDaten" colspan="3" height="22">
                                                                <asp:Label ID="lbl_18" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63" height="22">
                                                                5
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap height="22">
                                                                Abgasrichtlinie:
                                                            </td>
                                                            <td class="ABEDaten" colspan="3" height="22">
                                                                <asp:Label ID="lbl_22" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%" height="22">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TaskTitle" align="left" colspan="6">
                                                                Achsen / Bereifung
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                1
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Anzahl Achsen:
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_23" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                2
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Anzahl Antriebsachsen:
                                                            </td>
                                                            <td class="ABEDaten" colspan="3">
                                                                <asp:Label ID="lbl_24" runat="server">-</asp:Label>
                                                            </td>
                                                            <td class="ABEDaten" align="left" width="100%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                3
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Max. Achslast Achse 1,2,3 (kg):
                                                            </td>
                                                            <td class="ABEDaten" colspan="4">
                                                                <asp:Label ID="lbl_25" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" align="right" width="63">
                                                                4
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                                Bereifung Achse 1,2,3:
                                                            </td>
                                                            <td class="ABEDaten" colspan="4">
                                                                <asp:Label ID="lbl_27" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TaskTitle" align="left" width="63" colspan="6">
                                                                Bemerkungen
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" valign="top" align="right">
                                                                1
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" nowrap>
                                                            </td>
                                                            <td class="ABEDaten" colspan="4">
                                                                <asp:Label ID="lbl_30" runat="server" Font-Names="Arial" Font-Size="XX-Small">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trLebenslauf" runat="server">
                                                <td valign="top" align="left" colspan="2">
                                                    <asp:DataGrid ID="Datagrid2" runat="server" BackColor="White" AutoGenerateColumns="False"
                                                        Width="800px" AllowPaging="True" AllowSorting="True" bodyHeight="300" CssClass="tableMain"
                                                        bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50">
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle Wrap="False" CssClass="TaskTitle"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="KURZTEXT" SortExpression="KURZTEXT" HeaderText="Vorgang">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="STRMN" SortExpression="STRMN" HeaderText="Durchf&#252;hrungs-&lt;br&gt;datum"
                                                                DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Versandadresse">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.NAME2_Z5") %>'>
                                                                    </asp:Label>
                                                                    <asp:Literal ID="Literal1" runat="server" Text="<br>"></asp:Literal>
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STREET_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.HOUSE_NUM1_Z5") %>'>
                                                                    </asp:Label>
                                                                    <asp:Literal ID="Literal2" runat="server" Text="<br>"></asp:Literal>
                                                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1_Z5") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.CITY1_Z5") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
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
                                                </td>
                                            </tr>
                                            <tr id="trUebermittlung" runat="server">
                                                <td valign="top" align="left" colspan="2">
                                                    <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False"
                                                        Width="800px" AllowPaging="True" AllowSorting="True" bodyHeight="300" CssClass="tableMain"
                                                        bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50">
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle Wrap="False" CssClass="TaskTitle"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="MNCOD" SortExpression="MNCOD" HeaderText="Aktionscode">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="MATXT" SortExpression="MATXT" HeaderText="Vorgang"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PSTER" SortExpression="PSTER" HeaderText="Statusdatum" DataFormatString="{0:dd.MM.yyyy}">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ZZUEBER" SortExpression="ZZUEBER" HeaderText="&#220;bermittlungs-&lt;br&gt;datum"
                                                                DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="False" DataField="AEZEIT" SortExpression="AEZEIT" HeaderText="&#196;nderungs-&lt;br&gt;Zeit"
                                                                DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                        </Columns>
                                                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                            HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                            <!-- jj -->
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <!--#include File="../../../PageElements/Footer.html" -->
    </form>
</body>
</html>
