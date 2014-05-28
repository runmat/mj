<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change210.aspx.vb" Inherits="AppARVAL.Change210" %>

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
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Auftragsdaten)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trContinue" runat="server">
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdContinue" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trConfirm" runat="server">
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="StandardButton"> &#149;&nbsp;Bestätigen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trBack" runat="server">
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Abbrechen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trNew" runat="server">
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdNew" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neuer Auftrag</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" height="830">
                                        <table class="TableBackGround" id="t4" cellspacing="3" cellpadding="3" border="0">
                                            <tr>
                                                <td class="GridTableHead" colspan="2">
                                                    <table id="Table4" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td nowrap width="33%">
                                                                <strong>Datum:</strong>
                                                            </td>
                                                            <td nowrap width="33%">
                                                                <strong><u>Leasingvertrags-Nr*:</u></strong>
                                                            </td>
                                                            <td nowrap width="33%">
                                                                <strong>KFZ - Kennzeichen:</strong>
                                                            </td>
                                                            <td nowrap width="33%">
                                                                <strong></strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap width="33%">
                                                                <asp:Label ID="lblAuftragsdatum" runat="server"></asp:Label>
                                                            </td>
                                                            <td nowrap width="33%">
                                                                <asp:TextBox ID="txtLeasingvertragsNr" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td nowrap width="33%">
                                                                <asp:TextBox ID="txtKennz" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td nowrap width="33%">
                                                                <asp:TextBox ID="txtSachbearbeiter" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridTableHead" valign="top">
                                                    Auftrag für:<br>
                                                    <table class="TableFrame" id="Table5" cellspacing="0" cellpadding="3" border="0"
                                                        width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxErsatzKfzSchein" runat="server" Font-Bold="True" Text="Ersatz Kfz-Schein"
                                                                    GroupName="selection1"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxNeuesSchild" runat="server" Font-Bold="True" Text="Neues Schild"
                                                                    GroupName="selection1"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxUmmeldung" runat="server" Font-Bold="True" Text="Ummeldung"
                                                                    GroupName="selection1"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxUmkennzeichnung" runat="server" Font-Bold="True" Text="Umkennzeichnung"
                                                                    GroupName="selection1"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxVersicherungswechsel" runat="server" Font-Bold="True" Text="Versicherungswechsel"
                                                                    GroupName="selection1"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxBriefaufbietung" runat="server" Font-Bold="True" Text="Briefaufbietung"
                                                                    GroupName="selection1"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxTempversand" runat="server" Font-Bold="True" Text="Temp. Versand"
                                                                    GroupName="selection1"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="cbxSonstiges" runat="server" Font-Bold="True" Text="Sonstiges"
                                                                    GroupName="selection1" Checked="True"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="GridTableHead" valign="top">
                                                    Fahrzeugschein/-schilder an<br>
                                                    <table class="TableFrame" id="Table1" cellspacing="0" cellpadding="3" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Firma:</strong>
                                                            </td>
                                                            <td width="10%">
                                                                <asp:TextBox ID="txtZielFirma" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Firma 2:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtZielFirma2" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Strasse/Hs.-Nr.:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtZielStrasse" runat="server" Width="160px"></asp:TextBox><asp:TextBox
                                                                    ID="txtZielHNr" runat="server" Width="20px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>PLZ/Ort:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtZielPLZ" runat="server" Width="40px"></asp:TextBox><asp:TextBox
                                                                    ID="txtZielOrt" runat="server" Width="140px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Telefon:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtZielTelefon" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridTableHead" valign="top">
                                                    Alter Halter<br>
                                                    <table class="TableFrame" cellspacing="0" cellpadding="3" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Firma:</strong>
                                                            </td>
                                                            <td width="10%">
                                                                <asp:TextBox ID="txtHalterAltFirma" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Firma 2:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterAltFirma2" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Strasse/Hs.-Nr.:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterAltStrasse" runat="server" Width="160px"></asp:TextBox><asp:TextBox
                                                                    ID="txtHalterAltHNr" runat="server" Width="20px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>PLZ/Ort:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterAltPLZ" runat="server" Width="40px"></asp:TextBox><asp:TextBox
                                                                    ID="txtHalterAltOrt" runat="server" Width="140px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Telefon:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterAltTelefon" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="GridTableHead" valign="top">
                                                    Neuer Halter<br>
                                                    <table class="TableFrame" cellspacing="0" cellpadding="3" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Firma:</strong>
                                                            </td>
                                                            <td width="10%">
                                                                <asp:TextBox ID="txtHalterNeuFirma" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Firma 2:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterNeuFirma2" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Strasse/Hs.-Nr.:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterNeuStrasse" runat="server" Width="160px"></asp:TextBox><asp:TextBox
                                                                    ID="txtHalterNeuHNr" runat="server" Width="20px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>PLZ/Ort:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterNeuPLZ" runat="server" Width="40px"></asp:TextBox><asp:TextBox
                                                                    ID="txtHalterNeuOrt" runat="server" Width="140px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap align="right">
                                                                <strong>Telefon:</strong>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:TextBox ID="txtHalterNeuTelefon" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridTableHead" valign="top" nowrap colspan="2">
                                                    <table id="Table8" cellspacing="0" cellpadding="3" border="0">
                                                        <tr>
                                                            <td class="GridTableHead" valign="top">
                                                                Fahrzeugdaten:
                                                            </td>
                                                            <td valign="top">
                                                                <table id="Table7" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <strong>&nbsp; </strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableHead" valign="top">
                                                                Amtl. Kennzeichen
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox ID="txtAmtlichesKennzeichen" runat="server" Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableHead" valign="top">
                                                                Wunschkennzeichen:
                                                            </td>
                                                            <td valign="top">
                                                                <table id="Table4" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td nowrap width="33%">
                                                                            <asp:TextBox ID="txtWKZ1" runat="server" Width="80px"></asp:TextBox>&nbsp;<strong>1.&nbsp;&nbsp;&nbsp;
                                                                            </strong>
                                                                        </td>
                                                                        <td nowrap width="33%">
                                                                            <asp:TextBox ID="txtWKZ2" runat="server" Width="80px"></asp:TextBox>&nbsp;<strong>2.&nbsp;&nbsp;
                                                                            </strong>
                                                                        </td>
                                                                        <td nowrap width="33%">
                                                                            <asp:TextBox ID="txtWKZ3" runat="server" Width="80px"></asp:TextBox>&nbsp;<strong>3.</strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableHead">
                                                                Reservierungsnummer/-name:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReservierungsnummer" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="GridTableHead">
                                                                Gewünschter Zulassungstermin:&nbsp; &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtZulassungstermin" runat="server" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridTableHead" valign="top">
                                                    Alter Versicherungsträger<br>
                                                    <asp:TextBox ID="txtVersicherungAlt" runat="server" Width="280px"></asp:TextBox>
                                                </td>
                                                <td class="GridTableHead" valign="top">
                                                    Neuer Versicherungsträger<br>
                                                    <asp:TextBox ID="txtVersicherungNeu" runat="server" Width="280px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridTableHead" valign="top" colspan="2">
                                                    Bemerkung<br>
                                                    <asp:TextBox ID="txtBemerkung" runat="server" Width="580px" TextMode="MultiLine"
                                                        Height="40px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        *<u>Pflichtfeld</u>.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <!--		<script language="JavaScript">
<!-- //
 window.document.Form1.txtVertragsNr.focus();
//- - >
		</script>-->
</body>
</html>
