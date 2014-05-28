<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report13s.aspx.vb" Inherits="CKG.Components.ComCommon.Report13s"   
 MasterPageFile="../../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div id="site">
            <div id="content">
                  <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                      Text="zurück"></asp:LinkButton>
                  </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                      
                            <table id="tab1" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>                                      &nbsp;</td>
                                    </tr>
                                   </tbody>
                            </table>
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                            <ContentTemplate>
                                <div id="Result" runat="Server">

                                    <div class="ExcelDiv">     
                                    <div style="float:left;padding-left:15px"><asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                                    </div>                                                      
                                
                                        <div align="right" class="rightPadding">
                                        <img src="../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                    </div>
                                    <div id="pagination">   
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                        </uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server"
                                            ID="GridView1" CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True"
                                            AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle  CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>

                                                <asp:TemplateField SortExpression="Leasingvertragsnummer" HeaderText="col_Leasingvertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingvertragsnummer" runat="server" CommandName="Sort" CommandArgument="Leasingvertragsnummer">col_Leasingvertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLiznr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingvertragsnummer") %>'>
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
                                                 <asp:TemplateField SortExpression="Versanddatum" HeaderText="col_Versanddatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versanddatum" runat="server" CommandName="Sort" CommandArgument="Versanddatum">col_Versanddatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVersanddatum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>        
                                                 <asp:TemplateField SortExpression="Name" HeaderText="col_Name">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Name" runat="server" CommandName="Sort" CommandArgument="Name">col_Name</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="Strasse" HeaderText="col_Strasse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Strasse" runat="server" CommandName="Sort" CommandArgument="Strasse">col_Strasse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStrasse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Strasse") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="Nr" HeaderText="col_Nr">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Nr" runat="server" CommandName="Sort" CommandArgument="Name">col_Nr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Nr") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="PLZ" HeaderText="col_PLZ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_PLZ" runat="server" CommandName="Sort" CommandArgument="PLZ">col_PLZ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPLZ" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PLZ") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="Ort" HeaderText="col_Ort">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ort" runat="server" CommandName="Sort" CommandArgument="Ort">col_Ort</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ort") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                                 <asp:TemplateField SortExpression="Mahnstufe" HeaderText="col_Mahnstufe">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Mahnstufe" runat="server" CommandName="Sort" CommandArgument="Mahnstufe">col_Mahnstufe</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMahnstufe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahnstufe") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                                 <asp:TemplateField SortExpression="Mahndatum" HeaderText="col_Mahndatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Mahndatum" runat="server" CommandName="Sort" CommandArgument="Mahndatum">col_Mahndatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMahndatum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahndatum", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                                                                                                                                                                                                                                                                                    
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                           &nbsp;
                            
                            </div>
                    </div>
                </div>
            </div>
        </div>
    
</asp:Content>