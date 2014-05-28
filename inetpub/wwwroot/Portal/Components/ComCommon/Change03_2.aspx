<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change03_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change03_2" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
	<head>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</head>
	<body leftmargin="0" topmargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellspacing="0" cellpadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<table id="table1" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colspan="2"><asp:label id="lblHead" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td valign="top" width="120">
									<table id="table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120" border="0">
										<tr>
											<td class="TaskTitle">&nbsp;</td>
										</tr>
										<tr>
											<td valign="center" width="150"><asp:linkbutton id="lb_Confirm" runat="server" CssClass="StandardButton"> lb_Confirm</asp:linkbutton></td>
										</tr>
										<tr>
											<td valign="center" width="150"><asp:linkbutton id="lb_Back" runat="server" CssClass="StandardButton"> lb_Back</asp:linkbutton></td>
										</tr>
									</table>
								</td>
								<td valign="top">
									<table id="table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" valign="top"><asp:linkbutton id="lnkCreateExcel" runat="server">Excelformat</asp:linkbutton>&nbsp;</td>
										</tr>
									</table>
									<table id="table5" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td valign="top" align="left" colspan="3" height="41">
												<table cellspacing="0" cellpadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge" align="left" width="618" height="9"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="true"></asp:label></td>
														<td "nowrap" align="right" height="9">&nbsp;</td>
													</tr>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label><br>
												&nbsp;<br/>
												<table id="tbl0001" cellspacing="0" cellpadding="5" border="0">
													<tr id="tr_Auftrag" runat="server">
														<td class="TextLarge" valign="top" noWrap align="right"><asp:label id="lbl_Auftrag" runat="server">lbl_Auftrag</asp:label>&nbsp;</td>
														<td class="TextLarge"><asp:literal id="litAuftragShow" runat="server"></asp:literal></td>
													</tr>
													<tr id="tr_Haendler" runat="server">
														<td class="TextLarge" valign="top" noWrap align="right"><asp:label id="lbl_Haendler" runat="server">lbl_Haendler</asp:label>&nbsp;</td>
														<td class="TextLarge"><asp:literal id="litHaendlerShow" runat="server"></asp:literal></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td valign="top" align="left" colspan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="true" AllowPaging="true" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridtableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridtableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="Art" HeaderText="col_Art">
															<HeaderTemplate>
																<asp:LinkButton id="col_Art" runat="server" CommandArgument="Art" CommandName="Sort">col_Art</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="lblPruefungsart" runat="server" NAME="LabelPruefungsart" Text='<%# DataBinder.Eval(Container, "DataItem.Art") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fin" HeaderText="col_Fin">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fin" runat="server" CommandName="Sort" CommandArgument="Fin">col_Fin</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fin") %>' ID="LabelFin" NAME="LabelFin">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Referenz" HeaderText="col_Referenz">
															<HeaderTemplate>
																<asp:LinkButton id="col_Referenz" runat="server" CommandName="Sort" CommandArgument="Referenz">col_Referenz</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz") %>' ID="LabelReferenz" NAME="LabelReferenz">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>' ID="LabelKennzeichen" NAME="LabelKennzeichen">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Modell" HeaderText="col_Modell">
															<HeaderTemplate>
																<asp:LinkButton id="col_Modell" runat="server" CommandName="Sort" CommandArgument="Modell">col_Modell</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modell") %>' ID="LabelModell" NAME="LabelModell">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Farbe" HeaderText="col_Farbe">
															<HeaderTemplate>
																<asp:LinkButton id="col_Farbe" runat="server" CommandName="Sort" CommandArgument="Farbe">col_Farbe</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe") %>' ID="LabelFarbe" NAME="LabelFarbe">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Km_stand" HeaderText="col_Km_stand">
															<HeaderTemplate>
																<asp:LinkButton id="col_Km_stand" runat="server" CommandName="Sort" CommandArgument="Km_stand">col_Km_stand</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Km_stand") %>' ID="LabelKm_stand" NAME="LabelKm_stand">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Finanzierung" HeaderText="col_Finanzierung">
															<HeaderTemplate>
																<asp:LinkButton id="col_Finanzierung" runat="server" CommandName="Sort" CommandArgument="Finanzierung">col_Finanzierung</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Finanzierung") %>' ID="LabelFinanzierung" NAME="LabelFinanzierung">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Bemerkung" HeaderText="col_Bemerkung">
															<HeaderTemplate>
																<asp:LinkButton id="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="Bemerkung">col_Bemerkung</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>' ID="LabelBemerkung" NAME="LabelBemerkung">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fehler" HeaderText="col_Fehler">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fehler" runat="server" CommandName="Sort" CommandArgument="Fehler">col_Fehler</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fehler") %>' ID="LabelFehler" NAME="LabelFehler">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="true" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td width="120">&nbsp;</td>
								<td></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
