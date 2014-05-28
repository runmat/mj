<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="CKG.Components.ComCommon.Change04"
    MasterPageFile="../../../MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
    TagPrefix="custom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false" />
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Carportbeauftragung" />
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table id="Table1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" Visible="true" EnableViewState="false" />
                                    <asp:ValidationSummary ID="valsum" runat="server" ValidationGroup="validation" CssClass="ValidationSummary" />
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                        <asp:Panel ID="pSaveMessage" runat="server" BackColor="White" BorderWidth="2px" BorderStyle="Solid"
                            BorderColor="Black" DefaultButton="bOkay" Style="padding: 5px;">
                            <h3 style="margin: 15px;">
                                Beauftragung gespeichert</h3>
                            <p style="text-align: center;">
                                <asp:Button ID="bOkay" runat="server" Text="OK" />
                            </p>
                        </asp:Panel>
                        <AjaxToolkit:ModalPopupExtender ID="mpeSaveMessage" runat="server" TargetControlID="btnFake"
                            PopupControlID="pSaveMessage" />
                    </div>
                    <div id="TableQuery">
                        <asp:Panel ID="Container" runat="server" DefaultButton="bStatus">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblFahrgestellnummer" runat="server" Text="Fahrgestellnummer:" AssociatedControlID="txtFahrgestellnummer" />
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="long" CausesValidation="true"
                                                ValidationGroup="validation" />
                                            <%--<custom:EitherOrValidator ID="eovFahrgestellnummer" runat="server" ValidationGroup="validation"
                        ControlToValidate="txtFahrgestellnummer" OrControlToValidate="txtKennzeichen" Display="None"
                        ErrorMessage="Geben Sie bitte eine Fahrgestellnummer oder ein Kennzeichen an." />--%>
                                            <asp:RequiredFieldValidator ID="rfvFahrgestellnummer" runat="server" ValidationGroup="validation"
                                                Display="None" ControlToValidate="txtFahrgestellnummer" ErrorMessage="Geben Sie eine gültige Fahrgestellnummer ein."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revfahrgestellnummer" runat="server" ValidationGroup="validation"
                                                ControlToValidate="txtFahrgestellnummer" ValidationExpression="\w{17}" Display="None"
                                                ErrorMessage="Die Fahrgestellnummer muss genau 17 Zeichen lang sein." />
                                        </td>
                                        <td class="active" style="text-align: right; padding-right: 5px;">
                                            <asp:TextBox ID="txtStatus" runat="server" CssClass="middle" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 20%;">
                                            <asp:Label ID="lblKennzeichen" runat="server" Text="Kennzeichen:" AssociatedControlID="txtKennzeichen" />
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="long" />
                                        </td>
                                        <td class="active" style="text-align: right; padding-right: 5px;">
                                            <asp:Button ID="bStatus" runat="server" Text="Status ermittelen. (Enter)" OnClick="OnStatusClicked"
                                                Width="186px" />
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="row1" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblCarport" runat="server" Text="Carport:" AssociatedControlID="ddlCarport" />
                                        </td>
                                        <td class="active" colspan="2">
                                            <asp:DropDownList ID="ddlCarport" runat="server" CssClass="long" />
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="row2" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblLogistikPartner" runat="server" Text="Logistik Partner:" AssociatedControlID="ddlLogistikPartner" />
                                        </td>
                                        <td class="active" colspan="2">
                                            <asp:DropDownList ID="ddlLogistikPartner" runat="server" CssClass="long">
                                                <asp:ListItem Value="" Text="Keiner" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="row3" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblBereitstellungsdatum" runat="server" Text="Bereitstellungsdatum:"
                                                AssociatedControlID="txtBereitstellungsdatum" />
                                        </td>
                                        <td class="active" colspan="2">
                                            <asp:TextBox ID="txtBereitstellungsdatum" runat="server" CssClass="long" onchange="javascript:bereitstellungsdatumChanged(this.value);" />
                                            <script type="text/javascript">
                                                function bereitstellungsdatumChanged(p) {
                                                    var morgen = '<%= DateTime.Today.AddDays(1).ToShortDateString() %>';
                                                    if (p === morgen) {
                                                        alert('Kurzfristige Beauftragung zu Morgen?');
                                                    }
                                                }
                                            </script>
                                            <AjaxToolkit:CalendarExtender ID="ceBereitstellungsdatum" runat="server" TargetControlID="txtBereitstellungsdatum" />
                                            <asp:CompareValidator ID="cvBereitstellungsdatum" runat="server" ControlToValidate="txtBereitstellungsdatum"
                                                Type="Date" Operator="GreaterThan" ValidationGroup="validation" Display="None"
                                                ErrorMessage="Bereitstellungsdatum darf nicht in der Vergangenheit liegen." />
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="row4" runat="server">
                                        <td class="firstLeft active" style="vertical-align: top;">
                                            <asp:Label ID="lblBemerkung" runat="server" Text="Bemerkung:" AssociatedControlID="txtBemerkung" />
                                        </td>
                                        <td class="active" colspan="2">
                                            <asp:TextBox ID="txtBemerkung" runat="server" TextMode="MultiLine" Rows="6" CssClass="long" />
                                        </td>
                                    </tr>
                                    <%--                <tr class="formquery">
                  <td class="firstLeft active" colspan="4">
                    <custom:MultiselectBox ID="msbDienstleistungen" runat="server" FlowDirection="RightToLeft"
                      DataTextField="Text" DataValueField="Nummer">
                      <ItemsHeaderTemplate>
                        <div style="float: left; width: 208px; font-size: 9px; border: 1px solid #AFAFAF"
                          class="selection">
                          <h3 style="margin: 0pt; width: 200px; background-color: #B9C9FE; color: #003399;
                            border-top: 4px solid #AABCFE; vertical-align: middle; padding: 4px; font-size: 10px;
                            font-weight: bold;">
                            <img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/icon_checkbox_inactive.gif") %>'
                              style="vertical-align: middle; margin: 0pt 0pt 1px 2px;" alt="" />
                            Verfügbare Dienstleistungen</h3>
                      </ItemsHeaderTemplate>
                      <SelectedItemsHeaderTemplate>
                        <div style="float: left; width: 208px; font-size: 9px; border: 1px solid #AFAFAF">
                          <h3 style="margin: 0pt; width: 200px; background-color: #B9C9FE; color: #003399;
                            border-top: 4px solid #AABCFE; vertical-align: middle; padding: 4px; font-size: 10px;
                            font-weight: bold;">
                            <img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/icon_checkbox_active.gif") %>'
                              style="vertical-align: middle; margin: 0pt 0pt 1px 2px;" alt="" />
                            Ausgewählte Dienstleistungen</h3>
                      </SelectedItemsHeaderTemplate>
                      <SelectedItemsFooterTemplate>
                        </div>
                        <div style="float: left; height: 200px; width: 80px; text-align: center;">
                          <img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/switch_icon.gif") %>' style="margin-top: 100px;"
                            alt="" />
                        </div>
                      </SelectedItemsFooterTemplate>
                      <ItemsFooterTemplate>
                        </div>
                      </ItemsFooterTemplate>
                      <FooterTemplate>
                        <div style="clear: both;">
                        </div>
                      </FooterTemplate>
                    </custom:MultiselectBox>
                  </td>
                </tr>
                                    --%>
                                    <tr class="formquery" id="row5" runat="server">
                                        <td class="firstLeft active" style="vertical-align: top;">
                                            <asp:Label ID="lblDienstleistungen" runat="server" Text="Dienstleistungen:" AssociatedControlID="lbDienstleistungen" />
                                        </td>
                                        <td class="active">
                                            <asp:ListBox ID="lbDienstleistungen" runat="server" SelectionMode="Multiple" Rows="10"
                                                CssClass="long" />
                                        </td>
                                        <td style="width: 100%; vertical-align: top; padding-right: 5px">
                                            <div class="new_layout">
                                                <div id="infopanel" class="infopanel">
                                                    <label>
                                                        Tipp!</label>
                                                    <div>
                                                        Auswahl mehrerer Dienstleistungen mit gedrückter STRG / CTRL Taste.
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="row6" runat="server">
                                        <td class="firstLeft active" colspan="3" style="text-align: right;">
                                            <asp:Button ID="bSave" runat="server" Text="Speichern." OnClick="OnSaveClicked" Style="margin-bottom: 12px;" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;
                                clear: both;">
                                &nbsp;
                            </div>
                        </asp:Panel>
                    </div>
                    <div style="clear: both; height: 22px;">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
