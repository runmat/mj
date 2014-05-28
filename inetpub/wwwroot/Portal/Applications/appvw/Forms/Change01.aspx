<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01.aspx.vb" Inherits="AppVW.Change01" %>
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
			<input type="hidden" value="keine" name="txtAktion"> <input type="hidden" value=" " name="txtReferenz1">
			<input type="hidden" value=" " name="txtSchluessel"> <input type="hidden" value=" " name="txtName1">
			<input type="hidden" value=" " name="txtName2"> <input type="hidden" value=" " name="txtStrasse">
			<INPUT type="hidden" value=" " name="txtHausnummer"> <input type="hidden" value=" " name="txtPLZ">
			<INPUT type="hidden" value=" " name="txtOrt"><input type="hidden" value=" " name="txtZielbahnhof">
			<input type="hidden" value=" " name="txtBemerkung">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="150" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> Suchen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">&nbsp;</TD>
										</TR>
									</TABLE>
									<P>&nbsp;</P>
									<P>&nbsp;</P>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;&nbsp;
												<asp:hyperlink id="lnkExcel" runat="server" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:hyperlink id="lnkExcelResult" runat="server" Target="_blank">Excelformat Ergebnis</asp:hyperlink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Font-Size="8pt" Font-Bold="True" Visible="False">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left"><asp:panel id="PanelDatagrids" runat="server">
													<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
														<TR>
															<TD class="TextLarge" vAlign="top" width="150">Vorhaben-Nummer:&nbsp;&nbsp;&nbsp;</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:textbox id="txtVorhabenNummer" runat="server"></asp:textbox></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150">Kennzeichen:&nbsp;&nbsp;</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:radiobutton id="rbAlle" runat="server" GroupName="grpKennzeichen" Text="alle"></asp:radiobutton>&nbsp;&nbsp;
																<asp:radiobutton id="rbUnbearbeitet" runat="server" GroupName="grpKennzeichen" Text="unbearbeitet (U)"></asp:radiobutton>&nbsp;&nbsp;
																<asp:radiobutton id="rbFreigegeben" runat="server" GroupName="grpKennzeichen" Text="freigegeben (F)"></asp:radiobutton>&nbsp;&nbsp;
																<asp:radiobutton id="rbStorniert" runat="server" GroupName="grpKennzeichen" Text="storniert (S)"></asp:radiobutton></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" colSpan="2">
																<P>
																	<asp:datagrid id="Datagrid2" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="200" AutoGenerateColumns="False" Width="100%">
																		<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																		<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																		<Columns>
																			<asp:TemplateColumn SortExpression="Schluessel" HeaderText="Vorhaben">
																				<ItemTemplate>
																					<asp:LinkButton id=LinkButton2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vorhaben") %>' CommandName="Details">
																					</asp:LinkButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn Visible="False" SortExpression="HaendlerID" HeaderText="H&#228;ndler-ID-Nr.">
																				<ItemTemplate>
																					<asp:LinkButton id=LinkButton3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.HaendlerID") %>' CommandName="Details">
																					</asp:LinkButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="HaendlerNr" HeaderText="Hdl.-Nr.">
																				<ItemTemplate>
																					<asp:LinkButton id=LinkButton4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.HaendlerNr") %>' CommandName="Details">
																					</asp:LinkButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="HaendlerName" HeaderText="H&#228;ndlername">
																				<ItemTemplate>
																					<asp:HyperLink id=HyperLink1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.HaendlerName") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.HaendlerAdresse") %>'>
																					</asp:HyperLink>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn DataField="Anzahl" SortExpression="Anzahl" HeaderText="Anzahl Fahrzeuge"></asp:BoundColumn>
																			<asp:BoundColumn DataField="Zielbahnhof" SortExpression="Zielbahnhof" HeaderText="Zielbahnhof"></asp:BoundColumn>
																			<asp:TemplateColumn SortExpression="Status" HeaderText="Status">
																				<ItemTemplate>
																					<asp:Literal id=Literal4 runat="server" Text='<%# "<a name=""" &amp; Replace(Replace(Replace(DataBinder.Eval(Container, "DataItem.Schluessel"),"/",""),"-","")," ","") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.Status") &amp; "</a>" %>'>
																					</asp:Literal>
																					<asp:Label id=Label3 runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<asp:LinkButton id=lnkFreigeben runat="server" CssClass="SmallButtonTable" Text="Freigeben" CommandName="Freigeben" CausesValidation="False" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status") = "U" %>'>Freigeben</asp:LinkButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<asp:LinkButton id=LnkAendern runat="server" CssClass="SmallButtonTable" Text="Ändern" CommandName="Aendern" CausesValidation="False" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status") = "U" %>'>Ändern</asp:LinkButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<asp:LinkButton id=lnkStorno runat="server" CssClass="SmallButtonTable" Text="Storno" CausesValidation="False" CommandName="Storno" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status") = "U" %>'>Storno</asp:LinkButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn Visible="False" DataField="Schluessel" SortExpression="Schluessel" HeaderText="Schluessel"></asp:BoundColumn>
																		</Columns>
																		<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
																	</asp:datagrid></P>
															</TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" colSpan="2">
																<asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="300" AutoGenerateColumns="False" Width="100%" AllowSorting="True">
																	<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																	<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																	<Columns>
																		<asp:TemplateColumn SortExpression="Referenz2" HeaderText="Vorhaben">
																			<ItemTemplate>
																				<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz2") %>'>
																				</asp:Label>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:BoundColumn DataField="Referenz1" SortExpression="Referenz1" HeaderText="IKZ"></asp:BoundColumn>
																		<asp:BoundColumn DataField="FZGTYP" SortExpression="FZGTYP" HeaderText="Typ"></asp:BoundColumn>
																		<asp:BoundColumn DataField="VARIANTE" SortExpression="VARIANTE" HeaderText="Variante"></asp:BoundColumn>
																		<asp:TemplateColumn SortExpression="NAME1_SUS" HeaderText="H&#228;ndler">
																			<ItemTemplate>
																				<asp:HyperLink id=HyperLink2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_SUS") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.NAME2_SUS") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.HaendlerAdresse") %>'>
																				</asp:HyperLink>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn SortExpression="Status" HeaderText="Status">
																			<ItemTemplate>
																				<asp:Literal id=Literal2 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.Referenz1") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.Status") &amp; "</a>" %>'>
																				</asp:Literal>
																				<asp:Label id=Label4 runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
																				</asp:Label>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<ItemTemplate>
																				<asp:LinkButton id=Linkbutton1 runat="server" CssClass="SmallButtonTable" Text="Freigeben" CausesValidation="False" CommandName="Freigeben" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status") = "U" %>'>Freigeben</asp:LinkButton>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<ItemTemplate>
																				<asp:LinkButton id=Linkbutton5 runat="server" CssClass="SmallButtonTable" Text="Ändern" CausesValidation="False" CommandName="Aendern" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status") = "U" %>'>Ändern</asp:LinkButton>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<ItemTemplate>
																				<asp:LinkButton id=Linkbutton6 runat="server" CssClass="SmallButtonTable" Text="Storno" CausesValidation="False" CommandName="Storno" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status") = "U" %>'>Storno</asp:LinkButton>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:BoundColumn Visible="False" DataField="Referenz1" SortExpression="Referenz1" HeaderText="IKZ"></asp:BoundColumn>
																	</Columns>
																	<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
																</asp:datagrid></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150"></TD>
															<TD class="TextLarge" vAlign="top">
																<asp:Literal id="Literal3" runat="server"></asp:Literal>
																<asp:literal id="Literal1" runat="server"></asp:literal></TD>
														</TR>
													</TABLE>
												</asp:panel><asp:panel id="PanelAdressAenderung" runat="server">
													<TABLE id="Table5" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
														<TR>
															<TD class="TextLarge" vAlign="top" width="150">Vorgang&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:Label id="lblSchluessel" runat="server" Visible="False"></asp:Label>
																<asp:Label id="lblReferenz" runat="server" Visible="False"></asp:Label>
																<asp:Literal id="litVorgang" runat="server"></asp:Literal></TD>
														</TR>
														<TR>
															<TD class="StandardTableAlternate" vAlign="top" width="150">Händler 
																Anrede&nbsp;&nbsp;
															</TD>
															<TD class="StandardTableAlternate" vAlign="top">
																<asp:TextBox id="txtName1Input" runat="server" Width="350px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150">Händler Name&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtName2Input" runat="server" Width="350px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD class="StandardTableAlternate" vAlign="top" width="150">Händler 
																Straße&nbsp;&nbsp;
															</TD>
															<TD class="StandardTableAlternate" vAlign="top">
																<asp:TextBox id="txtStrasseInput" runat="server" Width="320px"></asp:TextBox>
																<asp:Label id="Label2" runat="server" Width="10px"></asp:Label>
																<asp:TextBox id="txtHausnummerInput" runat="server" Width="20px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150">PLZ&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtPLZInput" runat="server" Width="350px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD class="StandardTableAlternate" vAlign="top" width="150">Händler Ort&nbsp;&nbsp;
															</TD>
															<TD class="StandardTableAlternate" vAlign="top">
																<asp:TextBox id="txtOrtInput" runat="server" Width="350px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150">Zielbahnhof&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtZielbahnhofInput" runat="server" Width="350px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD class="StandardTableAlternate" vAlign="top" width="150">Memofeld&nbsp;&nbsp;
															</TD>
															<TD class="StandardTableAlternate" vAlign="top">
																<asp:TextBox id="txtBemerkungInput" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox></TD>
														</TR>
														<TR id="RadioVW" runat="server">
															<TD class="TextLarge" vAlign="top" width="150"></TD>
															<TD class="TextLarge" vAlign="top">
																<asp:radiobutton id="rbUnbearbeitetDP" runat="server" GroupName="grpKennzeichenDP" Text="unbearbeitet (U)"></asp:radiobutton>&nbsp;&nbsp;
																<asp:radiobutton id="rbFreigegebenDP" runat="server" GroupName="grpKennzeichenDP" Text="freigegeben (F)"></asp:radiobutton>&nbsp;&nbsp;
																<asp:radiobutton id="rbStorniertDP" runat="server" GroupName="grpKennzeichenDP" Text="storniert (S)"></asp:radiobutton></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150"></TD>
															<TD class="TextLarge" vAlign="top">
																<asp:LinkButton id="lnkAendern2" runat="server" CssClass="SmallButtonTable" CommandName="Freigeben" CausesValidation="False">Ändern</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																<asp:LinkButton id="cmdBack2" runat="server" CssClass="SmallButtonTable">Zurück</asp:LinkButton></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Literal id="Literal5" runat="server"></asp:Literal></TD>
															<TD class="TextLarge" vAlign="top"></TD>
														</TR>
													</TABLE>
												</asp:panel><BR>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="150">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="150">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
