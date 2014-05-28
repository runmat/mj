<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change46.aspx.vb" Inherits="CKG.Components.ComCommon.Change46" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</head>
	<body style="margin-left: 0; margin-top: 0; top: 0px">
		<form id="Form1" method="post" runat="server">
            <asp:ScriptManager ID="Scriptmanager1" runat="server" EnableScriptGlobalization="true"
                EnableScriptLocalization="true" AsyncPostBackTimeout="36000">
            </asp:ScriptManager>
			<input type="hidden" value="empty" name="txtAuftragsnummer"/>
			<table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colspan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</tr>
							<tr>
								<td valign="top" width="120">
									<table id="Table2" style="border-color: #ffffff" cellspacing="0" cellpadding="0" width="120" border="0">
										<tr>
											<td class="TaskTitle">&nbsp;</td>
										</tr>
										<tr>
											<td valign="middle" width="150"></td>
										</tr>
									</table>
								</td>
								<td>
									<table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" valign="top"><asp:linkbutton id="lbExcel" Visible="False" Runat="server">Excelformat</asp:linkbutton>&nbsp;&nbsp;</td>
										</tr>
									</table>
									<table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<tr>
											<td>
												<table cellspacing="0" cellpadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge" width="100%">
															<%--<p>--%>
                                                                <asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>
																<asp:label id="lblInfo" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
                                                            <%--</p>--%>
														</td>
														<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td>
                                                <asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ItemStyle-Height="22px">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" Runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Anforderungsdatum" SortExpression="Anforderungsdatum" HeaderText="Angefordert am:" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Nummer ZB2"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
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
														<asp:BoundColumn Visible="False" DataField="KVGR3"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
                                            </td>
										</tr>
									</table>
								</td>
							</tr>
							<tr id="ShowScript" runat="server" visible="False">
								<td colspan="2">
									<script type="text/javascript">
									<!-- //
									function StornoConfirm(AngefordertAm,Fahrgestellnummer,Briefnummer,Kontingentart)
									{
										var Check = window.confirm("Wollen Sie diesen Auftrag wirklich stornieren?\n\tAngefordert am\t" + AngefordertAm + "\n\tFahrgestellnr.\t" + Fahrgestellnummer + "\t\n\tKfz-Briefnr.\t" + Briefnummer + "\t\n\tKontingentart\t" + Kontingentart);
										return (Check);
									}
									//-->
									</script>
								</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
								<td valign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</table>
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
</html>
