<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DZFahrzeug.ascx.vb" Inherits="AppAvis.DZFahrzeug" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><asp:datagrid id="DG1" style="Z-INDEX: 101" runat="server" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="0" AutoGenerateColumns="False" Width="100%">
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
					<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="ABE-&lt;br&gt;Daten">
						<ItemTemplate>
							<asp:Literal id=litLink runat="server" Text='<%# "<a href=""../../../Shared/Change06_3.aspx?EqNr=" &amp; DataBinder.Eval(Container, "DataItem.Equipmentnummer") &amp;  """ Target=""_blank"">Anzeige</a>" %>'>
							</asp:Literal>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="Meldungsnummer" SortExpression="Meldungsnummer" HeaderText="Meldungsnummer"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Equipmentnummer" SortExpression="Equipmentnummer" HeaderText="Equipmentnummer"></asp:BoundColumn>
					<asp:TemplateColumn SortExpression="WK1" HeaderText="WK1">
						<ItemTemplate>
							<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrtsKennz") %>'>
							</asp:Label>&nbsp;-
							<asp:TextBox id=Textbox1 runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.WK1") %>' NAME="Textbox2" MaxLength="6">
							</asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="WK2" HeaderText="WK2">
						<ItemTemplate>
							<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrtsKennz") %>'>
							</asp:Label>&nbsp;-
							<asp:TextBox id=Textbox2 runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.WK2") %>' NAME="Textbox4" MaxLength="6">
							</asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="WK3" HeaderText="WK3">
						<ItemTemplate>
							<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrtsKennz") %>'>
							</asp:Label>&nbsp;-
							<asp:TextBox id=Textbox4 runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.WK3") %>' NAME="Textbox5" MaxLength="6">
							</asp:TextBox>
						</ItemTemplate>
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
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="Belegnummer" HeaderText="Ergebnis">
						<ItemTemplate>
							<asp:Label id=Label9 runat="server" Width="140px" Text='<%# DataBinder.Eval(Container, "DataItem.Belegnummer") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid></td>
	</tr>
</table>