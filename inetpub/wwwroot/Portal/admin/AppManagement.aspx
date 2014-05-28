<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppManagement.aspx.vb" Inherits="CKG.Admin.AppManagement" %>
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
										<td class="PageNavigation" height="25" colSpan="2">Administration 
											(Anwendungsverwaltung)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120" height="25">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TaskTitle">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="middle" width="150"><asp:linkbutton id="lbtnNew" runat="server" CssClass="StandardButton">&#149;&nbsp;Neue Anwendung anlegen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="middle" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Speichern</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="middle" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="middle" width="150"><asp:linkbutton id="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Löschen</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
												<TBODY>
													<tr>
														<TD class="TaskTitle">
															<asp:hyperlink id="lnkUserManagement" runat="server" NavigateUrl="UserManagement.aspx" CssClass="TaskTitle">Benutzerverwaltung</asp:hyperlink>
															<asp:hyperlink id="lnkGroupManagement" runat="server" NavigateUrl="GroupManagement.aspx" CssClass="TaskTitle">Gruppenverwaltung</asp:hyperlink>
															<asp:hyperlink id="lnkOrganizationManagement" runat="server" CssClass="TaskTitle" NavigateUrl="OrganizationManagement.aspx">Organisationsverwaltung</asp:hyperlink>
															<asp:hyperlink id="lnkCustomerManagement" runat="server" NavigateUrl="CustomerManagement.aspx" CssClass="TaskTitle">Kundenverwaltung</asp:hyperlink>
															<asp:hyperlink id="lnkArchivManagement" runat="server" CssClass="TaskTitle" NavigateUrl="ArchivManagement.aspx">Archivverwaltung</asp:hyperlink>&nbsp;</TD>
													</tr>
													<tr id="trSearch" runat="server">
														<td align="left">
															<table border="0" bgColor="white" height="30">
																<tr>
																	<td vAlign="bottom" width="127">Anwendung:</td>
																	<td vAlign="bottom"><asp:textbox id="txtFilterAppName" runat="server" Height="20px" Width="160px">*</asp:textbox></td>
																	<td vAlign="bottom"></td>
																</tr>
																<TR>
																	<TD vAlign="bottom" width="127">Freundlicher Name:</TD>
																	<TD vAlign="bottom">
																		<asp:textbox id="txtFilterAppFriendlyName" runat="server" Width="160px" Height="20px">*</asp:textbox></TD>
																	<TD vAlign="bottom"><asp:button id="btnSuche" runat="server" CssClass="StandardButton" Text="Suchen"></asp:button></TD>
																</TR>
															</table>
														</td>
													</tr>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left"><asp:datagrid id="dgSearchResult" runat="server" Width="100%" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" BackColor="White">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="True" DataField="AppID" SortExpression="AppID" HeaderText="AppID"></asp:BoundColumn>
																	<asp:ButtonColumn DataTextField="AppName" SortExpression="AppName" HeaderText="Anwendung" CommandName="Edit"></asp:ButtonColumn>
																	<asp:BoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Freundlicher Name"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="AppInMenu" HeaderText="im Men&#252;">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.AppInMenu") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="AppType" SortExpression="AppType" HeaderText="Typ"></asp:BoundColumn>
																	<asp:BoundColumn DataField="AppParentName" SortExpression="AppParentName" HeaderText="geh&#246;rt zu"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="BatchAuthorization" HeaderText="Sammel-&lt;br&gt;autorisierung">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox2 runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
																			</asp:CheckBox>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
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
																		<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD height="22">Anwendungs-Name:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtAppID" runat="server" Visible="False" Height="10px" Width="10px" BorderStyle="None" BorderWidth="0px" BackColor="#CEDBDE" ForeColor="#CEDBDE">-1</asp:textbox><asp:textbox id="txtAppName" runat="server" Height="20px" Width="160px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Freundlicher Name:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtAppFriendlyName" runat="server" Height="20px" Width="160px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Typ:</TD>
																				<TD align="right" height="22"><asp:dropdownlist id="ddlAppType" runat="server" Height="20px" Width="160px"></asp:dropdownlist></TD>
																			</TR>
																			<TR>
																				<TD height="22">URL:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtAppURL" runat="server" Height="20px" Width="160px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Parameter:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtAppParam" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD colSpan="2" height="22">
																					<asp:linkbutton id="lnkColumnTranslation" runat="server" CssClass="StandardButton">Spaltenübersetzungen</asp:linkbutton></TD>
																			</TR>
																			<TR>
																				<TD colSpan="2" height="22">
																					<asp:linkbutton id="lnkFieldTranslation" runat="server" CssClass="StandardButton">Feldübersetzungen</asp:linkbutton></TD>
																			</TR>
																			<TR>
																				<TD colSpan="2" height="22">
																					<asp:linkbutton id="lnkZugeordneteBAPIs" runat="server" CssClass="StandardButton">Zugeordnete BAPIs</asp:linkbutton></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD vAlign="top" width="185" height="22">in Menü:</TD>
																				<TD align="left" height="22"><asp:checkbox id="cbxAppInMenu" runat="server"></asp:checkbox></TD>
																			</TR>
																			<TR>
																				<TD vAlign="top" height="22">Kommentar:</TD>
																				<TD align="left" height="22"><asp:textbox id="txtAppComment" runat="server" Height="40px" Width="160px" TextMode="MultiLine"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22"><U>Gehört zu:</U></TD>
																				<TD align="right" height="22"></TD>
																			</TR>
																			<TR>
																				<TD colSpan="2" height="22"><asp:dropdownlist id="ddlAppParent" runat="server" Height="20px"></asp:dropdownlist></TD>
																			</TR>
																			<TR>
																				<TD height="22">Reihenfolge im Menü:</TD>
																				<TD align="left" height="22" vAlign="top">
																					<asp:textbox id="txtAppRank" runat="server" Height="20px" Width="160px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Autorisierungslevel:</TD>
																				<TD align="left" height="22">
																					<asp:dropdownlist id="ddlAuthorizationlevel" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
																			</TR>
																			<TR>
																				<TD height="22">Sammelautorisierung:</TD>
																				<TD align="left" height="22">
																					<asp:CheckBox id="cbxBatchAuthorization" runat="server"></asp:CheckBox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Schwellwert(in Sekunden)</TD>
																				<TD align="left" height="22">
																					<asp:TextBox ID="txtSchwellwert" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                                                                                </TD>
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
