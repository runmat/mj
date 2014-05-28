<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="AppKroschke.Change03" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title></title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body>
    
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2" height="19">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2" height="19">
                            &nbsp;<asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <asp:LinkButton ID="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" CssClass="StandardButton">Suchen</asp:LinkButton>
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <table border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                    <tr>
                        <td >
                            Auswahl:
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rblAuswahl" RepeatDirection="Vertical">
                                <asp:ListItem Text="Neue Aufträge (Alle neuen oder geänderten Aufträge)" Value=""
                                    Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Angenommene Aufträge (Alle angenommen Aufträge die noch nicht abgeschlossen sind)"
                                    Value="OK"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
            </td>
        </tr>
        <tr>
            <td valign="top">
                &nbsp;
            </td>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
        </table> </td> </tr>
    </table>
    </form>
</body>
</html>
