<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report100s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Report100s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="data" style="border-left: solid 1px #dfdfdf; border-right: solid 1px #dfdfdf">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                    &nbsp
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="vertical-align: top; padding-top: 5px; width: 10%"
                                    nowrap="nowrap">
                                    <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                                </td>
                                <td class="active" nowrap="nowrap" style="width: 90%">
                                    <span>
                                        <asp:RadioButtonList class="actives" ID="rdbCustomer" runat="server">
                                        </asp:RadioButtonList>
                                    </span>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td  class="firstLeft active" style="vertical-align: top; padding-top: 5px; "
                                    nowrap="nowrap">
                                    <asp:Label ID="lblERDatvon" runat="server" Text="Datum von:"></asp:Label>
                                </td>
                                <td  class="active" nowrap="nowrap">
                                    <asp:TextBox ID="txtERDatvon" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_ERDatvon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtERDatvon">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_ERDatvon" runat="server" TargetControlID="txtERDatvon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td  class="firstLeft active" style="vertical-align: top; padding-top: 5px; "
                                    nowrap="nowrap">
                                    <asp:Label ID="lblERDatbis" runat="server" Text="Datum bis:" />
                                </td>
                                <td class="active" nowrap="nowrap" style="width: 90%">
                                    <asp:TextBox ID="txtERDatbis" runat="server"  CssClass="TextBoxNormal"></asp:TextBox><cc1:CalendarExtender
                                        ID="CE_ERDatbis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtERDatbis">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_ERDatbis" runat="server" TargetControlID="txtERDatbis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap" colspan="2">
                                    &nbsp
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="False" Font-Underline="False">» Weiter</asp:LinkButton></div></div></div></div></div></asp:Content>