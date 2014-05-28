<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="jve_LogMessage2.aspx.vb" Inherits="CKG.Admin.jve_LogMessage2" %>
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
									<asp:label id="lblPageTitle" runat="server" Font-Bold="True"> (Nachrichten verwalten)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table5" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<P><asp:linkbutton id="btnNew" runat="server" CssClass="StandardButton">&#149;&nbsp;Neue Nachricht</asp:linkbutton></P>
											</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<P><asp:linkbutton id="btnSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Speichern</asp:linkbutton></P>
											</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="btnDelete" runat="server" CssClass="StandardButton">&#149;&nbsp;Löschen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="btnDelete2" runat="server" CssClass="StandardButton">&#149;&nbsp;Löschen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="btnCancel" runat="server" CssClass="StandardButton">&#149;&nbsp;Abbrechen</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<asp:textbox id="txtid" runat="server" Visible="False" Height="18px" Width="71px" DESIGNTIMEDRAGDROP="1031"></asp:textbox>
									<asp:calendar id="calCalendar1" runat="server" Visible="False" Height="86px" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<P>
										<asp:calendar id="calCalendar2" runat="server" Visible="False" Height="86px" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
											<TodayDayStyle Font-Bold="True"></TodayDayStyle>
											<NextPrevStyle ForeColor="White"></NextPrevStyle>
											<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
											<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
											<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
											<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
											<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
										</asp:calendar></P>
								</TD>
								<TD vAlign="top">
									<table id="TblSearch" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;
											</TD>
											<TD class="TaskTitle" vAlign="top" align="right">
												<asp:button id="Button1" runat="server" Height="20px" Width="114px" Text="Serverzeit" Font-Size="XX-Small"></asp:button>
												<asp:textbox id="txtServerzeit" runat="server" Width="160px" BorderWidth="1px" BorderStyle="Solid" ReadOnly="True" Wrap="False">Serverzeit</asp:textbox>&nbsp;
											</TD>
										</TR>
										<TR>
										</TR>
										<TR>
											<TD align="left" width="207">
												<P>Kunde</P>
											</TD>
											<TD vAlign="top" align="left"><asp:dropdownlist id="ddlKunde" runat="server"></asp:dropdownlist></TD>
										</TR>
										<tr id="trSearch" runat="server">
											<TD align="left" width="207">
												Datum&nbsp;Anzeige (tt.mm.jjjj)*<BR>
												von&nbsp;- bis</TD>
											<td vAlign="top" align="left">
												<P><asp:textbox id="txtDatumVon" tabIndex="1" runat="server" Width="150px"></asp:textbox>&nbsp;<asp:textbox id="txtDatumBis" tabIndex="2" runat="server" Width="150px"></asp:textbox>
													<asp:button id="btnCalendar" runat="server" Text="Kalender"></asp:button><asp:checkbox id="cbxAlter" runat="server" Visible="False" Enabled="False"></asp:checkbox></P>
												<P>&nbsp;</P>
											</td>
										</tr>
										<TR>
											<TD align="left" width="207" noWrap>Zeit Anzeige (hh:mm)*<BR>
												von&nbsp;- bis</TD>
											<TD vAlign="top" align="left">
												<P><asp:textbox id="txtZeitVon" tabIndex="3" runat="server" Width="150px"></asp:textbox>&nbsp;<asp:textbox id="txtZeitBis" tabIndex="4" runat="server" Width="150px"></asp:textbox>
													<asp:dropdownlist id="ddlTime" runat="server" Visible="False" AutoPostBack="True"></asp:dropdownlist>
													<asp:checkbox id="cbxAlterTime" runat="server" Visible="False" Enabled="False"></asp:checkbox></P>
											</TD>
										</TR>
										<TR>
											<TD align="left" width="207">
												<P>Betreff&nbsp; (bis 50 Zeichen)</P>
											</TD>
											<TD align="left">
												<asp:textbox id="txtBetreff" tabIndex="5" runat="server" Width="450px" MaxLength="50"></asp:textbox></TD>
										</TR>
										<TR>
											<TD align="left" width="207" height="72">
												<P>Nachricht (bis 500 Zeichen)</P>
											</TD>
											<TD align="left" height="72">
												<P>
													<asp:textbox id="txtMessage" tabIndex="6" runat="server" Width="450px" MaxLength="500" TextMode="MultiLine" Rows="4"></asp:textbox></P>
											</TD>
										</TR>
										<TR>
											<TD align="left" width="207">
												<asp:checkbox id="cbxLogin" runat="server" Visible="False" Text="Anmeldung zulassen für" Enabled="False" Checked="True"></asp:checkbox></TD>
											<TD align="left">
												<asp:checkbox id="cbxActive" runat="server" Text="Aktiv" Checked="True"></asp:checkbox>
												<asp:checkbox id="cbxAny" runat="server" Text="Onlinezeitraum" Font-Underline="True" ForeColor="Transparent"></asp:checkbox><BR>
												<BR>
												Anmeldung zulassen für
												<asp:checkbox id="cbxActiveOld" runat="server" Visible="False" Text="Aktiv" Checked="True"></asp:checkbox>
												<asp:checkbox id="cbxAll" runat="server" Visible="False"></asp:checkbox></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD>
												<P>
													<asp:checkbox id="cbxTest" runat="server" Text="TEST-Benutzer" Checked="True"></asp:checkbox><BR>
													<asp:checkbox id="cbxProd" runat="server" Text="PROD-Benutzer"></asp:checkbox><BR>
													<asp:checkbox id="CheckBox1" runat="server" Text="DAD-Admin" Enabled="False" Checked="True"></asp:checkbox></P>
											</TD>
										</TR>
										<TR>
											<TD colSpan="3">
												<HR width="100%" SIZE="1">
											</TD>
										</TR>
										<TR>
											<TD><U>Formatierungsanweisungen</U></TD>
											<TD width="100%">
												<P><FONT face="Courier New" size="2"><FONT face="Times New Roman" size="3">Text in rot</FONT>: 
														{c="#FF0000"}Text...{/c}</FONT></P>
											</TD>
										</TR>
										<TR>
											<TD></TD>
											<TD>
												<P><FONT face="Courier New" size="2"><FONT face="Times New Roman" size="3">Text fett</FONT>: 
														{b}Text...{/b}</FONT></P>
											</TD>
										</TR>
										<TR>
											<TD></TD>
											<TD><FONT face="Courier New" size="2"><FONT face="Times New Roman" size="3">Text kursiv</FONT>: 
													{i}Text...{/i}</FONT></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD><FONT face="Courier New" size="2"><FONT face="Times New Roman" size="3">Hyperlink</FONT>:&nbsp;{h}Text...{/h} 
													(nicht in der Betreffzeile)</FONT></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD><FONT face="Courier New" size="2"><FONT face="Times New Roman" size="3">Neue Zeile</FONT>:&nbsp;{br/}</FONT></TD>
										</TR>
									</table>
									<TABLE id="TblLog" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD align="right">
												<asp:label id="lblInfo" runat="server"></asp:label>
												<asp:dropdownlist id="ddlPageSize" tabIndex="102" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD>
												<asp:datagrid id="gridMain" tabIndex="103" runat="server" Width="100%" CellPadding="0" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="id" SortExpression="id" HeaderText="Nr."></asp:BoundColumn>
														<asp:BoundColumn DataField="creationDate" SortExpression="creationDate" HeaderText="Erstellt"></asp:BoundColumn>
														<asp:BoundColumn DataField="activeDateFrom" SortExpression="activeDateFrom" HeaderText="Datum von" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="activeDateTo" SortExpression="activeDateTo" HeaderText="Datum bis" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="activeTimeFrom" SortExpression="activeTimeFrom" HeaderText="Zeit von" DataFormatString="{0:HH:mm}"></asp:BoundColumn>
														<asp:BoundColumn DataField="activeTimeTo" SortExpression="activeTimeTo" HeaderText="Zeit bis" DataFormatString="{0:HH:mm}"></asp:BoundColumn>
														<asp:BoundColumn DataField="titleText" SortExpression="titleText" HeaderText="Betreff"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="active" HeaderText="Aktiviert">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:TextBox id=TextBox3 runat="server" Font-Bold="True" Width="100%" Visible='<%# DataBinder.Eval(Container, "DataItem.active") %>' ReadOnly="True" BorderStyle="Groove" BorderColor="Transparent" BorderWidth="1px" ForeColor="Black" BackColor="Lime">
																</asp:TextBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.active") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="active" HeaderText="Bei jedem&lt;br&gt;Seitenw.">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:TextBox id=Textbox2 runat="server" Font-Bold="True" Width="100%" Visible='<%# DataBinder.Eval(Container,"DataItem.messageColor") %>' ReadOnly="True" BorderStyle="Groove" BorderColor="Transparent" BorderWidth="1px" ForeColor="Black" BackColor="Lime">
																</asp:TextBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.messageColor") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="active" HeaderText="Login&lt;br&gt;CKE-User">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:TextBox id=Textbox5 runat="server" Font-Bold="True" Visible='<%# DataBinder.Eval(Container, "DataItem.onlyTEST") %>' Width="100%" ReadOnly="True" BorderColor="Transparent" BorderStyle="Groove" BorderWidth="1px" BackColor="Lime" ForeColor="Black">
																</asp:TextBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox6 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.onlyTEST") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="active" HeaderText="Login&lt;br&gt;CKP-User">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:TextBox id=Textbox7 runat="server" Font-Bold="True" Visible='<%# DataBinder.Eval(Container, "DataItem.onlyPROD") %>' Width="100%" ReadOnly="True" BorderColor="Transparent" BorderStyle="Groove" BorderWidth="1px" BackColor="Lime" ForeColor="Black">
																</asp:TextBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox8 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.onlyPROD") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Auswahl">
															<ItemTemplate>
																<asp:ImageButton id="btnSelect" runat="server" CommandName="Select" ImageUrl="../Images/Arrowright.gif" CausesValidation="false"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;" Position="Top" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left">
									<asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../PageElements/Footer.html" -->
									* Pflichtfelder</TD>
							</TR>
						</TABLE>
						<asp:literal id="litConfirm" runat="server"></asp:literal>
						<asp:literal id="serverzeit" runat="server"></asp:literal></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
