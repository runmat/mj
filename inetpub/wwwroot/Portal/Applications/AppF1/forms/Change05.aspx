<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05.aspx.vb" Inherits="AppF1.Change05" %>
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
				<TR>
					<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
						<asp:label id="lblPageTitle" runat="server">Autorisierungen</asp:label></td>
				</TR>
				<TR>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="TaskTitle" vAlign="top" width="120">&nbsp;</TD>
								<TD class="TaskTitle" vAlign="top" colSpan="2"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TitleTask" NavigateUrl="Change03.aspx" Visible="False">Händlersuche</asp:hyperlink>&nbsp;
									<asp:hyperlink id="lnkVertragssuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change03_2.aspx" Visible="False">Vertragssuche</asp:hyperlink></TD>
							</TR>
							<TR id="trSaveButton" runat="server" Visible="False">
								<TD vAlign="top"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:linkbutton></TD>
								<TD vAlign="middle" align="right" width="100%"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>&nbsp;</TD>
								<TD vAlign="top" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
							</TR>
							
							<tr id="trBackbutton" runat="server" Visible="False">
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD vAlign="middle"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" colSpan="2">
									<TABLE id="Table6" cellSpacing="0" cellPadding="5" width="100%" border="0">
										<TR>
											<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" Width="100%" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="AppID" SortExpression="AppID" HeaderText="AppID"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="AppName" SortExpression="AppName" HeaderText="AppName"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="AppURL" SortExpression="AppURL" HeaderText="AppURL"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="AuthorizationID" SortExpression="AuthorizationID" HeaderText="AuthorizationID"></asp:BoundColumn>
														<asp:BoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Anwendung"></asp:BoundColumn>
														<asp:BoundColumn DataField="InitializedBy" SortExpression="InitializedBy" HeaderText="Initiator"></asp:BoundColumn>
														<asp:BoundColumn DataField="InitializedWhen" SortExpression="InitializedWhen" HeaderText="Angelegt" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="CustomerReference" SortExpression="CustomerReference" HeaderText="H&#228;ndler"></asp:BoundColumn>
														<asp:BoundColumn DataField="ProcessReference" SortExpression="ProcessReference" HeaderText="Weiteres&lt;br&gt;Merkmal"></asp:BoundColumn>
														<asp:BoundColumn DataField="ProcessReference2" SortExpression="ProcessReference2" HeaderText="Zulassungsart*">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn SortExpression="BatchAuthorization" HeaderText="Sammel-&lt;br&gt;Autorisierung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Auswahl">
															<ItemTemplate>
																<TABLE id="Table11" cellSpacing="1" cellPadding="1" width="100%" border="0">
																	<TR>
																		<TD align="left">
																			<asp:LinkButton id="Linkbutton1" runat="server" CssClass="StandardButtonSmall" CausesValidation="false" CommandName="Autorisieren" Text="Autorisieren">Autorisieren</asp:LinkButton></TD>
																		<TD align="right">
																			<asp:LinkButton id=Linkbutton4 runat="server" CssClass="StandardButtonStorno" Visible='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>' CausesValidation="False" CommandName="Loeschen" Text="Löschen">Löschen</asp:LinkButton></TD>
																	</TR>
																</TABLE>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="ProcessReference3" HeaderText="Merkmal2"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Position="Top" CssClass="TextExtraLarge" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:datagrid id="Datagrid2" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" Width="100%" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" BackColor="White">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn Visible="False" DataField="AppID" SortExpression="AppID" HeaderText="AppID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="AppName" SortExpression="AppName" HeaderText="AppName"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="AppURL" SortExpression="AppURL" HeaderText="AppURL"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="AuthorizationID" SortExpression="AuthorizationID" HeaderText="AuthorizationID"></asp:BoundColumn>
											<asp:BoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Anwendung"></asp:BoundColumn>
											<asp:BoundColumn DataField="InitializedBy" SortExpression="InitializedBy" HeaderText="Initiator"></asp:BoundColumn>
											<asp:BoundColumn DataField="InitializedWhen" SortExpression="InitializedWhen" HeaderText="Angelegt" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="CustomerReference" SortExpression="CustomerReference" HeaderText="H&#228;ndler"></asp:BoundColumn>
											<asp:BoundColumn DataField="ProcessReference" SortExpression="ProcessReference" HeaderText="Weiteres&lt;br&gt;Merkmal"></asp:BoundColumn>
											<asp:BoundColumn DataField="Ergebnis" SortExpression="Ergebnis" HeaderText="Ergebnis"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Position="Top" CssClass="TextExtraLarge" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></td>
								<TD vAlign="top"></TD>
							</tr>
							<tr>
								<td></td>
								<td colSpan="2"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td colSpan="2"><asp:label id="lblLegende" runat="server" Visible="False">*Für Händlereigene Zulassung: N=Neufahrzeug, S=Selbstfahrervermietfahrzeug, V=Vorführfahrzeug</asp:label></td>
							</tr>
						</TABLE>
					</td>
				</TR>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function AutorisierenConfirm(Anwendung,Initiator,Angelegt,Haendler,Merkmal) {
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
