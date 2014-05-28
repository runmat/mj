<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report45_1.aspx.vb" Inherits="CKG.Components.ComCommon.Report45_1" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top"></TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<td>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<TD class="LabelExtraLarge">
															<asp:linkbutton id="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:linkbutton>&nbsp;&nbsp;</TD>
														<td align="right"></td>
													</tr>
													<TR>
														<TD class="LabelExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
														<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</TR>
												</table>
											</td>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Width="100%">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnr">
															<ItemTemplate>
																<asp:HyperLink id=VIN runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' ToolTip="Anzeige Fahrzeughistorie" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' Target="_blank">
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Rechnungsnummer">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="COC" SortExpression="COC" HeaderText="Datenblatt"></asp:BoundColumn>
														
														<asp:TemplateColumn HeaderText="col_ErfassungDatum" SortExpression="Erfassung Fahrzeug">
															<HeaderTemplate>
																<asp:LinkButton Runat="server" CommandName="sort" CommandArgument="Erfassung Fahrzeug" ID="col_ErfassungDatum">col_ErfassungDatum</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblErfassungsDatum" Runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Erfassung Fahrzeug","{0:dd.MM.yyyy}" )%>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
																												
														<asp:BoundColumn DataField="Nummer ZB2" SortExpression="Nummer ZB2" HeaderText="Nummer ZB2"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="col_Fahrzeugklasse" SortExpression="Fahrzeugklasse">
															<HeaderTemplate>
																<asp:LinkButton Runat="server" CommandName="sort" CommandArgument="Fahrzeugklasse" ID="col_Fahrzeugklasse">col_Fahrzeugklasse</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblFahrzeugklasse" Runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrzeugklasse")%>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="col_BriefkopieAnfordern">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton Runat="server" ID="col_BriefkopieAnfordern">col_BriefkopieAnfordern</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<p align="center">
																	<asp:LinkButton id="lbBriefkopie" runat="server" Visible="True" CssClass="StandardButtonSmall" CommandArgument=' <%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer")%> ' Text='Briefkopie' commandname="Briefkopie" CausesValidation="True">
																	</asp:LinkButton></p>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" CssClass="TextExtraLarge" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
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
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,Kennzeichen,Ordernummer,Angefordert,Versendet) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\n\tKfz-Kennzeichen\t" + Kennzeichen + "\t\n\tOrdernummer\t" + Ordernummer + "\t\n\tAngefordert\t" + Angefordert + "\t\n\tVersendet\t" + Versendet);
						return (Check);
						}
						//-->
						</script>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
