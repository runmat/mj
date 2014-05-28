<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report04.aspx.vb" Inherits="AppUeberf._Report04" %>
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
				<TBODY>
					<TR>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</TR>
					<TR>
						<TD>
							<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
									</TR>
									<tr>
										<td colSpan="2"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
											<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" Target="_blank">Zusammenstellung von Abfragekriterien</asp:hyperlink></td>
									</tr>
									<TR>
										<TD colSpan="2">&nbsp;</TD>
									</TR>
									<TR>
										<TD class="TaskTitle" colSpan="2">&nbsp;
											<asp:label id="lblNoData" runat="server" Font-Bold="True"></asp:label></TD>
									</TR>
									<TR>
										<TD vAlign="top" colSpan="2">
											<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TBODY>
													<TR>
														<TD vAlign="top" align="left">
															<table>
																<TBODY>
																	<tr>
																		<TD class="TextLarge" vAlign="top" noWrap width="120"><asp:linkbutton id="cmdCreate" tabIndex="12" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Erstellen</asp:linkbutton>&nbsp;<br>
																			<asp:calendar id="cal1" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
																				<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																				<NextPrevStyle ForeColor="White"></NextPrevStyle>
																				<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																				<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																				<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																				<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																				<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
																			</asp:calendar><asp:calendar id="cal2" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
																				<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																				<NextPrevStyle ForeColor="White"></NextPrevStyle>
																				<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																				<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																				<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																				<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																				<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
																			</asp:calendar><asp:calendar id="cal3" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
																				<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																				<NextPrevStyle ForeColor="White"></NextPrevStyle>
																				<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																				<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																				<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																				<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																				<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
																			</asp:calendar><asp:calendar id="cal4" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
																				<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																				<NextPrevStyle ForeColor="White"></NextPrevStyle>
																				<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																				<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																				<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																				<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																				<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
																			</asp:calendar></TD>
																		<td vAlign="top">
																			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
																				<TBODY>
																					<tr id="trKUNNR" runat="server">
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lbl_KundenNr" runat="server">Kunde</asp:label></TD>
																						<TD colSpan="3"><asp:dropdownlist id="cmb_KundenNr" runat="server" Width="485px"></asp:dropdownlist></TD>
																					</tr>
																					<TR>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lblAuftrag" runat="server">Unsere Auftragsnr.</asp:label>&nbsp;&nbsp;</TD>
																						<TD class="" vAlign="center" align="left"><asp:textbox id="txtAuftrag" tabIndex="1" runat="server" Visible="True" MaxLength="20"></asp:textbox></TD>
																						<TD class="" vAlign="center"></TD>
																						<TD vAlign="center" width="100%"></TD>
																					</TR>
																					<TR>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lblReferenz" runat="server">Ihre Referenz</asp:label>&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:textbox id="txtReferenz" tabIndex="2" runat="server" Visible="True" MaxLength="20" Height="20px"></asp:textbox></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																					</TR>
																					<TR>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lblKennzeichen" runat="server">Kennzeichen</asp:label>&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:textbox id="txtKennzeichen" tabIndex="3" runat="server" Visible="True" MaxLength="10" Height="20px"></asp:textbox></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																					</TR>
																					<TR>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lblAuftragdatum" runat="server">Auftragsdatum</asp:label>&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" noWrap align="left"><asp:textbox id="txtAuftragdatum" tabIndex="4" runat="server" Visible="True" MaxLength="10" Height="20px"></asp:textbox>&nbsp;
																							<asp:linkbutton id="btnCal1" tabIndex="5" runat="server" CssClass="StandardButtonTable" Width="20px">...</asp:linkbutton></TD>
																						<TD class="TextLarge" vAlign="center" noWrap align="left">&nbsp;&nbsp; -&nbsp;<asp:textbox id="txtAuftragdatumBis" tabIndex="6" runat="server" Visible="True" MaxLength="10" Height="20px"></asp:textbox>&nbsp;
																							<asp:linkbutton id="btnCal2" tabIndex="7" runat="server" CssClass="StandardButtonTable" Width="20px">...</asp:linkbutton></TD>
																						<TD class="TextLarge" vAlign="center" align="left">&nbsp;(TT.MM.JJJJ)</TD>
																					</TR>
																					<TR>
																						<TD class="TextLarge" vAlign="center" noWrap align="left">
																							<asp:label id="Label5" runat="server">Überführungsdatum</asp:label>&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" noWrap align="left"><asp:textbox id="txtUeberfuehrungdatumVon" tabIndex="8" runat="server" MaxLength="10" Height="20px"></asp:textbox>&nbsp;
																							<asp:linkbutton id="btnCal3" tabIndex="9" runat="server" CssClass="StandardButtonTable" Width="20px">...</asp:linkbutton>
																							&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" noWrap align="left">&nbsp;&nbsp; -&nbsp;<asp:textbox id="txtUeberfuehrungdatumBis" tabIndex="10" runat="server" MaxLength="10" Height="20px"></asp:textbox>&nbsp;
																							<asp:linkbutton id="btnCal4" tabIndex="11" runat="server" CssClass="StandardButtonTable" Width="20px">...</asp:linkbutton>
																							&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" align="left" width="100%">&nbsp;(TT.MM.JJJJ)</TD>
																					</TR>
																					<TR>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lbl_Leasinggesellschaft" runat="server">Leasinggesellschaft</asp:label>&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:textbox id="txt_Leasinggesellschaft" tabIndex="12" runat="server" Visible="True"></asp:textbox></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																					</TR>
																					<TR>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:label id="lbl_Leasingkunde" runat="server">Leasingkunde</asp:label>&nbsp;&nbsp;</TD>
																						<TD class="TextLarge" vAlign="center" align="left"><asp:textbox id="txt_Leasingkunde" tabIndex="13" runat="server" Visible="True"></asp:textbox></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																						<TD class="TextLarge" vAlign="center"></TD>
																					</TR>
																					<TR>
																						<TD class="TextLarge" vAlign="center" noWrap colSpan="3"><asp:radiobuttonlist id="rbAuftragart" runat="server" RepeatDirection="Horizontal">
																								<asp:ListItem Value="A" Selected="True">alle Auftr&#228;ge</asp:ListItem>
																								<asp:ListItem Value="O">offene Auftr&#228;ge</asp:ListItem>
																								<asp:ListItem Value="D">durchgef&#252;hrte Auftr&#228;ge</asp:ListItem>
																								<asp:ListItem Value="N">Nur Kl&#228;rf&#228;lle*</asp:ListItem>
																							</asp:radiobuttonlist></TD>
																					</TR>
																				</TBODY></TABLE>
																			<asp:label id="Label2" runat="server" Width="567px">*Bei Klärfällen Selektion nur über Auftragsdatum möglich. Standard die letzten 90 Tage.</asp:label></td>
																	</tr>
																</TBODY></table>
														</TD>
													</TR>
												</TBODY></TABLE>
											<asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="300" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
												<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
												<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
												<Columns>
													<asp:BoundColumn Visible="False" DataField="Counter" SortExpression="Counter" HeaderText="Lfd.Nr."></asp:BoundColumn>
													<asp:BoundColumn DataField="Zzrefnr" SortExpression="Zzrefnr" HeaderText="Ihre Referenz"></asp:BoundColumn>
													<asp:BoundColumn DataField="Aufnr" SortExpression="Aufnr" HeaderText="Unsere Auftragsnr."></asp:BoundColumn>
													<asp:BoundColumn DataField="ERDAT" SortExpression="ERDAT" HeaderText="Auftrags-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
													<asp:BoundColumn DataField="Fahrtnr" SortExpression="Fahrtnr" HeaderText="Fahrt"></asp:BoundColumn>
													<asp:BoundColumn DataField="Zzkenn" SortExpression="Zzkenn" HeaderText="Kennzeichen"></asp:BoundColumn>
													<asp:BoundColumn DataField="Zzbezei" SortExpression="Zzbezei" HeaderText="Typ"></asp:BoundColumn>
													<asp:BoundColumn DataField="VDATU" SortExpression="VDATU" HeaderText="Überführungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
													<asp:BoundColumn DataField="wadat_ist" SortExpression="wadat_ist" HeaderText="Abgabe-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
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
													<asp:TemplateColumn HeaderText="col_LG">
														<HeaderTemplate>
															<asp:LinkButton ID="col_LG" CommandName="Sort" Runat="server" CommandArgument="Name_LG">col_LG</asp:LinkButton>
														</HeaderTemplate>
														<ItemTemplate>
															<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name_LG") %>'>
															</asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="col_LN">
														<HeaderTemplate>
															<asp:LinkButton ID="col_LN" CommandName="Sort" Runat="server" CommandArgument="Name_LN">col_LN</asp:LinkButton>
														</HeaderTemplate>
														<ItemTemplate>
															<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name_LN") %>' >
															</asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Details">
														<ItemTemplate>
															<asp:HyperLink id=HyperLink1 runat="server" Target="_blank" CssClass="StandardButtonTable" Width="100px" ToolTip="Detailansicht (in neuem Fenster öffnen)" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.URL") %>'>Ansicht >></asp:HyperLink>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
												<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
											</asp:datagrid></TD>
									</TR>
									<TR>
										<TD vAlign="top" colSpan="2"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
									</TR>
									<TR>
										<TD vAlign="top" colSpan="2">&nbsp;
											<asp:label id="lblScript" runat="server"></asp:label><!--#include File="../../../PageElements/Footer.html" --></TD>
									</TR>
								</TBODY></TABLE>
						</TD>
					</TR>
				</TBODY></TABLE>
		</form>
		</TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></FORM>
	</body>
</HTML>
