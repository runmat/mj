<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ZulassungsVorbelegung.aspx.vb" Inherits="AppSIXT.ZulassungsVorbelegung" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<TBODY>
					<tr>
						<td height="18"><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<td class="PageNavigation" height="25" colSpan="2">&nbsp;Administration 
											(Zulassungsvorbelegung)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="120" height="25">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
												<TR>
													<TD class="TaskTitle">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" runat="server" CssClass="StandardButton" CausesValidation="False"> &#149;&nbsp;Neue Vorbelegung</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False" CausesValidation="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False" CausesValidation="False"> &#149;&nbsp;Löschen</asp:linkbutton></TD>
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
														<TD align="left"></TD>
													</TR>
													<tr id="trSearch" runat="server">
														<td><TABLE border="0" bgColor="white">
																<TBODY>
																	<TR>
																		<TD vAlign="bottom" width="80">Hersteller:</TD>
																		<TD vAlign="bottom">
																			<asp:dropdownlist id="ddlFilterHersteller" runat="server" Width="160px" Height="20px" AutoPostBack="True"></asp:dropdownlist></TD>
																	</TR>
																</TBODY></TABLE>
														</td>
													</tr>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25">
															<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False" DESIGNTIMEDRAGDROP="139"></asp:label></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left">
															<asp:datagrid id="dgSearchResult" runat="server" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" Width="100%" BackColor="White">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="ZVID" SortExpression="ZVID" HeaderText="ZVID"></asp:BoundColumn>
																	<asp:ButtonColumn DataTextField="VonFZN1_3" SortExpression="VonFZN1_3" HeaderText="Von FN&lt;br&gt;Ziff. 1-3" CommandName="Edit">
																		<HeaderStyle Width="100px"></HeaderStyle>
																	</asp:ButtonColumn>
																	<asp:BoundColumn DataField="VonFZN4_17" SortExpression="VonFZN4_17" HeaderText="Von FN&lt;br&gt;Ziff. 4-17"></asp:BoundColumn>
																	<asp:BoundColumn DataField="BisFZN1_3" SortExpression="BisFZN1_3" HeaderText="Bis FN&lt;br&gt;Ziff. 1-3"></asp:BoundColumn>
																	<asp:BoundColumn DataField="BisFZN4_17" SortExpression="BisFZN4_17" HeaderText="Bis FN&lt;br&gt;Ziff. 4-17"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Halter_Name" SortExpression="Halter_Name" HeaderText="Halter"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Versicherer_Name" SortExpression="Versicherer_Name" HeaderText="Versicherer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="AbDatum" SortExpression="AbDatum" HeaderText="g&#252;ltig ab&lt;br&gt;Zul.-Datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:BoundColumn DataField="BisDatum" SortExpression="BisDatum" HeaderText="g&#252;ltig bis&lt;br&gt;Zul.-Datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell"></asp:BoundColumn>
																	<asp:TemplateColumn SortExpression="Limo" HeaderText="Limo">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Limo") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Limo") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn SortExpression="Kennzeichen2zeilig" HeaderText="zwei-&lt;br&gt;zeilig">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox2 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen2zeilig") %>' Enabled="False">
																			</asp:CheckBox>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen2zeilig") %>'>
																			</asp:TextBox>
																		</EditItemTemplate>
																	</asp:TemplateColumn>
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
															<table width="740" border="0" id="Table3">
																<TR>
																	<TD vAlign="top" align="left">
																		<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD height="22">von Fahrgestellnr. Ziff. 1-3:</TD>
																				<TD align="right" height="22">
																					<P><asp:textbox id="txtZVID" runat="server" Visible="False" Height="0px" Width="0px" BorderStyle="None" BorderWidth="0px" BackColor="#CEDBDE" ForeColor="#CEDBDE">-1</asp:textbox><asp:textbox id="txtVonFZN1_3" runat="server" Height="20px" Width="160px" MaxLength="3"></asp:textbox>
																						<asp:RequiredFieldValidator id="valVonFZN1_3" runat="server" Width="160px" ErrorMessage="Bitte geben Sie die ersten 3 Ziffern der Fahrgestellnummer ein!" Display="Dynamic" ControlToValidate="txtVonFZN1_3"></asp:RequiredFieldValidator></P>
																				</TD>
																			</TR>
																			<TR>
																				<TD height="22">
																					von Fahrgestellnr. Ziff. 4-17:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtVonFZN4_17" runat="server" Height="20px" Width="160px" MaxLength="14"></asp:textbox>
																					<asp:RequiredFieldValidator id="valVonFZN4_17" runat="server" Width="160px" ErrorMessage="Bitte geben Sie eine die Ziffern 4-17 der Fahrgestellnummer ein!" Display="Dynamic" ControlToValidate="txtVonFZN4_17"></asp:RequiredFieldValidator></TD>
																			</TR>
																			<TR>
																				<TD height="22">bis&nbsp;Fahrgestellnr. Ziff. 1-3:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtBisFZN1_3" runat="server" Width="160px" Height="20px" MaxLength="3"></asp:textbox>
																					<asp:RequiredFieldValidator id="valBisFZN1_3" runat="server" Width="160px" ErrorMessage="Bitte geben Sie die ersten 3 Ziffern der Fahrgestellnummer ein!" Display="Dynamic" ControlToValidate="txtBisFZN1_3"></asp:RequiredFieldValidator></TD>
																			</TR>
																			<TR>
																				<TD height="22">bis&nbsp;Fahrgestellnr. Ziff. 4-17:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtBisFZN4_17" runat="server" Width="160px" Height="20px" MaxLength="14"></asp:textbox>
																					<asp:RequiredFieldValidator id="valBisFZN4_17" runat="server" Width="160px" ErrorMessage="Bitte geben Sie eine die Ziffern 4-17 der Fahrgestellnummer ein!" Display="Dynamic" ControlToValidate="txtBisFZN4_17"></asp:RequiredFieldValidator></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" border="0" bgColor="white">
																			<TR>
																				<TD height="22" width="104">Halter:</TD>
																				<TD align="right" height="22">
																					<asp:DropDownList id="ddlHalter_SAPNr" runat="server" Width="161px" Height="19px"></asp:DropDownList></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">
																					Versicherer:</TD>
																				<TD align="right" height="22">
																					<asp:DropDownList id="ddlVersicherer_SAPNr" runat="server" Width="161px" Height="19px"></asp:DropDownList></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">gültig ab Zul.-Datum:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtAbDatum" runat="server" Width="160px" Height="20px">01.01.2004</asp:textbox>
																					<asp:RequiredFieldValidator id="valAbDatum" runat="server" Width="160px" ErrorMessage="Bitte geben Sie ein Datum ein!" Display="Dynamic" ControlToValidate="txtAbDatum"></asp:RequiredFieldValidator></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">gültig bis Zul.-Datum:</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtBisDatum" runat="server" Width="160px" Height="20px">31.12.9999</asp:textbox>
																					<asp:RequiredFieldValidator id="valBisDatum" runat="server" Width="160px" ErrorMessage="Bitte geben Sie ein Datum ein!" Display="Dynamic" ControlToValidate="txtBisDatum"></asp:RequiredFieldValidator></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">Hersteller</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtHersteller" runat="server" Width="160px" Height="20px" MaxLength="10"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">Modell</TD>
																				<TD align="right" height="22">
																					<asp:textbox id="txtModell" runat="server" Width="160px" Height="20px" MaxLength="40"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">&nbsp;&nbsp;&nbsp;&nbsp;
																				</TD>
																				<TD align="right" height="22">&nbsp;&nbsp;&nbsp;&nbsp;
																				</TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">&nbsp;
																				</TD>
																				<TD align="right" height="22"><STRONG>Kennzeichenserie </STRONG>
																				</TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">Normale Serie</TD>
																				<TD align="right" height="22">
																					<asp:RadioButton id="chkKeineAuswahl" runat="server" Checked="True" GroupName="Kennzeichenserie"></asp:RadioButton></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">Limoservice</TD>
																				<TD align="right" height="22">
																					<asp:RadioButton id="chkLimo" runat="server" GroupName="Kennzeichenserie"></asp:RadioButton></TD>
																			</TR>
																			<TR>
																				<TD height="22" width="104">2-zeiliges Kennzeichen</TD>
																				<TD vAlign="top" align="right" height="22">
																					<asp:RadioButton id="chkKennzeichen2zeilig" runat="server" GroupName="Kennzeichenserie"></asp:RadioButton></TD>
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
										<td></td>
										<td><asp:label id="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:label></td>
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
