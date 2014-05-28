<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change09Edit.aspx.vb" Inherits="AppCSC.Change09Edit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body topmargin="0" leftmargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" noWrap colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>
									<asp:label id="lblPageTitle" runat="server"> - Werte ändern</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" class="TaskTitle" width="59">&nbsp;</TD>
								<TD vAlign="top" class="TaskTitle">
									<asp:hyperlink id="lnk" runat="server" NavigateUrl="Change09.aspx">Fahrzeugsuche</asp:hyperlink></TD>
							</TR>
							<tr>
								<TD vAlign="top" width="59">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="" height="20">
												<asp:LinkButton id="cmdConfirm" runat="server" CssClass="StandardButton">&#149;&nbsp;Anfordern</asp:LinkButton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="5" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD>
												<TABLE id="Table4" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white">
													<TR>
														<TD class="TextLarge" width="150">Kennzeichen:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge">
															<asp:label id="txtKennzeichen" runat="server" Width="350px" MaxLength="35"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" width="150">Vertragsnummer:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate">
															<asp:label id="txtVertragsnummer" runat="server" Width="350px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" width="150">Fahrgestellnummer:</TD>
														<TD class="TextLarge">
															<asp:label id="txtFahrgestellnummer" runat="server" Width="350px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" width="150">Brief-Nr:</TD>
														<TD class="StandardTableAlternate">
															<asp:label id="txtBriefNr" runat="server" Width="350px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" width="150">Label:</TD>
														<TD class="TextLarge">
															<asp:label id="txtLabel" runat="server" Width="350px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" width="150">aktuelles Versanddatum:</TD>
														<TD class="StandardTableAlternate">
															<asp:label id="txtVersanddatum" runat="server" Width="350px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" width="150">Modellbezeichnung:</TD>
														<TD class="TextLarge">
															<asp:label id="txtModell" runat="server" Width="350px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" width="150">
															CoC&nbsp;vorhanden:</TD>
														<TD class="StandardTableAlternate" vAlign="top">
															<asp:CheckBox id="cbxCOCBescheinigungVorhanden" runat="server" Enabled="False"></asp:CheckBox></TD>
													</TR>
													<TR id="rowFaxnummer" runat="server">
														<TD class="StandardTableAlternate" width="150">Faxnummer:</TD>
														<TD class="StandardTableAlternate">
															<asp:textbox id="txtFaxnummer" runat="server" Width="350px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="rowMailadresse" runat="server">
														<TD class="TextLarge" width="150">
															Mailadresse:</TD>
														<TD class="TextLarge">
															<asp:textbox id="txtMailadresse" runat="server" Width="350px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="rowZusatz" runat="server">
														<TD class="StandardTableAlternate" width="150">Zusatz:</TD>
														<TD class="StandardTableAlternate">
															<asp:CheckBox id="cbxZusatz" runat="server"></asp:CheckBox></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
							<tr>
								<td width="59"></td>
								<td height="17"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></td>
							</tr>
							<tr>
								<td width="59"></td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,Kennzeichen,Ordernummer,Angefordert,Versendet) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\n\tKfz-Kennzeichen\t" + Kennzeichen + "\t\n\tOrdernummer\t" + Ordernummer + "\t\n\tAngefordert\t" + Angefordert + "\t\n\tVersendet\t" + Versendet);
						return (Check);
						}
						//-->
						</script>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
