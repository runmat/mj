<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report07.aspx.vb" Inherits="AppInsurance.Report07" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        <asp:Label ID="lblSelection" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="active" valign="top" align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" 
                                                ImageUrl="../../../Images/queryArrow.gif" onclick="NewSearch_Click" 
                                                style="width: 18px" />
                                            
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                   
                    <div>

                    <table width="100%" id="tabError" cellpadding="0" cellspacing="0" style="border-bottom:0px">
                    <tr class="formquery"><td class="firstLeft active">
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false" Visible="false" />
                    <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false" Visible="false" />
                    </td></tr></table>
                   
                    <asp:Panel ID="pnlSelection" runat="server">
                    <table width="100%" id="tabOuter" runat="server" cellpadding="0" cellspacing="0" >
                            <tbody>
                                   
                                <tr class="formquery" >
                                    <td class="firstLeft active">
     
                            <table id="tabSelektion" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                      <tr class="formquery">
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_VersJahr" Text="lbl_VersJahr" runat="server"></asp:Label>
                                        </td>
                                     <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_VersJahr" runat="server" Width="200px" ></asp:TextBox>
                                    </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_SerialNr" Text="lbl_SerialNr" runat="server"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txt_SerialNr" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="200px" ></asp:TextBox>
                                          </td>
                                    </tr>
                                                         <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Vermittler" Text="lbl_Vermittler" runat="server"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txt_Vermittler" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="200px" ></asp:TextBox>
                                                <%--  &nbsp;<asp:Label ID="lblPlatzhaltersuche" runat="server">*(mit Platzhaltersuche)</asp:Label>--%>
                                                <cc1:MaskedEditExtender ID="txt_Vermittler_MaskedEditExtender" runat="server" AutoComplete="False"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True" Mask="CCCC-CCCC-C" TargetControlID="txt_Vermittler"
                                                MessageValidatorTip="False" Filtered="1234567890*">
                                            </cc1:MaskedEditExtender>
                                          </td>
                                    </tr>

                                </tbody>
                            </table>

                         </td>
                        <td align="right" style="width:100%;vertical-align:top;padding-right:5px;" >
                            &nbsp</td> 

                                    <td style="width:100%;vertical-align:top;padding-right:5px;">
                                        <asp:Panel ID="Panel1" runat="server" style="text-align:left" Visible="true" width="350px">
                                            <div class="new_layout">
                                                <div id="infopanel" class="infopanel">
                                                    <label style="margin-left:0; margin-right:0; padding-right:0; width:275px">
                                                    Information!</label>
                                                    <div>
                                                        Das Versicherungsjahr ist ein Pflichtfeld.<br/> 
                                                                                                   
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </td>

                        </tr>
                            <tr class="formquery" >
                            <td>&nbsp</td>  
                            <td>&nbsp;</td> 
                            <td>&nbsp;</td> 
                         </tr>
                        </tbody> 
                        </table>


                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>      

                    </asp:Panel>
                    </div>
    
                        
                    <style type="text/css">
                    .MyImageButton
                    {
                        cursor: hand;
                    }
                    .EditFormHeader td
                    {
                        font-size: 14px;
                        padding: 4px !important;
                        color: #0066cc;
                    }
                     </style>    


                <script type="text/javascript">



                    function onRequestStart(sender, args) {
                        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                            args.set_enableAjax(false);
                        }
                    }

                    function RowDblClick(sender, eventArgs) {
                        sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
                    }


                </script>
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                    <!--

                        var hasChanges, inputs, dropdowns, editedRow;

                        function ShowColumnHeaderMenu(ev, columnName) {
                            alert("");
                            var grid = $find("<%=fzgGrid.ClientID %>");
                            var columns = grid.get_masterTableView().get_columns();
                            for (var i = 0; i < columns.length; i++) {
                                if (columns[i].get_uniqueName() == columnName) {
                                    columns[i].showHeaderMenu(ev, 75, 20);
                                }
                            }
                        }
                        -->
                    </script>
                </telerik:RadCodeBlock>

                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="fzgGrid">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="fzgGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                                 <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                                <telerik:AjaxUpdatedControl ControlID="lblError" />
                                <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>

                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />

                <div id="Result" runat="server" style="height:460px" visible="false">

                <telerik:radgrid ID="fzgGrid" AllowSorting="True" AllowPaging="True" 
                    AutoGenerateColumns="False" runat="server" GridLines="None" Width="100%" height="420px" BorderWidth="0px" 
                    AllowAutomaticDeletes="True"
                    AllowAutomaticInserts="True"
                    AllowAutomaticUpdates="True" 
                    OnItemUpdated="fzgGrid_ItemUpdated" 
                    OnEditCommand="fzgGrid_EditCommand"
                    Culture="de-DE" Visible="False" 
                    OnExcelMLExportRowCreated="fzgGrid_ExcelMLExportRowCreated"
                    OnExcelMLExportStylesCreated="fzgGrid_ExcelMLExportStylesCreated"
                    OnItemCommand="FzgGridItemCommand" 
                    OnNeedDataSource="FzgGridNeedDataSource"
                    OnPageIndexChanged="FzgGridPageChanged"
                    OnPageSizeChanged="FzgGridPageSizeChanged"
                    OnSortCommand="FzgGridSortCommand" CellSpacing="0" 
                    ClientSettings-AllowColumnsReorder="false" VirtualItemCount="2">
                    <ExportSettings HideStructureColumns="true" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData ="true" >
                    <Excel Format="ExcelML"></Excel>
                    </ExportSettings>
                    <MasterTableView DataKeyNames="" CommandItemDisplay="Top" Summary="KFZ Steuer Avisierung" TableLayout="Auto" Width="100%" EditMode="InPlace" AllowAutomaticUpdates="True">
                        <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                        </EditFormSettings>

                        <PagerStyle Mode="NextPrevAndNumeric" 
                            PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" 
                            AlwaysVisible="True" />
                        <CommandItemSettings 
                            ShowExportToWordButton="false" 
                            ShowExportToExcelButton="true"
                            ShowExportToCsvButton="false" 
                            ShowExportToPdfButton="false"
                            ExportToWordText="Export to Word"
                            ExportToExcelText="Export to XLS"
                            ExportToCsvText="Export to CSV"
                            ExportToPdfText="Export to PDF"
                            ShowAddNewRecordButton="false" 
                            />

                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                         <Columns>

                         <telerik:GridEditCommandColumn ButtonType="ImageButton" SortExpression="EditCommandColumn" HeaderText="" UniqueName="EditCommandColumn">
                            <ItemStyle CssClass="MyImageButton" Wrap="False" />
                             </telerik:GridEditCommandColumn>

                             <%--Kennzeichen--%>
                            <telerik:GridBoundColumn DataField="SERNR" 
                                FilterControlAltText="Filter SERNR column" SortExpression="SERNR" 
                                UniqueName="SERNR" Groupable="False" HeaderText="SERNR" ReadOnly="True">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--Bemerkung --%>
                            <telerik:GridBoundColumn DataField="BEMERKUNG" 
                                FilterControlAltText="Filter BEMERKUNG column" SortExpression="BEMERKUNG" 
                                UniqueName="BEMERKUNG" Display="true" Groupable="False" 
                                 HeaderText="BEMERKUNG" EditFormColumnIndex="0" ColumnEditorID="GridTextBoxColumnEditor1">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--Agentur--%>
                            <telerik:GridBoundColumn DataField="EIKTO_VM" 
                                FilterControlAltText="Filter EIKTO_VM column" SortExpression="EIKTO_VM" 
                                UniqueName="EIKTO_VM" Groupable="False" HeaderText="EIKTO_VM" 
                                 ReadOnly="True" ShowSortIcon="False">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--Name1 --%>
                            <telerik:GridBoundColumn DataField="NAME1" 
                               FilterControlAltText="Filter NAME1 column" 
                                SortExpression="NAME1" UniqueName="NAME1" Groupable="False" 
                                 HeaderText="NAME1" ReadOnly="True">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--Name2 --%>
                            <telerik:GridBoundColumn DataField="NAME2" 
                               FilterControlAltText="Filter NAME2 column" 
                                SortExpression="NAME2" UniqueName="NAME2" Groupable="False" 
                                 HeaderText="NAME2" ReadOnly="True">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--Ort --%>
                            <telerik:GridBoundColumn DataField="ORT01" 
                               FilterControlAltText="Filter ORT01 column" 
                                SortExpression="ORT01" UniqueName="ORT01" Groupable="False" 
                                 HeaderText="ORT01" ReadOnly="True">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--Rücklaufdatum --%>
                            <telerik:GridBoundColumn DataField="DAT_RUECK" 
                                DataFormatString="{0:dd.MM.yyyy}" DataType="System.DateTime" 
                                FilterControlAltText="Filter DAT_RUECK column" SortExpression="DAT_RUECK" 
                                UniqueName="DAT_RUECK" Groupable="False" HeaderText="DAT_RUECK" 
                                 ReadOnly="True">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--DAT_LETZAEND_BEM --%>
                            <telerik:GridBoundColumn DataField="DAT_LETZAEND_BEM" SortExpression="DAT_LETZAEND_BEM" 
                                DataFormatString="{0:dd.MM.yyyy}" DataType="System.DateTime" 
                                FilterControlAltText="Filter DAT_LETZAEND_BEM column" 
                                 UniqueName="DAT_LETZAEND_BEM" Groupable="true" 
                                 HeaderText="DAT_LETZAEND_BEM" ReadOnly="True" >
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                            <%--  <Abrechnungsjahr--%>
                            <telerik:GridBoundColumn DataField="VERS_JAHR"
                                FilterControlAltText="Filter VERS_JAHR column" SortExpression="VERS_JAHR" 
                                UniqueName="VERS_JAHR" Groupable="False" HeaderText="VERS_JAHR" 
                                 ReadOnly="True">
                                <ItemStyle Wrap="False" />
                            </telerik:GridBoundColumn>

                        </Columns>
                                <EditFormSettings>
                                    <EditColumn ButtonType="ImageButton" InsertText="Insert Order" UpdateText="Update record"
                                    UniqueName="EditCommandColumn1" CancelText="Cancel edit">

                                 </EditColumn>
                             </EditFormSettings>
                    </MasterTableView>

                    <HeaderStyle CssClass="RadGridHeader" Wrap="False" />
                    <PagerStyle AlwaysVisible="True" />

                <FilterMenu EnableImageSprites="False"></FilterMenu>

                    <ClientSettings allowkeyboardnavigation="True">
                     <ClientEvents OnRowDblClick="RowDblClick"/>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ></Scrolling>
                        <Resizing AllowColumnResize="True" ClipCellContentOnResize="false"></Resizing>
                    </ClientSettings>

                </telerik:radgrid>
                <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor1"  runat="server" TextBoxStyle-Width="180px" />
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>

                </div>

                <div id="dataQueryFooter">
                <asp:LinkButton ID="lb_Weiter" runat="server" Text="lb_Weiter" CssClass="Tablebutton" Width="78px"
                    Height="16px" CausesValidation="False" Font-Underline="False" 
                        OnClick="lb_Weiter_Click">» Suchen</asp:LinkButton>

                    </div>

                 <div id="dataFooter">&nbsp;</div>
                 
                 </div>

            </div>
        </div>
    </div>
</asp:Content>
