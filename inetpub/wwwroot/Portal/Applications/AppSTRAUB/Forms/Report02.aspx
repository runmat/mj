<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report02.aspx.vb" Inherits="AppSTRAUB.Report02"%>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2" height="19">
									<asp:Label id="lblHead" runat="server"></asp:Label>
									<asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">
												&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calVon" runat="server" BorderStyle="Solid" BorderColor="Black" CellPadding="0" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:calendar id="calBis" runat="server" BorderStyle="Solid" BorderColor="Black" CellPadding="0" Width="120px" Visible="False">
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
										<TR>
											<TD class="" vAlign="top">
												<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="2" width="100%" border="0" bgColor="white">
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap>PDI:</TD>
														<TD class="TextLarge" vAlign="center" width="100%">
															<asp:DropDownList id="ddlPDIs" runat="server" CssClass="DropDownStyle"></asp:DropDownList></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap>Hersteller:</TD>
														<TD class="TextLarge" vAlign="center" width="100%">
															<asp:dropdownlist id="ddlHersteller" runat="server" CssClass="DropDownStyle"></asp:dropdownlist></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap>Lieferant:</TD>
														<TD class="TextLarge" vAlign="center" width="100%">
															<asp:TextBox id="txtLieferant" runat="server"></asp:TextBox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap>Eingangsdatum von:
														</TD>
														<TD class="TextLarge" vAlign="center" width="100%">
															<asp:TextBox id="txtErfassungsdatumVon" runat="server"></asp:TextBox>
															<asp:Label id="lblInputReq" runat="server" CssClass="TextError">*</asp:Label>&nbsp;
															<asp:LinkButton id="btnVon" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
														</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="center">
															Eingangsdatum bis :</TD>
														<TD class="StandardTableAlternate" vAlign="center">
															<asp:TextBox id="txtErfassungsdatumBis" runat="server"></asp:TextBox>
															<asp:Label id="Label1" runat="server" CssClass="TextError">*</asp:Label>&nbsp;
															<asp:LinkButton id="btnBis" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<asp:Label id="Label2" runat="server" CssClass="TextError">*</asp:Label>
									<asp:label id="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False"> Eingabe erforderlich. Format: TT.MM.JJJJ. Der maximale Zeitraum beträgt 30 Tage.</asp:label></TD>
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
