<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KVPErfassung.aspx.cs"
    Inherits="AppZulassungsdienst.forms.KVPErfassung" MasterPageFile="../MasterPage/App.Master" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <script src="/PortalZLD/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
        .DistanceText
        {
            padding: 3px 0px 0px 0px;
        }
        .DistanceButton
        {
            margin: 3px 0px 3px 0px;
        }
        .NoDistance td
        {
            margin: 0px;
            padding: 0px;
            border: none 0px white;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="KVP-Erfassung"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" width="100%" style="padding-top: 0">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError paddingTop"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="data" runat="server" style="text-align: center; border-bottom: solid 1px #DFDFDF;
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <div>&nbsp;</div>
                        <h2>KVP Kroschke-Vorschlags-Prozess</h2>    
                        <table width="80%" align="center" style="text-align: left">
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    Vorschlag von:
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox runat="server" ID="txtVorschlagVon" Width="200px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    Standort:
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox runat="server" ID="txtStandort" Width="200px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    Abteilung:
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox runat="server" ID="txtAbteilung" Width="200px" ReadOnly="True" Text="Zulassungsdienste"></asp:TextBox>
                                </td>
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    Funktion:
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox runat="server" ID="txtFunktion" Width="200px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    Vorgesetzter:
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox runat="server" ID="txtVorgesetzter" Width="200px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    KST:
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox runat="server" ID="txtKST" Width="200px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    Kurzbeschreibung:
                                </td>
                                <td colspan="3">
                                    <asp:TextBox runat="server" ID="txtBeschreibung" Width="556px" MaxLength="70"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="4" style="width:120px; text-align:left">
                                    Wie ist die derzeitige Situation?
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="5">
                                    <asp:TextBox runat="server" ID="txtSituation" Width="670px" TextMode="MultiLine" Rows="4" CssClass="maxlength400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="4" style="width:120px; text-align:left">
                                    Was sollte wie verändert werden?
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="5">
                                    <asp:TextBox runat="server" ID="txtVeraenderung" Width="670px" TextMode="MultiLine" Rows="4" CssClass="maxlength400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="4" style="width:120px; text-align:left">
                                    Wem entsteht welcher Vorteil?
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="5">
                                    <asp:TextBox runat="server" ID="txtVorteil" Width="670px" TextMode="MultiLine" Rows="4" CssClass="maxlength400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:LinkButton ID="btnParken" runat="server" Text="KVP Parken" OnClick="btnParken_Click" CssClass="TablebuttonLarge" Style="margin: 5px 10px 5px 10px;" Width="128px" Height="20px" />
                        <asp:LinkButton ID="btnVerwerfen" runat="server" Text="Verwerfen" OnClick="btnVerwerfen_Click" CssClass="TablebuttonLarge" Style="margin: 5px 10px 5px 10px;" Width="128px" Height="20px" Visible="False" />
                        <asp:LinkButton ID="btnSenden" runat="server" Text="Senden" OnClick="btnSenden_Click" CssClass="TablebuttonLarge" Style="margin: 5px 10px 5px 10px;" Width="128px" Height="20px" />
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="btnDummy" runat="server" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <ajaxToolkit:ModalPopupExtender ID="mpeConfirmDelete" runat="server" TargetControlID="btnDummy"
                            PopupControlID="pConfirmDelete" BackgroundCssClass="divProgress" CancelControlID="btnPanelConfirmDeleteCancel">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pConfirmDelete" runat="server" Style="overflow: auto; display: none;">
                            <table id="Table1" cellspacing="0" runat="server" bgcolor="white" cellpadding="0" style="overflow: auto;
                                min-height: 100px; min-width: 100px; border: solid 1px #646464; padding: 10px 10px 15px 0px;">
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft" style="padding-top: 0px; padding-bottom: 0px;
                                        padding-right: 15px;">
                                        Wollen Sie den Vorschlag wirklich verwerfen?
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft" align="center">
                                        <asp:LinkButton ID="btnPanelConfirmDeleteCancel" runat="server" Text="Abbruch" Height="16px"
                                            Width="78px" CssClass="Tablebutton" CausesValidation="false" OnClick="btnPanelConfirmDeleteCancel_Click" />
                                        <asp:LinkButton ID="btnPanelConfirmDeleteOK" runat="server" Text="OK" Height="16px" Width="78px"
                                            CssClass="Tablebutton" CausesValidation="false" OnClick="btnPanelConfirmDeleteOK_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div id="dataFooter" class="dataQueryFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            // MaxLength für TextAreas sicherstellen
            $('.maxlength400').keypress(function () {
                if ($(this).val().length > 399) {
                    return false;
                }
                return true;
            });
            $('.maxlength400').bind('paste', function () {
                var txt = this;
                setTimeout(function (e) {
                    if ($(txt).val().length > 400) {
                        $(txt).val($(txt).val().substr(0, 400));
                    }
                }, 100);
            });
        });
    
    </script>
</asp:Content>