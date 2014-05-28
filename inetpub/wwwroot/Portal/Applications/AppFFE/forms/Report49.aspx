<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report49.aspx.vb" Inherits="AppFFE.Report49" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

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
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" noWrap colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> - Zusammenstellung von Abfragekriterien</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR id="tr_Datumab" runat="server">
														<TD class="TextLarge" vAlign="top" width="150">
															<asp:label id="lbl_Datumab" runat="server" EnableViewState="False"></asp:label>
														</TD>
														<TD class="TextLarge" vAlign="center"><asp:textbox id="txtAbDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectAb" runat="server" Width="30px" CausesValidation="False" Height="22px" Text="..."></asp:button>
															<asp:calendar id="calAbDatum" runat="server" Width="160px" Visible="False" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
													<TR id="tr_Datumbis" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" width="150">
															<asp:label id="lbl_Datumbis" runat="server" EnableViewState="False"></asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="center"><asp:textbox id="txtBisDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectBis" runat="server" Width="30px" CausesValidation="False" Height="22px" Text="..."></asp:button><asp:calendar id="calBisDatum" runat="server" Width="160px" Visible="False" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
															</TR>
													<TR id="tr_Auswahl" runat="server">
														<TD class="TextLarge" vAlign="top" width="150">
															<asp:label id="lbl_Abrufgr0" runat="server"  Text="lbl_Abrufauswahl"></asp:label>
														</TD>
														<TD>
														    <asp:RadioButton ID="rb_Temp" runat="server" Checked="True" GroupName="Auswahl" 
                                                                Text="rb_Temp" AutoPostBack="True" />
                                                            <asp:RadioButton ID="rb_Endg" runat="server" GroupName="Auswahl" 
                                                                Text="rb_Endg" AutoPostBack="True" />
                                                        </TD>
													</TR>
													
													<TR id="tr_Abruf" runat="server">
														<TD class="TextLarge" vAlign="top" width="150">
															<asp:label id="lbl_Abrufgr" runat="server"  Text="lbl_Abrufgr"></asp:label>
														</TD>
														<TD>
														    <asp:DropDownList ID="ddlAbrufgrund" runat="server">
                                                            </asp:DropDownList>
                                                        </TD>
													</TR>
													
												</TABLE>
												
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="59" height="18">&nbsp;</TD>
								<TD vAlign="top" height="18"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="59">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
