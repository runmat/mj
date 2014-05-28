<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report07.aspx.vb" Inherits="AppPorsche.Report07"%>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
			&nbsp;
			<TABLE id="Table4" height="467" width="100%">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Selektionskriterien)</asp:label></TD>
				</TR>
				<TR>
					<TD class="TaskTitle" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" height="368" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" noWrap width="100" height="288"><asp:calendar id="calVon" runat="server" Visible="False" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></TD>
								<TD vAlign="top" height="288">
									<TABLE class="BorderLeftBottom" id="Table1" cellSpacing="0" cellPadding="2" bgColor="white" border="0" height="271">
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap colSpan="5"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" width="10" colSpan="1" rowSpan="1"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="457" colSpan="2"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="94"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="10"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" width="10"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="457" colSpan="2">Stichtag:&nbsp;
												<asp:textbox id="txtStichtag" runat="server" Width="188px"></asp:textbox>&nbsp;
												<asp:linkbutton id="btnVon" runat="server" Width="100px" CssClass="StandardButtonTable"> &#149;&nbsp;Kalender...</asp:linkbutton></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="94">
												<P align="right">&nbsp;</P>
											</TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="10"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" width="10"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="457" colSpan="2">&nbsp;</TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="94"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="10"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="457" colSpan="2"><asp:label id="lblTitel" runat="server">Aktueller Händlerbestand:</asp:label></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="94"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle"></TD>
											<TD class="TextLarge" vAlign="middle" colSpan="3" width="568"><asp:listbox id="lstHaendler" runat="server" Width="564px" Height="100px"></asp:listbox></TD>
											<TD class="TextLarge" vAlign="middle"></TD>
											<TD class="TextLarge" vAlign="middle"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="457" colSpan="2"><asp:checkbox id="cbxAlle" runat="server" Text="Alle Händler anzeigen"></asp:checkbox></TD>
											<TD class="TextLarge" vAlign="middle" width="94">
												<asp:linkbutton id="cmdCreate" runat="server" Width="100px" CssClass="StandardButton" Height="10px"> &#149;&nbsp;Weiter</asp:linkbutton>
											</TD>
											<TD class="TextLarge" vAlign="middle" width="11"><P align="right">&nbsp;</P>
											</TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle"></TD>
											<TD class="TextLarge" vAlign="middle" width="457" colSpan="2"><asp:label id="Label1" runat="server" Width="158px" EnableViewState="False" CssClass="TextInfo"> * Format: TT.MM.JJJJ</asp:label></TD>
											<TD class="TextLarge" vAlign="middle" width="94"><P align="right">&nbsp;</P>
											</TD>
											<TD class="TextLarge" vAlign="middle"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" height="42">&nbsp;</TD>
								<TD width="100%" height="42"><!--#include File="../../../PageElements/Footer.html" -->
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			</TD></TR></TABLE></TD></TR></TABLE></TD></TR></TABLE></TD></TR></TABLE></form>
	</body>
</HTML>
