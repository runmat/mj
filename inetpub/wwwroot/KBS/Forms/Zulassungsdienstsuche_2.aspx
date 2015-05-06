<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Zulassungsdienstsuche_2.aspx.vb" Inherits="KBS.Zulassungsdienstsuche_2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
    <link href="/PortalZLD/Styles/default.css" rel="stylesheet" type="text/css" media="screen, projection"/>
    <style type="text/css">
        .TableStyle
        {
            background-color: #ffffff;
            border: solid 1px #dfdfdf
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size: 10pt;color: #666666; padding-left:15px; font-weight:bold">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td valign="top" align="left" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0" class="TableStyle">
                    <tr>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top" align="left">
                                    
                                        <img alt="Drucken" style="float:left;cursor:pointer"  title="Drucken" onclick="javascript:window.print()" src="../../images/Drucker04_08.jpg" />
                                         &nbsp;
                                        <img alt="Schliessen" style="float:right; width:18px; height:18px;cursor:pointer"  title="Schliessen" onclick="javascript:window.close()" src="../../images/Delete01_06.jpg" />
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="left">
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" colspan="2">
                                                    <table id="Table2" cellspacing="0" cellpadding="2" border="0" width="100%">
                                                        <tr>
                                                            <td  valign="top" colspan="2">
                                                                <asp:Label ID="Label1" CssClass="lblKontaktFirma" Font-Size="10pt" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td  valign="top" width="120" style="font-weight:normal">
                                                                Post-Anschrift:
                                                            </td>
                                                            <td valign="top" width="100%">
                                                                <asp:Label ID="Label3" runat="server">-</asp:Label><br>
                                                                <asp:Label ID="Label4" runat="server"></asp:Label><br>
                                                                <asp:Label ID="Label5" runat="server"></asp:Label>&nbsp;
                                                                <asp:Label ID="Label6" runat="server"></asp:Label><br/>
                                                                <asp:Label ID="Label7" runat="server"></asp:Label>&nbsp;
                                                                <asp:Label ID="Label8" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-weight:normal">
                                                                Ansprechpartner:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="120" style="font-weight:normal">
                                                                Telefon 1:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label9" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="120" style="font-weight:normal">
                                                                Telefon 2:
                                                            </td>
                                                            <td class="style1">
                                                                <asp:Label ID="Label10" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120" style="font-weight:normal">
                                                                Telefon 3:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label11" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120" style="font-weight:normal">
                                                                Fax:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label12" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120" style="font-weight:normal">
                                                                eMail:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label13" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120" style="font-weight:normal">
                                                                48 Std. möglich:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label18" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" ID="trAbwAdresse" Visible="False">
                                                            <td  valign="top" width="120" style="font-weight:normal">
                                                                abw. Versandadresse:
                                                            </td>
                                                            <td valign="top" width="100%">
                                                                <asp:Label ID="Label21" runat="server">-</asp:Label><br>
                                                                <asp:Label ID="Label22" runat="server"></asp:Label><br>
                                                                <asp:Label ID="Label23" runat="server"></asp:Label><br/>
                                                                <asp:Label ID="Label24" runat="server"></asp:Label>&nbsp;
                                                                <asp:Label ID="Label25" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120" style="font-weight:normal">
                                                                Lieferuhrzeit:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label19" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120" style="font-weight:normal">
                                                                Nachreichen möglich:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label20" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" style="font-weight:normal">
                                                                Anforderungen:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label14" runat="server">-</asp:Label><br>
                                                                <asp:Label ID="Label15" runat="server">-</asp:Label><br>
                                                                <asp:Label ID="Label16" runat="server">-</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="120">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" style="font-weight:normal">
                                                                Bemerkungen:
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="Label17" runat="server">-</asp:Label>&nbsp;
                                                            </td>
                                                        </tr>
                                                                                                                <tr>
                                                            <td valign="top" width="120">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>