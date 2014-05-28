<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change12.aspx.vb" Inherits="AppCSC.Change12" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" noWrap colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server">: Vorgangssuche</asp:label>
								</td>
							</TR>
							<tr>
								<TD vAlign="top" width="57">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="175">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="175">
												<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="120" border="0">
													<TR>
														<TD>
															<asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<tr id="ShowCalendar" runat="server">
											<TD vAlign="center" width="175">
											</TD>
										</tr>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" border="0" bgColor="white">
													<TR>
														<TD class="TextLarge"></TD>
														<TD class="TextLarge">Anzahl</TD>
														<%--<td rowspan="11" valign="bottom">
															<asp:Calendar id="calAbDatum" runat="server" Visible="False" CellPadding="0" BorderStyle="Solid" BorderColor="Black" Width="160px">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:Calendar>
															<asp:Calendar id="calBisDatum" runat="server" Visible="False" CellPadding="0" BorderStyle="Solid" BorderColor="Black" Width="160px">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:Calendar></td>--%>
													</TR>
													<TR>
														<TD class="StandardTableAlternate">
															<asp:RadioButton id="chk080" runat="server" Text="Brief nicht bekannt" GroupName="grpAuswahl" AutoPostBack="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate">
															<asp:Label id="lbl080" runat="server"></asp:Label></TD>
													</TR>
													<TR>
														<TD class="TextLarge">
															<asp:RadioButton id="chk081" runat="server" Text="Versandsperre" GroupName="grpAuswahl" AutoPostBack="True"></asp:RadioButton></TD>
														<TD class="TextLarge">
															<asp:Label id="lbl081" runat="server"></asp:Label></TD>
													</TR>
													<TR id="ShowUnused" runat="server">
														<TD class="TextLarge">
															<asp:RadioButton id="chk083" runat="server" Text="nicht benutzt &amp; unsichtbar" GroupName="grpAuswahl" AutoPostBack="True"></asp:RadioButton></TD>
														<TD class="TextLarge">
															<asp:Label id="lbl083" runat="server"></asp:Label></TD>
													</TR>
													<%--<TR>
														<TD class="StandardTableAlternate">
															<asp:RadioButton id="chk084" runat="server" Text="Kein Versandschreiben" GroupName="grpAuswahl" AutoPostBack="True"></asp:RadioButton></TD>
														<TD class="StandardTableAlternate">
															<asp:Label id="lbl084" runat="server"></asp:Label></TD>
													</TR>--%>
													<TR>
														<TD class="TextLarge">
															<asp:RadioButton id="chk085" runat="server" GroupName="grpAuswahl" Text="Briefverlust" AutoPostBack="True"></asp:RadioButton></TD>
														<TD class="TextLarge">
															<asp:Label id="lbl085" runat="server"></asp:Label></TD>
													</TR>
													<TR id="ShowUnknown" runat="server">
														<TD class="TextLarge">
															<asp:RadioButton id="chk999" runat="server" Text="unbekanntes Problem" GroupName="grpAuswahl" AutoPostBack="True"></asp:RadioButton></TD>
														<TD class="TextLarge">
															<asp:Label id="lbl999" runat="server"></asp:Label></TD>
													</TR>
													<%--
													<TR>
														<TD colspan="2"><hr>
														</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate">
															<asp:RadioButton id="chk082" runat="server" Text="Brief zur Zeit nicht im Archiv" GroupName="grpAuswahl" AutoPostBack="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate">
															<asp:Label id="lbl082" runat="server" Visible="False"></asp:Label></TD>
													</TR>
													<TR id="ShowVon" runat="server">
														<TD class="TextLarge">ab Datum</TD>
														<TD class="TextLarge">
															<asp:textbox id="txtAbDatum" runat="server" Width="130px"></asp:textbox>
															<asp:Button id="btnOpenSelectAb" runat="server" Text="..." Width="30px" Height="22px" CausesValidation="False"></asp:Button></TD>
													</TR>
													<TR id="ShowBis" runat="server">
														<TD class="StandardTableAlternate">bis Datum</TD>
														<TD class="StandardTableAlternate">
															<asp:textbox id="txtBisDatum" runat="server" Width="130px"></asp:textbox>
															<asp:Button id="btnOpenSelectBis" runat="server" Text="..." Width="30px" Height="22px" CausesValidation="False"></asp:Button></TD>
													</TR>--%>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
								</TD>
							</tr>
							<TR>
								<TD vAlign="top" width="57">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="57">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
			<asp:Label id="lblFilename" runat="server" Visible="False"></asp:Label>
		</form>
	</body>
</HTML>
