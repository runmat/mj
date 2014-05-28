<%@ Page Language="C#" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASPNET Example ASPX .NetFramework 3.5 eID Connector</title>
    <style type="text/css">
        #form1
        {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h1 align="center">Fraunhofer FOKUS (BuDr Server)</h1><br /><br/>
        <br />
    
   
    <asp:Button ID="ButtonCreateSamlReq" runat="server" 
        onclick="ButtonCreateSamlReq_Click" Text="Create Saml Request" />
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Auto Kennzeichen"></asp:Label>
    <br />
    <asp:TextBox ID="TextBoxCarId" runat="server">B-DC 12345</asp:TextBox>
    </form>
</body>
</html>
