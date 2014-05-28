<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change06_2.aspx.vb" Inherits="AppPorsche.Change06_2" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD colSpan="3">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server"></asp:label>)</td>
							</TR>
							<TR>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="100" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdReset" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">
												<asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle">Händlersuche</asp:hyperlink>&nbsp;</TD>
										</TR>
										<TR>
											<TD class="" vAlign="top">
												<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
													<TR>
														<TD width="100%">
															<TABLE class="TableKontingent" id="Table3" height="0" cellSpacing="0" cellPadding="3" bgColor="white" border="0">
																<TR>
																	<TD class="TextLarge" vAlign="top" noWrap>Händler-Nr:</TD>
																	<TD class="TextLarge" vAlign="top" colSpan="2">
																		<asp:label id="lblHaendlerNummer" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="StandardTableAlternate" vAlign="top">Name:&nbsp;&nbsp;
																	</TD>
																	<TD class="StandardTableAlternate" vAlign="top" colSpan="2">
																		<asp:label id="lblHaendlerName" runat="server"></asp:label></TD>
																</TR>
																<TR>
																	<TD class="TextLarge" vAlign="top">Adresse:</TD>
																	<TD class="TextLarge" vAlign="top" colSpan="2">
																		<asp:label id="lblAdresse" runat="server"></asp:label></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD width="100%">
															<asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" CellPadding="1">
																<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
																<ItemStyle CssClass="TextLarge"></ItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="Kreditkontrollbereich" HeaderText="Kreditkontrollbereich"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Altes Kontingent">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		<ItemTemplate>
																			<asp:Label id=lblKontingent_Alt runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
																			</asp:Label>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox5 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
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
																			<asp:Label id=lblFrei runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																			</asp:Label>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="Gesperrt - Alt">
																		<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=Gesperrt_Alt runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="Gesperrt - Neu ">
																		<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=chkGesperrt_Neu runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Neu") %>'>
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR id="FocusScript" runat="server">
					<TD colSpan="3"></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
