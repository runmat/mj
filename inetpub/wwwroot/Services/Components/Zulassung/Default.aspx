<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="CKG.Components.Zulassung._Default" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="site" class="new_layout">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                    </div>

                    <div>
                        <asp:PlaceHolder runat="server" ID="ErrorDisplay" Visible="false">
                        <div class="VersandTabPanel">
                            <table cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="padding-bottom: 0px; width: 100%" class="PanelHead">
                                        <asp:Label ID="Label2" runat="server">Fehler</asp:Label>
                                    </td> 
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-top: 0px;">
                                        <asp:Label ID="Label3" runat="server" Text="Es ist ein Fehler aufgetreten."></asp:Label><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div style="min-height: 332px;">
                                            <asp:Label runat="server" ID="labelError"></asp:Label>
                                        </div>
                                        <div class="rightAlignedNav separator">
                                            <asp:LinkButton runat="server" ID="buttonRestart2" Text="Neuer Auftrag" onclick="buttonRestart_Click" CssClass="blueButton" />
                                            <asp:LinkButton runat="server" ID="buttonChangeData" Text="Auftragsdaten ändern" onclick="buttonChange_Click" CssClass="blueButton" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="SuccessDisplay" Visible="false">
                        <div class="VersandTabPanel">
                            <table cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="padding-bottom: 0px; width: 100%" class="PanelHead">
                                        <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Auftragsbestätigung</asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-top: 0px;">
                                        <asp:Label ID="Label11" runat="server" Text="Ihr Auftrag wurde erfolgreich angelegt."></asp:Label><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div style="min-height: 332px;">
                                            <asp:Label runat="server" ID="labelOrderNo"></asp:Label>
                                        </div>
                                        <div class="rightAlignedNav separator">
                                            <asp:LinkButton runat="server" ID="buttonRestart" Text="Neuer Auftrag" onclick="buttonRestart_Click" CssClass="blueButton" />
                                            <asp:LinkButton runat="server" ID="buttonDownload" Text="Auftragsbestätigung drucken" onclick="buttonPrint_Click" CssClass="blueButton" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </asp:PlaceHolder>
                        <custom:WizardControl ID="TabControl1" runat="server" DisableFutureSteps="true">
                            <Steps>
                                <custom:WizardStep VisitedHeaderCssClass="ZulassungButtonFahrzeugReady" HeaderCssClass="ZulassungButtonFahrzeugDisabled" SelectedHeaderCssClass="ZulassungButtonFahrzeugEnabled" UserControl="~/Components/Zulassung/UserControls/WizardStep1.ascx"></custom:WizardStep>
                                <custom:WizardStep VisitedHeaderCssClass="ZulassungButtonStammdatenReady" HeaderCssClass="ZulassungButtonStammdatenDisabled" SelectedHeaderCssClass="ZulassungButtonStammdatenEnabled" UserControl="~/Components/Zulassung/UserControls/WizardStep2.ascx"></custom:WizardStep>
                                <custom:WizardStep VisitedHeaderCssClass="ZulassungButtonDienstleistungenReady" HeaderCssClass="ZulassungButtonDienstleistungenDisabled" SelectedHeaderCssClass="ZulassungButtonDienstleistungenEnabled" UserControl="~/Components/Zulassung/UserControls/WizardStep3.ascx"></custom:WizardStep>
                                <custom:WizardStep VisitedHeaderCssClass="ZulassungButtonVersandReady" HeaderCssClass="ZulassungButtonVersandDisabled" SelectedHeaderCssClass="ZulassungButtonVersandEnabled" UserControl="~/Components/Zulassung/UserControls/WizardStep4.ascx"></custom:WizardStep>
                                <custom:WizardStep VisitedHeaderCssClass="ZulassungButtonUebersichtReady" HeaderCssClass="ZulassungButtonUebersichtDisabled" SelectedHeaderCssClass="ZulassungButtonUebersichtEnabled" UserControl="~/Components/Zulassung/UserControls/WizardStep5.ascx"></custom:WizardStep>
                            </Steps>
                            <WizardNavigationHeaderTemplate>
                                <div class="DivVersandTabContainer">
                            </WizardNavigationHeaderTemplate>
                            <WizardNavigationFooterTemplate>
                                    <custom:ProgressIndicator runat="server" CurrentIndex="<%# Container.SelectedIndex %>" StepCount="<%# Container.StepCount %>">
                                        <HeaderTemplate>
                                            <div class="DivPanelSteps">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td align="right" nowrap="nowrap">
                                                            <asp:Label ID="Label1" runat="server">Schritt <%# Container.CurrentStep + 1 %> von <%# Container.StepCount %></asp:Label>
                                                        </td>
                                        </HeaderTemplate>
                                        <CompleteStepTemplate>
                                                        <td class="PanelHeadSteps">
                                                            <div class="StepActive">
                                                            </div>
                                                        </td>
                                        </CompleteStepTemplate>
                                        <IncompleteStepTemplate>
                                                        <td class="PanelHeadSteps">
                                                            <div class="Steps">
                                                            </div>
                                                        </td>
                                        </IncompleteStepTemplate>
                                        <FooterTemplate>
                                                    </tr>
                                                </table>
                                            </div>
                                        </FooterTemplate>
                                    </custom:ProgressIndicator>
                                    </div>        
                            </WizardNavigationFooterTemplate>
                            <WizardPageHeaderTemplate>
                                <div class="VersandTabPanel">
                            </WizardPageHeaderTemplate>
                            <WizardPageFooterTemplate>
                                </div>
                            </WizardPageFooterTemplate>
                        </custom:WizardControl>
                    </div>                            
                </div>
            </div>
        </div>
    </div>
</asp:Content>