<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report24.aspx.vb" Inherits="AppEC.Report24" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>



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
									ein.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
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
											<TD class="" vAlign="top">
												<TABLE class="BorderLeftBottom" id="Table2" cellSpacing="1" cellPadding="1" border="0">
													<TR>
														<TD></TD>
														<TD colSpan="3">
															<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>&nbsp;</TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Zulaufdatum Von:
														</TD>
														<TD>
															<asp:TextBox id="txt_ErfassungsdatumVon" runat="server" ToolTip="Zulaufdatum Von"></asp:TextBox>
															<asp:Label id="lblInputReq" runat="server" CssClass="TextError">*</asp:Label></TD>
														<TD noWrap>
															<asp:LinkButton id="btnVon" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Zulaufdatum Bis:
														</TD>
														<TD>
															<asp:TextBox id="txt_ErfassungsdatumBis" runat="server" ToolTip="Zulaufdatum Bis"></asp:TextBox>
															<asp:Label id="Label1" runat="server" CssClass="TextError">*</asp:Label></TD>
														<TD noWrap>
															<asp:LinkButton id="lb_Bis" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Hersteller:
														</TD>
														<TD>
															<asp:DropDownList id="ddl_Hersteller" runat="server"></asp:DropDownList>
														</TD>
														<TD noWrap>
														</TD>
													</TR>
													<TR>
														<TD>&nbsp;&nbsp;</TD>
														<TD></TD>
														<TD></TD>
														<TD></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD></TD>
														<TD></TD>
														<TD>
															<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<asp:Label id="Label2" runat="server" CssClass="TextError">*</asp:Label>
												<asp:label id="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False"> Eingabe erforderlich(max. 180 Tage). Format: TT.MM.JJJJ</asp:label>
											</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left">
												&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left">
												&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left">
												<asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Ausgabe in Excel...</asp:hyperlink>
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">oder rechte Maustaste => Ziel speichern unter...</asp:label>
											</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left">
												<asp:hyperlink id="Hyperlink1" runat="server" Target="_blank" Visible="False">Druckausgabe in HTML...</asp:hyperlink>
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