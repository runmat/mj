<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report01.aspx.cs" Inherits="appCheckServices.forms.Report01"
    MasterPageFile="../Master/AppMaster.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
 
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblNoData" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px" OnClick="btnConfirm_Click">» Abfrage starten</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="DivPlaceholder" runat="server" style="height: 550px;">
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader"
                                                    bodyCSS="tableBody" CssClass="GridView" AllowSorting="True" AutoGenerateColumns="False"
                                                    OnSelectedIndexChanged="DataGrid1_SelectedIndexChanged" OnItemCommand="DataGrid1_ItemCommand" GridLines="None">
                                                    <AlternatingItemStyle BackColor="#DEE1E0"></AlternatingItemStyle>
                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateColumn SortExpression="Filename" HeaderText="col_Dokument">
                                                            <ItemStyle Width="40%" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument="Filename" CommandName="Sort">Dokument</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                    ImageUrl="../../../images/iconPDF.gif" ToolTip="PDF"></asp:ImageButton>
                                                                <asp:ImageButton ID="lbtExcel" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                    ImageUrl="../../../Images/iconXLS.gif" ToolTip="Excel"></asp:ImageButton>
                                                                <asp:ImageButton ID="lbtWord" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Word_Logo.jpg"
                                                                    Visible="False" ToolTip="Word" />
                                                                <asp:ImageButton ID="lbtJepg" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/jpg-file.png"
                                                                    ToolTip="JPG" Visible="False" />
                                                                <asp:ImageButton ID="lbtGif" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Gif_Logo.gif"
                                                                    ToolTip="GIF" Visible="False" />
                                                                <asp:ImageButton ID="lbtZip" runat="server" CommandName="open" Height="18px" ImageUrl="../../../Images/Zip.gif"
                                                                    ToolTip="ZIP" Visible="False" />
                                                                <asp:LinkButton ID="Linkbutton2" runat="server" Width="229px" Height="18px" CommandName="open"
                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' ForeColor="Blue"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn SortExpression="Filedate" HeaderText="col_Zeit">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zeit" runat="server" CommandArgument="Filedate" CommandName="Sort">Letzte Änderung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Filedate") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="Pattern" SortExpression="Pattern"></asp:BoundColumn>
                                                    </Columns>
                                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                        PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                       
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>
