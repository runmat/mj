<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report05.aspx.vb" Inherits="AppServicesCarRent.Report05"  MasterPageFile="../MasterPage/App.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
     <%-- version 2--%>
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
                       <div id="paginationQuery" >
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                               <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                       </tr>
                                    </tbody>
                                </table>
                                <asp:Panel ID="divTrenn" runat="server" visible="false" ><div id="PlaceHolderDiv" ></div>   </asp:Panel>
                            </div>
                        <asp:Panel ID="divSelection" runat="server" DefaultButton="btndefault">
                            <div id="TableQuery">
                               <table id="tab1" runat="server" cellpadding="0" cellspacing="0" >
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="2"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                     <tr class="formquery" id="tr_Kennzeichen" runat="server">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_AmtlKennzeichen" runat="server" Width="130px">lbl_AmtlKennzeichen</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtAmtlKennzeichen" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_Fahrgestellnummer" runat="server">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server" Width="130px">lbl_Fahrgestellnummer</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_Reference" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Reference" runat="server">lbl_Reference</asp:Label>
                                        </td>
                                        <td class="active"  style="width: 100%">
                                            <asp:TextBox ID="txtReference" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_NummerZB2" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_NummerZB2" runat="server">lbl_NummerZB2</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtNummerZB2" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>   
                                    <tr class="formquery" >
                                        <td class="firstLeft active" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>                                                                      
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>  
                            </div>
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                                <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                    Text="Button" />                               
                            </div>  
                           
                    </asp:Panel>
                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" Width="1050px"
                                            ID="gvSelectOne" CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True"
                                            AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle Wrap="false" CssClass="ItemStyle" />
                                            <EditRowStyle Wrap="False"></EditRowStyle>
                                            <Columns>
                                                <asp:BoundField Visible="false" HeaderText="EQUNR" DataField="EQUNR" ReadOnly="true" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                    <asp:ImageButton ID="lbWeiter" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'
                                                        ImageUrl="../../../Images/arrowgrey.gif" ToolTip="Auswahl" Height="16px" Width="16px" CommandName="weiter" />                                                            
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                            CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'
                                                            ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen" ItemStyle-Wrap="False">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                            CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'
                                                            ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="TIDNR" HeaderText="col_ZBIINummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBIINummer" runat="server" CommandArgument="ZBIINummer" CommandName="sort">col_ZBIINummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'
                                                            ID="lblZBIINummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Referenznummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Referenznummer" runat="server" CommandArgument="Referenznummer"
                                                            CommandName="sort">col_Referenznummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'
                                                            ID="lblReferenznummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
