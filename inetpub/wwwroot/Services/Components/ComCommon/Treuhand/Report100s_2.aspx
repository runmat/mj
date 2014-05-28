<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report100s_2.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Report100s_2"
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
                                              <asp:RadioButton ID="rbKeineZBII" GroupName="Vorgang" runat="server" 
                                                    Text="Sperr-/Freigabesatz mehrfach vorhanden" AutoPostBack="True" />
                                              </span> 
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                            &nbsp;</td>
                                        <td class="active">
                                            <span>
                                                <asp:RadioButton ID="rbohneDokumente" runat="server" GroupName="Vorgang" 
                                                Text="Daten ohne Dokumente" AutoPostBack="True" />
                                            </span>
                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" nowrap="nowrap">
                                             &nbsp;</td>
                                         <td class="active">
                                         <span> 
                                         <asp:RadioButton ID="rbAndererTG" runat="server" GroupName="Vorgang" 
                                                Text="Durch anderen Treugeber gesperrt" AutoPostBack="True" />
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
                                                    PageSize="20" CssClass="GridView">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                               <Columns>

                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderStyle Width="120px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                    <HeaderStyle Width="70px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZBII" runat="server" CommandName="Sort" CommandArgument="NummerZBII">col_NummerZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblTIDNR" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderStyle Width="70px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                    <HeaderStyle Width="85px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField SortExpression="Vorgang" HeaderText="col_Vorgang">
                                                   <HeaderStyle Width="60px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vorgang" runat="server" CommandName="Sort" CommandArgument="Fehlercode">col_Vorgang</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVorgang1" Text="Freigabe" Visible='<%# DataBinder.Eval(Container, "DataItem.Vorgang") = "F" %>'>
                                                        </asp:Label>
                                                        <asp:Label runat="server" ID="lblVorgang2" Text="Sperrung" Visible='<%# DataBinder.Eval(Container, "DataItem.Vorgang") = "S" %>'>
                                                        </asp:Label>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                               <asp:TemplateField SortExpression="Fehlercode" HeaderText="col_Fehlercode">
                                                   <HeaderStyle Width="60px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fehlercode" runat="server" CommandName="Sort" CommandArgument="Fehlercode">col_Fehlercode</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERROR" Text='<%# DataBinder.Eval(Container, "DataItem.Fehlercode") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fehlertext" HeaderText="col_Fehlertext">
                                                    <HeaderStyle Width="170px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fehlertext" runat="server" CommandName="Sort" CommandArgument="ERROR_TXT">col_Fehlertext</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERROR_TXT" Text='<%# DataBinder.Eval(Container, "DataItem.Fehlertext") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="bearbeitet" HeaderText="col_Bearbeitet">
                                                    <HeaderStyle Width="75px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Bearbeitet" runat="server" CommandName="Sort" CommandArgument="bearbeitet">col_Bearbeitet</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblbearbeitet" Text='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="bearbeitetam" HeaderText="col_Bearbeitetam">
                                                    <HeaderStyle Width="70px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Bearbeitetam" runat="server" CommandName="Sort" CommandArgument="bearbeitetam">col_Bearbeitetam</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblbearbeitetam" Text='<%# DataBinder.Eval(Container, "DataItem.bearbeitetam", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Erledigt" HeaderText="col_Erledigt">
                                                    <HeaderStyle Width="70px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Erledigt" runat="server" CommandName="Sort" CommandArgument="bearbeitetam">col_Erledigt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblErledigt" Text='<%# DataBinder.Eval(Container, "DataItem.Erledigt", "{0:d}") %>'>
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
                                &nbsp;
                            </div>                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
