<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PartnerAddress.ascx.vb" Inherits="CKG.Components.Logistik.PartnerAddress" %>
<table width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td class="First" style="padding-left: 21px" width="150">
      <asp:Label ID="lbl_RzAuswahl" runat="server">lbl_RzAuswahl</asp:Label>
    </td>
    <td>
      <div id="divRG" runat="server" style="width: 306px">
        <asp:DropDownList ID="ddlPartnerRG" runat="server" Width="306px" OnSelectedIndexChanged="OnSelectedIndexChanged"
          AutoPostBack="True" AppendDataBoundItems="true" DataValueField="KUNNR" DataTextField="DDLFELD">
          <asp:ListItem Value="" Text="Bitte auswählen" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvPartnerRG" runat="server" ControlToValidate="ddlPartnerRG"
          Display="None" SetFocusOnError="true" ErrorMessage="Bitte wählen Sie eine Adresse aus."
          EnableClientScript="false" />
      </div>
    </td>
  </tr>
  <tr>
    <td class="First" style="padding-left: 21px">
      <asp:Label ID="lbl_RzFirma" runat="server">lbl_RzFirma</asp:Label>
    </td>
    <td>
      <asp:TextBox ID="txtRzFirma" runat="server" Width="300px" Enabled="False" />
    </td>
  </tr>
  <tr>
    <td class="First" style="padding-left: 21px">
      <asp:Label ID="lbl_RzStrasse" runat="server">lbl_RzStrasse</asp:Label>
    </td>
    <td>
      <asp:TextBox ID="txtRzStrasse" runat="server" Width="300px" Enabled="False" />
    </td>
  </tr>
  <tr>
    <td class="First" style="padding-left: 21px">
      <asp:Label ID="lbl_RzPlzOrt" runat="server">lbl_RzPlzOrt</asp:Label>
    </td>
    <td>
      <asp:TextBox ID="txtRzPLZ" runat="server" Width="60px" Enabled="False" MaxLength="5" /><span>&nbsp;&nbsp;</span>
      <asp:TextBox ID="txtRzOrt" Width="222px" runat="server" Enabled="False" />
    </td>
  </tr>
  <tr>
    <td class="First" style="padding-left: 21px">
      <asp:Label ID="lbl_RzAnsprechpartner" runat="server">lbl_RzAnsprechpartner</asp:Label>
    </td>
    <td>
      <asp:TextBox ID="txtRzAnsprechpartner" runat="server" Width="300px" Enabled="False" />
    </td>
  </tr>
  <tr>
    <td class="First" style="padding-left: 21px">
      <asp:Label ID="lbl_RzTelefon" runat="server">lbl_RzTelefon</asp:Label>
    </td>
    <td>
      <asp:TextBox ID="txtRzTelefon" runat="server" Width="300px" Enabled="False" />
    </td>
  </tr>
</table>
