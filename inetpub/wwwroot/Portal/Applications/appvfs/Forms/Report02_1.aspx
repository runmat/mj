<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report02_1.aspx.vb" Inherits="AppVFS.Report02_1" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
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
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
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
														<TD class="LabelExtraLarge" width="100%"><asp:linkbutton id="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:linkbutton></TD>
														<TD align="right"></TD>
													</TR>
												</table>
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
														<asp:BoundColumn DataField="VD-Bezirk" SortExpression="VD-Bezirk"
                                                            HeaderText="VD-Bezirk"></asp:BoundColumn>
														<asp:BoundColumn DataField="Name1" SortExpression="Name1" HeaderText="Name 1"></asp:BoundColumn>
														<asp:BoundColumn DataField="Name2" SortExpression="Name2" HeaderText="Name 2"></asp:BoundColumn>
														<asp:BoundColumn DataField="Auftragsnummer" SortExpression="Auftragsnummer" HeaderText="Auftragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Versand am" SortExpression="Versand am" HeaderText="Versand am" DataFormatString="{0:d}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Stueckzahl" SortExpression="Stueckzahl" HeaderText="St&#252;ckzahl"></asp:BoundColumn>
														<asp:TemplateColumn Visible="False" HeaderText="Kennzeichenliste">
															<ItemTemplate>
																<asp:LinkButton id="lnkKennzeichenliste" runat="server" CommandName="Kennzeichenliste">Kennzeichenliste</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Kennz. anzeigen" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-HorizontalAlign="Center">
															<HeaderTemplate>
                                                                <asp:LinkButton CssClass="StandardButtonTable" ID="btnAlleKennzAnzeigen" runat="server"
                                                                    CommandName="AlleKennzAnzeigen" Text="Alle Kennz. anzeigen"></asp:LinkButton><br />
                                                                <asp:LinkButton CssClass="StandardButtonTable" ID="btnKennzAnzeigen" runat="server"
                                                                    CommandName="KennzAnzeigen" Text="Kennz. anzeigen"></asp:LinkButton>
																
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox id="chkKennzAnzeigen" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Adresse anzeigen" HeaderStyle-Wrap="false" 
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
															<HeaderTemplate>
                                                                <asp:LinkButton CssClass="StandardButtonTable" ID="btnAlleAdressenAnzeigen" runat="server"
                                                                    CommandName="AlleAdressenAnzeigen" Text="Alle Adressen anzeigen"></asp:LinkButton><br />
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
