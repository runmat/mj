<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report02.aspx.vb" Inherits="AppVW.Report02" %>
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
								<TD class="PageNavigation"><asp:label id="lblHead" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">
									<DIV align="center">
										<table style="WIDTH: 100%; BORDER-COLLAPSE: collapse; BACKGROUND-COLOR: white" cellSpacing="0" align="center" border="1">
											<tr class="GridTableHead" nowrap="nowrap">
												<td>&nbsp;</td>
												<td colSpan="10">
													<table cellSpacing="0" cellPadding="0" border="0">
														<tr class="GridTableHead" nowrap="nowrap">
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>O</td>
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>=</td>
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>Original wird benötigt</td>
														</tr>
														<tr class="GridTableHead" nowrap="nowrap">
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>K</td>
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>=</td>
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>Kopie ausreichend</td>
														</tr>
														<tr class="GridTableHead" nowrap="nowrap">
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>F</td>
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>=</td>
															<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
															<td>Formular der Zulassungsstelle wird benötigt (Muster)</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr nowrap="nowrap">
												<td>&nbsp;</td>
												<td colSpan="10">&nbsp;</td>
											</tr>
											<tr class="GridTableHead" nowrap="nowrap">
												<td vAlign="bottom" rowSpan="2">Dienstleistung<BR>
													&nbsp;</td>
												<td align="middle" colSpan="10">Erforderliche Unterlagen</td>
											</tr>
											<tr class="GridTableHead" nowrap="nowrap">
												<td noWrap align="middle">Fahrzeug-<BR>
													brief (ZB2)</td>
												<td noWrap align="middle">Fahrzeug-<BR>
													schein (ZB1)</td>
												<td noWrap align="middle">&nbsp;&nbsp;&nbsp;&nbsp; COC&nbsp;&nbsp; &nbsp;</td>
												<td noWrap align="middle">Deckungs-<BR>
													karte</td>
												<td noWrap align="middle">Vollmacht<BR>
													&nbsp;&nbsp;</td>
												<td noWrap align="middle">Personal-<BR>
													ausweis</td>
												<td noWrap align="middle">Gewerbe-<BR>
													anmeldung</td>
												<td noWrap align="middle">Handels-<BR>
													register</td>
												<td noWrap align="middle">Lastschrift-<BR>
													einzug</td>
												<td noWrap align="middle">Zulassungs-<BR>
													auftrag</td>
											</tr>
											<TR>
												<TD></TD>
												<TD></TD>
												<TD></TD>
												<TD></TD>
												<TD></TD>
												<TD align="middle">
													<asp:HyperLink id="lnkVollmacht" runat="server" NavigateUrl="../Docs/Allgemeine Vollmacht DAD-ORG-FD-F10-0202.pdf" Target="_blank">Download</asp:HyperLink></TD>
												<TD></TD>
												<TD></TD>
												<TD></TD>
												<TD align="middle">
													<asp:HyperLink id="lnkEinzug" runat="server" NavigateUrl="Report02_2.aspx">Download</asp:HyperLink></TD>
												<TD align="middle">
													<asp:HyperLink id="lnkZulassung" runat="server" Target="_blank" NavigateUrl="../Docs/VW Internet Anlage 01 Formular Zulassungsauftrag.pdf">Download</asp:HyperLink></TD>
											</TR>
											<tr class="GridTableHead" nowrap="nowrap">
												<td>PRIVATPERSON</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td class="GridTableHead" noWrap>Zulassung Gebrauchtwagen</td>
												<td align="middle">O</td>
												<td align="middle">O*</td>
												<td align="middle">O</td>
												<td align="middle">O</td>
												<td align="middle">O</td>
												<td align="middle">O</td>
												<td align="middle">&nbsp;</td>
												<td align="middle">&nbsp;</td>
												<td align="middle">O / F</td>
												<td align="middle">O</td>
											</tr>
											<tr>
												<td class="GridTableHead" noWrap>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
											</tr>
											<tr class="GridTableHead" nowrap="nowrap">
												<td>UNTERNEHMEN</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td class="GridTableHead" noWrap>Zulassung Gebrauchtwagen</td>
												<td align="middle">O</td>
												<td align="middle">O*</td>
												<td align="middle">O</td>
												<td align="middle">O</td>
												<td align="middle">O</td>
												<td align="middle">K</td>
												<td align="middle">K</td>
												<td align="middle">K</td>
												<td align="middle">O / F</td>
												<td align="middle">O</td>
											</tr>
											<tr>
												<td class="GridTableHead" noWrap>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
											</tr>
										</table>
									</DIV>
									&nbsp;&nbsp;
									<BR>
									<div align="center">* Bei alten Zulassungsdokumenten: Abmeldebescheinigung</div>
									<BR>
									&nbsp;&nbsp;
									<BR>
									<div class="GridTableHead">Bemerkungen:</div>
									<br>
									<STRONG>Wir weisen darauf hin, dass in einigen Zulassungskreisen andere als die 
										angezeigten Dokumente benötigt werden.</STRONG>
									<br>
									&nbsp;&nbsp;
								</TD>
							</TR>
							<TR>
								<TD vAlign="top"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
							</TR>
							<TR>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
