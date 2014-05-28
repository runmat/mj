<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report04.aspx.vb" Inherits="AppDCL._Report04" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (PDF-Split)</asp:label>
								</td>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;</TD>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">&nbsp;</TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"></td>
											<TD></TD>
											<TD></TD>
										<TR>
											<TD class="" colSpan="3" width="100%"><strong>&nbsp;</strong></TD>
											<TD>
												<P align="right">&nbsp;</P>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="" vAlign="top" align="left" colSpan="3">
											</TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<asp:radiobuttonlist id="rdbSplit" runat="server" RepeatDirection="Horizontal">
													<asp:ListItem Value="1" Selected="True">Startpunkt setzen</asp:ListItem>
													<asp:ListItem Value="2">Startpunkt zur&#252;cksetzen</asp:ListItem>
												</asp:radiobuttonlist></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td width="120">&nbsp;</td>
								<td>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</TR>
							<TR>
								<td width="120">&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<TR id="ShowScript" runat="server">
								<td width="120">&nbsp;</td>
								<td>
									<script language="JavaScript">
										<!-- //
										window.document.Form1.elements[window.document.Form1.length-1].focus();
										//-->
									</script>
								</td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
