<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSHalter.ascx.vb"
    Inherits="CKG.Components.Logistik.CWSHalter" %>
<%@ Register TagPrefix="uc1" TagName="TransportAddress" Src="~/Components/Logistik/Change02sUserControls/TransportAddress.ascx" %>
<asp:Panel runat="server" ID="pnlHalter" Width="100%">
    <uc1:TransportAddress runat="server" ID="HalterAddress" HalterAdressSelector="true"
        SearchType="Halter" ShowKmButton="false" ShowSearch="true" ShowTransportTypes="false"
        ValidationGroup="CWSHalter" />
</asp:Panel>
