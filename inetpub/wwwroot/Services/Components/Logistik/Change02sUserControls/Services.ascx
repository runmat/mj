<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Services.ascx.vb" Inherits="CKG.Components.Logistik.Services" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tlr" %>
<div style="float: left; width: 208px; font-size: 9px; border: 1px solid #AFAFAF"
    class="selection">
    <h3 style="margin: 0pt; width: 200px; background-color: #B9C9FE; color: #003399;
        border-top: 4px solid #AABCFE; vertical-align: middle; padding: 4px; font-size: 10px;
        font-weight: bold;">
        <img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/icon_checkbox_active.gif") %>'
            style="vertical-align: middle; margin: 0pt 0pt 1px 2px;" alt="" />
        Ausgewählte Dienstleistungen</h3>
    <tlr:RadListBox AllowTransfer="True" AllowTransferOnDoubleClick="True" AutoPostBackOnReorder="true" AutoPostBackOnTransfer="true"
        EnableDragAndDrop="true" Height="200px" ID="selectedServices" OnDropped="ServicesDropped" OnTransferred="ServicesTransferred"
        runat="server" SelectionMode="Multiple" TransferToID="availableServices" Width="208px" >
        <ButtonSettings ShowTransfer="False" ShowTransferAll="False" />
    </tlr:RadListBox>
    <asp:HiddenField runat="server" ID="selectedValues" />
</div>
<div style="float: left; height: 200px; width: 80px; text-align: center;">
    <img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/switch_icon.gif") %>' style="margin-top: 100px;"
        alt="" />
</div>
<div style="float: left; width: 208px; font-size: 9px; border: 1px solid #AFAFAF">
    <h3 style="margin: 0pt; width: 200px; background-color: #B9C9FE; color: #003399;
        border-top: 4px solid #AABCFE; vertical-align: middle; padding: 4px; font-size: 10px;
        font-weight: bold;">
        <img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/icon_checkbox_inactive.gif") %>'
            style="vertical-align: middle; margin: 0pt 0pt 1px 2px;" alt="" />
        Verfügbare Dienstleistungen</h3>
    <tlr:RadListBox AllowTransfer="True" AllowTransferOnDoubleClick="True" AutoPostBackOnReorder="true" AutoPostBackOnTransfer="true"
        EnableDragAndDrop="true" Height="200px" ID="availableServices" OnDropped="ServicesDropped" OnTransferred="ServicesTransferred"
        runat="server" SelectionMode="Multiple" TransferToID="selectedServices" Width="208px">
        <ButtonSettings ShowTransfer="False" ShowTransferAll="False" />
    </tlr:RadListBox>
</div>
<div id="infopanel" class="infopanel">
    <label>
        Tipp!</label>
    <div>
        Nutzen Sie Drag & Drop um Dienstleistungen der Auswahl hinzuzufügen oder zu entfernen.</div>
    <label id="price">
    </label>
</div>
<div style="clear: both;">
</div>
<table style="width: 496px; margin-top: 20px;" cellspacing="0" cellpadding="0">
    <tr>
        <td style="vertical-align: top; padding: 0 0 4px 0;">
            Bemerkung:
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top; padding: 0;">
            <asp:TextBox ID="txtAbBemerkung" runat="server" Height="70px" TextMode="MultiLine"
                Width="100%" />
        </td>
    </tr>
</table>
