<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report03.aspx.vb" Inherits="CKG.Components.ComCommon.Report03" %>
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
			<input type="hidden" name="txtBemerkung">&nbsp;
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"></asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD align="right">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD>
															<asp:imagebutton id="ImageButton1" runat="server" ImageUrl="../../images/empty.gif"></asp:imagebutton></TD>
														<TD align="left"></TD>
													</TR>
													<TR>
														<TD class="" colSpan="2"><STRONG>
																<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" bgColor="#ffffff" border="0">
																	<TR>
																		<TD vAlign="top" noWrap>
																			<asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">Erstellen</asp:linkbutton></TD>
																		<TD colspan="3">&nbsp;&nbsp;&nbsp;
																			<asp:label id="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:label><BR>
																			<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
																	</TR>
																	<TR>
																		<TD vAlign="top" noWrap></TD>
																		<TD noWrap>&nbsp;&nbsp;&nbsp;<STRONG>Datum von:</STRONG>&nbsp;&nbsp;&nbsp;&nbsp;<BR>
																			<asp:calendar id="calAbDatum" runat="server" Visible="False" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
																				<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																				<NextPrevStyle ForeColor="White"></NextPrevStyle>
																				<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																				<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																				<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																				<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																				<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
																			</asp:calendar></TD>
																		<TD vAlign="top" noWrap>&nbsp;
																			<asp:textbox id="txtDatumVon" runat="server" Width="100px" MaxLength="10"></asp:textbox>&nbsp;&nbsp;
																			<asp:button id="btnOpenSelectAb" runat="server" Width="30px" Text="..." CausesValidation="False" Height="22px"></asp:button></TD>
																		<td vAlign="top" noWrap align="right" width="100%"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">
																				<strong>Excelformat</strong></asp:hyperlink>&nbsp;
																			<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label>&nbsp;</td>
																	</TR>
																	<TR>
																		<TD noWrap></TD>
																		<TD noWrap>&nbsp;&nbsp;&nbsp;<STRONG>Datum bis:</STRONG>&nbsp;&nbsp;&nbsp;&nbsp;<BR>
																			<asp:calendar id="calBisDatum" runat="server" Visible="False" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
																				<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																				<NextPrevStyle ForeColor="White"></NextPrevStyle>
																				<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																				<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																				<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																				<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																				<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
																			</asp:calendar></TD>
																		<TD vAlign="top" noWrap>&nbsp;
																			<asp:textbox id="txtDatumBis" runat="server" Width="100px" MaxLength="10"></asp:textbox>&nbsp;&nbsp;
																			<asp:button id="btnOpenSelectBis" runat="server" Width="30px" Text="..." CausesValidation="False" Height="22px"></asp:button></TD>
																		<td>&nbsp;</td>
																	</TR>
																	<TR>
																		<TD></TD>
																		<td>&nbsp;</td>
																		<TD noWrap></TD>
																		<TD>&nbsp;</TD>
																	</TR>
																</TABLE>
															</STRONG>
														</TD>
													</TR>
												</table>
											</TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge"></TD>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Vertriebsbelegnummer" SortExpression="Vertriebsbelegnummer" HeaderText="Vertriebsbelegnummer"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Durchf&#252;hrung">
															<ItemTemplate>
																<asp:Image id="imgWarning" runat="server" Width="16px" Height="16px" ImageUrl="/Portal/Images/empty.gif"></asp:Image>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Durchf&#252;hrung" SortExpression="Durchf&#252;hrung" HeaderText="Durchf&#252;hrung" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Vertriebsbelegnummer" SortExpression="Vertriebsbelegnummer" HeaderText="Vertriebsbelegnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Gebiet" SortExpression="Gebiet" HeaderText="Gebiet"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="H&#228;ndlername 1" HeaderText="H&#228;ndlername">
															<ItemTemplate>
																<asp:Label id=lblName runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Händlername 1") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.Händlername 2") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="H&#228;ndler PLZ" SortExpression="H&#228;ndler PLZ" HeaderText="H&#228;ndler PLZ"></asp:BoundColumn>
														<asp:BoundColumn DataField="H&#228;ndlerort" SortExpression="H&#228;ndlerort" HeaderText="H&#228;ndlerort"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="ButtonText" HeaderText="Bemerkung">
															<ItemTemplate>
																<asp:LinkButton id=lbBemerkung runat="server" CssClass="StandardButtonTable" CausesValidation="False" Text='<%# DataBinder.Eval(Container, "DataItem.ButtonText") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>' CommandName="Select">
																</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD height="19">&nbsp;</TD>
								<TD vAlign="top" align="left" height="19"></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD colSpan="2">
						<SCRIPT language="Javascript">
						<!-- //
						function BemerkungSenden(Vertriebsbelegnummer,Abfrage,Bemerkung)
						  {
							window.document.Form1.txtBemerkung.value = "";
						    var Check = window.prompt(Abfrage,Bemerkung);

						    if (Check.length > 0)
						      {
								window.document.Form1.txtBemerkung.value = Check;
								return (true);
						      }
						      else
						      {
								return (false);
						      }
						  }

						//-->
						</SCRIPT>
						<asp:literal id="Literal1" runat="server"></asp:literal></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
