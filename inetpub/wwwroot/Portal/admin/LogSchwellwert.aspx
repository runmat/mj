<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogSchwellwert.aspx.vb" Inherits="CKG.Admin.LogSchwellwert" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
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
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
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
									<asp:calendar id="calAbDatum" runat="server" Width="108px" CellPadding="0" 
                                        BorderColor="Black" BorderStyle="Solid" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar><asp:calendar id="calBisDatum" 
                                        runat="server" Width="101px" CellPadding="0" BorderColor="Black" 
                                        BorderStyle="Solid" Visible="False">
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
									<table id="TblSearch" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr id="trSearch" runat="server">
											<td align="left">
												<table bgColor="white" border="0">
													<TR>
														<TD vAlign="bottom" width="100">ab Datum:</TD>
														<TD vAlign="bottom" width="160"><asp:textbox id="txtAbDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectAb" runat="server" Width="30px" CausesValidation="False" Text="..." Height="22px"></asp:button></TD>
														<TD vAlign="bottom">&nbsp;</TD>
													</TR>
													<TR>
														<TD vAlign="bottom" width="100">bis Datum:</TD>
														<TD vAlign="bottom" width="160"><asp:textbox id="txtBisDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectBis" runat="server" Width="30px" CausesValidation="False" Text="..." Height="22px"></asp:button></TD>
														<TD vAlign="bottom">&nbsp;</TD>
													</TR>
													<TR>
														<TD vAlign="bottom" width="100">&nbsp;</TD>
														<TD vAlign="bottom" width="160" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															</TD>
													</TR>
													</table>
											</td>
										</tr>
									</table>
									<TABLE id="TblLog" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></td>
										</tr>
										<TR>
											<TD>
                                                <asp:GridView ID="grvLogSchwellwert" runat="server" AutoGenerateColumns="False" 
                                                    BackColor="White" AllowPaging="True">
                                                    <Columns>
                                                        <asp:BoundField DataField="Seite" HeaderText="ASPX-Seite" />
                                                        <asp:BoundField DataField="Datum" HeaderText="Datum" 
                                                            DataFormatString="{0:dd.MM.yyyy}" />
                                                        <asp:BoundField DataField="GesamtAnz" HeaderText="Gesamtanzahl" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UeberschreitungAnz" 
                                                            HeaderText="Anzahl Überschreitung" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AktuellerSchwellwert" 
                                                            HeaderText="Aktueller Schwellwert" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Detail">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtDetail" runat="server" CommandName="Detail" 
                                                                    ImageUrl="../Images/lupe2.gif" CommandArgument='<%# Container.DataItemIndex %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle HorizontalAlign="Left" />
                                                    <HeaderStyle CssClass="GridTableHead" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                </asp:GridView>
                                            </TD>
										</TR>
										<TR>
											<TD>
                                                &nbsp;</TD>
										</TR>
										<tr>
											<td><asp:datagrid id="DataGrid1" runat="server" Width="100%" CellPadding="0" BackColor="White" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Testbenutzer">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=CheckBox1 runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="BAPI" SortExpression="BAPI" HeaderText="BAPI"></asp:BoundColumn>
														<asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Parameter"></asp:BoundColumn>
														<asp:BoundColumn DataField="Start" SortExpression="Start" HeaderText="Start"></asp:BoundColumn>
														<asp:BoundColumn DataField="Ende" SortExpression="Ende" HeaderText="Ende"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Dauer" HeaderText="Dauer">
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer"))) %>' Height="10px" BackColor="#8080FF">
																</asp:Label>
																<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Erfolg" HeaderText="Erfolg">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=CheckBox2 runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Erfolg") %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Fehlermeldung" SortExpression="Fehlermeldung" HeaderText="Fehlermeldung"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></td>
										</tr>
										<tr>
											<td><DBWC:HIERARGRID id="HGZ" runat="server" Width="100%" BorderStyle="None" BorderColor="#999999" CellPadding="0" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" TemplateDataMode="Table" LoadControlMode="UserControl" TemplateCachingBase="Tablename" BorderWidth="1px" AllowSorting="True">
													<PagerStyle Mode="NumericPages"></PagerStyle>
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="GridTableItem"></ItemStyle>
													<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Seite" SortExpression="Seite" HeaderText="Seite"></asp:BoundColumn>
														<asp:BoundColumn DataField="Anwendung" SortExpression="Anwendung" HeaderText="Anwendung"></asp:BoundColumn>
														<asp:BoundColumn DataField="Zugriffe SAP" SortExpression="Zugriffe SAP" HeaderText="Zugriffe&lt;br&gt;SAP"></asp:BoundColumn>
														<asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kundennr."></asp:BoundColumn>
														<asp:BoundColumn DataField="Customername" SortExpression="Customername" HeaderText="Kundenname"></asp:BoundColumn>
														<asp:BoundColumn DataField="AccountingArea" SortExpression="AccountingArea" HeaderText="AccountingArea"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Testbenutzer">
															<HeaderStyle Width="100px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="Checkbox4" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Start ASPX" HeaderText="Start ASPX">
															<HeaderStyle Width="150px"></HeaderStyle>
															<ItemTemplate>
																<asp:HyperLink id="Hyperlink4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Start ASPX") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Ende ASPX") %>'>
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Dauer ASPX" HeaderText="Dauer">
															<HeaderStyle HorizontalAlign="Center" Width="205px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<TABLE id="Table18" cellSpacing="0" cellPadding="0" border="0">
																	<TR>
																		<td width="50">ASPX</td>
																		<TD width="30" align="right">
																			<asp:Label id="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer ASPX") %>'> </asp:Label>&nbsp;</TD>
																		<TD width="125">
																			<asp:Label id="Label5" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer ASPX"))) %>' Height="10px" BackColor="#8080FF"> </asp:Label></TD>
																	</TR>
																	<TR>
																		<td width="25">SAP</td>
																		<TD width="30" align="right">
																			<asp:Label id="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer SAP") %>'> </asp:Label>&nbsp;</TD>
																		<TD width="125">
																			<asp:Label id="Label7" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer SAP"))) %>' Height="10px" BackColor="Highlight"> </asp:Label></TD>
																	</TR>
																</TABLE>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
												</DBWC:HIERARGRID>
											</td>
										</tr>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server" 
                                        Font-Bold="True" ForeColor="Red"></asp:label></TD>
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
