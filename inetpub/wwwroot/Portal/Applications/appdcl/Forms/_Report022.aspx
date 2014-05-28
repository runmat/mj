<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="_Report022.aspx.vb" Inherits="AppDCL.__Report022" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <script type="text/javascript">self.focus();</script>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td>
                <strong>‹bertragungsprotokoll vom&nbsp;</strong>
                <asp:Label ID="lblDatum" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" bgcolor="gainsboro">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:window.close()">Fenster schlieﬂen</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblInfo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                <asp:Label ID="lblOpen" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
