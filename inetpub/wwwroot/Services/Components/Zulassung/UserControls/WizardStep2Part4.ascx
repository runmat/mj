<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep2Part4.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep2Part4" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>
<%@ Register Assembly="CKG.Components.Zulassung" Namespace="CKG.Components.Zulassung.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div runat="server" id="Container">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
     <table cellpadding="0" cellspacing="0" style="width: 895px;" class="InputTable">
            <tr>
                <td width="120">
                    <asp:Label ID="lbl_Versicherungsgesellschaft" AssociatedControlID="txtVersicherungsgesellschaft" runat="server">lbl_Versicherungsgesellschaft</asp:Label>
                </td>
                <td>
                    <asp:Panel ID="Panel1" DefaultButton="btnVersichererSearch" runat="server">
                        <asp:TextBox ID="txtVersicherungsgesellschaft" runat="server" CssClass="long"></asp:TextBox> <asp:LinkButton ID="btnVersichererSearch" runat="server" Text="Suchen" CssClass="greyButton search" onclick="VersichererButton_Click" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_EvbNo" AssociatedControlID="txtEvbNo" runat="server">lbl_EvbNo</asp:Label>
                </td>
                <td>
                    <asp:Panel ID="Panel2" DefaultButton="btnEvbSearch" runat="server">
                        <asp:TextBox ID="txtEvbNo" runat="server" CssClass="short"></asp:TextBox> <asp:LinkButton ID="btnEvbSearch" runat="server" Text="Suchen" CssClass="greyButton search" onclick="EvbButton_Click" />
                        <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine eVB Nummer an."  ControlToValidate="txtEvbNo" EnableClientScript="false" ID="valEvbNo" ValidationGroup="ZulassungStep2Part4"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie genau 7 alphanumerische Zeichen an." ControlToValidate="txtEvbNo" EnableClientScript="false" ID="valEvbNo2" ValidationExpression="^\w{7}$" ValidationGroup="ZulassungStep2Part4" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_GueltigVon" AssociatedControlID="txtGueltigVon" runat="server">lbl_GueltigVon</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGueltigVon" runat="server" CssClass="short"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender runat="server" ID="Calendar1" TargetControlID="txtGueltigVon"></ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_GueltigBis" AssociatedControlID="txtGueltigBis" runat="server">lbl_GueltigBis</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGueltigBis" runat="server" CssClass="short"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender runat="server" ID="Calendar2" TargetControlID="txtGueltigBis"></ajaxToolkit:CalendarExtender>
               </td>
            </tr>
    </table>

    <custom:ColorAnimationExtender runat="server" ID="ColorAnimationExtender1">
        <TargetElements>
            <custom:ColorAnimationTarget ControlID="txtVersicherungsgesellschaft" />
        </TargetElements>
    </custom:ColorAnimationExtender>

    <custom:ColorAnimationExtender runat="server" ID="ColorAnimationExtender2">
        <TargetElements>
            <custom:ColorAnimationTarget ControlID="txtEvbNo" />
        </TargetElements>
    </custom:ColorAnimationExtender>

    </ContentTemplate>
    </asp:UpdatePanel>
</div>

   <custom:ModalOverlay runat="server" id="ModalOverlay1" Type="Click">
        <ContentTemplate>
            <div class="popUpWindow" style="width: 600px;">
                <div class="header">
                    Adressen (Schließen: ESC-Taste)
                    <a href="javascript: <%# Container.CloseScript %>;">X</a>
                </div>
                <uc:AddressSearch ID="AddressSearch1" runat="server" style="max-height: 450px; overflow: auto; overflow-x: hidden;"></uc:AddressSearch>
            </div>
        </ContentTemplate>
    </custom:ModalOverlay>

       <custom:ModalOverlay runat="server" id="ModalOverlay2" Type="Click">
        <ContentTemplate>
            <div class="popUpWindow" style="width: 600px;">
                <div class="header">
                    EVB Nummern
                    <a href="javascript: <%# Container.CloseScript %>;">X</a>
                </div>
                <uc:AddressSearch ID="AddressSearch2" runat="server" style="max-height: 450px; overflow: auto; overflow-x: hidden;"></uc:AddressSearch>
            </div>
        </ContentTemplate>
    </custom:ModalOverlay>