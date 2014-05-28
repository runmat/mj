<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report222_2.aspx.vb" Inherits="AppDCB.Report222_2" %>

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
                                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" NavigateUrl="Report01.aspx" Visible="False"
                                                            CssClass="TaskTitle">Zurück</asp:HyperLink>&nbsp;|&nbsp;
                                                        <asp:HyperLink ID="lnkExcel" runat="server" Visible="False" CssClass="TaskTitle"
                                                            Target="_blank">Excelformat</asp:HyperLink>&nbsp;
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
                        <td>
                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False"
                                PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain"
                                bodyHeight="500" AllowSorting="True" AllowPaging="True" Width="100%">
                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Kennzeichen" SortExpression="Kennzeichen"
                                        HeaderText="Kennzeichen"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Briefnumer" SortExpression="Briefnumer"
                                        HeaderText="Briefnumer"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Vertragsnummer">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Briefeingang" SortExpression="Briefeingang" HeaderText="Briefeingang">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Stillegung" SortExpression="Stillegung" HeaderText="Stillegung">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Versanddatum" SortExpression="Versanddatum" HeaderText="Versanddatum">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Versandart" SortExpression="Versandart" HeaderText="Versandart">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Plz" SortExpression="Plz" HeaderText="Plz"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Ort" SortExpression="Ort" HeaderText="Ort"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
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
    </table>
    </form>
</body>
</html>
