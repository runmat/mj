<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report16.aspx.vb" Inherits="AppEC.Report16" %>
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
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">Bitte geben Sie die Auswahlkriterien ein.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120"><asp:calendar id="calVon" runat="server" Visible="False" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar><asp:calendar id="calBis" runat="server" Visible="False" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px">
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
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="center">
															<TABLE class="BorderLeftBottom" id="Table5" cellSpacing="1" cellPadding="1" width="400" border="0">
																<TR>
																	<TD>&nbsp;</TD>
																	<TD colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>&nbsp;</TD>
																</TR>
																<TR>
																	<TD></TD>
																	<TD>PDI:
																	</TD>
																	<TD><asp:textbox id="txtPDI" runat="server"></asp:textbox></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD noWrap></TD>
																	<TD noWrap>Eingangsdatum von:</TD>
																	<TD><asp:textbox id="txtEingangsdatumVon" runat="server"></asp:textbox></TD>
																	<TD><asp:linkbutton id="btnVon" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender</asp:linkbutton></TD>
																</TR>
																<TR>
																	<TD></TD>
																	<TD>Eingangsdatum bis:</TD>
																	<TD><asp:textbox id="txtEingangsdatumBis" runat="server"></asp:textbox></TD>
																	<TD><asp:linkbutton id="btnBis" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender</asp:linkbutton></TD>
																</TR>
																<TR>
																	<TD></TD>
																	<TD>Fahrgestellnummer:</TD>
																	<TD><asp:textbox id="txtFahrgestellnummer" runat="server"></asp:textbox></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD></TD>
																	<TD>Fahrzeugmodell:</TD>
																	<TD><asp:textbox id="txtModell" runat="server"></asp:textbox></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD></TD>
																	<TD>&nbsp;</TD>
																	<TD></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD></TD>
																	<TD></TD>
																	<TD></TD>
																	<TD><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen...</asp:linkbutton></TD>
																</TR>
																<TR>
																	<TD></TD>
																	<TD></TD>
																	<TD></TD>
																	<TD><asp:linkbutton id="cmdDetails" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Detailanzeige&nbsp;&#187;</asp:linkbutton></TD>
																</TR>
															</TABLE>
															<asp:hyperlink id="lnkExcel" runat="server" CssClass="TaskTitle" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
															<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
													</TR>
													<TR id="rowResultate" runat="server">
														<TD class="TextLarge" vAlign="center"><asp:label id="lblResults" runat="server"></asp:label>
															<BR>
															<asp:datagrid id="DataGrid1" runat="server" Width="744px" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="PDINummer" SortExpression="PDINummer" HeaderText="PDI Nummer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="PDIName" SortExpression="PDIName" HeaderText="PDI Name"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Fahrzeuge" SortExpression="Fahrzeuge" HeaderText="Anzahl Fahrzeuge">
																		<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	</asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" CssClass="TextExtraLarge" Wrap="False"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
