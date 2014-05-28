<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change42_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change42_2" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR id="trcmdSave" runat="server">
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR id="trcmdSave2" runat="server">
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSave2" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change42.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;
												<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change42_2.aspx">Fahrzeugauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" width="100%"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
														<TD align="right"></TD>
													</TR>
													<tr>
														<td class="LabelExtraLarge" width="100%"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></td>
														<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
													<TR>
														<TD class="LabelExtraLarge" width="100%"><STRONG>Nachricht für Empfänger</STRONG>*<STRONG>:</STRONG>&nbsp;
															<asp:textbox id="txtKopf" runat="server" Width="100px" MaxLength="23"></asp:textbox><asp:linkbutton id="LinkButton1" runat="server" CssClass="StandardButtonTable" Visible="False">&#149;&nbsp;Kopftext erf.</asp:linkbutton></TD>
														<TD noWrap align="right"></TD>
													</TR>
												</table>
											</TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Kontonummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Kontonummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>' ID="Label1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>' ID="Label2">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
															<HeaderTemplate>
																<asp:LinkButton id="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>' ID="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>' ID="Label4">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM" CommandName="Sort">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="TEXT50" SortExpression="TEXT50" HeaderText="Kopftext"></asp:BoundColumn>
														<asp:TemplateColumn Visible="False" SortExpression="ZZBEZAHLT" HeaderText="Bezahlt">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chkBezahlt" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZCOCKZ">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_COC" runat="server" CommandArgument="LICENSE_NUM" CommandName="Sort">col_COC</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox id=Checkbox2 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="MANDT" HeaderText="Nicht&lt;br&gt;anfordern">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="rbNichtAnfordern" runat="server" GroupName="grpTyp" Checked='<%# (DataBinder.Eval(Container, "DataItem.MANDT")="0") %>'>
																</asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="MANDT" HeaderText="Temp.">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="rbTemporaer" runat="server" GroupName="grpTyp" Checked='<%# (DataBinder.Eval(Container, "DataItem.MANDT")="1") %>'>
																</asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="MANDT" HeaderText="Endg.">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="rbEndgueltig" runat="server" GroupName="grpTyp" Checked='<%# (DataBinder.Eval(Container, "DataItem.MANDT")="2") %>'>
																</asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="TEXT200" HeaderText="Referenz*">
															<ItemTemplate>
																<asp:TextBox id=txtPosition runat="server" MaxLength="23" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.TEXT200") %>'>
																</asp:TextBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="Versandart">
															<ItemTemplate>
																<asp:Label id=lblTemp runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="1" %>'>Temporär</asp:Label>
																<asp:Label id=lblEndg runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="2" %>'>Endgültig</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Abrufgrund">
															<ItemTemplate>
																<asp:DropDownList id="cmbAbrufgrund" runat="server" AutoPostBack="True" dataSource='<%# cmbAbrufgrund_ItemDataBound1( DataBinder.Eval(Container, "DataItem.MANDT"))%>' DataValueField = "SapWert" DataTextField = "WebBezeichnung" >
																</asp:DropDownList>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Abrufgrund-Info/Text">
															<ItemTemplate>
																<asp:Label ID="lblZusatzinfo" Runat="server" Visible="True" EnableViewState="True"></asp:Label><br>
																<asp:textbox BorderColor="red" BorderStyle="Solid" BorderWidth="1" id="txtZusatztext" runat="server" Visible="False" EnableViewState="True" MaxLength="50" Width="250px"></asp:textbox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</TABLE>
									*<FONT face="Arial" size="1">max.&nbsp;23 Zeichen</FONT></td>
							</tr>
							<TR>
								<td width="120">&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<TR id="ShowScript" runat="server">
								<td width="120">&nbsp;</td>
								<td>
									<script language="JavaScript">
										<!-- //
										// window.document.Form1.elements[window.document.Form1.length-3].focus();
										//-->
									</script>
								</td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
