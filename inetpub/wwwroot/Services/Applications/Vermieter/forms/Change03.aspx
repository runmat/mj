<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change03.aspx.cs" Inherits="Vermieter.forms.Change03"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <script type="text/javascript">
                        function onRequestStart(sender, args) {
                            if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                                    args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                                    args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                                    args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                                args.set_enableAjax(false);
                            }
                        }
                    </script>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="fzgGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="fzgGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Auswahl
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                ToolTip="Filter öffnen" Visible="false" OnClick="NewSearch_Click" />
                                            <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Filter schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                                Visible="false" OnClick="NewSearchUp_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 0px 0px 10px 15px;
                        margin-top: 10px">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>
                    <div id="TableQuery" style="margin-bottom: 10px">
                        <table>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td class="firstLeft active" style="padding-top: 5px; padding-bottom: 5px;">
                                                    <asp:Label ID="lbl_Fahrzeugbestand" runat="server">lbl_Fahrzeugbestand</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                    <asp:RadioButton ID="rdbAuswahl0" runat="server" Checked="true" AutoPostBack="true"
                                                        GroupName="Auswahl" OnCheckedChanged="rdbAuswahl_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="padding-top: 5px; padding-bottom: 5px;">
                                                    <asp:Label ID="lbl_ZulassungPlan" runat="server" Width="120px">lbl_ZulassungPlan</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                    <asp:RadioButton ID="rdbAuswahl1" runat="server" AutoPostBack="true" GroupName="Auswahl"
                                                        OnCheckedChanged="rdbAuswahl_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td style="width: 100%">
                                    <table id="tblAuswahlZulassung" runat="server" cellpadding="0" cellspacing="0" visible="false">
                                        <tbody>
                                            <tr>
                                                <td class="firstLeft active" style="padding-top: 5px; padding-bottom: 5px;">
                                                    <asp:Label ID="lbl_Zulassung" runat="server">lbl_Zulassung</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                    <asp:RadioButton ID="rdbZulassung" runat="server" GroupName="Zulassung" AutoPostBack="True"
                                                        OnCheckedChanged="rdbAuswahl_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="padding-top: 5px; padding-bottom: 5px;">
                                                    <asp:Label ID="lbl_Plan" runat="server" Width="110px">lbl_Plan</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                    <asp:RadioButton ID="rdbPlan" runat="server" GroupName="Zulassung" AutoPostBack="True"
                                                        OnCheckedChanged="rdbAuswahl_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td>
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="ibtFilterOpen" runat="server" ImageUrl="/services/images/FilterOpen.png"
                                                        Visible="False" OnClick="ibtFilterOpen_Click" />
                                                    <asp:ImageButton ID="ibtFilterClose" runat="server" ImageUrl="/services/images/FilterClose.png"
                                                        Visible="False" OnClick="ibtFilterClose_Click" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table id="tblEinzel" runat="server" visible="false" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" align="right">
                                        Anzahl gefundener Fahrzeuge:&nbsp;<asp:Label ID="lbl_FilterPreview" runat="server"
                                            Font-Bold="true" Font-Size="120%" ForeColor="Blue" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Carport" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Carport" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlCarport" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="KUNPDI" DataValueField="KUNPDI" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Hersteller" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Hersteller" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlHersteller" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="ZZHERST_TEXT" DataValueField="ZZHERSTELLER_SCH" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Modell" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Modell" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlModell" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="ZZHANDELSNAME" DataValueField="ZZHANDELSNAME" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Antrieb" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Antrieb" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlAntrieb" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="ZZKRAFTSTOFF_TXT"
                                            DataValueField="ZZKRAFTSTOFF_TXT" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Farbe" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Farbe" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlFarbe" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="FARBTEXT" DataValueField="FARBTEXT" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_FzgEingegangen" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_FzgEingegangen" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <span>
                                            <asp:CheckBox ID="cbxFzgEingegangen" runat="server" AutoPostBack="true" OnCheckedChanged="filterParamChanged" />
                                        </span>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_FzgBereit" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_FzgBereit" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <span>
                                            <asp:CheckBox ID="cbxFzgBereitShow" runat="server" AutoPostBack="true" OnCheckedChanged="filterParamChanged" />
                                        </span>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_FzgZBIIEingegangen" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_FzgZBIIEingegangen" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <span>
                                            <asp:CheckBox ID="cbxFzgZBIIEingegangenShow" runat="server" AutoPostBack="true" OnCheckedChanged="filterParamChanged" />
                                        </span>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Zulassungsbereit" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Zulassungsbereit" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <span>
                                            <asp:CheckBox ID="cbxZulassungsbereit" runat="server" AutoPostBack="true" OnCheckedChanged="filterParamChanged" />
                                        </span>
                                    </td>
                                </tr>
                                <tr id="tr_PlandatumFilter" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_PlandatumFilter" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:RadioButtonList ID="rblPlandatumFilter" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="filterParamChanged">
                                            <asp:ListItem Value="0" Selected="True">alle</asp:ListItem>
                                            <asp:ListItem Value="1">mit Plandatum</asp:ListItem>
                                            <asp:ListItem Value="2">ohne Plandatum</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_PlandatumVon" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_PlandatumVon" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:TextBox ID="txtPlandatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"
                                            AutoPostBack="true" OnTextChanged="filterParamChanged" />
                                        <ajaxToolkit:CalendarExtender ID="CE_PlandatumVon" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtPlandatumVon">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MEE_PlandatumVon" runat="server" TargetControlID="txtPlandatumVon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_PlandatumBis" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_PlandatumBis" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:TextBox ID="txtPlandatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"
                                            AutoPostBack="true" OnTextChanged="filterParamChanged" />
                                        <ajaxToolkit:CalendarExtender ID="CE_PlandatumBis" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtPlandatumBis">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MEE_PlandatumBis" runat="server" TargetControlID="txtPlandatumBis"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_ZulassunsdatumVon" runat="server" visible="false">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_ZulassunsdatumVon" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:TextBox ID="txtZulassunsdatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"
                                            AutoPostBack="true" OnTextChanged="filterParamChanged" />
                                        <ajaxToolkit:CalendarExtender ID="CE_ZulassunsdatumVon" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtZulassunsdatumVon">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MEE_ZulassunsdatumVon" runat="server" TargetControlID="txtZulassunsdatumVon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_ZulassunsdatumBis" runat="server" visible="false">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_ZulassunsdatumBis" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:TextBox ID="txtZulassunsdatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"
                                            AutoPostBack="true" OnTextChanged="filterParamChanged" />
                                        <ajaxToolkit:CalendarExtender ID="CE_ZulassunsdatumBis" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtZulassunsdatumBis">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MEE_ZulassunsdatumBis" runat="server" TargetControlID="txtZulassunsdatumBis"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Fahrgestellnummer" runat="server" visible="false">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                            Width="400px" AutoPostBack="true" OnTextChanged="filterParamChanged" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Fahrzeugnummer" runat="server" visible="false">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Fahrzeugnummer" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:TextBox ID="txtFahrzeugnummer" runat="server" CssClass="TextBoxNormal" MaxLength="25"
                                            Width="400px" AutoPostBack="true" OnTextChanged="filterParamChanged" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_EmpfScheinSchilder" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_EmpfScheinSchilder" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlEmpfZulUnterlagenFilter" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="EmpfZulUnterlagenText"
                                            DataValueField="ZZCARPORT_PLAN" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_HalterFilter" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_HalterFilter" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlHalterFilter" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="Halter" DataValueField="KUNNR_ZH" />
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_LieferantFilter" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_LieferantFilter" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                        <asp:DropDownList ID="ddlLieferantFilter" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="filterParamChanged" DataTextField="Display" DataValueField="KUNNR_ZP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbResetFilter" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbResetFilter_Click" Visible="false">» Alle</asp:LinkButton>
                        <asp:LinkButton ID="lbSetFilter" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbSetFilter_Click" Visible="false">» Filtern</asp:LinkButton>
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div style="padding-bottom: 10px">
                            <table id="tblZulassung" runat="server" visible="false" cellpadding="0" cellspacing="0"
                                width="100%" style="border-width: 1px; border-color: #dfdfdf; border-style: solid;">
                                <tbody>
                                    <tr>
                                        <td width="330px">
                                            <table cellpadding="0" cellspacing="0" >
                                                <tbody>
                                                    <tr class="formquery">
                                                        <td class="firstLeft active" style="color: #595959;">
                                                            <asp:Label ID="lbl_Halter" runat="server">lbl_Halter</asp:Label>
                                                        </td>
                                                        <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                            <asp:DropDownList ID="ddlHalter" runat="server" Width="200px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlHalter_SelectedIndexChanged" DataTextField="Name"
                                                                DataValueField="HALTER" />
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td class="firstLeft active" style="color: #595959;">
                                                            <asp:Label ID="lbl_Versicherer" runat="server">lbl_Versicherer</asp:Label>
                                                        </td>
                                                        <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                            <asp:DropDownList ID="ddlVersicherer" runat="server" Width="200px" DataTextField="NAME1"
                                                                DataValueField="VERSICHERER" />
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td class="firstLeft active" style="color: #595959;">
                                                            <asp:Label ID="lbl_Kennzeichenserie" runat="server">lbl_Kennzeichenserie</asp:Label>
                                                        </td>
                                                        <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                            <asp:DropDownList ID="ddlKennzeichenserie" runat="server" Width="200px" Enabled="False"
                                                                DataTextField="Kennzeichen" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr class="formquery">
                                                        <td class="firstLeft active" style="color: #595959; width:140px;">
                                                            <asp:Label ID="lbl_EmpfZulUnterlagen" runat="server" style="white-space:normal;">lbl_EmpfZulUnterlagen</asp:Label>
                                                        </td>
                                                        <td class="firstLeft active" style="padding-left: 4px; width: 100%;">
                                                            <asp:DropDownList ID="ddlEmpfZulUnterlagen" runat="server" Width="100%" DataTextField="Name"
                                                                DataValueField="DADPDI" />
                                                        </td>
                                                    </tr>
                                                    <tr class="formquery">
                                                        <td class="firstLeft active" style="color: #595959;">
                                                            <asp:Label ID="lbl_DatumZulassung" runat="server">lbl_DatumZulassung</asp:Label>
                                                        </td>
                                                        <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                            <asp:TextBox ID="txtDatumZulassung" runat="server" CssClass="TextBoxNormal" Width="80px"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd.MM.yyyy"
                                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumZulassung">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatumZulassung"
                                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_VerarbeitungZulassung" runat="server" class="formquery" visible="false">
                                                        <td class="firstLeft active" style="color: #595959;">
                                                            <asp:Label ID="lbl_DatumVerarbeitungZul" runat="server">lbl_DatumVerarbeitungZul</asp:Label>
                                                        </td>
                                                        <td class="firstLeft active" style="padding-left: 4px; width: 100%">
                                                            <asp:TextBox ID="txtDatumVerarbeitungZul" runat="server" CssClass="TextBoxNormal"
                                                                Width="80px"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd.MM.yyyy"
                                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVerarbeitungZul">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtDatumVerarbeitungZul"
                                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbSave" runat="server" CssClass="Tablebutton" Width="78px" Style="float: right;
                                                margin-right: 5px; margin-bottom: 5px; margin-top: 10px;" OnClick="lbSave_Click"
                                                Visible="false">» Absenden</asp:LinkButton>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <telerik:RadGrid ID="fzgGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                            AutoGenerateColumns="false" PageSize="10" runat="server" GridLines="None" Width="100%"
                            BorderWidth="0" Culture="de-DE" EnableHeaderContextMenu="true" OnExcelMLExportRowCreated="GridExcelMLExportRowCreated"
                            OnExcelMLExportStylesCreated="GridExcelMLExportStylesCreated" OnItemCommand="GridItemCommand"
                            OnItemCreated="GridItemCreated" OnNeedDataSource="GridNeedDataSource">
                            <ExportSettings HideStructureColumns="true">
                                <Excel Format="ExcelML" />
                            </ExportSettings>
                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true">
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                            </ClientSettings>
                            <MasterTableView CommandItemDisplay="Top" Summary="Fahrzeugbestand">
                                <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge"
                                    AlwaysVisible="True" />
                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                    ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false"
                                    ExportToExcelText="" />
                                <Columns>
                                    <telerik:GridTemplateColumn SortExpression="Selected" UniqueName="Selected" HeaderText=""
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="selectedCheckbox" AutoPostBack="true" OnCheckedChanged="selectedChanged"
                                                Checked='<%# Eval("Selected") %>' /></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="selectAll" AutoPostBack="true" OnCheckedChanged="selectedAllChanged" />
                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Status" SortExpression="Status" UniqueName="Status"
                                        DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" />
                                    <telerik:GridBoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" UniqueName="ZZREFERENZ1" />
                                    <telerik:GridBoundColumn DataField="KUNPDI" SortExpression="KUNPDI" UniqueName="KUNPDI"
                                        DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="ZZHERST_TEXT" SortExpression="ZZHERST_TEXT" UniqueName="ZZHERST_TEXT"
                                        DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="ZZHANDELSNAME" SortExpression="ZZHANDELSNAME"
                                        UniqueName="ZZHANDELSNAME" DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="ZZKRAFTSTOFF_TXT" SortExpression="ZZKRAFTSTOFF_TXT"
                                        UniqueName="ZZKRAFTSTOFF_TXT" DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="FARBTEXT" SortExpression="FARBTEXT" UniqueName="FARBTEXT"
                                        DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="ZZDAT_EIN" SortExpression="ZZDAT_EIN" UniqueName="ZZDAT_EIN"
                                        DataFormatString="{0:dd.MM.yyyy}" />
                                    <telerik:GridBoundColumn DataField="ZZDAT_BER" SortExpression="ZZDAT_BER" UniqueName="ZZDAT_BER"
                                        DataFormatString="{0:dd.MM.yyyy}" />
                                    <telerik:GridBoundColumn DataField="ERDAT_EQUI" SortExpression="ERDAT_EQUI" UniqueName="ERDAT_EQUI"
                                        DataFormatString="{0:dd.MM.yyyy}" />
                                    <telerik:GridTemplateColumn DataField="ZULBEREIT" SortExpression="ZULBEREIT" UniqueName="ZULBEREIT"
                                        Display="false">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Enabled="false" Checked='<%# "X".Equals(Eval("ZULBEREIT")) %>' /></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="PLZULDAT" SortExpression="PLZULDAT" UniqueName="PLZULDAT"
                                        DataFormatString="{0:dd.MM.yyyy}" />
                                    <telerik:GridBoundColumn DataField="DURCHFD" SortExpression="DURCHFD" UniqueName="DURCHFD"
                                        DataFormatString="{0:dd.MM.yyyy}" />
                                    <telerik:GridBoundColumn DataField="Halter" SortExpression="Halter" UniqueName="Halter"
                                        DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="NAME1_ZP" SortExpression="NAME1_ZP" UniqueName="NAME1_ZP"
                                        DataFormatString="<nobr>{0}</nobr>" Display="false" />
                                    <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM"
                                        DataFormatString="<nobr>{0}</nobr>" Display="false" />
                                    <telerik:GridBoundColumn DataField="Versicherung" SortExpression="Versicherung" UniqueName="Versicherung"
                                        DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridBoundColumn DataField="EmpfZulUnterlagenText" SortExpression="EmpfZulUnterlagenText"
                                        UniqueName="EmpfZulUnterlagenText" DataFormatString="<nobr>{0}</nobr>" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
