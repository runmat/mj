<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report04.aspx.vb" Inherits="AppFFE.Report04" %>
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
	    <style type="text/css">
            .style1
            {
                width: 529px;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<tr>
								<td></td>
								<td><asp:linkbutton id="cmdBack" runat="server" Visible="False" CssClass="StandardButton" Enabled="False">&#149;&nbsp;Zurück</asp:linkbutton></td>
							</tr>
							<tr>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="TaskTitle" valign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Report01.aspx" CssClass="TaskTitle">Zusammenstellung 
                                    von Abfragekriterien</asp:hyperlink><asp:linkbutton id="cmdPrint" runat="server" Visible="False" CssClass="StandardButton">Drucken</asp:linkbutton></td>
								<td class="TaskTitle" valign="top">&nbsp;</td>
							</tr>
										<TR>
											<TD align="right" colSpan="2"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:linkbutton></TD>
										<TR>
											<TD class="style1" colSpan="1"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
												&nbsp; &nbsp;&nbsp;
												</TD>
										
											<TD class="LabelExtraLarge" align="right"><strong>
                                                            <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /> <asp:LinkButton
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>
                                                &nbsp; </strong>&nbsp;<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist>
                                                &nbsp;</TD>
										</TR>
										<TR>
											<TD colSpan="2">
									<asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
										<asp:TemplateColumn SortExpression="HAENDLER" HeaderText="col_Haendler" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Haendler" CommandName="sort" CommandArgument="HAENDLER" Runat="server">col_Haendler</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="Label0">
														<%#DataBinder.Eval(Container, "DataItem.HAENDLER")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="ZZKKBER" HeaderText="col_Kontingentart" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Kontingentart" CommandName="sort" CommandArgument="ZZKKBER" Runat="server">col_Kontingentart</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="Label1">
														<%#DataBinder.Eval(Container, "DataItem.ZZKKBER")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="ZZTMPDT" HeaderText="col_Versanddatum" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Versanddatum" CommandName="sort" CommandArgument="ZZTMPDT" Runat="server">col_Versanddatum</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="Label3">
														<%#DataBinder.Eval(Container, "DataItem.ZZTMPDT", "{0:d}")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_Briefnummer" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Briefnummer" CommandName="sort" CommandArgument="TIDNR" Runat="server">col_Briefnummer</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="Label2">
														<%#DataBinder.Eval(Container, "DataItem.TIDNR")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Kennzeichen" CommandName="sort" CommandArgument="LICENSE_NUM" Runat="server">col_Kennzeichen</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="Label4">
														<%#DataBinder.Eval(Container, "DataItem.LICENSE_NUM")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Vertragsnummer" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Vertragsnummer" CommandName="sort" CommandArgument="LIZNR" Runat="server">col_Vertragsnummer</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="lblVertragsnummer">
														<%#DataBinder.Eval(Container, "DataItem.LIZNR")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Fahrgestellnummer" CommandName="sort" CommandArgument="CHASSIS_NUM" Runat="server">col_Fahrgestellnummer</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="lblFahrgestellnummer">
														<%#DataBinder.Eval(Container, "DataItem.CHASSIS_NUM")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="ZMELDBANK" HeaderText="col_MeldungBank" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_MeldungBank" CommandName="sort" CommandArgument="ZMELDBANK" Runat="server">col_MeldungBank</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label5">
														<%#DataBinder.Eval(Container, "DataItem.ZMELDBANK")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="ZZFRIST" HeaderText="col_Zahlungsfrist" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_Zahlungsfrist" CommandName="sort" CommandArgument="ZZFRIST" Runat="server">col_Zahlungsfrist</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label6">
														<%#DataBinder.Eval(Container, "DataItem.ZZFRIST")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"></PagerStyle>
									</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:label id="lblInfo" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
							</tr>
							<tr>
								<td></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" -->
									<asp:Label id="lblHidden" runat="server" Visible="False" Width="39px"></asp:Label></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			</td></tr>
			<tr id="ShowScript" runat="server" visible="False">
				<td>
					<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
						return (Check);
						}
						//-->
					</script>
				</td>
			</tr>
			</TBODY></table></form>
	</body>
</HTML>
