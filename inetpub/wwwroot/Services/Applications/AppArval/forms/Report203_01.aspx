<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report203_01.aspx.vb"
    Inherits="AppArval.Report203_01" MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc1" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                                <asp:HyperLink ID="lnkShowCSV" runat="server"></asp:HyperLink>
                                <asp:Label ID="lblDownloadTip" runat="server"></asp:Label>
                        </div>
                        <div id="pagination">
                           <uc1:GridNavigation ID="GridNavigation1" runat="server"></uc1:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" Visible="false" CssClass="TextError" EnableViewState="False"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"><br></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid CssClass="GridView" ID="DataGrid1" runat="server" PageSize="20" Width="100%"
                                            AutoGenerateColumns="false" AllowPaging="true" AllowSorting="True" ShowFooter="False"
                                            GridLines="None">
                                            <PagerStyle Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate" />
                                            <ItemStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" ID="Hyperlink3" Target='<%# "_blank" %>' CssClass="Tablebutton"
                                                            Height="16px" Width="78px" NavigateUrl='<%# "Report203_02.aspx?kunnr=" &amp; DataBinder.Eval(Container.DataItem, "Kundennr") %>'>Details</asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Halter" SortExpression="Halter" HeaderText="Halter">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="HOrt" SortExpression="HOrt" HeaderText="HOrt"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Kundennr" SortExpression="Kundennr" HeaderText="Kundennr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Vollst" SortExpression="Vollst" HeaderText="Vollst.">
                                                </asp:BoundColumn>                                               
                                              </Columns>
                                        </asp:DataGrid>
                                        <asp:Label ID="lblInfo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div id="dataFooter">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
