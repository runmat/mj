<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report01_2.aspx.vb" Inherits="AppVFS.Report01_2" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style1 {
                width: 208px;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"></asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"></td>
										</tr>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge" width="100%" height="19"><asp:label id="lblNoData" runat="server" Visible="False" Width="81px"></asp:label></td>
														<td align="right" height="19"></td>
													</tr>
													<TR>
														<TD class="LabelExtraLarge" width="100%">
															<asp:LinkButton id="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:LinkButton></TD>
														<TD align="right"></TD>
													</TR>
												</table>
												<TABLE id="Table4" height="0" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server">
													<TR>
														<TD class="style1"><STRONG>Stand:</STRONG></TD>
														<TD><asp:label id="lblStand" runat="server" Width="116px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="style1"><STRONG>Anzahl der Vermittler:</STRONG></TD>
														<TD><asp:label id="lblVermittlerAnzahl" runat="server" Width="57px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="style1"><STRONG><span lang="de">Gesamtbestand Kennzeichen</span>:</STRONG></TD>
														<TD><asp:label id="lblKennzeichenGesamtbestand" runat="server" Width="65px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="style1"><STRONG>Verkaufte Kennzeichen:</STRONG></TD>
														<TD><asp:label id="lblVerkaufteKennzeichen" runat="server" Width="65px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="style1"><STRONG>Unverkaufte Kennzeichen:</STRONG></TD>
														<TD><asp:label id="lblUnverkaufteKennzeichen" runat="server" Width="65px"></asp:label></TD>
													</TR>
                                                    <tr>
                                                        <td class="style1">
                                                            <strong>Bestand DAD (inkl. Rückläufer):</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblBestandDAD" runat="server" Width="116px"></asp:Label>
                                                        </td>
                                                    </tr>
													<TR>
														<TD class="style1"><STRONG>Verlust Kennzeichen:</STRONG></TD>
														<TD><asp:label id="lblVerlustKennzeichen" runat="server" Width="149px"></asp:label></TD>
													</TR>
													<TR>
														<TD class="style1"><STRONG>Rückläufer:</STRONG></TD>
														<TD><asp:label id="lblRuecklaeufer" runat="server" Width="65px"></asp:label></TD>
													</TR>
													
												</TABLE>
												<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD>
															<P align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></P>
														</TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="KUN_EXT_VM" SortExpression="KUN_EXT_VM" HeaderText="VD-Bezirk"></asp:BoundColumn>
														<asp:BoundColumn DataField="NAME1_VM" SortExpression="NAME1_VM" HeaderText="Name 1"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="NAME2_VM" SortExpression="NAME2_VM" HeaderText="Name 2">
                                                        </asp:BoundColumn>
														<asp:BoundColumn DataField="ANZ_GES" SortExpression="ANZ_GES" HeaderText="Kennzeichen&lt;br&gt;Gesamt"></asp:BoundColumn>
														<asp:BoundColumn DataField="ANZ_VERK" SortExpression="ANZ_VERK" HeaderText="Kennzeichen&lt;br&gt;Verkauft"></asp:BoundColumn>
														<asp:BoundColumn DataField="ANZ_UNVERK" SortExpression="ANZ_UNVERK" HeaderText="Kennzeichen&lt;br&gt;Unverkauft"></asp:BoundColumn>
														<asp:BoundColumn DataField="ANZ_VERL" SortExpression="ANZ_VERL" HeaderText="Kennzeichen&lt;br&gt;Verlust"></asp:BoundColumn>
														<asp:BoundColumn DataField="ANZ_RUECK" SortExpression="ANZ_RUECK" HeaderText="Kennzeichen&lt;br&gt;R&#252;ckl&#228;ufer"></asp:BoundColumn>
														<asp:TemplateColumn HeaderStyle-Wrap="false" HeaderText="Adresse anzeigen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
															<HeaderTemplate >
															
                                                                <asp:LinkButton CssClass="StandardButtonTable" ID="btnAlleAdressenAnzeigen"
                                                                    runat="server" CommandName="AlleAdressenAnzeigen" Text="Alle Adressen anzeigen"></asp:LinkButton><br />
                                                                
																<asp:LinkButton CssClass="StandardButtonTable" ID="btnAdressenAnzeigen" runat="server"
                                                                    CommandName="AdressenAnzeigen" Text="Adressen anzeigen"></asp:LinkButton>
																                                    
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox id="chkAdresseAnzeigen" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td width="120">&nbsp;</td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</TR>
							<TR>
								<td width="120">&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<TR id="ShowScript" runat="server">
								<td width="120">&nbsp;</td>
								<td>
									<script language="JavaScript">
										<!-- //
										// window.document.Form1.elements[window.document.Form1.length-3].focus();
										//-->
									</script>
								</td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
