<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report38.aspx.vb" Inherits="CKG.Components.ComCommon.Report38" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
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
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Schlüsselinformationen)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD class=""><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2">
															<TABLE id="Table10" cellSpacing="0" cellPadding="5" border="0" bgColor="white">
																<tr>
																	<TD class="PageNavigation" vAlign="top" colSpan="5">Allgemein
																	</TD>
																</tr>
																<TR>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">Fahrgestellnummer:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblFahrgestellnummer" runat="server"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">Eingang 
																		Schlüssel:&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblEingangSchluessel" runat="server"></asp:label></TD>
																</TR>
																<tr>
																	<TD class="PageNavigation" vAlign="top" colSpan="5">
																		Inhalt Tüte
																	</TD>
																</tr>
																<TR id="Tr1" runat="server">
																	<TD class="TextLargeDescription" vAlign="middle" align="left">Ersatzschlüssel:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="cbxErsatzschluessel" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">CoC-Papier:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="cbxCoCPapier" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="middle" align="left">Carpass:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:checkbox id="cbxCarpass" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="middle" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="middle" align="left">Navi-Codekarte:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:checkbox id="cbxNaviCodekarte" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">Radiocodekarte:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="cbxRadiocodekarte" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">Codekarte 
																		Wegfahrsperre:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="cbxCodekarteWFS" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="middle" align="left">CD-Navi:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:checkbox id="cbxCDNavi" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="middle" align="left"></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="middle" align="left">Ersatzfernbedienung 
																		Standheizung</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:checkbox id="cbxErsatzfernbedStandh" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">Chipkarte:</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="cbxChipkarte" runat="server" Enabled="False"></asp:checkbox></TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left"></TD>
																	<TD class="TextLargeDescription" vAlign="middle" align="left">Prüfbuch (bei LKW)</TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:checkbox id="cbxPruefbuch" runat="server" Enabled="False"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD class="PageNavigation" vAlign="top" colSpan="5">
																		Letzte Versanddaten
																	</TD>
																</TR>
																<TR>
																	<TD class="TextLargeDescription" vAlign="top" align="left"><asp:label id="lblVersendetAmDescription" runat="server">Versendet am:</asp:label></TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lblAngefordertAm" runat="server"></asp:label><asp:label id="lblVersendetAm" runat="server" Visible="False"></asp:label></TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLargeDescription" vAlign="top" align="left"><asp:label id="Label1" runat="server">Versandart:</asp:label></TD>
																	<TD class="TextLarge" vAlign="top" align="right"><asp:Label id="Label3" runat="server">   temporär</asp:Label>
																		<asp:RadioButton id="rbTemporaer" runat="server" Enabled="False" GroupName="Versandart"></asp:RadioButton><BR>
																		<asp:Label id="Label2" runat="server">   endgültig</asp:Label>
																		<asp:RadioButton id="rbEndgueltig" runat="server" Enabled="False" GroupName="Versandart"></asp:RadioButton></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">Versandanschrift:</TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right">
																		<asp:label id="lblVersandanschrift" runat="server"></asp:label></TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="StandardTableAlternateDescription" vAlign="top" align="left"></TD>
																	<TD class="StandardTableAlternate" vAlign="top" align="right"></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD><!--#include File="../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
