<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Zeiterfassung.aspx.vb"
    Inherits="KBS.Zeiterfassung" MasterPageFile="~/KBS.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="Zeiterfassung"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery" style="white-space: nowrap;">
                        <div style="padding: 3px 8px 3px 10px; font-weight: bold; text-align: left; overflow: visible;
                            white-space: nowrap; width: 30%; float: left;">
                            <asp:Label ID="lblUsername" runat="server"></asp:Label>
                        </div>
                        <div style="padding: 3px 8px 3px 10px; text-align: right; overflow: visible; white-space: nowrap;
                            width: 20%; float: right;">
                            <asp:Timer ID="timServerzeit" runat="server" Interval="30000" OnTick="timServerzeit_Tick">
                            </asp:Timer>
                            <asp:UpdatePanel ID="udpServerzeit" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="timServerzeit" EventName="Tick" />
                                </Triggers>
                                <ContentTemplate>
                                    Serverzeit:
                                    <asp:Label ID="lblServerzeit" runat="server">00:00</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div id="TableQuery" runat="server" style="border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="txtBedienerkarte" />
                            </Triggers>
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="CardScann" style="border-bottom: solid 1px #DFDFDF; border-right: solid 1px #DFDFDF;
                        border-left: solid 1px #DFDFDF;">
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table id="tblBedienerkarte" runat="server" cellspacing="0" width="100%" cellpadding="0"
                                    bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td align="center" class="firstLeft active">
                                            <asp:Label ID="lblBedienError" runat="server" CssClass="TextError">
                                                    Bitte Scannen Sie ihre Bedienerkarte.</asp:Label>
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

                                        if (control1.value.length == 15)
                                            if (control1.value.substring(control1.value.length - 1) == '}') {
                                                theForm.__EVENTTARGET.value = '__Page';
                                                theForm.__EVENTARGUMENT.value = 'MyCustomArgument';
                                                theForm.submit();
                                                var hiddenInput = document.getElementById("ctl00_ContentPlaceHolder1_SendTopSap");
                                                hiddenInput.value = 1;
                                            }
                                            else {
                                                control1.focus();
                                            }
                                        else
                                            control1.focus();
                                    }                                

                                </script>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="Zeiterfassung" runat="server" style="text-align: center; border-bottom: solid 1px #DFDFDF;
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <div>
                            &nbsp;</div>
                        <div>
                            Erfassen Sie ihre Zeit mit einem Klick.</div>
                        <div>
                            &nbsp;</div>
                        <asp:Button ID="btnKommen" runat="server" Text="Kommen" CssClass="ButtonTouch" Style="margin: 5px 10px 5px 10px;
                            background-color: #94D639;" />
                        <asp:Button ID="btnGehen" runat="server" Text="Gehen" CssClass="ButtonTouch" Style="margin: 5px 10px 5px 10px;
                            background-color: #FF3128;" />
                        <div>
                            &nbsp;</div>
                    </div>
                    <div id="Options" runat="server" style="border-bottom: solid 1px #DFDFDF; border-right: solid 1px #DFDFDF;
                        border-left: solid 1px #DFDFDF;">
                        <div style="background-color: #DFDFDF; padding: 3px 8px 3px 10px;">
                            weitere Optionen
                        </div>
                        <div style="text-align: center;">
                            <asp:Button ID="btnÜbersicht" runat="server" Text="Übersicht Stempelzeiten" CssClass="ButtonTouch Wide"
                                Style="margin: 5px 10px 5px 10px;" />
                        </div>
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="btnDummy" runat="server" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <asp:Button ID="btnDummy2" runat="server" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <cc1:ModalPopupExtender ID="mpeQuestion" runat="server" TargetControlID="btnDummy"
                            PopupControlID="pQuestion" BackgroundCssClass="divProgress" CancelControlID="btnPanelQuestionCancel">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pQuestion" runat="server" Style="overflow: auto; display: none;">
                            <table cellspacing="0" runat="server" bgcolor="white" cellpadding="0" style="overflow: auto;
                                min-height: 100px; min-width: 100px; border: solid 1px #646464; padding: 10px 10px 15px 0px;">
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        <asp:Label ID="lblPanelQuestionHeadline" runat="server">Fehler!</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft" style="padding-top: 0px; padding-bottom: 0px;
                                        padding-right: 15px;">
                                        <asp:Label ID="lblPanelQuestion" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstleft" style="padding-top: 0px; padding-bottom: 0px; padding-left: 15px;
                                        padding-right: 15px;">
                                        &nbsp;
                                        <hr />
                                    </td>
                                </tr>
                                <tr id="trPopSelectionPanel1" runat="server">
                                    <td align="center" class="firstLeft active">
                                        <table id="tblRuest" runat="server">
                                            <tr id="trOeffnung" runat="server">
                                                <td style="text-align: left;">
                                                    Filialöffnung:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkOeffnung" runat="server" Checked="false" Enabled="false"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr id="trAbrechnung" runat="server">
                                                <td style="text-align: left;">
                                                    Abrechnung:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkAbrechnung" runat="server" Checked="false" Enabled="false">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr id="trEinzahlung" runat="server">
                                                <td style="text-align: left;">
                                                    Einzahlung:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkEinzahlung" runat="server" Checked="false" Enabled="false">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trPopSelectionPanel2" runat="server" align="center" style="padding-top: 0px;
                                    padding-bottom: 0px; padding-left: 15px; padding-right: 15px;">
                                    <td class="firstleft" style="padding-top: 0px; padding-bottom: 0px; padding-left: 15px;
                                        padding-right: 15px;">
                                        &nbsp;
                                        <hr />
                                    </td>
                                </tr>
                                <tr class="formquerry">
                                    <td class="firstLeft" align="center">
                                        <asp:LinkButton ID="btnPanelQuestionCancel" runat="server" Text="Abbruch" Height="16px"
                                            Width="78px" CssClass="Tablebutton" CausesValidation="false" />
                                        <asp:LinkButton ID="btnPanelQuestionOK" runat="server" Text="OK" Height="16px" Width="78px"
                                            CssClass="Tablebutton" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="mpeZeitnachweis" runat="server" TargetControlID="btnDummy2"
                            PopupControlID="pZeitnachweis" BackgroundCssClass="divProgress" CancelControlID="btnPanelZeitnachweisCancel">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pZeitnachweis" runat="server" Style="overflow: auto; display: none;">
                            <table cellspacing="0" bgcolor="white" cellpadding="0" style="overflow: auto;
                                min-height: 100px; min-width: 100px; border: solid 1px #646464; padding: 10px 10px 15px 0px;">
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        <asp:Label ID="Label1" runat="server">Zeitnachweise</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft" style="padding-top: 0px; padding-bottom: 0px;
                                        padding-right: 15px;">
                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstleft" style="padding-top: 0px; padding-bottom: 0px; padding-left: 15px;
                                        padding-right: 15px;">
                                        &nbsp;
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        <asp:ListView ID="lvZeitnachweis" ItemPlaceholderID="ZnItemContainer" GroupPlaceholderID="ZnRowContainer"
                                            runat="server">
                                            <LayoutTemplate>
                                                <style type="text/css">
                                                .PDFLink {
                                                    color:#333333;
                                                }

                                                .PDFLink :hover{
                                                    color:#F84814;
                                                }
                                                </style>
                                                <table cellpadding="4" runat="server" id="tblZeitnachweis">                                                   
                                                    <tr runat="server" id="ZnRowContainer" />
                                                </table>
                                            </LayoutTemplate>
                                            <GroupTemplate>
                                                <tr runat="server" id="ZnRowContainer">
                                                    <td runat="server" id="ZnItemContainer" />
                                                </tr>
                                            </GroupTemplate>
                                            <ItemTemplate>
                                                <td style="text-align: left; width:200px; padding:5px 5px 5px 5px" runat="server">
                                                    <asp:ImageButton runat="server" ImageUrl="../Images/iconPDF.gif" CommandName="GetPDF" 
                                                    CommandArgument='<%# Eval("Jahr").ToString() + " " + Eval("Monat").ToString() %>' 
                                                    style="margin-right:5px;"/>   
                                                    <asp:LinkButton runat="server" Text='<%# Eval("Monatsbezeichnung") + " " + Eval("Jahr").ToString() %>' 
                                                        CommandName="GetPDF" CommandArgument='<%# Eval("Jahr").ToString() + " " + Eval("Monat").ToString() %>'
                                                       class="PDFLink"/>
                                                </td>
                                            </ItemTemplate>                                            
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr align="center" style="padding-top: 0px;
                                    padding-bottom: 0px; padding-left: 15px; padding-right: 15px;">
                                    <td class="firstleft" style="padding-top: 0px; padding-bottom: 0px; padding-left: 15px;
                                        padding-right: 15px;">
                                        &nbsp;
                                        <hr />
                                    </td>
                                </tr>
                                <tr class="formquerry">
                                    <td class="firstLeft" align="center">
                                        <asp:LinkButton ID="btnPanelZeitnachweisCancel" runat="server" Text="Schließen" Height="16px"
                                            Width="78px" CssClass="Tablebutton" CausesValidation="false" />                                       
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
</asp:Content>
