<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WSFahrten.ascx.vb"
    Inherits="CKG.Components.Logistik.WSFahrten" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
    TagPrefix="custom" %>
<script type="text/javascript">
    /*Init im PageLoad statt im document-ready, da Seite z.T. durch Ajax-Postback 
    neu aufgebaut wird, wobei kein document-ready ausgelöst wird*/
    function pageLoad() {
        Init();
    }

    function Init() {
        $(".zusatzfahrtCheckbox").change(function () {
            var cbx = $(this);
            var wert = "0";
            if (cbx.is(':checked')) {
                wert = "1";
            }
            $("#<%= ihShowZusatzfahrten.ClientID %>").val(wert);
            SetZusatzfahrtenVisibility();
        });

        SetZusatzfahrtenVisibility();
        SetCheckboxState();
    }
    function SetCheckboxState() {
        var hField = $("#<%= ihShowZusatzfahrten.ClientID %>");
        if (hField.val() == "1") {
            $(".zusatzfahrtCheckbox").prop('checked', true);
        } else {
            $(".zusatzfahrtCheckbox").prop('checked', false);
        }
    }
    function SetZusatzfahrtenVisibility() {
        var hField = $("#<%= ihShowZusatzfahrten.ClientID %>");
        if (hField.val() == "1") {
            $(".headerZusatzfahrt").show();
        } else {
            $(".headerZusatzfahrt").hide();
        }
    }
</script>
<table cellspacing="0" cellpadding="0">
    <tr>
        <td style="padding-bottom: 0px; width: 60%" class="PanelHead">
            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Festlegung der Fahrten</asp:Label>
        </td>
        <td rowspan="2" style="width: 380px; padding: 0;">
            <asp:UpdatePanel ID="upValidation" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CWSAbholadresse"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="CWSZusatzfahrtenKfz1"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="CWSZusatzfahrtenKfz1Extra"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="CWSZieladresseKfz1"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="CWSZusatzfahrtenKfz2"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary6" runat="server" ValidationGroup="CWSZusatzfahrtenKfz2Extra"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:ValidationSummary ID="ValidationSummary7" runat="server" ValidationGroup="CWSZieladresseKfz2"
                        ShowSummary="true" DisplayMode="BulletList" CssClass="ValidationSummary" />
                    <asp:Label ID="lblErrorStamm" runat="server" CssClass="ValidationSummary" ForeColor="Red"
                        Visible="false" EnableViewState="false">Bitte füllen Sie die rot umrahmten Felder aus.</asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td style="padding-top: 0px;">
            <asp:Label ID="Label11" runat="server" Text="Bitte geben Sie hier die Adressen für die Abholung und Anlieferung ein." /><br />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-left: 7px; padding-right: 5px;">
            <div style="min-height: 460px;">
                <custom:CollapsibleWizardControl ID="cwcSteps" runat="server" SelectedStepChangedClientCallback="OnStepChanged"
                    NavigateRequiredStepsOnly="true" OnWizardCompleted="OnWizardCompleted" CollapsibleContainerCssClass="containerFahrt">
                    <Steps>
                        <custom:CollapsibleWizardStep Title="Abholadresse" UserControl="~/Components/Logistik/Change02sUserControls/CWSAbholadresse.ascx"
                            IsRequired="true" />
                        <custom:CollapsibleWizardStep Title="Zusatzfahrten Fzg. 1" UserControl="~/Components/Logistik/Change02sUserControls/CWSZusatzfahrtenKfz1.ascx"
                            IsRequired="false" />
                        <custom:CollapsibleWizardStep Title="Zieladresse Fzg. 1" UserControl="~/Components/Logistik/Change02sUserControls/CWSZieladresseKfz1.ascx"
                            IsRequired="true" />
                        <custom:CollapsibleWizardStep Title="Zusatzfahrten Fzg. 2" UserControl="~/Components/Logistik/Change02sUserControls/CWSZusatzfahrtenKfz2.ascx"
                            IsRequired="false" Enabled="false" />
                        <custom:CollapsibleWizardStep Title="Zieladresse Fzg. 2" UserControl="~/Components/Logistik/Change02sUserControls/CWSZieladresseKfz2.ascx"
                            IsRequired="true" Enabled="false" />
                    </Steps>
                    <WizardStepHeaderTemplate>
                        <div class='<%# If(Container.IsRequired, If(Container.Title.StartsWith("Zusatzfahrt"), "StandardHeadDetail headerZusatzfahrt", "StandardHeadDetail"), If(Container.Title.StartsWith("Zusatzfahrt"), "StandardHeadDetail black headerZusatzfahrt", "StandardHeadDetail black")) %>'
                            onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                            <div>
                                <img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleDown.png") %>'
                                    alt="toggle" />&nbsp;<%# Container.Title %>
                            </div>
                        </div>
                    </WizardStepHeaderTemplate>
                    <WizardStepSelectedHeaderTemplate>
                        <div class='<%# If(Container.IsRequired,"StandardHeadDetail","StandardHeadDetail black") %> selected'
                            onclick="<%# Container.SelectionScript %>" style="height: 28px; width: 895px;">
                            <div>
                                <img id="toggleheader_<%# Container.Index %>" src='<%= Page.ResolveClientUrl("~/Images/Zulassung/toggleUp.png") %>'
                                    alt="toggle" />&nbsp;<%# Container.Title %>
                            </div>
                        </div>
                    </WizardStepSelectedHeaderTemplate>
                    <WizardPageHeaderTemplate>
                        <div>
                    </WizardPageHeaderTemplate>
                    <WizardPageFooterTemplate>
                        </div>
                    </WizardPageFooterTemplate>
                </custom:CollapsibleWizardControl>
            </div>
            <div class="rightAlignedNav separator" style="width: 895px;">
                <asp:LinkButton ID="lbNext" runat="server" Text="Weiter &gt;" OnClick="OnNextClick"
                    CssClass="blueButton" />
                <asp:LinkButton ID="lbPrevious" runat="server" Text="&lt; Zur&uuml;ck" OnClick="OnPreviousClick"
                    CssClass="blueButton" />
            </div>
            <input type="hidden" id="ihShowZusatzfahrten" runat="server" value="0"/>
        </td>
    </tr>
</table>
