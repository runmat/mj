<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change43_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change43_2" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server">Fahrzeugauswahl</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calZul" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="" align="left" width="618" height="9"><strong><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></strong>
														<!--<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="/PageElements/Datum.htm" Target="_blank">Zulassungsdatum:</asp:HyperLink></TD> -->
														<TD noWrap align="right" height="9">
															<P align="right"><STRONG>Zulassungsdatum&nbsp;</STRONG><A href="javascript:openinfo('../../../PageElements/Datum.htm');"><STRONG><IMG src="/Portal/Images/fragezeichen.gif" border="0"></STRONG></A>&nbsp;&nbsp;
																<asp:textbox id="txtZulDatum" runat="server"></asp:textbox>&nbsp;
																<asp:linkbutton id="Linkbutton2" runat="server" CssClass="StandardButtonTable" Width="57px">Kalender</asp:linkbutton>&nbsp;
																<asp:dropdownlist id="ddlPageSize" runat="server" Height="14px" AutoPostBack="True"></asp:dropdownlist></P>
														</TD>
													</TR>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" DESIGNTIMEDRAGDROP="61" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Kontonummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Kontonummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>' ID="Label1" NAME="Label1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>' ID="Label2" NAME="Label2">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
															<HeaderTemplate>
																<asp:LinkButton id="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>' ID="Label3" NAME="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>' ID="Label4" NAME="Label4">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label5 runat="server" NAME="Label5" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" SortExpression="ZZCOCKZ" HeaderText="COC Besch.&lt;br&gt;vorhanden">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=Checkbox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Wunschkennz">
															<HeaderTemplate>
																<asp:LinkButton id="col_Wunschkennz" runat="server">col_Wunschkennz</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox id="txt_wunschkennz" runat="server" Width="93px" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
																</asp:TextBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="HEZ">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chk0003" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="100">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD width="100">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
							<TR id="ShowScript" runat="server">
								<TD width="100">&nbsp;</TD>
								<TD>
									<SCRIPT language="JavaScript">
										<!-- //
										// window.document.Form1.elements[window.document.Form1.length-3].focus();
										//-->
										function openinfo (url) {
												fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=550,height=150");
												fenster.focus();
										}
									</SCRIPT>
								</TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
