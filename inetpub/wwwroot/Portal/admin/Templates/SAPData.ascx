<%@ Control Language="vb" AutoEventWireup="false" Codebehind="SAPData.ascx.vb" Inherits="CKG.Admin.SAPData" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><asp:datagrid id="DG1" style="Z-INDEX: 101" Width="100%" CellPadding="0" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" runat="server" AutoGenerateColumns="False">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="BAPI" HeaderText="BAPI"></asp:BoundColumn>
					<asp:BoundColumn DataField="StartTime" HeaderText="Start"></asp:BoundColumn>
					<asp:BoundColumn DataField="EndTime" HeaderText="Ende"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Erfolg">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Sucess") %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sucess") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="ErrorMessage" HeaderText="Fehlermeldung"></asp:BoundColumn>
				</Columns>
			</asp:datagrid></td>
	</tr>
</table>
