<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report013.aspx.vb" Inherits="CKG.Components.ComCommon.Report013" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:header id="ucHeader" runat="server" Visible="False"></uc1:header>
			<asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" bodyHeight="400" cssclass="" bodyCSS="" headerCSS="" PageSize="50" BackColor="White" Width="100%">
				<AlternatingItemStyle CssClass=""></AlternatingItemStyle>
				<HeaderStyle Wrap="False" CssClass=""></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Ihre Referenz" SortExpression="Ihre Referenz" HeaderText="Ihre Referenz"></asp:BoundColumn>
					<asp:BoundColumn DataField="Unsere Auftrags-Nr" SortExpression="Unsere Auftrags-Nr" HeaderText="Unsere&lt;br&gt;Auftrags-Nr."></asp:BoundColumn>
					<asp:BoundColumn DataField="Auftragseingang" SortExpression="Auftragseingang" HeaderText="Auftragseingang"></asp:BoundColumn>
					<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
					<asp:BoundColumn DataField="Hersteller/Typ" SortExpression="Hersteller/Typ" HeaderText="Hersteller/Typ"></asp:BoundColumn>
					<asp:BoundColumn DataField="Abgabedatum" SortExpression="Abgabedatum" HeaderText="Abgabedatum"></asp:BoundColumn>
					<asp:BoundColumn DataField="Abholort" SortExpression="Abholort" HeaderText="Abholort"></asp:BoundColumn>
					<asp:BoundColumn DataField="Anlieferort" SortExpression="Anlieferort" HeaderText="Anlieferort"></asp:BoundColumn>
					<asp:BoundColumn DataField="Km" SortExpression="Km" HeaderText="Km"></asp:BoundColumn>
				</Columns>
				<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
			<asp:Label id="lblError" runat="server"></asp:Label>
		</form>
	</body>
</HTML>
