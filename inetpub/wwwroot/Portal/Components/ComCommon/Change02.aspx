<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change02.aspx.vb" Inherits="CKG.Components.ComCommon.Change02" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
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
											<TD class="TaskTitle" width="120">&nbsp;
												<asp:imagebutton id="ImageButton1" runat="server" ImageUrl="../../images/empty.gif"></asp:imagebutton></TD>
										</TR>
										<TR id="trWeiter" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="lb_Weiter" runat="server" CssClass="StandardButton">lb_Weiter</asp:linkbutton></TD>
										</TR>
										<TR id="trConfirm" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="lb_Confirm" runat="server" CssClass="StandardButton">lb_Confirm</asp:linkbutton></TD>
										</TR>
										<TR id="trBack" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="lb_Back" runat="server" CssClass="StandardButton">lb_Back</asp:linkbutton></TD>
										</TR>
										<TR id="trNew" runat="server">
											<TD vAlign="center" width="120"><asp:linkbutton id="lb_New" runat="server" CssClass="StandardButton">lb_New</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" height="192">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" border="0">
													<TR id="tr_Haendler" runat="server">
														<TD class="TextLarge" noWrap align="right">&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lbl_Haendler" runat="server">lbl_Haendler</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:dropdownlist id="ddlHaendler" runat="server" Width="350px"></asp:dropdownlist><asp:label id="lblHaendlerShow" runat="server" Visible="False"></asp:label></TD>
													</TR>
													<TR id="tr_Region" runat="server">
														<TD class="TextLarge" noWrap align="right">&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lbl_Region" runat="server">lbl_Region</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtRegion" runat="server" Width="350px" MaxLength="5"></asp:textbox><asp:label id="lblRegionShow" runat="server" Visible="False"></asp:label></TD>
													</TR>
													<TR id="tr_Wldat" runat="server">
														<TD class="TextLarge" vAlign="top" noWrap align="right">&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lbl_Wldat" runat="server">lbl_Wldat</asp:label>&nbsp;&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:radiobutton id="rb_Tag" runat="server" Width="110px" AutoPostBack="True" Text="rb_Tag" GroupName="grpWldat" Checked="True"></asp:radiobutton><asp:radiobutton id="rb_Woche" runat="server" Width="110px" AutoPostBack="True" Text="rb_Woche" GroupName="grpWldat"></asp:radiobutton><asp:radiobutton id="rb_Monat" runat="server" Width="110px" AutoPostBack="True" Text="rb_Monat" GroupName="grpWldat"></asp:radiobutton><asp:label id="lblWldatShow" runat="server" Visible="False"></asp:label><BR>
															<asp:textbox id="txtWldat" runat="server" Width="315px" MaxLength="10"></asp:textbox>&nbsp;&nbsp;
															<asp:button id="btnOpenSelectWldat" runat="server" Width="30px" Text="..." CausesValidation="False" Height="22px"></asp:button><BR>
															<asp:calendar id="calWldat" runat="server" Visible="False" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
													<TR id="tr_Dienstleistung" runat="server">
														<TD class="TextLarge" vAlign="top" noWrap align="right">&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lbl_Dienstleistung" runat="server">lbl_Dienstleistung</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:radiobutton id="rb_Buchpruefung" runat="server" Text="rb_Buchpruefung" GroupName="grpDienstleistung" Checked="True"></asp:radiobutton><asp:literal id="Literal2" runat="server"></asp:literal><asp:radiobutton id="rb_KoerperlichePruefung" runat="server" Text="rb_KoerperlichePruefung" GroupName="grpDienstleistung"></asp:radiobutton><asp:literal id="Literal3" runat="server"></asp:literal><asp:radiobutton id="rb_Vollpruefung" runat="server" Text="rb_Vollpruefung" GroupName="grpDienstleistung"></asp:radiobutton></TD>
													</TR>
												</TABLE>
												<br>
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
								<td><!--#include File="../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
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
