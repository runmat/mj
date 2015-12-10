<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BestellungenSelection.aspx.vb"
    Inherits="KBS.BestellungenSelection" MasterPageFile="~/KBS.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Bestellungen"></asp:Label>&nbsp;
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="Auswahl" runat="server" style="text-align: center; border-bottom: solid 1px #DFDFDF;
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">   
                        <table width="100%">
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Bestellung_Platinen.aspx" style="font-size:medium">Bestellung Platinen und Zubehör</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Change06.aspx" style="font-size:medium">Bestellung Einzahlungsbelege</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Change04.aspx" style="font-size:medium">Bestellung Zentrallager</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Change05.aspx" style="font-size:medium">Bestellung Versicherungen</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Change01.aspx" style="font-size:medium">Bestellung EAN-Ware/Handelsware</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="ReportLetztePlatinenbestellungen.aspx" style="font-size:medium">Letzte Platinenbestellungen</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
