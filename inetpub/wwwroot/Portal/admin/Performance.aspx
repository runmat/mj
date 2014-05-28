<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Performance.aspx.vb" Inherits="CKG.Admin.Performance" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" onload="window.setInterval('ReloadThis()',15000)" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server">Administration</asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Allgemeine Leistungsangaben)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table5" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdCreate" runat="server" Visible="False" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									&nbsp;<br>
									<asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" Width="100%">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn SortExpression="CategoryName" HeaderText="Kategorie">
												<ItemTemplate>
													<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%# "Performance2.aspx?PerformanceCounterID=" &amp; DataBinder.Eval(Container, "DataItem.PerformanceCounterID") %>' Text='<%# DataBinder.Eval(Container, "DataItem.CategoryName") %>'>
													</asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="CounterName" SortExpression="CounterName" HeaderText="Bezeichnung"></asp:BoundColumn>
											<asp:BoundColumn DataField="InstanceName" SortExpression="InstanceName" HeaderText="Instanz"></asp:BoundColumn>
											<asp:BoundColumn DataField="PerformanceCounterValue" SortExpression="PerformanceCounterValue" HeaderText="Wert">
												<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CounterUnit" SortExpression="CounterUnit" HeaderText="Einheit"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<script language="javascript">
		<!--
			function ReloadThis() {
				var theform = document.Form1;
				theform.submit();
			}
		// -->
		</script>
	</body>
</HTML>
