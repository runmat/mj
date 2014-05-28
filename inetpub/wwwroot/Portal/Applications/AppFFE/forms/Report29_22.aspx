<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report29_22.aspx.vb" Inherits="AppFFE.Report29_22" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Auswählen)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Report29.aspx">Filialsuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr id="trVorgangsArt" runat="server">
											<td colSpan="2"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Weiter</asp:linkbutton></td>
											<TD></TD>
										</tr>
										<TR>
											<TD class="" width="100%"><strong><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></strong></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR id="trPageSize" runat="server">
											<TD class="LabelExtraLarge" align="right" colSpan="2"></TD>
											<TD class="LabelExtraLarge" align="right" height="6"></TD>
										</TR>
										<TR id="trDataGrid1" runat="server">
											<TD align="middle" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" bodyHeight="350" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Kunnr" SortExpression="Kunnr" HeaderText="H&#228;ndlernummer"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Kunnr" HeaderText="H&#228;ndler-Nr.">
															<HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%# "Report29_23.aspx?Kunnr=" &amp; DataBinder.Eval(Container, "DataItem.Kunnr") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Kunnr") %>' Target="_blank">
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Standard_Temp" SortExpression="Standard_Temp" HeaderText="Standard Tempor&#228;r">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Standard_Endg" SortExpression="Standard_Endg" HeaderText="Standard Endg&#252;ltig">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DP" SortExpression="DP" HeaderText="Delayed Payment">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="HEZ" SortExpression="HEZ" HeaderText="HEZ">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="KF_KL" SortExpression="KF_KL" HeaderText="KF/KL">
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
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr) {
						var Check = window.confirm("Wollen Sie dieses Dokument wirklich freigeben?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
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
