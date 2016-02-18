<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FirstLogin.aspx.vb" Inherits="CKG.Services.FirstLogin"MasterPageFile="../MasterPage/Services.Master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JScript/PasswordStrength.js"></script>

    <div>
        <asp:CheckBox ID="cbxLogin_TEST" runat="server" Visible="False"></asp:CheckBox>
        <asp:CheckBox ID="cbxLogin_PROD" runat="server" Visible="False"></asp:CheckBox>
        <asp:Literal ID="litAlert" runat="server"></asp:Literal>
    </div>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;</div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            &nbsp;
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="pagination">
                        <table cellpadding="0" cellspacing="0" style="height: 24px">
                            <tbody>
                                <tr>
                                    <td class="login">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="login">
                        <div id="StandardLogin" style="margin-bottom: 25px; " runat="server">
                            <table cellpadding="0" cellspacing="0" >
                                <tfoot>
                                    <tr>
                                        <td colspan="4" style="height:24px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr  style="font-size: 20px" colspan="4">
                                        <td align="left" nowrap="nowrap" style="font-size: 20px" colspan="4">
                                            Herzlich willkommen auf unserem Internet-Server!
                                        </td>
                                    </tr>                                
                                    <tr id="trPwdExp" class="bold" runat="server" colspan="4">
                                        <td align="left" nowrap="nowrap" colspan="4">
                                            Glückwunsch - die erste Hürde haben Sie bewältigt. Jetzt müssen Sie sich Ihre eigenes Passwort anlegen<br />
                                            Hier erhalten Sie eine Hilfestellung, wie Sie Ihr Passwort korrekt anlegen. <br />
                                            Bitte beachten Sie - DAD Deutscher Auto Dienst GmbH/Christoph Kroschke GmbH wird Sie 
                                            <b>niemals</b> nach Ihrem Passwort fragen!<br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold" align="left" colspan="4">
                                            <br />
                                            Hier erhalten Sie eine Hilfestellung, wie Sie Ihr Passwort korrekt ändern!<br />
                                            <br />
                                            Folgende Regeln gelten für Ihren Zugang:<br />
                                            <br />
                                            <asp:Label ID="lblLength" runat="server" Text="Label"></asp:Label><br />
                                            <asp:Label ID="lblSpecial" runat="server" Text="Label"></asp:Label><br />
                                            <asp:Label ID="lblUpperCase" runat="server" Text="Label"></asp:Label><br />
                                            <asp:Label ID="lblNumeric" runat="server" Text="Label"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                        
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="bold" width="12%">
                                            neues Passwort:
                                        </td>
                                        <td class="input">
                                            <asp:TextBox ID="txtNewPwd" CssClass="LoginTextbox" runat="server" TextMode="Password"
                                                Width="267px" autocomplete="off"></asp:TextBox>
                                        </td>
                                        <td  class="bold" rowspan="4" width="150px">
                                            <div id="spanUCase" style="color: #B70000; ">
                                                Großbuchstaben:</div>
                                            <br />
                                            <div id="spanNumeric" style="color: #B70000">
                                                Zahl:</div>
                                            <br />
                                            <div id="spanSpecial" style="color: #B70000">
                                                Sonderzeichen:</div>
                                            <br />
                                            <div id="spanLength" style="color: #B70000">
                                                Länge:</div>
                                            <br />
                                        </td>
                                        <td class="bold" rowspan="4" width="150px" >
                                            <div id="divUCase" style="color: #B70000; ">
                                                nicht erfüllt</div>
                                            <br />
                                            <div id="divNumeric" style="color: #B70000">
                                                nicht erfüllt</div>
                                            <br />
                                            <div id="divSpecial" style="color: #B70000">
                                                nicht erfüllt</div>
                                            <br />
                                            <div id="divLength" style="color: #B70000">
                                                nicht erfüllt</div>
                                            <br />
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Passwortbestätigung:
                                        </td>
                                        <td class="input">
                                            <asp:TextBox ID="txtNewPwdConfirm" CssClass="LoginTextbox" runat="server" TextMode="Password"
                                                Width="267px" autocomplete="off"></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="login" style="text-align: left;width: 35%">
                                            <asp:LinkButton ID="btnChange" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Speichern" Width="128px"></asp:LinkButton>
                                           
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                        <div id="RequestQuestion" runat="server" style="padding-bottom: 15px">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tfoot>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr>
                                        <td colspan="3" class="paddingTop">
                                            An dieser Stelle können Sie eine Kombination von geheimer Frage&nbsp;und nur Ihnen
                                            bekannter Antwort speichern.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Frage:
                                        </td>
                                        <td class="input">
                                            <asp:DropDownList ID="ddlFrage" runat="server" Width="500px" Font-Names="Verdana, sans-serif"
                                                Font-Size="10px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Antwort:
                                        </td>
                                        <td class="input">
                                            <asp:TextBox ID="txtAnfordernSpeichern" CssClass="LoginTextbox" runat="server" Width="500px" autocomplete="off"></asp:TextBox>
                                        </td>
                                        <td class="lock">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../Images/empty.gif"
                                                Height="16px" Width="1px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="login">
                                            <asp:LinkButton ID="cmdSetzeFrageAntwort" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Speichern" Width="78px"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkShowPassword" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; zurück" Width="78px"></asp:LinkButton>
                                        </td>
                                        <td class="login">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
