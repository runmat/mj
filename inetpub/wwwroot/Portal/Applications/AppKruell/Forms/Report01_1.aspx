<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report01_1.aspx.vb" Inherits="AppKruell.Report01_1" %>
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
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server">(Anzeige)</asp:label></td>
								<TR>
									<TD class="TaskTitle" vAlign="top" colSpan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
					</TD>
				</TR>
			</table>
			<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
				<TR>
					<TD width="157">
						<P><asp:linkbutton id="lb_Auswahl" runat="server" CssClass="StandardButton"> &#149;&nbsp;zur Auswahl</asp:linkbutton></P>
					</TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE>
			<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td align="right"><strong>
							<P align="left"><asp:label id="lblnodata" runat="server" EnableViewState="False"></asp:label><br>
								<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label><br>
								<asp:label id="lbl_SAPResultat" runat="server"></asp:label>
						</strong>
						<br>
						<asp:dropdownlist id="ddlPageSize" Runat="server" AutoPostBack="True"></asp:dropdownlist></P></td>
				</tr>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" PageSize="50" BackColor="White" Width="100%">
							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
							<HeaderStyle Wrap="False" HorizontalAlign="Center" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="eingegangen">
									<ItemTemplate>
										<p align="center">
											<asp:LinkButton Enabled='<%# (DataBinder.Eval(Container, "DataItem.EingangKMC")IS System.DBNull.Value) %>' Runat="server" ID="lbEingegangen" CommandName="Eingegangen" Text='<%# changeIcon2(DataBinder.Eval(Container, "DataItem.EingangKMC"))%>'>
											</asp:LinkButton></p>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" ReadOnly="True" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ordernummer" SortExpression="Ordernummer" ReadOnly="True" HeaderText="Ordernummer"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fahrzeugtyp" SortExpression="Fahrzeugtyp" ReadOnly="True" HeaderText="Fahrzeugtyp"></asp:BoundColumn>
								<asp:BoundColumn DataField="Aufbereitungsart" SortExpression="Aufbereitungsart" ReadOnly="True" HeaderText="Aufbereitungsart"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ausstattungspositionen" SortExpression="Ausstattungspositionen" ReadOnly="True" HeaderText="Ausstattungspositionen"></asp:BoundColumn>
								<asp:BoundColumn DataField="Aufbereitung" SortExpression="Aufbereitung" ReadOnly="True" HeaderText="zus. Einbauten"></asp:BoundColumn>
								<asp:BoundColumn DataField="DatumAuftragserteilung" SortExpression="DatumAuftragserteilung" ReadOnly="True" HeaderText="Auftragserteilung" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
								<asp:TemplateColumn SortExpression="Stoerungsmeldung" HeaderText="St&#246;rungsmeldung">
									<ItemTemplate>
										<asp:Label id=Label1 runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.Stoerungsmeldungsdatum") &amp; "<br>" &amp;  DataBinder.Eval(Container, "DataItem.Stoerungsmeldung")  %>'>
										</asp:Label>
									</ItemTemplate>
									<EditItemTemplate>
										<P align="center">
											<asp:TextBox id=txtStoerungsmeldung Runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.Stoerungsmeldung")%>' BorderStyle="Solid" BorderWidth="1" BorderColor="Red" TextMode="MultiLine" Rows="8">
											</asp:TextBox></P>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Fertigstellungsdatum" HeaderText="gepl. Fertigstellung">
									<ItemTemplate>
										<p align="center">
											<asp:Label id=Label2 runat="server" Text='<%# FormatDateBoundColumns(DataBinder.Eval(Container, "DataItem.Fertigstellungsdatum")) %>'>
											</asp:Label></p>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtFertigstellungsdatum" runat="server" Width="100" Text='<%# FormatDateBoundColumns(DataBinder.Eval(Container, "DataItem.Fertigstellungsdatum")) %>' tooltip="gepl. Fertigstellungsdatum">
										</asp:TextBox>
										<asp:Calendar id="calFertigstellung" Runat="server" OnSelectionChanged="Calendar_DateChanged" SelectedDate='<%# checkDate100(DataBinder.Eval(Container, "DataItem.Fertigstellungsdatum")) %>'>
										</asp:Calendar>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="St&#246;rung l&#246;schen">
									<EditItemTemplate>
										<P align="center">
											<asp:LinkButton Runat="server" CommandName="LoeschMeldung" id="lbStoerungLoeschen">
												<img src="../../../Images/loesch.gif" border="0">
											</asp:LinkButton>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Aktualisieren" CancelText="Abbrechen" EditText="Bearbeiten">
									<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
								</asp:EditCommandColumn>
								<asp:BoundColumn Visible="False" DataField="Eskalation" SortExpression="Eskalation" ReadOnly="True" HeaderText="Eskalation"></asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
						<P>&nbsp;</P>
					</TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
