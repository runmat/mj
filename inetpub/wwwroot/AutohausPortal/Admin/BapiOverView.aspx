<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BapiOverView.aspx.vb"
    Inherits="Admin.BapiOverView" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <%--<Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>--%>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="/PortalZLD/Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td nowrap="nowrap" class="firstLeft active">
                                                </td>
                                                <td class="firstLeft active" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Bapi Name:
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtFilter" runat="server" CssClass="InputTextbox" Text="**"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="/PortalZLD/images/empty.gif"
                                                        Width="1px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">Suchen »</asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server">

                                    <div class="divBapiOverView">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td nowrap="nowrap" valign="bottom">
                                                    <asp:ImageButton runat="server" ID="imgbDBVisible" Height="16px" Width="16px" ImageUrl="/PortalZLD/Images/minus.gif" />
                                                    <span lang="de">&nbsp;DB-Overview&nbsp;&nbsp;&nbsp;</span>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    <asp:Label ID="lblDBInfo" Font-Bold="True" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="active" align="left" colspan="2" valign="bottom">
                                                    <asp:Label ID="lblDBNoData" Font-Bold="True" runat="server"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblDBError" Font-Bold="True" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:Panel ID="PanelDB" runat="server">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            <img src="/PortalZLD/Images/iconXLS.gif" alt="Excel herunterladen" />
                                            <span class="ExcelSpan">
                                                <asp:LinkButton ID="imgbDBExcel" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <div id="pagination2">
                                        <uc2:GridNavigation ID="GridNavigation2" runat="server"></uc2:GridNavigation>
                                    
                                    <table cellspacing="0" class="TableGrid" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="DBDG" Width="100%" runat="server" AutoGenerateColumns="False" CellPadding="1"
                                                    CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="true"
                                                    AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:BoundField Visible="false" HeaderText="BapiName" DataField="BapiName" ReadOnly="true" />
                                                        <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_BapiName">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_BapiName" runat="server" CommandArgument="Vertragsnummer"
                                                                    CommandName="sort">col_BapiName</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                                    ID="lblBapiName" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="SourceModule" HeaderText="col_SourceModule">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_SourceModule" runat="server" CommandArgument="SourceModule"
                                                                    CommandName="sort">col_SourceModule</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SourceModule") %>'
                                                                    ID="lblSourceModule" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="BapiDate" HeaderText="col_BapiDate">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_BapiDate" runat="server" CommandArgument="BapiDate" CommandName="sort">col_BapiDate</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BapiDate","{0:d}") %>'
                                                                    ID="lblBapiDate" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_inserted">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_inserted" runat="server" CommandName="Sort" CommandArgument="inserted">col_inserted</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.inserted") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_updated">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_updated" runat="server" CommandName="Sort" CommandArgument="inserted">col_updated</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.updated") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Details" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Details" runat="server">col_Details</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDetails" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                                    runat="server" Width="16px" CommandName="Details" Height="16px">
																		<img  src="/PortalZLD/Images/Lupe_01.gif" Width="16px" Height="16px" alt="Details" border="0"/></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Delete" runat="server">col_Delete</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.BapiName") %>'
                                                                    CommandName="Delete" Height="10px">
																		                <img src="/PortalZLD/Images/Papierkorb_01.gif"  width="16px" height="16px"  alt="löschen" border="0" /></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                </asp:Panel>

                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div> 
            </div>
        </div>
    </div>
</asp:Content>
