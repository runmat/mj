<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change08_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change08_2" %>
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
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Equnr" SortExpression="Equnr" HeaderText="Equipment"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="MANDT" HeaderText="col_Anfordern">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_Anfordern" runat="server" CommandName="Sort" CommandArgument="MANDT">col_Anfordern</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox id=chk0000 runat="server" Visible='<%# NOT Trim(Eval("MANDT").ToString())="11" %>' Checked="true">
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="SWITCH" HeaderText="col_VersandartAendern">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_VersandartAendern" runat="server" CommandName="Sort" CommandArgument="SWITCH">col_VersandartAendern</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox id=chkSWITCH runat="server" Visible='<%# Eval("STATUS")="Temporär versendet" %>' Checked='<%# Eval("SWITCH") %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:HyperLink id=VIN runat="server" NavigateUrl='<%# Eval("CHASSIS_NUM") %>' Text='<%# Eval("CHASSIS_NUM") %>' ToolTip="Anzeige Fahrzeughistorie" Target="_blank">
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Leasingvertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Leasingvertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Leasingvertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="Label1" runat="server" Text='<%# Eval("LIZNR") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("LIZNR") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
															<HeaderTemplate>
																<asp:LinkButton id="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="Label2" runat="server" Text='<%# Eval("TIDNR") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox ID="TextBox2" runat="server" Text='<%# Eval("TIDNR") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="Label3" runat="server" Text='<%# Eval("LICENSE_NUM") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox ID="TextBox3" runat="server" Text='<%# Eval("LICENSE_NUM") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Ordernummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Ordernummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Ordernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="Label4" runat="server" Text='<%# Eval("ZZREFERENZ1") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox ID="TextBox4" runat="server" Text='<%# Eval("ZZREFERENZ1") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="EXPIRY_DATE" HeaderText="col_Abmeldedatum">
															<HeaderTemplate>
																<asp:LinkButton id="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="EXPIRY_DATE">col_Abmeldedatum</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=lblNoDate runat="server" Font-Bold="True" Visible='<%# String.IsNullOrEmpty(Eval("EXPIRY_DATE").ToString()) %>' ForeColor="Red">XX.XX.XXXX</asp:Label>
																<asp:Label id=lblYesDate runat="server" Visible='<%# Not String.IsNullOrEmpty(Eval("EXPIRY_DATE").ToString()) %>' Text='<%# IIf(String.IsNullOrEmpty(Eval("EXPIRY_DATE").ToString()), "", Eval("EXPIRY_DATE").ToString().Replace(" 00:00:00", "")) %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZCOCKZ" HeaderText="col_CoCvorhanden">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_CoCvorhanden" runat="server" CommandName="Sort" CommandArgument="ZZCOCKZ">col_CoCvorhanden</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# Trim(Eval("ZZCOCKZ").ToString())="X" %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# Eval("ZZCOCKZ") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="STATUS" HeaderText="col_Status">
															<HeaderTemplate>
																<asp:LinkButton id="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="Label5" runat="server" Text='<%# Eval("STATUS") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox ID="TextBox5" runat="server" Text='<%# Eval("STATUS") %>'>
																</asp:TextBox>
															</EditItemTemplate>
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

