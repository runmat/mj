<%@ Page Language="vb" AutoEventWireup="false" Codebehind="admZulassung.aspx.vb" Inherits="AppSIXT.admZulassung" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
										<td class="PageNavigation" colSpan="2" height="25">Administration 
											(Zulassungszuordnung)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TaskTitle">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"></TD>
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
									<TR>
										<TD class=""><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
									</TR>
									<TR id="trSearchSpacer" runat="server">
										<TD align="left" height="25"><asp:datagrid id="dgSearchResult" runat="server" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" BackColor="White" Width="500px">
												<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
												<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
												<Columns>
													<asp:BoundColumn Visible="False" DataField="GroupID" SortExpression="GroupID" HeaderText="GroupID"></asp:BoundColumn>
													<asp:ButtonColumn Visible="False" DataTextField="GroupName" SortExpression="GroupName" HeaderText="Gruppe" CommandName="Edit"></asp:ButtonColumn>
													<asp:TemplateColumn HeaderText="Gruppe">
														<ItemTemplate>
															<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupName") %>'>
															</asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Zulassungen ab 11 Uhr&lt;br&gt; (f&#252;r Folgetag) m&#246;glich*">
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
														<ItemTemplate>
															<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsCustomerGroup") %>'>
															</asp:CheckBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn Visible="False" DataField="Authorizationright" SortExpression="Authorizationright" HeaderText="Autorisierungs-&lt;br&gt;level">
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
													</asp:BoundColumn>
													<asp:TemplateColumn Visible="False" HeaderText="l&#246;schen">
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
														<ItemTemplate>
															<asp:ImageButton id="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="/Portal/Images/icon_nein_s.gif"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
												<PagerStyle Mode="NumericPages"></PagerStyle>
											</asp:datagrid></TD>
									</TR>
									<tr id="trEditUser" runat="server">
										<td align="left"></td>
									</tr>
									<TR>
										<TD align="left" height="25">*
											<asp:CheckBox id="CheckBox2" runat="server" Checked="True" Enabled="False"></asp:CheckBox>=Zulassung 
											erlaubt,
											<asp:CheckBox id="CheckBox3" runat="server" Enabled="False"></asp:CheckBox>=Zulassung 
											nicht erlaubt</TD>
									</TR>
								</TBODY></TABLE>
						</td>
					</tr>
					<tr>
						<td></td>
						<td><asp:label id="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:label></td>
					</tr>
					<tr>
						<td></td>
						<td><!--#include File="../../../PageElements/Footer.html" --></td>
					</tr>
				</TBODY></table>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
