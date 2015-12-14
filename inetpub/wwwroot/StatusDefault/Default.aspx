<%@ Page Title="Startseite" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="StatusDienste.Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>
       <asp:Label ID="Label1" runat="server" Text="ServicesMvc"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblStatusSQL" runat="server" Text="Label"></asp:Label>
         <br/> 
        <asp:Label ID="lblErrorSQL" runat="server" ></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblStatusSAP" runat="server"></asp:Label>
        <br/>
        <asp:Label ID="lblErrorSAP" runat="server" ></asp:Label>
    </p>
</asp:Content>
