<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep2Part2.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep2Part2" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div runat="server" id="Container">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:Panel ID="Panel1" DefaultButton="btnSearch" runat="server">
     <table cellpadding="0" cellspacing="0" style="width: 895px;" class="InputTable">
            <tr>
                <td width="120">
                    <asp:Label ID="lbl_Zulassungsart" AssociatedControlID="drpZulassungsart" runat="server">lbl_Zulassungsart</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="drpZulassungsart" runat="server" CssClass="long"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Zulassungsdatum" AssociatedControlID="txtZulassungsdatum" runat="server">lbl_Zulassungsdatum</asp:Label>
                    <asp:Label ID="lbl_ZulDatAddDays" runat="server" Visible="true" style="Display:none">lbl_ZulDatAddDays</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtZulassungsdatum" runat="server" CssClass="short"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender runat="server" ID="Calendar" TargetControlID="txtZulassungsdatum"></ajaxToolkit:CalendarExtender>
                    <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie ein Zulassungsdatum an."  ControlToValidate="txtZulassungsdatum" EnableClientScript="false" ID="valZulassungsdatum" ValidationGroup="ZulassungStep2Part2"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Zulassungskreis" AssociatedControlID="txtZulassungskreis" runat="server">lbl_Zulassungskreis</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtZulassungskreis" runat="server" CssClass="short"></asp:TextBox> <asp:LinkButton ID="btnSearch" runat="server" Text="Halteradresse" CssClass="greyButton" onclick="Button1_Click" />
                    <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie einen Zulassungskreis an."  ControlToValidate="txtZulassungskreis" EnableClientScript="false" ID="valZulassungskreis" ValidationGroup="ZulassungStep2Part2"></asp:RequiredFieldValidator>
                </td>
            </tr>
    </table>
    </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>


    <custom:ModalOverlay runat="server" id="SearchOverlay" ParentContainer="Container">
        <Triggers>
            <custom:ModalOverlayTrigger ControlID="btnSearch" />
        </Triggers>
        <ContentTemplate>
            <div style="background-color: #fff; width: 300px; padding: 15px; text-align: center; border: 3px solid #335393;">
                <img id="Img1" src="~/Images/Zulassung/loading.gif" align="middle" style="border-width:0px;" runat="server" />
                <br /><label style="font-size:14px;font-weight:bold;">Bitte warten...</label>
                <br /><label style="font-size:10px;font-weight:bold;">Ihr Zulassungskreis wird ermittelt.</label>
            </div>
        </ContentTemplate>
    </custom:ModalOverlay>