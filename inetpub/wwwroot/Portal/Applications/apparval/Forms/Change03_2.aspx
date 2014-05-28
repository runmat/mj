<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_2.aspx.vb" Inherits="AppARVAL.Change03_2" %>

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
    <table cellspacing="0" cellpadding="2" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" height="19" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugsuche/Fahrzeugauswahl)</asp:Label>
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
                                    <td valign="center" width="120">
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="120">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" height="81">
                                        <table class="" id="tblHalterEdit" cellspacing="0" cellpadding="5" width="100%" border="0"
                                            runat="server">
                                            <tr>
                                                <td class="StandardTableAlternate" valign="top" nowrap align="left" width="154" height="34">
                                                    <strong>Leasingvertrag-Nr.</strong>:
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" nowrap align="left" colspan="2" height="34">
                                                    <asp:TextBox ID="txtLeasingnummerErfassung" runat="server" Width=""></asp:TextBox>
                                                    <asp:TextBox ID="txtFahrgestellnr" runat="server" Width="" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" width="154">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Nach ARVAL filtern" TextAlign="Left"
                                                        Visible="False"></asp:CheckBox>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="2">
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label class="LabelExtraLarge" ID="lblNoData" runat="server" CssClass="TextError"></asp:Label>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:DropDownList ID="ddlPageSize" runat="server" Visible="False" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50"
                                            headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="250"
                                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn SortExpression="Ausgewaehlt" HeaderText="Ausw.">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbtnAuswahl" runat="server" ToolTip="Zur Auswahl markieren" CommandName="sort"
                                                            CommandArgument="Ausgewaehlt">Ausw.</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Auswahl" runat="server" Visible='<%# NOT (CStr(DataBinder.Eval(Container, "DataItem.Zulstelle")).Length=0) %>'
                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.Ausgewaehlt") %>'></asp:CheckBox>
                                                        <asp:Label ID="Label2" runat="server" Visible='<%# (CStr(DataBinder.Eval(Container, "DataItem.Zulstelle")).Length=0) %>'>unvollst.</asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="FUnterlagen" SortExpression="FUnterlagen" HeaderText="Fehl.&lt;br&gt;Unterl.*">
                                                    <ItemStyle Font-Size="XX-Small" ForeColor="Red"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="FUFlag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="EquipmentNummer" SortExpression="EquipmentNummer"
                                                    HeaderText="EquipmentNummer"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="HaendlerNummer" SortExpression="HaendlerNummer"
                                                    HeaderText="HaendlerNummer"></asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="LeasingNummer" HeaderText="LV-Nr.">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton0" runat="server" ToolTip="Leasingvertrags-Nr." CommandName="sort"
                                                            CommandArgument="LeasingNummer">LV-Nr.</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLvNr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LeasingNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="FhgstNummer" HeaderText="Fahrgestell-Nr.">
                                                    <ItemStyle Font-Size="XX-Small"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="FhgstNummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FhgstNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="DatumBriefeingang" SortExpression="DatumBriefeingang"
                                                    HeaderText="Datum&lt;br&gt;Briefeingang" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Halter" SortExpression="Halter" HeaderStyle-Width="150px"
                                                    HeaderText="Halter">
                                                    <ItemStyle Font-Size="XX-Small" Width="150px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="Standort" SortExpression="Standort" HeaderText="Standort">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="Evbnummer" HeaderText="EVB-Nummer">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evbnummer" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.Evbnummer") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Von" HeaderText="EVB von">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Von" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Von", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Bis" HeaderText="EVB bis">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Bis" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Bis", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="DatumZulassung" HeaderText="Datum&lt;br&gt;Zulassung">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="DatumZulassung" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container, "DataItem.DatumZulassung", "{0:dd.MM.yyyy}") %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Zulstelle" HeaderText="OrtsKZ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton5" runat="server" ToolTip="Ortskennzeichen" CommandName="sort"
                                                            CommandArgument="Zulstelle">OrtsKZ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulstelle") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulstelle") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Wuk01_Buchstaben" HeaderText="WK.1">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Wunschkennzeichen 1: Buchstabenkombination (max. 2-Stellig), Zahl 4-Stellig"
                                                            CommandArgument="Wuk01_Buchstaben" CommandName="sort">WK.1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Wuk01_Buchstaben" runat="server" Width="25px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk01_Buchstaben") %>'
                                                            MaxLength="2">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="Wuk01_Ziffern" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk01_Ziffern") %>'
                                                            MaxLength="4">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Wuk02_Buchstaben" HeaderText="WK.2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Wunschkennzeichen 2" CommandArgument="Wuk02_Buchstaben"
                                                            CommandName="sort">WK.2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Wuk02_Buchstaben" runat="server" Width="25px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk02_Buchstaben") %>'
                                                            MaxLength="2">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="Wuk02_Ziffern" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk02_Ziffern") %>'
                                                            MaxLength="4">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" SortExpression="Wuk03_Buchstaben" HeaderText="WK.3">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" ToolTip="Wunschkennzeichen 3" CommandArgument="Wuk03_Buchstaben"
                                                            CommandName="sort">WK.3</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Wuk03_Buchstaben" runat="server" Width="25px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk03_Buchstaben") %>'
                                                            MaxLength="2">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="Wuk03_Ziffern" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container, "DataItem.Wuk03_Ziffern") %>'
                                                            MaxLength="4">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Vorreserviert" HeaderText="Vorreserviert">
                                                    <ItemStyle Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="Vorreserviert" runat="server" TextAlign="Left" Checked='<%# (Not DataBinder.Eval(Container, "DataItem.Kennzeichenserie")) AND DataBinder.Eval(Container, "DataItem.Vorreserviert") %>'
                                                            GroupName="A1"></asp:RadioButton>
                                                        <asp:TextBox ID="Reservierungsdaten" runat="server" Width="75px" Text='<%# DataBinder.Eval(Container, "DataItem.Reservierungsdaten") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Kennzeichenserie" HeaderText="AS">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton9" runat="server" CommandArgument="Kennzeichenserie"
                                                            CommandName="sort" ToolTip="Aus Kennzeichen-Serie">AS</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="Kennzeichenserie" runat="server" GroupName="A1" Checked='<%# DataBinder.Eval(Container, "DataItem.Kennzeichenserie") AND (Not DataBinder.Eval(Container, "DataItem.Vorreserviert")) %>'>
                                                        </asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="KA">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" ToolTip="Keine Auswahl">KA</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="Radiobutton1" runat="server" GroupName="A1" Checked='<%# (Not DataBinder.Eval(Container, "DataItem.Kennzeichenserie")) AND (Not DataBinder.Eval(Container, "DataItem.Vorreserviert")) %>'>
                                                        </asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"
                                                Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120" height="17">
                            &nbsp;
                        </td>
                        <td height="18">
                            <asp:Label ID="lblLegende" runat="server" Visible="False">*(V)ollmacht, (H)andelsregistereintrag,(P)ersonalausweis,(G)ewerbeanmeldung,(E)inzugsermächtigung</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
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
