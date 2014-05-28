<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80.aspx.vb" Inherits="CKG.Components.ComCommon.Change80" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<input type="hidden" name="txtReturnSearch" value="_">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server" Visible="False"> (Fahrzeugsuche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120" height="192">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="120">&nbsp;
												<asp:imagebutton id="ImageButton1" runat="server" ImageUrl="../../images/empty.gif"></asp:imagebutton></TD>
										</TR>
										<TR id="trcmdSearch" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="lb_Search" runat="server" CssClass="StandardButton"> lb_Search</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" height="192">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;
												<asp:linkbutton id="lnkCreateExcel" runat="server">Excelformat</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR id="tr_Auswahl" runat="server">
														<TD class="TextLarge" noWrap align="right">
															<asp:Label id="lbl_Auswahl" runat="server">lbl_Auswahl</asp:Label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge">
															<asp:RadioButton id="rb_Alle" runat="server" Text="rb_Alle" GroupName="grpAuswahl" Checked="True"></asp:RadioButton>&nbsp;&nbsp;
															<asp:RadioButton id="rb_Geplant" runat="server" Text="rb_Geplant" GroupName="grpAuswahl"></asp:RadioButton>&nbsp;&nbsp;
															<asp:RadioButton id="rb_InDurchfuehrung" runat="server" Text="rb_InDurchfuehrung" GroupName="grpAuswahl"></asp:RadioButton>&nbsp;&nbsp;
															<asp:RadioButton id="rb_Erledigt" runat="server" Text="rb_Erledigt" GroupName="grpAuswahl"></asp:RadioButton></TD>
													</TR>
													<TR id="tr_Gebiet" runat="server">
														<TD class="TextLarge" noWrap align="right">
															<asp:Label id="lbl_Gebiet" runat="server">lbl_Gebiet</asp:Label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtGebiet" runat="server" Width="160px" MaxLength="5"></asp:textbox></TD>
													</TR>
													<TR id="tr_Von" runat="server">
														<TD class="TextLarge" noWrap align="right">
															<asp:Label id="lbl_Von" runat="server">lbl_Von</asp:Label>&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtVon" runat="server" MaxLength="10" Width="160px"></asp:textbox>&nbsp;&nbsp;
															<asp:button id="btnOpenSelectAb" runat="server" Width="30px" Text="..." CausesValidation="False" Height="22px"></asp:button><BR>
															<asp:calendar id="calAbDatum" runat="server" Visible="False" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
													<TR id="tr_Bis" runat="server">
														<TD class="TextLarge" noWrap align="right">
															<asp:Label id="lbl_Bis" runat="server">lbl_Bis</asp:Label>&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtBis" runat="server" MaxLength="10" Width="160px"></asp:textbox>&nbsp;&nbsp;
															<asp:button id="btnOpenSelectBis" runat="server" Width="30px" Text="..." CausesValidation="False" Height="22px"></asp:button><BR>
															<asp:calendar id="calBisDatum" runat="server" Visible="False" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
												</TABLE>
												<asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Gebiet" SortExpression="Gebiet" HeaderText="Gebiet"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kunnr"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Gebiet" HeaderText="col_GEBIET">
															<HeaderTemplate>
																<asp:LinkButton id="col_GEBIET" runat="server" CommandName="Sort" CommandArgument="Gebiet">col_GEBIET</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gebiet") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Kunnr" HeaderText="col_KUNNR">
															<HeaderTemplate>
																<asp:LinkButton id="col_KUNNR" runat="server" CommandName="Sort" CommandArgument="Kunnr">col_KUNNR</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kunnr") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Name1" HeaderText="col_NAME1">
															<HeaderTemplate>
																<asp:LinkButton id="col_NAME1" runat="server" CommandName="Sort" CommandArgument="Name1">col_NAME1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name1") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Name2" HeaderText="col_NAME2">
															<HeaderTemplate>
																<asp:LinkButton id="col_NAME2" runat="server" CommandName="Sort" CommandArgument="Name2">col_NAME2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name2") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Post_code1" HeaderText="col_POST_CODE1">
															<HeaderTemplate>
																<asp:LinkButton id="col_POST_CODE1" runat="server" CommandName="Sort" CommandArgument="Post_code1">col_POST_CODE1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Post_code1") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="City1" HeaderText="col_CITY1">
															<HeaderTemplate>
																<asp:LinkButton id="col_CITY1" runat="server" CommandName="Sort" CommandArgument="City1">col_CITY1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.City1") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Street" HeaderText="col_STREET">
															<HeaderTemplate>
																<asp:LinkButton id="col_STREET" runat="server" CommandName="Sort" CommandArgument="Street">col_STREET</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Street") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Anzahl" HeaderText="col_Anzahl">
															<HeaderTemplate>
																<asp:LinkButton id="col_Anzahl" runat="server" CommandName="Sort" CommandArgument="Anzahl">col_Anzahl</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id="lb_Auswahl" runat="server" CssClass="StandardButtonTable" Text="lb_Auswahl" CausesValidation="false" CommandName="Select">lb_Auswahl</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
												<br>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<td><!--#include File="../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<asp:literal id="Literal1" runat="server"></asp:literal>
		<SCRIPT language="JavaScript">										
				<!--
						function openinfo (url) {
								fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0");
								fenster.focus();
						}

						function submitsearch () {
								window.document.Form1.txtReturnSearch.value="Search";
								window.document.Form1.submit();
						}
				-->
		</SCRIPT>
	</body>
</HTML>
