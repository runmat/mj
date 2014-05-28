<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WSStammdaten.ascx.vb"
    Inherits="CKG.Components.Logistik.WSStammdaten" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
    TagPrefix="custom" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<table cellspacing="0" cellpadding="0">
    <tr>
        <td style="padding-bottom: 0px; width: 60%" class="PanelHead">
            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Fahrzeugstammdaten eingeben</asp:Label>
        </td>
        <td rowspan="2" style="width: 100%; padding: 0; float: right" valign="middle">
            <asp:UpdatePanel ID="upValidation" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <contenttemplate>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CWSKfz1"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="CWSKfz2"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="CWSRechnungszahler"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="CWSRechnungsadresse"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="CWSHalter"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:Label ID="lblErrorStamm" runat="server" CssClass="ValidationSummary" ForeColor="Red"
                        Visible="false" EnableViewState="false">Bitte füllen Sie die rot umrahmten Felder aus.</asp:Label>
                </contenttemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td style="padding-top: 0px;">
            <asp:Label ID="Label11" runat="server" Text="Bitte geben Sie hier alle notwendigen Fahrzeugdaten ein." /><br />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-left: 7px; padding-right: 5px;">
            <div style="min-height: 460px;">
                <custom:CollapsibleWizardControl ID="cwcSteps" runat="server" SelectedStepChangedClientCallback="OnStepChanged"
                    NavigateRequiredStepsOnly="true" OnWizardCompleted="OnWizardCompleted">
                    <Steps>
                        <custom:CollapsibleWizardStep Title="Fahrzeug 1" UserControl="~/Components/Logistik/Change02sUserControls/CWSKfz1.ascx"
                            IsRequired="true" />
                        <custom:CollapsibleWizardStep Title="Fahrzeug 2" UserControl="~/Components/Logistik/Change02sUserControls/CWSKfz2.ascx"
                            IsRequired="false" />
                        <custom:CollapsibleWizardStep Title="Rechnungszahler" UserControl="~/Components/Logistik/Change02sUserControls/CWSRechnungszahler.ascx"
                            IsRequired="false" />
                        <custom:CollapsibleWizardStep Title="Abweichende Rechnungsadresse" UserControl="~/Components/Logistik/Change02sUserControls/CWSRechnungsadresse.ascx"
                            IsRequired="false" />
                        <custom:CollapsibleWizardStep Title="Halter / Leistungsempfänger" UserControl="~/Components/Logistik/Change02sUserControls/CWSHalter.ascx"
                            IsRequired="false" />
                    </Steps>
                    <WizardStepHeaderTemplate>
                        <div class='<%# If(Container.IsRequired, "StandardHeadDetail", "StandardHeadDetail black") %>'
                            onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                            <div>
                                <img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleDown.png") %>'
                                    alt="toggle" />&nbsp;<%# Container.Title %>
                            </div>
                        </div>
                    </WizardStepHeaderTemplate>
                    <WizardStepSelectedHeaderTemplate>
                        <div class='<%# If(Container.IsRequired,"StandardHeadDetail","StandardHeadDetail black") %> selected'
                            onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                            <div>
                                <img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleUp.png") %>'
                                    alt="toggle" />&nbsp;<%# Container.Title %>
                            </div>
                        </div>
                    </WizardStepSelectedHeaderTemplate>
                    <WizardPageHeaderTemplate>
                        <div>
                    </WizardPageHeaderTemplate>
                    <WizardPageFooterTemplate>
                        </div>
                    </WizardPageFooterTemplate>
                </custom:CollapsibleWizardControl>
            </div>
            <div class="rightAlignedNav separator" style="width: 895px;">
                <asp:LinkButton runat="server" ID="lbNext" Text="Weiter &gt;" OnClick="OnNextClick"
                    CssClass="blueButton" />
            </div>
        </td>
    </tr>
</table>
