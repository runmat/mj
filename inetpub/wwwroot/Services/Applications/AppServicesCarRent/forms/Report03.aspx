<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03.aspx.vb" Inherits="AppServicesCarRent.Report03"  MasterPageFile="../MasterPage/App.Master" %>


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
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" nowrap="nowrap" colspan="2">
                                             Klicken Sie auf "Erstellen" um den Bericht zu starten.</td>
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
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Erstellen</asp:LinkButton>
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
                                                    PageSize="20" CssClass="GridView" style="width:auto">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Hersteller" HeaderText="col_Hersteller">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Hersteller" runat="server" CommandArgument="Hersteller"
                                                                    CommandName="sort">col_Hersteller</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'
                                                                    ID="lblHersteller" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Herstellername" HeaderText="col_Herstellername">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Herstellername" runat="server" CommandArgument="Herstellername" CommandName="sort">col_Herstellername</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Herstellername") %>'
                                                                    ID="lblHerstellername" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Handelsname" HeaderText="col_Handelsname">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Handelsname" runat="server" CommandArgument="Handelsname"
                                                                    CommandName="sort">col_Handelsname</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Handelsname") %>'
                                                                    ID="lblHandelsname" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                         
                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                                    CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                    ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Unitnummer" HeaderText="col_Unitnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Unitnummer" runat="server" CommandArgument="Unitnummer"
                                                                    CommandName="sort">col_Unitnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Unitnummer") %>'
                                                                    ID="lblUnitnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Eingangsdatum" HeaderText="col_Eingangsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandArgument="Eingangsdatum"
                                                                    CommandName="sort">col_Eingangsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:d}") %>'
                                                                    ID="lblEingangsdatum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      <asp:TemplateField SortExpression="Referenz2" HeaderText="col_Referenz2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz2" runat="server" CommandArgument="Referenz2"
                                                                    CommandName="sort">col_Referenz2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz2") %>'
                                                                    ID="lblReferenz2" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Briefabsender" HeaderText="col_Briefabsender">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Briefabsender" runat="server" CommandArgument="Briefabsender"
                                                                    CommandName="sort">col_Briefabsender</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefabsender") %>'
                                                                    ID="lblBriefabsender" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
