<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report06.aspx.cs" Inherits="Leasing.forms.Report06"  MasterPageFile="../Master/App.Master" %>

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
                                                        
                                                        <asp:TemplateField SortExpression="CARPC" HeaderText="col_EinCarportliste" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_EinCarportliste" runat="server" CommandName="Sort" CommandArgument="CARPC">col_EinCarportliste</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl22" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CARPC") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl23" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="SCHILD" HeaderText="col_AnzSchild" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_AnzSchild" runat="server" CommandName="Sort" CommandArgument="SCHILD">col_AnzSchild</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl24" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SCHILD") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="SCHEIN" HeaderText="col_Schein" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Schein" runat="server" CommandName="Sort" CommandArgument="SCHEIN">col_Schein</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl25" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SCHEIN") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ABMAUF" HeaderText="col_Abmeldeauftrag" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Abmeldeauftrag" runat="server" CommandName="Sort" CommandArgument="ABMAUF">col_Abmeldeauftrag</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl26" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ABMAUF") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="BRIEF" HeaderText="col_Brief" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Brief" runat="server" CommandName="Sort" CommandArgument="BRIEF">col_Brief</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl27" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BRIEF") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="CARPP" HeaderText="col_EingangPhysisch" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_EingangPhysisch" runat="server" CommandName="Sort" CommandArgument="CARPP">col_EingangPhysisch</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl28" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CARPP") %>'>
                                                                </asp:Label>
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
