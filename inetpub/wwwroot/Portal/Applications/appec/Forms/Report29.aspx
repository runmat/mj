<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report29.aspx.vb" Inherits="AppEC.Report29" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
        <table width="100%" align="center">
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblError"> </asp:Label>
                                
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
