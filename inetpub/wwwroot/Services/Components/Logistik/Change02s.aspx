<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02s.aspx.vb" Inherits="CKG.Components.Logistik.Change02s"
    MaintainScrollPositionOnPostback="true" MasterPageFile="/services/MasterPage/Services.Master"
    EnableEventValidation="false" %>

<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
    TagPrefix="custom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .gridLines
        {
            border-bottom: 3px solid #CCDCFF;
            height: 28px;
            color: White;
            padding-left: 0px;
        }
        .style1
        {
            color: #595959;
        }
        .Watermark
        {
            color: Gray;
        }
        .AbstandLinks
        {
            padding-left: 8px;
        }
        .AbstandRechts
        {
            padding-right: 8px;
        }
        .Pointer
        {
            cursor: pointer;
        }
    </style>
    <div id="site" class="new_layout">
        <div id="content">
            <div id="divMessage" runat="server" visible="false" style="top: 300px; margin-left: 235px;
                z-index: 3; width: 400px; position: absolute">
                <div style="background-image: url(/services/images/MsgTitle.png); width: 400px">
                    <table width="100%">
                        <tr>
                            <td style="width: 85%; padding-left: 10px">
                                <asp:Label ID="lblMsgHeader" runat="server" ForeColor="White" Font-Bold="true" Text="Achtung"></asp:Label>
                            </td>
                            <td align="right" style="padding-right: 5px">
                                <asp:ImageButton ID="ibtMsgCancel" runat="server" ImageAlign="Top" ImageUrl="/services/images/MsgClose.png"
                                    Width="47px" Height="15" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="border-left: solid 2px #576B96; border-right: solid 2px #576B96; width: 396px;
                    background-color: #DCDCDC">
                    <table width="100%">
                        <tr>
                            <td valign="top" style="padding-left: 10px; padding-top: 10px; width: 15%">
                                <img src="/services/images/MsgAtt.png" alt="Attention" />
                            </td>
                            <td style="padding-right: 10px; padding-top: 10px">
                                <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="border-left: solid 2px #576B96; border-right: solid 2px #576B96; text-align: right;
                    background-color: #DCDCDC; padding-bottom: 10px; padding-top: 10px">
                    <asp:LinkButton ID="cmdOKWarnung" runat="server" Width="78px" Height="16px" CssClass="TablebuttonMiddle"
                        Visible="false" Style="margin-right: 10px">» OK</asp:LinkButton>
                    <asp:LinkButton ID="cmdJaWarnung" runat="server" Width="78px" Height="16px" CssClass="TablebuttonMiddle">» Ja</asp:LinkButton>
                    <asp:LinkButton ID="cmdNeinWarnung" runat="server" Width="78px" Height="16px" Style="margin-left: 5px;
                        margin-right: 10px" CssClass="TablebuttonMiddle">» Nein</asp:LinkButton>
                </div>
                <div style="background-image: url(/services/images/MsgBottom.png); background-repeat: no-repeat;
                    height: 6px">
                </div>
            </div>
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" Font-Size="12" />
                    </div>
                    <custom:WizardControl ID="wizardControl" runat="server" DisableFutureSteps="true">
                        <Steps>
                            <custom:WizardStep VisitedHeaderCssClass="VersandButtonStammReady" HeaderCssClass="VersandButtonStammVersandButtonStammEnabled"
                                SelectedHeaderCssClass="VersandButtonStamm" UserControl="~/Components/Logistik/Change02sUserControls/WSStammdaten.ascx" />
                            <custom:WizardStep VisitedHeaderCssClass="LogistikButtonTourReady" HeaderCssClass="LogistikButtonTourDisabled"
                                SelectedHeaderCssClass="LogistikButtonTour" UserControl="~/Components/Logistik/Change02sUserControls/WSFahrten.ascx" />
                            <custom:WizardStep VisitedHeaderCssClass="LogistikButtonDL_Ready" HeaderCssClass="LogistikButtonDL_Disabled"
                                SelectedHeaderCssClass="LogistikButtonDL" UserControl="~/Components/Logistik/Change02sUserControls/WSDienstleistungen.ascx" />
                            <custom:WizardStep VisitedHeaderCssClass="VersandButtonOptionenReady" HeaderCssClass="VersandButtonOverviewEnabled"
                                SelectedHeaderCssClass="VersandButtonOverview" UserControl="~/Components/Logistik/Change02sUserControls/WSÜbersicht.ascx" />
                        </Steps>
                        <WizardNavigationHeaderTemplate>
                            <div class="DivVersandTabContainer">
                        </WizardNavigationHeaderTemplate>
                        <WizardNavigationFooterTemplate>
                            <custom:ProgressIndicator ID="ProgressIndicator1" runat="server" CurrentIndex="<%# Container.SelectedIndex %>"
                                StepCount="<%# Container.StepCount %>">
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
                                    </tr> </table> </div>
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
    <asp:DataGrid ID="aspdatacheck" runat="server">
    </asp:DataGrid>
</asp:Content>
