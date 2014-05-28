<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change81.aspx.vb" Inherits="AppFMS.Change81" %>
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
														<TD class="TextLarge" noWrap align="right">Auftragsnummer:&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtOrdernummer" runat="server" Width="250px" MaxLength="11"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">Kfz-Kennzeichen*:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtAmtlKennzeichen" runat="server" Width="250px" MaxLength="9"></asp:textbox>&nbsp;(XX-Y1234)</TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD>* Eingabe von nachgestelltem Platzhalter möglich. Mindestens Kreis und ein 
															Buchstabe (z.B. XX-Y*)</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">Fahrgestell-Nr**:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtFahrgestellnummer" runat="server" Width="250px" MaxLength="17"></asp:textbox></TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD>** Eingabe von vorangestelltem Platzhalter möglich. Mindestens acht Zeichen 
															(z.B. *12345678)
														</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right" vAlign="top">Platzhaltersuche:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate">
															<asp:RadioButton id="cbxPlatzhaltersuche" runat="server" Checked="True" GroupName="grpWeicheSuche" Text=" Platzhaltersuche möglich. Nur verwendbare Vorgänge werden angezeigt."></asp:RadioButton><BR>
															<asp:RadioButton id="cbxOhnePlatzhalter" runat="server" GroupName="grpWeicheSuche" Text=" Platzhalter werden ignoriert. Informationen zu dem Vorgang werden angezeigt, insofern er im System gefunden wurde."></asp:RadioButton></TD>
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
														<TD class="TextLarge" noWrap align="right">Dateiauswahl:&nbsp;&nbsp;</TD>
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
		<asp:Literal id="Literal1" runat="server"></asp:Literal>
	</body>
</HTML>
