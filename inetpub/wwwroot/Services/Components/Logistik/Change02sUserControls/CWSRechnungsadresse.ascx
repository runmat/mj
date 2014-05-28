<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSRechnungsadresse.ascx.vb" Inherits="CKG.Components.Logistik.CWSRechnungsadresse" %>
<%@ Register TagPrefix="uc1" TagName="PartnerAddress" Src="~/Components/Logistik/Change02sUserControls/PartnerAddress.ascx" %>
<asp:Panel ID="pnlAbwRechnungsadresse" runat="server" Width="100%">
  <uc1:PartnerAddress ID="paRechnungsadresse" runat="server" ValidationGroup="CWSRechnungsadresse" OnSelectedAddressChanged="OnSelectedAddressChanged" />
</asp:Panel>
