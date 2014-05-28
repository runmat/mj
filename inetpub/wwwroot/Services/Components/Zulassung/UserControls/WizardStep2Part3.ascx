<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep2Part3.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep2Part3" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="~/PageElements/GridNavigation.ascx" %>

<div id="Result" runat="Server">
    <div id="data" style="float: none;">
        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
            CssClass="GridView" GridLines="None" PageSize="4" AllowPaging="True" AllowSorting="True" OnRowEditing="GridView1_RowEditing"
             OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound">
            <PagerSettings Visible="False" />
            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
            <AlternatingRowStyle CssClass="GridTableAlternate" />
            <RowStyle CssClass="ItemStyle" />
            <EditRowStyle CssClass="EditItemStyle" />
            <EditRowStyle></EditRowStyle>
            <Columns>
                <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                    <HeaderStyle Width="125px" />
                    <ItemStyle Width="125px" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Wunschkennz1" HeaderText="col_Wunschkennz1">
                    <HeaderStyle Width="125px" />
                    <ItemStyle Width="125px" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Wunschkennz1" runat="server" CommandName="Sort" CommandArgument="Wunschkennz1">col_Wunschkennz1</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz1") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate><asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz1") %>' style="width: 120px;"></asp:TextBox></EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Wunschkennz2" HeaderText="col_Wunschkennz2">
                    <HeaderStyle Width="125px" />
                    <ItemStyle Width="125px" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Wunschkennz2" runat="server" CommandName="Sort" CommandArgument="Wunschkennz2">col_Wunschkennz2</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz2") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate><asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz2") %>' style="width: 120px;"></asp:TextBox></EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Wunschkennz3" HeaderText="col_Wunschkennz3">
                    <HeaderStyle Width="125px" />
                    <ItemStyle Width="125px" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Wunschkennz3" runat="server" CommandName="Sort" CommandArgument="Wunschkennz3">col_Wunschkennz3</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz3") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate><asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz3") %>' style="width: 120px;"></asp:TextBox></EditItemTemplate>
                </asp:TemplateField>          
                <asp:TemplateField SortExpression="ResNr" HeaderText="col_ResNr">
                    <HeaderStyle Width="125px" />
                    <ItemStyle Width="125px" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_ResNr" runat="server" CommandName="Sort" CommandArgument="ResNr">col_ResNr</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResNr") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate><asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResNr") %>' style="width: 120px;"></asp:TextBox></EditItemTemplate>
                </asp:TemplateField>   
                <asp:TemplateField SortExpression="ResName" HeaderText="col_ResName">
                    <HeaderStyle Width="145px" />
                    <ItemStyle Width="145px" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_ResName" runat="server" CommandName="Sort" CommandArgument="ResName">col_ResName</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResName") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate><asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResName") %>' style="width: 140px;"></asp:TextBox></EditItemTemplate>
                </asp:TemplateField>     
                <asp:CommandField ShowEditButton="True" ButtonType="Image" EditImageUrl="~/Images/Zulassung/edit_icon.png" CancelImageUrl="~/Images/Zulassung/cancel_icon.png" UpdateImageUrl="~/Images/Zulassung/save_icon.png">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:CommandField>                                                                                                                                      
            </Columns>
            <EmptyDataTemplate>
                <asp:Label ID="Label8" runat="server" ForeColor="Red">Es wurde kein Fahrzeug ausgewählt.</asp:Label>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div id="pagination">
        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
    </div>
</div>