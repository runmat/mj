<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="CKG.Services.Login"
    MasterPageFile="../MasterPage/Design.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #DoubleLogin
        {
            width: 73%;
        }
        #Table4
        {
            width: 549px;
        }
        .style1
        {
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                <div id="innerContentRight" style="width: 78%">
                    <div id="innerContentRightHeading">
                        <h1>
                            Willkommen auf unserem Internet-Server!</h1>
                    </div>
                    <div id="pagination">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="login">
                                        <img src="../Images/loginHeader_blue.jpg" alt="Anmeldung" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="login">
                        <div id="StandardLogin" runat="server" style="margin-bottom: 45px">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td colspan="3" class="paddingTop">
                                            Zum Anmelden geben Sie bitte Ihren Benutzernamen und Ihr Passwort ein.<br />
                                            Sollten Sie nicht über diese Informationen verfügen oder sollte bei der Anmeldung
                                            eine Fehlermeldung erscheinen, wenden Sie sich bitte an Ihren Systemverantwortlichen.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
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
                                            <asp:LinkButton ID="cmdLogin" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Login" Width="78px" TabIndex="3"></asp:LinkButton>
                                            <asp:ImageButton ID="btnEmpty" runat="server" ImageUrl="../Images/empty.gif" Height="1px"
                                                Width="1px" /> 
                                        </td>
                                    </tr>
                                    <tr id="trRequestNewPassword" Visible="false" runat="server">
                                        <td class="bold">
                                            Email-Adresse:
                                        </td>
                                        <td class="input">
                                            <asp:TextBox CssClass="TextBoxNormal" ID="txtConfirmEmail" runat="server" 
                                                TabIndex="4" autocomplete="off">
                                            </asp:TextBox>
                                        </td>
                                        <td class="lock">
                                            <asp:LinkButton ID="cmdRequestNewPassword" runat="server" CssClass="TablebuttonXXXLarge" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Neues Passwort anfordern" Width="175px" TabIndex="5" ></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr id="trSelfadminContact" visible="false" runat="server">
                                        <td class="bold">
                                            &nbsp;
                                        </td>
                                        <td class="help" colspan="2">
                                            <div id="divSelfadminContact" runat="server" style="margin-top: 10px; margin-bottom: 10px">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trHelpCenter" runat="server">
                                        <td class="bold">
                                            &nbsp;
                                        </td>
                                        <td class="help">
                                            <img alt="Hilfe" src="../Images/Fragezeichen_10.jpg" style="width: 11px; height: 11px" />
                                            <asp:LinkButton ID="lbtnHelpCenter" runat="server" Font-Underline="False" TabIndex="6"
                                                ForeColor="#333333">Benötigen Sie Hilfe?</asp:LinkButton>
                                        </td>
                                        <td class="lock">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr  id="trPasswortVergessen"  visible="false" runat="server">
                                        <td class="bold">
                                            &nbsp;</td>
                                        <td class="help">
                                            <img src="../Images/lock.gif" alt="Schloss" />
                                            <asp:LinkButton ID="lnkPasswortVergessen" runat="server" TabIndex="7" 
                                                ForeColor="#333333" >Passwort vergessen?</asp:LinkButton></td>
                                        <td class="lock">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                        </td>
                                        <td class="style1" colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left" valign="top" colspan="2">
                                            <span style="color: #B70000">
                                                <asp:Label ID="MessageLabel" runat="server" Font-Size="12pt"></asp:Label>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="left" valign="top" style="padding: 0px 0px 0px 0px" colspan="3">
                                            <table id="tblHelp" style="border: none" width="100%" align="left" 
                                                visible="false" runat="server" cellpadding="0" cellspacing="0">
                                                                                <tfoot>
                                    <tr>
                                        <td colspan="2" style="background-color: #dfdfdf; height: 22px; width: 100%">
                                           &nbsp;
                                        </td>
                                    </tr>
                                </tfoot><tbody>
                                                <tr>
                                                    <td colspan="2" style="padding-top: 15px">
                                                        <strong>Ihre Kontaktdaten:</strong>
                                                    </td>
                                                </tr>
                                                <tr id="trAnrede" runat="server">
                                                    <td valign="top">
                                                        Anrede:<span style="color: #B70000">*</span>
                                                    </td>
                                                    <td valign="top" style="width: 100%" align="left">
                                                        <asp:DropDownList ID="ddlAnrede" runat="server" Width="150px" BackColor="White">
                                                            <asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
                                                            <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                                            <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trWebUserName" runat="server">
                                                    <td valign="top">
                                                        Benutzername:<asp:Label ID="lblRedStar" runat="server" style="color: #B70000">*</asp:Label></td>
                                                    <td valign="top" style="width: 100%" align="left">
                                                                    <asp:TextBox ID="txtWebUserName" runat="server" Width="150px" BackColor="White" BorderColor="#CCCCCC"
                                                                        BorderStyle="Solid" BorderWidth="1px"></asp:TextBox></td>
                                                </tr>
                                                <tr id="trName" runat="server">
                                                    <td valign="top">
                                                        Name:<span style="color: #B70000">*</span>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:TextBox ID="txtName" runat="server" Width="150px" BackColor="White" BorderColor="#CCCCCC"
                                                            BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trVorName" runat="server">
                                                    <td valign="top">
                                                        Vorname:<span style="color: #B70000">*</span>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:TextBox ID="txtVorname" runat="server" Width="150px" BackColor="White" BorderColor="#CCCCCC"
                                                            BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trFirma" runat="server">
                                                    <td valign="top">
                                                        Firma:<span style="color: #B70000">*</span>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:TextBox ID="txtFirma" runat="server" Width="150px" BackColor="White" BorderColor="#CCCCCC"
                                                            BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trTelefon" runat="server">
                                                    <td valign="top">
                                                        Telefon:<span style="color: #B70000">*</span>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:TextBox ID="txtTelefon" runat="server" Width="150px" BackColor="White" BorderColor="#CCCCCC"
                                                            BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trEmail" runat="server">
                                                    <td valign="top" nowrap="nowrap">
                                                        Email-Adresse:<span style="color: #B70000">*</span>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:TextBox ID="txtEmail" runat="server" Width="150px" BackColor="White" BorderColor="#CCCCCC"
                                                            BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trProblem" visible="false" runat="server">
                                                    <td valign="top" nowrap="nowrap">
                                                        Frage/Problem:<span style="color: #B70000">*</span><br />
                                                        max. 250 Zeichen
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:TextBox ID="txtProblem" runat="server" Width="350px" BackColor="White" BorderColor="#CCCCCC"
                                                            BorderStyle="Solid" BorderWidth="1px" MaxLength="250" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap="nowrap" colspan="2">
                                                        <span style="color: #B70000">* Pflichfeld</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" colspan="2" style="padding-bottom: 15px">
                                                    <table cellpadding="0" cellspacing="0" style="border:none; width:370px">
                                                     <tr>
                                                            <td style="padding:  5px 0px 15px  0px">
                                                               <img src="" alt="" id="imgCatcha1" runat="server" />&nbsp;
                                                            </td>
                                                            <td style="padding: 5px 0px 15px 0px">
                                                               <span style="font-size:20px;height:37px;padding-bottom:15px">+</span>&nbsp;
                                                            </td>
                                                            <td style="padding: 5px 0px 15px 0px">
                                                               <img src="" alt="" id="imgCatcha2" runat="server" />&nbsp;
                                                            </td>   
                                                            <td style="padding: 5px 0px 15px 0px">
                                                                <asp:ImageButton ID="ibtnRefresh" ImageUrl="../images/Kreislauf_01.jpg" runat="server"
                                                                Height="32px" AlternateText="Generieren"  Width="32px" />
                                                            </td> 
                                                            <td style="width:420;padding: 5px 0px 15px 3px">
                                                                Gleichung neu generieren
                                                            </td>                                                       
                                                        </tr>                                                    
                                                    </table>
                                                           
                                                        <div>
                                                            <strong>Bitte geben Sie das Ergebnis der abgebildeten Gleichung ein! </strong>
                                                            <br /><br />
                                                            <asp:TextBox ID="CodeNumberTextBox" runat="server" Width="150px" BackColor="White" BorderColor="#CCCCCC"
                                                            BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                            <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Height="16px"
                                                                Text="&amp;nbsp;&amp;#187; Senden" Width="78px"></asp:LinkButton>
                                                        </div>
                                                        <div>
                                                            <br/>
                                                            <strong style="text-decoration: underline">Achtung:</strong>&nbsp;Die Bearbeitung Ihrer Anfrage kann bis zu 24 Stunden in Anspruch nehmen!
                                                        </div>
                                                    </td>
                                                </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #dfdfdf">
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <HeaderTemplate>
                                        <div style="background-color: #dfdfdf; height: 22px; width: 100%">
                                            &nbsp;
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table class="" border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="padding: 3px 1px 2px 28px">
                                                    <img alt="" src="/Portal/Images/arrow.gif" border="0" />
                                                </td>
                                                <td style="padding: 2px">
                                                    <b><%# Eval("Created").ToString() & " - " & Eval("Title")%></b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px">
                                                </td>
                                                <td style="padding: 2px">
                                                    <%# Eval("Message")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div id="divSicherheitszertifikat" runat="server">
                            </div>
                        </div>
                        <div id="DoubleLogin2" runat="server">
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
                                        <td colspan="3" class="bold">
                                            Bitte verlassen Sie unser Portal immer über den Menüpunkt &quot;Abmelden&quot;.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Wollen Sie mit&nbsp; der&nbsp; Anmeldung fortfahren?
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Fortfahren" Width="78px"></asp:LinkButton>
                                        </td>
                                        <td class="bold">
                                            Ein unter diesem Benutzernamen eventuell aktiver Benutzer wird damit abgemeldet.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="bold">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            Zurück zur Anmeldeseite
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="cmdBack" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Zurück" Width="78px"></asp:LinkButton>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
