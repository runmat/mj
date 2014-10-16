<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserUnlock.aspx.vb" Inherits="CKG.Admin.UserUnlock" %>
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
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" width="150" height="25">&nbsp;</td>
									<td class="PageNavigation" align="left" height="25">&nbsp;&nbsp;&nbsp; &nbsp;</td>
								</TR>
								<tr>
									<TD vAlign="top" width="150">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="1" cellPadding="0" width="150" border="1">
											<TR>
												<TD class="TextHeader" width="150">Administration</TD>
											</TR>
											<TR>
												<TD vAlign="middle" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Visible="False">Speichern</asp:linkbutton></TD>
											</TR>
											<TR>
												<TD vAlign="middle" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False">Verwerfen</asp:linkbutton></TD>
											</TR>
											</TABLE>
									</TD>
									<td vAlign="top">
										<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
											<TBODY>
												<tr>
													<TD class="TaskTitle">
                                                        <asp:Label ID="lblBenutzerverwaltung" runat="server"></asp:Label>
                                                    </TD>
												</tr>
												<tr id="trSearch" runat="server">
													<td align="left">
														<table bgColor="white" border="0">
															<TR>
																<TD vAlign="bottom" width="100" height="38">Firma:</TD>
																<TD vAlign="bottom" width="160" height="38"><asp:label id="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterCustomer" runat="server" Visible="False" Width="160px" AutoPostBack="True" Height="20px"></asp:dropdownlist></TD>
																<TD vAlign="bottom" height="38"></TD>
															</TR>
															<TR>
																<TD vAlign="bottom" width="100" height="3">Organisation:</TD>
																<TD vAlign="bottom" width="160" height="3"><asp:label id="lblOrganization" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterOrganization" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
																<TD vAlign="bottom" height="3"></TD>
															</TR>
															<TR>
																<TD vAlign="bottom" width="100">Gruppe:</TD>
																<TD vAlign="bottom" width="160"><asp:label id="lblGroup" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterGroup" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
																<TD vAlign="bottom">
																	<asp:LinkButton id="btnSuche" runat="server" CssClass="StandardButton">Suche</asp:LinkButton></TD>
															</TR>
															<tr>
																<td vAlign="bottom" width="100">Benutzername:</td>
																<td vAlign="bottom" width="160"><asp:textbox id="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:textbox></td>
																<td vAlign="bottom"></td>
															</tr>
														</table>
													</td>
												</tr>
												<TR id="trSearchSpacer" runat="server">
													<TD align="left" height="25"></TD>
												</TR>
												<TR id="trSearchResult" runat="server">
													<TD align="left"><asp:datagrid id="dgSearchResult" runat="server" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True">
															<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
															<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
															<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
															<Columns>
																<asp:BoundColumn Visible="False" DataField="UserID" SortExpression="UserID" HeaderText="UserID"></asp:BoundColumn>
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
																<asp:BoundColumn DataField="LastPwdChange" SortExpression="LastPwdChange" HeaderText="letzte&lt;br&gt;Kennwort&#228;nderung" DataFormatString="{0:dd.MM.yy}">
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																</asp:BoundColumn>
																<asp:TemplateColumn SortExpression="PwdNeverExpires" HeaderText="Kennwort&lt;br&gt;l&#228;uft nicht ab">
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
																		<asp:CheckBox id=cbxSRAccountIsLockedOut runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "AccountIsLockedOut") %>' Enabled="False">
																		</asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn SortExpression="LoggedOn" HeaderText="Angemeldet">
																	<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate>
																		<asp:CheckBox id=chkSRLoggedOn runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.LoggedOn") %>' Enabled="False">
																		</asp:CheckBox>
																	</ItemTemplate>
																	<EditItemTemplate>
																		<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LoggedOn") %>'>
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
																			<TD height="22"><asp:textbox id="txtUserName" runat="server" Width="160px" Height="20px"></asp:textbox><asp:textbox id="txtUserID" runat="server" Visible="False" Width="10px" Height="10px" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE">-1</asp:textbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Anrede:</TD>
																			<TD height="22">
																				<asp:dropdownlist id="ddlTitle" runat="server" Width="160px" Height="20px" AutoPostBack="True">
																					<asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
																					<asp:ListItem Value="Herr">Herr</asp:ListItem>
																					<asp:ListItem Value="Frau">Frau</asp:ListItem>
																				</asp:dropdownlist></TD>
																		</TR>
																		<TR>
																			<TD height="22">Vorname:</TD>
																			<TD height="22">
																				<asp:textbox id="txtFirstName" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Nachname:</TD>
																			<TD height="22">
																				<asp:textbox id="txtLastName" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Kundenreferenz:</TD>
																			<TD height="22"><asp:textbox id="txtReference" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Filiale:</TD>
																			<TD height="22">
																				<asp:textbox id="txtStore" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																		</TR>
																		<TR id="trTestUser" runat="server">
																			<TD height="22">Test-Zugang:</TD>
																			<TD height="22"><asp:checkbox id="cbxTestUser" runat="server"></asp:checkbox></TD>
																		</TR>
																		<TR id="trCustomer" runat="server">
																			<TD height="22">Firma:</TD>
																			<TD height="22"><asp:dropdownlist id="ddlCustomer" runat="server" Width="160px" AutoPostBack="True" Height="20px"></asp:dropdownlist></TD>
																		</TR>
																		<TR>
																			<TD height="22">Gülig ab:</TD>
																			<TD height="22">
																				<asp:textbox id="txtValidFrom" runat="server" Width="160px" Height="20px" 
                                                                                    MaxLength="10"></asp:textbox></TD>
																		</TR>
																		<TR id="trCustomerAdmin1" runat="server">
																			<TD height="22">Firmenadministrator:</TD>
																			<TD height="22"><asp:checkbox id="cbxCustomerAdmin" runat="server"></asp:checkbox></TD>
																		</TR>
																		<TR id="trCustomerAdmin2" runat="server">
																			<TD height="22">First-Level-Admin:</TD>
																			<TD height="22"><asp:checkbox id="cbxFirstLevelAdmin" runat="server"></asp:checkbox></TD>
																		</TR>
																		<TR id="trGroup" runat="server">
																			<TD height="22">Gruppe:</TD>
																			<TD height="22"><asp:dropdownlist id="ddlGroups" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
																		</TR>
																		<TR id="trOrganization" runat="server">
																			<TD height="22">Organisation:</TD>
																			<TD height="22"><asp:dropdownlist id="ddlOrganizations" runat="server" Width="160px" Height="20px"></asp:dropdownlist></TD>
																		</TR>
																		<TR id="trOrganizationAdministrator" runat="server">
																			<TD height="22">Organisationadministrator:</TD>
																			<TD height="22"><asp:checkbox id="cbxOrganizationAdmin" runat="server"></asp:checkbox></TD>
																		</TR>
                                                                        <tr id="trMail" runat="server">
                                                                    <td height="22">
                                                                        E-Mail (x@y.z):
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtMail" runat="server" Width="160px" MaxLength="75"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPhone" runat="server">
                                                                    <td height="22">
                                                                       Telefon:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtPhone" runat="server" Width="160px" MaxLength="75" ></asp:TextBox>
                                                                    </td>
                                                                </tr> 

																	</TABLE>
																</TD>
																<TD width="100%"></TD>
																<TD vAlign="top" align="right">
																	<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" bgColor="white" border="0">
																		<TR>
																			<TD height="22">letzte Kennwortänderung:</TD>
																			<TD align="right" height="22"><asp:label id="lblLastPwdChange" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR id="trPwdNeverExpires" runat="server">
																			<TD height="22">Kennwort läuft nie ab:</TD>
																			<TD align="left" height="22"><asp:checkbox id="cbxPwdNeverExpires" runat="server"></asp:checkbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">fehlgeschlagene Anmeldungen:</TD>
																			<TD align="right" height="22"><asp:label id="lblFailedLogins" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label></TD>
																		</TR>
																		<TR>
																			<TD height="22">Konto gesperrt:</TD>
																			<TD align="left" height="22"><asp:checkbox id="cbxAccountIsLockedOut" runat="server"></asp:checkbox>
																				<asp:CheckBox id="cbxApproved" runat="server" Visible="False"></asp:CheckBox><asp:Label ID="lblLockedBy" runat="server" visible="false"></asp:Label>
                                                                            </TD>
																		</TR>
																		<TR>
																			<TD height="22">Angemeldet:</TD>
																			<TD align="left" height="22"><asp:checkbox id="chkLoggedOn" runat="server"></asp:checkbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Matrix gefüllt:</TD>
																			<TD align="left" height="22">
																				<asp:checkbox id="chk_Matrix1" runat="server"></asp:checkbox></TD>
																		</TR>
																		<TR id="trReadMessageCount" runat="server">
																			<TD height="22">Anzahl der<BR>
																				Startmeldungs-Anzeigen:</TD>
																			<TD vAlign="top" align="right" height="22"><asp:textbox id="txtReadMessageCount" runat="server" Width="40px" MaxLength="2"></asp:textbox></TD>
																		</TR>
																		<TR id="trNewPassword" runat="server">
																			<TD height="22">Neues Passwort setzen:</TD>
																			<TD noWrap align="left" height="22"><asp:checkbox id="chkNewPasswort" runat="server"></asp:checkbox></TD>
																		</TR>
																		<TR id="trPassword" runat="server">
																			<TD height="22">Passwort:</TD>
																			<TD noWrap align="left" height="22"><asp:textbox id="txtPassword" runat="server" Visible="true" Width="160px" TextMode="Password"></asp:textbox><asp:linkbutton id="btnCreatePassword" runat="server" CssClass="StandardButtonTable" Visible="False">Kennwort generieren</asp:linkbutton></TD>
																		</TR>
																		<TR id="trConfirmPassword" runat="server">
																			<TD height="22">Passwort bestätigen:</TD>
																			<TD align="left" height="22"><asp:textbox id="txtConfirmPassword" runat="server" Visible="true" Width="160px" TextMode="Password"></asp:textbox></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</table>
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
