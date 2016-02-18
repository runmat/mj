<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IframeLogin.aspx.vb" Inherits="CKG.Services.IframeLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=iso-8859-1" />
      <link href="/Services/Styles/default.css" media="screen, projection" type="text/css" rel="stylesheet" />
   <%--<link href="/Services/Customize/default.css" media="screen, projection" type="text/css" rel="stylesheet" />
    --%>
</head>
<body>
    <form id="form1" runat="server">
                        <div id="StandardLogin" runat="server" style="margin-bottom: 45px">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="bold">
                                            Benutzername:
                                        </td>
                                        <td class="input">
                                            <asp:TextBox CssClass="LoginTextbox" ID="txtUsername" runat="server" 
                                                TabIndex="1" autocomplete="off"></asp:TextBox>
                                        </td>
                                        <td  class="lock" >     


                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Passwort:
                                        </td>
                                        <td class="input">
                                            <asp:TextBox ID="txtPassword" CssClass="LoginTextbox" runat="server" 
                                                TextMode="Password" TabIndex="2" autocomplete="off"></asp:TextBox>
                                        </td>
                                        <td class="lock">
                                            <asp:ImageButton ID="btnEmpty" runat="server" ImageUrl="../Images/empty.gif" Height="1px"
                                                Width="1px" />                                        
                                            <asp:LinkButton ID="cmdLogin" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Login" Width="78px" TabIndex="3"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold" colspan="3">
                                            <asp:Label ID="lblError" runat="server"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
    </form>
</body>
</html>
