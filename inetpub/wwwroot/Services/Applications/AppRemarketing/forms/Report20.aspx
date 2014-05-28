<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report20.aspx.cs" Inherits="AppRemarketing.forms.Report20" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="cc1" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
               <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width:100%">
                                    <asp:Label ID="lblInfo" runat="server" style="white-space:normal;">Dieses Formular dient der Beantragung einer Gutachtenauswertung. <emph>Beantragte Reports werden jeweils über Nacht generiert und per Mail versandt.</emph></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false" style="white-space:normal;"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Fahrgestellnummer:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtFin" runat="server" CssClass="TextBoxNormal" MaxLength="17" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Kennzeichen:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="10" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Inventarnummer:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtInventarnr" runat="server" CssClass="TextBoxNormal" MaxLength="10" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Vermieter:
                                </td>
                                <td class="firstLeft active">
                                    <asp:DropDownList ID="ddlVermieter" runat="server" CssClass="DropDownNormal" Style="width:204px" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Hereinnahmecenter:
                                </td>
                                <td class="firstLeft active">
                                    <asp:DropDownList ID="ddlHC" runat="server" CssClass="DropDownNormal" Style="width:204px" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trDatumVon" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumVon" runat="server">Datum von:</asp:Label>
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtDatumVon" CssClass="TextBoxNormal" runat="server" />
                                    <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumVon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:RequiredFieldValidator ControlToValidate="txtDatumVon" ID="rfvDatumVon" runat="server" ErrorMessage="Bitte wählen Sie ein Datum"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trDatumBis" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumBis" runat="server">Datum bis:</asp:Label>
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtDatumBis" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:RequiredFieldValidator ControlToValidate="txtDatumBis" ID="rfvDatumBis" runat="server" ErrorMessage="Bitte wählen Sie ein Datum"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvDatumBis" runat="server" ErrorMessage="Wählen Sie 'Datum bis' größer als 'Datum von'!"
                                        ControlToCompare="txtDatumvon" ControlToValidate="txtDatumBis" Operator="GreaterThanEqual"
                                        Type="Date"></asp:CompareValidator>
                                </td>
                            </tr>

                             <tr class="formquery">
                                <td class="firstLeft active">
                                    Wertminderung (von/bis):
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtWertMinVon" runat="server" CssClass="TextBoxNormal" style="width:96px" MaxLength="10" />
                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtWertMinVon"
                                        Mask="9999999.99" MaskType="Number" InputDirection="RightToLeft" DisplayMoney="Right">
                                    </cc1:MaskedEditExtender>
                                    <asp:TextBox ID="txtWertMinBis" runat="server" CssClass="TextBoxNormal" style="width:96px" MaxLength="10" />
                                    <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtWertMinBis"
                                        Mask="9999999.99" MaskType="Number" InputDirection="RightToLeft" DisplayMoney="Right">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Wertminderung AV (von/bis):
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtWertAvVon" runat="server" CssClass="TextBoxNormal" style="width:96px" MaxLength="10" />
                                    <cc1:MaskedEditExtender ID="MEE_WertAvVon" runat="server" TargetControlID="txtWertAvVon"
                                        Mask="9999999.99" MaskType="Number" InputDirection="RightToLeft" DisplayMoney="Right">
                                    </cc1:MaskedEditExtender>
                                    <asp:TextBox ID="txtWertAvBis" runat="server" CssClass="TextBoxNormal" style="width:96px" MaxLength="10" />
                                    <cc1:MaskedEditExtender ID="MEE_WertAvBis" runat="server" TargetControlID="txtWertAvBis"
                                        Mask="9999999.99" MaskType="Number" InputDirection="RightToLeft" DisplayMoney="Right">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Schadensklasse:
                                </td>
                                <td class="firstLeft active">
                                    <asp:ListBox ID="lbKlasse" runat="server" CssClass="DropDownNormal" style="width:100%" SelectionMode="Multiple" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Massnahmen:
                                </td>
                                <td class="firstLeft active">
                                    <asp:ListBox ID="lbMassnahme" runat="server" CssClass="DropDownNormal" style="width:100%" SelectionMode="Multiple" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Schadensnummer:
                                </td>
                                <td class="firstLeft active">
                                    <asp:ListBox ID="lbSchadensnummer" runat="server" CssClass="DropDownNormal" style="width:100%" SelectionMode="Multiple" />
                                </td>
                            </tr>
                             <tr class="formquery">
                                <td class="firstLeft active">
                                    Teilenummer:
                                </td>
                                <td class="firstLeft active">
                                    <asp:ListBox ID="lbTeileNr" runat="server" CssClass="DropDownNormal" style="width:100%" SelectionMode="Multiple" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Vertragsjahr
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtVertragsjahr" runat="server" CssClass="TextBoxNormal" MaxLength="4" />
                                    <cc1:MaskedEditExtender ID="MEE_Vertragsjahr" runat="server" TargetControlID="txtVertragsjahr"
                                        Mask="9999" MaskType="Number" InputDirection="RightToLeft">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table id="ReportRequested" runat="server" visible="false" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:100%">
                                    <span class="TextLarge">Report beantragt</span>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft" style="width:100%;white-space:normal;">
                                    <span id="ReportRequestMsg" runat="server" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table >
                       
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» Weiter</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>