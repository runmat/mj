<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_00.aspx.vb" Inherits="AppUeberf.Ueberfg_00"%>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
								<TD style="WIDTH: 360px"><STRONG>Schritt 1 von 4</STRONG></TD>
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
								<TD style="WIDTH: 428px"><asp:label id="lblKundeName1" runat="server" Width="312px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD><asp:label id="lblKundeStrasse" runat="server" Width="272px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD width="120"></TD>
								<TD style="WIDTH: 428px"><asp:label id="lblKundeAnsprechpartner" runat="server" Width="307px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD>
									<P><asp:label id="lblKundePlzOrt" runat="server" Width="134px" Font-Bold="True" Font-Italic="True"></asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 113px">&nbsp;</TD>
								<TD style="WIDTH: 428px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 113px"></TD>
								<TD style="WIDTH: 428px"></TD>
								<TD></TD>
							</TR>
						</TABLE>
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD width="120"></TD>
								<TD style="WIDTH: 150px" width="150"><STRONG>Rechnungszahler</STRONG></TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="120"></TD>
								<TD style="WIDTH: 150px" width="150">Auswahl</TD>
								<TD style="WIDTH: 255px"><asp:dropdownlist id="drpRegulierer" runat="server" Width="217px"></asp:dropdownlist></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="120"></TD>
								<TD style="WIDTH: 150px" width="150">Firma / Name</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbName" runat="server" Width="220px" Enabled="False"></asp:textbox></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">Strasse</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbStrasse" runat="server" Width="218px" Enabled="False"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Nr.</TD>
								<TD><asp:textbox id="txtAbNr" runat="server" Width="73px" Enabled="False"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">PLZ</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbPLZ" runat="server" Width="88px" Enabled="False" MaxLength="5"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Ort</TD>
								<TD><asp:textbox id="txtAbOrt" runat="server" Width="299px" Enabled="False"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">Ansprechpartner</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAbAnsprechpartner" runat="server" Width="223px" Enabled="False"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Tel.:</TD>
								<TD><asp:textbox id="txtAbTelefon" runat="server" Width="299px" Enabled="False" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">&nbsp;</TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 121px"></TD>
								<TD style="WIDTH: 150px"><STRONG>Postalischer Rechnungsempfänger</STRONG></TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">Auswahl</TD>
								<TD style="WIDTH: 255px"><asp:dropdownlist id="drpRechnungsempf" runat="server" Width="220px"></asp:dropdownlist></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">Firma / Name</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnName" runat="server" Width="219px" Enabled="False"></asp:textbox></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">Strasse</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnStrasse" runat="server" Width="214px" Enabled="False"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Nr.</TD>
								<TD><asp:textbox id="txtAnNr" runat="server" Width="73px" Enabled="False"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">PLZ</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnPLZ" runat="server" Width="88px" Enabled="False" MaxLength="5"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Ort</TD>
								<TD><asp:textbox id="txtAnOrt" runat="server" Width="299px" Enabled="False"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">Ansprechpartner</TD>
								<TD style="WIDTH: 255px"><asp:textbox id="txtAnAnsprechpartner" runat="server" Width="220px" Enabled="False"></asp:textbox></TD>
								<TD style="WIDTH: 68px">Tel.:</TD>
								<TD><asp:textbox id="txtAnTelefon" runat="server" Width="299px" Enabled="False" MaxLength="16"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">&nbsp;</TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px"></TD>
								<TD style="WIDTH: 255px">
									<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Visible="False" ImageUrl="/Portal/Images/arrowUeberfLeft.gif" Height="34px"></asp:imagebutton></P>
								</TD>
								<TD style="WIDTH: 68px"></TD>
								<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" 
                                        ImageUrl="/Portal/Images/arrowUeberfRight.gif" Height="34px"></asp:imagebutton></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px">&nbsp;</TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 108px"></TD>
								<TD style="WIDTH: 150px"></TD>
								<TD style="WIDTH: 255px"></TD>
								<TD style="WIDTH: 68px"></TD>
								<TD><STRONG><FONT color="red"></FONT></STRONG></TD>
							</TR>
						</TABLE>
						<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD style="WIDTH: 119px" align="center"></TD>
								<TD align="center">
									<P align="left"><asp:label id="lblError" runat="server" Width="325px" Height="19px" CssClass="TextError" EnableViewState="False"></asp:label></P>
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
