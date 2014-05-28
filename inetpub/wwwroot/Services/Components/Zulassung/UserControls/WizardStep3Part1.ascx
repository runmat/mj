<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep3Part1.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep3Part1" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>

<custom:MultiselectBox runat="server" ID="MultiselectBox1" FlowDirection="RightToLeft">
    <ItemsHeaderTemplate>
        <div style="float: left; width: 208px; font-size: 9px; border: 1px solid #AFAFAF" class="selection">
        <h3 style="margin: 0pt; width: 200px; background-color: #B9C9FE; color: #003399; border-top: 4px solid #AABCFE; vertical-align: middle; padding: 4px; font-size: 10px; font-weight:bold;"><img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/icon_checkbox_inactive.gif") %>' style="vertical-align: middle; margin: 0pt 0pt 1px 2px;" /> Verfügbare Dienstleistungen</h3>
    </ItemsHeaderTemplate>
    <SelectedItemsHeaderTemplate>
        <div style="float: left; width: 208px; font-size: 9px; border: 1px solid #AFAFAF">
        <h3 style="margin: 0pt; width: 200px; background-color: #B9C9FE; color: #003399; border-top: 4px solid #AABCFE; vertical-align: middle; padding: 4px; font-size: 10px; font-weight:bold;"><img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/icon_checkbox_active.gif") %>' style="vertical-align: middle; margin: 0pt 0pt 1px 2px;" /> Ausgewählte Dienstleistungen</h3>
    </SelectedItemsHeaderTemplate>
    <SelectedItemsFooterTemplate>
        </div>
        <div style="float: left; height: 200px; width: 80px; text-align: center;">
            <img src='<%= Page.ResolveClientUrl("~/Images/Zulassung/switch_icon.gif") %>' style="margin-top: 100px;" />
        </div>
    </SelectedItemsFooterTemplate>
    <ItemsFooterTemplate>
        </div>
    </ItemsFooterTemplate>
    <FooterTemplate>
        <div id="infopanel" class="infopanel">
	        <label style="width:275px">Tipp!</label>
	        <div >Nutzen Sie Drag & Drop um Dienstleistungen der<br/>Auswahl hinzuzufügen oder zu entfernen.</div>
            <label id="price"></label>
        </div>
        <div style="clear: both;"></div>
    </FooterTemplate>
</custom:MultiselectBox>