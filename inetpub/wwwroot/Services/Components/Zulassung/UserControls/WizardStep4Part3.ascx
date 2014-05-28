<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep4Part3.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep4Part3" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>
<%@ Register Assembly="CKG.Components.Zulassung" Namespace="CKG.Components.Zulassung.UserControls" TagPrefix="uc" %>

<div runat="server" id="Container">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>

<asp:Panel ID="Panel1" runat="server">

      <table cellpadding="0" cellspacing="0" style="width: 895px;" class="InputTable">
        <tr>
            <td class="First" style="padding-left: 21px" width="150">
                <asp:Label ID="lbl_ArAuswahl" runat="server">lbl_ArAuswahl</asp:Label>
            </td>
            <td>
                <div id="divRE" runat="server" style="width: 306px">
                    <asp:DropDownList ID="ddlPartnerRE" runat="server" Width="306px" AutoPostBack="True" OnTextChanged="ddlPartnerRE_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
            </td>
                                                        
        </tr>
        <tr>
            <td class="First" style="padding-left: 21px">
                <asp:Label ID="lbl_ArFirma" runat="server">lbl_ArFirma</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtArFirma" runat="server" Width="300px" Enabled="False"></asp:TextBox>
            </td>
                                                        
        </tr>
        <tr>
            <td class="First" style="padding-left: 21px">
                <asp:Label ID="lbl_ArStrasse" runat="server">lbl_ArStrasse</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtArStrasse" runat="server" Width="300px" Enabled="False"></asp:TextBox>
            </td>
                                                        
        </tr>
        <tr>
            <td class="First" style="padding-left: 21px">
                <asp:Label ID="lbl_ArPlzOrt" runat="server">lbl_ArPlzOrt</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtArPLZ" runat="server" Width="60px" Enabled="False" MaxLength="5"></asp:TextBox><span>&nbsp;&nbsp;</span>
                <asp:TextBox ID="txtArOrt" Width="222px" runat="server" Enabled="False"></asp:TextBox>
            </td>
                                                        
        </tr>
        <tr>
            <td class="First" style="padding-left: 21px">
                <asp:Label ID="lbl_ArAnsprechpartner" runat="server">lbl_ArAnsprechpartner</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtArAnsprechpartner" runat="server" Width="300px" Enabled="False"></asp:TextBox>
            </td>
                                                        
        </tr>
        <tr>
            <td class="First" style="padding-left: 21px">
                <asp:Label ID="lbl_ArTelefon" runat="server">lbl_ArTelefon</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtArTelefon" runat="server" Width="300px" Enabled="False"></asp:TextBox>
            </td>
        </tr>
    </table>

</asp:Panel>  


      <custom:ColorAnimationExtender runat="server" ID="ColorAnimationExtender1">
        <TargetElements>

            <custom:ColorAnimationTarget ControlID="ddlPartnerRE" />
            <custom:ColorAnimationTarget ControlID="txtArFirma" />
            <custom:ColorAnimationTarget ControlID="txtArStrasse" />
            <custom:ColorAnimationTarget ControlID="txtArPLZ" />
            <custom:ColorAnimationTarget ControlID="txtArOrt" />
            <custom:ColorAnimationTarget ControlID="txtArAnsprechpartner" />
            <custom:ColorAnimationTarget ControlID="txtArTelefon" />

        </TargetElements>
    </custom:ColorAnimationExtender>

  </ContentTemplate>
 </asp:UpdatePanel>
</div>