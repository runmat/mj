<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdatenhaendler.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report05.aspx.vb" Inherits="AppFFE.Report05" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style1
            {
                width: 753px;
            }
        </style>
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
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton">&#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">
												            <asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
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
														<td class="LabelExtraLarge" align="left"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
														<td align="right">
												            <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /> <asp:LinkButton id="lnkCreateExcel" CssClass="ExcelButton" runat="server" Visible="False"><strong>Excelformat</strong> </asp:LinkButton>
												            &nbsp;<strong>Anzahl Vorgänge / Seite</strong> <asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</TD>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="Auftragsnummer" HeaderText="col_Auftragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Auftragsnummer" runat="server" CommandName="Sort" CommandArgument="Auftragsnummer">col_Auftragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auftragsnummer") %>' ID="lblAuftragsnummer">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Vertragsnummer" CommandName="sort" CommandArgument="Vertragsnummer" Runat="server">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>' ID="lblVertragsnummer">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Angefordert am" HeaderText="col_Angefordertam">
															<HeaderTemplate>
																<asp:LinkButton id="col_Angefordertam" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Angefordertam</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Angefordert am", "{0:d}" ) %>' ID="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Briefnummer" HeaderText="col_Briefnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Briefnummer" CommandName="sort" CommandArgument="Briefnummer" Runat="server">col_Briefnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer")%>' ID="Label5">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Kontingentart" HeaderText="col_Kontingentart">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kontingentart" CommandName="sort" CommandArgument="Kontingentart" Runat="server">col_Kontingentart</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart")%>' ID="Label6">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Gesperrt" HeaderText="col_Gesperrt">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_Gesperrt" runat="server" CommandName="Sort" CommandArgument="Gesperrt">col_Gesperrt</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt") %>' ID="Label1">
																                                   
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:ButtonColumn Text="Storno" CommandName="Storno"></asp:ButtonColumn>
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
										var Check = window.confirm("Wollen Sie dieses Dokument wirklich stornieren?\n\tFinanzierungsnummer: " + Vertragsnummer + "\n\tAngefordert am:\t" + AngefordertAm + "\n\tFahrgestellnr.:\t" + Fahrgestellnummer + "\t\n\tZBII-Nummer:\t" + Briefnummer + "\t\n\tKontingentart:\t" + Kontingentart);
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
								<td>&nbsp;</td>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
