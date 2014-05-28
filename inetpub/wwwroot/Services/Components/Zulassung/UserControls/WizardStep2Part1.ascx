<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep2Part1.ascx.cs"
    Inherits="CKG.Components.Zulassung.UserControls.WizardStep2Part1" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
    TagPrefix="custom" %>
<%@ Register Assembly="CKG.Components.Zulassung" Namespace="CKG.Components.Zulassung.UserControls"
    TagPrefix="uc" %>
<div runat="server" id="Container">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" DefaultButton="btnSearch" runat="server">
                <table style="width: 895px;">
                    <tr>
                        <td style="width: 500px; padding-left: 0px">
                            <table cellpadding="0" cellspacing="0" style="width: 500px;" class="InputTable">
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="lbl_Name" AssociatedControlID="txtName" runat="server">lbl_Name</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="long"></asp:TextBox>
                                        <asp:LinkButton ID="btnSearch" runat="server" Text="Suchen" OnClick="Button1_Click"
                                            CssClass="greyButton search" />
                                        <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true"
                                            ErrorMessage="Bitte geben Sie einen Halternamen an." ControlToValidate="txtName"
                                            EnableClientScript="false" ID="valName" ValidationGroup="ZulassungStep2Part1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Name2" AssociatedControlID="txtName2" runat="server">lbl_Name2</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName2" runat="server" CssClass="long"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Strasse" AssociatedControlID="txtStrasse" runat="server">lbl_Strasse</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStrasse" runat="server" CssClass="long"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true"
                                            ErrorMessage="Bitte geben Sie eine Straße an." ControlToValidate="txtStrasse"
                                            EnableClientScript="false" ID="valStreet" ValidationGroup="ZulassungStep2Part1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Plz" AssociatedControlID="txtPlz" runat="server">lbl_Plz</asp:Label>,
                                        <asp:Label ID="lbl_Ort" runat="server">lbl_Ort</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlz" runat="server" CssClass="short"></asp:TextBox>
                                        <asp:TextBox ID="txtOrt" runat="server" CssClass="middle"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true"
                                            ErrorMessage="Bitte geben Sie eine Postleitzahl an." ControlToValidate="txtPlz"
                                            EnableClientScript="false" ID="valPlz" ValidationGroup="ZulassungStep2Part1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true"
                                            ErrorMessage="Bitte geben Sie einen Ort an." ControlToValidate="txtOrt" EnableClientScript="false"
                                            ID="ValOrt" ValidationGroup="ZulassungStep2Part1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Land" AssociatedControlID="drpLand" runat="server">lbl_Land</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpLand" runat="server" CssClass="long" AutoPostBack="true">
                                            <asp:ListItem>Auswahl</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField runat="server" ID="hKundenDebitorNummer" />
                        </td>
                        <td style="vertical-align: top; padding-right: 25px">
                            <div id="infopanel" class="infopanel" style="width: 250px; float: right">
                                <label id="lblTipHead" runat="server" style="width: 230px" />
                                <div style="width: 250px">
                                    <label id="lblTipMsg" runat="server" style="width: 230px" />
                                    <br />
                                </div>
                                <label id="price">
                                </label>
                            </div>
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
<custom:ModalOverlay runat="server" ID="SearchOverlay" ParentContainer="Container">
    <Triggers>
        <custom:ModalOverlayTrigger ControlID="btnSearch" />
    </Triggers>
    <ContentTemplate>
        <div style="background-color: #fff; width: 300px; padding: 15px; text-align: center;
            border: 3px solid #335393;">
            <img id="Img1" src="~/Images/Zulassung/loading.gif" align="middle" style="border-width: 0px;"
                runat="server" />
            <br />
            <label style="font-size: 14px; font-weight: bold;">
                Bitte warten...</label>
            <br />
            <label style="font-size: 10px; font-weight: bold;">
                Ihr Suchanfrage wird bearbeitet.</label>
        </div>
    </ContentTemplate>
</custom:ModalOverlay>
<custom:ModalOverlay runat="server" ID="ModalOverlay1" Type="Click">
    <ContentTemplate>
        <div class="popUpWindow">
            <div class="header">
                Adressen (Schließen: ESC-Taste) <a href="javascript: <%# Container.CloseScript %>;">
                    X</a>
            </div>
            <uc:AddressSearch ID="AddressSearch1" runat="server" Style="max-height: 450px; overflow: auto;
                overflow-x: hidden;">
            </uc:AddressSearch>
        </div>
    </ContentTemplate>
</custom:ModalOverlay>
