<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change03s"
     MasterPageFile="../../../MasterPage/Services.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                       </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                               <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="2"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                ></asp:Label>
                                                
                                            <asp:Label ID="lblNoData" runat="server" ></asp:Label>                                                
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                                        </td>
                                        <td class="active" style="width:100%">
                                              <span> 
                                              <asp:RadioButton ID="rbGesperrt" GroupName="Vorgang" runat="server" 
                                                    Text="Gesperrte Vorgänge" AutoPostBack="True" />
                                              </span> 
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                            &nbsp;</td>
                                        <td class="active">
                                            <span>
                                                <asp:RadioButton ID="rbAbgelehnt" runat="server" GroupName="Vorgang" 
                                                Text="Abgelehnte Vorgänge" AutoPostBack="True" />
                                            </span>
                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" nowrap="nowrap" colspan="2">
                                            &nbsp </td>

                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="2"  style="width:100%">
                                             &nbsp;</td>
                                     </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
   
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="GridView" width="1200px">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="true" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                <asp:TemplateField HeaderText="BELNR" SortExpression="BELNR" Visible="false" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBELNR" runat="server" Text='<%# Bind("BELNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Auswahl">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSperre" runat="server" />
                                                        <asp:Label runat="server" Text="in Aut."
                                                            ID="lblInAut" Visible="false"> </asp:Label>                                                                                                                                                                            
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="Ablehnungsgrund" HeaderText="col_Ablehnungsgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ablehnungsgrund" runat="server" CommandArgument="Ablehnungsgrund" CommandName="sort">col_Ablehnungsgrund</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAblehnungsgrund" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status")<>"A" %>' 
                                                                     text='<%# DataBinder.Eval(Container, "DataItem.Ablehnungsgrund") %>' runat="server" MaxLength="253" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Ablehnender" HeaderText="col_Ablehnender">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ablehnender" runat="server" CommandArgument="Ablehnender" CommandName="sort">col_Ablehnender</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ablehnender") %>'
                                                            ID="lblAblehnender" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kunnr" HeaderText="col_Kunnr">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kunnr" runat="server" CommandArgument="Kunnr"
                                                            CommandName="sort">col_Kunnr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kunnr") %>'
                                                            ID="lblKunnr" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Treunehmer" HeaderText="col_Treunehmer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Treunehmer" runat="server" CommandArgument="Treunehmer"
                                                            CommandName="sort">col_Treunehmer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Treunehmer") %>'
                                                            ID="lblTreunehmer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                
                                                <asp:TemplateField SortExpression="KunnrTG" HeaderText="col_KunnrTG">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_KunnrTG" runat="server" CommandArgument="KunnrTG"
                                                            CommandName="sort">col_KunnrTG</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KunnrTG") %>'
                                                            ID="lblKunnrTG" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField SortExpression="Treugeber" HeaderText="col_Treugeber">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Treugeber" runat="server" CommandArgument="Treugeber"
                                                            CommandName="sort">col_Treugeber</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Treugeber") %>'
                                                            ID="lblTreugeber" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                                                                
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer" HeaderStyle-Width="130px">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                            CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                            CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                            ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandadresse" runat="server" CommandArgument="Versandadresse" CommandName="sort">col_Versandadresse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'
                                                            ID="lblVersandadresse" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Ersteller" HeaderText="col_Ersteller">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ersteller" runat="server" CommandArgument="Ersteller" CommandName="sort">col_Ersteller</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ersteller") %>'
                                                            ID="lblErsteller" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                       <HeaderTemplate>
                                                           <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                       </HeaderTemplate>
                                                       <ItemTemplate>
                                                           <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                           </asp:Label>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>   
                                                   <asp:TemplateField SortExpression="Referenznummer" HeaderText="col_Referenznummer">
                                                       <HeaderTemplate>
                                                           <asp:LinkButton ID="col_Referenznummer" runat="server" CommandName="Sort" CommandArgument="Referenznummer">col_Referenznummer</asp:LinkButton>
                                                       </HeaderTemplate>
                                                       <ItemTemplate>
                                                           <asp:Label runat="server" ID="lblReferenznummer" Text='<%# DataBinder.Eval(Container, "DataItem.Referenznummer") %>'>
                                                           </asp:Label>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>                                                                                                                          
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="cmdFreigabe" runat="server" CssClass="Tablebutton" Width="78px">» Freigeben</asp:LinkButton>
                                <asp:LinkButton ID="cmdAblehnen" runat="server" CssClass="Tablebutton" Width="78px">» Ablehnen</asp:LinkButton>
                            </div>                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
