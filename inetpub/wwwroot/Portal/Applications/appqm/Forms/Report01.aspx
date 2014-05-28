<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report01.aspx.vb" Inherits="appQM.Report01"%>
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
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calVon" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:calendar id="calBis" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
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
														<TD class="TextLarge" vAlign="middle" width="132">
															Meldedatum&nbsp;von:
														</TD>
														<TD class="TextLarge" vAlign="middle"><asp:textbox id="txtMeldedatumVon" runat="server" MaxLength="10"></asp:textbox>&nbsp;(TT.MM.JJJJ)&nbsp;
															<asp:LinkButton id="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="middle" width="132">
															Meldedatum&nbsp;bis:</TD>
														<TD class="StandardTableAlternate" vAlign="middle"><asp:textbox id="txtMeldedatumBis" runat="server" MaxLength="10"></asp:textbox>&nbsp;(TT.MM.JJJJ)&nbsp;
															<asp:LinkButton id="btnCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="middle" width="132">VKORG:</TD>
														<TD class="StandardTableAlternate" vAlign="middle">
															<asp:textbox id="txtVKORG" runat="server" MaxLength="4"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="middle" width="132">VKBUR:</TD>
														<TD class="StandardTableAlternate" vAlign="middle">
															<asp:textbox id="txtVKBUR" runat="server" MaxLength="4"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" width="132">Kundennummer:</TD>
														<TD class="TextLarge" vAlign="middle">
															<asp:textbox id="txtKunnr" runat="server" MaxLength="10"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" width="132">Status:</TD>
														<TD class="TextLarge" vAlign="middle">
															<asp:dropdownlist id="ddStatus" tabIndex="14" runat="server" Width="159px"></asp:dropdownlist></TD>
													</TR>
												</TABLE>
												<BR>
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
