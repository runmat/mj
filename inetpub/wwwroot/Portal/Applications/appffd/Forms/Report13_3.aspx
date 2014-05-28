<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report13_3.aspx.vb" Inherits="AppFFD.Report13_3" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
			<table width="100%" align="center" cellpadding="2" cellspacing="0">
				<tr>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>
									<asp:label id="lblPageTitle" runat="server"> (Vertragsstatus)</asp:label>
								</td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">
												&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Report13_2.aspx">Vertragssuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3">
											</td>
										</tr>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white">
										<TR>
											<TD class="" vAlign="center" align="left" width="150">Händlernummer:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblHaendlernummer" runat="server"></asp:Label></TD>
											<TD class="" vAlign="center" align="left" width="150">Fahrgestellnummer:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblFahrgestellnummer" runat="server"></asp:Label></TD>
										</TR>
										<TR id="Tr1" runat="server">
											<TD class="" vAlign="center" align="left" width="150" height="30">Vertragsnummer:&nbsp;</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblVertragsnummer" runat="server"></asp:Label></TD>
											<TD class="" vAlign="center" align="left" width="150">Ordernummer:&nbsp;</TD>
											<TD class="TextLarge" vAlign="top" align="left" height="30">
												<asp:Label id="lblOrdernummer" runat="server"></asp:Label></TD>
										</TR>
										<TR>
											<TD class="" vAlign="center" align="left" width="150">Kfz-Briefnummer:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblBriefnummer" runat="server"></asp:Label></TD>
											<TD class="" vAlign="center" align="left" width="150">Kfz-Kennzeichen:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblKennzeichen" runat="server"></asp:Label></TD>
										</TR>
										<TR>
											<TD class="" vAlign="center" align="left" width="150">Finanzierungsart:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblFinanzierungsart" runat="server"></asp:Label></TD>
											<TD class="" vAlign="center" align="left" width="150">Mahnverfahren:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblMahnverfahren" runat="server"></asp:Label></TD>
										</TR>
										<TR>
											<TD class="" vAlign="center" align="left" width="150">Bezahlt:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:CheckBox id="chkBezahlt" runat="server" Enabled="False"></asp:CheckBox></TD>
											<TD class="" vAlign="center" align="left" width="150">Abrechnungsdatum:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblAbrechnungsdatum" runat="server"></asp:Label></TD>
										</TR>
										<TR>
											<TD class="" vAlign="center" align="left" width="150" noWrap>COC-Besch. vorhanden:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:CheckBox id="cbxCOCBesch" runat="server" Enabled="False"></asp:CheckBox></TD>
											<TD class="" vAlign="top" align="left" width="150">Kunde:</TD>
											<TD class="TextLarge" vAlign="top" align="left" width="100%">
												<asp:Label id="lblKunde" runat="server"></asp:Label></TD>
										</TR>
										<TR id="TrBetrag" runat="server" Visible="False">
											<TD vAlign="middle" noWrap align="left" width="150">Betrag:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:Label id="lblBetrag" runat="server" Visible="False"></asp:Label></TD>
											<TD vAlign="top" align="left" width="150"></TD>
											<TD class="TextLarge" vAlign="top" align="left" width="100%"></TD>
										</TR>
										<TR>
											<TD class="PageNavigation" vAlign="middle" align="left" width="150" colSpan="4">Anforderung&nbsp;&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD class="" vAlign="middle" align="left" width="150">Angefordert:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:RadioButton id="chkJa" runat="server" Enabled="False" GroupName="Bezahlt" Text="Ja"></asp:RadioButton>&nbsp;&nbsp;
												<asp:RadioButton id="chkNein" runat="server" Enabled="False" GroupName="Bezahlt" Text="Nein"></asp:RadioButton>
											</TD>
											<TD class="" vAlign="center" align="left" width="150">Anforderungsdatum:</TD>
											<TD class="TextLarge" vAlign="top" align="left"><asp:Label id="lblAnforderungsdatum" runat="server"></asp:Label></TD>
										</TR>
										<TR>
											<TD class="PageNavigation" vAlign="center" align="left" width="150" colSpan="4">Versand</TD>
										</TR>
										<TR>
											<TD class="" vAlign="center" align="left" width="150">Versendet:</TD>
											<TD class="TextLarge" vAlign="top" align="left">
												<asp:RadioButton id="chkVersJa" runat="server" Enabled="False" GroupName="Versendet" Text="Ja"></asp:RadioButton>&nbsp;&nbsp;&nbsp;
												<asp:RadioButton id="chkVersNein" runat="server" Enabled="False" GroupName="Versendet" Text="Nein"></asp:RadioButton>
											</TD>
											<TD class="" vAlign="center" align="left" width="150">Versanddatum:</TD>
											<TD class="TextLarge" vAlign="top" align="left"><asp:Label id="lblVersanddatum" runat="server"></asp:Label></TD>
										</TR>
										<TR id="Tr2" runat="server">
											<TD class="" vAlign="center" align="left" width="150">Versandart:</TD>
											<TD class="TextLarge" vAlign="top" align="left" colspan="3">
												<asp:radiobutton id="chkTemporaer" runat="server" Enabled="False" GroupName="grpAuftragsart" Text="Temporär"></asp:radiobutton>&nbsp;&nbsp;
												<asp:radiobutton id="chkEntgueltig" runat="server" Enabled="False" GroupName="grpAuftragsart" Text="Endgültig" Width="85px"></asp:radiobutton>
												<asp:radiobutton id="chkRetail" runat="server" Enabled="False" Text="Retail" GroupName="grpAuftragsart"></asp:radiobutton>&nbsp;&nbsp;
												<asp:radiobutton id="chkDelayPayment" runat="server" Enabled="False" Text="Erweitertes Zahlungsziel (Delayed Payment) endgültig" GroupName="grpAuftragsart" Visible="False"></asp:radiobutton>
											</TD>
										</TR>
										<TR>
											<TD class="" vAlign="top" align="left" width="150">Versandadresse:</TD>
											<TD class="TextLarge" vAlign="top" align="left" colspan="3">
												<asp:Label id="lblVersandadresse" runat="server"></asp:Label>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" class="LabelExtraLarge">
												<asp:Label id="lblNoData" runat="server" Visible="False"></asp:Label></TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3"></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td>&nbsp;</td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
