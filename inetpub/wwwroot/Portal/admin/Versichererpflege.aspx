<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Versichererpflege.aspx.vb" Inherits="CKG.Admin.Versichererpflege" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<TBODY>
					<tr>
						<td height="18"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<td class="PageNavigation" colSpan="2" height="25">&nbsp;Administration 
											(Versichererpflege)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="150" height="25">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TaskTitle">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" noWrap width="150"><asp:linkbutton id="lbtnNew" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neuer Versicherer</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Löschen</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
												<TBODY>
													<tr>
														<TD class="TaskTitle"><asp:hyperlink id="lnkHalterpflege" runat="server" CssClass="TaskTitle" NavigateUrl="Halterpflege.aspx">Halterpflege</asp:hyperlink></TD>
													</tr>
													<tr id="trSearch" runat="server">
														<td align="left">
															<table bgColor="white" border="0">
																<tr>
																	<td vAlign="bottom" width="100"></td>
																	<td vAlign="bottom"><asp:textbox id="txtFilterSAPNr" runat="server" MaxLength="10" Height="20px" Width="160px" Enabled="False" Visible="False">*</asp:textbox></td>
																	<td vAlign="bottom"></td>
																</tr>
																<TR>
																	<TD vAlign="bottom" width="100"></TD>
																	<TD vAlign="bottom"><asp:textbox id="txtFilterName1" runat="server" Height="20px" Width="160px" Enabled="False" Visible="False">*</asp:textbox></TD>
																	<TD vAlign="bottom"></TD>
																</TR>
																<TR>
																	<TD vAlign="bottom" width="100">Kunde:</TD>
																	<TD vAlign="bottom"><asp:dropdownlist id="ddlKundennr" runat="server"></asp:dropdownlist></TD>
																	<TD vAlign="bottom"><asp:button id="btnSuche" runat="server" CssClass="StandardButton" Text="Suchen"></asp:button></TD>
																</TR>
															</table>
														</td>
													</tr>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left"><asp:datagrid id="dgSearchResult" runat="server" BackColor="White" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="VersichererID" SortExpression="VersichererID" HeaderText="VersichererID"></asp:BoundColumn>
																	<asp:ButtonColumn DataTextField="SAP-Nr" SortExpression="SAP-Nr" HeaderText="SAP-Nr" CommandName="Edit">
																		<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
																	</asp:ButtonColumn>
																	<asp:BoundColumn DataField="CustomerID" SortExpression="CustomerID" HeaderText="Kundennummer">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Font-Bold="True" Wrap="False" HorizontalAlign="Left"></ItemStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="Name1" SortExpression="Name1" HeaderText="Name1">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="l&#246;schen">
																		<HeaderStyle Width="30px"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:ImageButton id="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="../Images/icon_nein_s.gif"></asp:ImageButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Mode="NumericPages"></PagerStyle>
															</asp:datagrid></td>
													</tr>
													<tr id="trEditUser" runat="server">
														<td align="left">
															<table width="740" border="0">
																<TR>
																	<TD vAlign="top" align="left">
																		<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="345" bgColor="white" border="0">
																			<TR>
																				<TD height="22">SAP-Nr:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtVersichererID" runat="server" Visible="False" Height="10px" Width="10px" BackColor="#CEDBDE" BorderStyle="None" BorderWidth="0px" ForeColor="#CEDBDE">-1</asp:textbox><asp:textbox id="txtSAPNr" runat="server" MaxLength="10" Height="20px" Width="160px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Name1:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtName1" runat="server" Height="20px" Width="160px"></asp:textbox></TD>
																			</TR>
																			<TR id="trKunde" runat="server">
																				<TD height="22">Kunde:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtKunde" runat="server" Height="20px" Width="160px" Enabled="False" BorderWidth="1px" BackColor="Yellow"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" border="0">
																			<TR>
																				<TD height="22"></TD>
																				<TD align="right" height="22"></TD>
																			</TR>
																			<TR>
																				<TD height="22"></TD>
																				<TD align="right" height="22"></TD>
																			</TR>
																			<TR>
																				<TD height="22"></TD>
																				<TD align="right" height="22"></TD>
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
