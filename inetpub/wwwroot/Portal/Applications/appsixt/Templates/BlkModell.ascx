<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="BlkModell.ascx.vb" Inherits="AppSIXT.BlkModell" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><DBWC:HIERARGRID id="HG1" style="Z-INDEX: 101" Width="100%" runat="server" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="0" TemplateCachingBase="Tablename" LoadControlMode="UserControl" TemplateDataMode="Table" AutoGenerateColumns="False">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:BoundColumn Visible="False" DataField="RegelID" SortExpression="RegelID" HeaderText="RegelID"></asp:BoundColumn>
					<asp:BoundColumn DataField="PDI_Name" SortExpression="PDI_Name" HeaderText="Bezeichnung lt. PDI"></asp:BoundColumn>
					<asp:BoundColumn DataField="BezeichnungltBrief" SortExpression="BezeichnungltBrief" HeaderText="Bezeichnung lt. Brief">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="SummeVorschlaege" SortExpression="SummeVorschlaege" HeaderText="Summer der vorgeschlagenen Fahrzeuge">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
			</DBWC:HIERARGRID></td>
	</tr>
</table>
