<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_2.aspx.vb" Inherits="AppAvis.Change01_2" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top"></TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">
                                                &nbsp;</TD>
										</TR>
										<tr>
											<td>
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> •&nbsp;Zurück</asp:LinkButton>
                                            </td>
										</tr>
										<TR>
											<td>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<TD class="LabelExtraLarge">
                                                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;</TD>
														<td align="right"></td>
													</tr>
													<TR>
														<TD class="LabelExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
														<td align="right"><strong>&nbsp;<img alt="" src="../../../images/excel.gif" style="width: 16px; height: 16px" />&nbsp; <asp:LinkButton
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>&nbsp; Anzahl 
                                                            Vorgänge / Seite </strong><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</TR>
												</table>
											</td>
										</TR>
										<TR>
											<TD>
											<asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowSorting="True" 
                                                    AllowPaging="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" 
                                                    headerCSS="tableHeader" PageSize="50" BackColor="White" 
                                                    AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" CssClass="TextExtraLarge" Wrap="False" Mode="NumericPages"></PagerStyle>
												<Columns>
												<asp:BoundColumn DataField="Carportnr" SortExpression="Carportnr" HeaderText="Carport"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Hersteller_ID_Avis" SortExpression="Hersteller_ID_Avis" HeaderText="Hersteller Id.">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Typ_ID_Avis" SortExpression="Typ_ID_Avis" HeaderText="Modell id."></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Modellbezeichnung" SortExpression="Modellbezeichnung" HeaderText="Modellbezeichnung">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Reifenart" SortExpression="Reifenart" HeaderText="Reifenart">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer">
                                                    </asp:BoundColumn>

                                                    <asp:BoundColumn DataField="Eingangsdatum" SortExpression="Datum Eingang" HeaderText="Datum Eingang" DataFormatString="{0:d}"></asp:BoundColumn>
                                                    <asp:BoundColumn DataFormatString="{0:d}" DataField="Datum_Bereit" SortExpression="Datum Bereit" HeaderText="Datum Bereit">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="NummerZBII" SortExpression="NummerZBII"  HeaderText="ZBII Nummer"></asp:BoundColumn>
                                                    <asp:BoundColumn DataFormatString="{0:d}" DataField="Datum_zur_Sperre" SortExpression="Datum Sperre" HeaderText="Datum Sperre"> </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Sperrvermerk" SortExpression="Sperrvermerk"  HeaderText="Sperrvermerk"></asp:BoundColumn>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="50" HeaderText="col_Bereit">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Bereit"  CommandName="sort" CommandArgument="Bereit" runat="server" >col_Bereit</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkBereit" Enabled="false" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="50px"></ItemStyle>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="EQUNR" SortExpression="EQUNR"  HeaderText="EQUNR" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="QMNUM" SortExpression="QMNUM" HeaderText="QMNUM" Visible="False"> </asp:BoundColumn>                                                                                                                                                         
                                                </Columns>											
												</asp:datagrid>
										   </TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td></td>
								<td><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" 
                                        Visible="False"> •&nbsp;Speichern&nbsp;»</asp:linkbutton></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
