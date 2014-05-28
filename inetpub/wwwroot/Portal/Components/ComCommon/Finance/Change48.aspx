<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change48.aspx.vb" Inherits="CKG.Components.ComCommon.Change48" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
                <tr>
                    <td class="PageNavigation" colspan="2">
                        <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                        <asp:Label ID="lblPageTitle" runat="server">Autorisierungen</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td class="TaskTitle" valign="top" width="120">
                                    &nbsp;
                                </td>
                                <td class="TaskTitle" valign="top" colspan="2">
                                    <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TitleTask" NavigateUrl="Change03.aspx"
                                        Visible="False">Händlersuche</asp:HyperLink>&nbsp;
                                    <asp:HyperLink ID="lnkVertragssuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change03_2.aspx"
                                        Visible="False">Vertragssuche</asp:HyperLink>
                                </td>
                            </tr>
                            <tr id="trSaveButton" runat="server" visible="False">
                                <td valign="top">
                                    <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:LinkButton>
                                </td>
                                <td valign="center" align="right" width="100%">
                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>&nbsp;
                                </td>
                                <td valign="top" align="right">
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trBackbutton" runat="server" visible="False">
                                <td valign="top" width="120">
                                    <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                        border="0">
                                        <tr>
                                            <td valign="center">
                                                <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" colspan="2">
                                    <table id="Table6" cellspacing="0" cellpadding="5" width="100%" border="0">
                                        <tr>
                                            <td colspan="2">
                                                <asp:DataGrid ID="DataGrid1" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True"
                                                    AutoGenerateColumns="False" Width="100%" headerCSS="tableHeader" bodyCSS="tableBody"
                                                    CssClass="tableMain" bodyHeight="400" BackColor="White">
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="False" DataField="AppID" SortExpression="AppID" HeaderText="AppID">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="AppName" SortExpression="AppName" HeaderText="AppName">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="AppURL" SortExpression="AppURL" HeaderText="AppURL">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="AuthorizationID" SortExpression="AuthorizationID"
                                                            HeaderText="AuthorizationID"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Anwendung">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="InitializedBy" SortExpression="InitializedBy" HeaderText="Initiator">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="InitializedWhen" SortExpression="InitializedWhen" HeaderText="Angelegt"
                                                            DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CustomerReference" SortExpression="CustomerReference"
                                                            HeaderText="H&#228;ndler"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ProcessReference" SortExpression="ProcessReference" HeaderText="Weiteres&lt;br&gt;Merkmal">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ProcessReference2" SortExpression="ProcessReference2"
                                                            HeaderText="Zulassungsart*">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn SortExpression="BatchAuthorization" HeaderText="Sammel-&lt;br&gt;Autorisierung">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Auswahl">
                                                            <ItemTemplate>
                                                                <table id="Table11" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:LinkButton ID="Linkbutton1" runat="server" CssClass="StandardButtonSmall" Text="Autorisieren"
                                                                                CommandName="Autorisieren" CausesValidation="false">Autorisieren</asp:LinkButton>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:LinkButton ID="Linkbutton4" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'
                                                                                CssClass="StandardButtonStorno" Text="Löschen" CommandName="Loeschen" CausesValidation="False">Löschen</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn Visible="False" DataField="ProcessReference3" HeaderText="Merkmal2">
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                        HorizontalAlign="Center" Position="Top" CssClass="TextExtraLarge" Wrap="False"
                                                        Mode="NumericPages"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="Datagrid2" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True"
                                        AutoGenerateColumns="False" Width="100%" headerCSS="tableHeader" bodyCSS="tableBody"
                                        CssClass="tableMain" bodyHeight="400" BackColor="White">
                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                        <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="AppID" SortExpression="AppID" HeaderText="AppID">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="AppName" SortExpression="AppName" HeaderText="AppName">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="AppURL" SortExpression="AppURL" HeaderText="AppURL">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="AuthorizationID" SortExpression="AuthorizationID"
                                                HeaderText="AuthorizationID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Anwendung">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="InitializedBy" SortExpression="InitializedBy" HeaderText="Initiator">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="InitializedWhen" SortExpression="InitializedWhen" HeaderText="Angelegt"
                                                DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="CustomerReference" SortExpression="CustomerReference"
                                                HeaderText="H&#228;ndler"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="ProcessReference" SortExpression="ProcessReference" HeaderText="Weiteres&lt;br&gt;Merkmal">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Ergebnis" SortExpression="Ergebnis" HeaderText="Ergebnis">
                                            </asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                            HorizontalAlign="Center" Position="Top" CssClass="TextExtraLarge" Wrap="False"
                                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                                <td valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblLegende" runat="server" Visible="False">*Für Händlereigene Zulassung: N=Neufahrzeug, S=Selbstfahrervermietfahrzeug, V=Vorführfahrzeug</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="ShowScript" runat="server" visible="False">
                    <td>
                        <script language="Javascript">
						<!-- //
						function AutorisierenConfirm(Anwendung,Initiator,Angelegt,Haendler,Merkmal) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\n\tKfz-Kennzeichen\t" + Kennzeichen + "\t\n\tOrdernummer\t" + Ordernummer + "\t\n\tAngefordert\t" + Angefordert + "\t\n\tVersendet\t" + Versendet);
						return (Check);
						}
						//-->
                        </script>
                    </td>
                </tr>
			</table>
		</form>
	</body>
</HTML>
