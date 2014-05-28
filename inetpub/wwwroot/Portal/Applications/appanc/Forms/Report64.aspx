<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report64.aspx.vb" Inherits="AppANC.Report64" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta name="vs_showGrid" content="False">
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
								<TD class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<P>
										<asp:calendar id="calVon" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
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
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="center">
															Zulassungsdatum&nbsp;von:
														</TD>
														<TD class="TextLarge" vAlign="center" width="100%"><asp:textbox id="txtZulassungsdatumVon" runat="server"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:linkbutton id="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="center">
															Zulassungsdatum&nbsp;&nbsp;bis:</TD>
														<TD class="StandardTableAlternate" vAlign="center"><asp:textbox id="txtZulassungsdatumBis" runat="server"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:linkbutton id="btnCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="180">&nbsp;</TD>
														<TD class="TextLarge" vAlign="center">&nbsp;&nbsp;</TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
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
