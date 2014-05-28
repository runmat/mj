<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report41_03Print.aspx.vb" Inherits="AppFFD.Report41_03Print"%>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Händlerübersicht Kontingente/Fälligkeiten - Druckansicht</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
	</HEAD>
	<body bgColor="white" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" height="203" cellSpacing="1" cellPadding="1" width="619" border="0">
				<TR>
					<TD noWrap colSpan="2"><FONT face="Arial"></FONT>
						<P><FONT face="Arial"><STRONG><FONT size="4"><U>Händlerübersicht - Versandadressen</U></FONT></STRONG></FONT></P>
					</TD>
				</TR>
				<TR>
					<TD align="left" colSpan="2"><FONT face="Arial"></FONT><FONT face="Arial"><FONT size="2">Benutzter:</FONT>&nbsp;
							<asp:textbox id="txtUser" runat="server" BackColor="Transparent" BorderWidth="0px" ReadOnly="True" BorderColor="Transparent"></asp:textbox></FONT></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" colSpan="2">
						<TABLE id="Table2" height="85" cellSpacing="1" cellPadding="1" width="614" border="0">
							<TR>
								<TD width="22" height="24"><FONT face="Arial" size="2"><STRONG>Händlernr:</STRONG></FONT></TD>
								<TD width="269" height="24"><asp:textbox id="txtNr" runat="server" BackColor="Transparent" BorderWidth="0px" ReadOnly="True" BorderColor="Transparent" Width="172px"></asp:textbox></TD>
								<TD width="897" height="24"><FONT face="Arial" size="2"><STRONG>Lastschrift:</STRONG></FONT></TD>
								<TD width="536" height="24"><INPUT type="checkbox">&nbsp;<FONT face="Arial" size="2"><STRONG>seit:<asp:textbox id="datseit" runat="server" BackColor="Transparent" BorderWidth="0px" ReadOnly="True" BorderColor="Transparent" Width="68px"></asp:textbox></STRONG></FONT></TD>
							</TR>
							<TR>
								<TD width="22"><FONT face="Arial" size="2"><STRONG>Name:</STRONG></FONT></TD>
								<TD width="269"><asp:textbox id="txtName" runat="server" BackColor="Transparent" BorderWidth="0px" ReadOnly="True" BorderColor="Transparent" Width="271px" Height="19px"></asp:textbox></TD>
								<TD width="897"><FONT face="Arial" size="2"><STRONG>letzte Änderung:</STRONG></FONT></TD>
								<TD width="232"><asp:textbox id="datLastChange" runat="server" BackColor="Transparent" BorderWidth="0px" ReadOnly="True" BorderColor="Transparent" Width="109px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="22"><FONT face="Arial" size="2"><STRONG>Adresse:</STRONG></FONT></TD>
								<TD width="269"><asp:textbox id="txtAdresse" runat="server" BackColor="Transparent" BorderWidth="0px" ReadOnly="True" BorderColor="Transparent" Width="287px" Height="19px"></asp:textbox></TD>
								<TD width="897"><FONT face="Arial" size="2"><STRONG>gehört zu:</STRONG></FONT></TD>
								<TD width="232"><asp:textbox id="txtDistrikt" runat="server" BackColor="Transparent" BorderWidth="0px" ReadOnly="True" BorderColor="Transparent" Width="112px"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" colSpan="2">
						<TABLE id="AddressTable" height="85" cellSpacing="1" cellPadding="1" width="614" border="0" runat="server">
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
