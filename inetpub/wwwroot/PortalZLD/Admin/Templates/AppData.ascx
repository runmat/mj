<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AppData.ascx.vb" Inherits="CKG.Admin.AppData" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><DBWC:HIERARGRID id="HG1" style="Z-INDEX: 101" Width="100%" runat="server" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="0" TemplateCachingBase="Tablename" LoadControlMode="UserControl" TemplateDataMode="Table" AutoGenerateColumns="False">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Inserted" HeaderText="Zeit"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Fehler">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Category") = "ERR" %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Category") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Task" HeaderText="Anwendung"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Anwendung">
						<ItemTemplate>
							<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Task") %>' Visible='<%# NOT IsNumeric(DataBinder.Eval(Container, "DataItem.Anzahl")) %>'>
							</asp:Label>
							<asp:HyperLink id=HyperLink1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Task") %>' NavigateUrl='<%# "../../LogDetails.aspx?StandardLogID=" &amp; DataBinder.Eval(Container, "DataItem.StandardLogID") %>' Target="LogDetails" Visible='<%# IsNumeric(DataBinder.Eval(Container, "DataItem.Anzahl")) %>'>
							</asp:HyperLink>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Task") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Identification" HeaderText="Identifikation"></asp:BoundColumn>
					<asp:BoundColumn DataField="Description" HeaderText="Beschreibung"></asp:BoundColumn>
				</Columns>
			</DBWC:HIERARGRID></td>
	</tr>
</table>
