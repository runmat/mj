<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TransportAddress.ascx.vb"
    Inherits="CKG.Components.Logistik.TransportAddress" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
    TagPrefix="custom" %>
<%@ Register Assembly="CKG.Components.Logistik" Namespace="CKG.Components.Logistik"
    TagPrefix="uc" %>
<script type="text/javascript">
    function checkAbDatum(sender, args) 
    {
        var jetzt = new Date()
        var heute = new Date(jetzt.getFullYear(), jetzt.getMonth(), jetzt.getDate())
        if (sender._selectedDate < heute)
        {
            alert("Sie können kein Datum wählen, das in der Vergangenheit liegt!");
            sender._selectedDate = heute;
            sender._textbox.set_Value(sender._selectedDate.format("dd.MM.yyyy"));
        }
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="CarportmessageError" runat="server" Text="" Visible="false" Style="border: 2px solid #fff;
            color: #fff !important; margin: 0 15px 15px 0; padding: 2px 5px 2px 30px; background: #c00 url(../../Images/Zulassung/error_icon.png) no-repeat 8px 25%;
            display: block; overflow: hidden; -moz-border-radius: 4px 4px 4px 4px; -webkit-border-radius: 4px 4px 4px 4px;
            border-radius: 4px 4px 4px 4px; -moz-box-shadow: 1px 1px 4px #898989; -webkit-box-shadow: 1px 1px 4px #898989;
            box-shadow: 1px 1px 4px #898989; font-weight: bold; width: 400px;"></asp:Label>
        <asp:Label ID="CarportmessageErfolg" runat="server" Text="" Visible="false" Style="border: 2px solid #fff;
            color: #000 !important; margin: 0 15px 15px 0; padding: 2px 5px 2px 30px; display: block;
            overflow: hidden; -moz-border-radius: 4px 4px 4px 4px; -webkit-border-radius: 4px 4px 4px 4px;
            border-radius: 4px 4px 4px 4px; -moz-box-shadow: 1px 1px 4px #898989; -webkit-box-shadow: 1px 1px 4px #898989;
            box-shadow: 1px 1px 4px #898989; font-weight: bold; width: 400px;"></asp:Label>
        <div style="float: right;">
            <div id="ipTransporttyp" runat="server" class="infopanel" style="float: none; width: 320px;
                margin-bottom: 15px;">
                <label>
                    Tipp!</label>
                <div>
                    Über den Transporttyp wird die Art der Fahrt definiert, und es werden automatisch
                    unterschiedliche Dienstleistungen ermittelt.</div>
            </div>
            <div id="fahrzeugdaten" runat="server" style="padding-top: 5px; background-image: url(/services/Images/Logistik/BackDetails.png);
                background-repeat: no-repeat; width: 320px;">
                <span class="style1" style="padding-left: 5px; font-weight: bold;">Fahrzeugdaten</span>
                <br />
                <br />
                <label style="padding-left: 5px; height: 15px;">
                    Fahrzeugtyp:
                </label>
                &nbsp;
                <asp:Label ID="lblAbDetailTyp" runat="server" Height="15px" Font-Bold="true" /><br />
                <label style="padding-left: 5px; height: 15px;">
                    Kennzeichen:
                </label>
                &nbsp;
                <asp:Label ID="lblAbDetailKennzeichen" runat="server" Height="15px" Font-Bold="true" /><br />
                <label style="padding-left: 5px; height: 12px;">
                    Fahrgestellnummer:
                </label>
                &nbsp;
                <asp:Label ID="lblAbDetailFin" runat="server" Height="12px" Font-Bold="true" />
            </div>
            <div runat="server" style="width: 320px; height: 10px;">
            </div>
            <div id="divZusatzfahrtCheckbox" runat="server" Visible="false">
                <input type="checkbox" class="zusatzfahrtCheckbox" runat="server" id="cbxShowZusatzfahrten">Zusatzfahrten einblenden</input>
            </div>
            <div id="filler" runat="server" style="width: 320px; height: 35px;">
            </div>
            <div id="ipUeberfuehrung" runat="server" class="infopanel" style="float: none; width: 320px; margin-top: 35px;">
                <label>
                    Tipp!</label>
                <div>
                    Falls Ihr gewünschtes Überführungsdatum mehr als 3 Tage in der Zukunft liegt, können
                    Sie es hier angeben. Zusätzlich zum Datum können Sie auch eine gewünschte Uhrzeit
                    für die Überführung angeben (keine Pflichtfelder).</div>
            </div>
        </div>
        <asp:Panel runat="server" DefaultButton="dummySearchButton">
            <asp:Button runat="server" ID="dummySearchButton" OnClick="OnSearchClick" Style="display: none;" />
            <table cellspacing="0" cellpadding="0" style="padding-left: 0px; width: 520px;">
                <tr id="trTransporttyp" runat="server">
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_Transporttyp" runat="server">Transporttyp</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTransporttyp" runat="server" CssClass="long" DataTextField="Text"
                            DataValueField="Id" />
                        <asp:CompareValidator ID="cvTransporttyp" runat="server" EnableClientScript="false"
                            ValueToCompare="00" Operator="NotEqual" ControlToValidate="ddlTransporttyp" Display="None"
                            SetFocusOnError="true" ErrorMessage="Bitte geben Sie einen Transporttyp an." />
                    </td>
                </tr>
                <tr>
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbFirma" runat="server">lbl_AbFirma</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbFirma" runat="server" MaxLength="35" CssClass="long" />
                        <asp:RequiredFieldValidator ID="rfvAbFirma" runat="server" EnableClientScript="false"
                            ControlToValidate="txtAbFirma" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine Firma an." />
                        <asp:LinkButton ID="lbSearch" runat="server" Text="Suchen" CssClass="greyButton search"
                            OnClick="OnSearchClick" />
                    </td>
                </tr>
                <tr>
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbStrasse" runat="server">lbl_AbStrasse</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbStrasse" runat="server" MaxLength="35" CssClass="long" />
                        <asp:RequiredFieldValidator ID="rfvAbStrasse" runat="server" EnableClientScript="false"
                            ControlToValidate="txtAbStrasse" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine Straße an." />
                        <asp:ImageButton ID="ibKM" runat="server" ImageUrl="/services/images/BAB.png" ToolTip="Berechnung der Entfernungskilometer."
                            Style="vertical-align: middle;" Visible="false" OnClick="OnKmClick" />
                    </td>
                </tr>
                <tr>
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbPlzOrt" runat="server">lbl_AbPlzOrt</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbPLZ" runat="server" MaxLength="7" CssClass="short" />
                        <asp:TextBox ID="txtAbOrt" runat="server" MaxLength="35" CssClass="middle" />
                        <asp:RequiredFieldValidator ID="rfvAbPLZ" runat="server" EnableClientScript="false"
                            ControlToValidate="txtAbPLZ" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine Postleitzahl an." />
                        <asp:CustomValidator ID="cvAbPLZ" runat="server" EnableClientScript="false" ControlToValidate="txtAbPLZ"
                            Display="None" SetFocusOnError="true" ErrorMessage="Bitte überprüfen Sie den Postleitzahl."
                            OnServerValidate="OnValidatePostcode" />
                        <asp:RequiredFieldValidator ID="rfvAbOrt" runat="server" EnableClientScript="false"
                            ControlToValidate="txtAbOrt" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie einen Ort an." />
                    </td>
                </tr>
                <tr>
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbLand" runat="server">lbl_AbLand</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAbLand" runat="server" CssClass="long" DataTextField="FullDesc"
                            DataValueField="Land1" />
                    </td>
                </tr>
                <tr id="trAnsprechpartner" runat="server">
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbAnsprechpartner" runat="server">lbl_AbAnsprechpartner</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbAnsprechpartner" runat="server" MaxLength="35" CssClass="long" />
                        <asp:RequiredFieldValidator ID="rfvAbAnsprechpartner" runat="server" EnableClientScript="false"
                            ControlToValidate="txtAbAnsprechpartner" Display="None" SetFocusOnError="true"
                            ErrorMessage="Bitte geben Sie einen Ansprechpartner an." />
                    </td>
                </tr>
                <%--<tr id="trPosText" runat="server">
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_Pos_Text" runat="server">lbl_Pos_Text</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtlbl_Pos_Text" runat="server" MaxLength="35" CssClass="long" />
                    </td>
                </tr>--%>
                <tr id="trTelefon" runat="server">
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbTelefon" runat="server">lbl_AbTelefon</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbTelefon" runat="server" MaxLength="16" CssClass="long" />
                        <asp:RequiredFieldValidator ID="rfvAbtelefon" runat="server" EnableClientScript="false"
                            ControlToValidate="txtAbTelefon" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine Telefonnummer an." />
                    </td>
                </tr>
                <tr id="trEmail" runat="server">
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbEMail" runat="server">lbl_AbEMail</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbEMail" runat="server" MaxLength="240" CssClass="long" />
                    </td>
                </tr>

                <tr id="trDatum" runat="server">
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbDatum" runat="server">lbl_AbDatum</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbDatum" runat="server" CssClass="long" />
                        <cc1:TextBoxWatermarkExtender ID="ExAbDatum" runat="server" TargetControlID="txtAbDatum"
                            WatermarkText="kein Fixtermin" WatermarkCssClass="long Watermarked" />
                        <cc1:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" TargetControlID="txtAbDatum" 
                            OnClientDateSelectionChanged="checkAbDatum" />
                        <asp:CustomValidator ID="cvAbDatum" runat="server" EnableClientScript="false" ControlToValidate="txtAbDatum"
                            Display="None" SetFocusOnError="true" ErrorMessage="Bitte überprüfen Sie das Datum."
                            OnServerValidate="OnValidateDate" />
                    </td>
                </tr>
                <tr id="trUhrzeit" runat="server">
                    <td class="First" style="padding-left: 0px">
                        <asp:Label ID="lbl_AbUhrzeit" runat="server">lbl_AbUhrzeit</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAbUhrzeit" runat="server" CssClass="long" DataTextField="Range"
                            DataValueField="ID" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="fAbDebitorNr" runat="server" />
        </asp:Panel>
        <div style="clear: both;">
        </div>
        <custom:ColorAnimationExtender runat="server" ID="ColorAnimationExtender1">
            <TargetElements>
                <custom:ColorAnimationTarget ControlID="txtAbFirma" />
                <custom:ColorAnimationTarget ControlID="txtAbStrasse" />
                <custom:ColorAnimationTarget ControlID="txtAbPLZ" />
                <custom:ColorAnimationTarget ControlID="txtAbOrt" />
                <custom:ColorAnimationTarget ControlID="ddlAbLand" />
                <custom:ColorAnimationTarget ControlID="txtAbAnsprechpartner" />
                <custom:ColorAnimationTarget ControlID="txtAbTelefon" />
            </TargetElements>
        </custom:ColorAnimationExtender>
    </ContentTemplate>
</asp:UpdatePanel>
<custom:ModalOverlay runat="server" ID="ModalOverlay1" Type="Click">
    <ContentTemplate>
        <div class="popUpWindow" style="width: 700px;">
            <div class="header">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="color: #ffffff; height: 10px">
                            Adressen
                        </td>
                        <td style="width: 100%; height: 10px">
                            <a href="javascript: <%# Container.CloseScript %>;" style="float: right; color: #ffffff">
                                X</a>
                        </td>
                    </tr>
                </table>
            </div>
            <uc:AddressSearch ID="AddressSearch1" runat="server" Style="max-height: 450px; overflow: auto;
                overflow-x: hidden; padding-right: 20px; padding-left: 20px" OnAddressSelected="OnAddressSelected">
            </uc:AddressSearch>
        </div>
    </ContentTemplate>
</custom:ModalOverlay>
<custom:ModalOverlay runat="server" ID="ModalOverlay2" Type="Click">
    <ContentTemplate>
        <div class="popUpWindow" style="width: 600px;">
            <div class="header">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="color: #ffffff; height: 10px">
                            Entfernungskilometer
                        </td>
                        <td style="width: 100%; height: 10px">
                            <a href="javascript: <%# Container.CloseScript %>;" style="float: right; color: #ffffff">
                                X</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvKilometer" runat="server" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image runat="server" ImageUrl="/services/images/lkw.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="STREET" HeaderText="Straße">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                </asp:BoundField>
                                <asp:BoundField DataField="POSTL_CODE" HeaderText="PLZ">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CITY" HeaderText="Ort">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                </asp:BoundField>
                                <asp:BoundField DataField="KM" HeaderText="Entfernung (km)">
                                    <HeaderStyle HorizontalAlign="Right" CssClass="AbstandRechts" />
                                    <ItemStyle HorizontalAlign="Right" CssClass="AbstandRechts" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </ContentTemplate>
</custom:ModalOverlay>
