<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Logout.aspx.vb" Inherits="CKG.Portal.Start.Logout" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>DAD Internet Reports - Abmeldung</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0">
		<LINK rel="stylesheet" type="text/css" href="../Styles.css">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
				<TR>
					<TD vAlign="center" align="middle" height="400">Die Sitzung wurde aufgrund einer 
						Doppelanmeldung beendet.<BR>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<BR>
						Bitte melden Sie sich gegebenefalls neu an bzw.&nbsp;wenden Sie sich&nbsp;an 
						Ihren Systemverantwortlichen.</TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
				<TR>
					<TD vAlign="center" align="middle" height="400">
						Die Sitzung wurde beendet.<BR>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<BR>
						Bitte benutzen Sie den vorgegebenen Link für einen Neueinstieg in die 
						Anwendung.</TD>
				</TR>
			</TABLE>
			<asp:Literal id="Literal1" runat="server"></asp:Literal>
		</form>
	</body>
</HTML>
