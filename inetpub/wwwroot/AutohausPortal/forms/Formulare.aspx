<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Formulare.aspx.cs" Inherits="AutohausPortal.forms.Formulare" MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                <div  style="margin-left: 65px">
                    
                        <asp:Label ID="lblError" runat="server" Style="color: #B54D4D; font-weight:bold" Text=""></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" Style="color: #269700; font-weight:bold" Text=""></asp:Label>      
                    
                </div>
    <div class="inhaltsseite">

        <div class="inhaltsseite_top">
            &nbsp;
            </div>

            <div class="innerwrap">
            <h1>
                Formulare</h1>

            <ul>
                <asp:Repeater ID="Repeater1" runat="server" 
                    onitemcommand="Repeater1_ItemCommand">
                    <ItemTemplate>
                        <li>
                            <asp:LinkButton ID="lnkFormulare" runat="server" CommandName="show" Text='<%# DataBinder.Eval(Container.DataItem, "Filename").ToString() %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Serverpfad").ToString() %>'></asp:LinkButton> </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="inhaltsseite_bot">
            &nbsp;</div>
    </div>

</asp:Content>