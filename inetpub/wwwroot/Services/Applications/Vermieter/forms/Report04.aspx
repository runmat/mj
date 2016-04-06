<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report04.aspx.cs" Inherits="Vermieter.forms.Report04"  MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="/Services/Images/queryArrow.gif"
                                                        Width="17px" onclick="NewSearch_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div> 
                            
                            
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 10px 0px 10px 15px;
                        margin-top: 10px">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>  
                    <asp:Panel ID="Panel1" runat="server">                                             
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" style="vertical-align: top; padding-top: 5px; width: 10%"
                                    nowrap="nowrap">
                                    <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                                </td>
                                <td class="active" nowrap="nowrap">
                                    <span>
                                        <asp:RadioButtonList class="actives" ID="rdbCustomer" runat="server" AutoPostBack="True"
                                            Style="white-space: nowrap">
                                        </asp:RadioButtonList>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» Weiter</asp:LinkButton>
                               <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                Text="Button" />
                        </div>
                    </div>
                    </asp:Panel>
                      <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="/services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                CssClass="GridView"  GridLines="None" PageSize="20" AllowPaging="True" 
                                AllowSorting="True" onsorting="GridView1_Sorting" Width="970px" 
                                onrowdatabound="GridView1_RowDataBound" ShowFooter="True" >                                
                                <PagerSettings Visible="False" />
                                <FooterStyle BorderStyle="Solid" Height="30px" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <FooterStyle CssClass="FooterStyle" />
                                <Columns>
                                    <asp:TemplateField SortExpression="Name" HeaderText="col_Name">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Name" runat="server" CommandName="Sort" CommandArgument="Name">col_Name</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                           <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                              <asp:TemplateField SortExpression="TIDNR" HeaderText="col_TIDNR">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_TIDNR" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_TIDNR</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTIDNR" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                            </asp:Label>
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
                                    
                                    <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_ZZREFERENZ1">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_ZZREFERENZ1" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_ZZREFERENZ1</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblZZREFERENZ1" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField SortExpression="ZZREFERENZ2" HeaderText="col_ZZREFERENZ2">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_ZZREFERENZ2" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ2">col_ZZREFERENZ2</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblZZREFERENZ2" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ2") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField SortExpression="Laufzeit" HeaderText="col_Laufzeit" ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Laufzeit" runat="server" CommandName="Sort" CommandArgument="Laufzeit">col_Laufzeit</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLaufzeit" Text='<%# DataBinder.Eval(Container, "DataItem.Laufzeit") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField SortExpression="Einkaufswert" HeaderText="col_Einkaufswert">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Einkaufswert" runat="server" CommandName="Sort" CommandArgument="Einkaufswert">col_Einkaufswert</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEinkaufswert" Text='<%# DataBinder.Eval(Container, "DataItem.Einkaufswert","{0:c}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>                                      
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Restwert" HeaderText="col_Restwert">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Restwert" runat="server" CommandName="Sort" CommandArgument="Restwert">col_Restwert</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRestwert" Text='<%# DataBinder.Eval(Container, "DataItem.Restwert", "{0:c}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField SortExpression="Tranchenmittelwert" HeaderText="col_Tranchenmittelwert" ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Tranchenmittelwert" runat="server" CommandName="Sort" CommandArgument="Tranchenmittelwert">col_Tranchenmittelwert</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTranchenmittelwert" Text='<%# DataBinder.Eval(Container, "DataItem.Tranchenmittelwert") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="RestwertBerechnet" HeaderText="col_RestwertBerechnet">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_RestwertBerechnet" runat="server" CommandName="Sort" CommandArgument="RestwertBerechnet">col_RestwertBerechnet</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRestwertBerechnet" Text='<%# DataBinder.Eval(Container, "DataItem.RestwertBerechnet", "{0:c}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Bezahlt" HeaderText="col_Bezahlt">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Bezahlt" runat="server" CommandName="Sort" CommandArgument="Bezahlt">col_Bezahlt</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblBezahlt" Text='<%# DataBinder.Eval(Container, "DataItem.Bezahlt") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                           </div>
                    
                        <div id="Div1">
                        </div>
                    </div>
                    
                    <div id="dataFooter">
                    </div>
                    
                </div>
            </div>
        </div>
    </div>

 
</asp:Content>