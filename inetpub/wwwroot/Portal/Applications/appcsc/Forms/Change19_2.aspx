<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change19_2.aspx.vb" Inherits="AppCSC.Change19_2" %>
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
								<td class="PageNavigation" noWrap colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Vorgangsanzeige)</asp:label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
								<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkExcel" runat="server" Target="_blank" Visible="False">Excelformat</asp:hyperlink>&nbsp;<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdConfirm" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdReset" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" CssClass="TaskTitle" NavigateUrl="Change07.aspx">Vorgangssuche</asp:hyperlink></td>
										</tr>
									</TABLE>
									<TABLE id="tblResult" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD vAlign="top" align="left" width="100%" colSpan="3"><strong>&nbsp;<asp:label id="lblNoData" runat="server"></asp:label></strong></TD>
											<TD vAlign="top" align="left"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
									</TABLE>
									<TABLE id="tblWait" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" Width="100%" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Equipment" SortExpression="Equipment" HeaderText="Equipment"></asp:BoundColumn>
														<asp:BoundColumn DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Kontonummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestell-&lt;br&gt;nummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="DatumAenderung" SortExpression="DatumAenderung" HeaderText="Datum WV-Liste" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fehlertext" SortExpression="Fehlertext" HeaderText="Status"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="KA">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionNOTHING runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionNOTHING") %>' GroupName="grpAction" AutoPostBack="True">
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
																<asp:RadioButton id=chkActionDELE runat="server" AutoPostBack="True" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>'>
																</asp:RadioButton>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="VS">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionVERS runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionVERS") %>' GroupName="grpAction" AutoPostBack="True">
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
																<asp:RadioButton id=chkActionCHAN runat="server" AutoPostBack="True" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionCHAN") %>' Enabled='<%# DataBinder.Eval(Container, "DataItem.Equipment")<>"" %>'>
																</asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Neue&lt;br&gt;Kontonummer*">
															<ItemTemplate>
																<asp:TextBox id="txtKontonummerNeu" runat="server" Visible="False" Width="100px"></asp:TextBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="Action" SortExpression="Action" HeaderText="Aktion"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Ergebnis">
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bemerkung")="Vorgang erfolgreich" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>' ForeColor="Black">
																</asp:Label>
																<asp:Label id=Label2 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bemerkung")<>"Vorgang erfolgreich" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>' ForeColor="Red">
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="3"></TD>
										</TR>
									</TABLE>
									<FONT face="Arial" size="1"><strong>KA ... Keine Auswahl, LÖ ... Löschen, VS ... 
										Versenden, ÄN ... Ändern der Kontonummer (*Nur bei Änderung)</FONT></STRONG>&nbsp;</TD>
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
