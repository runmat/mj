<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report022.aspx.vb" Inherits="AppDCL.__Report022" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<script>self.focus();</script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="0">
				<TR>
					<TD><strong>‹bertragungsprotokoll vom&nbsp;</strong>
						<asp:Label id="lblDatum" runat="server"></asp:Label>
					</TD>
				</TR>
				<TR>
					<TD align="right" bgColor="gainsboro">
						<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="javascript:window.close()">Fenster schlieﬂen</asp:HyperLink></TD>
				</TR>
				<TR>
					<TD>
						<asp:Label id="lblInfo" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>
						<asp:Label id="lblError" runat="server" CssClass="TextError"></asp:Label></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
