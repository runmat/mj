<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSHalteradresse.ascx.vb"
    Inherits="CKG.Components.Logistik.CWSHalteradresse" %>
<%@ Register TagPrefix="uc" TagName="TransportAddress" Src="~/Components/Logistik/Change02sUserControls/TransportAddress.ascx" %>
<asp:Panel ID="pnlHalteradresse" runat="server" Width="100%">
    <uc:TransportAddress ID="TransportAddress" runat="server" ValidationGroup="CWSHalteradresse"
        SearchType="Halteradresse" ShowSearch="true" OnValidatePostcode="OnValidatePostcode" OnValidateDate="OnValidateDate"
        ShowTransportTypes="false"  />
</asp:Panel>
