<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Modell.ascx.vb" Inherits="AppAvis.Modell" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>

<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>

		<td width="50">&nbsp;</td>
		<td>
		<DBWC:HIERARGRID id="HG1" style="Z-INDEX: 101" runat="server" AutoGenerateColumns="False" Width="100%" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="0" TemplateCachingBase="Tablename" LoadControlMode="UserControl" TemplateDataMode="Table">
				<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="GridTableItem"></ItemStyle>
				<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
				<Columns>
					<asp:BoundColumn Visible="False" DataField="ID" SortExpression="ID" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Carportnr" SortExpression="Carportnr" HeaderText="Carportnr."></asp:BoundColumn>
					<asp:BoundColumn DataField="Hersteller_ID_Avis" SortExpression="Hersteller_ID_Avis" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="Herstellername" SortExpression="Herstellername" HeaderText="Hersteller">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Typ_ID_Avis" SortExpression="Typ_ID_Avis" HeaderText="Typ ID Avis">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Modellbezeichnung" SortExpression="Modellbezeichnung" HeaderText="Modell">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Reifenart" SortExpression="Reifenart" HeaderText="Reifenart">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Liefermonat" SortExpression="Liefermonat" HeaderText="Liefermonat">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Kraftstoffart" SortExpression="Kraftstoffart" HeaderText="Kraftstoffart">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Navigation" SortExpression="Navigation" HeaderText="Navi">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Zulassungsort" SortExpression="Zulassungsort" HeaderText="Zulassungsort">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>					

					<asp:TemplateColumn Visible="False" SortExpression="Limo" HeaderText="VM">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="CheckBox1" runat="server" Enabled="False"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn SortExpression="Bezahltkennzeichen" HeaderText="Bez.kennz.">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id=CheckBox2 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Bezahltkennzeichen") %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox7 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bezahltkennzeichen") %>'>
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
					<asp:TemplateColumn Visible="False" SortExpression="ZielCarport" HeaderText="ZielCarport">
						<ItemTemplate>
							<asp:TextBox id="txtZielCarport" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZielCarport") %>'>
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=TextBox6 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZielCarport") %>'>
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
							<asp:Literal id="Literal1" runat="server" Text='<%# "<input type=""hidden"" name=""Anzahl_alt_" &amp; DataBinder.Eval(Container, "DataItem.ID") &amp; """ value=""" &amp; DataBinder.Eval(Container, "DataItem.Anzahl_alt") &amp; """>" %>'>
							</asp:Literal>
							
							<asp:TextBox id="Anzahl_neu" onfocus="this.select()" runat="server" Width="50px" 
                                Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl_neu") %>' 
                                CssClass="InputRight" >
							</asp:TextBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="Textbox3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl_neu") %>'>
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
                    
					<asp:TemplateColumn>
						<HeaderTemplate>
                            <asp:ImageButton ID="ibt_All" runat="server" CommandName="All" 
                                ImageUrl="../../../Images/Confirm_Mini_red.GIF" onclick="ibt_All_Click" 
                                ToolTip="Alle ausgewählten Positionen übernehmen" />
                        </HeaderTemplate>
						<ItemTemplate>
							<asp:Literal  Visible=false id="ConfirmImageButton" runat="server" Text="<%# &quot;<a href=&quot;&quot;javascript:SetValues('&quot; &amp; DataBinder.Eval(Container, &quot;DataItem.ID&quot;) &amp; &quot;','&quot; &amp; DataBinder.Eval(Container, &quot;DataItem.Task&quot;) &amp; &quot;')&quot;&quot;><img id=&quot;&quot;Picture_&quot; &amp; DataBinder.Eval(Container, &quot;DataItem.ID&quot;) &amp; &quot;&quot;&quot; border=&quot;&quot;0&quot;&quot; src=&quot;&quot;/PortalORM/Images/Confirm_mini_Grey.gif&quot;&quot; width=&quot;&quot;20&quot;&quot; height=&quot;&quot;18&quot;&quot;></a>&quot; %>">
							</asp:Literal><asp:ImageButton ID="ImageButton1"   runat="server" CommandName="Edit" ImageUrl="../../../Images/Confirm_Mini.GIF"  />
	  					</ItemTemplate>
					</asp:TemplateColumn>                    
                    

					
				</Columns>
			</DBWC:HIERARGRID>
			</td>
	</tr>
</table>
