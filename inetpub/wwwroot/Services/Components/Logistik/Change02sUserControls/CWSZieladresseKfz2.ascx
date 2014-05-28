<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSZieladresseKfz2.ascx.vb"
    Inherits="CKG.Components.Logistik.CWSZieladresseKfz2" %>
<%@ Register TagPrefix="uc" TagName="TransportAddress" Src="~/Components/Logistik/Change02sUserControls/TransportAddress.ascx" %>
<asp:Panel ID="pnlAbholadresse" runat="server" Width="100%">
    <uc:TransportAddress ID="TransportAddress" runat="server" ValidationGroup="CWSZieladresseKfz2"
        SearchType="Auslieferung" ShowSearch="true" OnValidatePostcode="OnValidatePostcode"
        ShowTransportTypes="true" ShowKmButton="true" />
</asp:Panel>
