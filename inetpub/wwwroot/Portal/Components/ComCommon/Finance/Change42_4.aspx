<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change42_4.aspx.vb" Inherits="CKG.Components.ComCommon.Change42_4" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Bestätigung)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
									</TABLE>
									&nbsp;
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change42.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change42_2.aspx" Visible="False">Fahrzeugauswahl</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkAdressAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change42_3.aspx" Visible="False">Adressauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<TABLE class="BorderFullAlert" id="tblMessage" cellSpacing="2" cellPadding="1" border="0" runat="server">
													<TR>
														<TD noWrap colSpan="3"><strong><STRONG><asp:label id="lbl_Hinweis" runat="server" CssClass="TextError" EnableViewState="False">Hinweis: Überzählige Anforderungen werden gesperrt angelegt.</asp:label></STRONG></strong></TD>
													</TR>
													<TR>
														<TD noWrap colSpan="3"></TD>
													</TR>
													<TR>
														<TD noWrap>Kontingentart <U>Standard temporär</U>:</TD>
														<TD noWrap><strong><asp:label id="lblTemp" runat="server"></asp:label></strong></TD>
														<TD noWrap>überzählige Anforderung(en)</TD>
													</TR>
													<TR>
														<TD>Kontingentart <U>Standard endgültig</U>:</TD>
														<TD><strong><asp:label id="lblEnd" runat="server"></asp:label></strong></TD>
														<TD noWrap>überzählige Anforderung(en)</TD>
													</TR>
												</TABLE>
												<strong><i>
														<asp:label id="lblMessage" runat="server"></asp:label></i></strong></TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE class="TableKontingent" id="Table7" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="top">Versandart:</TD>
														<TD class="TextLarge" vAlign="top" align="left" width="100%" colSpan="2"><asp:label id="lblVersandart" runat="server"></asp:label><asp:label id="lblMaterialNummer" runat="server" Visible="False"></asp:label><asp:label id="lbl_Versandhinweis" runat="server" Visible="False"> - Gilt nicht für gesperrt angelegte Anforderungen!</asp:label></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="top">Adresse:</TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" colSpan="2"><asp:label id="lblAddress" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
												<asp:datagrid id="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" Width="100%" AutoGenerateColumns="False" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Kontonummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Kontonummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" NAME="Label1" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>' ID="Label2" NAME="Label2">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
															<HeaderTemplate>
																<asp:LinkButton id="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>' ID="Label3" NAME="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>' ID="Label4" NAME="Label4">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM" CommandName="Sort">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label5 runat="server" NAME="Label5" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="Bezahlt">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="Bezahlt" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="CoC">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_COC" runat="server" CommandArgument="LICENSE_NUM" CommandName="Sort">col_COC</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox id=Checkbox1 runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="MANDT" HeaderText="Versandart">
															<ItemTemplate>
																<asp:Label id=lblTemp2 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="1" %>'>Temporär</asp:Label>
																<asp:Label id=lblEndg runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="2" %>'>Endgültig</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="AUGRU" HeaderText="Abrufgrund">
															<ItemTemplate>
																<asp:Label id=lblAUGRU runat="server" NAME="Label6" ToolTip='<%# DataBinder.Eval(Container, "DataItem.ANFNR") %>' Text='<%# DataBinder.Eval(Container, "DataItem.AUGRU_Klartext") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="VBELN" SortExpression="VBELN" HeaderText="Auftragsnr."></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="COMMENT" SortExpression="COMMENT" HeaderText="Kommentar"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
												</asp:datagrid></td>
										</tr>
										<tr>
											<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --></td>
										</tr>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
