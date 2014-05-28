<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSRechnungszahler.ascx.vb" Inherits="CKG.Components.Logistik.CWSRechnungszahler" %>
<%@ Register TagPrefix="uc1" TagName="PartnerAddress" Src="~/Components/Logistik/Change02sUserControls/PartnerAddress.ascx" %>
<asp:Panel ID="pnlRechnungszahler" runat="server" Width="100%">
  <uc1:PartnerAddress ID="paRechnungszahler" runat="server" OnSelectedAddressChanged="OnSelectedAddressChanged" ValidationGroup="CWSRechnungszahler" />
</asp:Panel>
