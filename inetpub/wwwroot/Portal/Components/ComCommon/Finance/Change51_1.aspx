<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change51_1.aspx.vb" Inherits="CKG.Components.ComCommon.change51_1" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
            <asp:ScriptManager ID="Scriptmanager1" runat="server" EnableScriptGlobalization="true"
                EnableScriptLocalization="true" AsyncPostBackTimeout="36000">
            </asp:ScriptManager>
			<input type="hidden" value="empty" name="txtAuftragsnummer">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:linkbutton id="lb_Haendlerauswahl" Visible="True" Runat="server">lb_Haendlerauswahl</asp:linkbutton>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<TD>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge" width="100%">
															<%--<P>--%>
                                                                <asp:label id="lblNoData" runat="server" Visible="False"></asp:label>&nbsp;<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;
                                                            <%--</P>--%>
															<%--<P>--%>
															    <asp:label id="label1" runat="server" EnableViewState="False"></asp:label>
                                                            <%--</P>--%>
														</td>
														<td align="right"><asp:linkbutton id="lbExcel" Visible="False" Runat="server">Excelformat</asp:linkbutton>&nbsp;<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</TD>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ItemStyle-Height="22px">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
													<asp:TemplateColumn HeaderText="col_Haendlernummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Haendlernummer" Runat="server" CommandName="Sort" CommandArgument="Haendlernummer">col_Haendlernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Labelxaq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn HeaderText="col_Adresse">
															<HeaderTemplate>
																<asp:LinkButton id="col_Adresse" Runat="server" CommandName="Sort" CommandArgument="Adresse">col_Adresse</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adresse") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
																											
														<asp:TemplateColumn HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" Runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Labelx1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn HeaderText="col_Anforderungsdatum">
															<HeaderTemplate>
																<asp:LinkButton id="col_Anforderungsdatum" Runat="server" CommandName="Sort" CommandArgument="Anforderungsdatum">col_Anforderungsdatum</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Labely1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anforderungsdatum", "{0:d}") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" Runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn HeaderText="col_Briefnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Briefnummer" Runat="server" CommandName="Sort" CommandArgument="Briefnummer">col_Briefnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn HeaderText="col_Kontingentart">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kontingentart" Runat="server" CommandName="Sort" CommandArgument="Kontingentart">col_Kontingentart</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Labelc1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														
														<asp:TemplateColumn SortExpression="gesperrt" HeaderText="col_gesperrt" ItemStyle-HorizontalAlign="Center">
															<HeaderTemplate>
																<asp:LinkButton id="col_gesperrt" CommandArgument="gesperrt" CommandName="Sort" Runat="server">col_gesperrt</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<%--<p align="center">--%>
																	<asp:Label id="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.gesperrt") %>'>
																	</asp:Label>
																<%--</p>--%>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
																<ItemTemplate>
																<%--<p align="center">--%>
																	<asp:LinkButton id="lbStorno" runat="server" Visible="True" CssClass="StandardButtonSmall" Text='Storno' CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' CommandName="Storno" CausesValidation="True">
																	</asp:LinkButton>
																<%--</p>--%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="VBELN"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="EQUNR"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
											</TD>
										</TR>
								
									</TABLE>
								</TD>
							</TR>
								<TR id="ShowScript" runat="server" visible="False">
								<TD colSpan="2">
									<script type="text/javascript">
									<!-- //
									function StornoConfirm(AngefordertAm,Fahrgestellnummer,Briefnummer,Kontingentart)
									{
										var Check = window.confirm("Wollen Sie diesen Auftrag wirklich stornieren?\n\tAngefordert am\t" + AngefordertAm + "\n\tFahrgestellnr.\t" + Fahrgestellnummer + "\t\n\tKfz-Briefnr.\t" + Briefnummer + "\t\n\tKontingentart\t" + Kontingentart);
										return (Check);
									}
									//-->
									</script>
								</TD>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
            <asp:Timer ID="timerHidePopup" runat="server" Interval="2000" Enabled="false"></asp:Timer>
            <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnFake"
                PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="mb" runat="server" BackColor="White" Width="300" Height="66"
                Style="display: none; border: solid 2px #000000">
                <div style="padding-top: 10px; padding-bottom: 5px; text-align: center">
                    <%--Meldungstext kann per Feldübersetzung individuell angepasst werden--%>
                    <asp:Label ID="lbl_AuftragStornoErfolgreichMessage" runat="server" Text="Versandauftrag erfolgreich gelöscht" Font-Bold="True"></asp:Label>
                </div>
                <table width="200" align="center">
                   <tr>
                        <td>
                            Fahrgestellnummer:
                        </td>
                        <td>
                            <asp:Label ID="lblStornoDetailsFahrgestellnummer" runat="server"></asp:Label>
                        </td>
                    </tr>           
                </table>
            </asp:Panel>
		</form>
	</body>
</HTML>
