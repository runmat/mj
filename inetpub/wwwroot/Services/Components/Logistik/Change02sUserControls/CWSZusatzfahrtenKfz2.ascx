<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSZusatzfahrtenKfz2.ascx.vb" Inherits="CKG.Components.Logistik.CWSZusatzfahrtenKfz2" %>
<%@ Register TagPrefix="uc" TagName="Zusatzfahrten" Src="~/Components/Logistik/Change02sUserControls/Zusatzfahrten.ascx" %>
<uc:Zusatzfahrten ID="Zusatzfahrten" runat="server" ValidationGroup="CWSZusatzfahrtenKfz2" />
<asp:CustomValidator ID="cvZusatzfahrten" runat="server" OnServerValidate="OnServerValidate"
  EnableClientScript="false" Display="None" SetFocusOnError="true" ControlToValidate="Zusatzfahrten"
  ErrorMessage="Änderungen bitte speichern oder verwerfen." ValidationGroup="CWSZusatzfahrtenKfz2Extra" />