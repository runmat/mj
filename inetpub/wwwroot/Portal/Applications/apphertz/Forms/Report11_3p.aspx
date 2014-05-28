<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report11_3p.aspx.vb" Inherits="AppHERTZ.Report11_3p" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
			<uc1:header id="ucHeader" runat="server" DESIGNTIMEDRAGDROP="167" Visible="False"></uc1:header>
			<TABLE id="Table10" cellSpacing="0" cellPadding="5" border="0" bgColor="white" width="700">
				<TR>
					<TD class="" vAlign="center" align="left" colSpan="5">
						<asp:Label id="lblHead" runat="server" Visible="False"></asp:Label></TD>
				</TR>
				<TR>
					<TD vAlign="center" align="left" colSpan="5"><STRONG>Fahrzeugdaten
							<HR width="100%" SIZE="1">
						</STRONG>
					</TD>
				</TR>
				<TR>
					<TD class="" vAlign="center" align="left">Status:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblStatus" runat="server"></asp:label></TD>
					<TD class="" vAlign="center" align="left" colSpan="3" width="100%">
						<asp:Label id="lblMindestlaufzeitDescription" runat="server" Visible="False">Mindestlaufzeit:</asp:Label></TD>
				</TR>
				<TR>
					<TD class="TextLargeDescription" vAlign="center" align="left">Kennzeichen:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblKennzeichen" runat="server"></asp:label></TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">Briefnummer:&nbsp;</TD>
					<TD class="TextLarge" vAlign="top" align="left" width="100%">
						<asp:label id="lblBriefnummer" runat="server"></asp:label></TD>
				</TR>
				<TR id="Tr5" runat="server">
					<TD class="" vAlign="center" align="left">Fahrgestellnummer:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblFahrgestellnummer" runat="server"></asp:label></TD>
					<TD class="" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="" vAlign="center" align="left">Briefeingang:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblBriefeingang" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="TextLargeDescription" vAlign="center" align="left">Hersteller:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblHersteller" runat="server"></asp:Label></TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">Fahrzeugmodell:&nbsp;</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblFahrzeugmodell" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="" vAlign="top" align="left">Eingangsdatum PDI:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblEingangsdatum" runat="server"></asp:Label></TD>
					<TD class="" vAlign="center" align="left"></TD>
					<TD class="" vAlign="top" align="left">Fahrzeughalter:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblFahrzeughalter" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="" vAlign="top" colSpan="5"><STRONG>Briefdaten
							<HR width="100%" SIZE="1">
						</STRONG>
					</TD>
				</TR>
				<TR id="Tr1" runat="server">
					<TD class="TextLargeDescription" vAlign="center" align="left">Erstzulassungsdatum:&nbsp;</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblErstzulassungsdatum" runat="server"></asp:label></TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="TextLargeDescription" vAlign="center" align="left" noWrap>Ordernummer:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblMindestlaufzeit" runat="server" Visible="False"></asp:Label>
						<asp:label id="lblOrdernummer" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="" vAlign="center" align="left" noWrap>Ehemaliges Kennzeichen:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblEhemaligesKennzeichen" runat="server"></asp:label></TD>
					<TD class="" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="" vAlign="center" align="left">Umgemeldet am:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblUmgemeldetAm" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="TextLargeDescription" vAlign="center" align="left">Ehemalige 
						Briefnummer:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblEhemaligeBriefnummer" runat="server"></asp:Label></TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">Briefaufbietung:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:CheckBox id="chkBriefaufbietung" runat="server" Enabled="False" Visible="False"></asp:CheckBox>
						<asp:Label id="lblBriefAufbietung" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="" vAlign="top" colSpan="5"><STRONG>Abmeldedaten</STRONG>&nbsp;
						<HR width="100%" SIZE="1">
					</TD>
				</TR>
				<TR>
					<TD class="TextLargeDescription" vAlign="center" align="left">Carport-Eingang:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblPDIEingang" runat="server"></asp:Label></TD>
					<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
					<TD class="TextLargeDescription" vAlign="center" align="left">Kennzeicheneingang:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblKennzeicheneingang" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="" vAlign="center" align="left">Check-In:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:Label id="lblCheckIn" runat="server"></asp:Label></TD>
					<TD class="" vAlign="center" align="left"></TD>
					<TD class="" vAlign="center" align="left">Fahrzeugschein:&nbsp;</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:CheckBox id="chkFahzeugschein" runat="server" Enabled="False" Visible="False"></asp:CheckBox>
						<asp:Label id="lblFahrzeugschein" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="TextLargeDescription" vAlign="center" align="left">Beide Kennzeichen 
						vorhanden:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:CheckBox id="chkVorhandeneElemente" runat="server" Enabled="False" Visible="False"></asp:CheckBox>
						<asp:Label id="lblBeideKennzeichen" runat="server"></asp:Label></TD>
					<TD class="TextLargeDescription" vAlign="center" align="left"></TD>
					<TD class="TextLargeDescription" vAlign="top" align="left">Stilllegung:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblStillegung" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="" vAlign="top" colSpan="5"><STRONG>Letzte Versanddaten</STRONG>
						<HR width="100%" SIZE="1">
					</TD>
				</TR>
				<TR>
					<TD class="TextLargeDescription" vAlign="top" align="left">
						<asp:Label id="lblVersendetAmDescription" runat="server">Versendet am:</asp:Label></TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblAngefordertAm" runat="server"></asp:label>
						<asp:Label id="lblVersendetAm" runat="server" Visible="False"></asp:Label></TD>
					<TD class="TextLargeDescription" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="TextLargeDescription" vAlign="top" align="left">
						<asp:Label id="lblVersandart" runat="server"> Versandart:</asp:Label></TD>
					<TD class="TextLarge" vAlign="top" align="left" noWrap>
						<asp:Label id="Label1" runat="server" Visible="False">   temporär</asp:Label>
						<asp:Label id="Label2" runat="server" Visible="False">   endgültig</asp:Label></TD>
				</TR>
				<TR>
					<TD class="" vAlign="top" align="left">Versandanschrift:</TD>
					<TD class="TextLarge" vAlign="top" align="left">
						<asp:label id="lblVersandanschrift" runat="server"></asp:label></TD>
					<TD class="" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;</TD>
					<TD class="" vAlign="top" align="left">
						<asp:RadioButton id="rbTemporaer" runat="server" Enabled="False" GroupName="Versandart" Visible="False"></asp:RadioButton></TD>
					<TD class="" vAlign="top" align="right">
						<asp:RadioButton id="rbEndgueltig" runat="server" Enabled="False" GroupName="Versandart" Visible="False"></asp:RadioButton></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
