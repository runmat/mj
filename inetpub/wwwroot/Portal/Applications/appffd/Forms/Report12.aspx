<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report12.aspx.vb" Inherits="AppFFD.Report12" %>
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
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="150" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle"><asp:linkbutton id="lnkCreateCSV" runat="server" CssClass="StandardButton" Enabled="False"> &#149;&nbsp;CSV erzeugen</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<P><asp:calendar id="calVon" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
											<TodayDayStyle Font-Bold="True"></TodayDayStyle>
											<NextPrevStyle ForeColor="White"></NextPrevStyle>
											<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
											<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
											<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
											<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
											<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
										</asp:calendar></P>
									<P><asp:calendar id="calBis" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
											<TodayDayStyle Font-Bold="True"></TodayDayStyle>
											<NextPrevStyle ForeColor="White"></NextPrevStyle>
											<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
											<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
											<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
											<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
											<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
										</asp:calendar></P>
								</TD>
								<TD vAlign="top" width="100%">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;</TD>
										</TR>
										<TR>
											<TD class="" vAlign="top" width="100%"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" width="100%">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="middle" width="150">Datum von:</TD>
														<TD class="TextLarge" vAlign="middle"><asp:textbox id="txtDatVon" runat="server"></asp:textbox>&nbsp;<FONT color="red">*</FONT>
															<asp:linkbutton id="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" width="150">Datum bis:</TD>
														<TD class="TextLarge" vAlign="middle"><asp:textbox id="txtDatBis" runat="server"></asp:textbox>&nbsp;<FONT color="red">*</FONT>
															<asp:linkbutton id="btnCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" width="150">Dateiname:</TD>
														<TD class="TextLarge" vAlign="middle"><asp:textbox id="txtDateiname" runat="server"></asp:textbox>&nbsp;<FONT color="red">*</FONT>
														</TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" width="100%"><FONT color="#ff0000">&nbsp;&nbsp;
													<BR>
													*</FONT> Geben Sie bitte entweder einen Datumsbereich oder einen Dateinamen 
												ein. Verwenden Sie bitte Datumsangaben im Format TT.MM.YYYY.<BR>
												&nbsp;&nbsp;</TD>
										</TR>
										<TR id="trCSV" runat="server">
											<TD vAlign="top" align="left" width="100%">
												<asp:HyperLink id="lnkShowExcel" runat="server" Target="_blank">CSV-Datei</asp:HyperLink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label>
												<BR>
												&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" width="100%">
												<TABLE id="tblResult" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0" runat="server">
													<TR>
														<TD class="TextLarge" vAlign="top"><asp:listbox id="ListBox1" runat="server" CssClass="ListStyle" Width="200px" Height="420px" AutoPostBack="True"></asp:listbox></TD>
														<TD class="TextLarge" vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="KdKz" SortExpression="KdKz" HeaderText="KdKz"></asp:BoundColumn>
																	<asp:BoundColumn DataField="VIN" SortExpression="VIN" HeaderText="VIN"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Pr&#252;fziffer" SortExpression="Pr&#252;fziffer" HeaderText="Pr&#252;fziffer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Zulassung" SortExpression="Zulassung" HeaderText="Zulassung" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
