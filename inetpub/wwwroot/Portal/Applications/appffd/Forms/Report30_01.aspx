<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report30_01.aspx.vb" Inherits="AppFFD.Report30_01" %>
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
					<td width="1586"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td width="1587">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Auswählen)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;
												<asp:hyperlink id="lnkExcel" runat="server" Target="_blank" Visible="False">Excelformat</asp:hyperlink><asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr id="trVorgangsArt" runat="server">
											<td colSpan="2"></td>
											<TD></TD>
										</tr>
										<TR>
											<TD class="" width="100%"><strong><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></strong></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR id="trPageSize" runat="server">
											<TD class="LabelExtraLarge" align="left" colSpan="2" height="39">
												<TABLE id="Table2" height="71" cellSpacing="1" cellPadding="1" width="812" border="0">
													<TR>
														<TD width="181"><asp:label id="Label1" runat="server" Font-Bold="True" Width="177px">Kontingentarten anzeigen:</asp:label></TD>
														<TD width="94"><asp:checkbox id="chkTemporaer" runat="server" Width="84px" Text="Temporär" Checked="True"></asp:checkbox></TD>
														<TD width="86"><asp:checkbox id="chkEndgueltig" runat="server" Width="82px" Text="Endgültig" Checked="True"></asp:checkbox></TD>
														<TD width="68"><asp:checkbox id="chkRetail" runat="server" Width="63px" Text="Retail" Checked="True"></asp:checkbox></TD>
														<TD width="87"><asp:checkbox id="chkDelayed" runat="server" Width="135px" Text="Delayed payment" Checked="True"></asp:checkbox></TD>
														<TD><asp:checkbox id="chkHEZ" runat="server" Width="175px" Text="Händlereigene Zulassung" Checked="True"></asp:checkbox></TD>
													</TR>
													<TR>
														<TD colSpan="6"><asp:linkbutton id="Linkbutton1" runat="server" Visible="True" Width="137px" CssClass="StandardButton">Anzeige aktualisieren</asp:linkbutton></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="LabelExtraLarge" align="right" height="39"></TD>
										</TR>
										<TR id="trDataGrid1" runat="server">
											<TD align="middle" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BorderColor="White" PageSize="50" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" bodyHeight="350" bodyCSS="tableBody" headerCSS="tableHeader" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Font-Bold="True" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="HaendlerNr" HeaderText="H&#228;ndler-Nr.">
															<HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%# "Report29_23.aspx?Kunnr=" &amp; DataBinder.Eval(Container, "DataItem.HaendlerNr") %>' Text='<%# DataBinder.Eval(Container, "DataItem.HaendlerNr") %>' Target="_blank">
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="TmpKontingent" DataFormatString="{0:#####}" HeaderText="Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TmpInanspruchnahme" DataFormatString="{0:#####}" HeaderText="Inanspuch-&lt;br&gt;nahme">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TmpFreiesKontingent" DataFormatString="{0:#####}" HeaderText="Freies&lt;br&gt;Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="EndgKontingent" DataFormatString="{0:#####}" HeaderText="Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="EndgInanspruchnahme" DataFormatString="{0:#####}" HeaderText="Inanspruch-&lt;br&gt;nahme">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="EndgFreiesKontingent" DataFormatString="{0:#####}" HeaderText="Freies&lt;br&gt;Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="RetailRichtwert" DataFormatString="{0:#####}" HeaderText="Richtwert">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="RetailAusschoepfung" DataFormatString="{0:#####}" HeaderText="Aussch&#246;pfung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DelayedRichtwert" DataFormatString="{0:#####}" HeaderText="Richtwert">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DelayedAusschoepfung" DataFormatString="{0:#####}" HeaderText="Aussch&#246;pfung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="HEZRichtwert" DataFormatString="{0:#####}" HeaderText="Richtwert">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="HEZAusschoepfung" DataFormatString="{0:#####}" HeaderText="Aussch&#246;pfung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
											<TD align="middle"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD width="1586">
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr) {
						var Check = window.confirm("Wollen Sie diesen Kfz-Brief wirklich freigeben?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
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
