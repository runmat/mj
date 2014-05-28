<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change43_4.aspx.vb" Inherits="CKG.Components.ComCommon.Change43_4" %>
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
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<P><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Absenden</asp:linkbutton><U></U></P>
											</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:HyperLink id="lnkAntrag" runat="server" CssClass="StandardButton" NavigateUrl="Antrag.aspx" Target="_blank" Visible="False">&#149;&nbsp;Antrag erstellen</asp:HyperLink></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
										</TR>
									</TABLE>
									<P align="left">&nbsp;</P>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx" CssClass="TaskTitle" Visible="False">Fahrzeugauswahl</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkAdressAuswahl" runat="server" NavigateUrl="Change04_3.aspx" CssClass="TaskTitle" Visible="False">Adressauswahl</asp:hyperlink><asp:label id="lblAddress" runat="server" Visible="False"></asp:label><asp:label id="lblVersandart" runat="server" Visible="False"></asp:label><asp:label id="lblMaterialNummer" runat="server" Visible="False"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" vAlign="top" align="left"><asp:label id="lblMessage" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
												<TABLE id="Table7" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0" class="TableKontingent">
													<TR>
														<TD class="TextLarge" vAlign="top" noWrap>Adresse Halter:</TD>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="2" width="100%"><asp:label id="lblHalter" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" noWrap>Adresse Schein &amp; Schilder:</TD>
														<TD class="TextLarge" vAlign="top" align="left" colSpan="2"><asp:label id="lblEmpf" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False" Width="100%" bodyHeight="250" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Kontonummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Kontonummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>' ID="Label1" NAME="Label1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>' ID="Label2" NAME="Label2">
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
																<asp:LinkButton id="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>' ID="Label5" NAME="Label5">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="HEZ">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="chk0003" runat="server" Enabled="False"></asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="VBELN" SortExpression="VBELN" HeaderText="Auftragsnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="COMMENT" SortExpression="COMMENT" HeaderText="Status"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left">
												<P align="left"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></P>
											</TD>
										</TR>
										<TR>
											<TD colSpan="3"><!--#include File="../../../PageElements/Footer.html" -->
												<P align="right">&nbsp;</P>
											</TD>
										</TR>
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
