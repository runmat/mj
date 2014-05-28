<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CKG.Portal.PageElements" Assembly="CKG.Portal"   %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChangeStdText.aspx.vb" Inherits="AppUeberf.ChangeStdText"%>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<TBODY>
				<table cellSpacing="0" cellPadding="0" width="100%" align="center">
					<TBODY>
						<tr>
							<td height="18"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
						</tr>
						<tr>
							<td>
								<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
									<TBODY>
										<TR>
											<td class="PageNavigation" colSpan="2" height="25"><asp:label id="lblHead" runat="server"></asp:label></td>
										</TR>
										<tr>
											<TD vAlign="top" width="138" height="383">
												<TABLE id="Table2" borderColor="#ffffff" height="127" cellSpacing="0" cellPadding="0" width="142" border="0">
													<TR>
														<TD class="TaskTitle" height="2">&nbsp;</TD>
													</TR>
													<TR>
														<TD height="132">
															<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
																<TR>
																	<TD vAlign="middle" width="132"><asp:linkbutton id="lbtnconfirm" runat="server" Width="120px" CssClass="StandardButton" Visible="False">&#149;&nbsp;Übernehmen</asp:linkbutton></TD>
																</TR>
																<TR>
																	<TD vAlign="middle" width="132"><asp:linkbutton id="lbtnGruppen" runat="server" Width="120px" CssClass="StandardButton">&#149;&nbsp;Gruppen</asp:linkbutton></TD>
																</TR>
																<TR>
																	<TD vAlign="middle" width="132"><asp:linkbutton id="lbtnTexte" runat="server" Width="120px" CssClass="StandardButton">&#149;&nbsp;Standardtexte</asp:linkbutton></TD>
																</TR>
																<TR>
																	<TD vAlign="middle" width="132"><asp:linkbutton id="lbtnBack" runat="server" Width="120px" CssClass="StandardButton" Visible="False">&#149;&nbsp;Zurück</asp:linkbutton></TD>
																</TR>
																<TR>
																	<TD vAlign="middle" width="132"></TD>
																</TR>
																<TR>
																	<TD vAlign="middle" width="132" height="11"></TD>
																</TR>
															</table>
														</TD>
													</TR>
												</TABLE>
											</TD>
											<TD vAlign="top" height="383">
												<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
													<tr>
														<TD class="TaskTitle">&nbsp; <INPUT id="txtSave" type="hidden" size="5" runat="server"></TD>
														<TD class="TaskTitle">&nbsp;</TD>
													</tr>
													<TR>
														<TD>&nbsp;</TD>
														<TD></TD>
													</TR>
													<TR>
														<TD align="left" height="235">
															<TABLE id="dgSpacer" height="231" cellSpacing="0" cellPadding="0" width="820" border="0">
																<TR id="trlvGruppelabel" runat="server" Visible="False">
																	<TD width="190" height="16"><asp:label id="Label6" runat="server">Gruppe:</asp:label></TD>
																	<TD width="180" height="16"><asp:label id="Label7" runat="server">Untergruppe:</asp:label></TD>
																	<TD width="25" height="16"></TD>
																	<TD height="16"></TD>
																</TR>
																<TR id="trlvGruppe" runat="server" Visible="False">
																	<TD width="190" height="65">
																		<P><asp:listbox id="lvGruppe" runat="server" Width="180px" CssClass="ListStyle" Height="230px" AutoPostBack="True"></asp:listbox></P>
																	</TD>
																	<TD width="180" height="65">
																		<P><asp:listbox id="lvUntergruppe" runat="server" Width="180px" CssClass="ListStyle" Height="230px" AutoPostBack="True"></asp:listbox></P>
																	</TD>
																	<TD></TD>
																	<TD>
																		<TABLE id="GruppeTable" cellSpacing="0" cellPadding="0" border="0">
																			<TR id="trHauptgruppe" runat="server" Visible="False">
																				<TD width="115"><asp:label id="Label2" runat="server" Width="56px">Gruppe:</asp:label></TD>
																				<TD><asp:textbox id="txtGruppe" runat="server" MaxLength="30"></asp:textbox><INPUT id="txtHauptID" type="hidden" size="5" name="txtHauptID" runat="server">&nbsp;</TD>
																			</TR>
																			<TR>
																				<TD width="115"></TD>
																				<TD></TD>
																			</TR>
																			<TR id="trUntergruppe" runat="server" Visible="False">
																				<TD width="115"><asp:label id="Label3" runat="server">Untergruppe:</asp:label></TD>
																				<TD><asp:textbox id="txtUGruppe" runat="server" MaxLength="30"></asp:textbox><INPUT id="txtUnterID" type="hidden" size="5" name="txtUnterID" runat="server"></TD>
																			</TR>
																			<TR>
																				<TD width="115">&nbsp;</TD>
																				<TD>&nbsp;</TD>
																			</TR>
																			<TR id="trStdTextName" runat="server" Visible="False">
																				<TD width="115">&nbsp;<asp:label id="Label5" runat="server" Height="16px">Standardtextname:</asp:label></TD>
																				<TD>&nbsp;<asp:textbox id="txtStdTextName" runat="server" MaxLength="30"></asp:textbox>
																					<INPUT id="txtTextID" type="hidden" size="5" name="txtTextID" runat="server"></TD>
																			</TR>
																			<TR>
																				<TD width="115">&nbsp;</TD>
																				<TD>&nbsp;</TD>
																			</TR>
																			<TR id="trEdit" runat="server" Visible="False">
																				<TD width="115">&nbsp;<asp:label id="Label1" runat="server" Width="97px" Height="82px">Standarttext:</asp:label></TD>
																				<TD>&nbsp;<cc1:textareacontrol id="txtBemerkung" runat="server" Width="350px" Height="100px" MaxLength="256" TextMode="MultiLine"></cc1:textareacontrol><INPUT id="txtDummy" type="hidden" size="5" name="txtDummy" runat="server"></TD>
																			</TR>
																			<TR id="trButtons" runat="server" Visible="False">
																				<TD width="115">&nbsp;</TD>
																				<TD><asp:linkbutton id="lbtnSaveGruppe" runat="server" Width="92px" CssClass="StandardButton">&#149;&nbsp;Speichern</asp:linkbutton>&nbsp;<asp:linkbutton id="lbtnCancel" runat="server" Width="100px" CssClass="StandardButton">&#149;&nbsp;Verwerfen</asp:linkbutton>&nbsp;<asp:linkbutton id="lbtnDelete" runat="server" Width="100px" CssClass="StandardButton">&#149;&nbsp;Löschen</asp:linkbutton></TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
																<TR id="lbuttons" runat="server" Visible="False">
																	<TD height="23">
																		<table cellSpacing="0" cellPadding="0" border="0">
																			<tr>
																				<TD width="94"><asp:linkbutton id="lbtnEditGruppe" runat="server" Width="85px" CssClass="StandardButton"> Bearbeiten</asp:linkbutton></TD>
																				<TD><asp:linkbutton id="lbtnNeuGruppe" runat="server" Width="85px" CssClass="StandardButton">Neu</asp:linkbutton></TD>
																			</tr>
																		</table>
																	</TD>
																	<TD width="180" height="23">
																		<table cellSpacing="0" cellPadding="0" border="0">
																			<tr>
																				<TD width="95"><asp:linkbutton id="lbtnEditUGruppe" runat="server" Width="85px" CssClass="StandardButton">Bearbeiten</asp:linkbutton></TD>
																				<TD width="89"><asp:linkbutton id="lbtnNeuUGruppe" runat="server" Width="85px" CssClass="StandardButton">Neu</asp:linkbutton></TD>
																			</tr>
																		</table>
																	</TD>
																	<TD></TD>
																	<TD></TD>
																</TR>
															</TABLE>
															<TABLE>
																<TR>
																	<TD>&nbsp;</TD>
																	<TD></TD>
																</TR>
																<TR id="TRResult" runat="server" Visible="False">
																	<TD><asp:datagrid id="dgSearchResult" runat="server" Width="460px" AutoGenerateColumns="False" cssclass="StandardTableAlternate">
																			<HeaderStyle Font-Bold="False" Font-Size="18px" BackColor="#ffffff"></HeaderStyle>
																			<Columns>
																				<asp:BoundColumn Visible="False" DataField="Text_ID" HeaderText="ID"></asp:BoundColumn>
																				<asp:BoundColumn Visible="False" DataField="Haupt_ID" HeaderText="Haupt_ID"></asp:BoundColumn>
																				<asp:BoundColumn Visible="False" DataField="Unter_ID" HeaderText="Unter_ID"></asp:BoundColumn>
																				<asp:BoundColumn DataField="Name" HeaderText="Textname"></asp:BoundColumn>
																				<asp:BoundColumn Visible="False" DataField="TEXT" SortExpression="TEXT" HeaderText="Standardtext"></asp:BoundColumn>
																				<asp:TemplateColumn HeaderText="bearbeiten">
																					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																					<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
																					<ItemTemplate>
																						<asp:ImageButton id="Imagebutton3" runat="server" CommandName="Edit" CausesValidation="false" ImageUrl="/Portal/Images/lupe2.gif"></asp:ImageButton>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn HeaderText="loeschen">
																					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																					<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
																					<ItemTemplate>
																						<asp:ImageButton id="Imagebutton4" runat="server" ImageUrl="/Portal/Images/loesch.gif" CausesValidation="false" CommandName="Delete"></asp:ImageButton>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn Visible="False" HeaderText="&#252;bernehmen">
																					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																					<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
																					<ItemTemplate>
																						<asp:CheckBox id="Checkbox2" runat="server"></asp:CheckBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn Visible="False" HeaderText="Vorschau">
																					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																					<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
																					<ItemTemplate>
																						<asp:ImageButton id="IbtnVorschau" runat="server" ImageUrl="/Portal/Images/arrow.gif" CausesValidation="false" CommandName="Vorschau"></asp:ImageButton>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																			</Columns>
																		</asp:datagrid>&nbsp;</TD>
																	<TD>&nbsp;
																		<cc1:textareacontrol id="txtVorschau" runat="server" Width="350px" Visible="False" Height="100px" MaxLength="256" TextMode="MultiLine" Enabled="False"></cc1:textareacontrol></TD>
																</TR>
																<TR>
																	<TD><asp:linkbutton id="lbtnNew" runat="server" Width="120px" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Neuer Text</asp:linkbutton></TD>
																	<TD></TD>
																</TR>
															</TABLE>
														</TD>
														<TD align="left" height="235"></TD>
													</TR>
												</table>
											</TD>
										</tr>
										<TR>
											<TD width="138"></TD>
											<TD>&nbsp;</TD>
										</TR>
										<TR>
											<TD width="138"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD width="138"></TD>
											<TD><asp:label id="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR></form>
		</TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TR></TABLE></TBODY></FORM>
	</body>
</HTML>
