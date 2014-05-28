<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ArchivManagement.aspx.vb" Inherits="CKG.Admin.ArchivManagement" %>
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
										<td class="PageNavigation" colSpan="2" height="25">Administration 
											(Archivsverwaltung)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120" height="25">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TaskTitle">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" runat="server" CssClass="StandardButton">&#149;&nbsp;Neues Archiv anlegen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Speichern</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Löschen</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
												<TBODY>
													<tr>
														<TD class="TaskTitle"><asp:hyperlink id="lnkUserManagement" runat="server" CssClass="TaskTitle" NavigateUrl="UserManagement.aspx">Benutzerverwaltung</asp:hyperlink><asp:hyperlink id="lnkGroupManagement" runat="server" CssClass="TaskTitle" NavigateUrl="GroupManagement.aspx">Gruppenverwaltung</asp:hyperlink><asp:hyperlink id="lnkOrganizationManagement" runat="server" CssClass="TaskTitle" NavigateUrl="OrganizationManagement.aspx">Organisationsverwaltung</asp:hyperlink><asp:hyperlink id="lnkCustomerManagement" runat="server" CssClass="TaskTitle" NavigateUrl="CustomerManagement.aspx">Kundenverwaltung</asp:hyperlink>
															<asp:hyperlink id="lnkAppManagement" runat="server" CssClass="TaskTitle" 
                                                                NavigateUrl="AppManagement.aspx" Visible="False">Anwendungsverwaltung</asp:hyperlink>&nbsp;</TD>
													</tr>
													<tr id="trSearch" runat="server">
														<td align="left">
															<table bgColor="white" border="0">
																<tr>
																	<td vAlign="bottom" width="100">Archiv:</td>
																	<td vAlign="bottom"><asp:textbox id="txtFilterEasyArchivName" runat="server" Width="160px" Height="20px">*</asp:textbox></td>
																	<td vAlign="bottom"><asp:button id="btnSuche" runat="server" CssClass="StandardButton" Text="Suchen"></asp:button></td>
																</tr>
															</table>
														</td>
													</tr>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left"><asp:datagrid id="dgSearchResult" runat="server" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="ArchivID" SortExpression="ArchivID" HeaderText="ArchivID"></asp:BoundColumn>
																	<asp:ButtonColumn DataTextField="EasyArchivName" SortExpression="EasyArchivName" HeaderText="Archiv" CommandName="Edit"></asp:ButtonColumn>
																	<asp:BoundColumn DataField="EasyLagerortName" SortExpression="EasyLagerortName" HeaderText="Lagerort-Name"></asp:BoundColumn>
																	<asp:BoundColumn DataField="EasyQueryIndexName" SortExpression="EasyQueryIndexName" HeaderText="QueryIndex-Name"></asp:BoundColumn>
																	<asp:BoundColumn DataField="EasyTitleName" SortExpression="EasyTitleName" HeaderText="Titel"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Archivetype" SortExpression="Archivetype" HeaderText="Archivtyp"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="l&#246;schen">
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
																				<TD height="22">Archiv-Name:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtArchivID" runat="server" Visible="False" Width="10px" Height="10px" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE">-1</asp:textbox><asp:textbox id="txtEasyArchivName" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Lagerort-Name:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtEasyLagerortName" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">QueryIndex:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtEasyQueryIndex" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">QueryIndex-Name:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtEasyQueryIndexName" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" bgColor="white" border="0">
																			<TR>
																				<TD vAlign="top" width="185" height="22">Titel:</TD>
																				<TD align="left" height="22">
																					<asp:textbox id="txtEasyTitleName" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD vAlign="top" height="22">DefaultQuery:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtDefaultQuery" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Archivetype:</TD>
																				<TD vAlign="top" align="right" height="22"><asp:textbox id="txtArchivetype" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">SortOrder:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtSortOrder" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
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
