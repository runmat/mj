<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change02.aspx.vb" Inherits="AppKruell.Change02" %>
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
									<TD class="TaskTitle" vAlign="top" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="120">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
							<TR id="trCreate" runat="server">
								<TD vAlign="center" width="150">
									<P><asp:linkbutton id="lb_Suche" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suche</asp:linkbutton>
									<br>
									<asp:linkbutton id="lb_Senden" runat="server" CssClass="StandardButton" Enabled="False"> &#149;&nbsp;Senden</asp:linkbutton></P>
									<P><br>
										&nbsp;
									</P>
								</TD>
							</TR>
						</TABLE>
						<asp:calendar id="calVon" runat="server" BorderStyle="Solid" BorderColor="Black" Visible="False" Width="120px" CellPadding="0">
							<TodayDayStyle Font-Bold="True"></TodayDayStyle>
							<NextPrevStyle ForeColor="White"></NextPrevStyle>
							<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
							<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
							<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
							<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
							<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
						</asp:calendar><asp:calendar id="calBis" runat="server" BorderStyle="Solid" BorderColor="Black" Visible="False" Width="120px" CellPadding="0">
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
												<td class="TextLarge" width="264" height="33">
													<P align="right">Fahrgestellnummer:&nbsp;&nbsp;
													</P>
												</td>
												<td height="33"><asp:textbox id="txt_Fahrgestellnummer" runat="server" Width="154px"></asp:textbox></td>
											</tr>
											<tr>
												<td class="TextLarge" width="264">
													<P align="right">Ordernummer:&nbsp;&nbsp;
													</P>
												</td>
												<td class="TextLarge"><asp:textbox id="txt_Ordernummer" runat="server" Width="155px"></asp:textbox></td>
											</tr>
											<TR>
												<TD class="TextLarge" vAlign="center" width="264">
													<P align="right">Anlagedatum Auftrag von:&nbsp;
													</P>
												</TD>
												<TD class="TextLarge" vAlign="center"><asp:textbox id="txt_DatumVonAnlage" runat="server" ToolTip="Zeitfenster Von" MaxLength="10"></asp:textbox>&nbsp;
													<asp:linkbutton id="btnAnlageCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton>&nbsp;&nbsp; 
													(TT.MM.JJJJ)</TD>
											</TR>
											<TR>
												<TD class="TextLarge" vAlign="center" width="264">
													<P align="right">Anlagedatum Auftrag bis:&nbsp;&nbsp;
													</P>
												</TD>
												<TD class="TextLarge" vAlign="center"><asp:textbox id="txt_DatumBisAnlage" runat="server" ToolTip="Zeitfenster Bis" MaxLength="10" DESIGNTIMEDRAGDROP="139"></asp:textbox>&nbsp;
													<asp:linkbutton id="btnAnlageCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton>&nbsp;&nbsp; 
													(TT.MM.JJJJ)</TD>
											</TR>
											<TR>
												<TD class="TextLarge" vAlign="center" width="264">
													<P align="right">Stornierungsdatum&nbsp;von:&nbsp;
													</P>
												</TD>
												<TD class="TextLarge" vAlign="center"><asp:textbox id="txt_DatumVonStornierung" runat="server" ToolTip="Zeitfenster Von" MaxLength="10"></asp:textbox>&nbsp;
													<asp:linkbutton id="btnStornierungCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton>&nbsp;&nbsp; 
													(TT.MM.JJJJ)</TD>
											</TR>
											<TR>
												<TD class="TextLarge" vAlign="center" width="264">
													<P align="right">Stornierungsdatum&nbsp; bis:&nbsp;&nbsp;
													</P>
												</TD>
												<TD class="TextLarge" vAlign="center"><asp:textbox id="txt_DatumBisStornierung" runat="server" ToolTip="Zeitfenster Bis" MaxLength="10" DESIGNTIMEDRAGDROP="139"></asp:textbox>&nbsp;
													<asp:linkbutton id="btnStornierungCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton>&nbsp;&nbsp; 
													(TT.MM.JJJJ)</TD>
											</TR>
										</TABLE>
										<TABLE id="TableAnzeige" cellSpacing="0" cellPadding="0" width="100%">
											<tr>
												<td></td>
												<td></td>
											</tr>
											<TR id="trSelectDropdown" runat="server">
												<TD class="TextLarge" vAlign="top" align="right" width="274">Auswahl 
													Aufträge:&nbsp;&nbsp;</TD>
												<TD class="TextLarge" vAlign="center">&nbsp;&nbsp;<asp:label id="lbl_Auswahl" Runat="server"></asp:label></TD>
											</TR>
											<TR id="Tr1" runat="server">
												<TD class="TextLarge" vAlign="top" align="right" width="274">Wiederhergestellte 
													Aufträge:&nbsp;&nbsp;</TD>
												<TD class="TextLarge" vAlign="center">&nbsp;&nbsp;<asp:label id="lbl_Wiederhergestellt" Runat="server"></asp:label></TD>
											</TR>
											<TR id="Tr2" runat="server">
												<TD class="TextLarge" vAlign="top" align="right" width="274">Fehlgeschlagene 
													Aufträge(Order Nummer):&nbsp;&nbsp;</TD>
												<TD class="TextLarge" vAlign="center">&nbsp;&nbsp;<asp:label id="lbl_Fehlgeschlagen" Runat="server"></asp:label></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TBODY></TABLE>
						<asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" AllowSorting="True">
							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
							<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Wiederherstellen">
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox ID="chbx_Wiederherstellen" Runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Order Nummer" SortExpression="Order Nummer" HeaderText="Order Nummer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fahrzeug Typ" SortExpression="Fahrzeug Typ" HeaderText="Fahrzeug Typ"></asp:BoundColumn>
								<asp:BoundColumn DataField="Leasingnehmer" SortExpression="Leasingnehmer" HeaderText="Leasingnehmer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Leasinggeber" SortExpression="Leasinggeber" HeaderText="Leasinggeber"></asp:BoundColumn>
								<asp:BoundColumn DataField="Erfassungsdatum" SortExpression="Erfassungsdatum" HeaderText="Erfassungsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
								<asp:BoundColumn DataField="L&#246;schdatum" SortExpression="L&#246;schdatum" HeaderText="L&#246;schdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Gel&#246;scht durch" SortExpression="Gel&#246;scht durch" HeaderText="Gel&#246;scht durch"></asp:BoundColumn>
							</Columns>
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
