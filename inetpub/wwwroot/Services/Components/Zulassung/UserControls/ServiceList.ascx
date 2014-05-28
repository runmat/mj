<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceList.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.ServiceList" %>

<div style="max-height: 500px; overflow: auto; overflow-x: hidden;">
     <asp:GridView ID="grvDL" runat="server" AutoGenerateColumns="False" 
                    EnableModelValidation="True" ShowHeader="False" Width="100%" CssClass="SmallListTable" BorderWidth="0">
        <Columns>
            <asp:TemplateField ItemStyle-Width="20px">
                <ItemTemplate>
                    <asp:Label ID="lblDL" runat="server" Text='<%# Bind("ASNUM") %>' Visible="false"></asp:Label>
                    <asp:CheckBox ID="cbxDL" runat="server" ></asp:CheckBox>
                </ItemTemplate>
                <ItemStyle Width="20px" />
            </asp:TemplateField>
            <asp:BoundField DataField="ASKTX" />
            <asp:BoundField DataField="TBTWR" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="AbstandRechts" ItemStyle-Width="60px" DataFormatString="{0:C}" >
            <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:TemplateField  ItemStyle-Width="20px">
                <ItemTemplate>
                    <asp:Image ID="ibtDL" runat="server" ImageUrl="/Services/Images/info.gif" Visible='<%# !string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.Description") as string) %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Description") %>' />
                </ItemTemplate>
                <ItemStyle Width="20px" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <asp:Label ID="Label1" runat="server" ForeColor="Red">Es stehen keine Dienstleistungen zur Auswahl.</asp:Label>
        </EmptyDataTemplate>
    </asp:GridView>
</div>