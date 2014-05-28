<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WSDienstleistungen.ascx.vb" Inherits="CKG.Components.Logistik.WSDienstleistungen" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
  TagPrefix="custom" %>
<table cellspacing="0" cellpadding="0">
  <tr>
    <td style="padding-bottom: 0px; width:50%;" class="PanelHead">
      <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Auswahl zusätzlicher Dienstleistungen</asp:Label>
    </td>
    <td rowspan="2" style="padding: 0; max-width:460px;">
      <asp:UpdatePanel ID="upValidation" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
          <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CWSDLZusatzfahrtenKfz1"
            ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
          <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="CWSDLZieladresseKfz1"
            ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
          <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="CWSDLZusatzfahrtenKfz2"
            ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
          <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="CWSDLZieladresseKfz2"
            ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
        </ContentTemplate>
      </asp:UpdatePanel>
    </td>
  </tr>
  <tr>
    <td style="padding-top: 0px;">
      <asp:Label ID="Label11" runat="server" Text="Hier können Sie zusätzlich durchzuführende Aufgaben je Einzelfahrt beauftragen." /><br />
    </td>
  </tr>
  <tr>
    <td colspan="2" style="padding-left: 7px; padding-right: 5px;">
      <div style="min-height: 460px;">
        <custom:CollapsibleWizardControl ID="cwcSteps" runat="server" SelectedStepChangedClientCallback="OnStepChanged"
          NavigateRequiredStepsOnly="true" OnWizardCompleted="OnWizardCompleted">
          <Steps>
            <custom:CollapsibleWizardStep Title="Zusatzfahrten Fzg. 1" UserControl="~/Components/Logistik/Change02sUserControls/CWSDLZusatzfahrtenKfz1.ascx"
              IsRequired="false" Enabled="false" />
            <custom:CollapsibleWizardStep Title="Zieladresse Fzg. 1" UserControl="~/Components/Logistik/Change02sUserControls/CWSDLZieladresseKfz1.ascx"
              IsRequired="true" />
            <custom:CollapsibleWizardStep Title="Zusatzfahrten Fzg. 2" UserControl="~/Components/Logistik/Change02sUserControls/CWSDLZusatzfahrtenKfz2.ascx"
              IsRequired="false" Enabled="false" />
            <custom:CollapsibleWizardStep Title="Zieladresse Fzg. 2" UserControl="~/Components/Logistik/Change02sUserControls/CWSDLZieladresseKfz2.ascx"
              IsRequired="true" Enabled="false" />
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
        <asp:LinkButton ID="lbNext" runat="server" Text="Weiter &gt;" OnClick="OnNextClick"
          CssClass="blueButton" />
        <asp:LinkButton ID="lbPrevious" runat="server" Text="&lt; Zur&uuml;ck" OnClick="OnPreviousClick"
          CssClass="blueButton" />
      </div>
    </td>
  </tr>
</table>
