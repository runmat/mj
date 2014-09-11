<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report16.aspx.vb" Inherits="AppSIXT.Report16" %>
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
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label>&nbsp;
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdDetails" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Detailanzeige</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink><asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">&nbsp;rechte Maustaste => Ziel speichern unter...</asp:label>&nbsp;</TD>
										</TR>
										<TR>
											<TD class="" vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="center">Briefnummer:
														</TD>
														<TD class="TextLarge" vAlign="center" width="100%"><asp:textbox id="txtBriefnummer" runat="server"></asp:textbox><asp:textbox id="txtEingangsdatumVon" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtEingangsdatumBis" runat="server" Visible="False"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="center">Fahrgestellnummer:</TD>
														<TD class="StandardTableAlternate" vAlign="center"><asp:textbox id="txtFahrgestellnummer" runat="server"></asp:textbox></TD>
													</TR>
													<TR id="rowHaendlerID" runat="server">
														<TD class="TextLarge" vAlign="center">Händler ID:</TD>
														<TD class="TextLarge" vAlign="center"><asp:textbox id="txtHaendlerID" runat="server"></asp:textbox></TD>
													</TR>
													<TR id="rowResultate" runat="server">
														<TD class="TextLarge" vAlign="top">Resultate:</TD>
														<TD class="TextLarge" vAlign="center"><asp:label id="lblResults" runat="server"></asp:label>&nbsp;&nbsp;
															<BR>
															<asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Fahrzeuge" SortExpression="Fahrzeuge" HeaderText="Fahrzeuge"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" CssClass="TextExtraLarge" Wrap="False"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
												<BR>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
