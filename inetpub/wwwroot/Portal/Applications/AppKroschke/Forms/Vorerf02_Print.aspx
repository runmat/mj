<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Vorerf02_Print.aspx.vb" Inherits="AppKroschke.Vorerf02_Print"%>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Vorerf02_Print</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table cellpadding="0" cellspacing="0" border="0" width="100%">
				<tr>
					<td>Ihre erfassten Daten am
						<asp:Label id="lblDate" Runat="server"></asp:Label></td>
					<td align="right"><asp:Image id="imgLogo" Runat="server"></asp:Image></td>
				</tr>
			</table>
			<asp:datagrid id="dataGrid" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False" PageSize="50">
				<Columns>
					<asp:BoundColumn Visible="False" DataField="id" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="id_sap" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="toDelete" HeaderText="L&#246;sch">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Kunde">
						<ItemTemplate>
							<asp:Literal id=Literal1 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.id_sap") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.kundenname") &amp; "</a>" %>'>
							</asp:Literal>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="VKkurz" HeaderText="VKkurz"></asp:BoundColumn>
					<asp:BoundColumn DataField="dienstleistung" HeaderText="Dienstleistung"></asp:BoundColumn>
					<asp:BoundColumn DataField="zulassungsdatum" HeaderText="Zul.Datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="toSave" HeaderText="ToSave"></asp:BoundColumn>
					<asp:BoundColumn DataField="haltername" HeaderText="Referenz"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Absenden">
						<ItemTemplate>
							<asp:CheckBox id="CheckBox2" runat="server" Checked="True"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="str_wunschkennz" HeaderText="Kennz."></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Status">
						<ItemTemplate>
							<asp:Label id=Label1 runat="server" Visible='<%# (Not (Typeof (DataBinder.Eval(Container, "DataItem.status")) is System.DBNull) AndAlso (DataBinder.Eval(Container, "DataItem.status")<>"Vorgang OK") AndAlso (DataBinder.Eval(Container, "DataItem.status")<>"Vorgang gelöscht")) %>' Text='<%# DataBinder.Eval(Container, "DataItem.status") %>' Font-Size="X-Small" ForeColor="Red">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid>
			<INPUT id="btnPrint" runat="server" onclick="JAVASCRIPT:window.print();return false;" type="button" value="Drucken" name="btnPrint">
			<asp:Button id="btnPrintPDF" runat="server" Text="Drucken"></asp:Button>
			<table border="0" width="100%">
				<tr>
					<td>&nbsp;<br>
						&nbsp;<br>
						&nbsp;<br>
						&nbsp;<br>
						&nbsp;<br>
						______________________________<br>
						Übernahme KCL Fahrer</td>
					<td align="right">&nbsp;<br>
						&nbsp;<br>
						&nbsp;<br>
						&nbsp;<br>
						&nbsp;<br>
						______________________________<br>
						Zurückerhalten Autohaus</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
