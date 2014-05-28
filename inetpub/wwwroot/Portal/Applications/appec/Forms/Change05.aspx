<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05.aspx.vb" Inherits="AppEC.Change05" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2" height="19">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>
                                <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                            </td>
                        </tr>
                        <tr>
            </td>
            <td valign="top" width="100%">
                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="TaskTitle" valign="top" width="100%">
                            &nbsp;Bitte PDI - Vorauswahl treffen.
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="PageNavigation" valign="top">
                            <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" id="Table3" width="100%">
                    <tr>
                        <td valign="top" align="middle" width="100%">
                            <table class="BorderLeftBottom" id="Table1" cellspacing="0" cellpadding="5" width="400"
                                bgcolor="white" border="0">
                                <tr>
                                    <td class="TextLarge" valign="middle" nowrap>
                                        Nur Fahrzeuge mit <br />
                                        PDI Bereitmeldung:
                                    </td>
                                    <td  valign="middle" width="100%">
                                        <asp:CheckBox ID="cbxPDI" runat="server" AutoPostBack="True" Checked="True"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" valign="middle"><hr style="color:#cccccc;"/></td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        PDI:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlPDIs" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>
                                        <%--&nbsp;
                                        <asp:CheckBox ID="cbxAlle" runat="server" Text="Alle"></asp:CheckBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        Hersteller:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlHersteller" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>                                       
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        ModellID:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlModellID" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>                                       
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        SIPP Code:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlSIPP" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>                                       
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        Reifenart:
                                    </td>
                                    <td valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlReifenart" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>                                       
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        Farbe:
                                    </td>
                                    <td valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlFarbe" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>                                       
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        Navigation:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlNavi" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>                                     
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        Anhängerkupplung:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:DropDownList ID="ddlAHK" runat="server" CssClass="DropDownStyle" AutoPostBack="true" Width="230">
                                        </asp:DropDownList>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        Auftragsnummer:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:TextBox id="txtAuftragsnummerVon" runat="server" AutoPostBack="true" Width="110"/> - <asp:TextBox id="txtAuftragsnummerBis" runat="server" AutoPostBack="true" Width="110"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        Fahrgestellnummer:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:TextBox id="txtFahrgestellnummer" runat="server" AutoPostBack="true" Width="230"/>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TextLarge" valign="middle" style="white-space:nowrap;">
                                        gefunden Fahrzeuge:
                                    </td>
                                    <td  valign="middle" width="100%" style="white-space:nowrap;">
                                        <asp:Label id="lblAnzahl" runat="server" Text="0"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" valign="middle"><hr style="color:#cccccc;"/></td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="top" nowrap>
                                        Aktion:
                                    </td>
                                    <td class="TextLarge" valign="middle" width="100%">
                                        <table class="TableBackGround" id="Table2" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rbAktion" runat="server" AutoPostBack="True">
                                                        <asp:ListItem Value="Zulassen" Selected="True">Zulassen</asp:ListItem>
                                                        <asp:ListItem Value="Sperren">Sperren</asp:ListItem>
                                                        <asp:ListItem Value="Entsperren">Entsperren</asp:ListItem>
                                                        <asp:ListItem Value="Verschieben">Verschieben</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="middle" nowrap>
                                    </td>
                                    <td  valign="middle" width="100%">
                                        <p align="right">
                                            <asp:LinkButton ID="btnConfirm" runat="server" CssClass="StandardButtonTable" Width="150px">Weiter&nbsp;&#187;</asp:LinkButton></p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <p align="right">
                    &nbsp;</p>
            </td>
        </tr>
        <tr>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </TD></TR></TBODY></TABLE>
    </form>
</body>
</html>
