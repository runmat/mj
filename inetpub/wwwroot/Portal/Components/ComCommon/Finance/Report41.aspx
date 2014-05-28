<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report41.aspx.vb" Inherits="CKG.Components.ComCommon.Report41" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
        
        <asp:Label runat="server" ID="lblError2" ForeColor="Red" Font-Bold="True" 
            Font-Size="14pt" Visible="False" />

    <table id="Table4" width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center">
                                        <asp:LinkButton ID="cmddel" runat="server" CssClass="StandardButton"> &#149;&nbsp;Löschen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>                        
                        </td>
                        <td valign="top" width="100%">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge">
                                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:LinkButton>&nbsp;&nbsp;
                                                </td>
                                                <td align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelExtraLarge">
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label><asp:Label ID="lblError"
                                                        runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="false" AllowSorting="false" BackColor="White"
                                            runat="server" Width="100%" ID="GridView1" PageSize="20" AllowPaging="true">
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                                            <Columns>
                                                <asp:TemplateField SortExpression="Fahrzeugklasse" HeaderText="col_Fahrzeugklasse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrzeugklasse" runat="server" CommandArgument="Fahrzeugklasse"
                                                            CommandName="sort">col_Fahrzeugklasse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrzeugklasse") %>'
                                                            ID="lblFahrzeugklasse" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                            CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZBII" runat="server" CommandArgument="NummerZBII"
                                                            CommandName="sort">col_NummerZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'
                                                            ID="lblNummerZBII" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                            CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                            ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Eingangsdatum" HeaderText="col_Eingangsdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandArgument="Eingangsdatum"
                                                            CommandName="sort">col_Eingangsdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:d}") %>'
                                                            ID="lblEingangsdatum" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField SortExpression="Händlernummer" HeaderText="col_Händlernummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Händlernummer" runat="server" CommandArgument="Händlernummer"
                                                            CommandName="sort">col_Haendlernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Händlernummer") %>'
                                                            ID="lblHaendlernummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Loeschbar" HeaderText="col_Loeschbar">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Loeschbar" runat="server" CommandArgument="Loeschbar" CommandName="sort">col_Loeschbar</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkLoeschbar"  Visible='<%# DataBinder.Eval(Container, "DataItem.Loeschbar")= "X" %>'
                                                            runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.LoeschFlag")= "X" %>' />
                                                        <asp:Label ID="lblLoeschbar" runat="server" Text="ZBII liegt vor" Visible='<%# DataBinder.Eval(Container, "DataItem.Loeschbar")<> "X" %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
