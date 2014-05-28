<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FieldTranslation.aspx.vb" Inherits="CKG.Admin.FieldTranslation" %>
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
										<td class="PageNavigation" align="left" height="25">Feldübersetzungen für
											<asp:label id="lblAppURL" runat="server"></asp:label>&nbsp;
											<asp:label id="lblKundeSprache" runat="server"></asp:label>&nbsp;</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TextHeader" width="120"></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" Visible="true" runat="server"  Enabled="false" CssClass="StandardButton">&#149;&nbsp;Neue Feldübersetzung anlegen</asp:linkbutton></TD>
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
                                                            <asp:hyperlink id="lnkAppManagment" runat="server" CssClass="TaskTitle" 
                                                                NavigateUrl="AppManagement.aspx" Visible="False">Anwendungsverwaltung</asp:hyperlink></TD>
													</tr>
													<TR>
														<TD align="left" height="25">
															<TABLE id="tblLeft2" cellSpacing="2" cellPadding="0" width="100%" bgColor="white" border="0">
																<TR>
																	<TD height="22">Kunde:</TD>
																	<TD align="left" height="22"><asp:dropdownlist id="ddlCustomer" runat="server" Width="350px" AutoPostBack="True"></asp:dropdownlist></TD>
																</TR>
																<TR>
																	<TD height="22">Sprache:</TD>
																	<TD align="left" height="22"><asp:dropdownlist id="ddlLanguage" runat="server" Width="350px" AutoPostBack="True"></asp:dropdownlist></TD>
																</TR>
																<TR>
																	<TD height="22">&nbsp;&nbsp;
																	</TD>
																	<TD align="left" height="22">&nbsp;&nbsp;
																	</TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left">
															<asp:datagrid id="dgSearchResult" runat="server" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="100%" BackColor="White">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="ApplicationFieldID" SortExpression="ApplicationFieldID" HeaderText="ApplicationFieldID"></asp:BoundColumn>
																	<asp:BoundColumn Visible="False" DataField="FieldType" SortExpression="FieldType" HeaderText="FieldType"></asp:BoundColumn>
																	<asp:BoundColumn Visible="False" DataField="FieldName" SortExpression="FieldName" HeaderText="FieldName"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Field" SortExpression="Field" HeaderText="Seitenelement"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="Visibility" HeaderText="Sichtbar">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Visibility") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="Content" SortExpression="Content" HeaderText="&#220;bersetzung"></asp:BoundColumn>
																	<asp:TemplateColumn>
																		<ItemTemplate>
																			<asp:LinkButton id=LinkButton2 runat="server" Visible='<%# CInt(DataBinder.Eval(Container, "DataItem.ApplicationFieldID")) = -1 %>' Text="Anlegen" CausesValidation="False" CommandName="Create">Anlegen</asp:LinkButton>
																			<asp:LinkButton id="lbAendern" runat="server" Visible='<%# Not (CInt(DataBinder.Eval(Container, "DataItem.ApplicationFieldID")) = -1) %>' Text="Ändern" CausesValidation="False" CommandName="Edit">Ändern</asp:LinkButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<ItemTemplate>
																			<asp:LinkButton id=LinkButton3 runat="server" Text="Löschen" CausesValidation="False" CommandName="Delete" Enabled='<%# Not (CInt(DataBinder.Eval(Container, "DataItem.ApplicationFieldID")) = -1) %>'>Löschen</asp:LinkButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Mode="NumericPages"></PagerStyle>
															</asp:datagrid></td>
													</tr>
													<tr id="trEditUser" runat="server">
														<td align="left">
															<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="100%" bgColor="white" border="0">
																<TR>
																	<TD height="22">Feld:</TD>
																	<TD align="left" height="22"><asp:label id="lblField" runat="server"></asp:label><asp:label id="lblFieldID" runat="server" Visible="False"></asp:label><asp:label id="lblFieldIDSave" runat="server" Visible="False"></asp:label></TD>
																</TR>
																<TR>
																	<TD height="22">Feldtyp:</TD>
																	<TD align="left" height="22"><asp:radiobutton id="rbLabel" runat="server" GroupName="grpFieldType" Checked="True" Text="Label" AutoPostBack="True"></asp:radiobutton>&nbsp;&nbsp;
																		<asp:radiobutton id="rbLinkButton" runat="server" Text="LinkButton" GroupName="grpFieldType" AutoPostBack="True"></asp:radiobutton>&nbsp;&nbsp;
																		<asp:radiobutton id="rbRadioButton" runat="server" Text="RadioButton" GroupName="grpFieldType" AutoPostBack="True"></asp:radiobutton>&nbsp;&nbsp;
																		<asp:radiobutton id="rbTableRow" runat="server" Text="Tabellenzeile" GroupName="grpFieldType" AutoPostBack="True"></asp:radiobutton>&nbsp;&nbsp;
																		<asp:radiobutton id="rbGridColumn" runat="server" Text="Grid-Spalte" GroupName="grpFieldType" AutoPostBack="True"></asp:radiobutton>
																		<asp:radiobutton id="rbTextBox" runat="server" Text="TextBox" GroupName="grpFieldType" AutoPostBack="True"></asp:radiobutton></TD>
																</TR>
																<TR>
																	<TD height="22">Feldname:</TD>
																	<TD align="left" height="22"><asp:textbox id="txtFieldName" runat="server" Width="350px" MaxLength="50" Height="20px"></asp:textbox></TD>
																</TR>
																<TR>
																	<TD height="22">Sichtbar:</TD>
																	<TD align="left" height="22"><asp:checkbox id="cbxVisible" runat="server"></asp:checkbox></TD>
																</TR>
																<TR>
																	<TD height="22">Übersetzung:</TD>
																	<TD align="left" height="22"><asp:textbox id="txtContent" runat="server" Width="350px" MaxLength="50" Height="20px"></asp:textbox></TD>
																</TR>
																<TR>
																	<TD height="22"><asp:Label ID="lbl_TextTooltip" Runat="server" Visible="False">ToolTip:</asp:Label></TD>
																	<TD align="left" height="22"><asp:textbox id="txt_Tooltip" Visible="False" runat="server" Width="350px" MaxLength="50" Height="20px"></asp:textbox></TD>
																</TR>
																<TR id="trStandard" runat="server">
																	<TD height="22">Standard:</TD>
																	<TD align="left" height="22"><asp:label id="lblStandard" runat="server"></asp:label></TD>
																</TR>
															</TABLE>
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
