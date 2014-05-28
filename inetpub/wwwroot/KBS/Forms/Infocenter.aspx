<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Infocenter.aspx.vb" Inherits="KBS.Infocenter"
    MasterPageFile="~/KBS.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../controls/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                   <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Infocenter"></asp:Label></h1>
                        </div>
                        <div id="TableQuery">
                            <table id="tab1" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                           
                                        </td>
                                    </tr>
                                        </tbody>
                            </table>
                        </div>
                        <div id="dataQueryFooter">
                            &nbsp; <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </div>
                        <div id="Result" runat="Server">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="ListInfo" style="float: left;" >
                                <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" Style="height:300px;"></asp:ListBox>
                            </div>
                            <div id="data" style="float: right; width: 77%">
                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                    <PagerSettings Visible="False" />
                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <EditRowStyle></EditRowStyle>
                                    <Columns>
                                        <asp:TemplateField SortExpression="Filename" HeaderText="col_Dokument">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument="Filename" CommandName="Sort">Dokument</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Height="18px" Visible="False"
                                                    ImageUrl="../../images/iconPDF.gif" ToolTip="PDF"></asp:ImageButton>
                                                <asp:ImageButton ID="lbtExcel" runat="server" CommandName="open" Height="18px" Visible="False"
                                                    ImageUrl="../../Images/iconXLS.gif" ToolTip="Excel"></asp:ImageButton>
                                                <asp:ImageButton ID="lbtWord" runat="server" CommandName="open" Height="18px" ImageUrl="../../Images/iconWORD.gif"
                                                    Visible="False" ToolTip="Word" />
                                                <asp:ImageButton ID="lbtJepg" runat="server" CommandName="open" Height="18px" ImageUrl="../../Images/Fotos.jpg"
                                                    ToolTip="JPG" Visible="False" />
                                                <asp:ImageButton ID="lbtGif" runat="server" CommandName="open" Height="18px" ImageUrl="../../Images/Fotos.jpg"
                                                    ToolTip="GIF" Visible="False" />
                                                <asp:LinkButton ID="Linkbutton2" runat="server" CommandName="open" Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Filedate" HeaderText="col_Zeit">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Zeit" runat="server" CommandArgument="Filedate" CommandName="Sort">Letzte Änderung</asp:LinkButton></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblFileDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Filedate") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServerpfad" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPattern" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Pattern") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Filedate" HeaderText="">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Status" runat="server" CommandArgument="Filedate" CommandName="Sort">Status</asp:LinkButton></HeaderTemplate>
                                            <ItemStyle Width="30%"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='*NEU*' Visible="false" ForeColor="#009900"
                                                    Font-Bold="True"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <div id="DivPlaceholder" runat="server" style="margin-bottom: 31px; margin-top: 10px;">
                                    &nbsp;</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
