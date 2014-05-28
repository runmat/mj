<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminTool.aspx.vb" Inherits="KBS.AdminTool"  MasterPageFile="~/KBS.Master" %>

<%@ Register TagPrefix="uc1" TagName="GridNavigation" Src="../controls/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="AdminTool"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                            </h1>
                        </div>
                        <div id="TableQuery">
                            <div id="statistics" style="margin-top: 0px;">
                                <table id="tblAnzeige" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2" width="100%">
                                            &nbsp;</td>
                                        
                                    </tr>
                                    <tr>
                                        <td colspan="2" width="100%">
                                            &nbsp;</td>
                                        
                                    </tr>                                    
                                </table>
                            </div>
                        </div>
                       
                        <asp:UpdatePanel runat="server" ID="upWareneingang">
                            <ContentTemplate>
                                <div id="pagination">
                                    <uc1:GridNavigation ID="GridNavigation1" runat="server"></uc1:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblError" runat="server" Visible="true" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                <asp:Label ID="lblNoData" runat="server" Visible="true" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView CssClass="GridView" ID="GridView1" runat="server" PageSize="50" Width="100%"
                                                    AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowFooter="False"
                                                    GridLines="None">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:BoundField Visible="false" DataField="IP" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="IP" SortExpression="IP"
                                                            DataField="IP" />
                                                        <asp:BoundField HeaderText="LGORT" DataField="LGORT" SortExpression="LGORT" />
                                                        <asp:BoundField HeaderText="WERKS" SortExpression="WERKS"
                                                            DataField="WERKS" />
                                                        <asp:BoundField HeaderText="Firma" SortExpression="Firma" DataField="Firma" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lbShow" runat="server" Width="16" Height="16" ImageUrl="~/Images/arrowgrey.gif"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IP") %>' 
                                                                CommandName="Show" onclick="lbShow_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                            </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
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
    </div>
</asp:Content>
