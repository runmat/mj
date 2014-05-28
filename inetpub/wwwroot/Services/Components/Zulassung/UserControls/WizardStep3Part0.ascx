<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep3Part0.ascx.cs"
  Inherits="CKG.Components.Zulassung.UserControls.WizardStep3Part0" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
  TagPrefix="custom" %>
<table cellpadding="0" cellspacing="0" style="width: 895px;" class="InputTable">
  <tr>
    <td style="width: 120px;">
      <asp:Label ID="lbl_Buchungscode" AssociatedControlID="txtBuchungscode" runat="server">lbl_Buchungscode</asp:Label>
    </td>
    <td>
      <asp:TextBox ID="txtBuchungscode" runat="server" CssClass="middle" MaxLength="12" />
      <asp:RequiredFieldValidator ID="rvBuchungscode" runat="server" ControlToValidate="txtBuchungscode"
        Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie einen Buchungscode an."
        EnableClientScript="false" ValidationGroup="ZulassungStep3Part0" />
    </td>
  </tr>
</table>
