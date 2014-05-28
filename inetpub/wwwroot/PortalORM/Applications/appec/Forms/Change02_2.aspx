<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change02_2.aspx.vb" Inherits="AppEC.Change02_2" %>
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
			<input id="txtOrtsKzOld" type="hidden" name="txtOrtsKzOld" runat="server"> <INPUT id="txtFree2" type="hidden" name="txtFree2" runat="server">
			<table width="100%" align="center">
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(Anzeige)</td>
								</TR>
								<tr>
									<TD class="" vAlign="top">
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top" align="left">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right">&nbsp;</TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge">
													<TABLE id="Table9" cellSpacing="1" cellPadding="1" width="100%" border="0">
														<TR>
															<TD vAlign="top" align="left" colSpan="2"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
														</TR>
														<TR>
															<TD vAlign="top" align="left" colSpan="2"><asp:label id="lblTableTitle" runat="server" Font-Bold="True"> Anzahl Datensätze:&nbsp;</asp:label><asp:label id="lblAnzahl" runat="server" EnableViewState="False" Font-Bold="True"></asp:label></TD>
														</TR>
													</TABLE>
													<asp:datagrid id="dataGrid" runat="server" CssClass="tableMain" CellPadding="1" AllowPaging="True" BackColor="White" Width="99%" AutoGenerateColumns="False" AllowSorting="True" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" bodyHeight="300">
														<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
														<HeaderStyle CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
														<Columns>
															<asp:BoundColumn DataField="UnitNR" SortExpression="UnitNR" HeaderText="Unit Nummer"></asp:BoundColumn>
															<%--<asp:BoundColumn Visible="False" DataField="UnitNrPruefziffer" SortExpression="UnitNrPruefziffer" HeaderText="UNPZ"></asp:BoundColumn>--%>
															<asp:TemplateColumn HeaderText="gesperrt">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
																<ItemTemplate>
																	<asp:CheckBox id="cbxDelete" runat="server"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Sperr Vermerk">
																<ItemTemplate>
																	<asp:TextBox runat="server" Width="120" maxlength="60" tooltip='<%# DataBinder.Eval(Container, "DataItem.Sperrdatum") + ", " + DataBinder.Eval(Container, "DataItem.Sperrbenutzer")%> ' Text='<%# DataBinder.Eval(Container, "DataItem.Sperrbemerkung") %>' ID="txtSperrVermerk">
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn Visible="False" DataField="RowId" SortExpression="RowId" HeaderText="Lfd.Nr."></asp:BoundColumn>
															<asp:BoundColumn DataField="ModelID" SortExpression="ModelID" HeaderText="ModelID"></asp:BoundColumn>
															<asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell"></asp:BoundColumn>
															<asp:BoundColumn DataField="BatchID" SortExpression="BatchID" HeaderText="BatchID"></asp:BoundColumn>
															<asp:BoundColumn DataField="Verwendungszweck" SortExpression="Verwendungszweck" HeaderText="Verwendungszweck"></asp:BoundColumn>
															<asp:BoundColumn DataField="AuftragsNrVonBis" SortExpression="AuftragsNrVonBis" HeaderText="AuftragsNr"></asp:BoundColumn>
															<asp:BoundColumn DataField="Anzahl" SortExpression="Anzahl" HeaderText="Anzahl"></asp:BoundColumn>
															<asp:BoundColumn DataField="Einsteuerung" SortExpression="Einsteuerung" HeaderText="Einsteuerung"></asp:BoundColumn>
															<asp:BoundColumn DataField="Leasing" SortExpression="Leasing" HeaderText="Leasing"></asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Bemerkung">
																<ItemTemplate>
																	<asp:TextBox runat="server" Width="120" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>' ID="txtBemerkung">
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn DataField="Sachbearbeiter" SortExpression="Sachbearbeiter" HeaderText="Erfasser">
																<ItemStyle Width="100px"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="EQUNR" SortExpression="EQUNR" HeaderText="EQUNR"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="Loesch" SortExpression="Loesch" HeaderText="LV"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="Laufzeitbindung" SortExpression="Laufzeitbindung" HeaderText="Laufzeitbindung"></asp:BoundColumn>
														</Columns>
														<PagerStyle Mode="NumericPages"></PagerStyle>
													</asp:datagrid></TD>
											</TR>
											<TR id="trSumme" runat="server">
												<TD class="LabelExtraLarge">
													<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
														<TR>
															<TD noWrap align="right">
																<P align="left"><asp:linkbutton id="Linkbutton1" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></P>
															</TD>
															<TD noWrap align="right" width="100%">&nbsp;
																<asp:linkbutton id="btnSaveSAP" runat="server" CssClass="StandardButton"> &#149;&nbsp;Liste abschicken!</asp:linkbutton></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
										<P align="left">&nbsp;</P>
									</TD>
								</tr>
							</TABLE>
		</form>
		</TD></TR></TBODY></TABLE>
	</body>
</HTML>
