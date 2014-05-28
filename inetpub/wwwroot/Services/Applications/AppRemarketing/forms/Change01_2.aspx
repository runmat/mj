<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change01_2.aspx.cs" Inherits="AppRemarketing.forms.Change01_2" 
    MasterPageFile="../Master/AppMaster.Master" %>

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
                        <table cellpadding="0" cellspacing="0">
                            <tr class="formquery" id="tr_HaendlerDetails1" runat="server">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblHaendlerDetailsNR" Font-Size="12px" Font-Bold="true"  runat="server"></asp:Label>
                                </td>
                                <td class="active" style="width:80%">
                                    <asp:Image ID="imgGesperrt" ImageUrl="../../../Images/Locked.gif"  
                                        runat="server" Visible="False"  Height="20px" Width="20px" />
                                        <asp:Image ID="imgOffen" ImageUrl="../../../Images/Unlock.gif" 
                                        runat="server" Visible="False" Height="20px" Width="20px" />
                                    &nbsp;<asp:Label ID="lblHaendlerGesperrt"  Font-Size="12px" Font-Bold="true" 
                                        runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery" id="tr_HaendlerDetails2" runat="server">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblHaendlerDetailsName1" Font-Size="12px"  Font-Bold="true"  runat="server"></asp:Label>
                                </td>
                                <td class="active">
                                    <asp:Label ID="lblHaendlerDetailsStrasse" Font-Size="12px"  Font-Bold="true" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery" id="tr_HaendlerDetails3" runat="server">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblHaendlerDetailsName2" Font-Size="12px"  Font-Bold="true" runat="server"></asp:Label>
                                </td>
                                <td class="active">
                                    <asp:Label ID="lblHaendlerDetailsPLZ" Font-Size="12px"  Font-Bold="true" runat="server"></asp:Label>
                                    &nbsp;<asp:Label ID="lblHaendlerDetailsOrt" Font-Size="12px"  Font-Bold="true" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdSperren" runat="server" CssClass="TablebuttonLarge" Width="130px"
                            Height="16px" OnClick="cmdSperren_Click">» Sperren</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="cmdEntsperren" runat="server" CssClass="TablebuttonLarge"
                            Width="130px" Height="16px" onclick="cmdEntsperren_Click">» Entsperren</asp:LinkButton>
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
                                            OnNeedDataSource="rgGrid1_NeedDataSource" ShowGroupPanel="True" >
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
                                                    <telerik:GridSortExpression FieldName="Fahrgestellnummer" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Belegnummer" SortExpression="Belegnummer" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" >
                                                        <HeaderStyle Width="105px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Rechnungsbetrag" SortExpression="Rechnungsbetrag" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Belegdatum" SortExpression="Belegdatum" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Valutadatum" SortExpression="Valutadatum" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Freigabe" SortExpression="Freigabe" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Zahlungsart" SortExpression="Zahlungsart" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Versandsperre" SortExpression="Versandsperre" >
                                                        <HeaderStyle Width="75px" />
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
