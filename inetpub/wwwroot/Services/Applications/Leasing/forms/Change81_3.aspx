<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change81_3.aspx.cs" Inherits="Leasing.forms.Change81_3"
    MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:HyperLink ID="step1" runat="server">Fahrzeugsuche</asp:HyperLink>
                <asp:HyperLink ID="step2" runat="server">| Fahrzeugauswahl</asp:HyperLink>
                <a class="active">| Adressen</a>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" />
                                    <asp:Label ID="lblMessage" runat="server" />
                                </td>
                            </tr>
                            <tr runat="server" id="tr_Dienstleistung" class="formquery">
                                <td class="firstLeft active" style="height: 22px; width: 15%">
                                    Dienstleistung
                                </td>
                                <td class="active" style="height: 22px;">
                                    <span>
                                        <asp:DropDownList ID="ddlDienstleistung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDienstleistung_SelectedIndexChanged" />
                                    </span>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlHalter" runat="server">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Neuer Halter:</span>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="width: 16%">
                                        Name1
                                    </td>
                                    <td class="active" style="width: 42%">
                                        <asp:TextBox ID="txtHalterName1" runat="server" TabIndex="1" Width="295px" MaxLength="35" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtHalterName1" Display="Dynamic" ErrorMessage="'Name1' ist ein Pflichtfeld"
                                            Text="*" />
                                    </td>
                                    <td class="firstLeft active">
                                        Name2
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtHalterName2" runat="server" TabIndex="2" Width="295px" MaxLength="35" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Strasse, Nr.
                                    </td>
                                    <td class="active" style="white-space: nowrap;">
                                        <asp:TextBox ID="txtHalterStrasse" runat="server" TabIndex="3" Width="238px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtHalterStrasse" Display="Dynamic" ErrorMessage="'Strasse' ist ein Pflichtfeld"
                                            Text="*" />&nbsp;
                                        <asp:TextBox ID="txtHalterHausnr" runat="server" TabIndex="4" Width="50px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtHalterHausnr" Display="Dynamic" ErrorMessage="'Nr.' ist ein Pflichtfeld"
                                            Text="*" />
                                    </td>
                                    <td class="firstLeft active" style="white-space: nowrap;">
                                        PLZ, Ort
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHalterPLZ" runat="server" TabIndex="5" Width="50px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtHalterPLZ" Display="Dynamic" ErrorMessage="'PLZ' ist ein Pflichtfeld"
                                            Text="*" /><asp:RegularExpressionValidator ID="revHalterPLZ" runat="server" ControlToValidate="txtHalterPLZ"
                                                Display="Dynamic" ErrorMessage="'PLZ' hat das falsche Format" Text="*" ValidationExpression="\d+" />&nbsp;
                                        <asp:TextBox ID="txtHalterOrt" runat="server" TabIndex="6" Width="238px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtHalterOrt" Display="Dynamic" ErrorMessage="'Ort' ist ein Pflichtfeld"
                                            Text="*" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td class="firstLeft active" style="white-space: nowrap;">
                                        Land
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlHalterLand" Style="width: auto" AutoPostBack="true"
                                            OnSelectedIndexChanged="HalterLandChanged" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlHalterLand" Display="Dynamic"
                                            ErrorMessage="'Land' ist ein Pflichtfeld" Text="*" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlNeuerStandort1" runat="server">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="3" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Neuer Standort</span>
                                    </td>
                                    <td align="right" style="background-color: #dfdfdf;">
                                        <asp:LinkButton ID="lnkStandortAnzeige" runat="server">Anzeigen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="width: 16%">
                                        Name1
                                    </td>
                                    <td class="active" style="width: 42%">
                                        <asp:TextBox ID="txtStandortName1" runat="server" TabIndex="7" Width="295px" MaxLength="35"></asp:TextBox>
                                    </td>
                                    <td class="firstLeft active">
                                        Name2
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtStandortName2" runat="server" TabIndex="8" Width="295px" MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Strasse, Nr.
                                    </td>
                                    <td class="active" style="white-space: nowrap;">
                                        <asp:TextBox ID="txtStandortStrasse" runat="server" TabIndex="9" Width="238px"></asp:TextBox>&nbsp;
                                        <asp:TextBox ID="txtStandortHausnr" runat="server" TabIndex="10" Width="50px"></asp:TextBox>
                                    </td>
                                    <td class="firstLeft active" style="white-space: nowrap;">
                                        PLZ, Ort
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStandortPLZ" runat="server" TabIndex="11" Width="50px"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="txtStandortOrt" runat="server" TabIndex="12" Width="238px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                    <td class="active">
                                        &nbsp;
                                    </td>
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                    <td class="active" style="width: 50%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlZulDaten" runat="server" Style="display: block">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Zulassungsdaten:</span>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trWunschkennzeichen" runat="Server">
                                    <td class="firstLeft active">
                                        Wunschkennzeichen
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <asp:TextBox ID="txtKreis" runat="server" TabIndex="13" Width="55px" />
                                        <asp:Label ID="lblKreis" runat="server" Visible="false" />&nbsp;-&nbsp;
                                        <asp:TextBox ID="txtWunschkennzeichen" runat="server" TabIndex="14" Width="78px" />
                                        <asp:ImageButton ID="btnZulkreis" runat="server" Height="16px" ImageUrl="/Services/images/Lupe_16x16.gif"
                                                    ToolTip="Zulassungskreis ermitteln" Width="16px" OnClick="SucheZulassungskreis" Visible="False" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="trReserviertAuf" runat="Server">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        reserviert auf
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtReserviertAuf" runat="server" TabIndex="15" Width="295px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trVersicherungstr" runat="Server">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Versicherungsträger
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtVersicherungstraeger" runat="server" TabIndex="16" Width="295px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trEvbNr" runat="Server">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        eVB-Nummer
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtEVBNummer" runat="server" Width="78px" MaxLength="7" TabIndex="17" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEVBNummer" Display="Dynamic"
                                            ErrorMessage="'eVB-Nummer' ist ein Pflichtfeld" Text="*" />
                                        &nbsp;gültig von&nbsp;
                                        <asp:TextBox ID="txtEVBVon" runat="server" Width="78px" TabIndex="18" />
                                        <ajaxToolkit:CalendarExtender ID="txtEVBVonCE" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtEVBVon" />
                                        &nbsp;bis&nbsp;
                                        <asp:TextBox ID="txtEVBBis" runat="server" Width="78px" TabIndex="19" />
                                        <ajaxToolkit:CalendarExtender ID="txtEVBBisCE" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtEVBBis" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlEmpfaenger" runat="server">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                        <span style="font-weight: bold">Empfänger Schein/Schilder:</span>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="width: 16%">
                                        Name1
                                    </td>
                                    <td class="active" style="width: 42%">
                                        <asp:TextBox ID="txtEmpfaengerName1" runat="server" TabIndex="20" Width="295px" MaxLength="35" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtEmpfaengerName1" Display="Dynamic" ErrorMessage="'Name1' ist ein Pflichtfeld"
                                            Text="*" />
                                    </td>
                                    <td class="firstLeft active">
                                        Name2
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtEmpfaengerName2" runat="server" TabIndex="21" Width="295px" MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Strasse, Nr.
                                    </td>
                                    <td class="active" style="white-space: nowrap;">
                                        <asp:TextBox ID="txtEmpfaengerStrasse" runat="server" TabIndex="22" Width="238px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtEmpfaengerStrasse" Display="Dynamic" ErrorMessage="'Strasse' ist ein Pflichtfeld"
                                            Text="*" />&nbsp;
                                        <asp:TextBox ID="txtEmpfaengerHausnr" runat="server" TabIndex="23" Width="50px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtEmpfaengerHausnr" Display="Dynamic" ErrorMessage="'Nr.' ist ein Pflichtfeld"
                                            Text="*" />
                                    </td>
                                    <td class="firstLeft active" style="white-space: nowrap;">
                                        PLZ, Ort
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmpfaengerPLZ" runat="server" TabIndex="24" Width="50px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtEmpfaengerPLZ" Display="Dynamic" ErrorMessage="'PLZ' ist ein Pflichtfeld"
                                            Text="*" /><asp:RegularExpressionValidator ID="revEmpfaengerPLZ" runat="server" ControlToValidate="txtEmpfaengerPLZ"
                                                Display="Dynamic" ErrorMessage="'PLZ' hat das falsche Format" Text="*" ValidationExpression="\d+" />&nbsp;
                                        <asp:TextBox ID="txtEmpfaengerOrt" runat="server" TabIndex="25" Width="238px" /><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txtEmpfaengerOrt" Display="Dynamic" ErrorMessage="'Ort' ist ein Pflichtfeld"
                                            Text="*" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td class="firstLeft active" style="white-space: nowrap;">
                                        Land
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEmpfaengerLand" Style="width: auto" AutoPostBack="true"
                                            OnSelectedIndexChanged="EmpfaengerLandChanged" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlEmpfaengerLand"
                                            Display="Dynamic" ErrorMessage="'Land' ist ein Pflichtfeld" Text="*" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlSonstiges" runat="server">
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px;
                                        width: 100%">
                                        <span style="font-weight: bold">Sonstiges:</span>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="height: 22px; width: 15%">
                                        gew. Durch-<br />
                                        führungsdatum
                                    </td>
                                    <td class="active" style="width: 100%; height: 22px;">
                                        <asp:TextBox ID="txtDurchfuehrungsDatum" runat="server" TabIndex="26"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalExDatum" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDurchfuehrungsDatum">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trHinweis" runat="server">
                                    <td class="firstLeft active">
                                        Hinweis
                                    </td>
                                    <td class="active">
                                        <asp:Label ID="lblHinweis" runat="server" Width="374px" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Bemerkung
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtBemerkung" runat="server" Width="374px" Height="71px" TextMode="MultiLine"
                                            TabIndex="28"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                    <td class="active">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div style="float: left;">
                            <asp:ValidationSummary runat="server" DisplayMode="BulletList" ShowSummary="true" />
                        </div>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                TabIndex="30" OnClick="cmdSave_Click">» Weiter</asp:LinkButton>
                        </div>
                    </div>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
