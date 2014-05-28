<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSDLZieladresseKfz2.ascx.vb"
    Inherits="CKG.Components.Logistik.CWSDLZieladresseKfz2" %>
<%@ Register TagPrefix="uc" TagName="Services" Src="~/Components/Logistik/Change02sUserControls/Services.ascx" %>
<%@ Register TagPrefix="uc" TagName="ProtocolUpload" Src="~/Components/Logistik/Change02sUserControls/ProtocolUpload.ascx" %>
<uc:Services ID="Services" runat="server" />
<asp:CustomValidator ID="cvExpress" runat="server" ControlToValidate="Services" ValidationGroup="CWSDLZieladresseKfz2"
    OnServerValidate="OnValidateExpress" Display="None" ValidateEmptyText="true" />
<uc:ProtocolUpload ID="ProtocolUpload" runat="server" />