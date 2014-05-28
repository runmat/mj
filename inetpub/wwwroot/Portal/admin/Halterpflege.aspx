<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Halterpflege.aspx.vb" Inherits="CKG.Admin.Halterpflege" %>
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
										<td class="PageNavigation" colSpan="2" height="25">&nbsp;Administration 
											(Halterpflege)</td>
									</TR>
									<tr>
										<TD vAlign="top" width="150" height="25">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="150" border="0">
												<TR>
													<TD class="TaskTitle">&nbsp;</TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnNew" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neuen Halter anlegen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="150"><asp:linkbutton id="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Löschen</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
												<TBODY>
													<tr>
														<TD class="TaskTitle"><asp:hyperlink id="lnkVersichererpflege" runat="server" CssClass="TaskTitle" NavigateUrl="Versichererpflege.aspx">Versichererpflege</asp:hyperlink></TD>
													</tr>
													<tr id="trSearch" runat="server">
														<td align="left">
															<table bgColor="white" border="0">
																<tr>
																	<td vAlign="bottom" width="100"></td>
																	<td vAlign="bottom"><asp:textbox id="txtFilterSAPNr" runat="server" MaxLength="10" Width="160px" Height="20px" Visible="False" Enabled="False">*</asp:textbox></td>
																	<td vAlign="bottom"></td>
																</tr>
																<TR>
																	<TD vAlign="bottom" width="100"></TD>
																	<TD vAlign="bottom"><asp:textbox id="txtFilterName1" runat="server" Width="160px" Height="20px" Visible="False" Enabled="False">*</asp:textbox></TD>
																	<TD vAlign="bottom"></TD>
																</TR>
																<TR>
																	<TD vAlign="bottom" width="100">Kundennr.:</TD>
																	<TD vAlign="bottom"><asp:dropdownlist id="ddlKundennr" runat="server"></asp:dropdownlist></TD>
																	<TD vAlign="bottom"><asp:button id="btnSuche" runat="server" CssClass="StandardButton" Text="Suchen"></asp:button></TD>
																</TR>
															</table>
														</td>
													</tr>
													<TR id="trSearchSpacer" runat="server">
														<TD align="left" height="25"></TD>
													</TR>
													<tr id="trSearchResult" runat="server">
														<td align="left"><asp:datagrid id="dgSearchResult" runat="server" BackColor="White" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True">
																<SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="HalterID" SortExpression="HalterID" HeaderText="HalterID">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:ButtonColumn DataTextField="SAP-Nr" SortExpression="SAP-Nr" HeaderText="SAP-Nr" CommandName="Edit">
																		<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
																	</asp:ButtonColumn>
																	<asp:BoundColumn DataField="CustomerID" SortExpression="CustomerID" HeaderText="Kundennr">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Font-Bold="True"></ItemStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="Name1" SortExpression="Name1" HeaderText="Name1">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="Name2" SortExpression="Name2" HeaderText="Name2">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="StrasseHNr" SortExpression="StrasseHNr" HeaderText="Strasse &amp; Haus-Nr.">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="Ort" SortExpression="Ort" HeaderText="Ort">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:BoundColumn DataField="KBANR" SortExpression="KBANR" HeaderText="KBANR">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="l&#246;schen">
																		<HeaderStyle Width="30px"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:ImageButton id="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="../Images/icon_nein_s.gif"></asp:ImageButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Mode="NumericPages"></PagerStyle>
															</asp:datagrid></td>
													</tr>
													<tr id="trEditUser" runat="server">
														<td align="left">
															<table width="740" border="0">
																<TR>
																	<TD vAlign="top" align="left">
																		<TABLE id="tblLeft" cellSpacing="2" cellPadding="0" width="345" bgColor="white" border="0">
																			<TR>
																				<TD height="22">SAP-Nr:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtHalterID" runat="server" Visible="False" Width="10px" Height="10px" BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None" ForeColor="#CEDBDE">-1</asp:textbox><asp:textbox id="txtSAPNr" runat="server" MaxLength="10" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Name1:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtName1" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Kunde:
																				</TD>
																				<TD align="right" height="22"><asp:textbox id="txtKunnr" runat="server" Width="160px" Height="20px" BackColor="Yellow" BorderWidth="1px" Enabled="False"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">KBANR:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtKBANR" runat="server" MaxLength="7" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																		</TABLE>
																	</TD>
																	<TD width="100%"></TD>
																	<TD vAlign="top" align="right">
																		<TABLE id="tblRight" cellSpacing="2" cellPadding="0" width="345" bgColor="white" border="0">
																			<TR>
																				<TD height="22">Name2:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtName2" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Strasse und Hausnr.:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtStrasseHNr" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
																			</TR>
																			<TR>
																				<TD height="22">Ort:</TD>
																				<TD align="right" height="22"><asp:textbox id="txtOrt" runat="server" Width="160px" Height="20px"></asp:textbox></TD>
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
										<td><asp:label id="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
									</tr>
									<tr>
										<td></td>
										<td><!--#include File="../PageElements/Footer.html" --></td>
									</tr>
								</TBODY></TABLE>
						</td>
					</tr>
				</TBODY></table>
		</form>
	</body>
</HTML>
