<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report44.aspx.vb" Inherits="AppFFE.Report44" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	    <style type="text/css">
            .style1
            {
                width: 832px;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<tr>
								<td></td>
								<td><asp:linkbutton id="cmdBack" runat="server" Visible="False" CssClass="StandardButton" Enabled="False">&#149;&nbsp;Zurück</asp:linkbutton></td>
							</tr>
							<tr>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" colSpan="2">
												&nbsp;</td>
										</tr>
										<TR>
											<TD align="right" colSpan="2"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:linkbutton></TD>
										<TR>
											<TD class="style1" colSpan="1"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
												&nbsp;
                                                            &nbsp;&nbsp;
												</TD>
										
											<TD class="LabelExtraLarge" align="right"><strong>
                                                            <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /> <asp:LinkButton
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>&nbsp; </strong>&nbsp;<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist>&nbsp;</TD>
										</TR>
										<TR>
											<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" 
                                                    AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" 
                                                    bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White" 
                                                    AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
													<asp:TemplateColumn SortExpression="UDATUM" HeaderText="col_Dateivom">
															<HeaderTemplate>
																<asp:LinkButton id="col_Dateivom" runat="server" CommandName="Sort" CommandArgument="UDATUM">col_Dateivom</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UDATUM", "{0:D}") %>' ID="Label1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ERDAT" HeaderText="col_Einspielungam">
															<HeaderTemplate>
																<asp:LinkButton id="col_Einspielungam" runat="server" CommandName="Sort" CommandArgument="ERDAT">col_Einspielungam</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ERDAT" , "{0:D}") %>' ID="Label2">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="UZEIT" HeaderText="col_Einspielungum">
															<HeaderTemplate>
																<asp:LinkButton id="col_Einspielungum" runat="server" CommandName="Sort" CommandArgument="UZEIT">col_Einspielungum</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UZEIT") %>' ID="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZDATENSAETZE" HeaderText="col_Datensaetze">
															<HeaderTemplate>
																<asp:LinkButton id="col_Datensaetze" runat="server" CommandName="Sort" CommandArgument="ZDATENSAETZE">col_Datensaetze</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZDATENSAETZE") %>' ID="Label4">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZABWEICHUNG" HeaderText="col_Abweichung">
															<HeaderTemplate>
																<asp:LinkButton id="col_Abweichung" runat="server" CommandName="Sort" CommandArgument="ZABWEICHUNG">col_Abweichung</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZABWEICHUNG") %>' ID="Label5">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZUMFINANZ" HeaderText="col_Umfinaziert">
															<HeaderTemplate>
																<asp:LinkButton id="col_Umfinaziert" runat="server" CommandName="Sort" CommandArgument="ZUMFINANZ">col_Umfinaziert</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZUMFINANZ") %>' ID="Label6">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZBEZAHLTE" HeaderText="col_Bezahlt">
															<HeaderTemplate>
																<asp:LinkButton id="col_Bezahlt" runat="server" CommandName="Sort" CommandArgument="ZBEZAHLTE">col_Bezahlt</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBEZAHLTE") %>' ID="Label7">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="IMPABWPRZ" HeaderText="col_Schwellwert">
															<HeaderTemplate>
																<asp:LinkButton id="col_Schwellwert" runat="server" CommandArgument="IMPABWPRZ" CommandName="Sort">col_Schwellwert</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IMPABWPRZ") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														<asp:TemplateColumn SortExpression="ERFOLG" HeaderText="col_Erfolg">
															<HeaderTemplate>
																<asp:LinkButton id="col_Erfolg" runat="server" CommandArgument="ERFOLG" CommandName="Sort">col_Erfolg</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label9" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ERFOLG") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>													
														</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:label id="lblInfo" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
							</tr>
							<tr>
								<td></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" -->
									<asp:Label id="lblHidden" runat="server" Visible="False" Width="39px"></asp:Label></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
