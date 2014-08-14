<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AktuellePflegeUebersicht.aspx.vb" Inherits="Admin.AktuellePflegeUebersicht"
    MasterPageFile="MasterPage/Admin.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Aktuelles"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                     <tr>
                                        <td>
                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" Font-Names="Verdana,sans-serif"
                                                    Height="20px" Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server" Width="160px"></asp:Label>
                                            </td>
                                        </tr>                                      
                                        <tr class="formquery">
                                             <td class="firstLeft active" style="width:100%" colspan="2">
                                                 &nbsp;</td>
                                        </tr>
                                        <tr style="background-color: #dfdfdf; height: 22px">
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>                                    
                                </table>
                               <div id="QueryFooter" runat="server">
                                    <div id="dataQueryFooter">
                                        <asp:LinkButton class="Tablebutton" ID="btnSuche" runat="server"
                                            Text="&amp;nbsp;&amp;#187; Suchen" CssClass="Tablebutton" Height="16px"
                                            Width="78px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>
                                        <asp:LinkButton class="Tablebutton" ID="btnNew" runat="server"
                                            Text="&amp;nbsp;&amp;#187; Neu" CssClass="Tablebutton" Height="16px"
                                            Width="78px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>                                            
                                    </div>
                                </div>
                            </div>
                           
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">                                    
                                </div>
                                <div id="pagination">
                                    <uc2:gridnavigation id="GridNavigation1" runat="server"></uc2:gridnavigation>
                                </div>
                                <div id="data">
                                    <asp:GridView ID="grvUebersicht" AutoGenerateColumns="False" BackColor="White" runat="server"
                                        CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                        DataKeyNames="ID">
                                        <PagerSettings Visible="False"  />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <EditRowStyle></EditRowStyle>
                                        <Columns>
                                            <asp:TemplateField visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_ID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:BoundField DataField="BeitragName" SortExpression="BeitragName" HeaderText="Beitrag" />
                                            <asp:BoundField DataField="Customername" SortExpression="Customername" HeaderText="Kunde" />
                                            <asp:BoundField DataField="GueltigBis" SortExpression="GueltigBis" HeaderText="Gültig bis" DataFormatString="{0:dd.MM.yyyy}" />
                                            <asp:CheckBoxField DataField="Aktiv" HeaderText="Aktiv" SortExpression="Aktiv"/>
                                            <asp:TemplateField HeaderText="Bearbeiten">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lbnBearbeiten" runat="server" 
                                                     ImageUrl="/PortalZLD/Images/Edit_01.gif" Height="16px" Width="16px"
                                                        Text="Bearbeiten" CommandName="Select"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>' ></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Löschen">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lbnLoeschen" runat="server" 
                                                     ImageUrl="/PortalZLD/Images/Papierkorb_01.gif" Height="16px" Width="16px"
                                                        Text="Löschen" CommandName="Delete"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>' ></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                        </Columns>
                                    </asp:GridView>                                    
                                </div>
                            </div>                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
