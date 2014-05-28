<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_05.aspx.vb" Inherits="AppUeberf.Ueberfg_05" %>
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
			<TABLE id="Table4" width="100%">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Zusammenstellung der Abfragekriterien</asp:label>)</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Beauftragen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
										<TR>
											<TD class="" vAlign="top"></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap>Kennzeichen:
														</TD>
														<TD class="TextLarge" vAlign="center" width="100%"><asp:textbox id="txtKennzeichen" runat="server"></asp:textbox>&nbsp;
														</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="center">Kundenreferenz&nbsp;:</TD>
														<TD class="StandardTableAlternate" vAlign="center"><asp:textbox id="txtReferenz" runat="server"></asp:textbox>&nbsp;</TD>
													</TR>
												</TABLE>
												&nbsp;</TD>
										</TR>
									</TABLE>
									<P><asp:label id="lblOutput" runat="server" Font-Italic="True" Font-Bold="True"></asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top">&nbsp;</TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top"><asp:label id="lblInfo" runat="server" CssClass="TextInfo" Font-Bold="True" EnableViewState="False"> Bitte geben Sie ein Kennzeichen und/oder eine Kundenreferenz ein.</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD>&nbsp;</TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
