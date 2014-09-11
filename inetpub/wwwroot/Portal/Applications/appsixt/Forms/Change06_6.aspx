<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change06_6.aspx.vb" Inherits="AppSIXT.Change06_6" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server" Visible="False"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top"></TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TBODY>
														<tr>
															<TD class="TaskTitle" width="100%"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank" CssClass="TaskTitle">Excelformat</asp:hyperlink>&nbsp;
																<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
															<td class="TaskTitle" noWrap><asp:hyperlink id="lnkFahrzeugsuche" runat="server" Visible="False" NavigateUrl="javascript:window.close()">Fenster schlieﬂen</asp:hyperlink></td>
											</TD>
										</TR>
									</TABLE>
									<asp:image id="imgWait" runat="server" ImageUrl="/Portal/Images/bittewarten.gif" Height="20px"></asp:image></td>
								<TD align="right"></TD>
							<TR>
								<TD class=""><asp:label id="lblNoData" runat="server" Font-Bold="True"></asp:label></TD>
								<TD align="right" colSpan="2"><asp:dropdownlist id="ddlPageSize" runat="server" Visible="False" AutoPostBack="True"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" AutoGenerateColumns="False" BackColor="White">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Modellbezeichnung" SortExpression="Modellbezeichnung" HeaderText="Modell"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="Eingangsdatum" SortExpression="Eingangsdatum" HeaderText="Eingangsdatum" DataFormatString="{0:dd.MM.yy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="Bemerkung" SortExpression="Bemerkung" HeaderText="Bemerkung"></asp:BoundColumn>
											<asp:BoundColumn DataField="Datum Erstzulassung" SortExpression="Datum Erstzulassung" HeaderText="Datum&lt;br&gt;Erstzulassung" DataFormatString="{0:dd.MM.yy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="Belegnummer" SortExpression="Belegnummer"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
								<TD colSpan="1"></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td></td>
					<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
				</tr>
				<tr>
					<td></td>
					<td><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			</table>
			</TD></TR>
			<TR id="ShowScript" runat="server" visible="False">
				<TD></TD>
			</TR>
			</TBODY></TABLE><asp:label id="lblScript" runat="server"></asp:label></form>
	</body>
</HTML>
