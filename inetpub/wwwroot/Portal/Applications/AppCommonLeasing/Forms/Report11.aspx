<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Report11.aspx.vb" Inherits="AppCommonLeasing.Report11" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body style="margin-left: 0px; margin-top: 0;" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server" />&nbsp;<asp:Label ID="lbl_PageTitle" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" colspan="2">
                                <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server" OnClick="NavigateBack">lb_zurueck</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 140px">
                                <asp:LinkButton ID="cmdWeiter" Height="18px" Width="120px" runat="server"  OnClientClick="Show_BusyBox1();" OnClick="CreateReport"
                                    CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:LinkButton>
                            </td>
                            <td valign="top">
                                <%-- Main Content Area.. --%>
                                <table id="TableKleinerAbstandVorGrid" runat="server" visible="false" cellspacing="0"
                                    cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td style="padding-left: 15px">
                                            <asp:Label ID="lblInfo" Font-Bold="true" runat="server" />
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                                Visible="True" Width="20px" OnClick="Export" />
                                            &nbsp;Ergebnisse/Seite:&nbsp;
                                            <asp:DropDownList ID="pageSize" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="PageSizeChanged">
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="50" Value="50" Selected="True" />
                                                <asp:ListItem Text="100" Value="100" />
                                                <asp:ListItem Text="200" Value="200" />
                                                <asp:ListItem Text="500" Value="500" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-left: 15px">
                                            <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" OnSortCommand="SortChanged" OnPageIndexChanged="PageIndexChanged">
                                                <AlternatingItemStyle CssClass="GridTableAlternate" />
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="col_Fahrgestellnummer" SortExpression="CHASSIS_NUM">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("CHASSIS_NUM") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="col_Kennzeichen" SortExpression="LICENSE_NUM">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("LICENSE_NUM") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="col_Vertragsnummer" SortExpression="LIZNR">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("LIZNR") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="col_Abmeldedatum" SortExpression="EXPIRY_DATE">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="EXPIRY_DATE">col_Abmeldedatum</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("EXPIRY_DATE","{0:dd.MM.yyyy}") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages" />
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                                <p>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False" />&nbsp;
                                    <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError" />
                                </p>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="left">
                    <!--#include File="../../../PageElements/Footer.html" -->
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
