<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change205_5.aspx.vb" Inherits="AppSIXTL.Change205_5" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:label>
								</td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calZul" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx" Visible="False">Fahrzeugsuche</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" height="41">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" align="left" width="618" height="9"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
														<TD noWrap align="right" height="9">
															<P align="right">&nbsp;
																<asp:dropdownlist id="ddlPageSize" runat="server" Height="14px" AutoPostBack="True"></asp:dropdownlist></P>
														</TD>
													</TR>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="id" SortExpression="id" HeaderText="ID"></asp:BoundColumn>
														<asp:BoundColumn DataField="Erstellt" SortExpression="Erstellt" HeaderText="Erstellt"></asp:BoundColumn>
														<asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Equipment" SortExpression="Equipment" HeaderText="Equi"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnr" SortExpression="Fahrgestellnr" HeaderText="Fahrg.Nr."></asp:BoundColumn>
														<asp:BoundColumn DataField="Lvnr" SortExpression="Lvnr" HeaderText="LV-Nr."></asp:BoundColumn>
														<asp:BoundColumn DataField="Versandadresse" SortExpression="Versandadresse" HeaderText="Versandadr."></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseName1" SortExpression="VersandadresseName1" HeaderText="VersandadresseName1"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseName2" SortExpression="VersandadresseName2" HeaderText="VersandadresseName2"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseStr" SortExpression="VersandadresseStr" HeaderText="VersandadresseStr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseNr" SortExpression="VersandadresseNr" HeaderText="VersandadresseNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadressePlz" SortExpression="VersandadressePlz" HeaderText="VersandadressePlz"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="VersandadresseOrt" SortExpression="VersandadresseOrt" HeaderText="VersandadresseOrt"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Haendlernummer" SortExpression="Haendlernummer" HeaderText="H&#228;nderlnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="TIDNr" SortExpression="TIDNr" HeaderText="TIDNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="LIZNr" SortExpression="LIZNr" HeaderText="LIZNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Materialnummer" SortExpression="Materialnummer" HeaderText="Materialnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="VersandartShow" SortExpression="VersandartShow" HeaderText="Versandart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
														<asp:TemplateColumn>
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:LinkButton id=btnFreigeben runat="server" CssClass="StandardButtonTable" Width="100px" Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>' CommandName="Freigeben">Freigeben</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id=LinkButton3 runat="server" CssClass="StandardButtonTable" Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>' CommandName="delete">Storno</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD></TD>
							</TR>
							<tr>
								<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><br>
								</td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
