<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep2.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep2" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>


<table cellspacing="0" cellpadding="0">
    <tr>
        <td style="padding-bottom: 0px;" class="PanelHead">
            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Zulassungsdaten eingeben</asp:Label>
        </td>
        <td rowspan="2" width="380" style="padding: 0;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ZulassungStep2Part1" ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="ZulassungStep2Part2" ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="ZulassungStep2Part4" ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:Label ID="Label1" runat="server" CssClass="ValidationSummary" ForeColor="Red" Visible="false">Bitte füllen Sie die rot umrahmten Felder aus.</asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td style="padding-top: 0px;" colspan="2">
            <asp:Label ID="Label11" runat="server" Text="Bitte geben Sie die für die Zulassung notwendigen Daten ein."></asp:Label><br />
            <asp:Label ID="lblErrorStamm" runat="server" ForeColor="Red" Visible="false">Bitte füllen Sie die rot umrahmten Felder aus.</asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-left: 7px; padding-right: 5px;">
            <div style="min-height: 328px;">
            <custom:CollapsibleWizardControl runat="server" ID="Wizard" SelectedStepChangedClientCallback="OnStepChanged" NavigateRequiredStepsOnly="false">
                <Steps>
                    <custom:CollapsibleWizardStep Title="Halter" UserControl="~/Components/Zulassung/UserControls/WizardStep2Part1.ascx" IsRequired="true" />
                    <custom:CollapsibleWizardStep Title="Zulassung" UserControl="~/Components/Zulassung/UserControls/WizardStep2Part2.ascx" IsRequired="true" />
                    <custom:CollapsibleWizardStep Title="Wunschkennzeichen" UserControl="~/Components/Zulassung/UserControls/WizardStep2Part3.ascx" IsRequired="true" />
                    <custom:CollapsibleWizardStep Title="Versicherungsdaten" UserControl="~/Components/Zulassung/UserControls/WizardStep2Part4.ascx" IsRequired="true" />
                    <custom:CollapsibleWizardStep Title="Abweichender Versicherungsnehmer" UserControl="~/Components/Zulassung/UserControls/WizardStep2Part5.ascx" IsRequired="false" />
                </Steps>
                <WizardStepHeaderTemplate>
                    <div class='<%# Container.IsRequired?"StandardHeadDetail":"StandardHeadDetail black" %>' onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                        <div><img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleDown.png") %>' />&nbsp;<%# Container.Title %></div>
                    </div>
                </WizardStepHeaderTemplate>
                <WizardStepSelectedHeaderTemplate>
                    <div class='<%# Container.IsRequired?"StandardHeadDetail":"StandardHeadDetail black" %> selected' onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                        <div><img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleUp.png") %>' />&nbsp;<%# Container.Title %></div>
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
                <asp:LinkButton ID="LinkButton2" runat="server" Text="Weiter &gt;" onclick="LinkButton2_Click" CssClass="blueButton" />
                <asp:LinkButton ID="LinkButton1" runat="server" Text="&lt; Zur&uuml;ck" onclick="LinkButton1_Click" CssClass="blueButton" />
                <asp:LinkButton ID="ButtonSetDefaultData" runat="server" Text="Kundenstammdaten verwenden" onclick="ButtonSetDefaultData_Click" CssClass="greyButton" />
            </div>
        </td>
    </tr>
</table>