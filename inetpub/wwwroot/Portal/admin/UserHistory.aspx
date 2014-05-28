<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserHistory.aspx.vb" Inherits="CKG.Admin.UserHistory" %>
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
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2">Benutzerhistorie</td>
								</TR>
								<tr>
									<TD vAlign="top" width="120">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
											<TR>
												<TD class="TaskTitle">&nbsp;</TD>
											</TR>
											<TR>
												<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
											</TR>
										</TABLE>
									</TD>
									<td vAlign="top">
										<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
											<TBODY>
												<tr>
													<TD class="TaskTitle">&nbsp;</TD>
												</tr>
												<tr id="trSearch" runat="server">
													<td align="left" height="182">
														<table bgColor="white" border="0">
															<TR>
																<TD vAlign="top" width="100">Firma:</TD>
																<TD vAlign="top" width="160"><asp:label id="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterCustomer" runat="server" Visible="False" Width="160px" AutoPostBack="True" Height="20px"></asp:dropdownlist></TD>
																<TD vAlign="top">&nbsp;&nbsp;</TD>
																<TD vAlign="top">Benutzername:</TD>
																<TD vAlign="top"><asp:textbox id="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:textbox></TD>
																<TD vAlign="top"></TD>
															</TR>
															<TR id="trSelectOrganization" runat="server">
																<TD vAlign="top" width="100">Organisation:
																</TD>
																<TD vAlign="top" width="160"><asp:label id="lblOrganization" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterOrganization" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
																<TD vAlign="top">&nbsp;&nbsp;</TD>
																<TD vAlign="top" rowSpan="2">Testzugang</TD>
																<TD vAlign="top" rowSpan="2"><asp:radiobutton id="rbTestUserAll" runat="server" Checked="True" Text="Alle" GroupName="grpTestUser"></asp:radiobutton><BR>
																	<asp:radiobutton id="rbTestUserProd" runat="server" Text="Nur produktiv" GroupName="grpTestUser"></asp:radiobutton><BR>
																	<asp:radiobutton id="rbTestUserTest" runat="server" Text="Nur Test" GroupName="grpTestUser"></asp:radiobutton></TD>
																<TD vAlign="top"></TD>
															</TR>
															<TR>
																<TD vAlign="top" width="100">Gruppe:</TD>
																<TD vAlign="top" width="160"><asp:label id="lblGroup" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterGroup" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
																<TD vAlign="top">&nbsp;&nbsp;</TD>
																<TD vAlign="bottom"><asp:linkbutton id="btnSuche" runat="server" CssClass="StandardButton">Suchen</asp:linkbutton></TD>
															</TR>
														</table>
														&nbsp;
													</td>
												</tr>
												<TR id="trSearchSpacer" runat="server">
													<TD align="left" height="25"><asp:hyperlink id="lnkExcel" runat="server" Visible="False">
															<strong>Excel-Download: rechte Maustaste -> Speichern unter...</strong></asp:hyperlink></TD>
												</TR>
												<TR id="trSearchResult" runat="server">
													<TD align="left"><asp:datagrid id="dgSearchResult" runat="server" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True">
															<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
															<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
															<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
															<Columns>
																<asp:BoundColumn Visible="False" DataField="UserHistoryID" SortExpression="UserHistoryID" HeaderText="UserHistoryID"></asp:BoundColumn>
																<asp:ButtonColumn DataTextField="UserName" SortExpression="UserName" HeaderText="Benutzername" CommandName="Edit"></asp:ButtonColumn>
																<asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Kunden-&lt;br&gt;referenz"></asp:BoundColumn>
																<asp:BoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe"></asp:BoundColumn>
																<asp:BoundColumn DataField="OrganizationName" SortExpression="OrganizationName" HeaderText="Organisation"></asp:BoundColumn>
																<asp:TemplateColumn SortExpression="CustomerAdmin" HeaderText="Firmen-&lt;br&gt;Administrator">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate>
																		<asp:CheckBox id=cbxSRCustomerAdmin runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
																		</asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn SortExpression="TestUser" HeaderText="Testzugang">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate>
																		<asp:CheckBox id=cbxSRTestUser runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>' Enabled="False">
																		</asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn DataField="LastPwdChange" SortExpression="LastPwdChange" HeaderText="letzte&lt;br&gt;Passwort&#228;nderung" DataFormatString="{0:dd.MM.yy}">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																</asp:BoundColumn>
																<asp:TemplateColumn SortExpression="PwdNeverExpires" HeaderText="Passwort&lt;br&gt;l&#228;uft nicht ab">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate>
																		<asp:CheckBox id=cbxSRPwdNeverExpires runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "PwdNeverExpires") %>' Enabled="False">
																		</asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn DataField="FailedLogins" SortExpression="FailedLogins" HeaderText="Anmelde-&lt;br&gt;Fehlversuche">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																</asp:BoundColumn>
																<asp:TemplateColumn SortExpression="AccountIsLockedOut" HeaderText="Konto&lt;br&gt;gesperrt">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate>
																		<asp:CheckBox id=cbxSRAccountIsLockedOut runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "AccountIsLockedOut") %>'>
																		</asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn SortExpression="Deleted" HeaderText="Konto&lt;br&gt;gel&#246;scht">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate>
																		<asp:CheckBox id=cbxAccountDeleted runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "Deleted") %>'>
																		</asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
															</Columns>
															<PagerStyle Mode="NumericPages"></PagerStyle>
														</asp:datagrid></TD>
												</TR>
												<TR id="trEditUser" runat="server">
													<td align="left">
														<table width="740" border="0">
															<TR>
																<TD vAlign="top" align="left">
																	<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="345" bgColor="white" border="0">
																		<TR>
																			<TD height="22">Benutzername:</TD>
																			<TD height="22"><asp:textbox id="txtUserHistoryID" runat="server" Visible="False" Width="10px" Height="10px" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE">-1</asp:textbox><asp:label id="txtUserName" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">Kundenreferenz:</TD>
																			<TD height="22"><asp:label id="txtReference" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR id="trTestUser" runat="server">
																			<TD height="22">Test-Zugang:</TD>
																			<TD height="22"><asp:checkbox id="cbxTestUser" runat="server" Enabled="False"></asp:checkbox></TD>
																		</TR>
																		<TR id="trCustomer" runat="server">
																			<TD height="22">Firma:</TD>
																			<TD height="22"><asp:label id="ddlCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR id="trCustomerAdmin" runat="server">
																			<TD height="22">Firmenadministrator:</TD>
																			<TD height="22"><asp:checkbox id="cbxCustomerAdmin" runat="server" Enabled="False"></asp:checkbox></TD>
																		</TR>
																		<TR id="trGroup" runat="server">
																			<TD height="22">Gruppe:</TD>
																			<TD height="22"><asp:label id="ddlGroups" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR id="trOrganization" runat="server">
																			<TD height="22">Organisation:</TD>
																			<TD height="22"><asp:label id="ddlOrganizations" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR id="trOrganizationAdministrator" runat="server">
																			<TD height="22">Organisationadministrator:</TD>
																			<TD height="22"><asp:checkbox id="cbxOrganizationAdmin" runat="server" Enabled="False"></asp:checkbox></TD>
																		</TR>
																	</TABLE>
																</TD>
																<TD width="100%"></TD>
																<TD vAlign="top" align="right">
																	<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" bgColor="white" border="0">
																		<TR>
																			<TD height="22">letzte Passwortänderung:</TD>
																			<TD align="left" height="22"><asp:label id="lblLastPwdChange" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR id="trPwdNeverExpires" runat="server">
																			<TD height="22">Passwort läuft nie ab:</TD>
																			<TD align="left" height="22"><asp:checkbox id="cbxPwdNeverExpires" runat="server" Enabled="False"></asp:checkbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">fehlgeschlagene Anmeldungen:</TD>
																			<TD align="left" height="22"><asp:label id="lblFailedLogins" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">Konto gesperrt:</TD>
																			<TD align="left" height="22"><asp:checkbox id="cbxAccountIsLockedOut" runat="server" Enabled="False"></asp:checkbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Passwort:&nbsp;</TD>
																			<TD noWrap align="left" height="22">&nbsp;
																				<asp:label id="txtPassword" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																			</TD>
																			<TD noWrap align="left" height="22">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																			</TD>
																		</TR>
																		<TR id="trMail" runat="server">
																			<TD height="22">Angelegt:</TD>
																			<TD height="22"><asp:label id="txtCreated" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">Letzte Änderung:</TD>
																			<TD height="22"><asp:label id="txtLastChange" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">Änderungsdatum:</TD>
																			<TD height="22"><asp:label id="txtLastChanged" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">Änderer:</TD>
																			<TD height="22"><asp:label id="txtLastChangedBy" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">Gelöscht:</TD>
																			<TD height="22"><asp:checkbox id="cbxDeleted" runat="server" Enabled="False"></asp:checkbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Löschdatum:</TD>
																			<TD height="22"><asp:label id="txtDeleteDate" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</table>
														&nbsp;
													</td>
												</TR>
												<TR>
													<TD align="left" height="25"></TD>
												</TR>
											</TBODY></table>
									</td>
								</tr>
								<tr>
									<td></td>
									<td><asp:label id="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
								</tr>
								<tr>
									<td></td>
									<td><!--#include File="../PageElements/Footer.html" --></td>
								</tr>
							</TABLE>
						</td>
					</tr>
				</TBODY></table>
		</form>
	</body>
</HTML>
