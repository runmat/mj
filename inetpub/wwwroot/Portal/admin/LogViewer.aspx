<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LogViewer.aspx.vb" Inherits="CKG.Admin.LogViewer" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
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
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server">Administration</asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Datenselektion)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table5" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<table id="TblSearch" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr id="trSearch" runat="server">
											<td align="left">
												<table border="0" bgColor="white">
													<TR>
														<TD vAlign="bottom" width="100">Firma:</TD>
														<TD vAlign="bottom" width="160"><asp:label id="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterCustomer" runat="server" Width="160px" Height="20px" AutoPostBack="True" Visible="False"></asp:dropdownlist></TD>
														<TD vAlign="bottom"></TD>
													</TR>
													<tr id="trGruppe" runat="server">
														<TD vAlign="bottom" width="100" height="10">Gruppe:</TD>
														<TD vAlign="bottom" width="160" height="10"><asp:label id="lblGroup" runat="server" CssClass="InfoBoxFlat" Width="160px" Visible="False"></asp:label><asp:dropdownlist id="ddlFilterGroup" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
														<TD vAlign="bottom"></TD>
													</tr>
													<TR id="trOrganisation" runat="server">
														<TD vAlign="bottom" width="100" height="10">Organisation:</TD>
														<TD vAlign="bottom" width="160" height="10"><asp:label id="lblOrganisation" runat="server" CssClass="InfoBoxFlat" Width="160px" Visible="False"></asp:label><asp:dropdownlist id="ddlFilterOrganization" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
														<TD vAlign="bottom"></TD>
													</TR>
													<tr>
														<td vAlign="bottom" width="100">Benutzername:</td>
														<td vAlign="bottom" width="160">
															<P><asp:textbox id="txtUserID" runat="server" Width="0px" Height="0px" Visible="False" ForeColor="#CEDBDE" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None">-1</asp:textbox><asp:label id="lblUserName" runat="server" CssClass="InfoBoxFlat" Width="160px" Visible="False"></asp:label><asp:textbox id="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:textbox></P>
														</td>
														<TD vAlign="bottom" height="10"><asp:button id="btnSuche" runat="server" CssClass="StandardButton" Text="Benutzer suchen"></asp:button></TD>
													</tr>
													<TR>
														<TD vAlign="bottom" width="100">ab Datum:</TD>
														<TD vAlign="bottom" width="160"><asp:textbox id="txtAbDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectAb" runat="server" Width="30px" Height="22px" Text="..." CausesValidation="False"></asp:button></TD>
														<TD vAlign="bottom"><asp:calendar id="calAbDatum" runat="server" Width="160px" Visible="False" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
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
														<TD vAlign="bottom" width="160"><asp:textbox id="txtBisDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectBis" runat="server" Width="30px" Height="22px" Text="..." CausesValidation="False"></asp:button></TD>
														<TD vAlign="bottom"><asp:calendar id="calBisDatum" runat="server" Width="160px" Visible="False" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
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
														<TD vAlign="bottom" width="100">Aufgabe:</TD>
														<TD vAlign="bottom" width="160" colSpan="2"><asp:dropdownlist id="ddlAction" runat="server" Width="320px"></asp:dropdownlist></TD>
													</TR>
												</table>
											</td>
										</tr>
										<TR id="trSearchResult" runat="server">
											<TD align="left"><asp:datagrid id="dgSearchResult" runat="server" Width="100%" BorderWidth="1px" BorderStyle="Solid" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White">
													<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="UserID" SortExpression="UserID" HeaderText="UserID"></asp:BoundColumn>
														<asp:ButtonColumn DataTextField="UserName" SortExpression="UserName" HeaderText="Benutzername" CommandName="Edit"></asp:ButtonColumn>
														<asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Kunden-&lt;br&gt;referenz"></asp:BoundColumn>
														<asp:BoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="CustomerAdmin" HeaderText="Firmen-&lt;br&gt;Administrator">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=cbxSRCustomerAdmin runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="TestUser" HeaderText="Testzugang">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=cbxSRTestUser runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</table>
									<TABLE id="TblLog" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;
												<asp:label id="Label2" runat="server"> Datenanzeige</asp:label></TD>
										</TR>
										<tr>
											<td>
												<asp:HyperLink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:HyperLink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></td>
										</tr>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" CellPadding="0" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:ImageButton id="ImageButton1" runat="server"></asp:ImageButton>
																<asp:CheckBox id="CheckBox1" runat="server" Visible="False"></asp:CheckBox>
																<asp:Label id=lblID runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="UserName" SortExpression="UserName" HeaderText="Benutzername"></asp:BoundColumn>
														<asp:BoundColumn DataField="Inserted" SortExpression="Inserted" HeaderText="Angelegt"></asp:BoundColumn>
														<asp:BoundColumn DataField="Task" SortExpression="Task" HeaderText="Anwendung"></asp:BoundColumn>
														<asp:BoundColumn DataField="Identification" SortExpression="Identification" HeaderText="Identifikation"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Description" HeaderText="Beschreibung">
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' NAME="Label1">
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' NAME="Textbox1">
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;" Position="Top"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
