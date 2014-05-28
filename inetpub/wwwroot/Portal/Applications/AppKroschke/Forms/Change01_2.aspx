<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01_2.aspx.vb" Inherits="AppKroschke.Change01_2" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Vorgangsanzeige)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdConfirm" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdReset" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Zurück</asp:linkbutton>&nbsp;</TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" 
                                                    runat="server" CssClass="TaskTitle" NavigateUrl="Change01.aspx">Vorgangssuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3" height="18"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
										</tr>
										<TR>
											<TD class="LabelExtraLarge" colSpan="3"><asp:label id="lblNoData" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
															<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></td>
														<td align="right"><asp:dropdownlist id="ddlPageSizeNotInUse" runat="server" Visible="False" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3"><!-- headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain"-->
												<table cellSpacing="0" cellPadding="0" border="0" width="650">
													<tr>
														<td><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" 
                                                                PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" 
                                                                bodyHeight="300" BackColor="White" Width="100%">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="Verfdat" SortExpression="Verfdat" HeaderText="Datum" DataFormatString="{0:ddd dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="Anzahl" HeaderText="U = Urlaub, K = Krank, I = eingeschränkt verfügbar, 0 = nicht verfügbar ohne Grund, > 0 = verfügbar mit n Fahrern">
																		<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																		    <asp:TextBox ID="txtAnzahl" runat="server" Width="25px" MaxLength="2" Text='<%# DataBinder.Eval(Container, "DataItem.ANZ_FAHRER") %>' Enabled='<%# (NOT DataBinder.Eval(Container, "DataItem.Verfdat")<CDate(Now.ToShortDateString)) %>'></asp:TextBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
															</asp:datagrid></td>
													</tr>
													<tr>
														<td align="right"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Width="150px"><center>Änderungen speichern</center></asp:linkbutton></td>
													</tr>
												</table>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td>&nbsp;</td>
								<td></td>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
