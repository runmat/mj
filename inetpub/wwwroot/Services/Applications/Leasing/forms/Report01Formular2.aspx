<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report01Formular2.aspx.cs"
    Inherits="Leasing.forms.Report01Formular2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
</head>
<body leftmargin="0" topmargin="0" ms_positioning="GridLayout">
    <table height="710" cellspacing="0" cellpadding="0" width="571" border="0" ms_2d_layout="TRUE">
        <tr valign="top">
            <td width="571" height="710">
                <form id="Form1" method="post" runat="server">
                <table height="569" cellspacing="0" cellpadding="0" width="708" border="0" ms_2d_layout="TRUE">
                    <tr valign="top">
                        <td width="3" height="16">
                        </td>
                        <td width="2">
                        </td>
                        <td width="3">
                        </td>
                        <td width="118">
                        </td>
                        <td width="266">
                        </td>
                        <td width="316">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="54">
                        </td>
                        <td colspan="3">
                            <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="Large">Erklärung über den Verbleib eines Fahrzeugscheines</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="34">
                        </td>
                        <td rowspan="5">
                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Width="223px" Visible="False"
                                ForeColor="Red" Font-Names="Arial" Font-Size="XX-Large">Fehler beim Seitenaufbau.</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="32">
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="Small">Die Firma</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="24">
                        </td>
                        <td colspan="2">
                            <%--<asp:Label ID="lblName1" runat="server" Font-Size="Small" Font-Names="Arial" Width="236px"> LeasePlan Deutschland GmbH</asp:Label>--%>
                            <asp:Label ID="lblName1" runat="server" Font-Size="Small" Font-Names="Arial" Width="236px"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="32">
                        </td>
                        <td colspan="2">
                            <%--<asp:Label ID="lblStrasse" runat="server" Font-Size="Small" Font-Names="Arial" Width="184px">Hellersbergstraße 10b</asp:Label>--%>
                            <asp:Label ID="lblStrasse" runat="server" Font-Size="Small" Font-Names="Arial" Width="184px"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="80">
                        </td>
                        <td colspan="2">
                            <%--<asp:Label ID="lblPlzOrt" runat="server" Font-Size="Small" Font-Names="Arial"> 41460 Neuss</asp:Label>--%>
                            <asp:Label ID="lblPlzOrt" runat="server" Font-Size="Small" Font-Names="Arial"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="45">
                        </td>
                        <td colspan="3">
                            <asp:Label ID="Label4" runat="server" Font-Names="Arial" Font-Size="Small">erklärt als Eigentümerin des Fahrzeuges mit dem amtlichen Kennzeichen</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="51">
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblFahrzeugkennzeichen" runat="server" Font-Bold="True" Font-Size="Large">________________________________,</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="48">
                        </td>
                        <td colspan="3">
                            <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="Small">dass der Fahrzeugschein zur Stillegung nicht vorgelegt werden kann.</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="111">
                        </td>
                        <td colspan="3">
                            <asp:Label ID="Label6" runat="server" Width="628px" Font-Names="Arial" Font-Size="Small"
                                Height="41px">Wir verpflichten uns, bei Fund der ersten Ausfertigung des Fahrzeugscheines, diesen unaufgefordert bei der Zulassungsstelle abzugeben.</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td height="23">
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True">.......................................................</asp:Label>
                        </td>
                        <td rowspan="2">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2" height="19">
                        </td>
                        <td colspan="3">
                            <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="Small">Stempel und Unterschrift</asp:Label>
                        </td>
                    </tr>
                </table>
                </form>
            </td>
        </tr>
    </table>
</body>
</html>
