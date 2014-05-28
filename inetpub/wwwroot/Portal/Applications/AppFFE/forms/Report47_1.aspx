<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report47_1.aspx.vb" Inherits="AppFFE.Report47_1" %>
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
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
										<tr>
											<td></td>
										</tr>
										<TR>
											<td>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<TD class="LabelExtraLarge">
                                                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;</TD>
														<td align="right"></td>
													</tr>
													<TR>
														<TD class="LabelExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
														<td align="right"><strong>
                                                            <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /> <asp:LinkButton
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>&nbsp; </strong><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
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
												<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Fahrgestellnummer" CommandName="sort" CommandArgument="Fahrgestellnummer" Runat="server">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label1">
																	<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer")%>
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
														
														<asp:TemplateColumn SortExpression="Finanzierungsnummer" HeaderText="col_Finanzierungsnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Finanzierungsnummer" CommandName="sort" CommandArgument="Finanzierungsnummer" Runat="server">col_Finanzierungsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label4">
																	<%#DataBinder.Eval(Container, "DataItem.Finanzierungsnummer")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn SortExpression="Laufend" HeaderText="col_Laufend">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Laufend" CommandName="sort" CommandArgument="Laufend" Runat="server">col_Laufend</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label5">
																	<%#DataBinder.Eval(Container, "DataItem.Laufend")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn SortExpression="DatAufb" HeaderText="col_DatAufb">
															<HeaderTemplate>
																<asp:LinkButton ID="col_DatAufb" CommandName="sort" CommandArgument="DatAufb" Runat="server">col_DatAufb</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label60">
																	<%#DataBinder.Eval(Container, "DataItem.DatAufb","{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
															
														
												
												<asp:TemplateColumn SortExpression="Ordernummer" HeaderText="col_Ordernummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Ordernummer" CommandName="sort" CommandArgument="Ordernummer" Runat="server">col_Ordernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label2">
																	<%# DataBinder.Eval(Container, "DataItem.Ordernummer")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														
														
														<asp:TemplateColumn SortExpression="NummerZBII" HeaderText="col_NummerZBII">
															<HeaderTemplate>
																<asp:LinkButton ID="col_NummerZBII" CommandName="sort" CommandArgument="NummerZBII" Runat="server">col_NummerZBII</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label71">
																	<%#DataBinder.Eval(Container, "DataItem.NummerZBII")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														
														<asp:TemplateColumn SortExpression="Haendlernummer" HeaderText="col_Haendlernummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Haendlernummer" CommandName="sort" CommandArgument="Haendlernummer" Runat="server">col_Haendlernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label7">
																	<%#DataBinder.Eval(Container, "DataItem.Haendlernummer")%>
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
