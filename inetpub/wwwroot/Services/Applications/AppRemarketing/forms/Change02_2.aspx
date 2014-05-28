<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change02_2.aspx.cs" Inherits="AppRemarketing.forms.Change02_2"
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
                    <div id="TableQuery" style="margin-bottom: 10px">
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
                                            OnNeedDataSource="rgGrid1_NeedDataSource" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="3" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="Fahrgestellnummer" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>     
                                                    <telerik:GridTemplateColumn HeaderText="Aktion" Groupable="false">
                                                        <HeaderStyle Width="40px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnSperren" ImageUrl="../../../Images/Locked.gif" runat="server"
                                                                ToolTip="Sperren" CommandName="Sperren" Visible='<%# Eval("Versandsperre")== "" %>'
                                                                CommandArgument='<%# Eval("Fahrgestellnummer") %>'
                                                                Height="20px" Width="20px" onclick="ibtnSperren_Click" />
                                                            <asp:ImageButton ID="ibtnEntsperren" ImageUrl="../../../Images/Unlock.gif" runat="server"
                                                                Visible='<%# Eval("Versandsperre") != "" %>'
                                                                ToolTip="Entsperren" CommandName="Entsperren" CommandArgument='<%# Eval("Fahrgestellnummer") %>'
                                                                Height="20px" Width="20px" onclick="ibtnEntsperren_Click" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>         
                                                    <telerik:GridTemplateColumn DataField="Versandsperre" SortExpression="Versandsperre" GroupByExpression="Versandsperre GROUP BY Versandsperre" >  
                                                        <HeaderStyle Width="60px" />
                                                        <ItemTemplate>  
                                                            <asp:Label runat="server" Text='<%# Eval("Versandsperre") == "" ? "Nein" : "Ja" %>'></asp:Label>  
                                                        </ItemTemplate>  
                                                    </telerik:GridTemplateColumn> 
                                                    <telerik:GridBoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Belegnummer" SortExpression="Belegnummer" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Freigabe" SortExpression="Freigabe" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Zahlungsart" SortExpression="Zahlungsart" >
                                                        <HeaderStyle Width="75px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Adresse" SortExpression="Adresse" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AdresseBank" SortExpression="AdresseBank" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trUeberschriftGrid2" runat="server" class="formquery">
                                <td class="firstLeft active" style="width: 100%">
                                    nicht gefundene Fahrzeuge
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="15"  
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnNeedDataSource="rgGrid2_NeedDataSource" >
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
                                                    <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" >
                                                    </telerik:GridBoundColumn>
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

