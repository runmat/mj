<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="jve_ZugriffsHistorie.aspx.vb" Inherits="CKG.Admin.jve_ZugriffsHistorieAbfrage" %>
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
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:linkbutton></TD>
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
												<table bgColor="white" border="0">
													<TR>
														<TD vAlign="top" width="100">ab Datum:</TD>
														<TD vAlign="top" width="177"><asp:textbox id="txtAbDatum" runat="server" Width="144px"></asp:textbox><asp:button id="btnOpenSelectAb" runat="server" Width="30px" Height="22px" Text="..." CausesValidation="False"></asp:button></TD>
														<TD vAlign="top">
															<P align="left">
																<asp:RadioButton id="cbxAll" runat="server" Text="Alle Vorgänge" Checked="True" GroupName="cbxOnline"></asp:RadioButton><BR>
																<asp:RadioButton id="cbxOnline" runat="server" Text="Nur Online-Benutzer" GroupName="cbxOnline"></asp:RadioButton><BR>
																<asp:RadioButton id="cbxError" runat="server" Text="Nur fehlerhafte Vorgänge" GroupName="cbxOnline"></asp:RadioButton></P>
														</TD>
														<TD vAlign="top">
															<P align="right"><FONT color="#ff0000">&nbsp;&nbsp;&nbsp;Länger als&nbsp;</FONT>
																<asp:dropdownlist id="ddbZeit" runat="server" Width="51px"></asp:dropdownlist><FONT color="#ff0000">&nbsp;Stunde(n) 
																	online.</FONT></P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="bottom" width="100"></TD>
														<TD vAlign="bottom" width="177"><asp:calendar id="calAbDatum" runat="server" Width="160px" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
														<TD vAlign="bottom"></TD>
														<TD vAlign="bottom"></TD>
													</TR>
													<TR>
														<TD vAlign="bottom" width="100">bis&nbsp;Datum:</TD>
														<TD vAlign="bottom" width="177"><asp:textbox id="txtBisDatum" runat="server" Width="144px"></asp:textbox><asp:button id="btnOpenSelectBis" runat="server" Width="30px" Height="22px" Text="..." CausesValidation="False"></asp:button></TD>
														<TD vAlign="bottom">
															<P align="left">&nbsp;</P>
														</TD>
														<TD vAlign="bottom">
															<P align="right"><FONT> &nbsp;&nbsp;</FONT></P>
														</TD>
													</TR>
													<TR>
														<TD vAlign="bottom" width="100"></TD>
														<TD vAlign="bottom" width="177"><asp:calendar id="calBisDatum" runat="server" Width="160px" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
														<TD vAlign="bottom"></TD>
														<TD vAlign="bottom"></TD>
													</TR>
													<TR>
														<TD vAlign="bottom" width="100">Kunde:</TD>
														<TD vAlign="bottom" width="177">
															<asp:DropDownList id="ddlCustomer" runat="server"></asp:DropDownList></TD>
														<TD vAlign="bottom"></TD>
														<TD vAlign="bottom"></TD>
													</TR>
													<tr>
														<td vAlign="bottom" width="100">Benutzername:</td>
														<td vAlign="bottom" width="177">
															<P><asp:textbox id="txtUserID" runat="server" Width="0px" Height="0px" Visible="False" ForeColor="#CEDBDE" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None">-1</asp:textbox><asp:textbox id="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:textbox></P>
														</td>
														<TD vAlign="bottom"></TD>
														<TD vAlign="bottom"></TD>
													</tr>
												</table>
											</td>
										</tr>
									</table>
									<TABLE id="TblLog" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<td>
												<table id="bla" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<TD class="TaskTitle" vAlign="top">&nbsp;
															<asp:label id="lblInfo" runat="server"> Datenanzeige</asp:label></TD>
														<TD class="TaskTitle" vAlign="top" align="right">
															<P align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist>&nbsp;</P>
														</TD>
													</tr>
												</table>
											</td>
										</TR>
										<tr>
											<td>&nbsp;&nbsp;
												<BR>
												<asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></td>
										</tr>
										<TR>
											<TD>&nbsp;<BR>
												<DBWC:HIERARGRID id="HGZ" runat="server" Width="100%" CellPadding="0" BorderColor="#999999" BorderStyle="None" AllowPaging="True" AutoGenerateColumns="False" TemplateDataMode="Table" LoadControlMode="UserControl" TemplateCachingBase="Tablename" BackColor="White" BorderWidth="1px">
													<PagerStyle Mode="NumericPages"></PagerStyle>
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="GridTableItem"></ItemStyle>
													<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="userName" SortExpression="userName" HeaderText="Benutzer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Customername" SortExpression="Customername" HeaderText="Kunde"></asp:BoundColumn>
														<asp:BoundColumn DataField="GroupName" HeaderText="Gruppe"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="TestUser" HeaderText="Test">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TestUser") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TestUser") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="hostName" SortExpression="hostName" HeaderText="Abmeldestatus"></asp:BoundColumn>
														<asp:BoundColumn DataField="requestType" SortExpression="requestType" HeaderText="Anfrageart"></asp:BoundColumn>
														<asp:BoundColumn DataField="browser" SortExpression="browser" HeaderText="Browser"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="startTime" HeaderText="&lt;u&gt;Startzeit&lt;/u&gt;">
															<ItemTemplate>
																<asp:Label id="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.startTime") %>' ForeColor='<%# DataBinder.Eval(Container, "DataItem.StartColor") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id="Textbox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.startTime") %>' ForeColor="Red">
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="endTime" SortExpression="endTime" HeaderText="Endzeit"></asp:BoundColumn>
													</Columns>
												</DBWC:HIERARGRID></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
