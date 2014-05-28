<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change01" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width:140px">
                            <table id="Table2" cellspacing="0" cellpadding="0" style="width:140px"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" >
                                        &nbsp;
                                    </td>
                                </tr>
                           
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="cmdFreigabe" Visible="false" runat="server" CssClass="StandardButton"> &#149;&nbsp;Freigeben</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="cmdAblehnen" Visible="false" runat="server" CssClass="StandardButton"> &#149;&nbsp;Ablehnen</asp:LinkButton>
                                    </td>
                                </tr>    
                            </table>  
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td  valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td  colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge">
                                                    <asp:Label ID="lblStatus" runat="server"  Text="Status:"></asp:Label>
                                                    </td>
                                                <td style="width:100%">
                                                    <asp:RadioButton ID="rbGesperrt" GroupName="Vorgang" runat="server" 
                                                        Text="Gesperrte Vorgänge" AutoPostBack="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge">
                                                    &nbsp;</td>
                                                <td style="width:100%">
                                                    <asp:RadioButton ID="rbAbgelehnt" runat="server" GroupName="Vorgang" 
                                                         Text="Abgelehnte Vorgänge" AutoPostBack="True" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td  colspan="2">
                                                    &nbsp;
                                               </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelExtraLarge" colspan="2">
                                                       <asp:Label ID="lblError"
                                                        runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trExcel" visible="false">
                                                <td class="LabelExtraLarge">
                                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:LinkButton>
                                                </td>
                                                <td align="right">
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                     </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" AllowSorting="false" BackColor="White"
                                            runat="server" Width="100%" ID="GridView1" AllowPaging="True" 
                                            PageSize="20" Visible="false">
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="BELNR" SortExpression="BELNR" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBELNR" runat="server" Text='<%# Bind("BELNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kunnr" HeaderText="col_Kunnr" Visible="false">
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
                                                
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
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
                                                <asp:TemplateField SortExpression="Ablehnungsgrund" HeaderText="col_Ablehnungsgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ablehnungsgrund" runat="server" CommandArgument="Ablehnungsgrund" CommandName="sort">col_Ablehnungsgrund</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                    <ItemTemplate>
<%--                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ablehnungsgrund") %>'
                                                            ID="lblAblehnungsgrund" Visible="true"> </asp:Label>--%>
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
                                                           
                                                                                                
                                                <asp:TemplateField HeaderText="Auswahl">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSperre" runat="server" />     
                                                        <asp:Label runat="server" Text="in Aut."
                                                            ID="lblInAut" Visible="false"> </asp:Label>                                                                                                                                                                             
                                                    </ItemTemplate>

                                                <HeaderStyle Width="50px"></HeaderStyle>
                                                </asp:TemplateField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
