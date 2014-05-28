<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change00_2.aspx.vb" Inherits="AppSTRAUB.Change00_2" %>
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
			<table width="100%" border="0">
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(Fahrzeugauswahl)</td>
								</TR>
								<tr>
									<TD class="" vAlign="top">
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top" align="left">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right" colSpan="3"><P align="left">Bitte wählen Sie 
														Fahrzeuge zum Zulassen aus.</P>
												</TD>
											</TR>
											<TR>
												<TD align="right" colSpan="3">
													<P align="left"><asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></P>
												</TD>
											</TR>
											<TR>
												<TD class="FadeOutOne" vAlign="center" noWrap align="right" colSpan="1" rowSpan="1">
													<P align="center">PDIs:
														<asp:label id="lblPDIs" runat="server"></asp:label></P>
												</TD>
												<TD class="FadeOutOne" vAlign="center" noWrap align="right">
													<P align="center">Modelle:
														<asp:label id="lblModelle" runat="server"></asp:label></P>
												</TD>
												<TD class="FadeOutOne" vAlign="center" noWrap align="right">
													<P align="center">Fahrzeuge:
														<asp:label id="lblFahrzeuge" runat="server"></asp:label></P>
												</TD>
											</TR>
											<TR id="trSumme" runat="server">
												<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="0" rowSpan="0"><asp:listbox id="lstPDI" runat="server" CssClass="DropDownStyle" Height="400px" AutoPostBack="True"></asp:listbox></TD>
												<TD class="LabelExtraLarge" vAlign="top" align="left">
													<P align="left"><asp:listbox id="lstMOD" runat="server" CssClass="DropDownStyle" Height="400px" AutoPostBack="True"></asp:listbox></P>
												</TD>
												<TD class="LabelExtraLarge" vAlign="top" noWrap align="left">
													<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
														<TR>
															<TD align="left">
																<P align="left"><asp:datagrid id="DataGrid1" runat="server" CssClass="tableMain" BackColor="White" headerCSS="tableHeader" bodyCSS="tableBody" Width="100%" AutoGenerateColumns="False" bodyHeight="400">
																		<AlternatingItemStyle CssClass="GridTableAlternateTwo"></AlternatingItemStyle>
																		<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																		<Columns>
																			<asp:BoundColumn Visible="False" DataField="RowId" SortExpression="RowId" HeaderText="ID"></asp:BoundColumn>
																			<asp:BoundColumn Visible="False" DataField="Art" SortExpression="Art" HeaderText="Art"></asp:BoundColumn>
																			<asp:BoundColumn Visible="False" DataField="KUNPDI" SortExpression="KUNPDI" HeaderText="PDI"></asp:BoundColumn>
																			<asp:BoundColumn Visible="False" DataField="ZZMODELL" SortExpression="ZZMODELL" HeaderText="Modell"></asp:BoundColumn>
																			<asp:BoundColumn Visible="False" DataField="Zusatzdaten" SortExpression="Zusatzdaten" HeaderText="A-S-A-N*">
																				<HeaderStyle Wrap="False"></HeaderStyle>
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZDAT_EIN" SortExpression="ZZDAT_EIN" HeaderText="Eing.Datum"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="FIN"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZFARBE" SortExpression="ZZFARBE" HeaderText="Farbe">
																				<ItemStyle HorizontalAlign="Left"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:TemplateColumn HeaderText="Zul.Datum">
																				<ItemTemplate>
																					<asp:TextBox id="txtZulassungsdatum" runat="server" CssClass="TextBoxStyle" Width="75px" BackColor="White" ToolTip="Format: TT.MM.JJJJ"></asp:TextBox>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn Visible="False" HeaderText="Verw.Zweck">
																				<ItemTemplate>
																					<P align="left">
																						<asp:DropDownList id="ddlVerwendung" runat="server" CssClass="DropDownStyle"></asp:DropDownList></P>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText="Versicherer">
																				<ItemTemplate>
																					<P align="left">
																						<asp:DropDownList id="ddlVersicherung" runat="server" CssClass="DropDownStyle" DESIGNTIMEDRAGDROP="306"></asp:DropDownList></P>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText="Bem.Datum">
																				<ItemTemplate>
																					<asp:TextBox id="txtDatumBemerkung" runat="server" CssClass="DropDownStyle" Width="75px" ToolTip="Format: TT.MM.JJJJ" ReadOnly="True"></asp:TextBox>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText="Bemerkung">
																				<ItemTemplate>
																					<asp:TextBox id="txtBemerkung" runat="server" CssClass="DropDownStyle" Width="150px" ToolTip="Maximal 75 Zeichen." ReadOnly="True"></asp:TextBox>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText="Typdaten">
																				<ItemTemplate>
																					<asp:Literal id=Literal1 runat="server" Text='<%# "<a href=""../../../Shared/Change06_3NEU.aspx?EqNr=" &amp; DataBinder.Eval(Container, "DataItem.Equipment") &amp;  """ Target=""_blank"">Anzeige</a>" %>'>
																					</asp:Literal>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText="Ausw.">
																				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																				<ItemStyle HorizontalAlign="Center"></ItemStyle>
																				<ItemTemplate>
																					<asp:CheckBox id="cbxAuswahl" runat="server"></asp:CheckBox>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn DataField="Status" SortExpression="Status">
																				<ItemStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Red"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<TABLE id="Table4" cellSpacing="1" cellPadding="0" border="0">
																						<TR>
																							<TD>
																								<asp:DropDownList id="ddlKopieren" runat="server" CssClass="DropDownStyle" AutoPostBack="True"></asp:DropDownList></TD>
																							<TD>
																								<asp:Image id="lblInfo1" runat="server" ToolTip="Werte der aktuellen Zeile für die nachfolgende(n) Zeile(n) übernehmen" ImageUrl="/Portal/Images/copy.jpg"></asp:Image></TD>
																						</TR>
																					</TABLE>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn Visible="False" DataField="Equipment" SortExpression="Equipment" HeaderText="Equipment"></asp:BoundColumn>
																		</Columns>
																	</asp:datagrid></P>
															</TD>
														</TR>
													</TABLE>
													<P align="left"><FONT face="Arial" size="1"></FONT>&nbsp;</P>
												</TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge" vAlign="top" colSpan="2">
													<P align="left">*<FONT face="Arial" size="1">Ausführung,Schaltung,Antrieb,Navi</FONT></P>
												</TD>
												<TD class="LabelExtraLarge"><FONT size="2">
														<P align="left"><FONT face="Arial" size="1"></FONT>&nbsp;</P>
													</FONT>
												</TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge" vAlign="top"></TD>
												<TD class="LabelExtraLarge" vAlign="top"></TD>
												<TD class="LabelExtraLarge">
													<P align="right"><asp:linkbutton id="btnConfirm" runat="server" CssClass="StandardButtonTable" Width="150px">Weiter&nbsp;&#187;</asp:linkbutton></P>
												</TD>
											</TR>
										</TABLE>
										<P align="left"><asp:literal id="Literal2" runat="server"></asp:literal></P>
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
