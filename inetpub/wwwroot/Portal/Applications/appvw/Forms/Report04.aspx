<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report04.aspx.vb" Inherits="AppVW.Report04" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
							<TBODY>
								<TR>
									<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
								</TR>
								<TR>
									<TD class="TaskTitle" vAlign="top" colSpan="2">&nbsp;
										<asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;&nbsp;<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">oder rechte Maustaste => Ziel speichern unter...</asp:label></TD>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="120">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
							<TR id="trCreate" runat="server">
								<TD vAlign="center" width="150"><asp:linkbutton id="cmdSelection" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
							</TR>
						</TABLE>
						<asp:calendar id="calVon" runat="server" Width="120px" Visible="False" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
							<TodayDayStyle Font-Bold="True"></TodayDayStyle>
							<NextPrevStyle ForeColor="White"></NextPrevStyle>
							<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
							<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
							<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
							<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
							<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
						</asp:calendar><asp:calendar id="calBis" runat="server" Width="120px" Visible="False" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
							<TodayDayStyle Font-Bold="True"></TodayDayStyle>
							<NextPrevStyle ForeColor="White"></NextPrevStyle>
							<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
							<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
							<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
							<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
							<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
						</asp:calendar></TD>
					<TD vAlign="top">
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<TD vAlign="top" align="left">
										<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
											<tr>
												<td class="TextLarge" width="162" height="33">
													<P align="right">Vorhabennummer:&nbsp;&nbsp;
													</P>
												</td>
												<td height="33"><asp:textbox id="txtVorhabennummer" runat="server" Width="154px"></asp:textbox></td>
											</tr>
											<tr>
												<td class="TextLarge" width="162">
													<P align="right">IKZ-Nummer:&nbsp;&nbsp;
													</P>
												</td>
												<td class="TextLarge"><asp:textbox id="txtIKZNummer" runat="server" Width="155px"></asp:textbox></td>
											</tr>
											<TR>
												<TD class="TextLarge" vAlign="center" width="162">
													<P align="right">Übergabedatum&nbsp;IST von:&nbsp;
													</P>
												</TD>
												<TD class="TextLarge" vAlign="center"><asp:textbox id="txtUebergabedatumVon" runat="server" ToolTip="Übergabedatum IST von" MaxLength="10"></asp:textbox>&nbsp;
													<asp:linkbutton id="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton>&nbsp;&nbsp; 
													(TT.MM.JJJJ)</TD>
											</TR>
											<TR>
												<TD class="TextLarge" vAlign="center" width="162">
													<P align="right">Übergabedatum IST&nbsp;bis:&nbsp;&nbsp;
													</P>
												</TD>
												<TD class="TextLarge" vAlign="center"><asp:textbox id="txtUebergabedatumBis" runat="server" ToolTip="Übergabedatum IST bis" MaxLength="10"></asp:textbox>&nbsp;
													<asp:linkbutton id="btnCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton>&nbsp;&nbsp; 
													(TT.MM.JJJJ)</TD>
											</TR>
											<TR>
												<TD class="TextLarge" vAlign="center" width="162">
													<P align="right">Rechnungsnummern:&nbsp;&nbsp;
													</P>
												</TD>
												<TD class="TextLarge" vAlign="center"><asp:textbox id="txtRechnungsnummern" runat="server" Width="369px"></asp:textbox>(mehrere 
													durch ',' getrennt möglich, (123,142,412....)
												</TD>
											</TR>
										</TABLE>
										<TABLE id="TableXX" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR id="trSelectDropdown" runat="server">
												<TD class="TextLarge" vAlign="top" align="right" width="150"></TD>
												<TD class="TextLarge" vAlign="center"></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TBODY>
						</TABLE>
						<asp:datagrid id="DataGrid1" runat="server" Width="100%" CellPadding="2" AutoGenerateColumns="False" bodyHeight="450" BackColor="White" AllowSorting="True" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader">
							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
							<HeaderStyle HorizontalAlign="Left" CssClass="GridTableHead"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Rechnungsnummer" SortExpression="Rechnungsnummer" HeaderText="Rechnungsnummer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Eingang Lieferschein" SortExpression="Eingang Lieferschein" HeaderText="Eingang Lieferschein"></asp:BoundColumn>
								<asp:BoundColumn DataField="&#220;bergabedatum SOLL" SortExpression="&#220;bergabedatum SOLL" HeaderText="&#220;bergabedatum SOLL"></asp:BoundColumn>
								<asp:BoundColumn DataField="&#220;bergabedatum IST" SortExpression="&#220;bergabedatum IST" HeaderText="&#220;bergabedatum IST"></asp:BoundColumn>
								<asp:BoundColumn DataField="IKZ-Nummer" SortExpression="IKZ-Nummer" HeaderText="IKZ-Nummer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
								<asp:BoundColumn DataField="Zulassungsdatum" SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum"></asp:BoundColumn>
								<asp:BoundColumn DataField="Vorhabennummer" SortExpression="Vorhabennummer" HeaderText="Vorhabennummer"></asp:BoundColumn>
							</Columns>
							<PagerStyle Position="Top" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD vAlign="top" width="150">&nbsp;</TD>
					<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="top">&nbsp;</TD>
					<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
