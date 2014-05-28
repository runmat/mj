<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Kopfdaten.ascx.vb" Inherits="AppF1.Kopfdaten"  %>
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
				<ItemStyle CssClass="TextLarge" HorizontalAlign="Center"></ItemStyle>
				<HeaderStyle HorizontalAlign=Center Wrap="False" CssClass="GridTableHead"></HeaderStyle>
				<Columns>
				
				<asp:BoundColumn DataField="RECART" HeaderStyle-HorizontalAlign=Left ItemStyle-HorizontalAlign="Left" HeaderText="Kontingentart" ></asp:BoundColumn>
				<asp:BoundColumn DataField="KLIMK" DataFormatString="{0:N0}" HeaderText="Gesamtkontingent" ></asp:BoundColumn>
				<asp:BoundColumn DataField="SKFOR" DataFormatString="{0:N0}" HeaderText="Inanspruchnahme" ></asp:BoundColumn>
				<asp:BoundColumn DataField="FREIKONTI" DataFormatString="{0:N0}" HeaderText="Frei" ></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Gesperrt" >
				<ItemTemplate>
				<asp:CheckBox runat="server" ID="chkGesperrt" Enabled="false" Checked='<%# DataBinder.Eval(Container, "DataItem.SPEERKZ")="X" %>' />
				</ItemTemplate>
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
