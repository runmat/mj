<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01_2.aspx.vb" Inherits="AppFW.Change01_2" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Bestätigung)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<P><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:linkbutton><U></U></P>
											</TD>
										</TR>
									</TABLE>
									<P align="right">&nbsp;</P>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Adressauswahl</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:hyperlink>&nbsp;<asp:label id="lblAddress" runat="server" Visible="False"></asp:label><asp:label id="lblMaterialNummer" runat="server" Visible="False"></asp:label>&nbsp;&nbsp;
												<asp:LinkButton id="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:LinkButton>&nbsp;
												<asp:LinkButton Visible="False" id="lnkHTMLFormat" runat="server">HTMLFormat</asp:LinkButton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" vAlign="top" align="left"><asp:label id="lblMessage" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
												<TABLE id="Table7" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowSorting="True" AutoGenerateColumns="False" bodyHeight="250" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="KUNNR_ZS" SortExpression="KUNNR_ZS" HeaderText="H&#228;ndler-NR"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Versand an">
																		<ItemTemplate>
																			<asp:Label id=Label2 runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") &amp; ", " &amp; DataBinder.Eval(Container, "DataItem.STREET") &amp; ", " &amp; DataBinder.Eval(Container, "DataItem.POST_CODE1") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.CITY1") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Finanzierung Fordbank">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Labelx21" runat="server" Visible="true" Text='<%# getBoolText(DataBinder.Eval(Container, "DataItem.EIGENTUEMER")) %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Abmeldung erforderlich">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Labelx22" runat="server" Visible="true" Text='<%# getBoolText(DataBinder.Eval(Container, "DataItem.VERMARKT")) %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Versandart">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Labelx2" runat="server" Visible="true" Text='<%# getVersandText(DataBinder.Eval(Container, "DataItem.CODE_VERSANDART")) %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
																	<asp:BoundColumn DataField="ZFCODE" SortExpression="ZFCODE" HeaderText="Bemerkung"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ERDAT" SortExpression="ERDAT" HeaderText="Datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left">
												<P align="left"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></P>
											</TD>
										</TR>
										<TR>
											<TD colSpan="3"><!--#include File="../../../PageElements/Footer.html" -->
												<P align="left">&nbsp;</P>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
