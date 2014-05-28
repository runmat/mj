<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Kopfdaten.ascx.vb" Inherits="AppFFE.Kopfdaten" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0" bgColor="white" class="TableKontingent">
	<TR>
		<TD vAlign="top" class="">
			<TABLE id="Table1" cellSpacing="0" cellPadding="3" width="100%" align="center" border="0">
				<TR>
					<TD class="TextLarge" vAlign="top">
						<asp:label id="lbl_HaendlerNummer" runat="server">Händlernummer:</asp:label></TD>
					<TD class="TextLarge"><asp:label id="lblHaendlerNummer" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="TextLarge" vAlign="top">Name:&nbsp;&nbsp;
					</TD>
					<TD class="TextLarge" width="100%"><asp:label id="lblHaendlerName" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="TextLarge" vAlign="top">Adresse:</TD>
					<TD class="TextLarge"><asp:label id="lblAdresse" runat="server"></asp:label></TD>
				</TR>
			</TABLE>
		</TD>
		<TD></TD>
		<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" CellPadding="3" GridLines="None" BorderWidth="2px">
				<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="TextLarge"></ItemStyle>
				<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn Visible="False" HeaderText="ZeigeKontingentart">
						<ItemTemplate>
							<asp:CheckBox id=CheckBox2 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Kontingent">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
							</asp:Label>
							<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>' Visible="False">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
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
							<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Gesperrt">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id=CheckBox1 runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
			</asp:datagrid></TD>
	</TR>
</TABLE>
<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
	<TR>
		<TD class="TextLarge" vAlign="top"><asp:label id="lblMessage" runat="server"></asp:label>
		</TD>
	</TR>
</TABLE>
