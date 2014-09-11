<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Fahrzeug.ascx.vb" Inherits="AppSIXT.Fahrzeug" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><asp:datagrid id="DG1" style="Z-INDEX: 101" Width="100%" AutoGenerateColumns="False" CellPadding="0" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" runat="server">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn Visible="False" SortExpression="Modell_ID" HeaderText="Modell_ID">
						<ItemTemplate>
							<asp:TextBox id=txtModell_ID runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modell_ID") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modell_ID") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Eingangsdatum" SortExpression="Eingangsdatum" HeaderText="Eingangsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Fahrgestellnummer">
						<ItemTemplate>
							<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' Visible='<%# DataBinder.Eval(Container, "DataItem.Briefnr")="A" %>' ForeColor="Red">
							</asp:Label>
							<asp:Label id=Label4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' Visible='<%# DataBinder.Eval(Container, "DataItem.Briefnr")="N" %>' ForeColor="Black">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox8 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="ABE-&lt;br&gt;Daten">
						<ItemTemplate>
							<asp:Literal id=litLink runat="server" Text='<%# "<a href=""../../../Shared/Change06_3.aspx?EqNr=" &amp; DataBinder.Eval(Container, "DataItem.Equipmentnummer") &amp;  """ Target=""_blank"">Alt</a>" %>' Visible="False">
							</asp:Literal>
							<asp:Literal id=Literal1 runat="server" Text='<%# "<a href=""../../../Shared/Change06_3NEU.aspx?EqNr=" &amp; DataBinder.Eval(Container, "DataItem.Equipmentnummer") &amp;  """ Target=""_blank"">Anzeige</a>" %>'>
							</asp:Literal>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="Meldungsnummer" SortExpression="Meldungsnummer" HeaderText="Meldungsnummer"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Equipmentnummer" SortExpression="Equipmentnummer" HeaderText="Equipmentnummer"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Bemerkung">
						<ItemTemplate>
							<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="Bemerkung" HeaderText="Bemerkung">
						<ItemTemplate>
							<asp:TextBox id=txtBemerkung runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Bemerkung&lt;br&gt;Datum">
						<ItemTemplate>
							<asp:TextBox id=txtBemerkungDatum runat="server" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.BemerkungDatum", "{0:dd.MM.yyyy}") %>'>
							</asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="DatumErstzulassung" HeaderText="Datum&lt;br&gt;Erstzulassung">
						<ItemTemplate>
							<asp:TextBox id=txtDatumErstzulassung Width="100px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DatumErstzulassung", "{0:dd.MM.yyyy}") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox5 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DatumErstzulassung", "{0:dd.MM.yyyy}") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="ZielPDI" HeaderText="Ziel&lt;br&gt;PDI">
						<ItemTemplate>
							<asp:TextBox id=txtZielPDI Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZielPDI") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox6 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZielPDI") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="Ausgewaehlt" HeaderText="Auswahl">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Literal id=HiddenInput3 runat="server" Text='<%# "<input type=""hidden"" name=""ZZFAHRG_" &amp; DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") &amp; """ value=""" &amp; DataBinder.Eval(Container, "DataItem.Ausgewaehlt") &amp; """>" %>'>
							</asp:Literal>
							<asp:CheckBox id=chkAuswahl runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Ausgewaehlt") %>'>
							</asp:CheckBox>
							<asp:Literal id=HiddenInput4 runat="server" Text='<%# "<input type=""hidden"" name=""Model_" &amp; DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") &amp; """ value=""Modell_ID_" &amp; DataBinder.Eval(Container, "DataItem.Modell_ID") &amp; """>" %>'>
							</asp:Literal>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auswahl") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="Belegnummer" HeaderText="Ergebnis">
						<ItemTemplate>
							<asp:Label id=Label2 runat="server" Width="140px" Text='<%# DataBinder.Eval(Container, "DataItem.Belegnummer") %>'>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox7 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Belegnummer") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="Briefnr" SortExpression="Briefnr" HeaderText="Brief"></asp:BoundColumn>
				</Columns>
			</asp:datagrid></td>
	</tr>
</table>
