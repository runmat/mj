<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change50_1.aspx.vb" Inherits="CKG.Components.ComCommon.Change50_1" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td colSpan="2">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</TR>
							<TR>
								<TD colSpan="2">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:linkbutton id="lb_Auswahl" runat="server" CssClass="TaskTitel"></asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<TR>
											<TD align="right">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD><asp:label id="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:label></TD>
														<TD align="right"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</TR>
													<tr>
														<td></td>
														<td></td>
													</tr>
												</table>
												&nbsp;&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge"></TD>
										</TR>
										<tr>
											<td>
											</td>
										</tr>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" CellPadding="0" Width="100%" BackColor="White" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" ReadOnly="True" DataField="EQUNR" SortExpression="EQUNR" HeaderText="EQUNR"></asp:BoundColumn>
														
														<asp:TemplateColumn SortExpression="Anforderer" HeaderText="col_Anforderer">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_Anforderer" runat="server" CommandName="Sort" CommandArgument="Anforderer">col_Anforderer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="lblAnforderer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anforderer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Haendlernummer" HeaderText="col_Haendlernummer">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_Haendlernummer" runat="server" CommandName="Sort" CommandArgument="Haendlernummer">col_Haendlernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="lblHaendlernummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_LIZNR">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_LIZNR" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_LIZNR</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
													    <asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="lnkHistorie"  Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' >
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Nummer ZB2" HeaderText="col_Briefnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="Nummer ZB2">col_Briefnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Nummer ZB2") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Memo" HeaderText="Memo">
															<ItemTemplate>
																<asp:Label id="lblMemo" Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Memo") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox Visible="True" ID="txtMemo" TextMode="MultiLine" Runat="server" Width="200px" Rows="4" BorderColor="red" BorderStyle="Solid" BorderWidth="1" Text='<%# DataBinder.Eval(Container, "DataItem.Memo") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Erstzulassung" SortExpression="Erstzulassung" ReadOnly="True" HeaderText="Erstzulassung" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Datum Ausgang" SortExpression="Datum Ausgang" ReadOnly="True" HeaderText="Datum Ausgang" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Datum Eingang" SortExpression="Datum Eingang" ReadOnly="True" HeaderText="Datum Eingang" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Details">
															<ItemTemplate>
																<P align="center">
																	<asp:Linkbutton id="Linkbutton1" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' CommandName="Details" Height="10px">
																		<img src="../../../Images/plus.gif" border="0">
																	</asp:Linkbutton></P>
															</ItemTemplate>
															<EditItemTemplate>
																<P align="center">
																	<asp:Linkbutton id="Linkbutton2" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' CommandName="Details" Height="10px">
																		<img src="../../../Images/minus.gif" border="0">
																	</asp:Linkbutton></P>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Erledigt">
															<ItemTemplate>
																<P align="center">
																	<asp:Linkbutton id="lbErledigt" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' CommandName="Erledigt" Height="10px">
																		<img src="../../../Images/Confirm_mini.gif" border="0"></asp:Linkbutton></P>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;" Position="Top"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				</TR></table>
		</form>
	</body>
</HTML>
