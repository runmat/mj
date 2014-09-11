<%@ Control Language="vb" AutoEventWireup="false" Codebehind="BlkFahrzeuge.ascx.vb" Inherits="AppSIXT.BlkFahrzeuge" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><asp:datagrid id="DG1" style="Z-INDEX: 101" Width="100%" AutoGenerateColumns="False" CellPadding="0" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" runat="server">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:BoundColumn Visible="False" DataField="MODELL_ID" SortExpression="MODELL_ID" HeaderText="MODELL_ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="PDI" SortExpression="PDI" HeaderText="PDI"></asp:BoundColumn>
					<asp:BoundColumn DataField="Referenz" SortExpression="Referenz" HeaderText="Referenz"></asp:BoundColumn>
					<asp:BoundColumn DataField="FIN" SortExpression="FIN" HeaderText="FIN"></asp:BoundColumn>
					<asp:BoundColumn DataField="Kraftstoff" SortExpression="Kraftstoff" HeaderText="Kraftstoff"></asp:BoundColumn>
					<asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller"></asp:BoundColumn>
					<asp:BoundColumn DataField="LeistungKW" SortExpression="LeistungKW" HeaderText="LeistungKW"></asp:BoundColumn>
					<asp:BoundColumn DataField="Navi" SortExpression="Navi" HeaderText="Navi"></asp:BoundColumn>
					<asp:BoundColumn DataField="Bereifung" SortExpression="Bereifung" HeaderText="Bereifung"></asp:BoundColumn>
					<asp:BoundColumn DataField="Farbe" SortExpression="Farbe" HeaderText="Farbe"></asp:BoundColumn>
					<asp:BoundColumn DataField="Ausfuehrung" SortExpression="Ausfuehrung" HeaderText="Ausf&#252;hrung"></asp:BoundColumn>
					<asp:TemplateColumn SortExpression="Ausgewaehlt" HeaderText="Auswahl">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Literal id=HiddenInput3 runat="server" Text='<%# "<input type=""hidden"" name=""ZZFAHRG_" &amp; DataBinder.Eval(Container, "DataItem.FIN") &amp; """ value=""" &amp; DataBinder.Eval(Container, "DataItem.Ausgewaehlt") &amp; """>" %>'>
							</asp:Literal>
							<asp:CheckBox id=chkAuswahl runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Ausgewaehlt") %>'>
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auswahl") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid></td>
	</tr>
</table>
