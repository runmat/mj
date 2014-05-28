<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change05.aspx.vb" Inherits="AppPorsche.Change05" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> Auswählen</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="100" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr id="trKopfdaten" runat="server">
											<td colSpan="2">
												<P align="left">
													<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></P>
											</td>
										</tr>
										<TR id="trDataGrid1" runat="server">
											<TD align="middle" colSpan="2">
												<P align="left"><asp:datagrid id="DataGrid1" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" BackColor="White">
														<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
														<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
														<Columns>
															<asp:BoundColumn Visible="False" DataField="H&#228;ndlernummerText" SortExpression="H&#228;ndlernummerText" HeaderText="H&#228;ndlernummer">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundColumn>
															<asp:TemplateColumn SortExpression="H&#228;ndlernummerText" HeaderText="H&#228;ndlernummer">
																<HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
																<ItemStyle HorizontalAlign="Left"></ItemStyle>
																<ItemTemplate>
																	<asp:LinkButton id=LinkButton1 runat="server" CssClass="StandardButtonTable" Text='<%# DataBinder.Eval(Container, "DataItem.HändlernummerText") %>' CommandName="Select" CausesValidation="False">
																	</asp:LinkButton>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn></asp:BoundColumn>
															<asp:BoundColumn DataField="H&#228;ndleradresse" SortExpression="H&#228;ndleradresse" HeaderText="H&#228;ndleradresse"></asp:BoundColumn>
															<asp:BoundColumn DataField="StandardTemp" SortExpression="StandardTemp" HeaderText="Standard tempor&#228;r">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="StandardEndg" SortExpression="StandardEndg" HeaderText="Standard endg&#252;ltig">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
															</asp:BoundColumn>
														</Columns>
														<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
													</asp:datagrid></P>
											</TD>
											<TD align="middle" colSpan="1">
												<P align="left">&nbsp;</P>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
