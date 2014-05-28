<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_1.aspx.vb" Inherits="AppBPLG.Change01_1" %>

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
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" colspan="2">
                                        <asp:LinkButton ID="lb_Back" runat="server">lb_Back</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:LinkButton ID="cmdsave" runat="server" CssClass="StandardButton">&#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                    <tr>
                                        <td align="left" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                        </td>
                                    </tr>
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
                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                                AllowPaging="True" Width="100%" bodyHeight="400" CssClass="tableMain" bodyCSS="tableBody"
                                                headerCSS="tableHeader" PageSize="50">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="EQUNR" Visible="false"></asp:BoundColumn>
                                                    <asp:TemplateColumn SortExpression="Halter" HeaderText="col_Halter">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Haendler" runat="server" CommandName="Sort" CommandArgument="Haendler">col_Haendler</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label7" runat="server" NAME="Label4" Text='<%# DataBinder.Eval(Container, "DataItem.Haendler") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label4" runat="server" NAME="Label4" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Modell" HeaderText="col_Modell">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Modell" runat="server" CommandName="Sort" CommandArgument="Modell">col_Modell</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Width="55px" NAME="Label2" Text='<%# DataBinder.Eval(Container, "DataItem.Modell") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Nummer ZB2" HeaderText="col_NummerZB2">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="Nummer ZB2">col_NummerZB2</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Nummer ZB2") %>'
                                                                ID="Label3" NAME="Label3">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label5" runat="server" NAME="Label5" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Eingangsdatum" HeaderText="col_Eingangsdatum">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandArgument="Eingangsdatum"
                                                                CommandName="Sort">col_Eingangsdatum</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label6" runat="server" NAME="Label8" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:d}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="col_Lizenznr">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Lizenznr" runat="server" CommandArgument="Lizenznr" CommandName="Sort">col_Lizenznr</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_Lizenznr" BorderColor="Red" BorderStyle="Solid" BorderWidth="1"
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Lizenznr") %>' Enabled='<%# DataBinder.Eval(Container, "DataItem.Lizenznr") ="" %>'
                                                                runat="server" Width="85px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="col_HaendlerNR">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_HaendlerNR" runat="server" CommandArgument="HaendlerNR" CommandName="Sort">col_HaendlerNR</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_HaendlerNR" BorderColor="Red" BorderStyle="Solid" BorderWidth="1"
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.HaendlerNR")%>' Enabled='<%# DataBinder.Eval(Container, "DataItem.HaendlerNR") ="" %>'
                                                                runat="server" Width="85px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="col_EndkundenNummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_EndkundenNummer" runat="server" CommandArgument="EndkundenNummer"
                                                                CommandName="Sort">col_EndkundenNummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_EndkundenNummer" BorderColor="Red" BorderStyle="Solid" BorderWidth="1"
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.EndkundenNummer")%>' Enabled='<%# DataBinder.Eval(Container, "DataItem.EndkundenNummer")="" %>'
                                                                runat="server" Width="85px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="col_Branding">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Branding" runat="server" CommandArgument="Branding" CommandName="Sort">col_Branding</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DropDownList ForeColor="Red" Visible='<%#  (DataBinder.Eval(Container, "DataItem.Branding")="")=True  %>'
                                                                ID="cmbBrandings" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:Label ForeColor="Red" Text='<%# getBranding(DataBinder.Eval(Container, "DataItem.Branding"))%>'
                                                                ID="lblBranding" Visible='<%# not DataBinder.Eval(Container, "DataItem.Branding")="" %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Status" SortExpression="Status" HeaderText="Status">
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                    PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False" Mode="NumericPages"
                                                    Position="Top"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                            </table>
                            <asp:Label ID="lbl_Info" runat="server" Font-Bold="True" EnableViewState="False"></asp:Label>
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
                            <!--#include File="../../../PageElements/Footer.html" -->
                            <asp:Label ID="lblHidden" runat="server" Visible="False" Width="39px"></asp:Label>
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
    </form>
</body>
</html>
