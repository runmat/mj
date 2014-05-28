<%@ Control Language="vb" AutoEventWireup="false" Codebehind="BAPIData.ascx.vb" Inherits="CKG.Admin.BAPIData" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><asp:datagrid id="DG1" style="Z-INDEX: 101" Width="100%" CellPadding="0" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" runat="server" AutoGenerateColumns="False">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="BAPI">
						<ItemTemplate>
							<asp:HyperLink id=HyperLink1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BAPI") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Beschreibung") %>'>
							</asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Erfolg">
						<HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id=CheckBox1 runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Erfolg") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Fehlermeldung") %>'>
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Start SAP">
						<HeaderStyle Width="150px"></HeaderStyle>
						<ItemTemplate>
							<asp:HyperLink id=HyperLink2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Start SAP") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Ende SAP") %>'>
							</asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Dauer SAP">
						<HeaderStyle HorizontalAlign="Center" Width="205px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<TABLE id="Table18" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD align="right" width="50">
										&nbsp;</TD>
									<TD align="right" width="30">
										<asp:Label id=Label6 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer SAP") %>'>
										</asp:Label>&nbsp;</TD>
									<TD width="125">
										<asp:Label id=Label7 runat="server" BackColor="Highlight" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer SAP"))) %>' Height="10px">
										</asp:Label></TD>
								</TR>
							</TABLE>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid></td>
	</tr>
</table>
