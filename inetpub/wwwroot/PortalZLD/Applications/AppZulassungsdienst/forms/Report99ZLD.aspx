<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report99ZLD.aspx.cs" Inherits="AppZulassungsdienst.forms.Report99ZLD"
    MasterPageFile="../MasterPage/App.Master" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1> 
                    </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <div id="Div1"  class="dataQueryFooter" style="margin-top: 0px;margin-bottom: 0px;background-color: #dfdfdf;width:100% ;height: 22px;">
                                        &nbsp;
                                    </div>
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennzeichen" runat="server">Ortskennzeichen:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="3" Width="50px" CssClass="TextBoxNormal" ControlToValidate="txtKennzeichen"></asp:TextBox>
                                                    &nbsp;
                                                    <asp:RequiredFieldValidator runat ="server" ControlToValidate="txtKennzeichen" CssClass="TextError" ID ="rfvKennzeichen" Text="Eingabe erforderlich"></asp:RequiredFieldValidator>
                                            
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" onclick="btnEmpty_Click" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click">» Suchen</asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" style="border: solid 1px #dfdfdf;
                                        margin-bottom: 5px" bgcolor="white" border="0">
                                        <tr class="formquery">
                                            <td class="firstLeft active" height="22">
                                                <u>Kategorie\Dokument</u>**
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                ZB1
                                            </td>
                                            <td class="firstLeft active">
                                                ZB2
                                            </td>
                                            <td class="firstLeft active">
                                                CoC
                                            </td>
                                            <td class="firstLeft active">
                                                eVB
                                            </td>
                                            <td class="firstLeft active">
                                                VM
                                            </td>
                                            <td class="firstLeft active">
                                                PA
                                            </td>
                                            <td class="firstLeft active">
                                                GewA
                                            </td>
                                            <td class="firstLeft active">
                                                HRA
                                            </td>
                                            <td class="firstLeft active">
                                                SEPA
                                                <asp:HyperLink ID="lnkEinzug" style="color:#4c4c4c;" runat="server" Visible="False" Font-Underline="True">SEPA</asp:HyperLink>
                                            </td>
                                            <td class="firstLeft active" style="width: 100%">
                                                Bemerkung
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="11" style="background-color: #dfdfdf; height: 22px;">
                                                Privat
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Zulassung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label01" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label00" runat="server" Width="100%"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label02" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label03" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label04" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label05" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label06" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label07" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label08" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:Label ID="Label09" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Umschreibung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label11" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label10" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label12" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label13" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label14" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label15" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label16" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label17" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label18" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:Label ID="Label19" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Umkennzeichnung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label21" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label20" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label22" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label23" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label24" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label25" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label26" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label27" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label28" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:Label ID="Label29" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" height="22">
                                                Ersatzfahrzeugschein
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label31" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label30" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label32" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:Label ID="Label33" runat="server" Width="100%"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label34" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label35" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label36" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label37" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label38" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:Label ID="Label39" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="11" style="background-color: #dfdfdf; height: 22px;">
                                                Unternehmen
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" height="22">
                                                Zulassung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label41" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label40" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label42" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label43" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label44" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label45" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label46" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label47" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label48" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:Label ID="Label49" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" height="22">
                                                Umschreibung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label51" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label50" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label52" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label53" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label54" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label55" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label56" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label57" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label58" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:Label ID="Label59" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" height="22">
                                                Umkennzeichnung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label61" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label60" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label62" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label63" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label64" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label65" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label66" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label67" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label68" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:Label ID="Label69" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" height="22">
                                                Ersatzfahrzeugschein
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label71" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label70" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:Label ID="Label72" runat="server"></asp:Label>
                                                <td class="firstLeft active" style="font-weight: normal">
                                                    <asp:Label ID="Label73" runat="server"></asp:Label>
                                                    <td class="firstLeft active" style="font-weight: normal">
                                                        <asp:Label ID="Label74" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" style="font-weight: normal">
                                                        <asp:Label ID="Label75" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" style="font-weight: normal">
                                                        <asp:Label ID="Label76" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" style="font-weight: normal">
                                                        <asp:Label ID="Label77" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" style="font-weight: normal">
                                                        <asp:Label ID="Label78" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                        <asp:Label ID="Label79" runat="server"></asp:Label>
                                                    </td></td>
                                        </tr>
                                    </table>
                                    <table class="TableLegende" id="Table2" height="107" cellspacing="1" cellpadding="2"
                                        bgcolor="#ffffff" border="0" style="font-size: 6pt">
                                        <tr>
                                            <td>
                                                <u>**Legende:</u>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                O=Original
                                            </td>
                                            <td>
                                                K=Kopie
                                            </td>
                                            <td>
                                                F=Formular Zulassungsstelle
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                ZB1=Fahrzeugschein,
                                            </td>
                                            <td>
                                                ZB2=Fahrzeugbrief,
                                            </td>
                                            <td nowrap>
                                                CoC=Certificate of Conformity,
                                            </td>
                                            <td>
                                                eVB=elektronische Versicherungsbestätigung,
                                            </td>
                                            <td>
                                                VM=Vollmacht,
                                            </td>
                                            <td>
                                                PA=Personalausweis,
                                            </td>
                                            <td>
                                                GewA=Gewerbeanmeldung,
                                            </td>
                                            <td>
                                                HRA=Handelsregister,
                                            </td>
                                            <td>
                                                SEPA=SEPA-Mandat für Kfz-Steuer
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                            </td>
                                            <td nowrap>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" height="18" style="font-size: 9pt">
                                                <u>Wir weisen darauf hin, dass diese Angaben unverbindliche Auskünfte der entsprechenden
                                                    Zulassungskreise sind.</u>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="TableLinks" id="Table4" cellspacing="1" cellpadding="2" align="center"
                                        border="0">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>&nbsp;Links:</b>
                                            </td>
                                            <td width="136">
                                                <p align="center">
                                                    <asp:LinkButton ID="cmdAmt" runat="server" Width="125"
                                                        Enabled="False" onclick="cmdAmt_Click" ForeColor="#2265BE">Amt</asp:LinkButton></p>
                                            </td>
                                            <td>
                                                <p align="center">
                                                    <asp:LinkButton ID="cmdWunsch" runat="server" Width="125"
                                                        Enabled="False" onclick="cmdWunsch_Click" ForeColor="#2265BE">Wunschkennzeichen</asp:LinkButton></p>
                                            </td>
                                            <td>
                                                <p align="center">
                                                    <asp:LinkButton ID="cmdFormulare" runat="server" Width="125"
                                                        Enabled="False" onclick="cmdFormulare_Click" ForeColor="#2265BE">Formulare</asp:LinkButton></p>
                                            </td>
                                            <td>
                                                <p align="center">
                                                    <asp:LinkButton ID="cmdGebuehr" runat="server" Width="125"
                                                        Enabled="False" onclick="cmdGebuehr_Click" ForeColor="#2265BE">Gebühren</asp:LinkButton></p>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table7" width="630" align="center">
                                        <tr>
                                            <td>
                                                <span style="font-size: 10pt; color: #0066cc">
                                                    <asp:Label ID="lblInfo" runat="server"></asp:Label></span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <span style="padding-top:15px; padding-bottom:15px; font-size: 10pt; color: #0066cc;font-weight:bold">
                                    Haftungsausschluss
                                
                                </span><br /><br />
                               
                                    
                                    <span style="font-size: 8pt;font-weight:bold">
                                        Haftungsausschluss für eigene Seiten
                                    </span>
                                    <br />
                                        <span style="font-size: 8pt;">
                                            DAD / Christoph Kroschke GmbH übernimmt keine Haftung oder Garantie für die Aktualität, Richtigkeit
                                            oder Vollständigkeit der auf dieser Website bereitgestellten Informationen. Die
                                            Inhalte dieser Internet-Seiten basieren teilweise auf gesetzlichen Grundlagen und
                                            werden regelmäßig überprüft. Es kann nicht garantiert werden, dass nach einer gesetzlichen
                                            Änderung eine sofortige Anpassung der Internet-Seiten erfolgt. DAD / Christoph Kroschke
                                            GmbH übernimmt keine Haftung für direkte oder indirekte Schäden, die aus der Benutzung
                                            diese Website entstehen können.
                                         </span>
                                                <br/>
                                                <br/>
                                                
                                         <span style="font-size: 8pt;font-weight:bold" >Haftungsausschluss für fremde Seiten</span><br />
                                                </b>
                                           <span style="font-size: 8pt">
                                                Mit dem Urteil vom 12.Mai 1998 - 312&nbsp;O&nbsp;85/98 - "Haftung für Links"
                                                hat das Landgericht Hamburg entschieden, dass durch die Anbringung eines Links die
                                                Inhalte der gelinkten Seite ggf. mit zu verantworten sind. Dies kann nur dadurch
                                                verhindert werden, dass man sich ausdrücklich von diesen Inhalten distanziert. Manche
                                                Verweise/Hyperlinks führen zu Angeboten außerhalb dieser Website, welche nicht vom
                                                DAD / der Christoph Kroschke GmbH erstellt oder gepflegt werden. Dies wird insbesondere
                                                dadurch deutlich, dass in der Regel der Verweis / Hyperlink mit &#8222;www&#8220;
                                                gekennzeichnet und nach dem Öffnen der Rahmen der Homepage der Christoph Kroschke
                                                GmbH in dem neuen Fenster nicht mehr sichtbar ist. Die Christoph Kroschke GmbH übernimmt
                                                keine Haftung oder Garantie für die Aktualität, Richtigkeit oder Vollständigkeit
                                                der auf der jeweiligen verzeichneten Websites veröffentlichten Inhalte, für deren
                                                Rechtmäßigkeit oder für die Erfüllung von Urheberrechtsbestimmungen im Zusammenhang
                                                mit den auf der jeweiligen Website veröffentlichten Inhalten. DAD / Christoph Kroschke
                                                GmbH übernimmt keine Haftung für direkte oder indirekte Schäden, die aus der Benutzung
                                                dieser Websites entstehen können und distanziert sich ausdrücklich von den Inhalten.
                                       </span>
                                    <br /><br />
                                    <span style="font-size: 8pt;font-weight:bold">
                                        Verbindlichkeit
                                    </span>
                                    <br />                                    
                                        <span style="font-size: 8pt;">
                                            Die in dieser Website gemachten Ausführungen dienen lediglich der 
                                            allgemeinen Information. Ein Rechtsanspruch auf eine bestimmte 
                                            Leistung des DAD / der Christoph Kroschke GmbH kann hierauf nicht 
                                            begründet werden. In jeder konkreten Angelegenheit ist daher eine 
                                            Einzelfallentscheidung nach dem vorgeschriebenen 
                                            Verwaltungsverfahren erforderlich.
                                         </span>      
                                    <br /><br />
                                    <span style="font-size: 8pt;font-weight:bold">
                                        Urheberrechte
                                    </span>
                                    <br />                                    
                                        <span style="font-size: 8pt;">
                                            Der gesamte Inhalt und die Abbildungen dieser 
                                            Website sowie die Gestaltung unterliegen dem Urheberrecht des DAD / 
                                            der Christoph Kroschke GmbH. Das Kopieren von Texten und Bildern 
                                            zum kommerziellen Gebrauch ist nur mit ausdrücklicher schriftlicher 
                                            Genehmigung des DAD / der Christoph Kroschke GmbH gestattet.
                                       </span>                                                                    
                            </div>

                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
