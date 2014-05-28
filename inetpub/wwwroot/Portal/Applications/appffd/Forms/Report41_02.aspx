<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report41_02.aspx.vb" Inherits="AppFFD.Report41_02"%>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server">Händlerübersicht</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="145">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="145"><asp:linkbutton id="cmdSelect" runat="server" CssClass="StandardButton"> &#149;&nbsp;Versandadressen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="145"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="True"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="145"><asp:hyperlink id="HyperLink1" runat="server" CssClass="StandardButton" NavigateUrl="Report41_02Print.aspx" Target="_blank">&#149;&nbsp;Druckversion</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="143"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" height="19" cellSpacing="0" cellPadding="0" width="800" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" height="828" cellSpacing="0" cellPadding="0" border="0">
										<tr>
											<td vAlign="top" align="left" width="782">
												<TABLE id="Table1" height="160" cellSpacing="0" cellPadding="5" border="0">
													<TR>
														<TD class="TextLarge" width="95" height="32"></TD>
														<TD class="TextLarge" width="258"></TD>
														<TD class="TextLarge" width="133"></TD>
														<TD class="TextLarge" width="140"></TD>
														<TD class="TextLarge"></TD>
													</TR>
													<TR id="trHaendlernummer" runat="server">
														<TD class="TextLarge" width="95" height="32">Händlernummer:&nbsp;</TD>
														<TD class="TextLarge" width="258"><asp:label id="lblHDNummer" runat="server"></asp:label></TD>
														<TD class="TextLarge" width="133">Lastschrift:</TD>
														<TD class="TextLarge" width="140"><asp:checkbox id="CheckBox3" runat="server" Enabled="False"></asp:checkbox>&nbsp;&nbsp; 
															seit:</TD>
														<TD class="TextLarge"><asp:label id="lblseit" runat="server"></asp:label></TD>
													</TR>
													<TR id="trName" runat="server">
														<TD class="TextLarge" width="95" height="32">Name:&nbsp;</TD>
														<TD class="TextLarge" width="258"><asp:label id="lblName" runat="server"></asp:label></TD>
														<TD class="TextLarge" width="133">letzte Änderung am :</TD>
														<TD class="TextLarge" width="140"></TD>
														<TD class="TextLarge"></TD>
													</TR>
													<TR id="trOrt" runat="server">
														<TD class="TextLarge" width="95" height="32">Adresse:&nbsp;</TD>
														<TD class="TextLarge" width="258"><asp:label id="lblAdresse" runat="server"></asp:label></TD>
														<TD class="TextLarge" width="133">Gehört zu:</TD>
														<TD class="TextLarge" width="140"><asp:label id="lblDistrikt" runat="server"></asp:label></TD>
														<TD class="TextLarge"></TD>
													</TR>
													<TR id="trDistrikt" runat="server" Visible="False">
														<TD class="TextLarge" width="95" height="32"></TD>
														<TD class="TextLarge" width="258"></TD>
														<TD class="TextLarge" width="133"></TD>
														<TD class="TextLarge" width="140"></TD>
														<TD class="TextLarge"></TD>
													</TR>
													<TR id="trHdAuswahl" runat="server">
														<TD class="TextLarge" width="95" height="32"></TD>
														<TD class="TextLarge" width="258"></TD>
														<TD class="TextLarge" width="133"></TD>
														<TD class="TextLarge" width="140"></TD>
														<TD class="TextLarge"></TD>
													</TR>
												</TABLE>
												<TABLE id="Table4" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR id="Tr2" runat="server">
														<TD height="32"><asp:datagrid id="DataGrid1" runat="server" BorderWidth="2px" GridLines="None" CellPadding="3" AutoGenerateColumns="False" Width="784px">
																<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
																<ItemStyle CssClass="TextLarge"></ItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:TemplateColumn Visible="False" HeaderText="ZeigeKontingentart">
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox2 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
																			</asp:CheckBox>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart" ItemStyle-Width="360px"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Kontingent">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		<ItemTemplate>
																			<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
																			</asp:Label>
																			<asp:Label id=Label2 runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>'>
																			</asp:Label>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="Ausschoepfung" HeaderText="Inanspruchnahme">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																	</asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Freies Kontingent">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		<ItemTemplate>
																			<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																			</asp:Label>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="Gesperrt">
																		<HeaderStyle HorizontalAlign="Center" Width="85px"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
															</asp:datagrid><asp:datagrid id="Datagrid3" runat="server" BorderWidth="2px" GridLines="None" CellPadding="3" AutoGenerateColumns="False" Width="784px">
																<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
																<ItemStyle CssClass="TextLarge"></ItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:TemplateColumn Visible="False" HeaderText="ZeigeKontingentart">
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox2 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
																			</asp:CheckBox>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Richtwert">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		<ItemTemplate>
																			<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
																			</asp:Label>
																			<asp:Label id=Label2 runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>'>
																			</asp:Label>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="Ausschoepfung" HeaderText="Inanspruchnahme">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																	</asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="" HeaderStyle-Width="100px">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		<ItemTemplate>
																			<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																			</asp:Label>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="" HeaderStyle-Width="100px">
																		<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
															</asp:datagrid><asp:datagrid id="Datagrid4" runat="server" BorderWidth="2px" GridLines="None" CellPadding="3" AutoGenerateColumns="False" Width="784px">
																<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
																<ItemStyle CssClass="TextLarge"></ItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="Gesamt" HeaderText="Gesamt">
																		<ItemStyle Width="350px"></ItemStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="Kontingente" HeaderText="Kontingente">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="Inanspruchnahme" HeaderText="Inanspruchnahme">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="DummyCol1" HeaderStyle-Width="100px"></asp:BoundColumn>
																	<asp:BoundColumn DataField="DummyCol2" HeaderStyle-Width="100px"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
													<TR>
														<TD><asp:datagrid id="Datagrid2" runat="server" GridLines="None" CellPadding="3" AutoGenerateColumns="False" Width="100%" BackColor="White" Height="156px">
																<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
																<ItemStyle CssClass="TextLarge"></ItemStyle>
																<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="KontingentID" HeaderText="KontingentID"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart">
																		<HeaderStyle Width="100px"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="Alte Zahlungsfrist" ReadOnly="True" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="F&#228;lligkeit "></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="" HeaderStyle-Width="164px"></asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="" HeaderStyle-Width="164px"></asp:TemplateColumn>
																	<asp:TemplateColumn Visible="False" HeaderText="Neue F&#228;lligkeit in Tagen"></asp:TemplateColumn>
																	<asp:BoundColumn Visible="False" HeaderText="ROW"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
												<asp:label id="lblMessage" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
										</tr>
									</TABLE>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
						</TABLE>
					</td>
					<TD vAlign="top"><!--#include File="../../../PageElements/Footer.html" --></TD>
				</tr>
			</table>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
