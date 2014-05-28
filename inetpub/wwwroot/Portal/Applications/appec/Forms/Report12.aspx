<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report12.aspx.vb" Inherits="AppEC.Report12" %>
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
								</TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2" height="19">Bitte geben Sie die Auswahlkriterien 
									ein.
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<asp:calendar id="calVon" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
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
											<TD class="" vAlign="top">
												<TABLE class="BorderLeftBottom" id="Table5" cellSpacing="1" cellPadding="1" border="0">
													<TR>
														<TD noWrap>&nbsp;</TD>
														<TD noWrap colSpan="3">
															<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</TR>
													<TR>
														<TD noWrap></TD>
														<TD noWrap>PDI:
														</TD>
														<TD noWrap>
															<asp:TextBox id="txtPDI" runat="server"></asp:TextBox></TD>
														<TD noWrap></TD>
													</TR>
													<TR>
														<TD noWrap></TD>
														<TD noWrap>Zulassungsdatum von:</TD>
														<TD noWrap>
															<asp:TextBox id="txtZulassungsdatumVon" runat="server"></asp:TextBox>
															<asp:Label id="lblInputReq" runat="server" CssClass="TextError">*</asp:Label></TD>
														<TD noWrap>
															<asp:LinkButton id="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD noWrap></TD>
														<TD noWrap>Zulassungsdatum bis:</TD>
														<TD noWrap>
															<asp:TextBox id="txtZulassungsdatumBis" runat="server"></asp:TextBox>
															<asp:Label id="Label1" runat="server" CssClass="TextError">*</asp:Label></TD>
														<TD noWrap>
															<asp:LinkButton id="LinkButton1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD noWrap>&nbsp;</TD>
														<TD noWrap></TD>
														<TD noWrap></TD>
														<TD noWrap></TD>
													</TR>
													<TR>
														<TD noWrap></TD>
														<TD noWrap></TD>
														<TD noWrap></TD>
														<TD noWrap>
															<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton></TD>
													</TR>
												</TABLE>
												<asp:Label id="Label2" runat="server" CssClass="TextError">*</asp:Label>
												<asp:label id="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False"> Eingabe erforderlich. Format: TT.MM.JJJJ.</asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
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
