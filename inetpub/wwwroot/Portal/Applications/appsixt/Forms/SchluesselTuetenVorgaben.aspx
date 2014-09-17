<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SchluesselTuetenVorgaben.aspx.vb" Inherits="AppSIXT.SchluesselTuetenVorgaben" %>
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
										<td class="PageNavigation" height="25" colSpan="2">&nbsp;Administration 
											(Schlüsseltüten - Vorgaben)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TaskTitle" width="150">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" runat="server" CausesValidation="False" CssClass="StandardButton"> &#149;&nbsp;Neue Vorgabe</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CausesValidation="False" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnDelete" runat="server" CausesValidation="False" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Löschen</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
												<TBODY>
													<tr>
														<TD class="TaskTitle">&nbsp;</TD>
													</tr>
													<TR id="trSearchSpacerTop" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left"><asp:datagrid id="dgSearchResult" runat="server" Width="100%" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" BackColor="White">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="VorgabeID" SortExpression="VorgabeID" HeaderText="VorgabeID"></asp:BoundColumn>
																	<asp:ButtonColumn DataTextField="Fahrgestellnummer von" SortExpression="Fahrgestellnummer von" HeaderText="Fahrgestellnummer&lt;br&gt;von" CommandName="Edit">
																		<HeaderStyle Width="100px"></HeaderStyle>
																	</asp:ButtonColumn>
																	<asp:BoundColumn DataField="Fahrgestellnummer bis" SortExpression="Fahrgestellnummer bis" HeaderText="Fahrgestellnummer&lt;br&gt;bis"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Ersatzschl&#252;ssel" SortExpression="Ersatzschl&#252;ssel" HeaderText="Ersatz-&lt;br&gt;schl&#252;ssel"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Carpass" SortExpression="Carpass" HeaderText="Carpass"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Radio Codekarte" SortExpression="Radio Codekarte" HeaderText="Radio&lt;br&gt;Codekarte"></asp:BoundColumn>
																	<asp:BoundColumn DataField="CD-Navigationssystem" SortExpression="CD-Navigationssystem" HeaderText="CD-&lt;br&gt;Navigations-&lt;br&gt;system"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Chipkarte" SortExpression="Chipkarte" HeaderText="Chipkarte"></asp:BoundColumn>
																	<asp:BoundColumn DataField="COC-Papier" SortExpression="COC-Papier" HeaderText="COC-Papier"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Navigationssystem Codekarte" SortExpression="Navigationssystem Codekarte" HeaderText="Navigations-&lt;br&gt;system&lt;br&gt;Codekarte"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Codekarte Wegfahrsperre" SortExpression="Codekarte Wegfahrsperre" HeaderText="Codekarte&lt;br&gt; Wegfahrsperre"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Ersatzfernbedienung Standheizung" SortExpression="Ersatzfernbedienung Standheizung" HeaderText="Ersatz-&lt;br&gt;fernbedienung&lt;br&gt;Standheizung"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Pr&#252;fbuch bei LKW" SortExpression="Pr&#252;fbuch bei LKW" HeaderText="Pr&#252;fbuch&lt;br&gt;(bei LKW)"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="l&#246;schen">
																		<HeaderStyle Width="30px"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:ImageButton id="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="/Portal/Images/icon_nein_s.gif"></asp:ImageButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Mode="NumericPages"></PagerStyle>
															</asp:datagrid></td>
													</tr>
													<tr id="trEditUser" runat="server">
														<td align="left">
															<table id="Table3" width="740" border="0">
																<TR>
																	<TD vAlign="top" align="left">
																		<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD height="22">von Fahrgestellnr.:</TD>
																				<TD align="right" height="22">
																					<P><asp:textbox id="txtZVID" runat="server" Visible="False" Height="0px" Width="0px" BorderStyle="None" BorderWidth="0px" ForeColor="#CEDBDE" BackColor="#CEDBDE">-1</asp:textbox><asp:textbox id="txtCHASSIS_NUM" runat="server" Height="20px" Width="160px" MaxLength="17"></asp:textbox><asp:requiredfieldvalidator id="valCHASSIS_NUM" runat="server" Width="160px" ControlToValidate="txtCHASSIS_NUM" Display="Dynamic" ErrorMessage="Bitte geben Sie einen Startwert für die Fahrgestellnummer ein!"></asp:requiredfieldvalidator>
																						<asp:textbox id="txtCHASSIS_NUM_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></P>
																				</TD>
																			</TR>
																			<TR>
																				<TD height="22">bis&nbsp;Fahrgestellnr.:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtCHASSIS_NUM_BIS" runat="server" Height="20px" Width="160px" MaxLength="17"></asp:textbox><asp:requiredfieldvalidator id="valCHASSIS_NUM_BIS" runat="server" Width="160px" ControlToValidate="txtCHASSIS_NUM_BIS" Display="Dynamic" ErrorMessage="Bitte geben Sie einen Endwert für die Fahrgestellnummer ein!"></asp:requiredfieldvalidator>
																					<asp:textbox id="txtCHASSIS_NUM_BIS_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Hersteller:</TD>
																				<TD align="right" height="22">
																					<P><asp:textbox id="Textbox1" runat="server" Visible="False" Height="0px" Width="0px" BorderStyle="None" BorderWidth="0px" ForeColor="#CEDBDE" BackColor="#CEDBDE">-1</asp:textbox>
																						<asp:textbox id="txtHersteller_alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="10"></asp:textbox><asp:textbox id="txtHersteller" runat="server" Height="20px" Width="160px" MaxLength="10"></asp:textbox></P>
																				</TD>
																			</TR>
																			<TR>
																				<TD height="22">Modell:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtModell" runat="server" Height="20px" Width="160px" MaxLength="20"></asp:textbox>
																					<asp:textbox id="txtModell_alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="20"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD height="22">Ersatzschlüssel:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtERSSCHLUESSEL" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtERSSCHLUESSEL_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Carpass:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtCARPASS" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtCARPASS_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Radio Codekarte:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtRADIOCODEKARTE" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtRADIOCODEKARTE_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">CD-Navigationssystem:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtNAVICD" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtNAVICD_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Chipkarte:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtCHIPKARTE" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtCHIPKARTE_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">COC-Papier:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtCOCBESCH" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtCOCBESCH_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Navigationssystem Codekarte:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtNAVICODEKARTE" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtNAVICODEKARTE_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Codekarte Wegfahrsperre:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtWFSCODEKARTE" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtWFSCODEKARTE_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Ersatzfernbedienung Standheizung:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtSH_ERS_FB" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtSH_ERS_FB_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Prüfbuch (bei LKW):</TD>
																				<TD align="right" height="22"><asp:textbox id="txtPRUEFBUCH_LKW" runat="server" Height="20px" Width="160px"></asp:textbox>
																					<asp:textbox id="txtPRUEFBUCH_LKW_Alt" runat="server" Visible="False" Width="160px" Height="20px" MaxLength="17"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																</TR>
															</table>
														</td>
													</tr>
													<TR>
														<TD align="left" height="25"></TD>
													</TR>
												</TBODY></table>
										</td>
									</tr>
									<tr>
										<td colspan="2"><asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></td>
									</tr>
                                    <tr>
										<td colspan="2"><asp:label id="lblMessage" runat="server" CssClass="TextLarge"></asp:label></td>
									</tr>
									<tr>
										<td></td>
										<td><!--#include File="../../../PageElements/Footer.html" --></td>
									</tr>
								</TBODY></TABLE>
						</td>
					</tr>
				</TBODY></table>
		</form>
	</body>
</HTML>
