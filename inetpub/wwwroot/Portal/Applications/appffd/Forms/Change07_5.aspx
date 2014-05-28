<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change07_5.aspx.vb" Inherits="AppFFD.Change07_5"%>
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
								<td class="PageNavigation" colSpan="3">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Bestätigung)</asp:label></td>
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
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx" CssClass="TaskTitle" Visible="False">Fahrzeugauswahl</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkAdressAuswahl" runat="server" NavigateUrl="Change04_3.aspx" CssClass="TaskTitle" Visible="False">Adressauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" vAlign="top" align="left"><asp:label id="lblMessage" runat="server"></asp:label>&nbsp;</TD>
													</TR>
												</TABLE>
												<TABLE id="Table7" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white" class="TableKontingent">
													<TR>
														<TD class="StandardTableAlternate" vAlign="top">Versandart:</TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" colSpan="2" width="100%"><asp:label id="lblVersandart" runat="server"></asp:label><asp:label id="lblMaterialNummer" runat="server" Visible="False"></asp:label><asp:label id="lblVersandhinweis" runat="server" Visible="False"> - Gilt nicht für gesperrt angelegte Anforderungen!</asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top">Adresse:</TD>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="2"><asp:label id="lblAddress" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
												<asp:datagrid id="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" Width="100%" AutoGenerateColumns="False" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnr."></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
														<asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Vertragsnr."></asp:BoundColumn>
														<asp:BoundColumn DataField="TEXT300" SortExpression="TEXT300" HeaderText="Anfragenr."></asp:BoundColumn>
														<asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="Briefnr."></asp:BoundColumn>
														<asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" HeaderText="Ordernr."></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Bezahlt">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=Bezahlt runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="COC">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=ZZCOCKZ runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="VBELN" SortExpression="VBELN" HeaderText="Auftragsnr."></asp:BoundColumn>
														<asp:BoundColumn DataField="COMMENT" SortExpression="COMMENT" HeaderText="Kommentar"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Stnd.&lt;br&gt;temp.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chk0001" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Stnd.&lt;br&gt;endg.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chk0002" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="DP&lt;br&gt;endg.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chk0004" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Retail">
															<ItemTemplate>
																<asp:CheckBox id="chk0003" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="Kontingentart"></asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
												</asp:datagrid>
											</td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
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
