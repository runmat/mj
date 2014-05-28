<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AutorisierungenLoeschen.aspx.vb" Inherits="CKG.Admin.AutorisierungenLoeschen" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<TBODY>
					<tr>
						<td height="18"><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<td class="PageNavigation" height="25" colSpan="2">Administration (Autorisierungen 
											löschen)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TaskTitle">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False" CausesValidation="False">&#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False" CausesValidation="False">&#149;&nbsp;Löschen</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
												<TBODY>
													<tr>
														<TD class="TaskTitle">&nbsp;</TD>
													</tr>
													<TR id="trSearchSpacerTop" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25">
															<asp:datagrid id="dgSearchResult" runat="server" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" Width="100%" BackColor="White">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="AuthorizationID" SortExpression="AuthorizationID" HeaderText="AuthorizationID"></asp:BoundColumn>
																	<asp:ButtonColumn DataTextField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Anwendung" CommandName="Edit">
																		<HeaderStyle Width="100px"></HeaderStyle>
																	</asp:ButtonColumn>
																	<asp:BoundColumn DataField="InitializedBy" SortExpression="InitializedBy" HeaderText="Angelegt von"></asp:BoundColumn>
																	<asp:BoundColumn DataField="InitializedWhen" SortExpression="InitializedWhen" HeaderText="Angelegt am"></asp:BoundColumn>
																	<asp:BoundColumn DataField="OrganizationName" SortExpression="OrganizationName" HeaderText="Organisation"></asp:BoundColumn>
																	<asp:BoundColumn DataField="CustomerReference" SortExpression="CustomerReference" HeaderText="Kunden-&lt;br&gt;Referenz"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ProcessReference" SortExpression="ProcessReference" HeaderText="Prozess-&lt;br&gt;Referenz"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="TestUser" HeaderText="TestUser">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TestUser") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="l&#246;schen">
																		<HeaderStyle Width="30px"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:ImageButton id="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="../Images/icon_nein_s.gif"></asp:ImageButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left"></td>
													</tr>
													<tr id="trEditUser" runat="server">
														<td align="left">
															<table width="740" border="0" id="Table3">
																<TR>
																	<TD vAlign="top" align="left">
																		<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD height="22">
																					Anwendung:</TD>
																				<TD align="right" height="22">
																					<P><asp:textbox id="txtAuthorizationID" runat="server" Visible="False" Height="0px" Width="0px" BorderStyle="None" BorderWidth="0px" BackColor="#CEDBDE" ForeColor="#CEDBDE">-1</asp:textbox><asp:textbox id="txtAppFriendlyName" runat="server" Height="20px" Width="160px" ReadOnly="True"></asp:textbox></P>
																				</TD>
																			</TR>
																			<TR>
																				<TD height="22">
																					Angelegt von:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtInitializedBy" runat="server" Height="20px" Width="160px" ReadOnly="True"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">
																					Angelegt am:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtInitializedWhen" runat="server" Width="160px" Height="20px" ReadOnly="True"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD height="22" width="104">
																					Organisation:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtOrganizationName" runat="server" Width="160px" Height="20px" ReadOnly="True"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">
																					Kundenreferenz:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtCustomerReference" runat="server" Width="160px" Height="20px" ReadOnly="True"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">Prozessreferenz:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtProcessReference" runat="server" Width="160px" Height="20px" ReadOnly="True"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
															</table>
														</td>
													</tr>
													<TR>
														<TD align="left" height="25"></TD>
													</TR>
												</TBODY></table>
										</td>
									</tr>
									<tr>
										<td></td>
										<td><asp:label id="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
									</tr>
									<tr>
										<td></td>
										<td><!--#include File="../PageElements/Footer.html" --></td>
									</tr>
								</TBODY></TABLE>
						</td>
					</tr>
				</TBODY></table>
		</form>
	</body>
</HTML>
