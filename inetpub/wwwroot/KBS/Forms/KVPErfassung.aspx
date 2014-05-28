<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="KVPErfassung.aspx.vb"
    Inherits="KBS.KVPErfassung" MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                            <asp:Label ID="lblHead" runat="server" Text="KVP-Erfassung"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery" runat="server" style="border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="txtBedienerkarte" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="CardScann">
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table id="tblBedienerkarte" runat="server" cellspacing="0" width="100%" cellpadding="0"
                                    bgcolor="white" border="0" style="border-bottom: solid 1px #DFDFDF; border-right: solid 1px #DFDFDF;
                                    border-left: solid 1px #DFDFDF;">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td align="center" class="firstLeft active">
                                            <asp:Label ID="lblBedienError" runat="server" CssClass="TextError">
                                                    Bitte scannen Sie ihre Bedienerkarte.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td id="Usercard" runat="server" align="center" class="firstLeft" style="padding-top: 10px;
                                            padding-bottom: 5px;">
                                            <asp:TextBox ID="txtBedienerkarte" Width="240px" runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            <input id="SendTopSap" type="hidden" runat="server" />&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <script type="text/javascript" language="javascript">

                                    function ControlField(control1) {

                                        if (control1.value.length == 15) {
                                            if (control1.value.substring(control1.value.length - 1) == '}') {
                                                theForm.__EVENTTARGET.value = '__Page';
                                                theForm.__EVENTARGUMENT.value = 'MyCustomArgument';
                                                theForm.submit();
                                                var hiddenInput = document.getElementById("ctl00_ContentPlaceHolder1_SendTopSap");
                                                hiddenInput.value = 1;
                                            } else {
                                                control1.focus();
                                            }
                                        } else {
                                            control1.focus();
                                        }
                                    }     
                                    
                                </script>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="Erfassung" runat="server" style="text-align: center; border-bottom: solid 1px #DFDFDF;
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <div>&nbsp;</div>
                        <h2>KVP Kroschke-Vorschlags-Prozess</h2>    
                        <table width="80%" align="center">
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
                                    <asp:TextBox runat="server" ID="txtAbteilung" Width="200px" ReadOnly="True" Text="Filialen"></asp:TextBox>
                                </td>
                                <td class="firstLeft active" style="width:120px; text-align:left">
                                    Funktion:
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox runat="server" ID="txtFunktion" Width="200px" ReadOnly="True" Text="Filialmitarbeiter"></asp:TextBox>
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
                                    <asp:TextBox runat="server" ID="txtBeschreibung" Width="550px" MaxLength="70"></asp:TextBox>
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
                                    <asp:TextBox runat="server" ID="txtSituation" Width="95%" TextMode="MultiLine" Rows="4" CssClass="maxlength400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="4" style="width:120px; text-align:left">
                                    Was sollte wie verändert werden?
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="5">
                                    <asp:TextBox runat="server" ID="txtVeraenderung" Width="95%" TextMode="MultiLine" Rows="4" CssClass="maxlength400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="4" style="width:120px; text-align:left">
                                    Wem entsteht welcher Vorteil?
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="5">
                                    <asp:TextBox runat="server" ID="txtVorteil" Width="95%" TextMode="MultiLine" Rows="4" CssClass="maxlength400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnParken" runat="server" Text="KVP Parken" CssClass="ButtonTouch" Style="margin: 5px 10px 5px 10px;" Width="120px" />
                        <asp:Button ID="btnVerwerfen" runat="server" Text="Verwerfen" CssClass="ButtonTouch" Style="margin: 5px 10px 5px 10px;" Width="120px" Visible="False" />
                        <asp:Button ID="btnSenden" runat="server" Text="Senden" CssClass="ButtonTouch" Style="margin: 5px 10px 5px 10px;" Width="120px" />
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="btnDummy" runat="server" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <cc1:ModalPopupExtender ID="mpeConfirmDelete" runat="server" TargetControlID="btnDummy"
                            PopupControlID="pConfirmDelete" BackgroundCssClass="divProgress" CancelControlID="btnPanelConfirmDeleteCancel">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pConfirmDelete" runat="server" Style="overflow: auto; display: none;">
                            <table cellspacing="0" runat="server" bgcolor="white" cellpadding="0" style="overflow: auto;
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
                                            Width="78px" CssClass="Tablebutton" CausesValidation="false" />
                                        <asp:LinkButton ID="btnPanelConfirmDeleteOK" runat="server" Text="OK" Height="16px" Width="78px"
                                            CssClass="Tablebutton" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div id="dataFooter">
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
