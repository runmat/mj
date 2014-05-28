<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07_2.aspx.vb" Inherits="AppF2.Change07_2"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkKreditlimit" runat="server" NavigateUrl="Change07.aspx">Suche</asp:HyperLink>
                    <a class="active">| Senden</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                        <div id="TableQuery">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table id="tab1" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="ShowScript" runat="server" class="formquery">
                                                <td class="active" width="100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="Result" runat="Server">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data" style="overflow: auto">
                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                    <PagerSettings Visible="False" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Versanddatum" SortExpression="Versanddatum" HeaderText="Versanddatum"
                                            DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                        <asp:BoundField Visible="False" DataField="Versandstatus" SortExpression="Versandstatus"
                                            HeaderText="Versandstatus"></asp:BoundField>
                                        <asp:TemplateField SortExpression="Versandadresse" HeaderText="Versandadresse">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVersandresse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>                                        
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Distrikt" SortExpression="Distrikt" HeaderText="Distrikt">
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Memo 1" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="#FFFFFF">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFleet20" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fleet20") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="txtFleet20" MaxLength="27" runat="server" Width="100px" Font-Size="8pt"
                                                    Font-Names="Verdana,sans-serif;">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Memo 2" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="#FFFFFF">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFleet21" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fleet21") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="txtFleet21" runat="server" MaxLength="27" Width="100px" Font-Size="8pt"
                                                    Font-Names="Verdana,sans-serif;">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <p align="center">
                                                    <asp:ImageButton ID="lbStatus" runat="server" ToolTip="Statusänderung übernehmen"
                                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' CommandName="Edit"
                                                        ImageUrl="../../../Images/Save.gif" Height="16px" Width="16px" />
                                                </p>
                                                <asp:Label ID="lblStatus" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'
                                                    Font-Bold="True"></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div id="dataFooter">
                            &nbsp;</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
