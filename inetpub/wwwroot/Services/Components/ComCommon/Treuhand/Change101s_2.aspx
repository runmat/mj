<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change101s_2.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change101_2s"
     MasterPageFile="../../../MasterPage/Services.Master" %>

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
                <div id="innerContentRight" style="width: 100%; margin-bottom: 10px">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="paginationQuery">
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand"  
                                            OnNeedDataSource="rgGrid1_NeedDataSource" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowKeyboardNavigation="true" >
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="BELNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="BELNR" SortExpression="BELNR" UniqueName="BELNR" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Auswahl" >
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSperre" runat="server" />
                                                            <asp:Label runat="server" Text="in Aut."
                                                                ID="lblInAut" Visible="false"> </asp:Label>                                                                                                                                                                            
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="NICHT_FREIG_GRU" SortExpression="NICHT_FREIG_GRU" >
                                                        <HeaderStyle Width="170px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAblehnungsgrund" Enabled='<%# Eval("SPERRSTATUS")<>"A" %>' 
                                                                Text='<%# Eval("NICHT_FREIG_GRU") %>' runat="server" MaxLength="253" TextMode="MultiLine" Rows="3" Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="FREIGABEUSER" SortExpression="FREIGABEUSER" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZKUNNR_AG" SortExpression="ZZKUNNR_AG" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="NAME1_AG" SortExpression="NAME1_AG" UniqueName="NAME1_AG" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="KUNNR_TG" SortExpression="KUNNR_TG" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="NAME1_TG" SortExpression="NAME1_TG" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Versandadresse" SortExpression="Versandadresse" >
                                                        <HeaderStyle Width="180px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Ersteller" SortExpression="Ersteller" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>                                  
                                                    <telerik:GridBoundColumn DataField="LIZNR" SortExpression="LIZNR" >
                                                        <HeaderStyle Width="110px" />
                                                    </telerik:GridBoundColumn> 
                                                    <telerik:GridBoundColumn DataField="ZZREFERENZ2" SortExpression="ZZREFERENZ2" >
                                                        <HeaderStyle Width="110px" />
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
                        <asp:LinkButton ID="cmdFreigabe" runat="server" CssClass="Tablebutton" Width="78px">» Freigeben</asp:LinkButton>
                        <asp:LinkButton ID="cmdAblehnen" runat="server" CssClass="Tablebutton" Width="78px">» Ablehnen</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
