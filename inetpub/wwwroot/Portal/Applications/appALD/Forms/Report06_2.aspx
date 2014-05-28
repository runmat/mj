<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report06_2.aspx.vb" Inherits="AppALD.Report06_2" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body topmarign="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<table width="750" align="center" cellpadding="10" cellspacing="0" border="0">
				<tr>
					<td colspan="2"><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td vAlign="top" align="left" class="TextMainHeader">
						<asp:Label id="lblHead" runat="server"></asp:Label></td>
					<td><a class="TextLarge" href="javascript:window.close();">Fenster schlieﬂen</a></td>
				</tr>
				<tr>
					<td colspan="2" vAlign="top" align="middle" class="TextExtraLarge">&nbsp;&nbsp;&nbsp;&nbsp;
						<BR>
						<asp:DataGrid id="grdResult" runat="server" Width="100%" BackColor="White">
							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
							<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
						</asp:DataGrid></td>
				</tr>
				<tr>
					<td colspan="2"><!--#include File="../../../PageElements/Footer.html" -->
						<asp:Label id="lblError" runat="server"></asp:Label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
