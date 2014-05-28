<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSDLZusatzfahrtenKfz1.ascx.vb" Inherits="CKG.Components.Logistik.CWSDLZusatzfahrtenKfz1" %>
<%@ Register TagPrefix="uc" TagName="ZusatzDienstleistungen" Src="~/Components/Logistik/Change02sUserControls/ZusatzDienstleistungen.ascx" %>
<uc:ZusatzDienstleistungen ID="ZusatzDienstleistungen" runat="server" OnNeedsServices="OnNeedsServices" ValidationGroup="CWSZusatzfahrtenKfz1" />
