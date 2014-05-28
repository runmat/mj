<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WSÜbersicht.ascx.vb"
    Inherits="CKG.Components.Logistik.WSÜbersicht" %>
<table cellspacing="0" cellpadding="0">
    <tr>
        <td style="padding-bottom: 0px;" class="PanelHead">
            <asp:Label runat="server">Übersicht</asp:Label>
        </td>
        <td align="right">
            <asp:ImageButton ID="ibtnCreatePDF" Style="height: 25px; padding-top: 10px; width: 22px"
                OnClick="CreatePDFClick" ToolTip="PDF herunterladen" runat="server" ImageUrl="/services/Images/pdf-logo.png"
                Visible="False" />
            &nbsp;<asp:Label ID="lblPDFPrint" runat="server" Text="Auftrag als PDF" Height="20px"
                Visible="False" Style="padding-right: 10px; padding-top: 10px" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-top: 0px;">
            <asp:Label runat="server" Text="Bitte überprüfen Sie hier noch einmal die Auftragsdaten zur Überführung." /><br />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div style="min-height: 332px;">
                <table cellpadding="0" class="review" style="width: 875px;">
                    <tr class="StandardHeadDetail" style="cursor: default;">
                        <th colspan="2" style="text-align: left;">
                            <div style="padding-top: 0;">
                                Fahrzeug 1 (<asp:Literal ID="lFzg1" runat="server" />)
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left; vertical-align: top;">
                            <asp:GridView ID="gvFzg1" runat="server" AutoGenerateColumns="False" GridLines="None"
                                ShowHeader="False" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Typ" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="Name" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="Adresse" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="PLZOrt" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="Termin" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr id="trFzg2Header" runat="server" visible="false" class="StandardHeadDetail" style="cursor: default;">
                        <th colspan="2" style="text-align: left;">
                            <div style="padding-top: 0;">
                                Fahrzeug 2 (<asp:Literal ID="lFzg2" runat="server" />)
                            </div>
                        </th>
                    </tr>
                    <tr id="trFzg2Data" runat="server" visible="false">
                        <td colspan="2" style="text-align: left; vertical-align: top;">
                            <asp:GridView ID="gvFzg2" runat="server" AutoGenerateColumns="False" GridLines="None"
                                ShowHeader="False" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Typ" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="Name" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="Adresse" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="PLZOrt" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="Termin" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr class="StandardHeadDetail" style="cursor: default;">
                        <th style="text-align: left;">
                            <div style="padding-top: 0;">
                                Rechnungszahler
                            </div>
                        </th>
                        <th style="text-align: left;">
                            <div style="padding-top: 0;">
                                Rechnungsempfänger
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <td style="text-align: left; vertical-align: top;">
                            <table cellpadding="0" cellspacing="0" class="review">
                                <tr>
                                    <td class="header">
                                        Firma:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="RechnungszahlerFirma" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="header">
                                        Straße, Nr.:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="RechnungszahlerStrasse" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="header">
                                        PLZ, Ort:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="RechnungszahlerPlz" />,
                                        <asp:Label runat="server" ID="RechnungszahlerOrt" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <table cellpadding="0" cellspacing="0" class="review">
                                <tr>
                                    <td class="header">
                                        Firma:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="RechnungsempfängerFirma" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="header">
                                        Straße, Nr.:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="RechnungsempfängerStrasse" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="header">
                                        PLZ, Ort:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="RechnungsempfängerFirmaPlz" />,
                                        <asp:Label runat="server" ID="RechnungsempfängerFirmaOrt" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="rightAlignedNav separator" style="width: 895px;">
                <asp:LinkButton CssClass="blueButton" ID="lbSubmit" OnClick="OnSubmitClick" OnClientClick="DisableButtonWithDelay(this);" runat="server" Text="Absenden" />
                <asp:LinkButton CssClass="blueButton" ID="lbPrevious" OnClick="OnPreviousClick" runat="server" Text="&lt; Zur&uuml;ck" />
                <asp:LinkButton CssClass="blueButton" ID="lbNewOrderSameAdress" OnClick="NewOrderSameAdress" runat="server" Text="&lt; Auftrag kopieren" ToolTip="Neuer Auftrag mit gleichen Adressen." Visible="False" />
                <asp:LinkButton CssClass="blueButton" ID="lbBackToStart" OnClick="BackToStartClick" runat="server" Text="&lt; Neuer Auftrag" Visible="false" />
            </div>
        </td>
    </tr>
</table>
