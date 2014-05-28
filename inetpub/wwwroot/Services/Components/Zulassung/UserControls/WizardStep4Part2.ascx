<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep4Part2.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep4Part2" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>
<%@ Register Assembly="CKG.Components.Zulassung" Namespace="CKG.Components.Zulassung.UserControls" TagPrefix="uc" %>

<div runat="server" id="Container">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>

<asp:Panel ID="Panel1" runat="server">
           <table cellpadding="0" cellspacing="0" style="width: 895px;" class="InputTable">
            <tr>
                 <td class="First" style="padding-left: 21px" width="150">
                    <asp:Label ID="lbl_RzAuswahl" runat="server">lbl_RzAuswahl</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPartnerRG" runat="server" Width="306px" AutoPostBack="True" OnTextChanged="ddlPartnerRG_SelectedIndexChanged">
                        </asp:DropDownList>
       
                </td>
                                                        
            </tr>
            <tr>
                <td class="First" style="padding-left: 21px">
                    <asp:Label ID="lbl_RzFirma" runat="server">lbl_RzFirma</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRzFirma" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                                                        
            </tr>
            <tr>
                <td class="First" style="padding-left: 21px">
                    <asp:Label ID="lbl_RzStrasse" runat="server">lbl_RzStrasse</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRzStrasse" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                                                        
            </tr>
            <tr>
                <td class="First" style="padding-left: 21px">
                    <asp:Label ID="lbl_RzPlzOrt" runat="server">lbl_RzPlzOrt</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRzPLZ" runat="server" Width="60px" Enabled="False" MaxLength="5"></asp:TextBox><span>&nbsp;&nbsp;</span>
                    <asp:TextBox ID="txtRzOrt" Width="222px" runat="server" Enabled="False"></asp:TextBox>
                </td>
                                                        
            </tr>
            <tr>
                <td class="First" style="padding-left: 21px">
                    <asp:Label ID="lbl_RzAnsprechpartner" runat="server">lbl_RzAnsprechpartner</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRzAnsprechpartner" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                                                        
            </tr>
            <tr>
                <td class="First" style="padding-left: 21px">
                    <asp:Label ID="lbl_RzTelefon" runat="server">lbl_RzTelefon</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRzTelefon" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                </td>
            </tr>
        </table>
</asp:Panel>  


      <custom:ColorAnimationExtender runat="server" ID="ColorAnimationExtender1">
        <TargetElements>

            <custom:ColorAnimationTarget ControlID="ddlPartnerRG" />
            <custom:ColorAnimationTarget ControlID="txtRzFirma" />
            <custom:ColorAnimationTarget ControlID="txtRzStrasse" />
            <custom:ColorAnimationTarget ControlID="txtRzPLZ" />
            <custom:ColorAnimationTarget ControlID="txtRzOrt" />
            <custom:ColorAnimationTarget ControlID="txtRzAnsprechpartner" />
            <custom:ColorAnimationTarget ControlID="txtRzTelefon" />

        </TargetElements>
    </custom:ColorAnimationExtender>

  </ContentTemplate>
 </asp:UpdatePanel>
</div>