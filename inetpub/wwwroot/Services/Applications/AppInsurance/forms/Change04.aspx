<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="AppInsurance.Change04"
    MasterPageFile="../MasterPage/App.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <%--<asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="navigateBack"></asp:LinkButton>--%>
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück"></asp:LinkButton>
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
                    <telerik:RadAjaxManager id="RadAjaxManager1" runat="server">
                        <clientevents onrequeststart="onRequestStart" />
                        <%-- <ajaxsettings>
                            <telerik:AjaxSetting AjaxControlID="fzgGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="fzgGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="buttonsPanel" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </ajaxsettings>--%>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel id="RadAjaxLoadingPanel1" runat="server" />
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 0px 0px 10px 15px;
                        margin-top: 10px">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <telerik:RadGrid id="fzgGrid" allowsorting="true" allowpaging="true" allowautomaticinserts="false"
                            autogeneratecolumns="false" pagesize="10" runat="server" gridlines="None" width="100%"
                            borderwidth="0" culture="de-DE" enableheadercontextmenu="true" onneeddatasource="GridNeedDataSource">
                            <clientsettings allowcolumnsreorder="true" allowkeyboardnavigation="true">
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                            </clientsettings>
                            <mastertableview commanditemdisplay="None" summary="Rückläuferklärfälle">
                                <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge"
                                    AlwaysVisible="True" />
                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false"
                                    ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false"
                                    ExportToExcelText="" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="KUNDE" SortExpression="KUNDE" UniqueName="KUNDE" />
                                    <telerik:GridBoundColumn DataField="EIKTO_VM" SortExpression="EIKTO_VM" UniqueName="EIKTO_VM" />
                                    <telerik:GridBoundColumn DataField="VERS_JAHR" SortExpression="VERS_JAHR" UniqueName="VERS_JAHR" />
                                    <telerik:GridBoundColumn DataField="SERNR" SortExpression="SERNR" UniqueName="SERNR"/>
                                        <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" UniqueName="ERDAT"
                                        DataFormatString="{0:dd.MM.yyyy}" />
                                    <telerik:GridBoundColumn DataField="FEHLER_SCAN" SortExpression="FEHLER_SCAN" UniqueName="FEHLER_SCAN" />
                                    <telerik:GridBoundColumn DataField="FEHLER_SCAN_TXT" SortExpression="FEHLER_SCAN_TXT"
                                        UniqueName="FEHLER_SCAN_TXT" DataFormatString="<nobr>{0}</nobr>" />
                                    <telerik:GridTemplateColumn DataField="INFO_VERS" SortExpression="INFO_VERS" UniqueName="INFO_VERS">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" DataValueField="INFO_VERS"
                                                            DataTextField="INFO_VERS_TXT" OnLoad="infoVersLoad" OnSelectedIndexChanged="infoVersChanged"
                                                            SelectedValue='<%# Eval("INFO_VERS") %>' AppendDataBoundItems="true" Width="100%">
                                                            <asp:ListItem Text="-- Bitte wählen --" Value="" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </mastertableview>
                        </telerik:RadGrid>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:Panel ID="buttonsPanel" runat="server">
                            <asp:LinkButton ID="lbQuery" runat="server" CssClass="TablebuttonLarge" Height="18px"
                                Width="120px" OnClick="runQuery">» Abfrage starten</asp:LinkButton>
                            <asp:LinkButton ID="lbSave" runat="server" CssClass="TablebuttonLarge" Height="18px"
                                Width="120px" OnClick="save" Enabled="false">» Speichern</asp:LinkButton>
                        </asp:Panel>
                    </div>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
