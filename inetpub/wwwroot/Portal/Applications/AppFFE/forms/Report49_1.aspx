<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report49_1.aspx.vb" Inherits="AppFFE.Report49_1" %>
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
											<TD class="TaskTitle" vAlign="top">
                                                <asp:HyperLink ID="lnkBack" runat="server" 
                                                    NavigateUrl="javascript:history.back()">zurück</asp:HyperLink>
                                            </TD>
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
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>&nbsp;Anzahl 
                                                            Vorgänge / Seite </strong><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
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
												<asp:BoundColumn DataField="Händlernummer" SortExpression="Händlernummer" HeaderText="Händlernummer"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Finanzierungsnummer" SortExpression="Finanzierungsnummer" HeaderText="Finanzierungsnummer">
                                                    </asp:BoundColumn>
												<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Fahrgestellnummer" CommandName="sort" CommandArgument="Fahrgestellnummer" Runat="server">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
														</asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ZBII-Nummer" SortExpression="ZBII-Nummer" HeaderText="ZBII-Nummer">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Ort" SortExpression="Ort" HeaderText="Ort"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Versanddatum" SortExpression="Versanddatum" HeaderText="Versanddatum"></asp:BoundColumn>
                                                    <asp:BoundColumn DataFormatString="{0:d}" DataField="Versanddatum" SortExpression="Versanddatum" HeaderText="Versanddatum">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Abrufgrund" SortExpression="Abrufgrund"
                                                        HeaderText="Abrufgrund"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Anforderungsdatum" SortExpression="Anforderungsdatum"
                                                        HeaderText="Anforderungsdatum" DataFormatString="{0:d}">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Statusänderung" SortExpression="Statusänderung" HeaderText="Statusänderung"
                                                        DataFormatString="{0:d}"></asp:BoundColumn>
                                                                                                        
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
