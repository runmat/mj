<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report444_2.aspx.vb" Inherits="AppDCB.Report444_2" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
    <table width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Briefdaten)</asp:Label>
                            <asp:HyperLink ID="lnkSchluesselinformationen" runat="server" NavigateUrl="Report38.aspx?chassisnum="
                                Visible="False" Target="_blank">Schlüsselinformationen</asp:HyperLink>
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
                                        <asp:LinkButton ID="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Sichern</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Equipment.aspx">Abfragekriterien</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelExtraLarge">
                                        <table id="Table9" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="middle" colspan="2">
                                                    <table id="Table10" cellspacing="0" cellpadding="5" align="left" bgcolor="white"
                                                        border="0">
                                                        <tr>
                                                            <td class="" valign="center" align="left" colspan="2">
                                                                <strong>Status:</strong>&nbsp;
                                                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="" valign="center" align="left">
                                                            </td>
                                                            <td class="" valign="center" align="right" colspan="2" nowrap>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableAlternate" valign="center" align="left" width="152">
                                                                <strong>Fahrzeugdaten</strong>
                                                            </td>
                                                            <td class="GridTableAlternate" valign="top" align="right">
                                                                &nbsp;
                                                            </td>
                                                            <td class="GridTableAlternate" valign="center" align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="GridTableAlternate" valign="center" align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="GridTableAlternate" valign="top" align="right">
                                                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="" Target="_blank">ABE-Daten</asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLargeDescription" valign="center" align="left" width="152">
                                                                Kfz-Kennzeichen:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblKennzeichen" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                Kfz-Briefnummer:&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblBriefnummer" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr5" runat="server">
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left" width="152">
                                                                Fahrgestellnummer:
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="top" align="right">
                                                                <asp:Label ID="lblFahrgestellnummer" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left">
                                                                Briefeingang:
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="top" align="right">
                                                                <asp:Label ID="lblBriefeingang" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLargeDescription" valign="center" align="left" width="152">
                                                                Hersteller:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblHersteller" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                Fahrzeugmodell:&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblFahrzeugmodell" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" valign="top" align="left" width="152">
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="top" align="right">
                                                                <asp:Label ID="lblEingangsdatum" runat="server" Visible="False"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="top" align="left">
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="top" align="right">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableAlternate" valign="center" align="left" width="152">
                                                                <strong>Briefdaten</strong>
                                                            </td>
                                                            <td class="GridTableAlternate" valign="top" align="right">
                                                                &nbsp;
                                                            </td>
                                                            <td class="GridTableAlternate" valign="center" align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="GridTableAlternate" valign="center" align="left">
                                                                &nbsp;
                                                            </td>
                                                            <td class="GridTableAlternate" valign="top" align="right">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="" valign="center" align="left" width="152">
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr1" runat="server">
                                                            <td class="TextLargeDescription" valign="center" align="left" width="152">
                                                                Erstzulassungsdatum:&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblErstzulassungsdatum" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                Leasingvertrags-Nr.:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblOrdernummer" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLargeDescription" valign="center" align="left" width="152">
                                                                <asp:Label ID="Label3" runat="server">CoC</asp:Label>
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:CheckBox ID="cbxCOC" runat="server" TextAlign="Left" Enabled="False"></asp:CheckBox>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                Fahrzeughalter:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblFahrzeughalter" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableAlternate" valign="top" colspan="5">
                                                                <strong>Abmeldedaten</strong>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLargeDescription" valign="center" align="left" width="152">
                                                                Carport-Eingang:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblPDIEingang" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                                Kennzeicheneingang:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblKennzeicheneingang" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left" width="152">
                                                                Check-In:
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="top" align="right">
                                                                <asp:Label ID="lblCheckIn" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="StandardTableAlternateDescription" valign="center" align="left">
                                                                Fahrzeugschein:&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" valign="top" align="right">
                                                                <asp:CheckBox ID="chkFahzeugschein" runat="server" Enabled="False"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLargeDescription" valign="top" align="left" width="152">
                                                                Beide Kennzeichen vorhanden:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:CheckBox ID="chkVorhandeneElemente" runat="server" Enabled="False"></asp:CheckBox>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="center" align="left">
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" align="left">
                                                                Stillegung:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblStillegung" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableAlternate" valign="top" colspan="5">
                                                                <strong>Letzte Versanddaten</strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLargeDescription" valign="top" align="left" colspan="2">
                                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblVersendetAmDescription" runat="server">Versendet am:</asp:Label>
                                                                        </td>
                                                                        <td class="TextLarge" align="right">
                                                                            <asp:Label ID="lblAngefordertAm" runat="server"></asp:Label><asp:Label ID="lblVersendetAm"
                                                                                runat="server" Visible="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblVersandart" runat="server" DESIGNTIMEDRAGDROP="45">Versandart:&nbsp;</asp:Label>
                                                                        </td>
                                                                        <td class="TextLarge" nowrap align="right">
                                                                            <asp:Label ID="Label1" runat="server" DESIGNTIMEDRAGDROP="46">   temporär</asp:Label>
                                                                            <asp:RadioButton ID="rbTemporaer" runat="server" GroupName="Versandart" Enabled="False">
                                                                            </asp:RadioButton>
                                                                            <asp:Label ID="Label2" runat="server">   endgültig</asp:Label>
                                                                            <asp:RadioButton ID="rbEndgueltig" runat="server" GroupName="Versandart" Enabled="False">
                                                                            </asp:RadioButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" align="left">
                                                            </td>
                                                            <td class="TextLargeDescription" valign="top" align="left">
                                                                Versandanschrift:
                                                            </td>
                                                            <td class="TextLarge" valign="top" align="right">
                                                                <asp:Label ID="lblVersandanschrift" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
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
                    <tr>
                        <td width="120">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
