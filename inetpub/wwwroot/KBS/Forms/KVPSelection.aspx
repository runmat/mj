<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="KVPSelection.aspx.vb"
    Inherits="KBS.KVPSelection" MasterPageFile="~/KBS.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="KVP"></asp:Label>&nbsp;
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
                                    <a href="KVPErfassung.aspx" style="font-size:medium">KVP-Erfassung</a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="KVPBewertung.aspx" style="font-size:medium">KVP-Bewertung</a>
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
