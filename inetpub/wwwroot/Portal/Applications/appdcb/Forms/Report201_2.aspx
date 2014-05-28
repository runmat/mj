<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report201_2.aspx.vb" Inherits="AppDCB.Report201_2" %>

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
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" colspan="2">
                                        <asp:HyperLink ID="lnkExcel" runat="server" Target="_blank" Visible="False">Excelformat</asp:HyperLink>&nbsp;&nbsp;
                                        <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:Label><asp:HyperLink
                                            ID="lnkKreditlimit" runat="server" Target="_blank" Visible="False">Zusammenstellung von Abfragekriterien</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:LinkButton ID="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:LinkButton>
                                    </td>
                                    <tr>
                                        <td class="" width="100%" colspan="1">
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td class="LabelExtraLarge" align="right">
                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Width="100%"
                                                AllowPaging="True" AllowSorting="True" bodyHeight="300" CssClass="tableMain"
                                                bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="Eingang Carportliste" SortExpression="Eingang Carportliste"
                                                        HeaderText="Eingang&lt;br&gt;Carportliste">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Kfz-Kennzeichen" SortExpression="Kfz-Kennzeichen" HeaderText="Kfz-Kennzeichen">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Anzahl Schilder" SortExpression="Anzahl Schilder" HeaderText="Anzahl&lt;br&gt;Schilder">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn SortExpression="Anzahl Schilder" HeaderText="Form.">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Anzahl Schilder")<>"2" %>'
                                                                CssClass="StandardButtonTable" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Kfz-Kennzeichen") %>'
                                                                Text="Zeigen" CausesValidation="False" CommandName="Schilder">&#149;&nbsp;Zeigen</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Eingang Physisch" SortExpression="Eingang Physisch" HeaderText="Eingang&lt;br&gt;physisch"
                                                        DataFormatString="{0:dd.MM.yy}">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Kfz-Schein" SortExpression="Kfz-Schein" HeaderText="Kfz-Schein">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn SortExpression="Kfz-Schein" HeaderText="Form.">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" Visible='<%# NOT DataBinder.Eval(Container, "DataItem.Kfz-Schein") IS NOTHING AndAlso NOT (CStr(DataBinder.Eval(Container, "DataItem.Kfz-Schein"))="X") %>'
                                                                CssClass="StandardButtonTable" CommandName="Schein" CausesValidation="False"
                                                                Text="Zeige" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Kfz-Kennzeichen") %>'>&#149;&nbsp;Zeigen</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Kfz Brief" SortExpression="Kfz Brief" HeaderText="Kfz Brief">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                            </table>
                            <asp:Label ID="lblInfo" runat="server" Font-Bold="True" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ShowScript" runat="server" visible="False">
            <td>
                <script language="Javascript">
						<!--                    //
                    function FreigebenConfirm(Fahrgest, Vertrag, BriefNr, Kennzeichen) {
                        var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
                        return (Check);
                    }
						//-->
                </script>
            </td>
        </tr>
    </table>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </form>
</body>
</html>
