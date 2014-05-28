<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change80_2" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
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
											<TD vAlign="center" width="150"><asp:linkbutton id="lb_Back" runat="server" CssClass="StandardButton"> lb_Back</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;&nbsp;
												<asp:linkbutton id="lnkCreateExcel" runat="server">Excelformat Aufträge</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" height="41">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" align="left" width="618" height="9"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
														<TD noWrap align="right" height="9">
															<P align="right"></P>
														</TD>
													</TR>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
												<TABLE id="Table3" cellSpacing="0" cellPadding="5" border="0">
													<TR id="tr_Gebiet" runat="server">
														<TD class="TextLarge" noWrap align="right"><asp:label id="lbl_Gebiet" runat="server">lbl_Gebiet</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:label id="lblGebietShow" runat="server">lblGebietShow</asp:label></TD>
													</TR>
													<TR id="tr_Kunnr" runat="server">
														<TD class="TextLarge" noWrap align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lbl_Kunnr" runat="server">lbl_Kunnr</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><asp:label id="lblKunnrShow" runat="server">lblKunnrShow</asp:label></TD>
													</TR>
												</TABLE>
												<asp:label id="lblVbeln" runat="server" Visible="False"></asp:label><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="200" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Vbeln" SortExpression="Vbeln" HeaderText="Vbeln"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Vbeln" HeaderText="col_VBELN">
															<HeaderTemplate>
																<asp:LinkButton id="col_VBELN" runat="server" CommandName="Sort" CommandArgument="Vbeln">col_VBELN</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vbeln") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Txt30" HeaderText="col_TXT30">
															<HeaderTemplate>
																<asp:LinkButton id="col_TXT30" runat="server" CommandName="Sort" CommandArgument="Txt30">col_TXT30</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Txt30") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Wldat" HeaderText="col_WLDAT">
															<HeaderTemplate>
																<asp:LinkButton id="col_WLDAT" runat="server" CommandName="Sort" CommandArgument="Wldat">col_WLDAT</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wldat", "{0:dd.MM.yyyy}") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Bemerkung" HeaderText="col_Bemerkung">
															<HeaderTemplate>
																<asp:LinkButton id="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="Bemerkung">col_Bemerkung</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Zzsend0" HeaderText="col_ZZSEND0">
															<HeaderTemplate>
																<asp:LinkButton id="col_ZZSEND0" runat="server" CommandArgument="Zzsend0" CommandName="Sort">col_ZZSEND0</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" ForeColor="ForestGreen" Text='<%# DataBinder.Eval(Container, "DataItem.Zzsend0") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Zzsend1" HeaderText="col_ZZSEND1">
															<HeaderTemplate>
																<asp:LinkButton id="col_ZZSEND1" runat="server" CommandArgument="Zzsend1" CommandName="Sort">col_ZZSEND1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label2 runat="server" ForeColor="Gold" Text='<%# DataBinder.Eval(Container, "DataItem.Zzsend1") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Zzsend2" HeaderText="col_ZZSEND2">
															<HeaderTemplate>
																<asp:LinkButton id="col_ZZSEND2" runat="server" CommandArgument="Zzsend2" CommandName="Sort">col_ZZSEND2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label3 runat="server" ForeColor="Red" Text='<%# DataBinder.Eval(Container, "DataItem.Zzsend2") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id="lb_Auswahl" runat="server" CssClass="StandardButtonTable" Text="Auswählen" CausesValidation="false" CommandName="Select">lb_Auswahl</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												&nbsp;<br>
												<b>
													<asp:linkbutton id="lnkCreateExcel2" runat="server" Visible="False">Excelformat Fahrzeuge</asp:linkbutton></b><br>
												&nbsp;<br>
												<asp:datagrid id="Datagrid2" runat="server" Visible="False" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="Chassis_num" HeaderText="col_CHASSIS_NUM">
															<HeaderTemplate>
																<asp:LinkButton id="col_CHASSIS_NUM" runat="server" CommandName="Sort" CommandArgument="Chassis_num">col_CHASSIS_NUM</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Chassis_num") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="License_num" HeaderText="col_LICENSE_NUM">
															<HeaderTemplate>
																<asp:LinkButton id="col_LICENSE_NUM" runat="server" CommandName="Sort" CommandArgument="License_num">col_LICENSE_NUM</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.License_num") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Modell" HeaderText="col_MODELL">
															<HeaderTemplate>
																<asp:LinkButton id="col_MODELL" runat="server" CommandName="Sort" CommandArgument="Modell">col_MODELL</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modell") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Finart" HeaderText="col_FINART">
															<HeaderTemplate>
																<asp:LinkButton id="col_FINART" runat="server" CommandName="Sort" CommandArgument="Finart">col_FINART</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Finart") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Farbe_soll" HeaderText="col_FARBE_SOLL">
															<HeaderTemplate>
																<asp:LinkButton id="col_FARBE_SOLL" runat="server" CommandName="Sort" CommandArgument="Farbe_soll">col_FARBE_SOLL</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe_soll") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Farbe_ist" HeaderText="col_FARBE_IST">
															<HeaderTemplate>
																<asp:LinkButton id="col_FARBE_IST" runat="server" CommandName="Sort" CommandArgument="Farbe_ist">col_FARBE_IST</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe_ist") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Km_stand_soll" HeaderText="col_KM_STAND_SOLL">
															<HeaderTemplate>
																<asp:LinkButton id="col_KM_STAND_SOLL" runat="server" CommandName="Sort" CommandArgument="Km_stand_soll">col_KM_STAND_SOLL</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Km_stand_soll") %>' ID="Label5" NAME="Label5">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Km_stand_ist" HeaderText="col_KM_STAND_IST">
															<HeaderTemplate>
																<asp:LinkButton id="col_KM_STAND_IST" runat="server" CommandName="Sort" CommandArgument="Km_stand_ist">col_KM_STAND_IST</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Km_stand_ist") %>' ID="Label6" NAME="Label6">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Prue_erg" HeaderText="col_PRUE_ERG">
															<HeaderTemplate>
																<asp:LinkButton id="col_PRUE_ERG" runat="server" CommandArgument="Prue_erg" CommandName="Sort">col_PRUE_ERG</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label8 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Prue_erg") = "grün" %>' ForeColor="ForestGreen" Text='<%# DataBinder.Eval(Container, "DataItem.Prue_erg") %>' NAME="Label4">
																</asp:Label>
																<asp:Label id=Label7 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Prue_erg") = "gelb" %>' ForeColor="Gold" Text='<%# DataBinder.Eval(Container, "DataItem.Prue_erg") %>' NAME="Label4">
																</asp:Label>
																<asp:Label id=Label9 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Prue_erg") = "rot" %>' ForeColor="Red" Text='<%# DataBinder.Eval(Container, "DataItem.Prue_erg") %>' NAME="Label4">
																</asp:Label>
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
