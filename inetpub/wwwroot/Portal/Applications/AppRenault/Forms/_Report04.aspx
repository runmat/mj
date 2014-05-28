<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report04.aspx.vb" Inherits="AppRenault._Report04" %>
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
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;
									<asp:label id="lblNoData" runat="server" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" colSpan="2">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="top" width="120"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButtonTable" tabIndex="12"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
														<TD class="TextLarge" vAlign="center" align="left">&nbsp;&nbsp;&nbsp;
															<BR>
															<asp:label id="lblAuftrag" runat="server">Auftragsnr.</asp:label>&nbsp;&nbsp;</TD>
														<TD class="" vAlign="center" align="left">&nbsp;&nbsp;
															<BR>
															<asp:textbox id="txtAuftrag" runat="server" Visible="True" MaxLength="20" Height="20px" tabIndex="1"></asp:textbox></TD>
														<TD class="" vAlign="center"></TD>
														<TD vAlign="center" width="100%"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" rowspan="4">
															<asp:calendar id="cal1" runat="server" Visible="False" Width="120px" CellPadding="0" BorderColor="Black" BorderStyle="Solid">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar>
															<asp:calendar id="cal2" runat="server" Visible="False" Width="120px" CellPadding="0" BorderColor="Black" BorderStyle="Solid">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar>
															<asp:calendar id="cal3" runat="server" Visible="False" Width="120px" CellPadding="0" BorderColor="Black" BorderStyle="Solid">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar>
															<asp:calendar id="cal4" runat="server" Visible="False" Width="120px" CellPadding="0" BorderColor="Black" BorderStyle="Solid">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
														<TD class="TextLarge" vAlign="center" align="left">
															<asp:label id="lblReferenz" runat="server">Referenznr.</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="center" align="left">
															<asp:textbox id="txtReferenz" runat="server" Visible="True" MaxLength="20" Height="20px" tabIndex="2"></asp:textbox></TD>
														<TD class="TextLarge" vAlign="center"></TD>
														<TD class="TextLarge" vAlign="center"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" align="left">
															<asp:label id="lblKennzeichen" runat="server">Kennzeichen</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="center" align="left">
															<asp:textbox id="txtKennzeichen" runat="server" Visible="True" MaxLength="10" Height="20px" tabIndex="3"></asp:textbox></TD>
														<TD class="TextLarge" vAlign="center"></TD>
														<TD class="TextLarge" vAlign="center"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" align="left">
															<asp:label id="lblAuftragdatum" runat="server">Auftragsdatum</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="center" noWrap align="left">
															<asp:textbox id="txtAuftragdatum" runat="server" Visible="True" MaxLength="10" Height="20px" tabIndex="4"></asp:textbox>&nbsp;
															<asp:linkbutton id="btnCal1" runat="server" CssClass="StandardButtonTable" tabIndex="5" Width="20px">...</asp:linkbutton></TD>
														<TD class="TextLarge" vAlign="center" noWrap align="left">
															&nbsp;&nbsp; -&nbsp;<asp:textbox id="txtAuftragdatumBis" runat="server" Height="20px" MaxLength="10" Visible="True" tabIndex="6"></asp:textbox>&nbsp;
															<asp:linkbutton id="btnCal2" runat="server" CssClass="StandardButtonTable" tabIndex="7" Width="20px">...</asp:linkbutton></TD>
														<TD class="TextLarge" vAlign="center" align="left">
															&nbsp;(TT.MM.JJJJ)</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap align="left">
															<asp:label id="Label5" runat="server"> Überführungsdatum</asp:label>&nbsp;&nbsp;<BR>
															&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="center" noWrap align="left">
															<asp:textbox id="txtUeberfuehrungdatumVon" runat="server" MaxLength="10" Height="20px" tabIndex="8"></asp:textbox>&nbsp;
															<asp:linkbutton id="btnCal3" tabIndex="9" runat="server" CssClass="StandardButtonTable" Width="20px">...</asp:linkbutton><BR>
															&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="center" noWrap align="left">
															&nbsp;&nbsp; -&nbsp;<asp:textbox id="txtUeberfuehrungdatumBis" runat="server" MaxLength="10" Height="20px" tabIndex="10"></asp:textbox>&nbsp;
															<asp:linkbutton id="btnCal4" tabIndex="11" runat="server" CssClass="StandardButtonTable" Width="20px">...</asp:linkbutton><BR>
															&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="center" align="left" width="100%">
															&nbsp;(TT.MM.JJJJ)<BR>
															&nbsp;&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" noWrap></TD>
														<TD class="TextLarge" vAlign="center" noWrap colSpan="3">
															<asp:RadioButtonList id="rbAuftragart" runat="server" RepeatDirection="Horizontal">
																<asp:ListItem Value="A" Selected="True">alle Auftr&#228;ge</asp:ListItem>
																<asp:ListItem Value="O">offene Auftr&#228;ge</asp:ListItem>
																<asp:ListItem Value="D">durchgef&#252;hrte Auftr&#228;ge</asp:ListItem>
															</asp:RadioButtonList></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									<asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="300" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn Visible="False" DataField="Counter" SortExpression="Counter" HeaderText="Lfd.Nr."></asp:BoundColumn>
											<asp:BoundColumn DataField="Zzrefnr" SortExpression="Zzrefnr" HeaderText="Referenz"></asp:BoundColumn>
											<asp:BoundColumn DataField="Aufnr" SortExpression="Aufnr" HeaderText="Auftrag"></asp:BoundColumn>
											<asp:BoundColumn DataField="ERDAT" SortExpression="ERDAT" HeaderText="Auftrags-&lt;br&gt;datum"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fahrtnr" SortExpression="Fahrtnr" HeaderText="Fahrt"></asp:BoundColumn>
											<asp:BoundColumn DataField="Zzkenn" SortExpression="Zzkenn" HeaderText="Kennzeichen"></asp:BoundColumn>
											<asp:BoundColumn DataField="Zzbezei" SortExpression="Zzbezei" HeaderText="Typ"></asp:BoundColumn>
											<asp:BoundColumn DataField="wadat_ist" SortExpression="wadat_ist" HeaderText="Abgabe-&lt;br&gt;datum"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fahrtvon" SortExpression="Fahrtvon" HeaderText="Von"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fahrtnach" SortExpression="Fahrtnach" HeaderText="Nach"></asp:BoundColumn>
											<asp:BoundColumn DataField="Gef_Km" SortExpression="Gef_Km" HeaderText="Km"></asp:BoundColumn>
											<asp:TemplateColumn SortExpression="KFTEXT" HeaderText="Kl&#228;rfall">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=Label1 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.KFTEXT")<>String.Empty %>'>X</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Details">
												<ItemTemplate>
													<asp:HyperLink id=HyperLink1 runat="server" CssClass="StandardButtonTable" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.URL") %>' Target="_blank" ToolTip="Detailansicht (in neuem Fenster öffnen)">Ansicht >></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="KFTEXT" SortExpression="KFTEXT" HeaderText="KF"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD vAlign="top" colSpan="2">
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" colSpan="2">&nbsp;
									<asp:Label id="lblScript" runat="server"></asp:Label><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
