<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditVorschaden.ascx.cs" Inherits="AppRemarketing.PageElements.EditVorschaden" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr class="formquery">
                    <td style="padding-left:15px;padding-right:5px;white-space:nowrap;" colspan="2">
                        <asp:Label ID="lblError" runat="server" style="font-weight: bold;color: #BC2B2B;" 
                            EnableViewState="False" Font-Bold="True" Font-Names="Verdana" 
                            Font-Size="10px"></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" 
                            Font-Bold="True" ForeColor="#009900" Font-Names="Verdana" 
                            Font-Size="10px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:15px;padding-right:5px;white-space:nowrap;">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr style="padding-top:5px">
                    <td style="padding-left:15px;padding-right:5px;white-space:nowrap;color:#595959;font-weight:bold;font-size:10px;font-family:Verdana, sans-serif;"> 
                        lfd. Nummer&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtLfdNummer" runat="server" Enabled="false" 
                            style="border:solid 1px #bfbfbf"></asp:TextBox>
                    </td>
                </tr>
                <tr style="padding-top:5px">
                    <td style="padding-left:15px;padding-right:5px;white-space:nowrap;color:#595959;font-weight:bold;font-size:10px;font-family:Verdana, sans-serif;"> 
                        Fahrgestellnummer
                    </td>
                    <td>
                        <asp:TextBox ID="txtFin" runat="server" Enabled="false" style="border:solid 1px #bfbfbf"></asp:TextBox>
                    </td>
                </tr>
                <tr style="padding-top:5px">
                    <td style="padding-left:15px;padding-right:5px;white-space:nowrap;color:#595959;font-weight:bold;font-size:10px;font-family:Verdana, sans-serif;">
                        Kennzeichen
                    </td>
                    <td class="active" nowrap="nowrap">
                        <asp:TextBox ID="txtKennzeichen" runat="server" Enabled="false" style="border:solid 1px #bfbfbf"></asp:TextBox>
                    </td>
                </tr>
                <tr style="padding-top:5px">
                    <td valign="top" style="padding-top:5px;padding-left:15px;padding-right:5px;white-space:nowrap;color:#595959;font-weight:bold;font-size:10px;font-family:Verdana, sans-serif;">
                        Beschreibung
                    </td>
                    <td style="vertical-align: top">
                        <asp:TextBox ID="txtBeschreibung" runat="server" Rows="3" TextMode="MultiLine" Width="450px" style="border:solid 1px #bfbfbf"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtBeschreibung" ID="rfvBeschreibung"
                            runat="server" ErrorMessage="Eingabe erforderlich." Font-Bold="True" 
                            Font-Names="Verdana" Font-Size="10px"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="padding-top:5px">
                    <td style="padding-left:15px;padding-right:5px;white-space:nowrap;color:#595959;font-weight:bold;font-size:10px;font-family:Verdana, sans-serif;">
                        Preis ohne Währung
                    </td>
                    <td>
                        <asp:TextBox ID="txtPreis" runat="server" style="border:solid 1px #bfbfbf;padding-left:5px"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPreis" ID="rfcPreis" runat="server"
                            ErrorMessage="Eingabe erforderlich." Font-Bold="True" Font-Names="Verdana"
                            Font-Size="10px"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="padding-top:5px">
                    <td style="padding-left:15px;padding-right:5px;white-space:nowrap;color:#595959;font-weight:bold;font-size:10px;font-family:Verdana, sans-serif;">
                        Schadensdatum
                    </td>
                    <td>
                        <asp:TextBox ID="txtDatum" runat="server" style="border:solid 1px #bfbfbf"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy"
                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatum">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MEE_Datum" runat="server" TargetControlID="txtDatum"
                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ControlToValidate="txtDatum" ID="rfvDatum" runat="server"
                            ErrorMessage="Eingabe erforderlich." Font-Bold="True" Font-Names="Verdana" 
                            Font-Size="10px"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </tbody>
        </table>
        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
        <div id="dataQueryFooter" style="padding-top:5px">
                        <asp:LinkButton ID="lbCancel" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="lbCancel_Click" CausesValidation="False">» Zurück </asp:LinkButton>
                        
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" 
                            onclick="lbCreate_Click" Width="78px">» Speichern </asp:LinkButton>
                        
                    </div>
