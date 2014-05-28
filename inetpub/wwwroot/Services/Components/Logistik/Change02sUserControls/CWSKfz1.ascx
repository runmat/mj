<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSKfz1.ascx.vb" Inherits="CKG.Components.Logistik.CWSKfz1" %>
<%@ Register TagPrefix="uc1" TagName="KfzStammdaten" Src="~/Components/Logistik/Change02sUserControls/KfzStammdaten.ascx" %>
<asp:Panel ID="pnlAllgDaten" runat="server" Width="100%">
  <uc1:KfzStammdaten ID="ksHinfahrt" runat="server" ValidationGroup="CWSKfz1" OnFillData="OnFillData" />
</asp:Panel>
