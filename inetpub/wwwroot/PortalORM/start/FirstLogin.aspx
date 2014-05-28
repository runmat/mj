<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FirstLogin.aspx.vb" Inherits="CKG.Portal.FirstLogin" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
    
<script type="text/javascript" src="../PageElements/JavaScript/PasswordStrength.js"></script> 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright" />
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .validateLeft
        {
            border-top: solid 1px #000000;
            border-bottom: solid 1px #000000;
            border-left: solid 1px #000000;
            padding-left: 5px;
        }
        .validateRight
        {
            border-top: solid 1px #000000;
            border-bottom: solid 1px #000000;
            border-right: solid 1px #000000;
        }
    </style>
</head>
<body  class="tableLogin">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center">
        <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" width="150" colspan="2">
                                Passwort ändern
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="120">
                                <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                    border="0">
                                    <tr>
                                        <td class="TextHeader" width="150">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table id="tblChange" style="background-color: #ffffff;
                                    border-left: solid 1px #cccccc; border-right: solid 1px #cccccc;
                                    border-bottom: solid 1px #cccccc " width="780" cellspacing="0" cellpadding="0" align="left" runat="server">

                                    <tr>
                                        <td align="left" height="25">
                                        </td>
                                    </tr>
                                                                        <tr>
                                        <td  class="TextLarge" style="font-size: 20px;padding-left:15px;" align="left">
                                            Herzlich willkommen auf unserem Internet-Server!<br />
                                            &nbsp;
                                        </td>
                                    </tr>                                    
                                    <tr class="TextLarge"  id="trPwdExp" runat="server">
                                        <td style="padding-left:15px;" align="left">
                                            Glückwunsch - die erste Hürde haben Sie bewältigt. Jetzt müssen Sie sich Ihr eigenes 
                                            Passwort anlegen<br />
                                            Hier erhalten Sie eine Hilfestellung, wie Sie Ihr Passwort korrekt anlegen. <br />
                                            Bitte beachten Sie:<br />
                                            DAD Deutscher Auto Dienst GmbH/Christoph Kroschke GmbH wird Sie    <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left:15px;" class="TextNormal" align="left"><br />
                                            Folgende Regeln gelten für die Einrichtung Ihres neuen Zugangs:<br /><br />
                                            <asp:Label ID="lblLength" runat="server" Text="Label"></asp:Label><br />
                                            <asp:Label ID="lblSpecial" runat="server" Text="Label"></asp:Label><br />
                                            <asp:Label ID="lblUpperCase" runat="server" Text="Label"></asp:Label><br />
                                            <asp:Label ID="lblNumeric" runat="server" Text="Label"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left:15px;" align="left">
                                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td width="12%">
                                                        &nbsp;</td>
                                                   <td>
                                                       &nbsp;</td>
                                                    <td valign="bottom" width="66%" rowspan="3">
                                                        <asp:LinkButton ID="btnChange" runat="server" CssClass="StandardButton" 
                                                            TabIndex="4">Speichern</asp:LinkButton>
                                                        
                                                    </td>                                                    
                                                </tr>
                                                <tr>
                                                    <td width="12%" style="padding-right:5px">
                                                        neues Passwort:
                                                    </td>
                                                   <td>
                                                       <asp:TextBox ID="txtNewPwd" TabIndex="2" runat="server" Width="150px" autocomplete="off" TextMode="Password"></asp:TextBox>
                                                    
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="12%" style="padding-right:5px">
                                                       Passwortbestätigung:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNewPwdConfirm" TabIndex="3" runat="server" Width="150px" TextMode="Password"></asp:TextBox>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;</td>
                                                </tr>                                                
                                                <tr>
                                                    <td width="12%" >
                                                    <div id="spanUCase" style="color: #B70000; padding-top: 10px;">Großbuchstaben:</div><br />
                                                    <div id="spanNumeric" style="color: #B70000" >Zahl:</div><br />
                                                    <div id="spanSpecial" style="color: #B70000">Sonderzeichen:</div><br />
                                                    <div id="spanLength" style="color: #B70000">Länge:</div><br />
                                                    </td>
                                                    <td  width="25%" >
                                                        <div id="divUCase" style="color: #B70000; padding-top: 10px;">nicht erfüllt</div><br />
                                                        <div id="divNumeric" style="color: #B70000">nicht erfüllt</div><br />
                                                        <div id="divSpecial" style="color: #B70000">nicht erfüllt</div><br />
                                                        <div id="divLength" style="color: #B70000">nicht erfüllt</div><br />
                                                    </td>
                                                    <td width="75%">

                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td colspan="2">

                                                    </td>                                                    
                                                    <td width="75%">
                                                    </td>
                                                </tr>                                                
                                            </table>
                                      
                                        </td>
                                    </tr>
                                </table>
                                    <table id="tblRequestQuestion" width="750" align="left" visible="false" runat="server">
                                    <tr>
                                        <td align="left" height="25">
                                        </td>
                                    </tr>
                                    <tr class="TextLarge" id="Tr2" runat="server">
                                        <td align="left"  style="padding-left:15px;">
                                            An dieser Stelle können Sie eine Kombination von geheimer Frage&nbsp;und nur Ihnen
                                            bekannter Antwort speichern.<br>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextNormal" align="left">
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="padding-left:15px;" align="left">
                                            &nbsp;
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td  style="padding-left:15px;" width="12%">
                                                        Frage:
                                                    </td>
                                                    <td width="22%">
                                                        <asp:DropDownList ID="ddlFrage" runat="server" Width="500px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="bottom" width="66%" rowspan="3">
                                                        &nbsp; <asp:LinkButton
                                                ID="cmdSetzeFrageAntwort" runat="server" CssClass="StandardButton">Speichern</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left:15px;" nowrap width="12%">
                                                        Antwort:
                                                    </td>
                                                    <td width="22%">
                                                        <asp:TextBox ID="txtAnfordernSpeichern" TabIndex="3" runat="server" Width="500px"
                                                            MaxLength="150"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" height="25">
                                            &nbsp;</td>
                                    </tr>
                                </table>                                      
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="padding-left:15px;">
                                <asp:Label ID="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:Label><br />
                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                    <p align="right">
                        <font face="Arial" size="1">©&nbsp;2001&nbsp;-&nbsp;2010&nbsp;DAD&nbsp;Deutscher&nbsp;Auto&nbsp;Dienst&nbsp;GmbH&nbsp;/&nbsp;Christoph
                            Kroschke&nbsp;GmbH</font>
                    </p>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</form>
</body>
</html>
