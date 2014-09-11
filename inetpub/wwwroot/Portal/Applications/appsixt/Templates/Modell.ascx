<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Modell.ascx.vb" Inherits="AppSIXT.Modell" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		<td><DBWC:HIERARGRID id="HG1" style="Z-INDEX: 101" Width="100%" runat="server" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="0" TemplateCachingBase="Tablename" LoadControlMode="UserControl" TemplateDataMode="Table" AutoGenerateColumns="False">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:BoundColumn Visible="False" DataField="ID" SortExpression="ID" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="PDI_Nummer" SortExpression="PDI_Nummer" HeaderText="PDI_Nummer"></asp:BoundColumn>
					<asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Modellbezeichnung" SortExpression="Modellbezeichnung" HeaderText="Modell">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Schaltung" SortExpression="Schaltung" HeaderText="Schaltung">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ausfuehrung" SortExpression="Ausfuehrung" HeaderText="Ausf&#252;hrung">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Antrieb" SortExpression="Antrieb" HeaderText="Antrieb">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Bereifung" SortExpression="Bereifung" HeaderText="Reifen">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Navigation" SortExpression="Navigation" HeaderText="Navi">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn SortExpression="Beklebung" HeaderText="Bekleb.">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id=chkBeklebung runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Beklebung") %>'>
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sperre") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="Limo" HeaderText="VM">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="CheckBox1" runat="server" Enabled="False"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="Limo" HeaderText="Limo">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id=CheckBox2 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Limo") %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox7 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Limo") %>'>
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
							<asp:TextBox id=txtBemDatum runat="server" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.BemerkungDatum", "{0:dd.MM.yyyy}") %>'>
							</asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="DatumErstzulassung" HeaderText="Datum&lt;br&gt;Erstzulassung">
						<ItemTemplate>
							<asp:TextBox id=txtDatumErstzulassung runat="server" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.DatumErstzulassung", "{0:dd.MM.yyyy}") %>'>
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
					<asp:BoundColumn DataField="Anzahl_alt" SortExpression="Anzahl_alt" HeaderText="Anzahl&lt;br&gt;vorhanden">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Anzahl&lt;br&gt;Auswahl">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Literal id=Literal1 runat="server" Text='<%# "<input type=""hidden"" name=""Anzahl_alt_" &amp; DataBinder.Eval(Container, "DataItem.ID") &amp; """ value=""" &amp; DataBinder.Eval(Container, "DataItem.Anzahl_alt") &amp; """>" %>'>
							</asp:Literal>
							<asp:TextBox id=Anzahl_neu runat="server" Width="50px" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl_neu") %>' CssClass="InputRight">
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=Textbox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl_neu") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:Literal id=ConfirmImageButton runat="server" Text="<%# &quot;<a href=&quot;&quot;javascript:SetValues('&quot; &amp; DataBinder.Eval(Container, &quot;DataItem.ID&quot;) &amp; &quot;','&quot; &amp; DataBinder.Eval(Container, &quot;DataItem.Task&quot;) &amp; &quot;')&quot;&quot;><img id=&quot;&quot;Picture_&quot; &amp; DataBinder.Eval(Container, &quot;DataItem.ID&quot;) &amp; &quot;&quot;&quot; border=&quot;&quot;0&quot;&quot; src=&quot;&quot;/Portal/Images/Confirm_mini_Grey.gif&quot;&quot; width=&quot;&quot;20&quot;&quot; height=&quot;&quot;18&quot;&quot;></a>&quot; %>">
							</asp:Literal>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</DBWC:HIERARGRID></td>
	</tr>
</table>
