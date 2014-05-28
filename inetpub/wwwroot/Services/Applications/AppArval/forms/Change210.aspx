<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change210.aspx.vb"
    Inherits="AppArval.Change210" MasterPageFile="../../../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            Datum:
                                            <asp:Label ID="lblAuftragsdatum" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Leasingvertrags-Nr*:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLeasingvertragsNr" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            KFZ - Kennzeichen:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtKennz" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="txtSachbearbeiter" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="firstLeft active">
                                            Auftrag für:
                                        </td>
                                        <td colspan="3" nowrap="nowrap">
                                            <ul>
                                                <asp:RadioButton ID="cbxErsatzKfzSchein" runat="server" CssClass="radio" Text="Ersatz Kfz-Schein"
                                                    GroupName="selection1"></asp:RadioButton>
                                                <asp:RadioButton ID="cbxNeuesSchild" runat="server" CssClass="radio" Text="Neues Schild"
                                                    GroupName="selection1"></asp:RadioButton>
                                                <asp:RadioButton ID="cbxUmmeldung" runat="server" CssClass="radio" Text="Ummeldung"
                                                    GroupName="selection1"></asp:RadioButton>
                                                <asp:RadioButton ID="cbxUmkennzeichnung" runat="server" CssClass="radio" Text="Umkennzeichnung"
                                                    GroupName="selection1"></asp:RadioButton>
                                                <ul>
                                                </ul>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td colspan="3" nowrap="nowrap">
                                            <ul>
                                                <asp:RadioButton ID="cbxVersicherungswechsel" runat="server" CssClass="radio" Text="Versicherungswechsel"
                                                    GroupName="selection1"></asp:RadioButton>
                                                <asp:RadioButton ID="cbxBriefaufbietung" runat="server" CssClass="radio" Text="Briefaufbietung"
                                                    GroupName="selection1"></asp:RadioButton>
                                                <asp:RadioButton ID="cbxTempversand" runat="server" CssClass="radio" Text="Temp. Versand"
                                                    GroupName="selection1"></asp:RadioButton>
                                                <asp:RadioButton ID="cbxSonstiges" runat="server" CssClass="radio" Text="Sonstiges"
                                                    GroupName="selection1" Checked="True"></asp:RadioButton>
                                                <ul>
                                                </ul>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            Fahrzeugschein/-schilder an
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Firma:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZielFirma" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Firma 2:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZielFirma2" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Strasse:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZielStrasse" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Hausnummer:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZielHNr" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            PLZ:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZielPLZ" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Ort:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZielOrt" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Telefon:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZielTelefon" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            Alter Halter
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Firma:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterAltFirma" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Firma 2:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterAltFirma2" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Strasse:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterAltStrasse" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Hausnummer:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterAltHNr" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            PLZ:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterAltPLZ" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Ort:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterAltOrt" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Telefon:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterAltTelefon" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            Neuer Halter
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Firma:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterNeuFirma" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Firma 2:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterNeuFirma2" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Strasse:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterNeuStrasse" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Hausnummer:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterNeuHNr" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            PLZ:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterNeuPLZ" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            Ort:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterNeuOrt" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Telefon:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHalterNeuTelefon" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            Fahrzeugdaten
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Amtliches Kennzeichen:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAmtlichesKennzeichen" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Wunschkennzeichen:
                                        </td>
                                        <td class="active">
                                            1.
                                            <asp:TextBox ID="txtWKZ1" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="active">
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="active">
                                            2.
                                            <asp:TextBox ID="txtWKZ2" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="active">
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="active">
                                            3.
                                            <asp:TextBox ID="txtWKZ3" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="active">
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Reservierungsnummer/-name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReservierungsnummer" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Gewünschter Zulassungstermin:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZulassungstermin" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Alter Versicherungsträger
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVersicherungAlt" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Neuer Versicherungsträger
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVersicherungNeu" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Bemerkung
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtBemerkung" runat="server" CssClass="TextBoxXLarge" TextMode="MultiLine"
                                                Height="40px"></asp:TextBox>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            *Pflichtfeld
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                <tr id="trContinue" runat="server">
                                    <td>
                                        <asp:LinkButton ID="cmdContinue" Text="Weiter" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trConfirm" runat="server">
                                    <td>
                                        <asp:LinkButton ID="cmdConfirm" Text="Bestätigen" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trBack" runat="server">
                                    <td>
                                        <asp:LinkButton ID="cmdBack" Text="Abbrechen" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trNew" runat="server">
                                    <td>
                                        <asp:LinkButton ID="cmdNew" Text="Neuer Auftrag" Height="16px" Width="88px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
