<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change81_4.aspx.cs" Inherits="Leasing.forms.Change81_4"
    MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="step1" runat="server">Fahrzeugsuche</asp:HyperLink>
                    <asp:HyperLink ID="step2" runat="server">| Fahrzeugauswahl</asp:HyperLink>
                    <asp:HyperLink ID="step3" runat="server">| Adressen</asp:HyperLink>
                    <a class="active">| Absenden</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSummary" />
                            </Triggers>
                            <ContentTemplate>
                                <div id="TableQuery">
                                    <asp:Panel ID="Panel1" runat="server" Style="display: block;">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Blue"></asp:Label>
                                                    <asp:ImageButton ID="btnSummary" runat="server" ImageUrl="../../../Images/iconPDF.gif"
                                                        ToolTip="Zusammenfassung herunterladen" Visible="false" OnClick="DownloadSummary" />
                                                    <asp:Label ID="lblError" EnableViewState="False" runat="server" CssClass="TextError"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlHalter" runat="server" Style="display: block">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr runat="server" id="tr_Dienstleistung" class="formquery">
                                                <td class="firstLeft active" style="height: 22px; font-size: 16px">
                                                    Dienstleistung:
                                                </td>
                                                <td class="active" colspan="2" style="height: 22px; font-size: 16px">
                                                    <asp:Label ID="lblBeauftragteDienstleistungAnzeige" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                    <span style="font-weight: bold">Halter:</span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="width: 10%">
                                                    Adresse:&nbsp;
                                                </td>
                                                <td class="active" style="width: 90%">
                                                    <asp:Label ID="lblAdresseName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblAdresseStrasseNr" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblAdressePLZOrt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlZulDaten" runat="server" Style="display: block">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="5" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                    <span style="font-weight: bold">Zulassungsdaten:</span>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trWunschkennzeichen" runat="Server">
                                                <td class="firstLeft active" style="height: 19px">
                                                    Wunschkennzeichen
                                                </td>
                                                <td class="active" style="width: 100%; height: 19px;" colspan="4">
                                                    <asp:Label ID="lblKreis" runat="server"></asp:Label>&nbsp;-&nbsp;
                                                    <asp:Label ID="lblWunschkennzeichen" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trReserviertAuf" runat="Server">
                                                <td class="firstLeft active" nowrap="nowrap">
                                                    reserviert auf
                                                </td>
                                                <td class="active" colspan="4">
                                                    <asp:Label ID="lblReserviertAuf" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trVersicherungstr" runat="Server">
                                                <td class="firstLeft active" nowrap="nowrap">
                                                    Versicherungsträger:
                                                </td>
                                                <td class="active" colspan="4">
                                                    <asp:Label ID="lblVersicherungstraeger" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trEvbNr" runat="Server">
                                                <td class="firstLeft active" nowrap="nowrap">
                                                    eVB-Nummer:
                                                </td>
                                                <td class="active" colspan="3">
                                                    <asp:Label ID="lblEVB" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" nowrap="nowrap" style="height: 19px">
                                                    Gültigkeit:
                                                </td>
                                                <td class="active" nowrap="nowrap">
                                                    &nbsp;gültig von&nbsp;
                                                </td>
                                                <td class="active" style="height: 19px">
                                                    <asp:Label ID="lblDatumVON" runat="server"></asp:Label>
                                                </td>
                                                <td class="active" nowrap="nowrap" style="height: 19px">
                                                    &nbsp;bis&nbsp;
                                                </td>
                                                <td class="active" style="width: 100%; height: 19px;">
                                                    <asp:Label ID="lblDatumBis" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEmpfaenger" runat="server" Style="display: block">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                    <span style="font-weight: bold">Empfänger Schein/Schilder:</span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label8" runat="server">Adresse:&nbsp;</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblAdresseNameEmpf" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblAdresseStrasseNrEmpf" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblAdressePLZOrtEmpf" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSonstiges" runat="server" Style="display: block">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px;
                                                    width: 100%">
                                                    <span style="font-weight: bold">Sonstiges:</span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label2" runat="server">gew. Durchführungsdatum:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblDurchfuehrungsDatum" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trHinweis" runat="server">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label19" runat="server">Hinweis:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblHinweis" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="vertical-align: top">
                                                    <asp:Label ID="Label29" runat="server" Width="106px">Bemerkung:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblBemerkung" runat="server" Width="374px" Height="71px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                                <div id="Result" runat="Server">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            &nbsp;</div>
                                    </div>
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                            OnSorting="GridView1_Sorting">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EditRowStyle></EditRowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="MANDT" SortExpression="MANDT" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMANDT" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MANDT") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Leasingnummer" HeaderText="col_Leasingnummer">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingnummer" runat="server" CommandName="Sort" CommandArgument="Leasingnummer">col_Leasingnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLeasingnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NummerZB2" HeaderText="col_NummerZB2">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="NummerZB2">col_NummerZB2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblNummerZB2" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZB2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="STATUS" HeaderText="col_Status">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STATUS") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div id="dataFooter">
                                        <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" Width="78px"
                                            OnClick="cmdContinue_Click">» Absenden</asp:LinkButton>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
