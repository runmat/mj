<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report12.aspx.vb" Inherits="AppF2.Report12"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    </script>
                    <telerik:RadAjaxManager runat="server">
                        <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="bestandGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="cmdSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="bestandGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="cmdSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="cmdSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="bestandGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="lblError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadGrid ID="bestandGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                        BorderWidth="0" Culture="de-DE" Visible="false" OnItemCommand="GridItemCommand"
                        OnNeedDataSource="GridNeedDataSource" OnExcelMLExportRowCreated="GridExportRowCreated" OnExcelMLExportStylesCreated="GridExportStylesCreated">
                        <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" Summary="Händlerbestand">
                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="NAME1" SortExpression="NAME1" UniqueName="NAME1" />
                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" />
                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" UniqueName="TIDNR" />
                                <telerik:GridBoundColumn DataField="ZZFINART_TEXT" SortExpression="ZZFINART_TEXT"
                                    UniqueName="ZZFINART_TEXT" />
                                <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM" />
                                <telerik:GridTemplateColumn DataField="ZZCOCKZ" SortExpression="ZZCOCKZ" UniqueName="ZZCOCKZ">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("ZZCOCKZ") = "X" %>'
                                            Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="DATAB" SortExpression="DATAB" UniqueName="DATAB"
                                    DataFormatString="{0:dd.MM.yyyy}" />
                                <telerik:GridBoundColumn DataField="ZZHERSTELLER_SCH" SortExpression="ZZHERSTELLER_SCH"
                                    UniqueName="ZZHERSTELLER_SCH" />
                                <telerik:GridBoundColumn DataField="ZZTYP_SCHL" SortExpression="ZZTYP_SCHL" UniqueName="ZZTYP_SCHL" />
                                <telerik:GridBoundColumn DataField="ZZHUBRAUM_SORT" SortExpression="ZZHUBRAUM_SORT" UniqueName="ZZHUBRAUM_SORT"
                                    ItemStyle-HorizontalAlign="Right" />
                                <telerik:GridBoundColumn DataField="ZZNENNLEISTUNG_SORT" SortExpression="ZZNENNLEISTUNG_SORT"
                                    UniqueName="ZZNENNLEISTUNG_SORT" ItemStyle-HorizontalAlign="Right" />
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
