<%@ Page EnableEventValidation="false" Language="vb" AutoEventWireup="false" Codebehind="Change05_0.aspx.vb" Inherits="AppEC.Change05_0" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
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
			<table width="100%" align="left">
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(Fahrzeugauswahl)</td>
								</TR>
								<tr>
									<TD class="" vAlign="top">
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top" align="left">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right" colSpan="3"><P align="left">Aktion:
														<asp:label id="lblTask" runat="server" CssClass="TaskName"></asp:label>&nbsp;-&nbsp;Bitte 
														wählen Sie die gewünschten Fahrzeuge aus...</P>
												</TD>
											</TR>
											<TR>
												<TD align="right" colSpan="3">
													<P align="left"><asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></P>
												</TD>
											</TR>
											<TR>
												<TD class="PageNavigation" vAlign="top" noWrap align="right" colSpan="1" rowSpan="1">
													<P align="center">PDIs:
														<asp:label id="lblPDIs" runat="server"></asp:label></P>
												</TD>
												<TD class="PageNavigation" vAlign="top" noWrap align="right">
													<P align="center">Modelle:
														<asp:label id="lblModelle" runat="server"></asp:label></P>
												</TD>
												<TD class="PageNavigation" vAlign="top" noWrap align="right">
													<P align="left">
														<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
															<TR>
																<TD noWrap><asp:label id="lblBezeichnung" runat="server" CssClass="TaskNameHighlight"></asp:label></TD>
																<TD noWrap width="100%"><STRONG>
																		<P align="center">Fahrzeuge:
																			<asp:label id="lblFahrzeuge" runat="server"></asp:label></P>
																	</STRONG>
																</TD>
															</TR>
														</TABLE>
													</P>
												</TD>
											</TR>
											<TR>
												<TD class="" vAlign="top" align="left" colSpan="2" rowSpan="1">
													<P align="right">
														<TABLE id="Table2" cellSpacing="0" cellPadding="3" width="100%" border="0" runat="server">
															<TR>
																<TD class="BorderDateInputLeftBottom"><P align="center"><STRONG>Zulassungsdatum:</STRONG></P>
																</TD>
															</TR>
														</TABLE>
													</P>
												</TD>
												<TD class="" vAlign="top" noWrap align="left"><TABLE id="Table5" height="0" cellSpacing="0" cellPadding="3" border="0" runat="server">
														<TR>
															<TD class="BorderDateInputRightBottom">&nbsp;
																<ew:calendarpopup id="calZulassung" runat="server" ClearDateText=" " Nullable="True" ControlDisplay="TextBoxImage" ImageUrl="/PortalORM/Images/calendar.jpg" PopupLocation="Bottom" Width="75px" AutoPostBack="True">
																	<TextboxLabelStyle CssClass="DropDownStyle"></TextboxLabelStyle>
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
															<td>&nbsp;&nbsp;&nbsp;</td>
															<td class="BorderDateInputLeftBottom" width="100"><P align="center"><STRONG>Zulassung 
																		ges.&nbsp;</STRONG></P>
															</td>
															<td bgColor="#66ccff" width="30">
																<p align="center"><asp:label id="lZulassungGesamtAnzahl" Runat="server">0</asp:label></p>
															</td>
															<td class="BorderDateInputLeftBottom" width="100"><P align="center"><STRONG>Zulassung 
																		PDI&nbsp;</STRONG></P>
															</td>
															<td bgColor="#66ccff" width="30">
																<p align="center"><asp:label id="lZulassungPDIAnzahl" Runat="server">0</asp:label></p>
															</td>
														</TR>
													</TABLE>
												</TD>
											</TR>
											<TR id="trSumme" runat="server">
												<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="0" rowSpan="0"><asp:listbox id="lstPDI" runat="server" CssClass="ListStyle" Height="400px" AutoPostBack="True"></asp:listbox></TD>
												<TD class="LabelExtraLarge" vAlign="top" align="left">
													<P align="left"><asp:listbox id="lstMOD" runat="server" CssClass="ListStyle" Height="400px" AutoPostBack="True"></asp:listbox></P>
												</TD>
												<TD class="LabelExtraLarge" vAlign="top" noWrap align="left">
													<P align="left"><asp:datagrid id="DataGrid1" runat="server" CssClass="tableMain" Width="100%" BackColor="White" headerCSS="tableHeader" bodyCSS="tableBody" AutoGenerateColumns="False" bodyHeight="400">
															<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
															<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
															<Columns>
																<asp:BoundColumn Visible="False" DataField="RowId" SortExpression="RowId" HeaderText="ID"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="Art" SortExpression="Art" HeaderText="Art"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="KUNPDI" SortExpression="KUNPDI" HeaderText="PDI"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="ZZBEZEI" SortExpression="ZZBEZEI" HeaderText="Modell"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="SIPPCODE" SortExpression="SIPPCODE" HeaderText="SIPP-Code"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="ZZANTR" SortExpression="ZZANTR" HeaderText="Antrieb"></asp:BoundColumn>
																<asp:BoundColumn DataField="ZZNAVI" SortExpression="ZZNAVI" HeaderText="Navi"></asp:BoundColumn>
																<asp:BoundColumn DataField="ZZREIFEN" SortExpression="ZZREIFEN" HeaderText="Reifen"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="ZZAKTSPERRE" SortExpression="ZZAKTSPERRE" HeaderText="Gesperrt">
																	<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="Eingangsdatum" SortExpression="Eingangsdatum" HeaderText="Eingangsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
																<asp:BoundColumn DataField="ZZFARBE" SortExpression="ZZFARBE" HeaderText="Farbe">
																	<ItemStyle HorizontalAlign="Left"></ItemStyle>
																</asp:BoundColumn>
																<asp:TemplateColumn HeaderText="Zul.Datum">
																	<ItemTemplate>
																		<asp:TextBox id="calControl" runat="server" CssClass="DropDownStyle" Width="75px"></asp:TextBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn HeaderText="Kennz.Serie">
																	<ItemTemplate>
																		<P align="left">
																			<asp:DropDownList id="ddlKennzeichenserie" runat="server" CssClass="DropDownStyle"></asp:DropDownList></P>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn HeaderText="Bemerkungen">
																	<ItemTemplate>
																		<asp:TextBox id="txtBemerkung" runat="server" CssClass="InputEnabledStyle" Width="150px" ToolTip="Maximal 75 Zeichen." MaxLength="75"></asp:TextBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn HeaderText="Ziel-PDI">
																	<ItemTemplate>
																		<asp:DropDownList id="ddlZielPDI" runat="server" CssClass="DropDownStyle"></asp:DropDownList>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn DataField="ZZAKTSPERRE" SortExpression="ZZAKTSPERRE" HeaderText="Gesperrt">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																</asp:BoundColumn>
																<asp:TemplateColumn HeaderText="Auswahl">
																	<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate>
																		<asp:CheckBox id="cbxAuswahl" runat="server"></asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn DataField="Status" SortExpression="Status">
																	<ItemStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Red"></ItemStyle>
																</asp:BoundColumn>
																<asp:TemplateColumn HeaderText="Menge">
																	<ItemStyle Wrap="False"></ItemStyle>
																	<ItemTemplate>
																		<FONT size="2"></FONT>
																		<asp:TextBox id="txtMenge" runat="server" CssClass="DropDownStyle" Width="30px" MaxLength="3"></asp:TextBox>
																		<asp:Button id="btnKopieren" runat="server" CssClass="StandardButtonTable" Height="20px" Width="15px" ToolTip="Kopieren" Font-Names="Arial" Text="V" CommandName="Kopieren"></asp:Button>
																	</ItemTemplate>
																</asp:TemplateColumn>
															</Columns>
														</asp:datagrid></P>
												</TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge" vAlign="top"></TD>
												<TD class="LabelExtraLarge" vAlign="top"></TD>
												<TD class="LabelExtraLarge">
													<P align="right"><asp:linkbutton id="btnConfirm" runat="server" CssClass="StandardButtonTable" Width="150px">Weiter&nbsp;&#187;</asp:linkbutton></P>
												</TD>
											</TR>
										</TABLE> <!--#include File="../../../PageElements/Footer.html" -->
										<P align="left"></P>
									</TD>
								</tr>
							</TABLE>
		</form>
		</TD></TR></TBODY></TABLE>
	</body>
</HTML>
