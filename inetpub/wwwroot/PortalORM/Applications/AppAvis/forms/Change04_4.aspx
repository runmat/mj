<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_4.aspx.vb" Inherits="AppAvis.Change04_4" %>

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
    <style type="text/css">
        .style2
        {
            font-size: xx-small;
        }
        .style3
        {
            width: 100%;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server"> (Regeln bearbeiten)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="0" cellspacing="0" class="style3">
                                <tr>
                                    <td width="120">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> •&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                    <td>
                                        <strong>&nbsp;</strong><asp:Label ID="lblErrMessage" runat="server" CssClass="TextError"
                                            EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                    <td>
                                        <strong>&nbsp;</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlAuswahl" runat="server">
                                <table class="style3">
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Carport:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpCarport" runat="server" AppendDataBoundItems="True" Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Hersteller-ID:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpHersteller" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                    Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Modellgruppe:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpModellgruppe" runat="server" AppendDataBoundItems="True"
                                                    Enabled="False" Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="style2">(Zum Aktivieren bitte Hersteller auswählen.)</span></strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Kraftstoff:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpKraftstoff" runat="server" AppendDataBoundItems="True" Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Navigation:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpNavi" runat="server" AppendDataBoundItems="True" Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Bereifung:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpReifenart" runat="server" AppendDataBoundItems="True" Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Aufbauart:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpAufbauart" runat="server" AppendDataBoundItems="True" Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Typ:
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:TextBox ID="txtTyp" runat="server"></asp:TextBox>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            Händlernummer:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHaendler" runat="server" MaxLength="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">
                                            &nbsp;
                                        </td>
                                        <td width="180">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <strong>
                                                <asp:DropDownList ID="drpLiefermonat" runat="server" AppendDataBoundItems="True"
                                                    Height="16px" Visible="False" Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="drpFarbe" runat="server" AppendDataBoundItems="True" Visible="False"
                                                    Width="157px">
                                                    <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </strong>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:GridView ID="grvRegeln" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                CssClass="tableMain">
                                <Columns>
                                    <asp:BoundField DataField="ID_BLOCK_RG" HeaderText="ID Regel" />
                                    <asp:TemplateField HeaderText="Carport">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CARPORT") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <strong>
                                                <asp:DropDownList ID="drpCarportGrid" runat="server" AppendDataBoundItems="True"
                                                    Width="60px">
                                                </asp:DropDownList>
                                            </strong>
                                            <asp:Label ID="lblCarport" runat="server" Text='<%# Bind("CARPORT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Farbe">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("FARBE_DE") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFarbe" runat="server" Text='<%# Bind("FARBE_DE") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpFarbeGrid" runat="server" AppendDataBoundItems="True" Width="60px">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stückzahl soll">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("ANZ_FZG") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSoll" runat="server" Text='<%# Bind("ANZ_FZG") %>' Visible="False"></asp:Label>
                                            <asp:TextBox ID="txtSoll" runat="server" Width="50px" Text='<%# Bind("ANZ_FZG") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ANZ_BER_GESP_FZG" HeaderText="Stückzahl ist" />
                                    <asp:TemplateField HeaderText="Liefermonat">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("GEPL_LIEFTERMIN") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLiefermonat" runat="server" Text='<%# Bind("GEPL_LIEFTERMIN") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpLiefermonatGrid" runat="server" AppendDataBoundItems="True"
                                                    Width="80px">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hersteller ID">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("HERST_NUMMER") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblHersteller" runat="server" Text='<%# Bind("HERST_NUMMER") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpHerstellerGrid" runat="server" AppendDataBoundItems="True"
                                                    Width="60px" AutoPostBack="True" OnSelectedIndexChanged="drpHerstellerGrid_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Modellgruppe">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("MODELLGRUPPE") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblModellgruppe" runat="server" Text='<%# Bind("MODELLGRUPPE") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpModellgruppeGrid" runat="server" AppendDataBoundItems="True"
                                                    Width="80px" Enabled="False">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Kraftstoff">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("KRAFTSTOFF") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblKraftstoff" runat="server" Text='<%# Bind("KRAFTSTOFF") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpKraftstoffGrid" runat="server" AppendDataBoundItems="True"
                                                    Width="80px">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Navigation">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("NAVIGATION") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNavi" runat="server" Text='<%# Bind("NAVIGATION") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpNaviGrid" runat="server" AppendDataBoundItems="True" Width="60px">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bereifung">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("REIFENART") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBereifung" runat="server" Text='<%# Bind("REIFENART") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpBereifungGrid" runat="server" AppendDataBoundItems="True"
                                                    Width="60px">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aufbauart">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("AUFBAUART") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAufbauart" runat="server" Text='<%# Bind("AUFBAUART") %>'></asp:Label>
                                            <strong>
                                                <asp:DropDownList ID="drpAufbauartGrid" runat="server" AppendDataBoundItems="True"
                                                    Width="80px">
                                                </asp:DropDownList>
                                            </strong>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Typ">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("TYP") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTyp" runat="server" Text='<%# Bind("TYP") %>'></asp:Label>
                                            <asp:TextBox ID="txtTyp" runat="server" Visible='<%# typeof (DataBinder.Eval(Container, "DataItem.TYP")) is System.DBNull %>'
                                                Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="LIEFERANT" HeaderText="Händler" />
                                    <asp:TemplateField HeaderText="Sperre ab">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox12" runat="server" Text='<%# Bind("DAT_SPERR_AB") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSperreAb" runat="server" Text='<%# Bind("DAT_SPERR_AB") %>'></asp:Label>
                                            <asp:TextBox ID="txtSperreAb" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.DAT_SPERR_AB" & "")="" %>'
                                                Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sperre bis">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("DAT_SPERR_BIS") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSperreBis" runat="server" Text='<%# Bind("DAT_SPERR_BIS") %>'></asp:Label>
                                            <asp:TextBox ID="txtSperreBis" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.DAT_SPERR_BIS" & "")="" %>'
                                                Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SPERRVERMERK" HeaderText="Sperrvermerk" />
                                    <asp:BoundField DataField="USER_ANL_BREG" HeaderText="Regel angelegt von" />
                                    <asp:TemplateField HeaderText="Löschen">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLoeschen" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" Visible="False" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            &nbsp;
                        </td>
                        <td valign="top">
                            &nbsp;<table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
