<%@ Control Language="vb" AutoEventWireup="false" Codebehind="StandardLog.ascx.vb" Inherits="CKG.Portal.PageElements.StandardLog" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
		<TD class="PageNavigation" colSpan="2">
			<asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
			<asp:label id="lblPageTitle" runat="server">Datenanzeige</asp:label>)</TD>
	</TR>
	<TR>
		<TD vAlign="top" width="92" style="WIDTH: 92px">
			<TABLE id="Table5" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
				<TR>
					<TD class="TaskTitle">&nbsp;
					</TD>
				</TR>
				<TR>
					<TD vAlign="center">
						<asp:radiobutton id="chkSession" runat="server" Text="Aktuelle Sitzung" GroupName="SessionOrUser" Checked="True" AutoPostBack="True"></asp:radiobutton></TD>
				</TR>
				<TR>
					<TD vAlign="center" width="150">
						<asp:radiobutton id="chkUser" runat="server" Text="Datumsauswahl" GroupName="SessionOrUser" AutoPostBack="True"></asp:radiobutton></TD>
				</TR>
				<TR>
					<TD vAlign="center" width="150"></TD>
				</TR>
			</TABLE>
			<asp:calendar id="Calendar1" runat="server" Visible="False" Width="32px" CellPadding="1" Height="72px">
				<DayHeaderStyle CssClass="GridTableHead"></DayHeaderStyle>
				<TitleStyle Font-Bold="True"></TitleStyle>
			</asp:calendar>
		</TD>
		<TD vAlign="top">
			<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
				</TR>
			</TABLE>
			<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD class="LabelExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label>
						<asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False">Sichern</asp:linkbutton></TD>
				</TR>
				<TR>
					<TD class="LabelExtraLarge"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" CellPadding="0" AllowPaging="True" AutoGenerateColumns="False" Width="100%" BackColor="White">
							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
							<HeaderStyle CssClass="GridTableHead"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImageButton1" runat="server"></asp:ImageButton>
										<asp:CheckBox id="CheckBox1" runat="server" Visible="False"></asp:CheckBox>
										<asp:Label id=lblID runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Inserted" SortExpression="Inserted" HeaderText="Angelegt"></asp:BoundColumn>
								<asp:BoundColumn DataField="Task" SortExpression="Task" HeaderText="Anwendung"></asp:BoundColumn>
								<asp:TemplateColumn SortExpression="Description" HeaderText="Beschreibung">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'>
										</asp:Label>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'>
										</asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;" Position="Top"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<TR>
		<TD style="WIDTH: 92px">&nbsp;</TD>
		<TD vAlign="top" align="left"><asp:label id="lblError" runat="server"></asp:label></TD>
	</TR>
	<TR>
		<TD style="WIDTH: 92px">&nbsp;</TD>
		<TD vAlign="top" align="left"><!--#include File="Footer.html" --></TD>
	</TR>
</TABLE>
