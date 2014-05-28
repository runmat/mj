<%@ Register TagPrefix="cc1" Namespace="AppUeberf.Controls" Assembly="AppUeberf" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberf01.aspx.vb" Inherits="AppUeberf.Ueberf01" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Ueberf01</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center" cellSpacing="0" cellPadding="0">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD class="PageNavigation" colSpan="2" style="HEIGHT: 18px"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
						<asp:label id="lblPageTitle" runat="server">Adressdaten</asp:label>)
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table1" cellSpacing="0" cellPadding="1" width="100%" border="0">
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 449px" colspan="4">
									<uc1:ProgressControl id="ProgressControl1" runat="server"></uc1:ProgressControl></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 449px" colspan="3"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 21px" width="144"></TD>
								<TD style="WIDTH: 449px; HEIGHT: 21px" colspan="3">
									<asp:label id="Label2" runat="server" Font-Bold="True" Width="122px" Height="17px">Abholung</asp:label></TD>
								<TD style="HEIGHT: 21px"></TD>
							</TR>
							<TR>
								<TD width="144">&nbsp;</TD>
								<TD style="WIDTH: 449px" colspan="3">
									<uc1:AddressSearchInputControl id="ctrlAddressSearchAbholung" runat="server"></uc1:AddressSearchInputControl></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">Auswahl</TD>
								<TD style="WIDTH: 245px">
									<asp:dropdownlist id="drpAbholung" runat="server" Width="220px" AutoPostBack="True"></asp:dropdownlist></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD align="left">
									<asp:CheckBox id="chkUbernahmeLeasingnehmerAbholung" runat="server" Text="Leasingnehmerdaten übernehmen" AutoPostBack="True"></asp:CheckBox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD width="120">Firma / Name*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAbName" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD>
									<asp:Label id="lblAbKundennummer" runat="server" Visible="False"></asp:Label></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">Strasse*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAbStrasse" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Nr.*</TD>
								<TD><asp:textbox id="txtAbNr" runat="server" Width="73px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">PLZ*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAbPLZ" runat="server" Width="88px" MaxLength="5"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Ort*</TD>
								<TD><asp:textbox id="txtAbOrt" runat="server" Width="299px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">Ansprechpartner*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAbAnsprechpartner" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Tel. 1:*</TD>
								<TD><asp:textbox id="txtAbTelefon1" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 245px"></TD>
								<TD style="WIDTH: 68px">Tel. 2:</TD>
								<TD>
									<asp:textbox id="txtAbTelefon2" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 245px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px" colSpan="3">
									<asp:label id="Label7" runat="server" Font-Bold="True" Width="122px" Height="17px">Anlieferung</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD colSpan="3">
									<uc1:AddressSearchInputControl id="ctrlAddressSearchAnlieferung" runat="server"></uc1:AddressSearchInputControl></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">Auswahl</TD>
								<TD style="WIDTH: 245px"><asp:dropdownlist id="drpAnlieferung" runat="server" Width="220px" AutoPostBack="True"></asp:dropdownlist></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD align="left">
									<asp:CheckBox id="chkUbernahmeLeasingnehmerAnlieferung" runat="server" Text="Leasingnehmerdaten übernehmen" AutoPostBack="True"></asp:CheckBox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">Firma / Name*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAnName" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD>
									<asp:Label id="lblAnKundennummer" runat="server" Visible="False"></asp:Label></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">Strasse*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAnStrasse" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Nr.*</TD>
								<TD><asp:textbox id="txtAnNr" runat="server" Width="73px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">PLZ*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAnPLZ" runat="server" Width="88px" MaxLength="5"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Ort*</TD>
								<TD><asp:textbox id="txtAnOrt" runat="server" Width="299px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">Ansprechpartner*</TD>
								<TD style="WIDTH: 245px"><asp:textbox id="txtAnAnsprechpartner" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Tel. 1:*</TD>
								<TD><asp:textbox id="txtAnTelefon1" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">&nbsp;</TD>
								<TD style="WIDTH: 245px"></TD>
								<TD style="WIDTH: 68px">Tel. 2:</TD>
								<TD>
									<asp:textbox id="txtAnTelefon2" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 245px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px" colSpan="4">
									<P align="left"><asp:label id="lblError" runat="server" Width="734px" Height="13px" EnableViewState="False" CssClass="TextError"></asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 245px">
									<P align="right">
										<asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Customize/Ueberf/arrowUeberfLeft.gif" Visible="False"></asp:imagebutton></P>
								</TD>
								<TD style="WIDTH: 68px"></TD>
								<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Customize/Ueberf/arrowUeberfRight.gif"></asp:imagebutton></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px">&nbsp;</TD>
								<TD style="WIDTH: 245px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="144"></TD>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 245px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD><STRONG><FONT color="red">* = Pflichtfeld</FONT></STRONG></TD>
							</TR>
						</TABLE>
						<P>&nbsp;</P>
					</TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
