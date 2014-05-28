<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report44.aspx.vb" Inherits="CKG.Components.ComCommon.Report44" %>
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
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" vAlign="top" width="120">&nbsp;</TD>
								<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Report01.aspx" CssClass="TaskTitle">Zusammenstellung von Abfragekriterien</asp:hyperlink>&nbsp;</TD>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TextHeader" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Enabled="False">OK</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdPrint" runat="server" Visible="False" CssClass="StandardButton">Drucken</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0">
										<tr>
											<td>
												<P align="center"><b><font size="4"><u>Erinnerung</u></font></b></P>
												<P><BR>
													sollte sich diese Erinnerung mit der Rücksendung bzw. mit der Bezahlung 
													überschnitten haben, betrachten Sie bitte diese Meldung als gegenstandslos.<BR>
													<BR>
												</P>
											</td>
										</tr>
										<TR>
											<TD>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge">
															<asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
															<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label>
														</td>
														<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label>
											</TD>
										</TR>
										<TR>
											<TD>
												<asp:datagrid id="DataGrid1" runat="server" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" Width="100%" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart" SortExpression="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Versanddatum" HeaderText="Versanddatum" SortExpression="Versanddatum"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kennzeichen" HeaderText="Kennzeichen" SortExpression="Kennzeichen"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer" Visible="True">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Vertragsnummer" CommandName="sort" CommandArgument="Vertragsnummer" Runat="server">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True"  Runat="server" ID="lblVertragsnummer"><%# DataBinder.Eval(Container, "DataItem.Vertragsnummer")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer" Visible="True">
															<ItemTemplate>
																<asp:HyperLink Runat="server" ID="lnkFahrgestellnummer" Target="_blank" NavigateUrl='<%# createLink(DataBinder.Eval(Container, "DataItem.Fahrgestellnummer"))%>' >
																	<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer")%>
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"></PagerStyle>
												</asp:datagrid>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td width="100"></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</TR>
							<tr>
								<td width="100"></td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
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
