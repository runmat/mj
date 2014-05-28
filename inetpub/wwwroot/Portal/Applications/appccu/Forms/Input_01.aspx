<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Input_01.aspx.vb" Inherits="AppCCU.Input_01"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<BODY leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" noWrap colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>(
									<asp:label id="lblPageTitle" runat="server">Zusammenstellung von Abfragekriterien</asp:label>)
								</TD>
							</TR>
							<TR>
								<TD class="TaskTitle" noWrap colSpan="2">&nbsp;</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TextHeader"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">Report erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calDatum" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar><asp:textbox id="txtField" runat="server" Width="50px" Visible="False">0</asp:textbox></TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="" vAlign="top">
												<TABLE id="Table1" cellSpacing="2" cellPadding="1" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="" vAlign="center" noWrap width="169">Amtl. Kennzeichen
														</TD>
														<TD class="" vAlign="center" noWrap><asp:textbox id="txtKennzeichenVon" runat="server" Width="100px"></asp:textbox></TD>
														<TD class="" vAlign="center" width="100%"><asp:textbox id="txtKennzeichenBis" runat="server" Width="100px" Visible="False"></asp:textbox>&nbsp;(FL-X1000)&nbsp;</TD>
													</TR>
													<TR>
														<TD class="" vAlign="center" width="169">
															Fahrgestellnummer
														</TD>
														<TD class="" vAlign="center" noWrap><asp:textbox id="txtFahrgestellVon" runat="server" Width="100px"></asp:textbox></TD>
														<TD class="" vAlign="center" width="100%"><asp:textbox id="txtFahrgestellBis" runat="server" Width="100px" Visible="False"></asp:textbox>&nbsp;(WDX11111111111111)</TD>
													</TR>
													<TR>
														<TD class="" vAlign="center" width="169">
															Referenznr.</TD>
														<TD class="" vAlign="center"><asp:textbox id="txtLeasVVon" runat="server" Width="100px"></asp:textbox></TD>
														<TD class="" vAlign="center" width="100%"><asp:textbox id="txtLeasVBis" runat="server" Width="100px" Visible="False"></asp:textbox>&nbsp;(1000000012)</TD>
													</TR>
													<TR>
														<TD vAlign="center" width="169">Kundennummer</TD>
														<TD vAlign="center"><asp:textbox id="txtKundennr" runat="server" Width="100px"></asp:textbox></TD>
														<TD vAlign="center" width="100%">&nbsp;(23632)</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" noWrap colSpan="3">
															<HR width="100%" SIZE="1">
														</TD>
													</TR>
													<TR>
														<TD vAlign="top" noWrap width="200" colSpan="2"><asp:radiobuttonlist id="rbStatus" runat="server">
																<asp:ListItem Value="ALL" Selected="True">Alle</asp:ListItem>
																<asp:ListItem Value="DAD1">Angelegt beim DAD</asp:ListItem>
																<asp:ListItem Value="LEA1">Beim LN</asp:ListItem>
																<asp:ListItem Value="DAD2">Zur&#252;ck vom LN</asp:ListItem>
																<asp:ListItem Value="VER1">Bei Versicherung</asp:ListItem>
																<asp:ListItem Value="DAD3">Zur&#252;ck von Versicherung</asp:ListItem>
															</asp:radiobuttonlist></TD>
														<TD vAlign="top" width="100%"><asp:radiobuttonlist id="rbMahnung" runat="server" Visible="False">
																<asp:ListItem Value="MALL" Selected="True">Alle</asp:ListItem>
																<asp:ListItem Value="M1LN">Stufe 1 LN</asp:ListItem>
																<asp:ListItem Value="M2LN">Stufe 2 LN</asp:ListItem>
																<asp:ListItem Value="M3LN">Stufe 3 LN</asp:ListItem>
																<asp:ListItem Value="M4LN">Stufe 4 LN</asp:ListItem>
																<asp:ListItem Value="M1VG">Stufe 1 VG</asp:ListItem>
																<asp:ListItem Value="M2VG">Stufe 2 VG</asp:ListItem>
																<asp:ListItem Value="M3VG">Stufe 3 VG</asp:ListItem>
																<asp:ListItem Value="M4VG">Stufe 4 VG</asp:ListItem>
															</asp:radiobuttonlist><asp:radiobuttonlist id="rbSelect" runat="server" Visible="False" Height="49px">
																<asp:ListItem Value="H" Selected="True">Historie</asp:ListItem>
																<asp:ListItem Value="M">Mahnstufen</asp:ListItem>
															</asp:radiobuttonlist><asp:checkbox id="lblKF" runat="server" Visible="False" Text="nur Klärfälle"></asp:checkbox></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
