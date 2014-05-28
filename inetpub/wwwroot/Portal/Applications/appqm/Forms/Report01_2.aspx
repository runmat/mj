<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report01_2.aspx.vb" Inherits="appQM.Report01_2" %>
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
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top"></TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td lass="TaskTitle"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" Visible="False" NavigateUrl="Report01.aspx">Zusammenstellung von Abfragekriterien</asp:hyperlink></td>
											<TD colSpan="2"></TD>
										</tr>
										<TR>
											<TD align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TBODY>
														<tr>
															<td class="TaskTitle"><asp:hyperlink id="lnkExcel" runat="server" CssClass="TaskTitle" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
																<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Sichern</asp:linkbutton></td>
											</TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
								</td>
								<TD align="right"></TD>
							<TR>
								<TD class=""><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
								<TD align="right" colSpan="2">
									<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" Width="100%">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Bearbeiten">
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:ImageButton id="ibtnSREdit" runat="server" CommandName="Edit" ImageUrl="/Portal/Images/select.gif" CausesValidation="false"></asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="Nr."></asp:BoundColumn>
											<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="VKORG" SortExpression="VKORG" HeaderText="VKORG"></asp:BoundColumn>
											<asp:BoundColumn DataField="VKBUR" SortExpression="VKBUR" HeaderText="VKBUR"></asp:BoundColumn>
											<asp:BoundColumn DataField="ErfasstAm" SortExpression="ErfasstAm" HeaderText="ErfasstAm"></asp:BoundColumn>
											<asp:BoundColumn DataField="Erfasser" SortExpression="Erfasser" HeaderText="Erfasser"></asp:BoundColumn>
											<asp:BoundColumn DataField="Referenz" SortExpression="Referenz" HeaderText="Referenz"></asp:BoundColumn>
											<asp:BoundColumn DataField="AnzPos" SortExpression="AnzPos" HeaderText="AnzPos"></asp:BoundColumn>
											<asp:BoundColumn DataField="Meldedatum" SortExpression="Meldedatum" HeaderText="Meldedatum"></asp:BoundColumn>
											<asp:BoundColumn DataField="Kundennr" SortExpression="Kundennr" HeaderText="Kundennr"></asp:BoundColumn>
											<asp:BoundColumn DataField="Kundenname" SortExpression="Kundenname" HeaderText="Kundenname"></asp:BoundColumn>
											<asp:BoundColumn DataField="Ansprechpartner" SortExpression="Ansprechpartner" HeaderText="Ansprechpartner"></asp:BoundColumn>
											<asp:BoundColumn DataField="Kontaktdaten" SortExpression="Kontaktdaten" HeaderText="Kontaktdaten"></asp:BoundColumn>
											<asp:BoundColumn DataField="Kundenreklamation" SortExpression="Kundenreklamation" HeaderText="Kundenreklamation"></asp:BoundColumn>
											<asp:BoundColumn DataField="Prozess" SortExpression="Prozess" HeaderText="Prozess"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fehler" SortExpression="Fehler" HeaderText="Fehler"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fehlerbeschreibung" SortExpression="Fehlerbeschreibung" HeaderText="Fehlerbeschreibung"></asp:BoundColumn>
											<asp:BoundColumn DataField="FehlerverursacherFirma" SortExpression="FehlerverursacherFirma" HeaderText="FehlerverursacherFirma"></asp:BoundColumn>
											<asp:BoundColumn DataField="FehlerverursacherName" SortExpression="FehlerverursacherName" HeaderText="FehlerverursacherName"></asp:BoundColumn>
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
					<td></td>
				</tr>
				<tr>
					<td></td>
					<td><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			</table>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
