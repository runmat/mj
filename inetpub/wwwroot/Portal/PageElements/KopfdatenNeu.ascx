<%---------------------------------------------------%>
<%--für die Verwendung auf Seiten mit DOCTYPE XHTML--%>
<%---------------------------------------------------%>

<%@ Control Language="vb" AutoEventWireup="false" Codebehind="KopfdatenNeu.ascx.vb" Inherits="CKG.Portal.PageElements.KopfdatenNeu" TargetSchema="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" %>
<table id="Table0" cellspacing="0" cellpadding="0" width="100%" align="center" style="background-color: White" class="TableKontingent">
	<tr>
		<td valign="top" class="">
			<table id="Table1" cellspacing="0" cellpadding="3" width="100%" align="center" border="0">
				<tr>
					<td class="TextLarge" valign="top">
						<asp:label id="lbl_HaendlerNummer" runat="server">Händlernummer:</asp:label></td>
					<td class="TextLarge"><asp:label id="lblHaendlerNummer" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="TextLarge" valign="top">Name:&nbsp;&nbsp;
					</td>
					<td class="TextLarge" width="100%"><asp:label id="lblHaendlerName" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="TextLarge" valign="top">Adresse:</td>
					<td class="TextLarge"><asp:label id="lblAdresse" runat="server"></asp:label></td>
				</tr>
			</table>
		</td>
		<td></td>
		<td valign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" cellpadding="3" GridLines="None" BorderColor="#C0C0C0" BorderWidth="1px">
				<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="TextLarge"></ItemStyle>
				<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn Visible="False" HeaderText="ZeigeKontingentart">
						<ItemTemplate>
							<asp:CheckBox id="CheckBox2" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Kontingent">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
							</asp:Label>
							<asp:Label id="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>' Visible="False">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Ausschoepfung" HeaderText="Inanspruchnahme">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Freies Kontingent">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Gesperrt">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
			</asp:datagrid></td>
	</tr>
</table>
<table id="Table3" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
	<tr>
		<td class="TextLarge" valign="top"><asp:label id="lblMessage" runat="server"></asp:label>
		</td>
	</tr>
</table>
