<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChangePassword.aspx.vb"
    Inherits="CKG.PortalZLD.ChangePassword" MasterPageFile="../MasterPage/Services.Master" %>


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
                        <div id="StandardLogin" style="margin-bottom: 25px" runat="server">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="4" style="height:24px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr id="trPwdExp" class="bold" runat="server" colspan="4">
                                        <td align="left" nowrap="nowrap" colspan="4">
                                        <b>Ihr Passwort ist abgelaufen. Bitte ändern Sie Ihr Passwort !</b><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold" align="left" colspan="4">
                                            <br />
                                            Hier erhalten Sie eine Hilfestellung, wie Sie Ihr Kennwort korrekt ändern!<br />
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
                                        <td colspan="4" style="height: 15px">
                                        
                                            </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            altes Passwort:
                                        </td>
                                        <td class="input" style="width: 40%">
                                            <asp:TextBox CssClass="LoginTextbox" ID="txtOldPwd" runat="server" TextMode="Password"
                                                Width="267px"></asp:TextBox>
                                        </td>
                                        <td id="tdValidation1" runat="server" width="12%" valign="top" class="bold" rowspan="6">
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
                                        <td  id="tdValidation2" runat="server" valign="top"   class="bold" rowspan="6" style="width: 100%">
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
                                            neues Passwort:
                                        </td>
                                        <td class="input" style="width: 40%">
                                            <asp:TextBox ID="txtNewPwd" CssClass="LoginTextbox" runat="server" TextMode="Password"
                                                Width="267px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Passwortbestätigung:
                                        </td>
                                        <td class="input" style="width: 40%">
                                            <asp:TextBox ID="txtNewPwdConfirm" CssClass="LoginTextbox" runat="server" TextMode="Password"
                                                Width="267px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="login" style="text-align: left;width: 40%">
                                            <asp:LinkButton ID="btnChange" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Speichern" Width="128px"></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton ID="lnkShowQuestion" runat="server" CssClass="TablebuttonLarge" Visible="False"
                                                Height="16px" Width="128px" Text="&amp;nbsp;&amp;#187; geheime Frage"></asp:LinkButton>
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                        <div id="RequestPassword" runat="server" style="padding-bottom: 15px">
                            <table cellpadding="0" cellspacing="0">
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
                                            An dieser Stelle können Sie ein neues Passwort anfordern bzw. Ihren Benutzer 
                                            entsperren.
                                            <br />
                                            Beantworten Sie zunächst die von Ihnen gewählte Frage.
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
                                            <strong>
                                                <asp:Label ID="lblFrage" runat="server"></asp:Label></strong>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Antwort:
                                        </td>
                                        <td class="input">
                                            <asp:TextBox ID="txtAntwortAnforderung" TabIndex="3" runat="server" Width="500px"
                                                MaxLength="150"></asp:TextBox>
                                        </td>
                                        <td class="lock">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="login">
                                            <asp:LinkButton ID="lnkRequest" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Anfordern" Width="78px"></asp:LinkButton>
                                        </td>
                                        <td class="login">
                                            &nbsp;
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
                                            <asp:TextBox ID="txtAnfordernSpeichern" CssClass="LoginTextbox" runat="server" Width="500px"></asp:TextBox>
                                        </td>
                                        <td class="lock">
                                            &nbsp;
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
                                    <asp:HyperLink ID="lnkLogout" TabIndex="5" runat="server"  CssClass="Tablebutton" Visible="False"
                                         NavigateUrl="/Services/Start/Logout.aspx">Erneut anmelden</asp:HyperLink>
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
