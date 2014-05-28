<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change09.aspx.vb" Inherits="AppSTRAUB.Change09" %>
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
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">Bitte geben Sie die Auswahlkriterien ein.</TD>
							</TR>
							<tr>
								<TD vAlign="top" width="100"></TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left">
												<TABLE class="BorderFull" id="Table1" cellSpacing="0" cellPadding="2" border="0">
													<TR>
														<TD>&nbsp;</TD>
														<TD colSpan="3"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;</TD>
													</TR>
													<TR>
														<TD></TD>
														<TD class="" width="150">Fahrgestellnummer*:&nbsp;</TD>
														<TD class="" colSpan="1" rowSpan="1"><asp:textbox id="txtFahrgestellNr" runat="server" CssClass="TextBoxStyleLarge" MaxLength="35" Width="200px"></asp:textbox></TD>
														<TD></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD class="" width="150" colSpan="1" rowSpan="1">Kennzeichen:
														</TD>
														<TD class="" vAlign="top"><asp:textbox id="txtKennzeichen" runat="server" CssClass="TextBoxStyleLarge" MaxLength="35" Width="200px"></asp:textbox></TD>
														<TD vAlign="top"></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD colSpan="1" rowSpan="1">Equipmenttyp:</TD>
														<TD class="" vAlign="top"><asp:radiobuttonlist id="rbTyp" runat="server" RepeatDirection="Horizontal">
																<asp:ListItem Value="B" Selected="True">Brief</asp:ListItem>
																<asp:ListItem Value="T">Schl&#252;sselt&#252;te</asp:ListItem>
															</asp:radiobuttonlist></TD>
														<TD vAlign="top"></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD colSpan="1" rowSpan="1"></TD>
														<TD vAlign="top" colSpan="1" rowSpan="1"></TD>
														<TD vAlign="top" colSpan="1" rowSpan="1"><asp:linkbutton id="Linkbutton2" runat="server" CssClass="StandardButton">&#149&nbsp;Weiter&nbsp;&#187</asp:linkbutton></TD>
													</TR>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
									*<FONT face="Arial" size="1">Eingabe von Platzhaltern möglich, z.B. *12345 oder 
										12345*</FONT>
								</td>
							</tr>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
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
	</body>
</HTML>
