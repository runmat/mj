<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ForbiddenUserNameManagement.aspx.vb" Inherits="CKG.Admin.ForbiddenUserNameManagement" %>
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
										<td class="PageNavigation" colSpan="2">Administration (Verwaltung verbotener 
											Benutzernamen)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="150" border="0">
												<TR>
													<TD class="TaskTitle" width="150">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neuen Eintrag erzeugen</asp:linkbutton></TD>
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
														<TD class="TaskTitle">&nbsp;</TD>
													</tr>
									</tr>
									<tr id="trSearch" runat="server">
										<td align="left">
											<table border="0" bgColor="white">
												<tr>
													<td vAlign="bottom" width="100">Benutzername</td>
													<td vAlign="bottom" width="160"><asp:label id="lblForbiddenUserNameName" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:label><asp:textbox id="txtFilterForbiddenUserNameName" runat="server" Width="160px" Height="20px">*</asp:textbox></td>
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
													<asp:BoundColumn Visible="False" DataField="ID" SortExpression="ID" HeaderText="ID"></asp:BoundColumn>
													<asp:ButtonColumn DataTextField="UserName" SortExpression="UserName" HeaderText="Benutzername" CommandName="Edit"></asp:ButtonColumn>
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
																<TD colspan="2">Eintrag bearbeiten:</TD>
															</TR>
															<TR id="trForbiddenUserNameName" runat="server">
																<TD height="22">Benutzername:</TD>
																<TD align="right" height="22"><asp:textbox id="txtForbiddenUserNameName" runat="server" Width="160px" Height="20px"></asp:textbox><asp:textbox id="txtID" runat="server" Visible="False" Width="10px" Height="10px" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE" BackColor="#CEDBDE">-1</asp:textbox></TD>
															</TR>
															<tr id="trApp" runat="server">
																<td class="InfoBoxFlat" align="left" colSpan="2"></td>
															</tr>
														</TABLE>
													</TD>
													<TD width="100%">
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
