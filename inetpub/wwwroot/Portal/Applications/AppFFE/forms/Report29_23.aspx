<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report29_23.aspx.vb" Inherits="AppFFE.Report29_23" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Überzogene Kontingente (Details)</TITLE>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			&nbsp;
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server">Überzogene Kontingente (Details)</asp:Label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">
									<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="javascript:window.close()">Fenster schließen</asp:HyperLink></TD>
							</TR>
						</TABLE>
						<uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD colSpan="2">
						<SCRIPT language="Javascript">
						<!-- //						
						//-->
						</SCRIPT>
						<asp:Label id="lblError" runat="server"></asp:Label>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>

