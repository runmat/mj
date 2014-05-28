<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04s.aspx.vb" Inherits="CKG.Components.ComCommon.Change04s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Watermark
        {
            color: Gray;
            text-align: center;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="divControls" runat="server">
                    <div id="TableQuery" style="margin-bottom: 10px">
                        <table id="tab1" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td nowrap="nowrap" width="100%">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:200px">
                                    Datum:
                                </td>
                                <td class="firstLeft active" style="width:200px">
                                    <u><asp:Label ID="lbl_Vertragsnummer" runat="server"></asp:Label></u>
                                </td>
                                <td style="width:200px;font-weight:bold">
                                    <asp:Label ID="lbl_Kennzeichen" runat="server"></asp:Label>
                                </td><td class="firstLeft active">
                                </td>
                            </tr>
                            <tr>
                                <td class="active" style="padding-left: 15px">
                                    <asp:Label ID="lblAuftragsdatum" runat="server"></asp:Label></td><td style="padding-left: 15px">
                                    <asp:TextBox ID="txt_Vertragsnummer" runat="server"></asp:TextBox></td><td>
                                    <asp:TextBox ID="txtKennz" runat="server"></asp:TextBox></td><td class="active">
                                    <asp:TextBox ID="txtSachbearbeiter" runat="server" Enabled="False" Visible="False"></asp:TextBox></td></tr></table><div style="background-color: #dfdfdf; float: left; width: 100%">
                            <table>
                                <tr>
                                    <td style="padding-left: 15px;font-weight:bold;width:200px">
                                        <asp:Label ID="lbl_Auftrag" runat="server"></asp:Label>
                                    </td>
                                    <td style="padding-left:15px;font-weight:bold">
                                        <asp:Label ID="lbl_ScheinSchilderAn" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td class="firstLeft active" style="width:200px">
                                    <asp:RadioButton ID="rb_ErsatzKfzSchein" runat="server" Font-Bold="True" GroupName="selection1"
                                        Text="Ersatz Kfz-Schein" />
                                </td>
                                <td class="firstLeft active" style="width:200px">
                                    <asp:Label id="lbl_Firma" runat="server"></asp:Label>
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtZielFirma" runat="server" Width="189px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rb_NeuesSchild" runat="server" Font-Bold="True" GroupName="selection1"
                                        Text="Neues Schild" />
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label id="lbl_Firma2" runat="server"></asp:Label>
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtZielFirma2" runat="server" Width="189px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rb_Ummeldung" runat="server" Font-Bold="True" GroupName="selection1"
                                        Text="Ummeldung" />
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label id="lbl_StrasseHsNr" runat="server"></asp:Label>
                                   
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtZielStrasse" runat="server" Width="160px"></asp:TextBox><asp:TextBox ID="txtZielHNr" runat="server" MaxLength="5" Width="20px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rb_Umkennzeichnung" runat="server" Font-Bold="True" GroupName="selection1"
                                        Text="Umkennzeichnung" />
                                </td>
                                <td class="firstLeft active">
                                <asp:Label id="lbl_PlzOrt" runat="server"></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtZielPLZ" runat="server" MaxLength="5" Width="40px"></asp:TextBox><asp:TextBox ID="txtZielOrt" runat="server" Width="140px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rb_Versicherungswechsel" runat="server" Font-Bold="True" GroupName="selection1"
                                        Text="Versicherungswechsel" />
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label id="lbl_Telefon" runat="server"></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtZielTelefon" runat="server" MaxLength="20" Width="189px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rb_Briefaufbietung" runat="server" Font-Bold="True" GroupName="selection1"
                                        Text="Briefaufbietung" />
                                </td>
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                                <td class="active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rb_Tempversand" runat="server" Font-Bold="True" GroupName="selection1"
                                        Text="Temp. Versand" />
                                </td>
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                                <td class="active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active">
                                    <asp:RadioButton ID="rb_Sonstiges" runat="server" Checked="True" Font-Bold="True"
                                        GroupName="selection1" Text="Sonstiges" />
                                </td>
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                                <td class="active">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div style="background-color: #dfdfdf; float: left; width: 100%;">
                            <table>
                                <tr>
                                    <td style="padding-left: 15px;width:407px;font-weight:bold">
                                        <asp:Label ID="lbl_HalterAlt" runat="server" ></asp:Label>
                                        
                                    </td>
                                    <td style="padding-left:15px;font-weight:bold">
                                        <asp:Label ID="lbl_HalterNeu" runat="server" ></asp:Label>
                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td class="firstLeft active" style="width:200px">
                                    <asp:Label ID="lbl_FirmaAlt" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active" style="width:200px" >
                                    <asp:TextBox ID="txtHalterAltFirma" runat="server" Width="189px"></asp:TextBox></td><td class="firstLeft active">
                                    <asp:Label ID="lbl_FirmaNeu" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterNeuFirma" runat="server" Width="189px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_Firma2Alt" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterAltFirma2" runat="server" Width="189px"></asp:TextBox></td><td class="firstLeft active">
                                     <asp:Label ID="lbl_Firma2Neu" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterNeuFirma2" runat="server" Width="189px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_StrasseHsNrAlt" runat="server" ></asp:Label>
                                   
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterAltStrasse" runat="server" Width="160px"></asp:TextBox><asp:TextBox ID="txtHalterAltHNr" runat="server" MaxLength="5" Width="20px"></asp:TextBox></td><td class="firstLeft active">
                                     <asp:Label ID="lbl_StrasseHsNrNeu" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterNeuStrasse" runat="server" Width="160px"></asp:TextBox><asp:TextBox ID="txtHalterNeuHNr" runat="server" MaxLength="5" Width="20px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_PlzOrtAlt" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterAltPLZ" runat="server" MaxLength="5" Width="40px"></asp:TextBox><asp:TextBox ID="txtHalterAltOrt" runat="server" Width="140px"></asp:TextBox></td><td class="firstLeft active">
                                    <asp:Label ID="lbl_PlzOrtNeu" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterNeuPLZ" runat="server" MaxLength="5" Width="40px"></asp:TextBox><asp:TextBox ID="txtHalterNeuOrt" runat="server" Width="140px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                     <asp:Label ID="lbl_TelefonAlt" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterAltTelefon" runat="server" Width="189px"></asp:TextBox></td><td class="firstLeft active">
                                    <asp:Label ID="lbl_TelefonNeu" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtHalterNeuTelefon" runat="server" Width="189px"></asp:TextBox></td></tr></table><div style="background-color: #dfdfdf; float: left; width: 100%;">
                            <table>
                                <tr>
                                    <td style="padding-left: 15px;font-weight:bold">
                                        <asp:Label ID="lbl_Fahrzeugdaten" runat="server" ></asp:Label>
                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td class="firstLeft active" style="width:200px">
                                    <asp:Label ID="lbl_AmtlKennz" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active" colspan="3">
                                    <asp:TextBox ID="txtAmtlichesKennzeichen" runat="server" Width="80px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                     <asp:Label ID="lbl_WunschKennz" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active" style="width:97px">
                                    <asp:TextBox ID="txtWKZ1" runat="server" Width="80px"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender ID="txtWKZ1_TextBoxWatermarkExtender" runat="server"
                                        Enabled="True" TargetControlID="txtWKZ1" WatermarkCssClass="Watermark" WatermarkText="1">
                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                </td>
                                <td class="active" style="width:98px">
                                    <asp:TextBox ID="txtWKZ2" runat="server" Width="80px"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                                        Enabled="True" TargetControlID="txtWKZ2" WatermarkCssClass="Watermark" WatermarkText="2">
                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                </td>
                                <td class="active" style="padding-left:15px">
                                    <asp:TextBox ID="txtWKZ3" runat="server" Width="80px"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtWKZ3" WatermarkCssClass="Watermark" WatermarkText="3">
                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_Reservierung" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active" colspan="3">
                                    <asp:TextBox ID="txtReservierungsnummer" runat="server" Width="180px"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_WunschZulassungstermin" runat="server" ></asp:Label>
                                    
                                </td>
                                <td class="active" colspan="3">
                                    <asp:TextBox ID="txtZulassungstermin" runat="server" Width="180px"></asp:TextBox></td></tr></table><div style="background-color: #dfdfdf; float: left; width: 100%;">
                            <table>
                                <tr>
                                    <td  style="padding-left: 15px;font-weight:bold;width:407px">
                                        <asp:Label ID="lbl_VersicherungAlt" runat="server" ></asp:Label>
                                        
                                    </td>
                                    <td style="padding-left: 15px;font-weight:bold">
                                        <asp:Label ID="lbl_VersicherungNeu" runat="server" ></asp:Label>
                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td class="active" style="padding-left: 15px;width:407px">
                                    <asp:TextBox ID="txtVersicherungAlt" runat="server" Width="280px"></asp:TextBox></td><td class="active" style="padding-left:15px">
                                    <asp:TextBox ID="txtVersicherungNeu" runat="server" Width="280px"></asp:TextBox></td></tr></table><div style="background-color: #dfdfdf; float: left; width: 100%;">
                            <table>
                                <tr>
                                    <td style="padding-left: 15px;font-weight:bold">
                                         <asp:Label ID="lbl_Bemerkung" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td style="padding-left:15px">
                                    <asp:TextBox ID="txtBemerkung" runat="server" Height="40px" TextMode="MultiLine"
                                Width="98%"></asp:TextBox></td></tr><tr>
                                <td class="firstLeft active">
                                     *Pflichtfeld
                                </td>
                            </tr>
                        </table>
                        
                        
                         <div id="dataQueryFooter">
                                                    <asp:LinkButton ID="cmdContinue" runat="server" CssClass="TablebuttonLarge" Width="110px"
                                                        Height="16px">» Weiter</asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="TablebuttonLarge" Width="110px"
                                                        Height="16px">» Bestätigen</asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="cmdCancel" runat="server" CssClass="TablebuttonLarge" Width="110px"
                                                        Height="16px">» Abbrechen</asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="cmdNew" runat="server" CssClass="TablebuttonLarge" Width="110px"
                                                        Height="16px">» Neuer Auftrag</asp:LinkButton>&nbsp;
                                                </div>
                        
                    </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
