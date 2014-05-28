<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep4Part1.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep4Part1" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>
<%@ Register Assembly="CKG.Components.Zulassung" Namespace="CKG.Components.Zulassung.UserControls" TagPrefix="uc" %>


<div runat="server" id="Container">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:Panel ID="Panel1" DefaultButton="btnSearch" runat="server">
        <table cellpadding="0" cellspacing="0" style="width: 895px;" class="InputTable">
            <tr>
                <td>
                    <asp:Label AssociatedControlID="rblAdresse" ID="lbl_Versand" runat="server" Text="Versand an:" />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblAdresse" runat="server" AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal" OnSelectedIndexChanged="VersandAnChanged">
                        <asp:ListItem Text="Halter" Value="Halter" Selected="True" />
                        <asp:ListItem Text="Auftraggeber" Value="Auftraggeber" />
                        <asp:ListItem Text="andere Adresse" Value="Andere" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Name" AssociatedControlID="txtName" runat="server">lbl_Name</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Enabled="false" CssClass="long"></asp:TextBox> <asp:LinkButton ID="btnSearch" runat="server" Enabled="false" Text="Suchen" CssClass="greyButton search" onclick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Name2" AssociatedControlID="txtName2" runat="server">lbl_Name2</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtName2" runat="server" Enabled="false" CssClass="long"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Strasse" AssociatedControlID="txtStrasse" runat="server">lbl_Strasse</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtStrasse" runat="server" Enabled="false" CssClass="long"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Plz" AssociatedControlID="txtPlz" runat="server">lbl_Plz</asp:Label>, <asp:Label ID="lbl_Ort" runat="server">lbl_Ort</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPlz" runat="server" Enabled="false" CssClass="short"></asp:TextBox> 
                    <asp:TextBox ID="txtOrt" runat="server" Enabled="false" CssClass="middle"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Land" AssociatedControlID="drpLand" runat="server">lbl_Land</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="drpLand" runat="server" CssClass="long" Enabled="false" AutoPostBack="true">
                        <asp:ListItem>Auswahl</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
    </table>
    </asp:Panel>

    <custom:ColorAnimationExtender runat="server" ID="ColorAnimationExtender1">
        <TargetElements>
            <custom:ColorAnimationTarget ControlID="txtName" />
            <custom:ColorAnimationTarget ControlID="txtName2" />
            <custom:ColorAnimationTarget ControlID="txtStrasse" />
            <custom:ColorAnimationTarget ControlID="txtPlz" />
            <custom:ColorAnimationTarget ControlID="txtOrt" />
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