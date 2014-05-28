<%@ Page enableEventValidation="false" Language="vb" AutoEventWireup="false" Codebehind="Report06.aspx.vb" Inherits="AppPorsche.Report06" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
			<input type="hidden" value="empty" name="txtAuftragsnummer"> <input type="hidden" value="empty" name="txtFahrgestellnummer">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<TD>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge" width="100%"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
														<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</TD>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" Width="100%" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" PageSize="50" BackColor="#FFFFFF">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Auftragsnummer" SortExpression="Auftragsnummer" HeaderText="Auftragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontonummer" SortExpression="Kontonummer" HeaderText="Kontonummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Angefordert am" SortExpression="Angefordert am" HeaderText="Anforderungsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Nummer ZB2"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Gesperrt" SortExpression="Gesperrt" HeaderText="Gesperrt">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:ButtonColumn Text="Storno" CommandName="Storno"></asp:ButtonColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:Panel id="lblWait" runat="server" Width="100%" HorizontalAlign="Center" Height="400px" CssClass="TextError">
										<P>&nbsp;&nbsp;&nbsp;
											<BR>
											&nbsp;&nbsp;
											<BR>
											&nbsp;&nbsp;&nbsp;
											<BR>
											&nbsp;&nbsp;&nbsp;
											<BR>
											Bitte warten, die Daten werden übermittelt.</P>
									</asp:Panel>
								</TD>
							</TR>
							<TR id="ShowScript" runat="server" visible="False">
								<TD colSpan="2">
									<script language="Javascript">
									<!-- //
									function StornoConfirm(Auftragsnummer,Vertragsnummer,AngefordertAm,Fahrgestellnummer,Briefnummer,Kontingentart)
									{
										var Check = window.confirm("Wollen Sie diesen Auftrag wirklich stornieren?\n\tKontonummer\t" + Vertragsnummer + "\n\tAnforderungsdatum\t" + AngefordertAm + "\n\tFahrgestellnr.\t" + Fahrgestellnummer + "\t\n\tKfz-Briefnr.\t" + Briefnummer + "\t\n\tKontingentart\t" + Kontingentart);
										return (Check);
									}
									//-->
									</script>
								</TD>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
			<asp:Literal id="Literal1" runat="server"></asp:Literal>
		</form>
	</body>
</HTML>
