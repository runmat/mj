<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report50_3.aspx.vb" Inherits="AppFFE.Report50_3" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top">
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" NavigateUrl="Report23.aspx" CssClass="TaskTitle">Händlersuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkVertragssuche" runat="server" NavigateUrl="Report23_2.aspx" CssClass="TaskTitle">Vertragssuche</asp:hyperlink></TD>
										</TR>
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<td>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<TD class="LabelExtraLarge">&nbsp;&nbsp;&nbsp;
															</TD>
														<td align="right"></td>
													</tr>
													<TR>
														<TD class="LabelExtraLarge">
															<asp:label id="lblNoData" runat="server" Visible="False"></asp:label>
															<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
														<td align="right">
															<img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /><strong> <asp:hyperlink id="lnkExcel"  CssClass="ExcelButton" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink></STRONG>&nbsp;
															<asp:label id="lblDownloadTip" 	Style ="height: 20px; text-align: center" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label>
															<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</TR>
												</table>
											</td>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" Width="100%" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Finanzierungsnummer">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnr.">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Ordernummer" SortExpression="Ordernummer" HeaderText="Ordernummer">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="COC" SortExpression="COC" HeaderText="COC"></asp:BoundColumn>
														<asp:BoundColumn DataField="Erfassung Fahrzeug" SortExpression="Erfassung Fahrzeug" HeaderText="Eingang ZBII" DataFormatString="{0:dd.MM.yyyy}">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Angefordert am" SortExpression="Angefordert am" HeaderText="Angefordert&lt;br&gt;am" DataFormatString="{0:dd.MM.yyyy}">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Angefordert um" SortExpression="Angefordert um" HeaderText="Angefordert&lt;br&gt;um" DataFormatString="{0:hh:mm:ss}">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Gesperrt" SortExpression="Gesperrt" HeaderText="Gesperrt">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Freigegeben am" SortExpression="Freigegeben am" HeaderText="Freigegeben&lt;br&gt;am" DataFormatString="{0:dd.MM.yyyy}">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Freigegeben um" SortExpression="Freigegeben um" HeaderText="Freigegeben&lt;br&gt;um" DataFormatString="{0:hh:mm:ss}">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Versendet am" SortExpression="Versendet am" HeaderText="Versendet&lt;br&gt;am" DataFormatString="{0:dd.MM.yyyy}">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														</asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" CssClass="TextExtraLarge" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td></td>
								<td></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,Kennzeichen,Ordernummer,Angefordert,Versendet) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\n\tKfz-Kennzeichen\t" + Kennzeichen + "\t\n\tOrdernummer\t" + Ordernummer + "\t\n\tAngefordert\t" + Angefordert + "\t\n\tVersendet\t" + Versendet);
						return (Check);
						}
						//-->
						</script>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
