<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>--%>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_4.aspx.vb" Inherits="AppARVAL.Change03_4" %>

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
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td colspan="3">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" width="150" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Bestätigung)</asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="120">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:HyperLink ID="btnPrint" runat="server" CssClass="StandardButton" NavigateUrl="Change03_5.aspx"
                                            Target="_blank" Enabled="False" Visible="False"> &#149;&nbsp;Druckansicht</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                        <asp:HyperLink ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change03_2.aspx"
                                            CssClass="TaskTitle">Fahrzeugsuche/Fahrzeugauswahl</asp:HyperLink>&nbsp;
                                       <%-- <asp:HyperLink ID="lnkAdressAuswahl" runat="server" NavigateUrl="Change03_3.aspx"
                                            CssClass="TaskTitle" Visible="False">Fahrzeugliste</asp:HyperLink>--%>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge" valign="top" align="left" colspan="5">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelExtraLarge" valign="top" align="left" colspan="5">
                                                    <u>Sie haben folgenden&nbsp;Zulassungsauftrag erstellt</u>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top" align="left" colspan="5">
                                                    <asp:Label ID="Label3" runat="server">Versicherung (optional):&nbsp;</asp:Label>
                                                    <asp:TextBox ID="txtVersicherung" runat="server" Width="250px" MaxLength="125"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="3">
                                        <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            Width="100%" bodyHeight="300" CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader"
                                            BackColor="White" PageSize="50" AllowPaging="True" BorderWidth="0px" BorderColor="Transparent">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="EquipmentNummer" SortExpression="EquipmentNummer"
                                                    HeaderText="Equipment"></asp:BoundColumn>
                                                <asp:TemplateColumn Visible="False" SortExpression="Ausgewaehlt" HeaderText="Auswahl">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="VA">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="FUnterlagen" SortExpression="FUnterlagen" HeaderText="Fehl.&lt;br&gt;Unterl.">
                                                    <ItemStyle ForeColor="Red"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="LeasingNummer" HeaderText="LV-Nr.">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Leasingvertrags-Nr." CommandArgument="LeasingNummer"
                                                            CommandName="sort">LV-Nr.</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label99" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LeasingNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Halter" SortExpression="Halter" HeaderText="Halter">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="FhgstNummer" HeaderText="Fahrgestellnr.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FhgstNummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FhgstNummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evbnummer" HeaderText="EVB-Nummer">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evbnummer" runat="server" Enabled="False" Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.Evbnummer") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Von" HeaderText="EVB von">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Von" runat="server" Enabled="False" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Von", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Evb_Bis" HeaderText="EVB bis">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Evb_Bis" runat="server" Enabled="False" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.Evb_Bis", "{0:dd.MM.yyyy}" ) %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="DatumZulassung" HeaderText="Zulassungsdat.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="DatumZulassung" runat="server" Enabled="False" Width="80px" Text='<%# DataBinder.Eval(Container, "DataItem.DatumZulassung", "{0:dd.MM.yyyy}") %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Wunschkennzeichen" HeaderStyle-Width="150px"
                                                    HeaderText="Wunschkennzeichen">
                                                    <HeaderStyle Width="150px"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label222" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Vorreserviert" HeaderText="Vorreserviert">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Vorreserviert" runat="server" Enabled="False" Checked='<%# (Not DataBinder.Eval(Container, "DataItem.Kennzeichenserie")) AND DataBinder.Eval(Container, "DataItem.Vorreserviert") %>'>
                                                        </asp:CheckBox>&nbsp;
                                                        <asp:TextBox ID="Reservierungsdaten" runat="server" Visible="False" Enabled="False"
                                                            Width="75px" Text='<%# DataBinder.Eval(Container, "DataItem.Reservierungsdaten") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Kennzeichenserie" HeaderText="AS">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Kennzeichenserie" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Kennzeichenserie") AND (Not DataBinder.Eval(Container, "DataItem.Vorreserviert")) %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Zulstelle" SortExpression="Zulstelle" HeaderText="OrtsKZ">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="StandardButtonTable" CommandName="delete">Verwerfen</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"
                                                Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <!--#include File="../../../PageElements/Footer.html" -->
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
