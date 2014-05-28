<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80.aspx.vb" Inherits="AppVW.Change80" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server" Visible="False"> (Fahrzeugsuche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120" height="192">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="120">&nbsp;</TD>
										</TR>
										<TR id="trcmdUpload" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="cmdUpload" runat="server" CssClass="StandardButton"> &#149;&nbsp;Mehrfachauswahl</asp:linkbutton></TD>
										</TR>
										<TR id="trcmdSearch" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
										<TR id="trcmdContinue" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="cmdContinue" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" height="192">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="TextLarge" noWrap align="right">Vorhaben:&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtVorhaben" runat="server" MaxLength="20" Width="250px"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">IKZ*:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtIKZ" runat="server" Width="250px"></asp:textbox></TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD>* Eingabe von mehreren, mit Komma getrennten&nbsp;Werten möglich</TD>
													</TR>
													<TR id="trVIN1" runat="server">
														<TD class="StandardTableAlternate" noWrap align="right">Fahrgestell-Nr**:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtFahrgestellnummer" runat="server" MaxLength="11" Width="250px"></asp:textbox></TD>
													</TR>
													<TR id="trVIN2" runat="server">
														<TD>&nbsp;</TD>
														<TD>** Eingabe der ersten 11 Stellen
														</TD>
													</TR>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
									<TABLE id="tblUpload" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="tbl0001" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="TextLarge" noWrap align="right">Dateiauswahl <A href="javascript:openinfo('Info01.htm');">
																<IMG src="../../../images/fragezeichen.gif" border="0"></A>:&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><INPUT id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">&nbsp;</TD>
														<TD class="TextLarge">&nbsp;
															<asp:label id="lblExcelfile" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<asp:literal id="Literal1" runat="server"></asp:literal>
		<SCRIPT language="JavaScript">										
				<!--
						function openinfo (url) {
								fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0");
								fenster.focus();
						}
				-->
		</SCRIPT>
	</body>
</HTML>
