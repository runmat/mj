<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report15.aspx.vb" Inherits="AppF2.Report15"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <uc1:Kopfdaten ID="kopfdaten" runat="server" />
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
                            <telerik:AjaxSetting AjaxControlID="logGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="logGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="cmdSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="logGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                    <telerik:AjaxUpdatedControl ControlID="tab1" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearchUp" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="NewSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="tab1" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearchUp" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="NewSearchUp">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="tab1" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="NewSearchUp" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <div id="TableQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="false" />
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" ToolTip="Abfrage öffnen" ImageUrl="../../../Images/queryArrow.gif"
                                                Visible="false" OnClick="NewSearch_Click" CausesValidation="false" />
                                            <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                                Visible="false" OnClick="NewSearchUp_Click" CausesValidation="false" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" style="padding-right: 70px; white-space: nowrap">
                                    <asp:RadioButton ID="rbtnCurrSession" runat="server" GroupName="Mode" AutoPostBack="true"
                                        Checked="true" Text="Aktuelle Sitzung" OnCheckedChanged="ModeChanged" />
                                </td>
                                <td class="active" colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rbtnDate" runat="server" GroupName="Mode" AutoPostBack="true"
                                        Text="Datumsauswahl" OnCheckedChanged="ModeChanged" />
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtDate" runat="server" Width="100px" Enabled="false" />
                                    <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" TargetControlID="txtDate" />
                                    <ajaxToolkit:MaskedEditExtender ID="meeDate" runat="server" TargetControlID="txtDate"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight" />
                                </td>
                                <td class="active">
                                    <asp:RequiredFieldValidator ControlToValidate="txtDate" ID="rfvDate" runat="server"
                                        ErrorMessage="Bitte wählen Sie ein Datum" Enabled="false" />
                                </td>
                                <td class="active">
                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" CausesValidation="True" Font-Underline="False" OnClick="SearchClick"
                                        Style="margin-left: 20px">» Weiter</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 2px;">
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false"
                            Visible="false" />
                        <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false"
                            Visible="false" Text="Keine ausgeführten Tätigkeiten für die angegebenen Kriterien gefunden" />
                    </div>
                    <telerik:RadGrid ID="logGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                        BorderWidth="0" Culture="de-DE" Visible="false" OnItemCommand="GridItemCommand"
                        OnNeedDataSource="GridNeedDataSource" OnExcelMLExportRowCreated="GridExportRowCreated"
                        OnExcelMLExportStylesCreated="GridExportStylesCreated">
                        <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" Summary="Ausgeführte Tätigkeiten">
                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="Inserted" SortExpression="Inserted" UniqueName="Inserted"
                                    DataFormatString="<nobr>{0:dd.MM.yyyy}</nobr>" />
                                <telerik:GridBoundColumn DataField="Task" SortExpression="Task" UniqueName="Task"
                                    DataFormatString="<nobr>{0}</nobr>" />
                                <telerik:GridBoundColumn DataField="Description" SortExpression="Description" UniqueName="Description"
                                    DataFormatString="<nobr>{0}</nobr>" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
