<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report12_2.aspx.cs" Inherits="AppRemarketing.forms.Report12_2"  MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu" style="margin-top: 10px; margin-bottom: 10px">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
                    <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="innerContentRight">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="innerContentRight" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <script type="text/javascript">
                    function onRequestStart(sender, args) {
                        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
                            args.set_enableAjax(false);
                        }
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading" style="float: none">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemCreated="rgGrid1_ItemCreated" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" 
                                            OnItemDataBound="rgGrid_ItemDataBound" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FAHRGNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn DataField="FAHRGNR" SortExpression="FAHRGNR" GroupByExpression="FAHRGNR GROUP BY FAHRGNR" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                                Text='<%# Eval("FAHRGNR") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="KENNZ" SortExpression="KENNZ" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="INVENTAR" SortExpression="INVENTAR" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZULDAT" SortExpression="ZULDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SOLLZULDAT" SortExpression="SOLLZULDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="POS_TEXT" SortExpression="POS_TEXT" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="15"  
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnNeedDataSource="rgGrid2_NeedDataSource" OnItemDataBound="rgGrid_ItemDataBound" >
                                            <ClientSettings AllowKeyboardNavigation="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="CHASSIS_NUM" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" GroupByExpression="CHASSIS_NUM GROUP BY CHASSIS_NUM" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                                Text='<%# Eval("CHASSIS_NUM") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="BEM" SortExpression="BEM" >
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
