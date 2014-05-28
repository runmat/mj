<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ColumnTranslation.aspx.vb" Inherits="CKG.Admin.ColumnTranslation" %>
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
										<td class="PageNavigation" width="120" height="25">Administration</td>
										<td class="PageNavigation" align="left" height="25">Spaltenübersetzungen für
											<asp:label id="lblAppName" runat="server"></asp:label>&nbsp;-
											<asp:Label id="lblAppFriendlyName" runat="server"></asp:Label></td>
									</TR>
									<tr>
										<TD vAlign="top" width="120">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TextHeader" width="120"></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" runat="server" 
                                                            CssClass="StandardButton" Enabled="false" Visible="False">&#149;&nbsp;Neue Spaltenübersetzung anlegen</asp:linkbutton></TD>
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
														<TD class="TaskTitle"><asp:hyperlink id="lnkUserManagement" runat="server" NavigateUrl="UserManagement.aspx" CssClass="TaskTitle">Benutzerverwaltung</asp:hyperlink><asp:hyperlink id="lnkGroupManagement" runat="server" NavigateUrl="GroupManagement.aspx" CssClass="TaskTitle">Gruppenverwaltung</asp:hyperlink><asp:hyperlink id="lnkOrganizationManagement" runat="server" NavigateUrl="OrganizationManagement.aspx" CssClass="TaskTitle">Organisationsverwaltung</asp:hyperlink><asp:hyperlink id="lnkCustomerManagement" runat="server" NavigateUrl="CustomerManagement.aspx" CssClass="TaskTitle">Kundenverwaltung</asp:hyperlink><asp:hyperlink  Visible="false" id="lnkAppManagment" runat="server" NavigateUrl="AppManagement.aspx" CssClass="TaskTitle">Anwendungsverwaltung</asp:hyperlink></TD>
													</tr>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left"><asp:datagrid id="dgSearchResult" runat="server" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="100%" BackColor="White">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="AppID" SortExpression="AppID" HeaderText="AppID"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="OrgName" HeaderText="SAP-Name" >
																		<ItemTemplate>
																			<asp:LinkButton  Enabled="false"  id="lbEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrgName") %>' CommandName="Edit" CausesValidation="False" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.OrgName") %>'>
																			</asp:LinkButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="NewName" SortExpression="NewName" HeaderText="&#220;bersetzung"></asp:BoundColumn>
																	<asp:BoundColumn DataField="DisplayOrder" SortExpression="DisplayOrder" HeaderText="Reihenfolge Nr"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="NULLENENTFERNEN" HeaderText="Nullen entfernen">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.NULLENENTFERNEN") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn SortExpression="TEXTBEREINIGEN" HeaderText="Text bereinigen">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id="Checkbox2" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TEXTBEREINIGEN") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn SortExpression="ISTDATUM" HeaderText="ist Datum">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id="Checkbox3" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ISTDATUM") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn SortExpression="ISTZEIT" HeaderText="ist Zeit">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id="Checkbox5" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ISTZEIT") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn SortExpression="ABEDaten" HeaderText="ABE-Daten">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id="Checkbox4" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ABEDaten") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="l&#246;schen" Visible="false">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:ImageButton id=ibtnSRDelete runat="server" CausesValidation="False" ImageUrl="../Images/icon_nein_s.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.OrgName") %>' CommandName="Delete">
																			</asp:ImageButton>
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
																				<TD height="22">SAP-Name:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtAppID" runat="server" Visible="False" BorderStyle="None" BorderWidth="0px" Width="0px" BackColor="#CEDBDE" ForeColor="#CEDBDE" Height="0px">-1</asp:textbox><asp:textbox id="txtOrgNameAlt" runat="server" Visible="False" BorderStyle="None" BorderWidth="0px" Width="0px" BackColor="#CEDBDE" ForeColor="#CEDBDE" Height="0px">-1</asp:textbox><asp:textbox id="txtOrgNameNeu" runat="server" Width="160px" Height="20px" MaxLength="50"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Übersetzung:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtNewName" runat="server" Width="160px" Height="20px" MaxLength="50"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Reihenfolge Nr.:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtDisplayOrder" runat="server" Width="160px" Height="20px" MaxLength="2"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Ausrichtung:</TD>
																				<TD align="right" height="22"><asp:dropdownlist id="ddlAlignment" runat="server"></asp:dropdownlist></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD vAlign="top" width="185" height="22">Nullen entfernen:</TD>
																				<TD align="left" height="22"><asp:checkbox id="cbxNullenEntfernen" runat="server"></asp:checkbox></TD>
																			</TR>
																			<TR>
																				<TD vAlign="top" height="22">Text bereinigen:</TD>
																				<TD align="left" height="22"><asp:checkbox id="cbxTextBereinigen" runat="server"></asp:checkbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">ist Datum:</TD>
																				<TD align="left" height="22"><asp:checkbox id="cbxIstDatum" runat="server"></asp:checkbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">ist Zeit:</TD>
																				<TD align="left" height="22"><asp:checkbox id="cbxIstZeit" runat="server"></asp:checkbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">ABE-Daten:</TD>
																				<TD align="left" height="22"><asp:checkbox id="cbxABEDaten" runat="server"></asp:checkbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
															</table>
														</td>
													</tr>
													<TR>
														<TD align="left"><BR>
															<asp:linkbutton id="lnkBack" runat="server" CssClass="StandardButton">&#149;&nbsp;Zurück</asp:linkbutton></TD>
													</TR>
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
