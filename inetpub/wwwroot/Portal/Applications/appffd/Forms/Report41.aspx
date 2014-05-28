<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report41.aspx.vb" Inherits="AppFFD.Report41"%>
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
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"></asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"><asp:linkbutton id="cmdSelect" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Auswählen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suche</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"></TD>
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
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR id="trHaendlernummer" runat="server">
														<TD class="TextLarge" width="4" height="32">Händlernummer:&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtNummer" runat="server"></asp:textbox><asp:label id="lblHDNummer" runat="server" Visible="False"></asp:label></TD>
													</TR>
													<TR id="trName" runat="server">
														<TD class="TextLarge" width="4" height="32">Name:&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtName" runat="server"></asp:textbox><asp:label id="lblName" runat="server" Visible="False"></asp:label></TD>
													</TR>
													<TR id="trHdAuswahl" runat="server" Visible="False">
														<TD class="TextLarge" width="4" height="32"><asp:label id="lblAuswahl" runat="server" Visible="False">Auswahl:</asp:label></TD>
														<TD class="TextLarge"><asp:dropdownlist id="cmbHaendler" runat="server" Visible="False"></asp:dropdownlist><asp:label id="lblTreffer" runat="server" Visible="False"></asp:label></TD>
													</TR>
													<TR id="DistrictRow" runat="server" Visible="False">
														<TD class="TextLarge" width="4">Regionalbüros:</TD>
														<TD class="TextLarge"><asp:dropdownlist id="DistrictDropDown" runat="server"></asp:dropdownlist>&nbsp;&nbsp;</TD>
													</TR>
												</TABLE>
												<asp:label id="lblMessage" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
