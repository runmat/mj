<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="CKG.Portal.Login" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            height: 24px;
        }
        .style3
        {
            height: 19px;
        }
    </style>

</head>
<body class="tableLogin">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center">
        <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td height="543" valign="top" align="center">
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" align="left" nowrap colspan="3">
                                &nbsp;Internet Server - <strong>Anmeldung</strong>&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr> 
                        <td valign="top">
                                &nbsp;
                            </td>

                            <td style="vertical-align: top; width:100%; text-align:center">
                                <table width="700px" style="background-color: #ffffff;
                                    border-left: solid 1px #cccccc; border-right: solid 1px #cccccc;
                                    border-bottom: solid 1px #cccccc " align="center" id="StandardLogin" runat="server">
                                    <tr>
                                        <td align="left" height="25">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: 20px; padding-left:15px; " align="left">
                                            Willkommen auf unserem Internet-Server!<br />
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left:15px;" align="left">
                                            Zum Anmelden geben Sie bitte Ihren Benutzernamen und Ihr Passwort ein.<br>
                                            &nbsp;<br />
                                            Sollten Sie nicht über diese Informationen verfügen oder sollte bei der Anmeldung
                                            eine Fehlermeldung erscheinen,<br />
                                            wenden Sie sich bitte an Ihren Systemverantwortlichen.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" rowspan="1" >
                                            &nbsp;
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td style="padding-left:15px;padding-right: 5px">
                                                        Benutzername
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUsername" TabIndex="1" ToolTip="Bitte geben Sie hier Ihren Benutzernamen ein!"
                                                            runat="server" CssClass="LoginInput" Font-Bold="True" Width="150px"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" class="style1">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left:15px;padding-right: 5px">
                                                        Passwort
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:TextBox ID="txtPassword" TabIndex="2" runat="server" CssClass="LoginInput" Width="150px"
                                                            TextMode="Password" ToolTip="Bitte geben Sie hier Passwort ein!"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Button ID="cmdLogin" TabIndex="3" runat="server" Width="100px" Text="Login"
                                                            Font-Bold="True" BackColor="#CCCCCC" BorderWidth="0px" 
                                                            BorderStyle="Outset">
                                                        </asp:Button>&nbsp;
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="/Portal/Images/ssl.gif"></asp:Image>                                                    
                                                    </td>
                                                </tr>
                                                <tr id="trHelpcenter" runat="server">
                                                    <td align="right">
                                                    </td>
                                                    <td nowrap="nowrap" align="left" valign="top" colspan="2">
                                                        <img id="imgHelp" runat="server" alt="Hilfe" src="../Images/Fragezeichen_10.jpg" style="width: 16px; height: 16px" />
                                                        <asp:LinkButton ID="lbtnHelpCenter" runat="server" Font-Underline="False">Benötigen Sie Hilfe?</asp:LinkButton>

                                                    </td>
                                                </tr>
                                                <tr  id="trPasswortVergessen" runat="server"  visible="false">
                                                    <td align="right" class="style2">
                                                    </td>
                                                    <td nowrap="nowrap" align="left" valign="top" colspan="2" class="style2">
                                                        <img id="imgPasswort" runat="server" alt="Passwort" src="../Images/Schluessel01_05.jpg" style="width: 16px; height: 16px" />
                                                        <asp:LinkButton ID="lnkPasswortVergessen"  runat="server" Font-Underline="False">Passwort vergessen?</asp:LinkButton>
                                                    
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="padding-left:15px;" colspan="3" 
                                                        class="style3">
                                                        <span style="color: #B70000">
                                                          <asp:Label ID="lblError" runat="server" Font-Size="10pt" EnableViewState="False"></asp:Label></span>
                                                     </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        &nbsp;
                                                    </td>
                                                    <td nowrap="nowrap" align="left" valign="top" colspan="2">
                                                        <span style="color: #B70000">
                                                            <asp:Label ID="MessageLabel" runat="server" Font-Size="12pt"></asp:Label></span>
                                                          
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        &nbsp;
                                                    </td>
                                                    <td nowrap="nowrap" align="left" valign="top" colspan="2">
                                                        <table id="tblHelp" width="100%" align="left" visible="false" runat="server">
                                                            <tr>
                                                                <td colspan="2">
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
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
                                                                <td valign="top" align="left"> 
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
                                                                    Frage/Problem:<span style="color: #B70000">*</span><br />max. 250 Zeichen
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
                                                                <td valign="top" colspan="2">
                                                    <table cellpadding="0" cellspacing="0" style="border:none; width:370px">
                                                     <tr>
                                                            <td style="padding:  5px 0px 15px  0px">
                                                               <img src="" alt="" id="imgCatcha1" runat="server" />&nbsp;
                                                            </td>
                                                            <td style="padding: 5px 0px 15px 0px">
                                                               <span style="font-size:20px;padding-bottom:15px">+</span>&nbsp;
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
                                                                        <asp:Button ID="cmdSend" TabIndex="3" runat="server" Width="100px" Text="Senden"
                                                                            Font-Bold="True" BackColor="#CCCCCC" BorderWidth="0px" BorderStyle="Outset">
                                                                        </asp:Button>
                                                        </div>
<br />
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left:15px;" align="left" colspan="3" height="25>
                                            <div style="border-right: #cccccc 0px solid; border-top: #cccccc 0px solid; overflow: auto;
                                                border-left: #cccccc 0px solid; border-bottom: #cccccc 0px solid; height: auto">
                                                <asp:Repeater ID="Repeater1" runat="server">
                                                    <HeaderTemplate>
                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="20">
                                                                <img alt="" src="/Portal/Images/arrow.gif" border="0">
                                                            </td>
                                                            <td>
                                                                <b>
                                                                    <%# DataBinder.Eval(Container.DataItem, "titleText") %></b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="20">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <%# DataBinder.Eval(Container.DataItem, "messageText") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </TABLE>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td  style="background-color: #ffffff">
                                <table id="DoubleLogin" style="background-color: #ffffff" width="100%" align="left"
                                    runat="server">
                                    <tr>
                                        <td class="TextLarge" align="left" height="25">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" align="left">
                                            Bitte verlassen Sie unser Portal immer über den Menüpunkt "Abmelden".<br>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" rowspan="1">
                                            <table id="Table4" width="100%" border="0" cellspacing="0" cellpadding="10">
                                                <tr>
                                                    <td nowrap bgcolor="#f5f5f5">
                                                        Wollen Sie mit&nbsp; der&nbsp; Anmeldung fortfahren?
                                                    </td>
                                                    <td bgcolor="#f5f5f5">
                                                        <asp:Button ID="cmdContinue" TabIndex="3" runat="server" Width="150px" Font-Bold="True"
                                                            BorderStyle="Outset" Height="25px" BorderWidth="0px" BackColor="#CCCCCC" Text="Fortfahren">
                                                        </asp:Button>
                                                    </td>
                                                    <td bgcolor="#f5f5f5" valign="center" nowrap width="100%">
                                                        Ein unter diesem Benutzernamen eventuell<br>
                                                        aktiver Benutzer wird damit abgemeldet.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td bgcolor="#f5f5f5">
                                                        Zurück zur Anmeldeseite
                                                    </td>
                                                    <td nowrap="nowrap" bgcolor="#f5f5f5">
                                                        <asp:Button ID="cmdBack" TabIndex="3" runat="server" Width="150px" Font-Bold="True"
                                                            BorderStyle="Outset" Height="25px" BorderWidth="0px" BackColor="#CCCCCC" Text="Zurück">
                                                        </asp:Button>
                                                    </td>
                                                    <td valign="top" nowrap="nowrap" align="left" bgcolor="#f5f5f5">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                    </td>
                                                    <td valign="top" nowrap="nowrap" align="left" colspan="2" rowspan="2">
                                                        &nbsp;
                                                    </td>
                                                    <td valign="top" nowrap="nowrap" align="left">
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td > &nbsp;
                            </td>
                            <td width="50%">&nbsp;</td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td  align="center" valign="top">

                                <script type="text/javascript" src="https://seal.verisign.com/getseal?host_name=sgw.kroschke.de&amp;size=M&amp;use_flash=NO&amp;use_transparent=YES&amp;lang=de"></script>

                            </td>
                            <td width="10" align="center" valign="top">
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <p align="right">
                        <font face="Arial" size="1">©&nbsp;2001&nbsp;-&nbsp;2012&nbsp;DAD&nbsp;Deutscher&nbsp;Auto&nbsp;Dienst&nbsp;GmbH&nbsp;/&nbsp;Christoph
                            Kroschke&nbsp;GmbH</font>
                    </p>
                </td>
            </tr>
        </tbody>
    </table>
    </form>

    <script language="JavaScript">
				<!--        //
        window.document.Form1.txtUsername.focus();
				//-->
    </script>

    <asp:CheckBox ID="cbxLogin_TEST" runat="server" Visible="False"></asp:CheckBox>
    <asp:CheckBox ID="cbxLogin_PROD" runat="server" Visible="False"></asp:CheckBox>
    <asp:Literal ID="litAlert" runat="server"></asp:Literal><br>
</body>
</html>
