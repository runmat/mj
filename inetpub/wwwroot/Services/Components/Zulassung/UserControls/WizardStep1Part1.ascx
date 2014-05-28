<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep1Part1.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep1Part1" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>

<div runat="server" id="Container">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:Panel DefaultButton="button1" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 695px; float:left;" class="InputTable">
        <tr valign="top">
            <td width="90">
                <asp:Label ID="lbl_Vertragsnummer" AssociatedControlID="txtVertragsnummer" runat="server">lbl_Vertragsnummer</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="middle"></asp:TextBox>
            </td>
            <td width="80">
                <asp:Label ID="lbl_NummerZB2Suche" AssociatedControlID="txt_NummerZB2Suche" runat="server">lbl_NummerZB2Suche</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_NummerZB2Suche" runat="server" MaxLength="8" CssClass="middle"></asp:TextBox>
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
            </td>
            <td>
                <asp:Label ID="lbl_Land" AssociatedControlID="drpLand" runat="server">lbl_Land</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="drpLand" runat="server" CssClass="middle" AutoPostBack="true" OnSelectedIndexChanged="OnCountryChanged">
                    <asp:ListItem>Auswahl</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="vertical-align: top; text-align: right;">
                <asp:LinkButton CssClass="greyButton search" runat="server" ID="button1" Text="Suchen" onclick="buttonSearch_Click" />
            </td>
        </tr>
    </table>
    <div id="infopanel" class="infopanel" style="width: 155px; float:right;">
	    <label style="width: 135px">Tipp!</label>
	    <div>Nutzen Sie bei der FIN<br/>Platzhalter um Ihre Suche<br/>auszuweiten. (z.B. 12345*)</div>
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
                <br /><label style="font-size:10px;font-weight:bold;">Ihr Suchanfrage wird bearbeitet.</label>
            </div>
        </ContentTemplate>
    </custom:ModalOverlay>
</div>
