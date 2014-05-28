<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report01.aspx.cs" Inherits="Leasing.forms.Report01" MasterPageFile="../Master/App.Master" %>

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
                                            onsorting="GridView1_Sorting" onrowcommand="GridView1_RowCommand" >
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                            <Columns>
                                
                                                <asp:TemplateField SortExpression="Leasingvertragsnr" HeaderText="col_Leasingvertragsnr">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingvertragsnr" runat="server" CommandName="Sort" CommandArgument="Leasingvertragsnr">col_Leasingvertragsnr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeasingvertragsnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingvertragsnr") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                                 <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKennzeichen" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                                
                                                 <asp:TemplateField SortExpression="EingangPhysisch" HeaderText="col_EingangPhysisch">
                                                 
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_EingangPhysisch" runat="server" CommandName="Sort" CommandArgument="EingangPhysisch">col_EingangPhysisch</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEingangPhysisch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EingangPhysisch", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                                 <asp:TemplateField SortExpression="Abmeldeauftrag" HeaderText="col_Abmeldeauftrag">
                                                 
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abmeldeauftrag" runat="server" CommandName="Sort" CommandArgument="Abmeldeauftrag">col_Abmeldeauftrag</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAbmeldeauftrag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldeauftrag") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                 <asp:TemplateField SortExpression="AnzahlSchilder" HeaderText="col_AnzahlSchilder">
                                                
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_AnzahlSchilder" runat="server" CommandName="Sort" CommandArgument="AnzahlSchilder">col_AnzahlSchilder</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAnzahlSchilder" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnzahlSchilder") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>    
                                                 <asp:TemplateField SortExpression="AnzahlSchilder" HeaderText="col_FormAnzahlSchilder">
                                                
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_FormAnzahlSchilder" runat="server" CommandName="Sort" CommandArgument="AnzahlSchilder">col_FormAnzahlSchilder</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                    <asp:LinkButton id="lbtnFormAnzahlSchilder" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.AnzahlSchilder")!="2" %>' CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>' Text="Zeigen" CausesValidation="False" CommandName="Schilder">Zeigen</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                              
                                                 <asp:TemplateField SortExpression="Schein" HeaderText="col_KfzSchein">
                                                
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_KfzSchein" runat="server" CommandName="Sort" CommandArgument="Schein">col_KfzSchein</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKfzSchein" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Schein") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>    
                                                 <asp:TemplateField SortExpression="Schein" HeaderText="col_FormKfzSchein">
                                                 
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_FormKfzSchein" runat="server" CommandName="Sort" CommandArgument="Schein">col_FormKfzSchein</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                    <asp:LinkButton id="lbtnFormKfzSchein" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Schein")!= null && DataBinder.Eval(Container, "DataItem.Schein")!= "X" %>' CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>' Text="Zeigen" CausesValidation="False" CommandName="Schein">Zeigen</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>    
                                                 <asp:TemplateField SortExpression="Brief" HeaderText="col_Brief">
                                               
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Brief" runat="server" CommandName="Sort" CommandArgument="Brief">col_Brief</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBrief" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Brief") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer" Visible="false">
                                                
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFahrgestellnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                                                                                                                                                                                                                                                                


                                            </Columns>
                                        </asp:GridView>
                                </div>
                            </div>
                         
                    <div id="dataFooter">
                        <asp:Literal id="Literal1" runat="server"></asp:Literal></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
