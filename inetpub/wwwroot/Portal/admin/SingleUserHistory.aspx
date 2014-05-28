<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SingleUserHistory.aspx.vb" Inherits="CKG.Admin.SingleUserHistory" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>

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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td>
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server">Administration</asp:label><asp:label id="lblPageTitle" runat="server">  (Datenselektion)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table5" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
									&nbsp;
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" align="right">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="javascript:window.close()">Fenster schlieﬂen</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<table id="TblSearch" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr id="trSearch" runat="server">
											<td align="left">
												<table bgColor="white" border="0">
													<TR>
														<TD vAlign="bottom" width="100">ab Datum:</TD>
														<TD vAlign="bottom" width="160"><asp:textbox id="txtAbDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectAb" runat="server" Width="30px" CausesValidation="False" Text="..." Height="22px"></asp:button></TD>
														<TD vAlign="bottom"><asp:calendar id="calAbDatum" runat="server" Width="160px" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
													<TR>
														<TD vAlign="bottom" width="100">bis Datum:</TD>
														<TD vAlign="bottom" width="160"><asp:textbox id="txtBisDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectBis" runat="server" Width="30px" CausesValidation="False" Text="..." Height="22px"></asp:button></TD>
														<TD vAlign="bottom"><asp:calendar id="calBisDatum" runat="server" Width="160px" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
													</table>
											</td>
										</tr>
									</table>
									<TABLE id="TblLog" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD>
										</TR>
										<tr>
											<td><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label>
                                                <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
										</tr>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" CellPadding="0" BackColor="White" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false"></asp:BoundColumn>
														<asp:BoundColumn DataField="Aktion" SortExpression="Aktion" HeaderText="Aktion"></asp:BoundColumn>
														<asp:BoundColumn DataField="ƒnderer" SortExpression="ƒnderer" HeaderText="ƒnderer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Alter Wert" SortExpression="Alter Wert" HeaderText="Alter Wert"></asp:BoundColumn>
														<asp:BoundColumn DataField="Neuer Wert" SortExpression="Neuer Wert" HeaderText="Neuer Wert"></asp:BoundColumn>
														<asp:BoundColumn DataField="Zeitpunkt" SortExpression="Zeitpunkt" HeaderText="Zeitpunkt"></asp:BoundColumn>
														<asp:BoundColumn DataField="Typ" SortExpression="Typ" HeaderText="Typ"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
										<tr>
											<td>&nbsp;</td>
										</tr>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../PageElements/Footer.html" -->
                                    <asp:TextBox ID="txtUserName" runat="server" Visible="False" Wrap="False"></asp:TextBox>
                                </TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
