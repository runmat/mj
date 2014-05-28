<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSAbholadresse.ascx.vb"
    Inherits="CKG.Components.Logistik.CWSAbholadresse" %>
<%@ Register TagPrefix="uc" TagName="TransportAddress" Src="~/Components/Logistik/Change02sUserControls/TransportAddress.ascx" %>
<asp:Panel ID="pnlAbholadresse" runat="server" Width="100%">
    <uc:TransportAddress ID="TransportAddress" runat="server" ValidationGroup="CWSAbholadresse"
        SearchType="Abholadresse" ShowSearch="true" OnValidatePostcode="OnValidatePostcode"
        ShowTransportTypes="false" ShowZusatzfahrtenCheckbox="true" />
</asp:Panel>
