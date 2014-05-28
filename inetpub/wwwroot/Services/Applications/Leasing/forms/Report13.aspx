<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report13.aspx.cs" Inherits="Leasing.forms.Report13"
    MasterPageFile="../Master/App.Master" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Neue Abfrage
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="cmdNewSearchOpen" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearchOpen" />
                                            <asp:ImageButton ID="cmdNewSearchClose" runat="server" ToolTip="Abfrage schließen"
                                                ImageUrl="../../../Images/queryArrowUp.gif" Visible="false" OnClick="NewSearchClose" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:Panel ID="SearchParams" runat="server" DefaultButton="cmdSearch">
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" style="width: 100%">
                                        <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" EnableViewState="false"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" EnableViewState="false"></asp:Label>&nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server" />
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtFahrgestellnummer" CssClass="TextBoxNormal" runat="server" MaxLength="30" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Kennzeichen" runat="server" />
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtKennzeichen" CssClass="TextBoxNormal" runat="server" MaxLength="15" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Vertragsnummer" runat="server" />
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtVertragsnummer" CssClass="TextBoxNormal" runat="server" MaxLength="20" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Objektnummer" runat="server" />
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtObjektnummer" CssClass="TextBoxNormal" runat="server" MaxLength="25" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label runat="server" ID="lbl_Datum" />
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtDatumVon" CssClass="TextBoxNormal" runat="server" Width="75px" />
                                        <ajax:CalendarExtender ID="ceDatumVon" runat="server" TargetControlID="txtDatumVon"
                                            Format="dd.MM.yyyy" PopupPosition="BottomLeft" Animated="false" />
                                        <ajax:MaskedEditExtender ID="meeDatumVon" runat="server" TargetControlID="txtDatumVon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight" />
                                        &nbsp;
                                        <asp:TextBox ID="txtDatumBis" CssClass="TextBoxNormal" runat="server" Width="75px" />
                                        <ajax:CalendarExtender ID="ceDatumBis" runat="server" TargetControlID="txtDatumBis"
                                            Format="dd.MM.yyyy" PopupPosition="BottomLeft" Animated="false" />
                                        <ajax:MaskedEditExtender ID="meeDatumBis" runat="server" TargetControlID="txtDatumBis"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <telerik:RadAjaxManager runat="server">
                        <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="tempBriefGrid">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="tempBriefGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="cmdEnd" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="cmdEnd">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="tempBriefGrid" />
                                    <telerik:AjaxUpdatedControl ControlID="cmdEnd" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadGrid ID="tempBriefGrid" runat="server" AllowSorting="true" AllowPaging="true"
                        AutoGenerateColumns="False" GridLines="None" Width="100%" BorderWidth="0px" Culture="de-DE"
                        Visible="false" CellSpacing="0" OnItemCommand="TempBriefItemCommand" OnNeedDataSource="TempBriefNeedDataSource">
                        <ExportSettings HideStructureColumns="true" IgnorePaging="True" OpenInNewWindow="True">
                            <Excel Format="ExcelML" />
                            <Pdf AllowCopy="true" />
                        </ExportSettings>
                        <ClientSettings AllowKeyboardNavigation="true" AllowColumnsReorder="false">
                            <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                        </ClientSettings>
                        <HeaderStyle CssClass="RadGridHeader" Wrap="False" />
                        <FilterMenu EnableImageSprites="False" />
                        <MasterTableView CommandItemDisplay="Top" Summary="Temporär versendete Briefe" TableLayout="Auto"
                            Width="100%">
                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge"
                                AlwaysVisible="True" />
                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ExportToExcelText="Export to XLS"
                                ShowAddNewRecordButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn SortExpression="Status" UniqueName="Status" HeaderText=" ">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# (string)Eval("Status") == "X" %>' Visible='<%# (string)Eval("Status") == "-" || (string)Eval("Status") == "X" %>'
                                            AutoPostBack="True" OnCheckedChanged="ToggleSelection" />
                                        <asp:Image runat="server" Visible='<%# !string.IsNullOrEmpty((string)Eval("Status"))  && (string)Eval("Status") != "-" && (string)Eval("Status") != "X" %>'
                                            Width="12px" Height="12px" ToolTip='<%# Eval("Status") %>' ImageUrl="/Services/Images/Ausrufezeichen01_10.jpg" />
                                        <asp:Image runat="server" Visible='<%# string.IsNullOrEmpty((string)Eval("Status")) %>'
                                            Width="12px" Height="12px" ToolTip="Entsperrt" ImageUrl="/Services/Images/ok02_10.jpg" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%--Equipmentnummer--%>
                                <telerik:GridBoundColumn DataField="EQUNR" SortExpression="EQUNR" UniqueName="EQUNR"
                                    Display="False" Groupable="False" ItemStyle-Wrap="false" />
                                <%--Fahrgestellnummer--%>
                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM"
                                    Display="True" HeaderStyle-Width="22%" Groupable="False" ItemStyle-Wrap="false" />
                                <%--Kfz-Kennzeichen--%>
                                <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM"
                                    Display="True" HeaderStyle-Width="22%" Groupable="False" ItemStyle-Wrap="false" />
                                <%--Vertragsnummer--%>
                                <telerik:GridBoundColumn DataField="LIZNR" SortExpression="LIZNR" UniqueName="LIZNR"
                                    Display="True" HeaderStyle-Width="22%" Groupable="False" ItemStyle-Wrap="false" />
                                <%--Objektnummer--%>
                                <telerik:GridBoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" UniqueName="ZZREFERENZ1"
                                    Display="True" HeaderStyle-Width="22%" Groupable="False" ItemStyle-Wrap="false" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" Style="float: left;" CausesValidation="False" Font-Underline="False"
                            OnClick="SearchClick">» Weiter</asp:LinkButton>
                        <asp:LinkButton ID="cmdEnd" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                            Style="float: left;" CausesValidation="False" Font-Underline="False" OnClick="Entsperren"
                            Enabled="False">» Entsperren</asp:LinkButton>
                    </div>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
