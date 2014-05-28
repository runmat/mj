<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="_Report01s.aspx.vb" Inherits="CKG.Components.ComArchive._Report01s"   %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server" >
    <meta http-equiv="content-type" content="text/html; charset=iso-8859-1" />
    <link href="/Services/Styles/default.css" media="screen, projection" type="text/css" rel="stylesheet" />
    <title>Details</title>
  </head>
<body id="top">
    <form id="Form1" runat="server">
                <div id="innerContentRight" style="width: 100%;">
                    <asp:Panel ID="DivSearch" runat="server">
                        <div id="TableQuery">
                            <table width="300" align="left">
                                <tr>
                                    <td>
                                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="PageNavigation" align="right" colspan="2" height="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableButtonFrame" valign="top">
                                                </td>
                                                <td valign="top">
                                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td class="TaskTitle" align="right">
                                                                                                                        <div id="btnCloseParent" >
                                                                schliessen <asp:LinkButton ID="btnClose" runat="server" 
                                                                    OnClientClick="javascript:window.close()" Text="X" ToolTip="schliessen  X"  Style=" color: #666666; text-align: center;font-size: 10pt;  font-weight: bold; text-decoration: none;padding: 2px;" />
                                                            </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="LabelExtraLarge">
                                                                <table id="Table9" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                    <tr>
                                                                        <td valign="top" align="left">
                                                                            <table id="Table10" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                                                                                border="0" runat="server">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td valign="top" align="left" width="100%" bgcolor="#ffffff">
                                                                                            <table  id="Table4" cellspacing="1"
                                                                                                cellpadding="1" width="100%"  border="0">
                                                                                                <tr>
                                                                                                    <td class="GridTableHead" height="30" colspan="2" rowspan="1">
                                                                                                        <strong>Detailinformationen</strong>
                                                                                                    </td>
                                                                                           </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        &nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Länge in Bytes:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtLAenge" runat="server"  BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Erstellungsdatum:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtErstellDatum" runat="server"  BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Letzte Änderung:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtAenderDatum" runat="server"  BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Titel:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtTitel" runat="server" BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Anzahl Felder gesamt:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtFelderGesamt" runat="server" BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Anzahl Textfelder:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtFelderText" runat="server" BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Anzahl Bildfelder:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtFelderBild" runat="server" BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="formquery">
                                                                                                    <td class="firstLeft active">
                                                                                                        Anzahl Binärfelder:
                                                                                                    </td>
                                                                                                    <td class="active">
                                                                                                        <asp:TextBox ID="txtFelderBlob" runat="server" BackColor="Transparent"
                                                                                                            BorderWidth="0px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
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
                    </asp:Panel>
                </div>
 
    </form>
</body>
</html>

