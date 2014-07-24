<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressSearch.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.AddressSearch" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>

<div runat="server" id="Container" style="max-height: 450px; overflow: auto; overflow-x: hidden;">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    <asp:Panel ID="Panel1" DefaultButton="btnSearch" runat="server">
     <table cellpadding="0" cellspacing="0">
        
            <tr>
                <td width="70">
                    <asp:Label ID="lbl_Name" AssociatedControlID="txtName" runat="server">lbl_Name</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="long"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Plz" AssociatedControlID="txtPlz" runat="server">lbl_Plz</asp:Label>, <asp:Label ID="lbl_Ort" runat="server">lbl_Ort</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPlz" runat="server" CssClass="short"></asp:TextBox>
                    <asp:TextBox ID="txtOrt" runat="server" CssClass="middle"></asp:TextBox>
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Suchen" CssClass="blueButton search" onclick="Button1_Click" />
                </td>
            </tr>
    </table>
    </asp:Panel>

    <asp:GridView ID="grvDL" runat="server" AutoGenerateColumns="False" 
                    EnableModelValidation="True" ShowHeader="True" Width="100%" 
            CssClass="SearchResultTable" BorderWidth="0" 
            onselectedindexchanging="grvDL_SelectedIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="col_Name1">
                <HeaderTemplate>
                    <asp:Label ID="col_Name1" runat="server">col_Name1</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="col_Name2">
                <HeaderTemplate>
                    <asp:Label ID="col_Name2" runat="server">col_Name2</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME2") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="col_PLZ">
                <HeaderTemplate>
                    <asp:Label ID="col_PLZ" runat="server">col_PLZ</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PSTLZ") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="col_Strasse">
                <HeaderTemplate>
                    <asp:Label ID="col_Strasse" runat="server">col_Strasse</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STRAS") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="col_Ort">
                <HeaderTemplate>
                    <asp:Label ID="col_Ort" runat="server">col_Ort</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ORT01") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="col_Description">
                <HeaderTemplate>
                    <asp:Label ID="col_Description" runat="server">col_Description</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.POS_Text") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField CommandName="Select" ImageUrl="~/Images/Zulassung/icon_select.gif" ButtonType="Image" Text="Auswählen" />
        </Columns>
        <EmptyDataTemplate>
            <asp:Label runat="server" ForeColor="Red">Es konnte keine Adresse gefunden werden.</asp:Label>
        </EmptyDataTemplate>
    </asp:GridView>

    </ContentTemplate>
    </asp:UpdatePanel>
</div>

    <custom:ModalOverlay runat="server" id="SearchOverlay" ParentContainer="Container">
        <Triggers>
            <custom:ModalOverlayTrigger ControlID="btnSearch" />
        </Triggers>
        <ContentTemplate>
            <div style="background-color: #fff; width: 300px; padding: 15px; text-align: center; border: 3px solid #335393;">
                <img id="Img1" src="~/Images/Zulassung/loading.gif" align="middle" style="border-width:0px;" runat="server" />
                <br /><label style="font-size:14px;font-weight:bold;">Bitte warten...</label>
                <br /><label style="font-size:10px;font-weight:bold;">Ihr Suchanfrage wird bearbeitet.</label>
            </div>
        </ContentTemplate>
    </custom:ModalOverlay>