<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Zulg_01.aspx.vb" Inherits="AppUeberf.Zulg_01" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD width="100%" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Fahrzeugdaten</asp:label>)</TD>
							</TR>
							<tr>
								<TD style="WIDTH: 144px" vAlign="top" width="144"><asp:calendar id="calVon" runat="server" Visible="False" BorderColor="Black" BorderStyle="Solid" CellPadding="0" Width="120px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></TD>
								<TD vAlign="top">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
										<TR>
											<TD style="WIDTH: 424px" width="424">
												<P align="right"><STRONG>Schritt&nbsp;1 von 2</STRONG></P>
											</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="lblKundeName1" runat="server" Width="225px" Font-Italic="True" Font-Bold="True"></asp:label></TD>
											<TD><asp:label id="lblKundeStrasse" runat="server" Width="278px" Font-Italic="True" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="lblKundeAnsprechpartner" runat="server" Width="307px" Font-Italic="True" Font-Bold="True"></asp:label></TD>
											<TD><asp:label id="lblKundePlzOrt" runat="server" Width="134px" Font-Italic="True" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="Label5" runat="server" Width="206px" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px; HEIGHT: 12px" width="424"></TD>
											<TD style="HEIGHT: 12px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px; HEIGHT: 12px" width="424"><asp:label id="Label1" runat="server" Width="150px" Height="22px">Haltername:*</asp:label><asp:textbox id="txtHalter" runat="server" Width="266px"></asp:textbox></TD>
											<TD style="HEIGHT: 12px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424" height="21"><asp:label id="Label3" runat="server" Width="150px" Height="22px">Halter-PLZ:*</asp:label><asp:textbox id="txtHalterPLZ" runat="server" Width="88px" MaxLength="5"></asp:textbox></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424" height="21"><asp:label id="Label6" runat="server" Width="150px" Height="22px">gew. Zulassungsdatum:</asp:label><asp:textbox id="txtAbgabetermin" runat="server" Width="88px"></asp:textbox><asp:linkbutton id="btnVon" runat="server" Width="100px" CssClass="StandardButtonTable"> &#149;&nbsp;Kalender...</asp:linkbutton></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424" height="21"></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="Label12" runat="server" Width="150px" Font-Bold="True">Zulassungskreis:</asp:label><asp:textbox id="txtZulkreis" runat="server" Width="39px" Font-Bold="True" Enabled="False"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:linkbutton id="btnZulkreis" runat="server" Width="177px" CssClass="StandardButtonTable"> &#149;&nbsp;Zulassungskeis ermitteln*</asp:linkbutton></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="Label2" runat="server" Width="150px">1. Wunschkennzeichen:</asp:label><asp:textbox id="txtKennzeichen1" runat="server" Width="39px" Enabled="False"></asp:textbox><asp:label id="Label10" runat="server" Width="11px" Font-Bold="True"> -</asp:label><asp:textbox id="txtKennZusatz1" runat="server" Width="149px"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="Label7" runat="server" Width="150px">2. Wunschkennzeichen:</asp:label><asp:textbox id="txtKennzeichen2" runat="server" Width="39px" Enabled="False"></asp:textbox><asp:label id="Label9" runat="server" Width="11px" Font-Bold="True"> -</asp:label><asp:textbox id="txtKennZusatz2" runat="server" Width="149px"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="Label8" runat="server" Width="150px">3. Wunschkennzeichen:</asp:label><asp:textbox id="txtKennzeichen3" runat="server" Width="39px" Enabled="False"></asp:textbox><asp:label id="Label11" runat="server" Width="11px" Font-Bold="True"> -</asp:label><asp:textbox id="txtKennZusatz3" runat="server" Width="149px"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424"><asp:label id="Label4" runat="server" Width="150px">Referenz-Nr.:*</asp:label><asp:textbox id="txtRef" runat="server" Width="200px"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 424px" width="424">&nbsp;</TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top">&nbsp;</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top">
									<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 324px">
												<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
											</TD>
											<TD style="WIDTH: 79px"></TD>
											<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif"></asp:imagebutton></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px">&nbsp;</TD>
											<TD style="WIDTH: 79px"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px"></TD>
											<TD style="WIDTH: 79px"><asp:label id="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="Red">*=Pflichtfeld</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px"></TD>
											<TD style="WIDTH: 79px"></TD>
											<TD></TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" Width="770px" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
