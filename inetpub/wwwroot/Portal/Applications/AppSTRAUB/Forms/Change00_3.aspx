<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change00_3.aspx.vb" Inherits="AppSTRAUB.Change00_3" %>
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
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(Bestätigen)</td>
								</TR>
								<tr>
									<TD class="" vAlign="top">
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top" align="left">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right">
													<P align="left">&nbsp;</P>
												</TD>
											</TR>
											<TR>
												<TD class="" align="right">
													<P align="left"><asp:label id="lblError" runat="server"></asp:label></P>
												</TD>
											</TR>
											<TR id="trSumme" runat="server">
												<TD>
													<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
														<TR>
															<TD align="left">
																<P align="left"><asp:datagrid id="DataGrid1" runat="server" bodyHeight="500" AutoGenerateColumns="False" Width="100%" CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader">
																		<AlternatingItemStyle CssClass="GridTableAlternateTwo"></AlternatingItemStyle>
																		<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																		<Columns>
																			<asp:BoundColumn Visible="False" DataField="RowId" SortExpression="RowId" HeaderText="ID"></asp:BoundColumn>
																			<asp:BoundColumn Visible="False" DataField="Art" SortExpression="Art" HeaderText="Art"></asp:BoundColumn>
																			<asp:TemplateColumn HeaderText="PDI">
																				<ItemTemplate>
																					<asp:Literal id=Literal1 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.RowID") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.KUNPDI") &amp; "</a>" %>'>
																					</asp:Literal>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn DataField="ZZBEZEI" SortExpression="ZZBEZEI" HeaderText="Modell"></asp:BoundColumn>
																			<asp:BoundColumn Visible="False" DataField="Anzahl" SortExpression="Anzahl" HeaderText="Anzahl&lt;br&gt;Fahrzeuge"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZDAT_EIN" SortExpression="ZZDAT_EIN" HeaderText="Eingangs-&lt;br&gt;Datum"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestell-&lt;br&gt;Nummer"></asp:BoundColumn>
																			<asp:BoundColumn DataField="ZZFARBE" SortExpression="ZZFARBE" HeaderText="Farbcode"></asp:BoundColumn>
																			<asp:BoundColumn DataField="SelectedDate" SortExpression="SelectedDate" HeaderText="Zul.Datum"></asp:BoundColumn>
																			<asp:BoundColumn DataField="SelectedVersicherungText" SortExpression="SelectedVersicherungText" HeaderText="Versicherung"></asp:BoundColumn>
																			<asp:BoundColumn DataField="DatumBemerkung" SortExpression="DatumBemerkung" HeaderText="Bem.Datum"></asp:BoundColumn>
																			<asp:BoundColumn DataField="Bemerkung" SortExpression="Bemerkung" HeaderText="Bemerkung"></asp:BoundColumn>
																			<asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
																		</Columns>
																	</asp:datagrid></P>
															</TD>
														</TR>
													</TABLE>
													<P align="right"><asp:linkbutton id="btnConfirm" runat="server" Width="150px" CssClass="StandardButtonTable">Absenden!</asp:linkbutton></P>
												</TD>
											</TR>
										</TABLE>
										<!--#include File="../../../PageElements/Footer.html" -->
										<P align="right">&nbsp;</P>
									</TD>
								</tr>
							</TABLE>
		</form>
		</TD></TR></TBODY>
		<SCRIPT language="JavaScript">										
							<!--	
								function WriteID(RowID)
									{
									window.document.Form1.txtRowID.value = RowID;
									}												
							-->
		</SCRIPT>
		</TABLE>
	</body>
</HTML>
