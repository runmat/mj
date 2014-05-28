<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Infocenter.aspx.cs"
    Inherits="AppZulassungsdienst.forms.Infocenter" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI"  TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Infocenter"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" width="100%">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="Result" runat="Server" style="width: 100%">
                        <div style="display: none">
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </div>
                        <div id="data" runat="server">
                            <telerik:RadGrid ID="rgDokumente" runat="server" PageSize="10" AllowPaging="true" AllowSorting="true" 
                                AutoGenerateColumns="False" GridLines="None" OnNeedDataSource="rgDokumente_NeedDataSource" 
                                OnItemDataBound="rgDokumente_ItemDataBound" OnItemCommand="rgDokumente_ItemCommand" Skin="Default">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <ItemStyle CssClass="ItemStyle" />
                                <AlternatingItemStyle CssClass="ItemStyle" />
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
                                    <FooterStyle BackColor="#FFB27F" HorizontalAlign="Right" Wrap="false" />
                                    <Columns>
                                        <telerik:GridBoundColumn SortExpression="DocumentId" DataField="DocumentId" visible="false" />
                                        <telerik:GridTemplateColumn SortExpression="FileName" HeaderText="Dokument" HeaderButtonType="TextButton">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbtPDF" runat="server" CommandName="showDocument" Height="18px" ImageUrl="/PortalZLD/Images/iconPDF.gif" 
                                                    ToolTip="PDF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() == "pdf" %>' />
                                                <asp:ImageButton ID="lbtExcel" runat="server" CommandName="showDocument" Height="18px" ImageUrl="/PortalZLD/Images/iconXLS.gif" 
                                                    ToolTip="Excel" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString().StartsWith("xls") %>' />
                                                <asp:ImageButton ID="lbtWord" runat="server" CommandName="showDocument" Height="18px" ImageUrl="/PortalZLD/Images/Word_Logo.jpg"
                                                    ToolTip="Word" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString().StartsWith("doc") %>' />
                                                <asp:ImageButton ID="lbtJepg" runat="server" CommandName="showDocument" Height="18px" ImageUrl="/PortalZLD/Images/Fotos.jpg"
                                                    ToolTip="JPG" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString().StartsWith("jp") %>' />
                                                <asp:ImageButton ID="lbtGif" runat="server" CommandName="showDocument" Height="18px" ImageUrl="/PortalZLD/Images/Fotos.jpg"
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
                    <div id="dataFooter" class="dataQueryFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
