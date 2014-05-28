<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change07_2.aspx.vb" Inherits="AppCSC.Change07_2" %>
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
								<td class="PageNavigation" noWrap colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> - Vorgangsanzeige</asp:label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
								<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Change07.aspx" CssClass="TaskTitle">Vorgangssuche</asp:hyperlink></TD>
							</TR>
							<tr>
								<TD vAlign="top">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TextHeader"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdConfirm" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdReset" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdBack" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3">&nbsp;</td>
										</tr>
										<TR>
											<TD class="LabelExtraLarge" colSpan="3"><asp:label id="lblNoData" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="tblResult" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="3"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="3">Legende: KA ... 
												Keine Auswahl, LÖ ... Löschen, VS ... Versenden, ÄN ... Ändern der Kontonummer</TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" Width="100%" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Kontonummer" SortExpression="Kontonummer" HeaderText="Kontonummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestell-&lt;br&gt;nummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="Label" SortExpression="Label" HeaderText="Label"></asp:BoundColumn>
														<asp:BoundColumn DataField="Modellbezeichnung" SortExpression="Modellbezeichnung" HeaderText="Modellbezeichnung"></asp:BoundColumn>
														<asp:BoundColumn DataField="Erstellt" SortExpression="Erstellt" HeaderText="Erstellt" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Versendet" SortExpression="Versendet" HeaderText="Versendet" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="KontonummerIdentisch" HeaderText="doppelte&lt;br&gt;VIN">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=DoppelteVIN runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.KontonummerIdentisch") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KontonummerIdentisch") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="NurBrief" HeaderText="Nur Brief">
															<ItemTemplate>
																<asp:CheckBox id=NurBrief runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.NurBrief") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox7 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NurBrief") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="NurDaten" HeaderText="Nur Daten">
															<ItemTemplate>
																<asp:CheckBox id=NurDaten runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.NurDaten") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox8 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NurDaten") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="KA">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionNOTHING runat="server" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionNOTHING") %>'>
																</asp:RadioButton>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ActionNOTHING") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="L&#214;">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionDELE runat="server" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>'>
																</asp:RadioButton>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="VS">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionVERS runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionVERS") %>' GroupName="grpAction">
																</asp:RadioButton>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox5 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ActionVERS") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="&#196;N">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionCHAN runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionCHAN") %>' GroupName="grpAction">
																</asp:RadioButton>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox6 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ActionCHAN") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="Action" HeaderText="Aktion"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Bemerkung" HeaderText="Bemerkung"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</TABLE>
									<TABLE id="tblWait" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="middle" colSpan="3">&nbsp;&nbsp;&nbsp;
												<BR>
												&nbsp;&nbsp;&nbsp;<BR>
												&nbsp;&nbsp;
												<BR>
												Bitte warten Sie.<BR>
												&nbsp;&nbsp;
												<BR>
												Die Daten werden ermittelt.</TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
							<TR>
								<td>&nbsp;</td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
			<asp:checkbox id="chkDataLoaded" runat="server" Visible="False"></asp:checkbox></form>
	</body>
</HTML>
