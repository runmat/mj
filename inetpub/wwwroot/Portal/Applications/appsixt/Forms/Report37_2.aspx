<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report37_2.aspx.vb" Inherits="AppSIXT.Report37_2" %>
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
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top"></TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td lass="TaskTitle"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" Visible="False" NavigateUrl="Report01.aspx">Zusammenstellung von Abfragekriterien</asp:hyperlink></td>
											<TD colSpan="2"></TD>
										</tr>
										<TR>
											<TD align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TBODY>
														<tr>
															<td class="TaskTitle"><asp:hyperlink id="lnkExcel" runat="server" CssClass="TaskTitle" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
																<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Sichern</asp:linkbutton></td>
											</TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
								</td>
								<TD align="right"></TD>
							<TR>
								<TD class=""><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
								<TD align="right" colSpan="2">
									<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
							</TR>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="StandardButton">&#149;&nbsp;Liste abschicken!</asp:LinkButton>
                                </td>
                                <td>
                                </td>
                            </tr>
							<TR>
								<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" Width="100%">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
                                            <asp:BoundColumn Visible="False" DataField="Equipmentnummer" SortExpression="Equipmentnummer"
                                                HeaderText="Equipmentnummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fahrg-Nr" SortExpression="Fahrg-Nr" HeaderText="Fahrg-Nr"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Equipmenttyp" SortExpression="Equipmenttyp" HeaderText="Equipmenttyp"></asp:BoundColumn>
											<asp:BoundColumn DataField="Ersatzsschl&#252;ssel" SortExpression="Ersatzsschl&#252;ssel" HeaderText="Ersatzs-&lt;br&gt;schl&#252;ssel"></asp:BoundColumn>
											<asp:BoundColumn DataField="Carpass" SortExpression="Carpass" HeaderText="Carpass"></asp:BoundColumn>
											<asp:BoundColumn DataField="Radiocodekarte" SortExpression="Radiocodekarte" HeaderText="Radio-&lt;br&gt;codekarte"></asp:BoundColumn>
											<asp:BoundColumn DataField="CD-Navi" SortExpression="CD-Navi" HeaderText="CD-Navi"></asp:BoundColumn>
											<asp:BoundColumn DataField="Chipkarte" SortExpression="Chipkarte" HeaderText="Chipkarte"></asp:BoundColumn>
											<asp:BoundColumn DataField="COC-Papier" SortExpression="COC-Papier" HeaderText="COC-Papier"></asp:BoundColumn>
											<asp:BoundColumn DataField="Navi-Codekarte" SortExpression="Navi-Codekarte" HeaderText="Navi-&lt;br&gt;Codekarte"></asp:BoundColumn>
											<asp:BoundColumn DataField="Codekarte WFS" SortExpression="Codekarte WFS" HeaderText="Codekarte&lt;br&gt;WFS"></asp:BoundColumn>
											<asp:BoundColumn DataField="Ersatzfernbed Standh" SortExpression="Ersatzfernbed Standh" HeaderText="Ersatzfernbed&lt;br&gt;Standh"></asp:BoundColumn>
											<asp:BoundColumn DataField="Pr&#252;fbuch LKW" SortExpression="Pr&#252;fbuch LKW" HeaderText="Pr&#252;fbuch&lt;br&gt;(bei LKW)"></asp:BoundColumn>
											<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
											<asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Briefnummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="Zulassungsdatum" SortExpression="Zulassungsdatum" HeaderText="Erstzulassungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="PDI-Nr" SortExpression="PDI-Nr" HeaderText="PDI-Nr"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="l&#246;schen">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxDelete" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Delete") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
								<TD colSpan="1"></TD>
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
			</table>
			</TD></TR>
			<TR id="ShowScript" runat="server" visible="False">
				<TD>
					<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
						return (Check);
						}
						//-->
					</script>
				</TD>
			</TR>
			</TBODY></TABLE></form>
	</body>
</HTML>
