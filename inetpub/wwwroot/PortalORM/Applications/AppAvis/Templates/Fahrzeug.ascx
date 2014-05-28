<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Fahrzeug.ascx.vb" Inherits="AppAvis.Fahrzeug" %>

<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="50">&nbsp;</td>
		
		<td>

            <asp:datagrid id="DG1" style="Z-INDEX: 101" Width="100%" AutoGenerateColumns="False" CellPadding="0" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" runat="server">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn Visible="False" SortExpression="Modell_ID" HeaderText="Modell_ID">
						<ItemTemplate>
							<asp:TextBox id="txtModell_ID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modell_ID") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Modell_ID") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Eingangsdatum" SortExpression="Eingangsdatum" HeaderText="Eingangsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
					<asp:BoundColumn DataField="Verwendungszweck" SortExpression="Verwendungszweck" HeaderText="Verw.-zweck" ></asp:BoundColumn>
					<asp:BoundColumn DataField="Owner_Code" SortExpression="Owner Code" HeaderText="Owner Code" ></asp:BoundColumn>
          <asp:BoundColumn DataField="Reifenart" SortExpression="Reifenart" HeaderText="Reifenart" ></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Fahrgestellnummer">
						<ItemTemplate>
							<asp:Label id="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' >
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' AutoPostBack="False">
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Sperrvermerk">
						<ItemTemplate>
							<asp:Label id="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sperrvermerk") %>'>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sperrvermerk") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="Sperrvermerk" HeaderText="Sperrvermerk">
						<ItemTemplate>
							<asp:TextBox id="txtBemerkung" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sperrvermerk") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Sperrvermerk") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Sperre bis" >
						<ItemTemplate>
							<asp:TextBox id="txtBemerkungDatum" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.Datum_zur_Sperre","{0:d}") %>'>
							</asp:TextBox>
						    <asp:ImageButton ID="IbtnSperr"   runat="server" ImageUrl="../../../images/Protect.png" ToolTip="Sperren" CommandName="Sperren" Visible="false" />
                            <asp:ImageButton ID="IbtnEntsperr" runat="server" ImageUrl="../../../images/OpenProtect.png" ToolTip="Entsperren"  CommandName="Entsperren" Visible="false"/>
                            <asp:Label id="lblFehler" runat="server" Visible="false" ForeColor="Red" Font-Size="9">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="DatumErstzulassung" HeaderText="Datum&lt;br&gt;Erstzulassung">
						<ItemTemplate>
							<asp:TextBox id="txtDatumErstzulassung" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DatumErstzulassung", "{0:dd.MM.yyyy}") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox5 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DatumErstzulassung", "{0:dd.MM.yyyy}") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="ZielCarport" HeaderText="Ziel&lt;br&gt;Carport">
						<ItemTemplate>
							<asp:TextBox id=txtZielPDI Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZielCarport") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox6  runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZielCarport") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="Ausgewaehlt" HeaderText="Auswahl">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Literal id="HiddenInput3" runat="server" Text='<%# "<input type=""hidden"" name=""Fahrgestellnummer_" &amp; DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") &amp; """ value=""" &amp; DataBinder.Eval(Container, "DataItem.Ausgewaehlt") &amp; """>" %>'>
							</asp:Literal>
							<asp:CheckBox id="chkAuswahl"  runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Ausgewaehlt") %>' >
							</asp:CheckBox>
								<asp:Literal id="HiddenInput4" runat="server" Text='<%# "<input type=""hidden"" name=""Model_" &amp; DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") &amp; """ value=""Modell_ID_" &amp; DataBinder.Eval(Container, "DataItem.Modell_ID") &amp; """>" %>'>
							</asp:Literal>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auswahl") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" SortExpression="Belegnummer" HeaderText="Ergebnis">
						<ItemTemplate>
							<asp:Label id="Label2" runat="server" Width="140px" >
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox7" runat="server" >
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="neuGesperrt" SortExpression="neuGesperrt" HeaderText="neuGesperrt" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="neuEntsperrt" SortExpression="neuEntsperrt" HeaderText="neuEntsperrt" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="Ergebnis" SortExpression="Ergebnis" HeaderText="Ergebnis" Visible="False"></asp:BoundColumn>
				</Columns>
			</asp:datagrid>
                   
				</td>
	</tr>
</table>
