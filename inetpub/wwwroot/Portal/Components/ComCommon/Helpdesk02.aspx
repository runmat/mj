<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Helpdesk02.aspx.vb" Inherits="CKG.Components.ComCommon.Helpdesk02" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Helpdesk01</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body bgcolor="#ffffff">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="left">
        <tr>
            <td>
                <table id="Table4" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td>
                            <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                        </td>
                    </tr>
                    <tr>
                        <td class="PageNavigation">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="center" align="middle">
                <table class="HelpDeskTable" id="Table2" cellspacing="1" cellpadding="1" width="500"
                    border="0">
                    <tr>
                        <td valign="center" align="middle">
                            <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0" runat="server">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px; text-align: left; vertical-align: top">
                                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Underline="True">Vorgangsart:</asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButtonList ID="rbVorgang" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="1" Selected="True">Neuanlage</asp:ListItem>
                                            <asp:ListItem Value="2">&#196;nderung</asp:ListItem>
                                            <asp:ListItem Value="3">L&#246;schung</asp:ListItem>
                                            <asp:ListItem Value="4">Sperrung</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trOrganization" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        <asp:Label ID="lblOrganization" runat="server" Font-Bold="True">Organisation:</asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlOrganization" runat="server" AutoPostBack="True"  Width="160px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trGruppe" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        <asp:Label ID="lblGroup" runat="server" Font-Bold="True">Gruppenname:</asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGroups" runat="server" AutoPostBack="True" Width="160px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trBenutzer" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        <asp:Label ID="lblUser" runat="server" Font-Bold="True">Benutzername:</asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtUser" runat="server" Width="160px" Enabled="False"></asp:TextBox>
                                        <asp:DropDownList ID="ddlUsers" runat="server" AutoPostBack="True" Width="160px">
                                        </asp:DropDownList>
                                        <font color="red">&nbsp;*</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trAnrede" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        Anrede:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAnrede" runat="server" AutoPostBack="True" Width="310px"
                                            BackColor="White">
                                            <asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
                                            <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                            <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trName" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        Name:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtName" runat="server" Width="310px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trVorName" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        Vorname:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtVorname" runat="server" Width="310px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trTelefon" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        Telefon:</td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtTelefon" runat="server" Width="310px"></asp:TextBox></td>
                                </tr>
                                <tr id="trEmail" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        Email-Adresse:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtEmail" runat="server" Width="310px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trReferenz" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        Händlernummer:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtReferenz" runat="server" Width="310px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trBemerkung" runat="server">
                                    <td style="width: 150px; text-align: left">
                                        Bemerkung:</td>
                                    <td style="text-align: left">
                                         <asp:TextBox ID="txtBemerk" runat="server" MaxLength="200" Width="310px" 
                                             TextMode="MultiLine" Height="103px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trLegende" runat="server">
                                    <td colspan="2" style="text-align: left">
                                        <font color="red" size="1">* wird durch den DAD vergeben</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:LinkButton ID="btnConfirm" runat="server" CssClass="StandardButton">Abschicken</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Large" Visible="False">Auftrag versendet.</asp:Label>
                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="middle">
                <!--#include File="../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
