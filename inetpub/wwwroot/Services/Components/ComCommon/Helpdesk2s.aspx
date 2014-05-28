<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Helpdesk2s.aspx.vb" Inherits="CKG.Components.ComCommon.Helpdesk02s"
 MasterPageFile="../../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                            </h1>
                        </div>
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td colspan="2" class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="tr_Filialen" class="formquery">
                                        <td class="firstLeft active" style="vertical-align:top" >
                                           Vorgangsart:
                                          </td>
                                        <td class="active" style="width: 100%">
                                            <span>
                                            <asp:RadioButtonList ID="rbVorgang" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="1" Selected="True">Neuanlage</asp:ListItem>
                                            <asp:ListItem Value="2">&#196;nderung</asp:ListItem>
                                            <asp:ListItem Value="3">L&#246;schung</asp:ListItem>
                                            <asp:ListItem Value="4">Sperrung</asp:ListItem>
                                            
                                        </asp:RadioButtonList></span></td>
                                    </tr>
                                    <tr runat="server" id="trOrganization" class="formquery">
                                        <td class="firstLeft active">
                                            Organisation:
                                        </td>
                                        <td class="active">
                                        <asp:TextBox ID="txtOrganization" runat="server"  CssClass="TextBoxNormal"></asp:TextBox>
                                        <asp:DropDownList
                                            ID="ddlOrganization" runat="server" AutoPostBack="True" Width="150px" BackColor="White">
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trGruppe" class="formquery">
                                        <td class="firstLeft active">
                                            Gruppe:
                                        </td>
                                        <td class="active">
                                        <asp:TextBox ID="txtGroup" runat="server"  CssClass="TextBoxNormal"></asp:TextBox>
                                        <asp:DropDownList
                                            ID="ddlGroups" runat="server" AutoPostBack="True" Width="150px" BackColor="White">
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr  id="trBenutzer" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                            Benutzername:
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtUser" runat="server"  CssClass="TextBoxNormal" Enabled="False"></asp:TextBox>
                                            <asp:DropDownList
                                                ID="ddlUsers" runat="server" AutoPostBack="True" Width="150px" BackColor="White">
                                            </asp:DropDownList>
                                            <font color="red">&nbsp;*</font>
                                        </td>
                                    </tr>
                                    <tr  id="trAnrede" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                           Anrede:
                                        </td>
                                        <td class="active">
                                        <asp:DropDownList ID="ddlAnrede" runat="server" >
                                            <asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
                                            <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                            <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr   id="trName" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                           Name:</td>
                                        <td class="active">
                                           <asp:TextBox ID="txtName" runat="server" Width="150px" BackColor="White"></asp:TextBox></td>
                                    </tr>
                                    <tr  id="trVorName" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                             Vorname:</td>
                                        <td class="active">
                                            <asp:TextBox ID="txtVorname" runat="server"  CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr  id="trTelefon" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                             Telefon:</td>
                                        <td class="active">
                                            <asp:TextBox ID="txtTelefon" runat="server"  CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr  id="trEmail" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                              Email-Adresse:
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtEmail" runat="server"  CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                    </tr> 
                                    <tr  id="trReferenz" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                              Händlernummer:
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtReferenz" runat="server"  CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                    </tr> 
                                    <tr id="trBemerkung" runat="server" class="formquery">
                                        <td class="firstLeft active"   >
                                             Bemerkung:
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtBemerk" TextMode="MultiLine" Height="103px" runat="server"  CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                    </tr>
                                <tr id="trLegende" runat="server" class="formquery">
                                    <td class="firstLeft active" colspan="2">
                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Large" Visible="False">Auftrag versendet.</asp:Label>
                                        <font color="red" size="1">* wird durch den DAD vergeben</font>
                                    </td>
                                </tr>   
                                <tr>
                                    <td align="left" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>                                                                                                                                                                                                                                                     
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="btnConfirm" Text="Erstellen" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton" >» Senden</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
