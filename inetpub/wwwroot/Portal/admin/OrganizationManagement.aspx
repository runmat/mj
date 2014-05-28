<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OrganizationManagement.aspx.vb" Inherits="CKG.Admin.OrganizationManagement" %>
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
								<TBODY>
									<TR>
										<td class="PageNavigation" colSpan="2">Administration (Organisationsverwaltung)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="150" border="0">
												<TR>
													<TD class="TaskTitle" width="150">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neue Organisation anlegen</asp:linkbutton></TD>
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
														<TD class="TaskTitle"><asp:hyperlink id="lnkUserManagement" runat="server" CssClass="TaskTitle" NavigateUrl="UserManagement.aspx">Benutzerverwaltung</asp:hyperlink><asp:hyperlink id="lnkGroupManagement" runat="server" CssClass="TaskTitle" NavigateUrl="GroupManagement.aspx">Gruppenverwaltung</asp:hyperlink><asp:hyperlink id="lnkCustomerManagement" runat="server" CssClass="TaskTitle" NavigateUrl="CustomerManagement.aspx">Kundenverwaltung</asp:hyperlink>
                                                            <asp:hyperlink id="lnkAppManagement" runat="server" CssClass="TaskTitle" 
                                                                NavigateUrl="AppManagement.aspx" Visible="False">Anwendungsverwaltung</asp:hyperlink>
															<asp:hyperlink id="lnkArchivManagement" runat="server" CssClass="TaskTitle" NavigateUrl="ArchivManagement.aspx">Archivverwaltung</asp:hyperlink>&nbsp;</TD>
													</tr>
									</tr>
									<tr id="trSearch" runat="server">
										<td align="left">
											<table border="0" bgColor="white">
												<TR>
													<TD vAlign="bottom" width="100">Firma:</TD>
													<TD vAlign="bottom" width="160"><asp:label id="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:label><asp:dropdownlist id="ddlFilterCustomer" runat="server" Visible="False" Width="160px" Height="20px"></asp:dropdownlist></TD>
													<TD vAlign="bottom"></TD>
												</TR>
												<tr>
													<td vAlign="bottom" width="100">Organisation:</td>
													<td vAlign="bottom" width="160"><asp:label id="lblOrganizationName" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:label><asp:textbox id="txtFilterOrganizationName" runat="server" Visible="False" Width="160px" Height="20px">*</asp:textbox></td>
													<td vAlign="bottom">
														<asp:LinkButton id="btnSuche" runat="server" CssClass="StandardButton">Suche</asp:LinkButton></td>
												</tr>
											</table>
										</td>
									</tr>
									<TR id="trSearchSpacer" runat="server">
										<TD align="left" height="25"></TD>
									</TR>
									<tr id="trSearchResult" runat="server">
										<td align="left"><asp:datagrid id="dgSearchResult" runat="server" Width="100%" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" BackColor="White">
												<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
												<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
												<Columns>
													<asp:BoundColumn Visible="False" DataField="OrganizationID" SortExpression="OrganizationID" HeaderText="OrganizationID"></asp:BoundColumn>
													<asp:ButtonColumn DataTextField="OrganizationName" SortExpression="OrganizationName" HeaderText="Organisation" CommandName="Edit"></asp:ButtonColumn>
													<asp:BoundColumn DataField="OrganizationReference" SortExpression="OrganizationReference" HeaderText="Referenz"></asp:BoundColumn>
													<asp:TemplateColumn SortExpression="AllOrganizations" HeaderText="Zeige ALLE&lt;br&gt;Organisationen">
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
														<ItemTemplate>
															<asp:CheckBox id=CheckBox1 runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.AllOrganizations") %>'>
															</asp:CheckBox>
														</ItemTemplate>
														<EditItemTemplate>
															<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AllOrganizations") %>'>
															</asp:TextBox>
														</EditItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="Customername" SortExpression="Customername" HeaderText="Firma"></asp:BoundColumn>
													<asp:BoundColumn DataField="OName" SortExpression="OName" HeaderText="Kontakt-Name"></asp:BoundColumn>
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
																<TD height="22">Firma:</TD>
																<TD align="right" height="22"><asp:textbox id="txtCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px" Height="20px" Enabled="False"></asp:textbox><asp:textbox id="txtCustomerID" runat="server" Visible="False" Width="10px" Height="10px" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE" BackColor="#CEDBDE">-1</asp:textbox></TD>
															</TR>
															<TR id="trOrganizationName" runat="server">
																<TD height="22">Organisationsname:</TD>
																<TD align="right" height="22"><asp:textbox id="txtOrganizationName" runat="server" Width="160px" Height="20px"></asp:textbox><asp:textbox id="txtOrganizationID" runat="server" Visible="False" Width="10px" Height="10px" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE" BackColor="#CEDBDE">-1</asp:textbox></TD>
															</TR>
															<TR>
																<TD height="22">Organisationsreferenz:</TD>
																<TD align="right" height="22"><asp:textbox id="txtOrganizationReference" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
															</TR>
															<TR>
																<TD height="22">Zeige ALLE Organisationen:</TD>
																<TD align="right" height="22"><asp:checkbox id="cbxAllOrganizations" runat="server"></asp:checkbox></TD>
															</TR>
															<tr id="trApp" runat="server">
																<td class="InfoBoxFlat" align="left" colSpan="2"></td>
															</tr>
														</TABLE>
													</TD>
													<TD width="100%">
														<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
															<TR>
																<TD height="22">Kontakt-Name:</TD>
																<TD align="right" height="22"><asp:textbox id="txtCName" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
															</TR>
															<TR>
																<TD vAlign="top" height="22">Kontakt-Adresse:</TD>
																<TD align="right" height="22"><asp:textbox id="txtCAddress" runat="server" Width="160px" Height="80px" TextMode="MultiLine"></asp:textbox></TD>
															</TR>
															<TR>
																<TD height="22">Mailadresse Anzeigetext:</TD>
																<TD align="right" height="22"><asp:textbox id="txtCMailDisplay" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
															</TR>
															<TR>
																<TD height="22">Mailadresse:</TD>
																<TD align="right" height="22"><asp:textbox id="txtCMail" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
															</TR>
   															<TR>
																<TD height="22">Web-Adresse Anzeigetext:</TD>
																<TD align="right" height="22"><asp:textbox id="txtCWebDisplay" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
															</TR>
															<TR>
																<TD height="22">Web-Adresse:</TD>
																<TD align="right" height="22"><asp:textbox id="txtCWeb" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
															</TR>
															<TR id="trStyle" runat="server">
																<TD class="InfoBoxFlat" colSpan="2" height="22">
																	<TABLE id="tblStyle" cellSpacing="0" cellPadding="0" width="100%" border="0">
																		<TR>
																			<TD align="middle" colSpan="2" height="22">Style</TD>
																		</TR>
																		<TR>
																			<TD height="22">Pfad zum Logo:</TD>
																			<TD align="right" height="22"><asp:textbox id="txtLogoPath" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																		</TR>
																		<TR>
																			<TD height="22">Pfad zu den Stylesheets:</TD>
																			<TD align="right" height="22"><asp:textbox id="txtCssPath" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																		</TR>
																		<TR>
																			<TD height="22"></TD>
																			<TD align="right" height="22"></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
													<TD vAlign="top" align="right"></TD>
												</TR>
											</table>
										</td>
									</tr>
									<TR>
										<TD align="left" height="25"></TD>
									</TR>
								</TBODY></TABLE>
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
				</TBODY></table>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
