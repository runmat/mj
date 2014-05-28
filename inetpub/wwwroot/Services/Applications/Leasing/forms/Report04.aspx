<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report04.aspx.cs" Inherits="Leasing.forms.Report04" MasterPageFile="../Master/App.Master" %>

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
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
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
                                        <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20" 
                                            onsorting="GridView1_Sorting" >
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                            <Columns>
                                
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
 
                                                 <asp:TemplateField SortExpression="Referenznummer" HeaderText="col_Referenznummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Referenznummer" runat="server" CommandName="Sort" CommandArgument="Referenznummer">col_Referenznummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReferenznummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenznummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  

                                                <asp:TemplateField SortExpression="Klaerfalltext" HeaderText="col_Klaerfalltext">
                                                      <HeaderStyle Width="450px"  />
                                                       <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Klaerfalltext" runat="server" CommandName="Sort" CommandArgument="Klaerfalltext">col_Klaerfalltext</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKlaerfalltext" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Klaerfalltext") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                                                                                                                
                                                <asp:TemplateField SortExpression="Belegdatum" HeaderText="col_Belegdatum">
                                                 <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Belegdatum" runat="server" CommandName="Sort" CommandArgument="Belegdatum">col_Belegdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBelegdatum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Belegdatum","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  

                                                                                                                                                                                                                                                                                                                                       


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
</asp:Content>
