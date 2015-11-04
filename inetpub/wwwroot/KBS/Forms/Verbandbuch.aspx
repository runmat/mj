<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Verbandbuch.aspx.vb"
    Inherits="KBS.Verbandbuch" MasterPageFile="~/KBS.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="Verbandbuch"></asp:Label>&nbsp;
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
                                    <a href="VerbBuchErfassung.aspx" style="font-size:medium">Verbandbuch-Erfassung</a> 
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <a href="VerbBuchReport.aspx" style="font-size:medium">Verbandbuch-Report</a> 
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
