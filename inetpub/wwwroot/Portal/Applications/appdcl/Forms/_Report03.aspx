<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report03.aspx.vb" Inherits="AppDCL._Report03" %>

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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td height="25"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Abfragekriterien)</asp:label><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="PageNavigation" NavigateUrl="Equipment.aspx" Visible="False">Abfragekriterien</asp:hyperlink></td>
							</TR>
							<tr>
								<TD class="StandardTableButtonFrame" vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;<asp:label id="lblDatensatz" runat="server" Visible="False" Font-Bold="True"></asp:label>
												<asp:label id="lblNoData" runat="server" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2"><TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0" runat="server">
																<TBODY>
																	<TR>
																		<TD vAlign="top" align="left" width="100%" bgColor="#ffffff">
																			<TABLE class="TableBackGround" id="Table4" borderColor="#cccccc" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffffff" border="0">
																				<TR>
																					<TD class="TableBackGround" id="td0" vAlign="middle" noWrap align="left" bgColor="#ffffff" rowSpan="1" runat="server"><STRONG>
																							<P align="center">Dateien (Web-Verzeichnis)</P>
																						</STRONG>
																					</TD>
																				</TR>
																				<TR>
																					<TD vAlign="top" align="center" width="100%" bgColor="#ffffff" colSpan="1" rowSpan="1"><asp:datagrid id="gridServer" runat="server" width="1050px" BackColor="White" BorderColor="Transparent" bodyHeight="350" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" AllowSorting="True" AutoGenerateColumns="False">
																							<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																							<HeaderStyle HorizontalAlign="Left" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																							<Columns>
																								<asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad" HeaderText="Serverpfad"></asp:BoundColumn>
																								<asp:BoundColumn Visible="False" DataField="Zeit" SortExpression="Zeit" HeaderText="Erstellt am"></asp:BoundColumn>
																								<asp:TemplateColumn Visible="False" SortExpression="Filename" HeaderText="Dateiname">
																									<ItemTemplate>
																										<asp:HyperLink id=lnkFile runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>' BorderColor="Transparent" Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>'>
																										</asp:HyperLink>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																								<asp:TemplateColumn HeaderText="Protokolle">
																									<ItemTemplate>
																										<TABLE id="Table12" height="0" cellSpacing="1" cellPadding="1" width="100%" border="0">
																											<TR>
																											<TR>
																												<TD>&nbsp;</TD>
																												<TD></TD>
																											</TR>
																											<TR>
																												<TD>
																													<asp:HyperLink id=Hyperlink5 runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>' ImageUrl="../../../images/pdf.gif" Height="10px" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' Target="_blank" Width="23px">HyperLink</asp:HyperLink>
																													<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>' Height="11px" Target="_blank" Width="107px" ForeColor="Blue" Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>'>
																													</asp:HyperLink></TD>
																												<TD></TD>
																											</TR>
																										</TABLE>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																								<asp:BoundColumn Visible="False" DataField="Filename" SortExpression="Filename" HeaderText="Dateiname"></asp:BoundColumn>
																								<asp:TemplateColumn HeaderText="Auftrag suchen und zuordnen">
																									<ItemTemplate>
																										<TABLE id="Table11" height="0" cellSpacing="1" cellPadding="1" width="100%" border="0">
																											<TR>
																											<TR>
																												<TD>
																													<asp:Label id="Label1" runat="server" ToolTip="Referenznummer (siehe Protokoll)">Referenz</asp:Label>
																													<asp:Label id="lblDummyForScrollHere" runat="server"></asp:Label></TD>
																												<TD>
																													<asp:Label id="Label2" runat="server" ToolTip="Auftragsnummer (Auto-Eintrag)">Auftrag-Nr.</asp:Label></TD>
																												<TD>
																													<asp:Label id="lblAbgabedatum" runat="server" Visible="False" Font-Bold="True" ToolTip="Format: TT.MM.JJJJ">Abgabedatum</asp:Label></TD>
																												<TD>
																													<asp:Label id="lblKategorie" runat="server" Visible="False" Font-Bold="True" ToolTip="Kategorie">Kategorie</asp:Label></TD>
																												<TD></TD>
																												<TD></TD>
																												<TD></TD>
																											</TR>
																											<TR>
																												<TD>
																													<asp:TextBox id="txtReferenz" runat="server" BackColor="White" Width="100px"></asp:TextBox></TD>
																												<TD>
																													<asp:TextBox id="txtAuftrag" runat="server" BackColor="White" Width="100px"></asp:TextBox></TD>
																												<TD>
																													<asp:TextBox id="txtAbgabedatum" runat="server" Visible="False" BackColor="#80FF80" Width="100px"></asp:TextBox></TD>
																												<TD>
																													<asp:DropDownList id="drpKategorie" runat="server" Visible="False"></asp:DropDownList>
																													<asp:TextBox id="txtKategorie" runat="server" Visible="False" Width="105px" Enabled="False"></asp:TextBox></TD>
																												<TD noWrap><!--<asp:LinkButton id="btnSuche" runat="server" CssClass="StandardButtonTable" ToolTip="Auftrags-/Referenznummer aus SAP lesen" CommandName="searchSAP">Suchen...</asp:LinkButton>-->
																													<asp:ImageButton id="ibtnNoSearch" runat="server" ImageUrl="../../../Images/empty.gif" CommandName="noSearch" CausesValidation="false"></asp:ImageButton>
																													<asp:Button id="ibtnSearch" runat="server" ToolTip="Auftrags-/Referenznummer aus SAP lesen" Text="Suchen..." CommandName="searchSAP" CausesValidation="false" BorderStyle="NotSet" Font-Underline="false"></asp:Button>
																													<asp:LinkButton id="btnKategorie" runat="server" Visible="False" CssClass="StandardButtonTable" CommandName="KategorieZuordnen">Kategorie zuordnen...</asp:LinkButton>
																												<TD noWrap>
																													<asp:LinkButton id="btnAssignOrder" runat="server" Visible="False" CssClass="StandardButtonTable" ToolTip="Auftrags-/Referenznummer zuordnen" CommandName="assignOrder">Zuordnen</asp:LinkButton></TD>
																												<TD noWrap width="100%">
																													<asp:LinkButton id="btnReAssignOrder" runat="server" Visible="False" CssClass="StandardButtonTable" ToolTip="Auftrags-/Referenznummer Zuordnung aufheben" CommandName="unassignOrder">Zuordnung aufheben</asp:LinkButton></TD>
																											</TR>
																										</TABLE>
																										<TABLE id="Table15" cellSpacing="1" cellPadding="1" border="0">
																											<TR>
																												<TD>
																													<asp:DropDownList id="ddlOrders" runat="server" Visible="False" DataValueField="id" DataTextField="value"></asp:DropDownList></TD>
																											</TR>
																										</TABLE>
																										<asp:Label id="lblInfo" runat="server" Visible="False"></asp:Label>
																										<asp:Label id="lblData" runat="server" Visible="False"></asp:Label>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																								<asp:TemplateColumn HeaderText="Archivieren">
																									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																									<ItemTemplate>
																										<P align="center">
																											<asp:CheckBox id="cbxArchiv" runat="server" ToolTip="Dokument zum Archivieren markieren" Enabled="False"></asp:CheckBox></P>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																								<asp:TemplateColumn HeaderText="l&#246;schen">
																									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																									<ItemStyle HorizontalAlign="Center"></ItemStyle>
																									<ItemTemplate>
																										<asp:ImageButton id="ibtnSRDelete" runat="server" CommandName="Delete" CausesValidation="false" ToolTip="Zeile löschen" ImageUrl="../../../Images/loesch.gif"></asp:ImageButton>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																								<asp:BoundColumn Visible="False" DataField="Status" HeaderText="Status">
																									<ItemStyle Font-Size="XX-Small"></ItemStyle>
																								</asp:BoundColumn>
																								<asp:BoundColumn Visible="False" DataField="FilenameNew" SortExpression="FilenameNew" HeaderText="DateinameNeu"></asp:BoundColumn>
																								<asp:BoundColumn Visible="False" DataField="Save" SortExpression="Save" HeaderText="Speichern"></asp:BoundColumn>
																								<asp:BoundColumn Visible="False" DataField="Auftrag" SortExpression="Auftrag" HeaderText="Auftrag"></asp:BoundColumn>
																								<asp:BoundColumn Visible="False" DataField="Tour" SortExpression="Tour" HeaderText="Tour"></asp:BoundColumn>
																							</Columns>
																							<PagerStyle Mode="NumericPages"></PagerStyle>
																						</asp:datagrid></TD>
																				</TR>
																				<TR>
																					<TD id="td3" vAlign="top" noWrap align="right" bgColor="#ffffff" runat="server"><asp:linkbutton id="btnBack" runat="server" CssClass="StandardButtonTable" Visible="False">&#149;&nbsp;Zurück</asp:linkbutton>&nbsp;<asp:linkbutton id="Linkbutton1" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Weiter zur Auftragsbestätigung</asp:linkbutton>&nbsp;<asp:linkbutton id="btnUpload" runat="server" CssClass="StandardButtonTable" Visible="False">&#149;&nbsp;Protokoll(e) archivieren!</asp:linkbutton></TD>
																				</TR>
																			</TABLE>
																		</TD>
																	</TR>
																</TBODY></TABLE>
														</TD>
													</TR>
												</TABLE>
												<asp:label id="lblError" runat="server" CssClass="TextError"></asp:label><asp:label id="lblOpen" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>