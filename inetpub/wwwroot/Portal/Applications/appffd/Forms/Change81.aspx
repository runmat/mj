<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change81.aspx.vb" Inherits="AppFFD.Change81" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server" Visible="False"> (Fahrzeugsuche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120" height="192">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="120">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="120"><asp:linkbutton id="cmdUpload" runat="server" CssClass="StandardButton"> Datei-Upload</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="120"><asp:linkbutton id="cmdContinue" runat="server" CssClass="StandardButton"> Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" height="192">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="TextLarge" noWrap align="right">Händlernummer&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtHaendlernummer" runat="server" MaxLength="10" Width="250px"></asp:textbox>&nbsp; 
															&nbsp;
															<asp:linkbutton id="lnkHaendlerSuchen" runat="server" CssClass="StandardButton">Existenz prüfen</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">Name&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtName" runat="server" MaxLength="40" Width="250px" Enabled="False" CssClass="InfoBoxFlat"></asp:textbox>&nbsp;&nbsp;
															<asp:linkbutton id="lnkAdresseManuell" runat="server" CssClass="StandardButton">Adresse manuell</asp:linkbutton></TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">Straße&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtStrasse" runat="server" MaxLength="60" Width="250px" Enabled="False" CssClass="InfoBoxFlat"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" noWrap align="right">PLZ / Ort&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtPLZ" runat="server" MaxLength="5" Width="50px" Enabled="False" CssClass="InfoBoxFlat"></asp:textbox>/
															<asp:textbox id="txtOrt" runat="server" MaxLength="40" Width="195px" Enabled="False" CssClass="InfoBoxFlat"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" noWrap align="right">Versandart&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge">
                                                            <asp:RadioButtonList ID="rblVersandart" runat="server">
                                                                <asp:ListItem Selected="True">Versand Standardbrief Deutsche Post AG</asp:ListItem>
                                                                <asp:ListItem >Standardversand mit Sendungsverfolgung und Bestätigung vom Empfänger</asp:ListItem>
                                                                <asp:ListItem >Expressversand mit Zeitoption vor 12:00 Uhr</asp:ListItem>
                                                                <asp:ListItem >Expressversand mit Zeitoption vor 10:00 Uhr</asp:ListItem>
                                                                <asp:ListItem >Expressversand mit Zeitoption vor 09:00 Uhr</asp:ListItem>
                                                                <asp:ListItem >Selbstabholer beim DAD Ahrensburg</asp:ListItem>
                                                            </asp:RadioButtonList>
														</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="top" noWrap align="right">Versandcode&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate">
															<table cellSpacing="0" cellPadding="0" border="0">
																<tr>
																	<td class="StandardTableAlternate" width="100"><asp:radiobutton id="rbVK" runat="server" Text="VK" GroupName="grpVersandcode" Checked="True"></asp:radiobutton></td>
																	<td class="StandardTableAlternate" width="100">&nbsp;&nbsp;&nbsp;<asp:radiobutton id="rbVKLN" runat="server" Text=" VKLN" GroupName="grpVersandcode"></asp:radiobutton></td>
																</tr>
															</table>
														</TD>
													</TR>
													<TR id="trCalendar" runat="server">
														<TD class="TextLarge" vAlign="top" noWrap align="right">gewünschtes 
															Durchführungsdatum&nbsp;&nbsp;&nbsp;<BR>
															(tt.mm.jjjj)&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtDurchfuehrungsDatum" runat="server" Width="180px"></asp:textbox>&nbsp;&nbsp;
															<asp:button id="btnOpenSelectAb" runat="server" Width="30px" Text="..." Height="22px" CausesValidation="False"></asp:button><BR>
															<asp:calendar id="calAbDatum" runat="server" Visible="False" Width="160px" CellPadding="0" BorderStyle="Solid" BorderColor="Black">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
									<TABLE id="tblUpload" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="tbl0001" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="TextLarge" noWrap align="right">Dateiauswahl <A href="javascript:openinfo('Info01.htm');">
																<IMG src="../../../images/fragezeichen.gif" border="0"></A>:&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><INPUT id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">&nbsp;</TD>
														<TD class="TextLarge">&nbsp;
															<asp:label id="lblExcelfile" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<!-- ,width=650,height=250 -->
		<asp:literal id="Literal1" runat="server"></asp:literal>
		<SCRIPT language="JavaScript">										
				<!--
						function openinfo (url) {
								fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0");
								fenster.focus();
						}
				-->
		</SCRIPT>
	</body>
</HTML>
