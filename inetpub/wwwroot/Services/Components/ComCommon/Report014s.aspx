<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report014s.aspx.vb" Inherits="CKG.Components.ComCommon.Report014s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
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
                                    <tr id="trGruppe" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                            Gruppe:
                                        </td>
                                        <td class="active" style="width: 100%">
                                            <asp:DropDownList ID="ddlGruppe" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="active" style="width: 100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery" runat="server">
                                        <td class="firstLeft active" colspan="2">
                                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataQueryFooter">
                            &nbsp;
                        </div>
                        <div id="Result" runat="Server">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="ListInfo" runat="server" style="float: left; width: 200px;">
                                <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" Width="100%"></asp:ListBox>
                            </div>
                            <div id="data" runat="server" style="float: right; width: 700px;">
                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                    Width="100%">
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
                                            <ItemStyle HorizontalAlign="Left" Wrap="false"/>
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
                                                <span style="width:185px; white-space: normal;">
                                                    <asp:LinkButton ID="Linkbutton2" runat="server" CommandName="open" Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' Width="185px">
                                                    </asp:LinkButton></td>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Filedate" HeaderText="col_Zeit" HeaderStyle-Width="120px" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
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
                                        <asp:TemplateField SortExpression="Filedate">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Status" runat="server" CommandArgument="Filedate" CommandName="Sort">Status</asp:LinkButton></HeaderTemplate>                                            
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='*NEU*' Visible="false" ForeColor="#009900"
                                                    Font-Bold="True"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderStyle-Wrap="false">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckb_SelectAll" runat="server" ToolTip="Auf dieser Seite alle Dokumente löschen setzen."
                                                    AutoPostBack="True" OnCheckedChanged="ckb_SelectAll_CheckedChanged" />
                                                <asp:Label ID="Label1" runat="server" Text="Löschen" ForeColor="Black"></asp:Label></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="rb_sel" runat="server" GroupName="Auswahl" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div id="DivPlaceholder" runat="server" style="margin-bottom: 31px; margin-top: 10px;">
                                    &nbsp;</div>
                            </div>
                        </div>
                    </div>
                    <div id="dataFooter" style="float: right">
                        <asp:Panel ID="PanelUpload" runat="server">
                            <asp:FileUpload ID="upFile" runat="server" />
                            <asp:LinkButton ID="lbtnUpload" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" OnClick="lbtnUpload_Click">» hochladen</asp:LinkButton>
                            <asp:LinkButton ID="cmdDel" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                Visible="False">» Löschen</asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Wollen Sie die markierten Dateien wirklich löschen?"
                                OnClientCancel="cancelClick" TargetControlID="cmdDel" />
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
