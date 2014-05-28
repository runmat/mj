<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep1.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep1" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>
<%@ Register TagPrefix="uc" TagName="SearchResult" Src="~/Components/Zulassung/UserControls/VehicleSearchResult.ascx" %>

<table cellspacing="0" cellpadding="0">
    <tr>
        <td style="padding-bottom: 0px;" class="PanelHead">
            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Zuzulassende Fahrzeuge auswählen</asp:Label>
        </td>
        <td rowspan="2" width="380" style="padding: 0;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ZulassungStep1Part2" ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:Label ID="lblErrorStamm" runat="server" CssClass="ValidationSummary" ForeColor="Red" Visible="false">Bitte füllen Sie die rot umrahmten Felder aus.</asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td style="padding-top: 0px;">
            <asp:Label ID="Label11" runat="server" Text="Bitte geben Sie die Suchkriterien für ein Fahrzeug ein oder machen Sie einen Upload."></asp:Label><br />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-left: 7px; padding-right: 5px;">
            <div style="min-height: 328px;">
                <custom:CollapsibleWizardControl runat="server" ID="Wizard" SelectedStepChangedClientCallback="OnStepChanged" NavigateRequiredStepsOnly="false">
                    <Steps>
                        <custom:CollapsibleWizardStep Title="Fahrzeugsuche" UserControl="~/Components/Zulassung/UserControls/WizardStep1Part1.ascx" IsRequired="true" />
                        <custom:CollapsibleWizardStep Title="Fahrzeuganlage" UserControl="~/Components/Zulassung/UserControls/WizardStep1Part2.ascx" IsRequired="false" />
                        <custom:CollapsibleWizardStep Title="Dateiupload" UserControl="~/Components/Zulassung/UserControls/WizardStep1Part3.ascx" IsRequired="false" />
                    </Steps>
                    <WizardStepHeaderTemplate>
                        <div class='<%# Container.IsRequired?"StandardHeadDetail":"StandardHeadDetail black" %>' onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                            <div><img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleDown.png") %>' />&nbsp;<%# Container.Title %></div>
                        </div>
                    </WizardStepHeaderTemplate>
                    <WizardStepSelectedHeaderTemplate>
                        <div class='<%# Container.IsRequired?"StandardHeadDetail":"StandardHeadDetail black" %> selected' onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                            <div><img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleUp.png") %>' />&nbsp;<%# Container.Title %></div>
                        </div>
                    </WizardStepSelectedHeaderTemplate>
                    <WizardPageHeaderTemplate>
                        <div>
                    </WizardPageHeaderTemplate>
                    <WizardPageFooterTemplate>
                        </div>
                    </WizardPageFooterTemplate>
                </custom:CollapsibleWizardControl>
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                       <div style="width: 894px;">
                       <uc:SearchResult ID="SearchResult1" runat="server" Visible="false"></uc:SearchResult>
                       <asp:Label runat="server" ID="SelectionCount" CssClass="infolabel" Visible="false"></asp:Label>
                       </div>
                        <custom:ModalOverlay runat="server" id="moDocuments" Type="Click">
                            <ContentTemplate>
                                <div style="background-color: #fff; width: 450px; padding: 15px; text-align: center; border: 3px solid #335393;">
                                    <asp:LinkButton ID="lbClose" runat="server" CausesValidation="false" Text="Schließen X" style="float: right;" OnClick="OnClose" />
                                    <custom:DocumentList ID="dlZulassung" runat="server" ondocumentlistcommand="OnDocumentListCommand" onlayoutcreated="OnLayoutCreated">
                                    <LayoutTemplate>
                      <table cellpadding="0" cellspacing="0" style="width: 100%; text-align: left; margin-top: 20px;">
                      <tr>
                      <th style="padding: 5px 0px 5px 15px; text-align: left;">
                      Dokumententyp
                      </th>
                      <th style="padding: 5px 0px 5px 15px; text-align: left;">
                      <asp:ImageButton ID="ibSelectAll" runat="server" CommandName="SelectAll" CommandArgument="Select" ImageUrl="~/Images/Zulassung/icon_checkbox_active.gif" AlternateText="Alle auswählen" />
                      <asp:ImageButton ID="ibDeselectAll" runat="server" CommandName="SelectAll" CommandArgument="Deselect" ImageUrl="~/Images/Zulassung/icon_checkbox_inactive.gif" AlternateText="Auswahl aufheben" />&nbsp;Dokument
                      </th>
                      <th style="padding: 5px 0px 5px 15px; text-align: left;">
                      Gültig bis
                      </th>
                      </tr>
                      <tr id="groupPlaceholder" runat="server"></tr>
                      <tr>
                      <td colspan="3">
                      <asp:LinkButton ID="lbSave" runat="server" CssClass="greyButton save" style="float: right;" Text="Herunterladen" CommandName="Save" />
                      </td>
                      </tr>
                      </table>
                      </LayoutTemplate>
                      <GroupTemplate>
                      <tr>
                      <td rowspan="<%# Container.GroupCount %>" valign="top">
                        <asp:Label ID="lblGroupName" runat="server" Text="<%# Container.GroupName %>" />
                      </td>
                      <td id="itemPlaceholder" runat="server"></td>
                      </tr>
                      </GroupTemplate>
                      <ItemTemplate>
                      <td>
                      <asp:CheckBox ID="cbTest" runat="server" />
                      <asp:HiddenField ID="hfTest" runat="server" Value='<%# Eval("FileName") %>' />
                      <a target="_blank" href='<%# VirtualPathUtility.ToAbsolute("~/Components/Zulassung/Dokumente/Download/" + (string)Eval("FileName")) %>'>
                      <asp:Label ID="lblTest" runat="server" Text='<%# Eval("Identifier") %>' /></a>
                      </td>
                      <td><asp:Label ID="lblTest2" runat="server" Text='<%# Eval("ValidUntil", "{0:d}") %>' /></td>
                      </ItemTemplate>
                      <ItemSeparatorTemplate>
                      </tr><tr>
                      </ItemSeparatorTemplate>
                                    </custom:DocumentList>
                                </div>
                            </ContentTemplate>
                        </custom:ModalOverlay>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="rightAlignedNav separator" style="width: 895px;">
                <asp:LinkButton runat="server" ID="buttonNext" Text="Weiter &gt;" onclick="buttonNext_Click" CssClass="blueButton" />
            </div>
        </td>
    </tr>
</table>