<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdatenhaendler.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report24_2.aspx.vb" Inherits="AppFFE.Report24_2"%>
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
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Report23.aspx">Händlersuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkVertragssuche" runat="server" CssClass="TaskTitle" NavigateUrl="Report23_2.aspx">Vertragssuche</asp:hyperlink></TD>
										</TR>
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<td>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<TD> <asp:hyperlink id="lnkExcel" runat="server" Target="_blank" Visible="False">Excelformat</asp:hyperlink>&nbsp;&nbsp;
															<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
														<td></td>
													</tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label><asp:Label ID="lblError"
                                                                runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td align="right" >
                                                            <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />
                                                            <asp:LinkButton ID="lnkCreateExcel" CssClass="ExcelButton" runat="server" Visible="False"><strong>Excelformat</strong> </asp:LinkButton>
                                                            &nbsp;
                                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
												</table>
											</td>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Vertragsnummer" CommandName="sort" CommandArgument="Vertragsnummer" Runat="server">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="lblVertragsnummer">
																	<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton Runat="server" CommandName="sort" CommandArgument="Fahrgestellnummer" ID="col_Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:HyperLink id="VIN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' ToolTip="Anzeige Fahrzeughistorie" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' Target="_blank">
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Ordernummer" HeaderText="col_Ordernummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Ordernummer" CommandName="sort" CommandArgument="Ordernummer" Runat="server">col_Ordernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label1">
																	<%# DataBinder.Eval(Container, "DataItem.Ordernummer")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="COC" HeaderText="col_COC">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton ID="col_COC" CommandName="sort" CommandArgument="COC" Runat="server">col_COC</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label4">
																	<%# DataBinder.Eval(Container, "DataItem.COC")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
										                <asp:TemplateColumn SortExpression="Erfassung Fahrzeug" HeaderText="col_ErfassungFahrzeug">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton ID="col_ErfassungFahrzeug" CommandName="sort" CommandArgument="Erfassung Fahrzeug" Runat="server">col_ErfassungFahrzeug</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label4">
																	<%# DataBinder.Eval(Container, "DataItem.Erfassung Fahrzeug", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>														
										                <asp:TemplateColumn SortExpression="Angefordert am" HeaderText="col_Angefordertam">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton ID="col_Angefordertam" CommandName="sort" CommandArgument="Angefordert am" Runat="server">col_Angefordertam</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label4">
																	<%# DataBinder.Eval(Container, "DataItem.Angefordert am", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
										                <asp:TemplateColumn SortExpression="Angefordert um" HeaderText="col_Angefordertum">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton ID="col_Angefordertum" CommandName="sort" CommandArgument="Angefordert um" Runat="server">col_Angefordertum</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label4">
																	<%# DataBinder.Eval(Container, "DataItem.Angefordert um", "{0:hh:mm:ss}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														<asp:TemplateColumn SortExpression="Kontingentart" HeaderText="col_Kontingentart">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kontingentart" CommandName="sort" CommandArgument="Kontingentart" Runat="server">col_Kontingentart</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart")%>' ID="Label2">
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
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt") %>' ID="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Freigegeben am" HeaderText="col_Freigegebenam">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton ID="col_Freigegebenam" CommandName="sort" CommandArgument="Freigegeben am" Runat="server">col_Freigegebenam</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label5">
																	<%# DataBinder.Eval(Container, "DataItem.Freigegeben am", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Freigegeben um" HeaderText="col_Freigegebenum">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton ID="col_Freigegebenum" CommandName="sort" CommandArgument="Freigegeben um" Runat="server">col_Freigegebenum</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label6">
																	<%# DataBinder.Eval(Container, "DataItem.Freigegeben um", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Versendet am" HeaderText="col_Versendetam">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton ID="col_Versendetam" CommandName="sort" CommandArgument="Versendet am" Runat="server">col_Versendetam</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label7">
																	<%# DataBinder.Eval(Container, "DataItem.Versendet am", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Status">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_Status" Runat="server"></asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<P align="center">
																	<asp:HyperLink id="Status" runat="server" Visible="False" Text='Änderung Status' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' Target="_blank">
																	</asp:HyperLink>
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
