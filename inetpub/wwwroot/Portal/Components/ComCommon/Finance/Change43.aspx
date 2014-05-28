<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change43.aspx.vb" Inherits="CKG.Components.ComCommon.Change43" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Fahrzeugsuche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR id="tr_HaendlerNr" runat="server">
														<TD class="TextLarge" noWrap width="150"><asp:label id="lbl_HaendlerNr" runat="server">Händlernummer</asp:label>:</TD>
														<TD class="TextLarge"><asp:textbox id="txtHaendlerNr" runat="server" Width="250px" MaxLength="11"></asp:textbox></TD>
													</TR>
													<TR id="tr_Haendler" runat="server">
														<TD class="TextLarge" noWrap width="150"><asp:label id="lbl_Haendler" runat="server">lbl_Haendler</asp:label>:</TD>
														<TD class="TextLarge"><asp:DropDownList ID="ddlHaendler" Runat="server"></asp:DropDownList></TD>
													</TR>
													<TR id="tr_Vertragsnummer" runat="server">
														<TD class="TextLarge" noWrap><asp:label id="lbl_Vertragsnummer" runat="server">Vertragsnummer</asp:label>:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge" width="100%"><asp:textbox id="txtVertragsNr" runat="server" Width="250px" MaxLength="11"></asp:textbox></TD>
													</TR>
													<TR id="tr_Ordernummer" runat="server">
														<TD class="StandardTableAlternate" noWrap><asp:label id="lbl_Ordernummer" runat="server">Ordernummer</asp:label>:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtOrderNr" runat="server" Width="250px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="tr_Fahrgestellnummer" runat="server">
														<TD class="TextLarge" noWrap><asp:label id="lbl_Fahrgestellnummer" runat="server">Fahrgestellnummer</asp:label>:<BR>
															&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge"><asp:textbox id="txtFahrgestellNr" runat="server" Width="250px" MaxLength="35"></asp:textbox><BR>
															(Eingabe von Platzhaltern möglich, z.B. *12345 oder 12345*)</TD>
													</TR>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<asp:literal id="Literal1" runat="server"></asp:literal>
	</body>
</HTML>
