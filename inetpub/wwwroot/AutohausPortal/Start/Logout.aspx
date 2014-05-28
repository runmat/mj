<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="AutohausPortal.Start.Logout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;</div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 78%"><div id = "FormDiv" runat="server">
                    <div id="innerContentRightHeading">
                    </div>
                </div>
                    <div id="pagination">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>

                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                        <tr>
                            <td valign="middle" align="center"  height="400">
                                Die Sitzung wurde aufgrund einer Doppelanmeldung beendet.<br>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                                Bitte melden Sie sich gegebenefalls neu an bzw.&nbsp;wenden Sie sich&nbsp;an Ihren
                                Systemverantwortlichen.
                            </td>
                        </tr>
                    </table>
                    <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                        <tr>
                            <td valign="middle" align="center" height="400">
                                Die Sitzung wurde beendet.<br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                                Bitte benutzen Sie den vorgegebenen Link für einen Neueinstieg in die Anwendung.
                            </td>
                        </tr>
                    </table>
                    <asp:Literal id="Literal1" runat="server"></asp:Literal>

                </div>
            </div>
        </div>
    </div>
</body>
</html>
