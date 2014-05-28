<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Infocenter.aspx.cs" Inherits="AutohausPortal.forms.Infocenter" 
MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>
<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI"  TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="Scriptmanager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" LoadScriptsBeforeUI="true" ScriptMode="Release">
    </asp:ScriptManager>
    <div style="margin-left: 65px">                    
        <asp:Label ID="lblError" runat="server" Style="color: #B54D4D; font-weight:bold" Text=""></asp:Label>
        <asp:Label ID="lblMessage" runat="server" Style="color: #269700; font-weight:bold" Text=""></asp:Label>                          
    </div>
    <div class="inhaltsseite">
        <div class="inhaltsseite_top">
            &nbsp;
        </div>
        <div class="innerwrap">
            <h1>
                Formulare
            </h1>
            <div>
                &nbsp;
            </div>
            <div id="Result" runat="Server">
                <div style="display: none">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                </div>
                <div id="data" runat="server">
                    <div>
                        <telerik:RadGrid ID="rgDokumente" runat="server" PageSize="10" AllowPaging="true" AllowSorting="true" 
                            AutoGenerateColumns="False" GridLines="None" OnNeedDataSource="rgDokumente_NeedDataSource" 
                            OnItemDataBound="rgDokumente_ItemDataBound" OnItemCommand="rgDokumente_ItemCommand">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true">
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="DocTypeName" SortOrder="Ascending" />
                                    <telerik:GridSortExpression FieldName="LastEdited" SortOrder="Descending" />
                                </SortExpressions>
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                                <HeaderStyle ForeColor="White" />
                                <Columns>
                                    <telerik:GridBoundColumn SortExpression="DocumentId" DataField="DocumentId" visible="false" />
                                    <telerik:GridTemplateColumn SortExpression="FileName" HeaderText="Dokument" HeaderButtonType="TextButton">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lbtPDF" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../images/iconPDF.gif" 
                                                ToolTip="PDF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() == "pdf" %>' />
                                            <asp:ImageButton ID="lbtExcel" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../Images/iconXLS.gif" 
                                                ToolTip="Excel" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString().StartsWith("xls") %>' />
                                            <asp:ImageButton ID="lbtWord" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../Images/Word_Logo.jpg"
                                                ToolTip="Word" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString().StartsWith("doc") %>' />
                                            <asp:ImageButton ID="lbtJepg" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../Images/Fotos.jpg"
                                                ToolTip="JPG" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString().StartsWith("jp") %>' />
                                            <asp:ImageButton ID="lbtGif" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../Images/Fotos.jpg"
                                                ToolTip="GIF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() == "gif" %>' />                                         
                                            <span>
                                                <asp:LinkButton ID="lbtDateiOeffnen" runat="server" CommandName="showDocument" Text='<%# Eval("FileName") %>' >
                                                </asp:LinkButton>
                                            </span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="FileType" DataField="FileType" visible="false" />
                                    <telerik:GridBoundColumn SortExpression="DocTypeId" DataField="docTypeId" Visible="false" />
                                    <telerik:GridBoundColumn SortExpression="DocTypeName" HeaderText="Art" AllowSorting="false" 
                                        DataField="DocTypeName" HeaderStyle-Width="200px" />
                                    <telerik:GridBoundColumn SortExpression="LastEdited" HeaderText="Letzte Änderung" HeaderButtonType="TextButton" 
                                        DataField="LastEdited" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderStyle-Width="150px">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>
        <div class="inhaltsseite_bot">
            &nbsp;
        </div>
    </div>

</asp:Content>