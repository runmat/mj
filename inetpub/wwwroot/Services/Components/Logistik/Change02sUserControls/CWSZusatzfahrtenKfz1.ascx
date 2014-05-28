<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSZusatzfahrtenKfz1.ascx.vb" Inherits="CKG.Components.Logistik.CWSZusatzfahrtenKfz1" %>
<%@ Register TagPrefix="uc" TagName="Zusatzfahrten" Src="~/Components/Logistik/Change02sUserControls/Zusatzfahrten.ascx" %>
<uc:Zusatzfahrten ID="Zusatzfahrten" runat="server" ValidationGroup="CWSZusatzfahrtenKfz1" />
<asp:CustomValidator ID="cvZusatzfahrten" runat="server" OnServerValidate="OnServerValidate"
  EnableClientScript="false" Display="None" SetFocusOnError="true" ControlToValidate="Zusatzfahrten"
  ErrorMessage="Änderungen bitte speichern oder verwerfen." ValidationGroup="CWSZusatzfahrtenKfz1Extra" />