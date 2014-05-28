<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report01.aspx.vb" Inherits="AppKroschke.Report01" %>
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
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120" height="350">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="75" height="19"></TD>
										</TR>
										<TR>
											<TD width="75"></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150" height="19"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButtonTable" Width="100px">&#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calVon" runat="server" BorderStyle="Solid" BorderColor="Black" CellPadding="0" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar><asp:calendar id="calBis" runat="server" BorderStyle="Solid" BorderColor="Black" CellPadding="0" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></TD>
								<TD vAlign="top" height="350">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="448" height="19"></TD>
											<TD class="TaskTitle" vAlign="top" height="19"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" width="100%"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" width="100%">
												<P>Bitte geben Sie Ihre Selektion ein:</P>
											</TD>
											<TD vAlign="top" height="19"></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" height="282">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="middle" noWrap width="207" height="49">
															<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
																<TR>
																	<TD class="TextLarge" width="72">Kunde<FONT color="red">*</FONT></TD>
																	<TD align="right"><asp:textbox id="txtKundennummer" runat="server" Width="62px" MaxLength="8"></asp:textbox></TD>
																</TR>
															</TABLE>
														</TD>
														<TD class="TextLarge" vAlign="middle" noWrap width="100%" height="49">
															<TABLE id="Table8" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD><asp:dropdownlist id="ddlKunnr" runat="server" Width="377px" EnableViewState="False" Font-Names="Courier New" Font-Bold="True"></asp:dropdownlist><INPUT id="txtDummy" type="hidden" size="14" name="txtDummy" runat="server"></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" noWrap width="206">Spezieller Datensatz:</TD>
														<TD class="TextLarge" vAlign="middle" width="100%"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" noWrap width="206">Referenz:</TD>
														<TD class="TextLarge" vAlign="middle" width="100%"><asp:textbox id="txtReferenz" runat="server"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" noWrap width="206">Kennzeichen:</TD>
														<TD class="TextLarge" vAlign="middle" width="100%">
															<P><asp:textbox id="txtKennzeichen" runat="server"></asp:textbox></P>
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" noWrap width="206"></TD>
														<TD class="TextLarge" vAlign="middle" width="100%"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" noWrap width="206">Unspezifische Suche:</TD>
														<TD class="TextLarge" vAlign="middle" width="100%"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="middle" noWrap width="206">Zulassungsdatum von:
														</TD>
														<TD class="TextLarge" vAlign="middle" width="100%"><asp:textbox id="txtZulassungsdatumVon" runat="server"></asp:textbox><asp:label id="lblInputReq" runat="server" CssClass="TextError">**</asp:label>&nbsp;
															<asp:linkbutton id="btnVon" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="middle" width="206">Zulassungsdatum bis 
															:</TD>
														<TD class="StandardTableAlternate" vAlign="middle"><asp:textbox id="txtZulassungsdatumBis" runat="server"></asp:textbox><asp:label id="Label1" runat="server" CssClass="TextError">**</asp:label>&nbsp;
															<asp:linkbutton id="btnBis" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="middle" width="206"></TD>
														<TD class="StandardTableAlternate" vAlign="middle"></TD>
													</TR>
												</TABLE>
												<TABLE id="Table5" height="34" cellSpacing="1" cellPadding="1" width="561" border="0">
													<TR>
														<TD><asp:radiobutton id="RB_G" runat="server" Checked="True" GroupName="test" Text="Alle Datensätze"></asp:radiobutton></TD>
														<TD width="229"><asp:radiobutton id="RB_A" runat="server" GroupName="test" Text="durchgeführte Zulassungen"></asp:radiobutton></TD>
														<TD><asp:radiobutton id="RB_Leer" runat="server" GroupName="test" Text="offene Zulassungen"></asp:radiobutton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="top" align="left"><asp:label id="lblError" runat="server" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><FONT color="red">* Eingabe erforderlich</FONT></TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top"><asp:label id="Label2" runat="server" CssClass="TextError">**</asp:label><asp:label id="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False"> Format: TT.MM.JJJJ. Der Datumsbereich darf maximal zwei Monate (60 Tage) umfassen.</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<SCRIPT language="JavaScript">
								function SetKnr()
								{	var found = 0;
									var s;
								
									for (var i = 0; i<=document.Form1.ddlKunnr.value.length-1; i++)
									{
										s = String(document.Form1.ddlKunnr.value);
										if (s.charAt(i) != '0')
										{
											found=i;
											break;
										}										
									}						
									s = String(document.Form1.ddlKunnr.value);
									document.Form1.txtKundennummer.value = String(s).substring(found,String(s).length)							
									//20061102
									document.Form1.txtDummy.value = document.Form1.ddlKunnr.selectedIndex;
								}

								function SetKunnr()
								{									
									document.Form1.txtDummy.value = "";
									document.Form1.ddlKunnr.selectedIndex = 0;
									for (var i = 0; i <= document.Form1.ddlKunnr.length-1; i++)
									{
										if (String(parseInt(document.Form1.ddlKunnr.options[i].value,10)).substr(0, document.Form1.txtKundennummer.value.length) == document.Form1.txtKundennummer.value.toUpperCase())
										{
											document.Form1.ddlKunnr.selectedIndex = i;
											document.Form1.txtDummy.value = document.Form1.ddlKunnr.selectedIndex;											
											break;
										}
									}
								}
								if (typeof window.event != 'undefined')
									document.onkeydown = function()
									{
										if (event.srcElement.tagName.toUpperCase() != 'INPUT')
										return (event.keyCode != 8);
									}
								else
									document.onkeypress = function(e)
									{
										if (e.target.nodeName.toUpperCase() != 'INPUT')
										return (e.keyCode != 8);
								}
								
								function showhide()
								{
									o = document.getElementById("trAdresse").style;
									
									if (o.display != "none"){
											o.display = "none";											
										} else {											
											o.display = "";											
									}					
								}

				</SCRIPT>
			</TABLE>
		</form>
	</body>
</HTML>
