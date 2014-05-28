<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report04_1.aspx.vb" Inherits="AppF1.Report04_1" %>
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
											<TD class="TaskTitle" vAlign="top" nowrap="nowrap">
                                                <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>&nbsp;  <asp:Label ID="lblAuswahl" runat="server" Font-Bold="True" Font-Size="8pt"></asp:Label>
                                            </TD>
										</TR>
										<tr>
											<td></td>
										</tr>
										<TR>
											<td>
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="LabelExtraLarge">
                                                                                                                        &nbsp;
                                                            &nbsp;&nbsp;
                                                        </td>
                                                        <td align="right">
                                                        </td>
                                                    </tr>
													<TR>
														<TD class="LabelExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
														<td align="right">
														<strong>
                                                            <img alt="" src="../../../images/excel.gif" style="height: 17px" />
                                                            <asp:LinkButton ID="lnkCreateExcel" CssClass="ExcelButton" runat="server" Visible="False">Excelformat</asp:LinkButton></strong>&nbsp;Ergebnisse/Seite:&nbsp;<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													
													</TR>
												</table>
											</td>
										</TR>
										<TR>
											<TD>
											<asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowSorting="True" 
                                                    AllowPaging="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" 
                                                    headerCSS="tableHeader" PageSize="50" BackColor="White" 
                                                    AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" CssClass="TextExtraLarge" Wrap="False" Mode="NumericPages"></PagerStyle>
												<Columns>
												
												<asp:TemplateColumn SortExpression="EQUNR" HeaderText="col_EQUNR" Visible="false">
															<HeaderTemplate>
																<asp:LinkButton ID="col_EQUNR" CommandName="sort" CommandArgument="EQUNR" Runat="server">col_EQUNR</asp:LinkButton>
															</HeaderTemplate >
															<ItemTemplate>
																<asp:Label Visible="false" Runat="server" ID="Label1">
																	<%#DataBinder.Eval(Container, "DataItem.EQUNR")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Fahrgestellnummer" CommandName="sort" CommandArgument="Fahrgestellnummer" Runat="server">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:LinkButton ToolTip="zur Fahrzeughistorie" Visible="True"  Runat="server"   Text='<%#DataBinder.Eval(Container, "DataItem.Fahrgestellnummer")%>' CommandArgument='<%#DataBinder.Eval(Container, "DataItem.Fahrgestellnummer")%>' CommandName="Fahrzeughistorie"  ID="lbFahrgestellnummer">
																</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>	
														<asp:TemplateColumn SortExpression="Finanzierungsart" HeaderText="col_Finanzierungsart">
															<HeaderTemplate>
																<asp:LinkButton  ID="col_Finanzierungsart"  CommandName="sort" CommandArgument="Finanzierungsart" Runat="server">col_Finanzierungsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label4">
																	<%#DataBinder.Eval(Container, "DataItem.Finanzierungsart")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>														
															<asp:TemplateColumn SortExpression="Haendlernummer" HeaderText="col_Haendlernummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Haendlernummer" CommandName="sort" CommandArgument="Haendlernummer" Runat="server">col_Haendlernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server"  ID="Label7">
																	<%#DataBinder.Eval(Container, "DataItem.Haendlernummer")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
												        <asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_LIZNR">
															<HeaderTemplate>
																<asp:LinkButton ID="col_LIZNR" CommandName="sort" CommandArgument="LIZNR" Runat="server">col_LIZNR</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label2">
																	<%#DataBinder.Eval(Container, "DataItem.LIZNR")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
																												
																												
														<asp:TemplateColumn SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kennzeichen" CommandName="sort" CommandArgument="Kennzeichen" Runat="server">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label3">
																	<%#DataBinder.Eval(Container, "DataItem.Kennzeichen")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
												
														<asp:TemplateColumn SortExpression="DatErsetzung" HeaderText="col_DatErsetzung">
															<HeaderTemplate>
																<asp:LinkButton ID="col_DatErsetzung" CommandName="sort" 
                                                                    CommandArgument="DatErsetzung" Runat="server">col_DatErsetzung</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label5">
																	<%#DataBinder.Eval(Container, "DataItem.DatErsetzung", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
																						
														<asp:TemplateColumn SortExpression="Gueltigkeitsende" HeaderText="col_Gueltigkeitsende">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Gueltigkeitsende" CommandName="sort" 
                                                                    CommandArgument="Gueltigkeitsende" Runat="server">col_Gueltigkeitsende</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label69">
																	<%#DataBinder.Eval(Container, "DataItem.Gueltigkeitsende", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
							
														</Columns>											
												</asp:datagrid>
										   </TD>
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
			</table>
		</form>
	</body>
</HTML>
