<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_01.aspx.vb" Inherits="AppUeberf.Ueberfg_01" %>
<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Ueberfg_01</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
						<asp:label id="lblPageTitle" runat="server">Adressdaten</asp:label>)
					</TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE id="Table5" style="WIDTH: 1032px; HEIGHT: 27px" cellSpacing="1" cellPadding="1" width="1032" border="0">
							<TR>
								<TD style="WIDTH: 431px"></TD>
								<TD style="WIDTH: 360px"><STRONG>Schritt&nbsp;2 von 4</STRONG></TD>
								<TD></TD>
							</TR>
						</TABLE>
						&nbsp;</TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD style="WIDTH: 113px"></TD>
								<TD style="WIDTH: 449px"><asp:label id="lblKundeName1" runat="server" Font-Italic="True" Font-Bold="True" Width="312px"></asp:label></TD>
								<TD><asp:label id="lblKundeStrasse" runat="server" Font-Italic="True" Font-Bold="True" Width="272px"></asp:label></TD>
							</TR>
							<TR>
								<TD width="120"></TD>
								<TD style="WIDTH: 449px"><asp:label id="lblKundeAnsprechpartner" runat="server" Font-Italic="True" Font-Bold="True" Width="307px"></asp:label></TD>
								<TD>
									<P><asp:label id="lblKundePlzOrt" runat="server" Font-Italic="True" Font-Bold="True" Width="134px"></asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 113px">&nbsp;</TD>
								<TD style="WIDTH: 449px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 113px"></TD>
								<TD style="WIDTH: 449px"><STRONG>
										<asp:label id="Label1" runat="server" Width="122px" Height="17px">Abholung</asp:label></STRONG></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 113px"></TD>
								<TD style="WIDTH: 449px"><STRONG>
										<uc1:AddressSearchInputControl id="ctrlAddressSearchAbholung" runat="server"></uc1:AddressSearchInputControl></STRONG></TD>
								<TD></TD>
							</TR>
						</TABLE>
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD width="121" style="WIDTH: 121px"></TD>
								<TD width="120">Auswahl</TD>
								<TD style="WIDTH: 255px"><asp:dropdownlist id="drpAbholung" runat="server" Width="217px" AutoPostBack="True"></asp:dropdownlist></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="121" style="WIDTH: 121px"></TD>
								<TD width="120">Firma / Name*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbName" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">Strasse*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbStrasse" runat="server" Width="218px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Nr.*</TD>
								<TD><asp:textbox id="txtAbNr" runat="server" Width="73px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">PLZ*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbPLZ" runat="server" Width="88px" MaxLength="5"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Ort*</TD>
								<TD><asp:textbox id="txtAbOrt" runat="server" Width="299px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">Ansprechpartner*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbAnsprechpartner" runat="server" Width="223px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">1. Tel.:*</TD>
								<TD><asp:textbox id="txtAbTelefon" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">&nbsp;Fax</TD>
								<TD style="WIDTH: 255px">
									<asp:textbox id="txtAbFax" runat="server" Width="223px" MaxLength="16"></asp:textbox></TD>
								<TD style="WIDTH: 68px">2. Tel.:</TD>
								<TD>
									<asp:textbox id="txtAbTelefon2" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">&nbsp;</TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px"><STRONG>Anlieferung</STRONG></TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="121" style="WIDTH: 121px"></TD>
								<TD colSpan="3">
									<uc1:AddressSearchInputControl id="ctrlAddressSearchAnlieferung" runat="server"></uc1:AddressSearchInputControl>
								</TD>
								<TD></TD>
							</TR>
							<TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">Auswahl</TD>
								<TD style="WIDTH: 255px"><asp:dropdownlist id="drpAnlieferung" runat="server" Width="220px" AutoPostBack="True"></asp:dropdownlist></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">Firma / Name*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnName" runat="server" Width="219px"></asp:textbox></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">Strasse*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnStrasse" runat="server" Width="214px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Nr.*</TD>
								<TD><asp:textbox id="txtAnNr" runat="server" Width="73px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">PLZ*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnPLZ" runat="server" Width="88px" MaxLength="5"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Ort*</TD>
								<TD><asp:textbox id="txtAnOrt" runat="server" Width="299px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">Ansprechpartner*</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnAnsprechpartner" runat="server" Width="220px"></asp:textbox></TD>
								<TD style="WIDTH: 68px">1. Tel.:*</TD>
								<TD><asp:textbox id="txtAnTelefon" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">&nbsp;Fax</TD>
								<TD style="WIDTH: 255px">
									<asp:textbox id="txtAnFax" runat="server" Width="220px" MaxLength="16"></asp:textbox></TD>
								<TD style="WIDTH: 68px">2. Tel.:</TD>
								<TD>
									<asp:textbox id="txtAnTelefon2" runat="server" Width="299px" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">&nbsp;</TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 255px">
									<P align="right">
										<asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
								</TD>
								<TD style="WIDTH: 68px"></TD>
								<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif"></asp:imagebutton></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px">&nbsp;</TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD><STRONG><FONT color="red">* = Pflichtfeld</FONT></STRONG></TD>
							</TR>
						</TABLE>
						<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD style="WIDTH: 119px" align="center"></TD>
								<TD align="center">
									<P align="left"><asp:label id="lblError" runat="server" Width="325px" Height="19px" EnableViewState="False" CssClass="TextError"></asp:label></P>
								</TD>
							</TR>
						</TABLE>
						<P>&nbsp;</P>
					</TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
