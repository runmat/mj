<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report11.aspx.vb" Inherits="AppLeaseTrend.Report11" %>
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
								<TD class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2" height="19">Bitte Suchkriterien eingeben.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE class="BorderLeftBottom" id="Table2" cellSpacing="1" cellPadding="1" border="0">
													<TR>
														<TD>
															<TABLE id="Table1" cellSpacing="0" cellPadding="2" border="0">
																<TR>
																	<TD class="TextLarge" vAlign="center" noWrap></TD>
																	<TD class="TextLarge" vAlign="center" noWrap colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="" vAlign="center" noWrap>&nbsp;</TD>
																	<TD class="" vAlign="center" noWrap>Versanddatum von*
																	</TD>
																	<TD class="TextLarge" vAlign="center" noWrap width="100%"><ew:calendarpopup id="txtErfassungsDatumVon" runat="server" Text=" " PopupLocation="Bottom" ImageUrl="/Portal/Images/calendar.gif" ControlDisplay="TextBoxImage" Nullable="True" ClearDateText=" ">
																			<TextboxLabelStyle CssClass="TextInput"></TextboxLabelStyle>
																			<WeekdayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="PowderBlue"></WeekdayStyle>
																			<MonthHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="Yellow"></MonthHeaderStyle>
																			<OffMonthStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Gray" CssClass="CalOffMonthStyle" BackColor="AntiqueWhite"></OffMonthStyle>
																			<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></GoToTodayStyle>
																			<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalTodayDayStyle" BackColor="LightGoldenrodYellow"></TodayDayStyle>
																			<DayHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalDayHeaderStyle" BackColor="Orange"></DayHeaderStyle>
																			<WeekendStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalWeekendStyle" BackColor="LightGray"></WeekendStyle>
																			<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalSelectedDateStyle" BackColor="Yellow"></SelectedDateStyle>
																			<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></ClearDateStyle>
																			<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></HolidayStyle>
																		</ew:calendarpopup></TD>
																	<TD class="TextLarge" vAlign="center" noWrap width="100%">&nbsp;
																	</TD>
																</TR>
																<TR>
																	<TD class="" vAlign="center"></TD>
																	<TD class="" vAlign="center">Versanddatum bis*</TD>
																	<TD class="" vAlign="center"><ew:calendarpopup id="txtErfassungsDatumBis" runat="server" Text=" " PopupLocation="Bottom" ImageUrl="/Portal/Images/calendar.gif" ControlDisplay="TextBoxImage" Nullable="True" ClearDateText=" ">
																			<TextboxLabelStyle CssClass="TextInput"></TextboxLabelStyle>
																			<WeekdayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="PowderBlue"></WeekdayStyle>
																			<MonthHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="Yellow"></MonthHeaderStyle>
																			<OffMonthStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Gray" CssClass="CalOffMonthStyle" BackColor="AntiqueWhite"></OffMonthStyle>
																			<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></GoToTodayStyle>
																			<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalTodayDayStyle" BackColor="LightGoldenrodYellow"></TodayDayStyle>
																			<DayHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalDayHeaderStyle" BackColor="Orange"></DayHeaderStyle>
																			<WeekendStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalWeekendStyle" BackColor="LightGray"></WeekendStyle>
																			<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" CssClass="CalSelectedDateStyle" BackColor="Yellow"></SelectedDateStyle>
																			<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></ClearDateStyle>
																			<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></HolidayStyle>
																		</ew:calendarpopup></TD>
																	<TD class="StandardTableAlternate" vAlign="center">&nbsp;
																	</TD>
																</TR>
																<TR>
																	<TD class="" vAlign="center"></TD>
																	<TD class="" vAlign="center">Leasingvertragsnummer</TD>
																	<TD class="" vAlign="center"><asp:textbox id="txtLV" runat="server" CssClass="TextInput"></asp:textbox></TD>
																	<TD class="" vAlign="center"></TD>
																</TR>
																<TR>
																	<TD class="" vAlign="center">&nbsp;</TD>
																	<TD class="" vAlign="center"></TD>
																	<TD class="" vAlign="center"></TD>
																	<TD class="" vAlign="center"></TD>
																</TR>
																<TR>
																	<TD class="" vAlign="center"></TD>
																	<TD class="" vAlign="center"></TD>
																	<TD class="" vAlign="center" colSpan="2">
																		<P align="right"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></P>
																	</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left"></TD>
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
								<TD vAlign="top"><asp:label id="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False" Font-Size="XX-Small">*Eingabe erforderlich. Format: TT.MM.JJJJ. Der Datumsbereich darf maximal einen Monat (30 Tage) umfassen.</asp:label></TD>
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
