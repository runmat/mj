<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report07.aspx.cs" Inherits="Leasing.forms.Report07" MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;border: none">
                                <tr class="formquery">
                                
                                    <td class="firstLeft active" style="width: 100%; height: 19px;">
                                        <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </div>
                            <div id="Result" visible="false" runat="Server">
                                <div class="ExcelDiv">
                                    <div align="right">
                                        <img src="/services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lbCreateExcel" runat="server" 
                                            Text="Excel herunterladen" ForeColor="White" 
                                            onclick="lbCreateExcel_Click" ></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>                            
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20" OnSorting="GridView1_Sorting">
                                                    <HeaderStyle CssClass="GridTableHead" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="ZBRIEFEINGANG" HeaderText="col_Briefeingang">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Briefeingang" runat="server" CommandName="Sort" CommandArgument="ZBRIEFEINGANG">col_Briefeingang</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("ZBRIEFEINGANG","{0:d}") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZLVNR" HeaderText="col_LvNr">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_LvNr" runat="server" CommandName="Sort" CommandArgument="ZZLVNR">col_LvNr</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("ZZLVNR") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NAME1_ZH" HeaderText="col_Halter" >
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Halter" runat="server" CommandName="Sort" CommandArgument="NAME1_ZH">col_Halter</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("NAME1_ZH") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NAME1_HAENDLER" HeaderText="col_Haendler" >
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Haendler" runat="server" CommandName="Sort" CommandArgument="NAME1_HAENDLER">col_Haendler</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("NAME1_HAENDLER") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KUNDENNAME" HeaderText="col_Kundenname" >
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="KUNDENNAME">col_Kundenname</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("KUNDENNAME") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NUTZER" HeaderText="col_Nutzer" >
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Nutzer" runat="server" CommandName="Sort" CommandArgument="NUTZER">col_Nutzer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("NUTZER") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="WUNSCH_LIEF_DAT" HeaderText="col_Wunschlieferdatum" >
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Wunschlieferdatum" runat="server" CommandName="Sort" CommandArgument="WUNSCH_LIEF_DAT">col_Wunschlieferdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("WUNSCH_LIEF_DAT","{0:d}") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="STATUS" HeaderText="col_Status" >
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("STATUS") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                         
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
