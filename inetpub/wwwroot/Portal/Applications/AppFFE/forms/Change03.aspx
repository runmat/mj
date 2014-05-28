<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="AppFFE.Change03" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="suche" Src="../PageElements/Suche.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body topmargin="0" leftmargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td vAlign="top" align="left">
						<uc1:suche id="suche1" runat="server"></uc1:suche></td>
				</tr>
			</table>
			<table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
				<TR>
					<td width="150"></td>
					<TD vAlign="top" align="left"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
				</TR>
				<tr>
					<td width="150"></td>
					<td vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
