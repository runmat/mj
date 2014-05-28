<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Kantine.Login"
    MasterPageFile="Kantine.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div id="Main" runat="server">
        <br />
        <br />
        <center>
            <div class="Tablehead" style="width: 300px;">
                Login
            </div>
            <div class="Rahmen" style="width: 313px;"><asp:Panel DefaultButton="btnLogin" runat="server">            
                <table style="margin-top:5px;">
                    <tr>
                        <td class="Beschriftung" width="100" style="padding-left:15px;">
                            Login:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogin" runat="server" align="left" style="Width:183px; margin-right:15px; margin-left: 15px;" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Beschriftung" width="100" style="padding-left:15px;">
                            Passwort:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPasswort" runat="server" TextMode="Password" align="left"  style="width:183px; margin-right:15px; 
                                margin-left: 15px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                        <asp:Label ID="lblError" runat="server" CssClass="Error" style="margin-right:10px;"></asp:Label>
                            <asp:Button ID="btnLogin" runat="server" CssClass="Button" Text="Login" 
                                style="margin:10px 15px 15px 0;" onclick="btnLogin_Click"/>
                        </td>
                    </tr>
                </table></asp:Panel>
            </div>
        </center>        
    </div>
</asp:Content>