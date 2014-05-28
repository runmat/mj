<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep1Part2.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep1Part2" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>

<div runat="server" id="Container">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:Panel ID="Panel1" DefaultButton="button1" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 695px; float: left;" class="InputTable">
        <tr valign="top">
            <td width="90">
                <asp:Label ID="lbl_Vertragsnummer" AssociatedControlID="txtVertragsnummer" runat="server">lbl_Vertragsnummer</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="middle"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine Vertragsnummer an."  ControlToValidate="txtVertragsnummer" EnableClientScript="false" ID="valVertragsnummer" ValidationGroup="ZulassungStep1Part2"></asp:RequiredFieldValidator>
            </td>
            <td width="90">
                <asp:Label ID="lbl_NummerZB2Anlage" AssociatedControlID="txt_NummerZB2Anlage" runat="server">lbl_NummerZB2Anlage</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_NummerZB2Anlage" runat="server" MaxLength="8" CssClass="middle"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine ZBII Nummer an." ControlToValidate="txt_NummerZB2Anlage" EnableClientScript="false" ID="valNummerZB2" ValidationGroup="ZulassungStep1Part2"></asp:RequiredFieldValidator>
            </td>
            <td>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Label ID="lbl_Fahrgestellnummer" AssociatedControlID="txtFahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="17" CssClass="middle"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie eine Fahrgestellnummer an."  ControlToValidate="txtFahrgestellnummer" EnableClientScript="false" ID="valFahrgestellnummer" ValidationGroup="ZulassungStep1Part2"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lbl_Land" AssociatedControlID="drpLand" runat="server">lbl_Land</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="drpLand" runat="server" CssClass="middle">
                    <asp:ListItem>Auswahl</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="vertical-align: top; text-align: right;">
                <asp:LinkButton CssClass="greyButton" runat="server" ID="button1" Text="Anlegen" onclick="buttonAdd_Click" />
            </td>
        </tr>
    </table>
    <div id="infopanel" class="infopanel" style="width: 175px; float:right;">
	    <label style="width: 155px">Tipp!</label>
	    <div>Nutzen Sie die Suche um<br/>Datensätze aus dem<br/>Bestand auszuwählen.</div>
        <label id="price"></label>
    </div>
    <div style="clear:both;"></div>
    </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>

    <custom:ModalOverlay runat="server" id="SearchOverlay" ParentContainer="Container">
        <Triggers>
            <custom:ModalOverlayTrigger ControlID="button1" />
        </Triggers>
        <ContentTemplate>
            <div style="background-color: #fff; width: 300px; padding: 15px; text-align: center; border: 3px solid #335393;">
                <img id="Img1" src="~/Images/Zulassung/loading.gif" align="middle" style="border-width:0px;" runat="server" />
                <br /><label style="font-size:14px;font-weight:bold;">Bitte warten...</label>
                <br /><label style="font-size:10px;font-weight:bold;">Ihr Fahrzeug wird angelegt.</label>
            </div>
        </ContentTemplate>
    </custom:ModalOverlay>
</div>
