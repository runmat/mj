<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CWSKfz2.ascx.vb" Inherits="CKG.Components.Logistik.CWSKfz2" %>
<%@ Register TagPrefix="uc1" TagName="KfzStammdaten" Src="~/Components/Logistik/Change02sUserControls/KfzStammdaten.ascx" %>
<asp:Panel ID="pnlRueckholung" runat="server" Width="100%">
  <uc1:KfzStammdaten ID="ksRückholung" runat="server" validationgroup="CWSKfz2" OnFillData="OnFillData"/>
</asp:Panel>
