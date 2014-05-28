<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report05_02.aspx.vb" Inherits="CKG.Components.ComCommon.Report05_02"%>
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
			<input type="hidden" value="empty" name="txtAuftragsnummer">
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
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<TD>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr id="trExcel" runat="server">
														<td class="LabelExtraLarge"><asp:linkbutton id="lnkCreateExcel" runat="server">Excelformat</asp:linkbutton>&nbsp;&nbsp;
														</td>
														<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
													<tr>
														<td class="LabelExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></td>
														<td align="right"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
													</tr>
												</table>
											</TD>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="VBELN"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="EQUNR"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Rechnungsnummer" SortExpression="Rechnungsnummer" HeaderText="Rechnungsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Zulassungsdatum" SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Angefordert am" SortExpression="Angefordert am" HeaderText="Angefordert am" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Nummer ZB2" SortExpression="Nummer ZB2" HeaderText="Nummer ZB2"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Gesperrt" SortExpression="Gesperrt" HeaderText="Gesperrt">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton CssClass="StandardButtonTable" ID="lbStorno" Runat="server" CommandName="Storno">Storno</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR id="ShowScript" runat="server" visible="False">
								<TD colSpan="2">
									<script language="Javascript">
									<!-- //
									function StornoConfirm(Auftragsnummer,Vertragsnummer,AngefordertAm,Fahrgestellnummer,Briefnummer,Kontingentart)
									{
										var Check = window.confirm("Wollen Sie diesen Auftrag wirklich stornieren?\n\tVertrag\t\t" + Vertragsnummer + "\n\tAngefordert am\t" + AngefordertAm + "\n\tZulassungsdatum\t" + Fahrgestellnummer + "\t\n\tKfz-Briefnr.\t" + Briefnummer + "\t\n\tKontingentart\t" + Kontingentart);
										if (Check)
										{
											window.document.Form1.txtAuftragsnummer.value = Auftragsnummer;
										}
										return (Check);
									}
									//-->
									</script>
								</TD>
							</TR>
							<TR>
								<td width="120">&nbsp;</td>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
