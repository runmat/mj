<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change47_02.aspx.vb" Inherits="CKG.Components.ComCommon.Change47_02"%>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Autorisieren</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change47.aspx">Suche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" width="100%"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
														<TD align="right"></TD>
													</TR>
													<tr>
														<td class="LabelExtraLarge" width="100%"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></td>
														<td align="right"></td>
													</tr>
												</table>
											</TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3"><asp:DataGrid ID="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody"
                                                    CssClass="tableMain" Width="100%" BackColor="White" PageSize="50" bodyHeight="400"
                                                    AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="EQUNR" SortExpression="EQUNR" HeaderText="EQUNR"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Rechnungsnummer" HeaderText="col_Rechnungsnummer">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_Rechnungsnummer" runat="server" CommandName="Sort" CommandArgument="Rechnungsnummer">col_Rechnungsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Rechnungsnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="Versanddatum" SortExpression="Versanddatum" HeaderText="Versanddatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Versandstatus" SortExpression="Versandstatus" HeaderText="Versandstatus"></asp:BoundColumn>
														<asp:BoundColumn DataField="Abrufgrund" SortExpression="Abrufgrund" HeaderText="Abrufgrund"></asp:BoundColumn>
														<asp:BoundColumn DataField="Versandadresse" SortExpression="Versandadresse" HeaderText="Versandadresse">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														
                                                        <asp:TemplateColumn  HeaderText="col_Lastschrifterzeugung">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Lastschrifterzeugung" runat="server">col_Lastschrifterzeugung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                               <asp:DropDownList  ID="cboLastschrifterzeugung" runat="server"  EnableViewState="true">
                                                               <asp:ListItem Value="J" Text="ja"></asp:ListItem>
                                                                   <asp:ListItem  Value="N" Text="nein"></asp:ListItem>
                                                               </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
														
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id="lbStatus" CssClass="StandardButtonSmall" CommandName="Status" Runat="server">Statusänderung</asp:LinkButton>
																<asp:Label id="lblStatus" runat="server" Visible="False" Font-Bold="True"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td width="120">&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<TR id="ShowScript" runat="server">
								<td width="120">&nbsp;</td>
								<td>
									<script language="JavaScript">
										<!-- //
										// window.document.Form1.elements[window.document.Form1.length-3].focus();
										//-->
									</script>
								</td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
