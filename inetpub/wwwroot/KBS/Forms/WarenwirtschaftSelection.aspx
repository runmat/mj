<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WarenwirtschaftSelection.aspx.vb"
    Inherits="KBS.WarenwirtschaftSelection" MasterPageFile="~/KBS.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="Warenwirtschaft"></asp:Label>&nbsp;
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
                                    <a href="Change02.aspx" style="font-size:medium">Wareneingangsprüfung</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Change07.aspx" style="font-size:medium">Umlagerung</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Report01.aspx" style="font-size:medium">offene Bestellungen</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="Change12.aspx" style="font-size:medium">Retoure zum Lieferanten</a>
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
