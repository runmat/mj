<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report05.aspx.vb" Inherits="AppFFD.Report05" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="KopfdatenNeu" Src="../../../PageElements/KopfdatenNeu.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
        <%--Style-Anpassungen bedingt durch Umstellung von HTML 4.0 auf XHTML--%>
        <style type="text/css">
            #DataGrid1 td
            {
                border-color: #C0C0C0;
            }
            #DataGrid1 td .StandardButtonTable
            {
                padding-left: 32px;
                padding-right: 32px;
                padding-top: 4px;
                padding-bottom: 4px;
            }
        </style>
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
											<td valign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Sichern</asp:linkbutton></td>
										</tr>
									</table>
								</td>
								<td>
									<table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" valign="top"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></td>
										</tr>
									</table>
									<table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td><uc1:KopfdatenNeu id="Kopfdaten1" runat="server"></uc1:KopfdatenNeu></td>
										</tr>
										<tr>
											<td>
												<table cellspacing="0" cellpadding="0" width="100%" border="0">
													<tr>
														<td class="LabelExtraLarge" width="100%"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
														<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td>
                                                <asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" Width="100%" headerCSS="tableHeader" 
                                                    bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" PageSize="50" BackColor="White" ItemStyle-Height="24px" GridColor="#C0C0C0" BorderColor="#C0C0C0">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Auftragsnummer" SortExpression="Auftragsnummer" HeaderText="Auftragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Vertragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Angefordert am" SortExpression="Angefordert am" HeaderText="Angefordert am:" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Kfz-Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Gesperrt" SortExpression="Gesperrt" HeaderText="Gesperrt" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
														<asp:ButtonColumn Text="Storno" CommandName="Storno"></asp:ButtonColumn>
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
									function StornoConfirm(Auftragsnummer,Vertragsnummer,AngefordertAm,Fahrgestellnummer,Briefnummer,Kontingentart)
									{
										var Check = window.confirm("Wollen Sie diesen Auftrag wirklich stornieren?\n\tVertrag\t\t" + Vertragsnummer + "\n\tAngefordert am\t" + AngefordertAm + "\n\tFahrgestellnr.\t" + Fahrgestellnummer + "\t\n\tKfz-Briefnr.\t" + Briefnummer + "\t\n\tKontingentart\t" + Kontingentart);
										if (Check)
										{
											window.document.Form1.txtAuftragsnummer.value = Auftragsnummer;
										}
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
                            Auftragsnummer:
                        </td>
                        <td>
                            <asp:Label ID="lblStornoDetailsAuftragsnummer" runat="server"></asp:Label>
                        </td>
                    </tr>           
                </table>
            </asp:Panel>
		</form>
	</body>
</html>
