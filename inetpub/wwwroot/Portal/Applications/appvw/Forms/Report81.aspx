<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report81.aspx.vb" Inherits="AppVW.Report81" %>
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
								<TD class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>
									<asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR id="trCreate" runat="server">
											<TD vAlign="center" width="150">
												<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calVon" runat="server" Width="120px" Visible="False" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:calendar id="calBis" runat="server" Width="120px" Visible="False" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
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
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white">
													<TR>
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Auftragseingang von:&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center">
															<asp:TextBox id="txtBriefeingangVon" runat="server" MaxLength="10"></asp:TextBox>&nbsp;
															<asp:LinkButton id="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>&nbsp;&nbsp; 
															(TT.MM.JJJJ)</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Auftragseingang bis:&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center">
															<asp:TextBox id="txtBriefeingangBis" runat="server" MaxLength="10"></asp:TextBox>&nbsp;
															<asp:LinkButton id="btnCal2" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>&nbsp;&nbsp; 
															(TT.MM.JJJJ)</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Referenznummer*:&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center">
															<asp:TextBox id="txtReferenznummer" runat="server" MaxLength="9"></asp:TextBox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150">
															<P align="right">Fahrgestell-Nr**:&nbsp;&nbsp;&nbsp;
															</P>
														</TD>
														<TD class="TextLarge" vAlign="center">
															<asp:TextBox id="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:TextBox>
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150"></TD>
														<TD class="TextLarge" vAlign="center"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center" width="150"></TD>
														<TD class="TextLarge" vAlign="center">
															* Eingabe von Platzhalter möglich.<BR>
															** Eingabe von vorangestelltem Platzhalter möglich. Mindestens&nbsp;sechs 
															Zeichen (z.B. *12345678)</TD>
													</TR>
													<TR id="trSelectDropdown" runat="server">
														<TD class="TextLarge" vAlign="top" width="150" align="right">Auswahl:&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" vAlign="center">
														<asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="300" AllowSorting="True" AllowPaging="True" Width="100%" AutoGenerateColumns="False">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="Zzfahrg" SortExpression="Zzfahrg" HeaderText="Zzfahrg"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="Zzfahrg" HeaderText="Fahrgestellnummer">
																		<ItemTemplate>
																			<asp:LinkButton id=LinkButton1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zzfahrg") %>' CausesValidation="False" CommandName="Select">
																			</asp:LinkButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="Liznr" SortExpression="Liznr" HeaderText="Referenznummer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Briefeingang" SortExpression="Briefeingang" HeaderText="Auftragseingang" DataFormatString="{0:dd.MM.yy}"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Zulassungsdatum" SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yy}"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Zzkenn" SortExpression="Zzkenn" HeaderText="Wunschkennzeichen"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
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
								<TD vAlign="top">
									<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
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
