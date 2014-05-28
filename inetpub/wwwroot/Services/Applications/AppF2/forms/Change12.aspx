<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change12.aspx.vb" Inherits="AppF2.Change12"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <uc1:Kopfdaten ID="kopfdaten" runat="server" />
                    <div id="TableQuery">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false"
                            Visible="false" />
                        <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false"
                            Visible="false" />
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="False" Font-Underline="False" OnClick="SearchClick">» Weiter</asp:LinkButton>
                    </div>
                    <script type="text/javascript">
                        function onRequestStart(sender, args) {
                            if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                                    args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                                    args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                                    args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                                args.set_enableAjax(false);
                            }
                        }

                        function ConfirmStorno(auftragsnummer, fahrgestellnummer) {
                            return confirm("Möchten Sie den Auftrag '" + auftragsnummer + "'\nfür das Fahrzeug '" + fahrgestellnummer + "' stornieren?");
                        }
                    </script>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="oaGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="cmdSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="oaGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="cmdSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="cmdSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="oaGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadGrid ID="oaGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                        BorderWidth="0" Culture="de-DE" Visible="false" OnItemCommand="GridItemCommand"
                        OnNeedDataSource="GridNeedDataSource" OnPageIndexChanged="GridPageChanged" OnPageSizeChanged="GridPageSizeChanged"
                        OnSortCommand="GridSortCommand" OnItemDataBound="GridItemDataBound" OnExcelMLExportRowCreated="GridExportRowCreated"
                        OnExcelMLExportStylesCreated="GridExportStylesCreated">
                        <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" Summary="Offene Anforderungen">
                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="VBELN" SortExpression="VBELN" UniqueName="VBELN" />
                                <telerik:GridBoundColumn DataField="ZZREFNR1" SortExpression="ZZREFNR1" UniqueName="ZZREFNR1" />
                                <telerik:GridBoundColumn DataField="ZZANFDT" SortExpression="ZZANFDT" UniqueName="ZZANFDT"
                                    DataFormatString="{0:dd.MM.yyyy}" />
                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" />
                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" UniqueName="TIDNR" />
                                <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" UniqueName="MAKTX" />
                                <telerik:GridBoundColumn DataField="KONTART" SortExpression="KONTART" UniqueName="KONTART" />
                                <telerik:GridBoundColumn DataField="STATUS" SortExpression="STATUS" UniqueName="STATUS" />
                                <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Storno" Text="Storno"
                                    UniqueName="STORNO" HeaderButtonType="None" />
                                <telerik:GridBoundColumn DataField="EQUNR" SortExpression="EQUNR" UniqueName="EQUNR" />
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
