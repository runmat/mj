<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BenutzerAnlegen.aspx.cs"
    Inherits="Kantine.BenutzerAnlegen" MasterPageFile="Kantine.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div id="Main" style="text-align: center;">
        <br />
        <div style="float: left;">
            <asp:Label ID="lblError" runat="server" class="Error" meta:resourcekey="lblErrorResource1"></asp:Label>
        </div>
        <br />
        <br />
        <div class="Heading">
            <asp:Label ID="lblÜberschrift" runat="server">Benutzer anlegen</asp:Label>
        </div>
        <div class="Rahmen">
            <div>
                &nbsp;</div>
            <div id="staticData" runat="server">
                <table id="tblStaticData" runat="server" visible="false">
                    <tr>
                        <td class="Beschriftung" width="150">
                            Kundennummer:
                        </td>
                        <td align="left">
                            <asp:Label ID="lblKundennummer" runat="server"></asp:Label>
                        </td>
                        <td class="Beschriftung" width="150">
                            Kartennummer:
                        </td>
                        <td align="left">
                            <asp:Label ID="lblKartennummer" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Beschriftung" width="150">
                            Guthaben:
                        </td>
                        <td align="left">
                            <asp:Label ID="lblGuthaben" runat="server"></asp:Label>
                        </td>
                        <td class="Beschriftung" width="150">
                            Kartenverlust:
                        </td>
                        <td align="left">
                            <asp:Button ID="btnNeueKarte" runat="server" Text="neue Karte" CssClass="ButtonMiddle"
                                OnClick="btnNeueKarte_Click" TabIndex="1" ToolTip="aktuelle Karte nochmals drucken"/>                            
                            <asp:Button ID="btnCardLost" runat="server" Text="Karte verloren" CssClass="ButtonMiddle"
                                OnClick="btnCardLost_Click" TabIndex="2" ToolTip="alte Karte sperren und Karte mit neuer Kartennummer drucken" />
                            <asp:Button ID="btnDummy" runat="server" Width="0" Height="0" BackColor="Transparent"
                                BorderStyle="none" />
                            <cc1:ModalPopupExtender ID="mpepReminder" runat="server" TargetControlID="btnDummy" BackgroundCssClass="BackgroundPopUp"
                                CancelControlID="btnCancel" PopupControlID="pReminder">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="pReminder" runat="server" Style="padding: 5px 5px 5px 5px; background-color:#ffffff; border:solid 1px #595959; width:400px;">
                                <table runat="server" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Label2" runat="server" style="color:Black; font-size:14px; font-weight:bold;">Vorbereitung Kartendruck</asp:Label>
                                            <hr style="border:none; border-top:solid 1px #595959;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Label3" runat="server" Style="font-size: 10pt;">
                                                Bitte schalten Sie jetzt den Kartendrucker ein! Sobald dieser bereit ist, klicken Sie auf <b>Drucken</b>, um den Druck der neuen Karte zu starten.
                                            </asp:Label>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" CssClass="Beschriftung">Drucker:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPrinters" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left">
                                           <%-- <asp:ImageButton id="ibtnRefreshPrinter" runat="server" Width="16px" 
                                                Height="16px" AlternateText="AktualisierenButton" 
                                                ImageUrl=".\Images\arrow_refresh.png" onclick="ibtnRefreshPrinter_Click"/>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <hr style="border:none; border-top:solid 1px #595959;"/>
                                            <asp:Button ID="btnPrintNeu" runat="server" Text="Drucken" CssClass="Button" OnClick="btnPrintNeu_Click" />
                                            <asp:Button ID="btnPrint" runat="server" Text="Drucken" CssClass="Button" OnClick="btnPrint_Click" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Abbrechen" CssClass="Button" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <div>
                    &nbsp;</div>                    
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Benutzername:
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox ID="txtBenutzername" runat="server" Width="200px" OnTextChanged="txtBenutzername_TextChanged"
                                    AutoPostBack="false" TabIndex="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Nachname:
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox ID="txtNachname" runat="server" Width="200px" OnTextChanged="txtNachname_TextChanged"
                                    AutoPostBack="false" TabIndex="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Vorname:
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox ID="txtVorname" runat="server" Width="200px" OnTextChanged="txtVorname_TextChanged"
                                    AutoPostBack="false" TabIndex="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Passwort:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtPasswort" runat="server" Width="200px" OnTextChanged="txtPasswort_TextChanged"
                                    AutoPostBack="false" TabIndex="6" TextMode="Password"></asp:TextBox>                                    
                            </td>
                            <td class="Infotext">
                                <asp:Label runat="server">Nur für Benutzer mit Admin-, Verkäufer- oder Benutzer-Admin- Berechtigungen zu füllen!</asp:Label>
                            </td>
                        </tr>
                        <tr id="trPWReset" runat="server" visible="false">
                        <td></td>
                        <td >
                        <asp:Button id="btnPWReset" runat="server" Text="PW Zurücksetzen" CssClass="ButtonLarge"
                                onclick="btnPWReset_Click" TabIndex="7"/>
                        </td>
                        <td class="Infotext">
                            <asp:Label ID="lblPWReset" runat="server"></asp:Label>
                        </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="trAdmin" runat="server" visible="false">
                            <td class="Beschriftung" width="150">
                                Admin:
                            </td>
                            <td align="left" colspan="2">
                                <asp:CheckBox ID="chkAdmin" runat="server" Checked="false" AutoPostBack="false" OnCheckedChanged="chkAdmin_CheckedChanged" TabIndex="8"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Verkäufer:
                            </td>
                            <td align="left" colspan="2">
                                <asp:CheckBox ID="chkSeller" runat="server" Checked="false" AutoPostBack="false" OnCheckedChanged="chkSeller_CheckedChanged" TabIndex="9"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Benutzer-Admin:
                            </td>
                            <td align="left" colspan="2">
                                <asp:CheckBox ID="chkUseradmin" runat="server" Checked="false" AutoPostBack="false"
                                    OnCheckedChanged="chkUseradmin_CheckedChanged" TabIndex="10"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Gesperrt:
                            </td>
                            <td align="left" colspan="2">
                                <asp:CheckBox ID="chkGesperrt" runat="server" Checked="false" AutoPostBack="false"
                                    OnCheckedChanged="chkGesperrt_CheckedChanged" TabIndex="11" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                &nbsp;
            </div>
        </div>
        <div class="Buttoncontainer">
            <table>
                <tr>
                    <td width="100%">
                    </td>
                    <td>
                        <asp:Button ID="btnSpeichern" runat="server" Text="Speichern" CssClass="Button" OnClick="btnSpeichern_Click" TabIndex="12" />
                    </td>
                    <td>
                        <asp:Button ID="btnBack" runat="server" Text="Zurück" CssClass="Button" OnClick="btnBack_Click" TabIndex="13" />
                    </td>
                </tr>
            </table>
        </div>        
    </div>
</asp:Content>
