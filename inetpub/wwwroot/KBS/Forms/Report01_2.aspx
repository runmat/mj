<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01_2.aspx.vb" Inherits="KBS.Report01_2"
    MasterPageFile="~/KBS.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="offene Bestellungen"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                            </h1>
                        </div>
                        <div id="TableQuery">
                            <div id="statistics" style="margin-top: 0px;">
                                <table id="tblAnzeigeVersandDaten" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bestellnummer,Lieferant:
                                        </td>
                                        <td width="100%">
                                            <asp:Label ID="lblBestellnummerLieferant" runat="server"></asp:Label>
                                            <asp:Label ID="lblBelegnummer" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <asp:UpdatePanel runat="server" ID="upWareneingang">
                            <ContentTemplate>
                                <div id="data">
                                    <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblError" runat="server" Visible="true" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                <asp:Label ID="lblNoData" runat="server" Visible="true" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView CssClass="GridView" ID="GridView1" runat="server" PageSize="50" Width="100%"
                                                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="True" ShowFooter="False"
                                                    GridLines="None">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEAN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bestellposition") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="ARTNR Lieferant" SortExpression="MaterialnummerLieferant"
                                                            DataField="MaterialnummerLieferant" />
                                                        <asp:BoundField HeaderText="EAN" DataField="EAN" SortExpression="EAN" />
                                                        <asp:BoundField HeaderText="Artikelbezeichnung" SortExpression="Artikelbezeichnung"
                                                            DataField="Artikelbezeichnung" />
                                                        <asp:BoundField HeaderText="Bestellmenge" HeaderStyle-Width="80" SortExpression="BestellteMenge"
                                                            DataField="BestellteMenge" />
                                                        <asp:BoundField HeaderText="Mengeneinheit" DataField="Mengeneinheit" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
