<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report05.aspx.vb" Inherits="AppDCL._Report05" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style1 {
                width: 208px;
                text-align: center;
            }
            .StandardButtonTable
            {
            }
        </style>
	</HEAD>
	<body leftmargin="0" topmargin="0" MS_POSITIONING="FlowLayout">
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
											<TD class="TaskTitle">
                                                <asp:FileUpload ID="UpFile" runat="server" />
                                            </TD>
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
																					<TD class="style1" id="td10" vAlign="middle" noWrap align="left" 
                                                                                        bgColor="#ffffff" rowSpan="1" runat="server">
                                                                                        <asp:linkbutton id="btnAdd" 
                                                                                            runat="server" CssClass="StandardButtonTable" Width="87px">&#149;&nbsp;Hinzufügen</asp:linkbutton>
                                                                                        <asp:linkbutton id="btnRemove" runat="server" CssClass="StandardButtonTable" 
                                                                                            Width="76px">&#149;&nbsp;Entfernen</asp:linkbutton></TD>
																					<TD class="TableBackGround" id="td0" vAlign="middle" noWrap align="left" bgColor="#ffffff" rowSpan="1" runat="server"><STRONG>
																							<P align="center">Dateien (Web-Dateien (Web-Verzeichnis)</P>
																						</STRONG>
																					</TD>
																				</TR>
																				<TR>
																					<TD vAlign="top" align="center" bgColor="#ffffff" rowSpan="1" class="style1">
                                                                                        <asp:ListBox ID="gridfiles" runat="server" Height="330px" Width="185px" 
                                                                                            style="margin-left: 0px; margin-right: 6px;">
                                                                                        </asp:ListBox>
                                                                                    </TD>
																					<TD vAlign="top" align="center" width="100%" bgColor="#ffffff" colSpan="1" rowSpan="1">
                                                                                        <asp:datagrid id="gridServer" runat="server" width="100%" BackColor="White" 
                                                                                            BorderColor="Transparent" bodyHeight="350" cssclass="tableMain" 
                                                                                            bodyCSS="tableBody" headerCSS="tableHeader" AllowSorting="True" 
                                                                                            AutoGenerateColumns="False">
																							<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																							<HeaderStyle HorizontalAlign="Left" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																							<Columns>
																								<asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad" HeaderText="Serverpfad"></asp:BoundColumn>
																								<asp:BoundColumn Visible="False" DataField="Zeit" SortExpression="Zeit" HeaderText="Erstellt am"></asp:BoundColumn>
																								<asp:TemplateColumn Visible="False" SortExpression="Filename" HeaderText="Dateiname">
																									<ItemTemplate>
																										<asp:HyperLink id=lnkFile runat="server" 
                                                                                                            NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") & DataBinder.Eval(Container, "DataItem.Filename") %>' 
                                                                                                            BorderColor="Transparent" Target="_blank" 
                                                                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>'></asp:HyperLink>
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
																								    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                                                                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
																								</asp:TemplateColumn>
																								<asp:BoundColumn Visible="False" DataField="Filename" SortExpression="Filename" HeaderText="Dateiname"></asp:BoundColumn>
																								<asp:TemplateColumn HeaderText="Auftrag suchen und zuordnen">
																									<ItemTemplate>
																										<TABLE id="Table11" height="0" cellSpacing="1" cellPadding="1" width="100%" border="0">
																											<TR>
																											    <td>
                                                                                                                    <asp:Label ID="Label1" runat="server" 
                                                                                                                        ToolTip="Referenznummer (siehe Protokoll)">Referenz</asp:Label>
                                                                                                                    <asp:Label ID="lblDummyForScrollHere" runat="server"></asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="Label2" runat="server" ToolTip="Auftragsnummer (Auto-Eintrag)">Auftrag-Nr.</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblAbgabedatum" runat="server" Font-Bold="True" 
                                                                                                                        ToolTip="Format: TT.MM.JJJJ" Visible="False">Abgabedatum</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblKategorie" runat="server" Font-Bold="True" 
                                                                                                                        ToolTip="Kategorie" Visible="False">Kategorie</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                </td>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtReferenz" runat="server" BackColor="White" Enabled="False" 
                                                                                                                        Width="100px"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtAuftrag" runat="server" BackColor="White" Enabled="False" 
                                                                                                                        Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.auftrag") %>'></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtAbgabedatum" runat="server" BackColor="#80FF80" 
                                                                                                                        Visible="False" Width="100px"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drpKategorie" runat="server" Visible="False">
                                                                                                                    </asp:DropDownList>
                                                                                                                    <asp:TextBox ID="txtKategorie" runat="server" Enabled="False" Visible="False" 
                                                                                                                        Width="105px"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td nowrap>
                                                                                                                    <!--<asp:LinkButton id="btnSuche" runat="server" CssClass="StandardButtonTable" ToolTip="Auftrags-/Referenznummer aus SAP lesen" CommandName="searchSAP">Suchen...</asp:LinkButton>-->
                                                                                                                    <asp:ImageButton ID="ibtnNoSearch" runat="server" CausesValidation="false" 
                                                                                                                        CommandName="noSearch" ImageUrl="../../../Images/empty.gif" />
                                                                                                                    <asp:Button ID="ibtnSearch" runat="server" BorderStyle="NotSet" 
                                                                                                                        CausesValidation="false" CommandName="searchSAP" Font-Underline="false" 
                                                                                                                        Text="Suchen..." ToolTip="Auftrags-/Referenznummer aus SAP lesen" 
                                                                                                                        Visible="False" />
                                                                                                                    <asp:LinkButton ID="btnKategorie" runat="server" 
                                                                                                                        CommandName="KategorieZuordnen" CssClass="StandardButtonTable" Visible="False">Kategorie 
                                                                                                                    zuordnen...</asp:LinkButton>
                                                                                                                    <td nowrap>
                                                                                                                        <asp:LinkButton ID="btnAssignOrder" runat="server" CommandName="assignOrder" 
                                                                                                                            CssClass="StandardButtonTable" ToolTip="Auftrags-/Referenznummer zuordnen" 
                                                                                                                            Visible="False">Zuordnen</asp:LinkButton>
                                                                                                                    </td>
                                                                                                                    <td nowrap width="100%">
                                                                                                                        <asp:LinkButton ID="btnReAssignOrder" runat="server" 
                                                                                                                            CommandName="unassignOrder" CssClass="StandardButtonTable" 
                                                                                                                            ToolTip="Auftrags-/Referenznummer Zuordnung aufheben" Visible="False">Zuordnung 
                                                                                                                        aufheben</asp:LinkButton>
                                                                                                                    </td>
                                                                                                                </td>
                                                                                                            </tr>
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
																					<TD id="td12" vAlign="top" noWrap align="right" bgColor="#ffffff" runat="server" 
                                                                                        class="style1">
                                                                                        <asp:linkbutton id="btnUploadPDF" runat="server" 
                                                                                            CssClass="StandardButtonTable" Width="166px">&#149;&nbsp;Datei(en) hochladen</asp:linkbutton></TD>
																					<TD id="td11" vAlign="top" noWrap align="right" bgColor="#ffffff" runat="server"><asp:linkbutton id="btnBack" runat="server" CssClass="StandardButtonTable" Visible="False">&#149;&nbsp;Zurück</asp:linkbutton>&nbsp;<asp:linkbutton id="Linkbutton1" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Weiter zur Auftragsbestätigung</asp:linkbutton>&nbsp;<asp:linkbutton id="btnUpload" runat="server" CssClass="StandardButtonTable" Visible="False">&#149;&nbsp;Protokoll(e) archivieren!</asp:linkbutton></TD>
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
