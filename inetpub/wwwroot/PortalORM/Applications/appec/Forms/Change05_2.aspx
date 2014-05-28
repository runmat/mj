<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change05_2.aspx.vb" Inherits="AppEC.Change05_2" %>
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
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(Bestätigen)</td>
								</TR>
								<tr>
									<TD class="" vAlign="top">
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top" align="left">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right" colSpan="2" height="11">
													<P align="left">Sie haben folgende Fahrzeuge zum&nbsp;
														<asp:label id="lblTask" runat="server" CssClass="TaskName"></asp:label>&nbsp;ausgewählt. 
														Bestätigen Sie den Auftrag durch Klick auf "Absenden!".</P>
												</TD>
											</TR>
											<TR>
												<TD align="right" colSpan="2" height="11">
													<P align="left"><asp:hyperlink id="lnkExcel" runat="server" CssClass="" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
														<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></P>
												</TD>
											</TR>
											<TR>
												<TD align="right" colSpan="2">
													<P align="left"><asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></P>
												</TD>
											</TR>
											<TR>
												<TD class="PageNavigation" align="right">
													<P align="left">Gesamtübersicht</P>
												</TD>
												<TD class="PageNavigation" align="right">
													<P align="center">Fahrzeugübersicht</P>
												</TD>
											</TR>
											<TR id="trSumme" runat="server">
												<TD vAlign="top">
													<P align="left"><asp:listbox id="lstSumme" runat="server" CssClass="DropDownStyle" Height="300px"></asp:listbox></P>
												</TD>
												<TD vAlign="top">
													<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
														<TR>
															<TD align="left">
																<P align="left"><asp:datagrid id="DataGrid1" runat="server" CssClass="tableMain" BackColor="White" headerCSS="tableHeader" bodyCSS="tableBody" Width="100%" AutoGenerateColumns="False" bodyHeight="400">
																		<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																		<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
																		<Columns>
																			<asp:BoundColumn Visible="False" DataField="RowId" SortExpression="RowId" HeaderText="ID"></asp:BoundColumn>
																			<asp:BoundColumn Visible="False" DataField="Art" SortExpression="Art" HeaderText="Art"></asp:BoundColumn>
																			<asp:TemplateColumn HeaderText="PDI">
																				<ItemTemplate>
																					<asp:Literal id=Literal1 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.RowID") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.KUNPDI") &amp; "</a>" %>'>
																					</asp:Literal>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn DataField="ZZBEZEI" SortExpression="ZZBEZEI" HeaderText="Modell"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZDAT_EIN" SortExpression="ZZDAT_EIN" HeaderText="Eingangsdatum"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZFARBE" SortExpression="ZZFARBE" HeaderText="Farbcode"></asp:BoundColumn>
																			<asp:BoundColumn DataField="SelectedDate" SortExpression="SelectedDate" HeaderText="Zul.Datum"></asp:BoundColumn>
																			<asp:BoundColumn DataField="SelectedKennzeichenserieText" SortExpression="SelectedKennzeichenserieText" HeaderText="Kennz.Serie"></asp:BoundColumn>
																			<asp:BoundColumn DataField="Bemerkung" SortExpression="Bemerkung" HeaderText="Bemerkungen"></asp:BoundColumn>
																			<asp:BoundColumn DataField="SelectedZielPDIText" SortExpression="SelectedZielPDIText" HeaderText="Ziel-PDI"></asp:BoundColumn>
																			<asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
																		</Columns>
																	</asp:datagrid></P>
															</TD>
														</TR>
													</TABLE>
													<P align="right">
														<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
															<TR>
																<TD width="100%">
																	<asp:linkbutton id="btnBack" runat="server" Visible="False" CssClass="StandardButtonTable" Width="150px">&#149;&nbsp;Zurück</asp:linkbutton></TD>
																<TD>
																	<asp:linkbutton id="btnConfirm" runat="server" CssClass="StandardButtonTable" Width="150px">&#149;&nbsp;Absenden!</asp:linkbutton></TD>
															</TR>
														</TABLE>
													</P>
												</TD>
											</TR>
										</TABLE>
										<P align="right">&nbsp;</P>
									</TD>
								</tr>
							</TABLE>
		</form>
		</TD></TR></TBODY>
		<SCRIPT language="JavaScript">										
							<!--	
								function WriteID(RowID)
									{
									window.document.Form1.txtRowID.value = RowID;
									}												
							-->
		</SCRIPT>
		</TABLE>
	</body>
</HTML>
