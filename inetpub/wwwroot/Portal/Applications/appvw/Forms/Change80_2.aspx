<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80_2.aspx.vb" Inherits="AppVW.Change80_2" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdConfirm" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Bestätigen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;&nbsp;&nbsp;
												<asp:linkbutton id="lnkCreateExcel" runat="server">Excelformat</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" height="41">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" align="left" width="618" height="9"><asp:label id="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:label>
														<TD noWrap align="right" height="9">
															<P align="right">&nbsp;
																<asp:dropdownlist id="ddlPageSize" runat="server" Height="14px" AutoPostBack="True"></asp:dropdownlist></P>
														</TD>
													</TR>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White" Width="100%">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="REFERENZ2" SortExpression="REFERENZ2" HeaderText="Vorhaben"></asp:BoundColumn>
														<asp:BoundColumn DataField="VHB_TEIL" SortExpression="VHB_TEIL" HeaderText="Vorhaben Teil"></asp:BoundColumn>
														<asp:BoundColumn DataField="FZGTYP" SortExpression="FZGTYP" HeaderText="Fahrzeugtyp"></asp:BoundColumn>
														<asp:BoundColumn DataField="REFERENZ1" SortExpression="REFERENZ1" HeaderText="IKZ"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="CHASSIS_NUM1" SortExpression="CHASSIS_NUM1" HeaderText="Fahrgestellnummer">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn SortExpression="CHASSIS_NUM2">
															<ItemStyle HorizontalAlign="Left"></ItemStyle>
															<ItemTemplate>
																<asp:TextBox id=txtVIN2 runat="server" Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM2") %>' MaxLength="6" Enabled='<%# DataBinder.Eval(Container, "DataItem.WAEHLBAR") %>'>
																</asp:TextBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="FCODE" SortExpression="FCODE" HeaderText="Bemerkung"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
