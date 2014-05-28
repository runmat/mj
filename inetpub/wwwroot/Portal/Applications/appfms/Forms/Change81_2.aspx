<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change81_2.aspx.vb" Inherits="AppFMS.Change81_2" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" height="41">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" align="left" width="618" height="9"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
														<TD noWrap align="right" height="9">
															<P align="right">&nbsp;
																<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Height="14px"></asp:dropdownlist></P>
														</TD>
													</TR>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<tr>
											<TD class="TextLarge" vAlign="top" align="left" colSpan="3">
												<P>&nbsp;Sachbearbeiternummer:&nbsp;&nbsp;
													<asp:dropdownlist id="ddlSachbearbeiter" runat="server" AutoPostBack="True"></asp:dropdownlist><BR>
													&nbsp;&nbsp;
												</P>
											</TD>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Equnr" SortExpression="Equnr" HeaderText="Equipment"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Anfordern">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=chk0000 runat="server" Visible='<%# NOT Trim(DataBinder.Eval(Container, "DataItem.MANDT"))="11" %>' Checked='<%# Trim(DataBinder.Eval(Container, "DataItem.MANDT"))="99" %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Auftragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="Kfz-Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kfz-Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" HeaderText="Ordernummer"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="ZZCOCKZ" HeaderText="CoC&lt;br&gt;vorh.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=CheckBox1 runat="server" Enabled="False" Checked='<%# Trim(DataBinder.Eval(Container, "DataItem.ZZCOCKZ"))="X" %>'>
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="STATUS" SortExpression="STATUS" HeaderText="Status"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Sachbearbeiter" HeaderText="Sachbearb.-Nr.">
															<ItemTemplate>
																<asp:TextBox id=txtSachbearbeiter runat="server" Width="79px" Text='<%# DataBinder.Eval(Container, "DataItem.Sachbearbeiter") %>' MaxLength="10" Enabled='<%# NOT Trim(DataBinder.Eval(Container, "DataItem.MANDT"))="11" %>'>
																</asp:TextBox>
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
