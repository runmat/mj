<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report111_2.aspx.vb" Inherits="AppDCB.Report111_2" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Anzeige Report)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" colspan="3">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tbody>
                                                <tr>
                                                    <td class="TaskTitle">
                                                        <asp:HyperLink ID="lnkExcel" runat="server" Target="_blank" Visible="False" CssClass="TaskTitle">Excelformat</asp:HyperLink>&nbsp;
                                                        <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="">
                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                        </td>
                        <td align="right" colspan="2">
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
                <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False"
                    PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain"
                    bodyHeight="400" AllowSorting="True" AllowPaging="True" Width="100%">
                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                    <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Label ID="lblInfo" runat="server" Visible='<%# not (typeof (DataBinder.Eval(Container, "DataItem.Langtext")) is System.DBNull) AndAlso (DataBinder.Eval(Container, "DataItem.Langtext")<>String.Empty) %>'
                                    Text="<%# &quot;&amp;nbsp;<b><A href='javascript:showhide(&quot;&quot;&quot; &amp; DataBinder.Eval(Container, &quot;DataItem.Fahrgestellnummer&quot;) &amp; &quot;&quot;&quot;)'>Bemerkungen...</A></b>&quot; %>"
                                    ToolTip="Bemerkungen ein-/ausblenden">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer"
                            HeaderText="Fahrgestellnummer"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Briefnummer">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Vertragsnummer">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Eingangsdatum" SortExpression="Eingangsdatum" HeaderText="Eingangsdatum">
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Langtext" SortExpression="Langtext" HeaderText="Langtext">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Versandgrund" SortExpression="Versandgrund" HeaderText="Versandgrund">
                        </asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Literal ID="Literal2" runat="server" Visible='<%# not (typeof (DataBinder.Eval(Container, "DataItem.Langtext")) is System.DBNull) AndAlso (DataBinder.Eval(Container, "DataItem.Langtext")<>String.Empty) %>'
                                    Text='<%# "<tr id=""" &amp; DataBinder.Eval(Container, "DataItem.Fahrgestellnummer")  &amp; """ style=""DISPLAY:none""><td></td><td colspan=""6"" class=""Banner"">" &amp; DataBinder.Eval(Container, "DataItem.Langtext") &amp; "</td></tr>" %>'>
                                </asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                        HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <script language="JavaScript">										
						<!--
            function showhide(tRow) {
                o = document.getElementById(tRow).style;
                if (o.display != "none") {
                    o.display = "none";
                } else {
                    o.display = "";
                }
            }															
						-->
        </script>
    </table>
    </form>
</body>
</html>
