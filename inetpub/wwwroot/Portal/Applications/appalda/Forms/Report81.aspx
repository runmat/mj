<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report81.aspx.vb" Inherits="AppALDA.Report81" %>
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
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR id="trCreate" runat="server">
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
										<TR id="trSelect" runat="server">
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSelect" runat="server" CssClass="StandardButton"> &#149;&nbsp;Auswählen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Brief-Nr*:&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center"><asp:textbox id="txtOrdernummer" runat="server" MaxLength="8"></asp:textbox>&nbsp;(1234567)</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Kfz-Kennzeichen**:&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center"><asp:textbox id="txtAmtlKennzeichen" runat="server" MaxLength="9"></asp:textbox>&nbsp;(XX-Y1234)</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Fahrgestell-Nr***:&nbsp;&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center"><asp:textbox id="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150"></TD>
														<TD class="TextLarge" vAlign="center"><asp:textbox id="txtBriefnummer" runat="server" Visible="False"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" align="right" width="150">
															<P>*<br>
																**<br>
																***</P>
														</TD>
														<TD class="TextLarge" vAlign="center">Eingabe von nachgestelltem Platzhalter 
															möglich.&nbsp;Mindestens drei Zeichen.<BR>
															Eingabe von nachgestelltem Platzhalter möglich. Mindestens Kreis und ein 
															Buchstabe (z.B. XX-Y*)<BR>
															Eingabe von vorangestelltem Platzhalter möglich. Mindestens acht Zeichen (z.B. 
															*12345678)</TD>
													</TR>
													<TR id="trSelectDropdown" runat="server">
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Auswahl:&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center"><asp:dropdownlist id="ddlSelect" runat="server"></asp:dropdownlist></TD>
													</TR>
												</TABLE>
												<BR>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
