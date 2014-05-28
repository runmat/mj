<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EFA_Change01.aspx.vb"
    Inherits="AppKBS.EFA_Change01" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            height: 38px;
        }
    </style>
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
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (EFA-SQL-Administration)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="style1">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label><br />
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#009933"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                            <asp:LinkButton ID="lbZurueck" runat="server" CssClass="StandardButton">&#149;&nbsp;zurück</asp:LinkButton>
                        </td>
                        <td valign="top">
                            <table id="Table1" cellspacing="0" cellpadding="0" width="150" border="0">
                                <tr>
                                    <td nowrap="nowrap" style="padding-right: 5px">
                                        Suche über:
                                    </td>
                                    <td style="padding-right: 5px">
                                        <asp:DropDownList ID="ddlSearch" runat="server" Style="width: auto">
                                            <asp:ListItem Text="IP" Value="IP"> </asp:ListItem>
                                            <asp:ListItem Text="Lagerort" Value="LGORT"> </asp:ListItem>
                                            <asp:ListItem Text="WERK" Value="WERKS"> </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="padding-right: 5px">
                                        <asp:TextBox ID="txtSuche" runat="server" Width="120px" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td style="padding-right: 5px">
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                    <td style="padding-right: 5px">
                                        <asp:LinkButton ID="cmdnoSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;zurücksetzen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table5" cellspacing="0" cellpadding="0" width="150" border="0">
                                <tr>
                                    <td valign="center" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="border-top: solid,1px,black; border-bottom: solid,1px,black; border: solid,1px,black;"
                                    id="tr_KasseHinzufuegen">
                                    <td width="120px">
                                        <b>KassenIP hinzufügen</b>
                                        <br />
                                        <asp:TextBox runat="server" ID="txtNeuKasseIP" Width="120" MaxLength="15"></asp:TextBox>
                                        <asp:LinkButton ID="lbHinzufuegen" runat="server" CssClass="StandardButton">•&nbsp;hinzufügen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:GridView AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true" BorderWidth="1" BorderStyle="Solid"
                                runat="server" Width="100%" ID="gv" PageSize="100">
                                <AlternatingRowStyle BorderWidth="1" BorderStyle="solid" VerticalAlign="Middle" HorizontalAlign="Center"
                                    Font-Bold="true" Font-Size="Medium" BackColor="WhiteSmoke" />
                                <RowStyle Height="60px" Width="120px" BorderWidth="0" BorderStyle="none" VerticalAlign="Middle"
                                    HorizontalAlign="Center" Font-Bold="true" Font-Size="Medium" />
                                <HeaderStyle Font-Size="Large" Font-Bold="true" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <PagerSettings NextPageText="N&#228;chste Seite" PreviousPageText="Vorherige Seite"
                                         Position="Top" Mode="Numeric" />
                                <Columns>
                                    <asp:BoundField Visible="false" DataField="IP" ReadOnly="true" />
                                    <asp:BoundField Visible="false" DataField="CustomerID" ReadOnly="true" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="IP" CommandName="sort"
                                                Text="Kassen IP"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" MaxLength="15" ID="txtIP" Text='<%# DataBinder.Eval(Container, "DataItem.IP") %>'></asp:TextBox>
                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IP") %>'
                                                CommandName="saveIP" Text="speichern" ID="lbSaveIP"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="LGORT" CommandName="sort"
                                                Text="Lagerort"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" MaxLength="4" ID="txtLGORT" Text='<%# DataBinder.Eval(Container, "DataItem.LGORT") %>'></asp:TextBox>
                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IP") %>'
                                                CommandName="saveLGORT" Text="speichern" ID="lbSaveLGORT"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="WERKS" CommandName="sort"
                                                Text="WERK"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" MaxLength="4" ID="txtWERKS" Text='<%# DataBinder.Eval(Container, "DataItem.WERKS") %>'></asp:TextBox>
                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IP") %>'
                                                CommandName="saveWERKS" Text="speichern" ID="lbSaveWERKS"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Firma" ID="lblFirma"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlFirma" AutoPostBack="true" OnSelectedIndexChanged="ddlFirma_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Superadmin">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkMaster" Checked='<%# DataBinder.Eval(Container, "DataItem.Master") %>'
                                                runat="server" AutoPostBack="True" OnCheckedChanged="chkMaster_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Entfernen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ToolTip="Diese Kasse entfernen" ID="imgDelete" Height="14"
                                                CommandName="entfernen" Width="14" ImageUrl="../../../Images/loesch.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IP") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
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
