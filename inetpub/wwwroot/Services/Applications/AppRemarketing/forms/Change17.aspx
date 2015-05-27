<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change17.aspx.cs" Inherits="AppRemarketing.forms.Change17" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
                    function openinfo(url) {
                        fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=800,height=400");
                        fenster.focus();
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading" style="float: none">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="paginationQuery" style="float: none">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="active">
                                    Neue Abfrage
                                </td>
                                <td align="right">
                                    <div id="queryImage">
                                        <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                            ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearch_Click" />
                                        <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                            Visible="false" OnClick="NewSearchUp_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="lbCreate">
                        <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Upload:
                                    </td>
                                    <td class="firstLeft active">
                                        <input ID="upFileFin" runat="server" name="upFileFin" size="49" type="file" />
                                        <a href="javascript:openinfo('InfoUebergabeTUEV.htm');">
                                        <img alt="Struktur Uploaddatei" border="0" height="16px" 
                                            src="/Services/Images/info.gif" title="Struktur Uploaddatei" 
                                            width="16px" /></a> &nbsp; * max. 
                                        900 Datensätze
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
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClientClick="Show_BusyBox1();" OnClick="lbCreate_Click">» Laden </asp:LinkButton>
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
                                            OnItemDataBound="rgGrid1_ItemDataBound" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FAHRGNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn DataField="FAHRGNR" SortExpression="FAHRGNR" Groupable="false">
                                                        <HeaderStyle Width="175px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# Eval("FAHRGNR") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" Visible="False" 
                                                                Text='<%# Eval("FAHRGNR") %>' 
                                                                BorderColor="Red" Width="170px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="KENNZ" SortExpression="KENNZ" Groupable="false">
                                                        <HeaderStyle Width="105px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# Eval("KENNZ") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtKennzeichen" runat="server" Visible="False" 
                                                                Text='<%# Eval("KENNZ") %>' 
                                                                BorderColor="Red" Width="100px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="DAT_UEB_HC_TUEVSUED" SortExpression="DAT_UEB_HC_TUEVSUED" Groupable="false">
                                                        <HeaderStyle Width="85px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblUebergabedatum" Text='<%# Eval("DAT_UEB_HC_TUEVSUED") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtUebergabedatum" runat="server" Visible="False" 
                                                                Text='<%# Eval("DAT_UEB_HC_TUEVSUED", "{0:dd.MM.yyyy}") %>' 
                                                                BorderColor="Red" Width="80px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="HCORT" SortExpression="HCORT" Groupable="False">
                                                        <HeaderStyle Width="90px" />
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblHCOrt" Text='<%# Eval("HCORT") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtHCOrt" runat="server" Visible="False" 
                                                                Text='<%# Eval("HCORT") %>' 
                                                                BorderColor="Red" Width="75px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="KM_STAND_UEB" SortExpression="KM_STAND_UEB" Groupable="false">
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblKMSTAND" Text='<%# Eval("KM_STAND_UEB") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtKM" runat="server" Visible="False" 
                                                                Text='<%# Eval("KM_STAND_UEB") %>' 
                                                                BorderColor="Red" Width="75px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="RET" SortExpression="RET" Visible="false" UniqueName="Bemerkung" >
                                                        <HeaderStyle Width="150px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ID" SortExpression="ID" Visible="false" UniqueName="ID" >
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div style="float:right;width:100%;text-align:right">
                            <asp:LinkButton ID="lbSend" runat="server" CssClass="Tablebutton"
                                Width="78px" onclick="lbSend_Click" Visible ="false" style="margin-top:10px;margin-bottom:5px">» Senden</asp:LinkButton>
                        </div>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
