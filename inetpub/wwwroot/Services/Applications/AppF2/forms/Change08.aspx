<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change08.aspx.vb" Inherits="AppF2.Change08"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                               Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                             <td nowrap="nowrap" style="width:100%" class="firstLeft active" >
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" >
                                            <td class="firstLeft active" >
                                            </td>
                                        </tr>                                        
                                        <tr class="formquery">
                                            <td class="firstLeft">
                                                <span>
                                                    <asp:RadioButton ID="rb_ZBI" runat="server" Text="rb_ZBI" Checked="True" GroupName="Auswahl" /></span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft">
                                                <span>
                                                    <asp:RadioButton ID="rb_Kennz" runat="server" Text="rb_Kennz" GroupName="Auswahl" /></span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft">
                                                <span>
                                                    <asp:RadioButton ID="rb_Halter" runat="server" Text="rb_Halter" GroupName="Auswahl" /></span>
                                            </td>
                                        </tr>                                        
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>  
                               </div>   
                                 <div id="dataQueryFooter">   
                                               <asp:LinkButton ID="lbCreate" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px">» Abfrage starten</asp:LinkButton>
                                 </div>                                                         
                            
      
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lbCreateExcel" runat="server" Text="Excel herunterladen" ForeColor="White"></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavi" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvAusgabe" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20" >
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                       <asp:TemplateField HeaderText="Erledigt" SortExpression="Erledigt">
                                                            <HeaderStyle Width="50px"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Erledigt" runat="server" CommandArgument="Erledigt" CommandName="sort">col_Erledigt</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cboErledigt" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Erledigt") %>'/>
                                                            </ItemTemplate>
                                                            
                                                        </asp:TemplateField>                                                    
                                                        <asp:TemplateField SortExpression="Aenderungsdatum" HeaderText="col_Aenderungsdatum">
                                                           <HeaderStyle Width="95px"/>                                                            
                                                           <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Aenderungsdatum" runat="server" CommandArgument="Aenderungsdatum"
                                                                    CommandName="sort">col_Aenderungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Aenderungsdatum","{0:d}") %>' 
                                                                    ID="lblAenderungsdatum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Distrikt" HeaderText="col_Distrikt" HeaderStyle-HorizontalAlign="Center">
                                                         <HeaderStyle Width="115px" HorizontalAlign="Left"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Distrikt" runat="server" CommandArgument="Distrikt" 
                                                                    CommandName="sort" Height="16px" style="padding-top:4px">col_Distrikt</asp:LinkButton>&nbsp;
                                                                <asp:DropDownList ID="ddlDistrikte" runat="server" AutoPostBack="True" 
                                                                    Width="60" Height="16px" 
                                                                    onselectedindexchanged="ddlDistrikte_SelectedIndexChanged" Font-Size="10px">
                                                                </asp:DropDownList>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Distrikt") %>'
                                                                    ID="lblDistrikt" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                         <HeaderStyle Width="100px"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandArgument="Vertragsnummer"
                                                                    CommandName="sort">col_Vertragsnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'
                                                                    ID="lblVertragsnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KennzeichenAlt" HeaderText="col_KennzeichenAlt">
                                                             <HeaderStyle Width="100px"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KennzeichenAlt" runat="server" CommandArgument="KennzeichenAlt"
                                                                    CommandName="sort">col_KennzeichenAlt</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KennzeichenAlt") %>'
                                                                    ID="lblKennzeichenAlt" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KennzeichenNeu" HeaderText="col_KennzeichenNeu">
                                                             <HeaderStyle Width="100px"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KennzeichenNeu" runat="server" CommandArgument="KennzeichenNeu"
                                                                    CommandName="sort">col_KennzeichenNeu</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KennzeichenNeu") %>'
                                                                    ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="BriefnummerAlt" HeaderText="col_BriefnummerAlt">
                                                             <HeaderStyle Width="100px"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_BriefnummerAlt" runat="server" CommandArgument="BriefnummerAlt"
                                                                    CommandName="sort">col_BriefnummerAlt</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BriefnummerAlt") %>'
                                                                    ID="lblBriefnummerAlt" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="BriefnummerNeu" HeaderText="col_BriefnummerNeu">
                                                         <HeaderStyle Width="100px"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_BriefnummerNeu" runat="server" CommandArgument="BriefnummerNeu"
                                                                    CommandName="sort">col_BriefnummerNeu</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BriefnummerNeu") %>'
                                                                    ID="lblBriefnummerNeu" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="HalterAdresse" HeaderText="col_HalterAdresse">
                                                         <HeaderStyle Width="150px"/>   
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_HalterAdresse" runat="server" CommandArgument="Fahrgestellnummer"
                                                                    CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.HalterAdresse") %>'
                                                                    ID="lblHalterAdresse" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
 
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dataFooter">
                                    <asp:LinkButton ID="lbSpeichern" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px" >» Sichern</asp:LinkButton>
                               </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbCreateExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>
