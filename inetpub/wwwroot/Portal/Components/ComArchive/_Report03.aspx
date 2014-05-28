<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report03.aspx.vb" Inherits="CKG.Components.ComArchive._Report03" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
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
			<table width="100%" align="center" cellSpacing="0" cellPadding="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td colSpan="1">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" height="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Abfragekriterien)</asp:label><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="PageNavigation" NavigateUrl="Equipment.aspx" Visible="False">Abfragekriterien</asp:hyperlink></td>
							</TR>
							<tr>
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
																					<TD bgColor="#ffffff" rowSpan="1"><STRONG>Archivtypen&nbsp;</STRONG></TD>
																					<TD id="tdSearch" bgColor="#ffffff" runat="server"><asp:radiobuttonlist id="rblTypes" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"></asp:radiobuttonlist></TD>
																				</TR>
																				<TR>
																					<TD bgColor="#ffffff"><STRONG>Verfügbare&nbsp;Archive</STRONG></TD>
																					<TD bgColor="#ffffff"><asp:checkboxlist id="cblArchive" runat="server" RepeatDirection="Horizontal" BackColor="White"></asp:checkboxlist></TD>
																				</TR>
																				<TR>
																					<TD bgColor="#ffffff"><asp:linkbutton id="btnSuche" runat="server" CssClass="StandardButtonTable" Width="100%">&#149&nbsp;Suchen</asp:linkbutton></TD>
																					<TD bgColor="#ffffff"><asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></TD>
																				</TR>
																				<TR>
																					<TD class="GridTableHead" align="left">&nbsp;<STRONG>Suchfelder*</STRONG></TD>
																					<TD class="GridTableHead"><STRONG>Trefferliste**</STRONG></TD>
																				</TR>
																				<TR>
																					<TD vAlign="top" bgColor="#ffffff" colSpan="1" rowSpan="1">
																						<TABLE id="tblSearch" cellSpacing="0" cellPadding="0" border="0" runat="server">
																							<TR id="tRow" runat="server">
																								<TD id="tCell" runat="server"></TD>
																							</TR>
																						</TABLE>
																					</TD>
																					<TD vAlign="top" width="100%" bgColor="#ffffff"><asp:datagrid id="DataGrid1" runat="server" Width="100%" PageSize="50" BackColor="White" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True">
																							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																							<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																							<Columns>
																								<asp:TemplateColumn Visible="False">
																									<HeaderStyle Width="30px"></HeaderStyle>
																									<ItemTemplate>
																										<asp:ImageButton id="ImageButton1" runat="server" ToolTip="Vorschau anzeigen" ImageUrl="../Images/camera.gif" CommandName="vorschau"></asp:ImageButton>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																								<asp:TemplateColumn>
																									<ItemTemplate>
																										<asp:ImageButton id=Imagebutton2 runat="server" Visible='<%# typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull %>' CommandName="ansicht" ImageUrl="/Portal/Images/PDF_grey.gif" ToolTip="Dokumente vom Server laden">
																										</asp:ImageButton>
																										<asp:HyperLink id=Hyperlink5 runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>' Visible='<%# not (typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull) %>' ImageUrl="/Portal/images/pdf.gif" ToolTip="Dokumente anzeigen (.PDF)" Target="_blank">HyperLink</asp:HyperLink>
																										<asp:ImageButton id="btnDetails" runat="server" CommandName="Details" ImageUrl="/Portal/Images/ausruf.gif" ToolTip="Zusatzinformationen"></asp:ImageButton>
																										<asp:Literal id=Literal3 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.DOC_ID") &amp; """><font color=""#FFFFFF"">.</font></a>" %>'>
																										</asp:Literal>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																								<asp:TemplateColumn Visible="False" HeaderText="Seiten">
																									<ItemTemplate>
																										<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>' Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Link") %>'>
																										</asp:HyperLink>
																									</ItemTemplate>
																								</asp:TemplateColumn>
																							</Columns>
																							<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
																						</asp:datagrid></TD>
																				</TR>
																			</TABLE>
																			<INPUT id="txtShowAll" type="hidden" runat="server">
																		</TD>
																	</TR>
																</TBODY></TABLE>
														</TD>
													</TR>
													<tr>
														<td><asp:literal id="Literal1" runat="server"></asp:literal></td>
													</tr>
												</TABLE>
												<FONT face="Arial" size="1">*Nur ausgefüllte Felder werden bei der Suche 
													berücksichtigt.
													<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server">
														<TR>
															<TD noWrap><FONT size="1">**Nur die mit </FONT>
																<asp:checkbox id="cbx1" runat="server" Checked="True" Enabled="False"></asp:checkbox><FONT face="Arial" size="1">markierten 
																	Felder werden in der Trefferliste angezeigt.</FONT>
															</TD>
														</TR>
													</TABLE>
													<br>
												</FONT>
											</TD>
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
