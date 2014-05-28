<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02s.aspx.vb" Inherits="CKG.Components.ComCommon.Beauftragung2.Change02s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Wenn maximale Feldlänge des aktuellen Controls erreicht, dann auf ein beliebiges ZielControl springen.
        function autotab(original, destination) {
            if (original.getAttribute && original.value.length == original.getAttribute("maxlength"))
                destination.focus();     
        }        
        function FilterItems(value, ddlClientID) {
            var ddl = ddlClientID;
            var found = 0;

            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].value.substr(0, value.length) == value.toUpperCase()) {
                    ddl.selectedIndex = i;
                    found = 1;
                    break;
                }
            }
            if (found == 0) {
                ddl.selectedIndex = 0;
            }
        }
        function SetItemText(ddlClientID, Textbox) {
            var wert = ddlClientID.options[ddlClientID.selectedIndex].value;
            if (wert != "0") {
                Textbox.value = ddlClientID.options[ddlClientID.selectedIndex].value;
            } else {
                Textbox.value = "";
            }     
            __doPostBack(ddlClientID.id, '');
        }
    </script>
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
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                    </div>
                    <br />
                    <div>
                        * = Pflichteingabe
                    </div>
                    <br />
                    <div>
                        <asp:UpdatePanel runat="server" ID="UP1">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lbCreate" />
                            </Triggers>
                            <ContentTemplate>
                                <div>
                                    <asp:Button ID="btnDefault" runat="server" Text="" Width="0px" Height="0px" BackColor="White"
                                        Style="height: 0; width: 0" BorderStyle="None" />
                                </div>
                                <div style="border: 1px solid #C0C0C0; color: #595959;">
                                    <div style="width: auto; background-color: #DFDFDF; height: 20px; font-weight: bold;
                                        color: #595959; padding-top: 10px; padding-left: 15px; padding-right: 5px;" >
                                        <asp:Label runat="server" Text="Basisdaten"></asp:Label>&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblHeadInfo" runat="server" Font-Size="10px"></asp:Label>
                                        <span>
                                            <asp:ImageButton ID="ibtBasisdatenResize" runat="server" ImageAlign="Right" ImageUrl="../../../Images/queryArrow.gif"
                                                Style="margin-top: 0px;" />
                                        </span>
                                    </div>
                                    <table id="tblBasisdaten" runat="server">
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                <asp:Label ID="lblKunde" runat="server" Text="Kunde*"></asp:Label>
                                            </td>
                                            <td class="active" align="left" nowrap="nowrap" style="width: 103%">
                                                <asp:TextBox ID="txtKunnr" runat="server" Width="75px" CssClass="InputSolid" TabIndex="1"></asp:TextBox>
                                                <asp:DropDownList runat="server" ID="ddlKunde" CssClass="InputSolid" TabIndex="2"/>
                                                <asp:Image ID="imgKunde" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                    Width="18px" Visible="False" />
                                                <asp:Label ID="lblKundeInfo" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                <asp:Label ID="lblKreise" runat="server" Text="StVA"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtStva" runat="server" Width="75px" TabIndex="3"
                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                <asp:DropDownList runat="server" ID="ddlStva" CssClass="InputSolid" TabIndex="4"/>
                                                <asp:Image ID="imgKreise" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                    Width="18px" Visible="False" />
                                                <asp:Label ID="lblKreiseInfo" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                <asp:Label ID="lblDienstleistung" runat="server" Text="Dienstleistung"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                <asp:TextBox ID="txtDienstleistung" runat="server" Width="75px" TabIndex="5"
                                                    CssClass="InputSolid"></asp:TextBox>
                                                <asp:DropDownList ID="ddlDienstleistung" runat="server" TabIndex="6"
                                                    CssClass="InputSolid">
                                                </asp:DropDownList>
                                                <asp:Image ID="imgDienstleistung" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                    Width="18px" Visible="False" />
                                                <asp:Label ID="lblDienstleisungInfo" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="3" align="right" class="firstLeft active" nowrap="nowrap">
                                                <asp:LinkButton ID="lbtAcceptBasisdaten" runat="server" CssClass="TablebuttonMiddle" Text="Übernehmnen" Width="100px" 
                                                Height="20px" TabIndex="7"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                &nbsp;
                                <div style="padding-left: 5px" runat="server">
                                    <asp:LinkButton ID="lbtGrunddaten" runat="server" Visible="false" tabindex="8" CssClass="TabButtonActive" 
                                        Text="Halterdaten" Width="100" Height="22" />
                                    <asp:LinkButton ID="lbtFahrzeugdaten" runat="server" Visible="false" tabindex="9" CssClass="TabButton" 
                                        Text="Fahrzeugdaten" Width="100" Height="22" />
                                    <asp:LinkButton ID="lbtDienstleistung" runat="server" Visible="false" tabindex="10" CssClass="TabButton" 
                                        Text="Dienstleistung" Width="100" Height="22" />
                                    <asp:LinkButton ID="lbtZusatzdienstleistungen" runat="server" Visible="false" tabindex="11" CssClass="TabButton" 
                                        Text="Zusatzdienstl." Width="100" Height="22" />
                                    <asp:LinkButton ID="lbtZusammenfassung" runat="server" Visible="false" tabindex="12" CssClass="TabButton" 
                                        Text="Zusammenfassung" Width="100" Height="22" />
                                </div>
                                <div id="TableQuery" runat="server">
                                    <div style="border: 1px solid #C0C0C0">
                                        <div id="Grunddaten" runat="server" visible="false">
                                            <asp:UpdatePanel ID="upKunde" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 635px">
                                                                <table>
                                                                    <tr runat="server" id="trSelectGrosskundeHalter" class="formquery">
                                                                        <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                        </td>
                                                                        <td class="active" style="width: 103%" nowrap="nowrap">
                                                                            <span>
                                                                                <asp:RadioButtonList ID="rblHalterauswahl" runat="server" RepeatDirection="Horizontal" TabIndex="13" AutoPostBack="True">
                                                                                    <asp:ListItem Value="Grosskunde" Text="Großkunde" Selected="True" />
                                                                                    <asp:ListItem Value="Halter" Text="Halter" />
                                                                                </asp:RadioButtonList>
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trGrossKunde" class="formquery">
                                                                        <td class="firstLeft active" style="width: 130px">
                                                                            <asp:Label ID="lblGrosskunde" runat="server" Text="Großkundennumer*"></asp:Label>
                                                                        </td>
                                                                        <td class="active" style="width: 103%">
                                                                            <asp:TextBox ID="txtGrosskundennummer" runat="server" MaxLength="6" Width="70px" 
                                                                                TabIndex="15" CssClass="InputSolid"></asp:TextBox>
                                                                            <asp:DropDownList ID="ddlGrosskunde" runat="server" TabIndex="16"
                                                                                CssClass="InputSolid">
                                                                            </asp:DropDownList>
                                                                            <asp:Image ID="imgGrossKunde" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                                Height="18px" Width="18px" Visible="False" />
                                                                            <asp:Label ID="lblGrosskundeInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <div id="divHalter" runat="server" visible="false">
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                <asp:Label ID="lblAnrede" runat="server" Text="Anrede*"></asp:Label>
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:DropDownList ID="ddlAnrede" runat="server" TabIndex="17" AutoPostBack="True"
                                                                                    CssClass="InputSolid">
                                                                                </asp:DropDownList>
                                                                                <asp:Image ID="imgAnrede" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                                                    Width="18px" Visible="False" />
                                                                                <asp:Label ID="lblAnredeInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                <asp:Label ID="lblName" runat="server" Text="Name1*"></asp:Label>
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:TextBox ID="txtName" runat="server" Width="330px" TabIndex="18" Style="margin-left: 0px"
                                                                                    MaxLength="60" CssClass="InputSolid"></asp:TextBox>
                                                                                <asp:Image ID="imgHaltername" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Image ID="imgHalternameNpa" runat="server" ImageUrl="../../../Images/nPALogoSmall.gif"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Label ID="lblNameInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                <asp:Label ID="lblName2" runat="server" Text="Name2*"></asp:Label>
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:TextBox ID="txtName2" runat="server" Width="330px" TabIndex="19" MaxLength="45"
                                                                                    CssClass="InputSolid"></asp:TextBox>
                                                                                <asp:Image ID="imgHaltername2" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Image ID="imgHaltername2Npa" runat="server" ImageUrl="../../../Images/nPALogoSmall.gif"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Label ID="lblName2Info" runat="server" CssClass="TextError"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trGeburtstag" runat="server" class="formquery" visible="false">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                Geburtstag
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:TextBox ID="txtGeburtstag" runat="server" MaxLength="10" TabIndex="20" Width="80px"
                                                                                    CssClass="InputSolid"></asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="txtGeburtstag_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtGeburtstag">
                                                                                </cc1:MaskedEditExtender>
                                                                                <asp:Label ID="lblGeburtstagInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                                <asp:Image ID="imgGeburtstagNpa" runat="server" ImageUrl="../../../Images/nPALogoSmall.gif"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trGeburtsort" runat="server" class="formquery" visible="false">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                Geburtsort
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:TextBox ID="txtGeburtsort" runat="server" TabIndex="25" Width="330px" MaxLength="40"
                                                                                    CssClass="InputSolid"></asp:TextBox>
                                                                                <asp:Label ID="lblGeburtsortInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                                <asp:Image ID="imgGeburtsortNpa" runat="server" ImageUrl="../../../Images/nPALogoSmall.gif"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                <asp:Label ID="lblStrasse" runat="server" Text="Strasse und Hausnr.*"></asp:Label>
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:TextBox ID="txtStrasse" runat="server" Width="257px" TabIndex="26" CssClass="InputSolid"></asp:TextBox>
                                                                                <asp:TextBox ID="txtHausnummer" runat="server" Width="30px" MaxLength="4" TabIndex="27"
                                                                                    CssClass="InputSolid"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="fteHausnummer" runat="server" TargetControlID="txtHausnummer"
                                                                                     FilterMode="ValidChars" FilterType="Numbers" ></cc1:FilteredTextBoxExtender>
                                                                                <asp:TextBox ID="txtHnrZusatz" runat="server" Width="30px" MaxLength="7" TabIndex="28"
                                                                                    CssClass="InputSolid"></asp:TextBox>
                                                                                <asp:Image ID="imgHalterstrasse" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Image ID="imgHalterstrasseNpa" runat="server" ImageUrl="../../../Images/nPALogoSmall.gif"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Label ID="lblStrasseInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                <asp:Label ID="lblOrt" runat="server" Text="PLZ und Ort*"></asp:Label>
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:TextBox ID="txtPLZ" runat="server" MaxLength="5" Width="50px" TabIndex="29"
                                                                                    CssClass="InputSolid" AutoPostBack="True"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender id="ftePLZ" runat="server" TargetControlID="txtPLZ" FilterMode="ValidChars" 
                                                                                    FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                                                <asp:TextBox ID="txtOrt" runat="server" Width="273px" TabIndex="30" CssClass="InputSolid"></asp:TextBox>
                                                                                <asp:DropDownList runat="server" ID="ddlOrt" Width="278px" TabIndex="31" CssClass="InputSolid" Visible="false"/>
                                                                                <asp:Image ID="imgHalterPlzOrt" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Image ID="imgHalterPlzOrtNpa" runat="server" ImageUrl="../../../Images/nPALogoSmall.gif"
                                                                                    Height="18px" Width="18px" Visible="False" />
                                                                                <asp:Label ID="lblOrtInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </div>
                                                                    <tr class="formquery">
                                                                        <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                            <asp:Label ID="lblReferenz" runat="server" Text="Referenzen"></asp:Label>
                                                                        </td>
                                                                        <td class="active" style="width: 103%" nowrap="nowrap">
                                                                            <asp:TextBox ID="txtReferenz" runat="server" Width="162px" TabIndex="32" MaxLength="40"
                                                                                CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                            <asp:TextBox ID="txtBestellnr" runat="server" MaxLength="40" TabIndex="33" Width="162px"
                                                                                CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <div id="divNpa" runat="server" visible="false" style="background-color: #DFDFDF;
                                                                    width: 260px; height: 240px; text-align: center; padding-top: 15px; float: left">
                                                                    Möchten Sie den neuen
                                                                    <br />
                                                                    elektronischen<br />
                                                                    Personalausweis<br />
                                                                    für die Identifizierung verwenden,<br />
                                                                    klicken Sie bitte hier:<br />
                                                                    <br />
                                                                    <asp:ImageButton ID="ibtNpaLogo" runat="server" Height="96px" Width="175px" ImageUrl="../../../Images/nPALogo.jpg" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2" style="background-color: #DFDFDF;">
                                                                <div style="height: 20px; vertical-align: middle;">
                                                                    <asp:Label ID="lblKundenInfoHeader" runat="server">Kundeninterne Information</asp:Label></div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                <asp:Label ID="lblVerkaeuferkuerzel" runat="server" Text="Verk&auml;uferk&uuml;rzel"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtVerkaeuferkuerzel" runat="server" MaxLength="4" Width="50px"
                                                                    TabIndex="34" CssClass="InputSolid"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                <asp:Label ID="lblKundenreferenz" runat="server" Text="Kundeninterne&lt;br /&gt;Referenz"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtKundenreferenz" runat="server" Width="330px" MaxLength="13" TabIndex="35"
                                                                    CssClass="InputSolid"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                <asp:Label ID="lblNotiz" runat="server" Text="Notiz"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtNotiz" runat="server" Width="330px" TabIndex="36" CssClass="InputSolid"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="active" colspan="2" align="right">
                                                                <asp:LinkButton ID="lbForwardGrundDat" runat="server" CssClass="Tablebutton" Width="78px" TabIndex="37">» Weiter </asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtKunnr" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtGrosskundennummer" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div id="Fahrzeugdaten" runat="server" visible="false">
                                            <table>
                                                <tr class="formquery" id="trHersteller" runat="server">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblHersteller" runat="server" Text="Hersteller"></asp:Label>
                                                    </td>
                                                    <td style="width: 15px">
                                                        <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="Feld 2.1 auf der ZB1"/>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtHersteller" runat="server" MaxLength="4" Width="45px" TabIndex="38"
                                                            CssClass="InputSolid"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="fteHersteller" runat="server" TargetControlID="txtHersteller"
                                                            FilterMode="ValidChars" FilterType="Numbers" ></cc1:FilteredTextBoxExtender>
                                                        <asp:Image ID="imgHersteller" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                            Height="18px" Width="18px" Visible="False" />
                                                        <asp:Label ID="lblHerstellerInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <asp:UpdatePanel ID="upTypdaten" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <tr class="formquery" id="trTypdaten" runat="server">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblTyp" runat="server" Text="Typ/Variante u. Version/Pr&uuml;fziffer"></asp:Label>
                                                            </td>
                                                            <td style="width: 15px">
                                                                <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="Feld 2.2 auf der ZB1"/>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtTyp" runat="server" MaxLength="3" Width="27px" TabIndex="39"
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:TextBox ID="txtVarianteVersion" runat="server" MaxLength="5" Width="35px" TabIndex="40"
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:TextBox ID="txtTypPruef" runat="server" MaxLength="1" Width="10px" AutoPostBack="true"
                                                                    CssClass="InputSolid TextUpperCase" TabIndex="41"></asp:TextBox>
                                                                <asp:Image ID="imgTyp" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                                    Width="18px" Visible="False" />
                                                                <asp:Image ID="imgTypInfo" runat="server" ImageUrl="../../../Images/Info01_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblTypInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery" id="trFahrzeugklasse" runat="server">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblFahrzeugklasse" runat="server" Text="Fahrzeugklasse"></asp:Label>*
                                                            </td>
                                                            <td style="width: 15px">
                                                                <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="Feld J auf der ZB1"/>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtFahrzeugklasse" runat="server" TabIndex="42" MaxLength="4" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:Image ID="imgFahrzeugklasse" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblFahrzeugklasseInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery" id="trAufbauArt" runat="server">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblAufbauArt" runat="server" Text="Art des Aufbaus"></asp:Label>*
                                                            </td>
                                                            <td style="width: 15px">
                                                                <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="Feld 4 auf der ZB1"/>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtAufbauArt" runat="server" TabIndex="43" MaxLength="4" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:Image ID="imgAufbauArt" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblAufbauArtInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblFahrgestellnummer" runat="server" Text="Fahrgestellnummer/Pr&uuml;fziffer"></asp:Label>*
                                                            </td>
                                                            <td style="width: 15px">
                                                                <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="Fahrgestellnummer: Feld E auf der ZB1&#013;Prüfziffer zur Fahrgestellnummer: Feld 3 auf der ZB1"/>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtFahrgestellnummer" runat="server" Width="160px" TabIndex="44"
                                                                    MaxLength="17" CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:TextBox ID="txtFinPruef" runat="server" AutoPostBack="true" Width="10px" TabIndex="45"
                                                                    MaxLength="1" CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:Image ID="imgFahrgestellnummer" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblPruefzifferInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery" id="trFarbe" runat="server">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblFarbe" runat="server" Text="Code zur Fahrzeugfarbe"></asp:Label>*
                                                            </td>
                                                            <td style="width: 15px">
                                                                <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="Feld 11 auf der ZB1"/>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:DropDownList ID="ddlFarbe" runat="server" 
                                                                    CssClass="InputSolid" TabIndex="46">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblFarbeInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblBriefnummer" runat="server" Text="Nr. ZBII"></asp:Label>*
                                                            </td>
                                                            <td style="width: 15px">
                                                                <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="Feld 16 auf der ZB1"/>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtBriefnummer" runat="server" TabIndex="47" MaxLength="8" AutoPostBack="true"
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:Image ID="imgBriefnummer" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblBriefnummerInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery" id="trNummerZB1" runat="server">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblNummerZB1" runat="server" Text="Nr. ZBI"></asp:Label>*
                                                            </td>
                                                            <td style="width: 15px">
                                                                <asp:Image runat="server" ImageUrl="../../../Images/fragezeichen_normal.png" ToolTip="siehe erste Seite der ZB1"/>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtNummerZB1_1" runat="server" TabIndex="48" MaxLength="3" Width="30px" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <span>-</span>
                                                                <asp:TextBox ID="txtNummerZB1_2" runat="server" TabIndex="49" MaxLength="1" Width="10px" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <span>-</span>
                                                                <asp:TextBox ID="txtNummerZB1_3" runat="server" TabIndex="50" MaxLength="1" Width="10px" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <span>-</span>
                                                                <asp:TextBox ID="txtNummerZB1_4" runat="server" TabIndex="51" MaxLength="3" Width="30px" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <span>/</span>
                                                                <asp:TextBox ID="txtNummerZB1_5" runat="server" TabIndex="52" MaxLength="2" Width="20px" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <span>-</span>
                                                                <asp:TextBox ID="txtNummerZB1_6" runat="server" TabIndex="53" MaxLength="5" Width="50px" 
                                                                    CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                                <asp:Image ID="imgNummerZB1" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblNummerZB1Info" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="3" style="background-color: #DFDFDF;">
                                                                <div style="height: 20px; vertical-align: middle;">
                                                                    <asp:Label ID="lblDetails" runat="server" Text="Details"></asp:Label></div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2">
                                                                <asp:Label ID="lblMarke" runat="server" Text="Marke"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblMarkeShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2">
                                                                <asp:Label ID="lblTypText" runat="server" Text="Typ"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblTypTextShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2">
                                                                <asp:Label ID="lblVariante" runat="server" Text="Variante"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblVarianteShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2">
                                                                <asp:Label ID="lblVersion" runat="server" Text="Version"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblVersionShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2">
                                                                <asp:Label ID="lblHandelsname" runat="server" Text="Handelsbezeichnung"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblHandelsnameShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2">
                                                                <asp:Label ID="lblHerstellerKurz" runat="server" Text="Hersteller-Kurzbezeichnung"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblHerstellerKurzShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="active" colspan="3" align="right">
                                                                <asp:LinkButton ID="lbForwardFzgDat" runat="server" CssClass="Tablebutton" Width="78px" TabIndex="54">» Weiter </asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtFinPruef" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtTypPruef" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtBriefnummer" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </table>
                                        </div>
                                        <div id="Dienstleistung" runat="server" visible="false">
                                            <table>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblEVB" runat="server" Text="eVB-Nummer"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtEVB" runat="server" MaxLength="7" TabIndex="55" CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                        <asp:Label ID="lblEVBInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblZulDatum" runat="server" Text="Datum Zulassung </br>(bzw. Abmeldung/ Änderungen) "></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtZulDatum" runat="server" Width="80px" TabIndex="56" CssClass="InputSolid"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtZulDatum_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtZulDatum">
                                                        </cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="meetxtZuldatum" runat="server" TargetControlID="txtZulDatum"
                                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <asp:Label ID="lblZulDatumInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblKennzeichen" runat="server" Text="Kennzeichen"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtKennz1" runat="server" MaxLength="3" Width="30px" TabIndex="57"
                                                            CssClass="InputSolid TextUpperCase"></asp:TextBox>-
                                                        <asp:TextBox ID="txtKennz2" runat="server" MaxLength="6" Width="70px" TabIndex="58"
                                                            CssClass="InputSolid TextUpperCase"></asp:TextBox>
                                                        <asp:Label ID="lblKennzeichenInfo" runat="server" CssClass="TextError"></asp:Label>                                                    
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trKennzAlt" runat="server" visible="false">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblKennzeichenAlt" runat="server" Text="Bisheriges Kennzeichen"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtKennzAlt1" runat="server" MaxLength="3" Width="30px" TabIndex="59"
                                                            CssClass="InputSolid TextUpperCase"></asp:TextBox>-
                                                        <asp:TextBox ID="txtKennzAlt2" runat="server" MaxLength="6" Width="70px" TabIndex="60"
                                                            CssClass="InputSolid TextUpperCase"></asp:TextBox>                                                         
                                                            <asp:Label ID="lblKennzeichenAltInfo" runat="server" CssClass="TextError"></asp:Label>                                                    
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblDienstBemerkung" runat="server" Text="Bemerkung"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtDienstBemerkung" runat="server" Width="330px" TabIndex="61" MaxLength="56"
                                                            CssClass="InputSolid"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trNaechsteHU" runat="server">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblNaechsteHU" runat="server" Text="Nächste HU"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtNaechsteHU" runat="server" Width="60px" TabIndex="62" MaxLength="6"
                                                            CssClass="InputSolid"></asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="MEENaechsteHU" runat="server" TargetControlID="txtNaechsteHU"
                                                            Mask="99/9999" MaskType="Number">
                                                        </cc1:MaskedEditExtender>
                                                        <asp:Label ID="Label4" runat="server">MM.JJJJ</asp:Label>
                                                        <asp:Label ID="lblNaechsteHUInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <table width="100%">
                                                            <tr class="formquery">
                                                                <td colspan="2" class="firstLeft active" style="background-color: #DFDFDF;">
                                                                    <div style="height: 20px; vertical-align: middle; width: 397px;">
                                                                        <asp:Label ID="Label1" runat="server" Text="Bankdaten"></asp:Label></div>
                                                                </td>
                                                                <td class="firstLeft active" style="background-color: #DFDFDF;width: 220px">
                                                                    <div style="height: 20px; vertical-align: middle">
                                                                        <asp:Label ID="Label2" runat="server" Text="Zusätze"></asp:Label></div>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 120px">
                                                                    <asp:Label ID="lblBLZ" runat="server" Text="BLZ"></asp:Label>
                                                                    <asp:Label ID="lblIBAN" runat="server" Text="IBAN" Visible="false"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 320px">
                                                                    <asp:TextBox ID="txtBlz" runat="server" Enabled="false" TabIndex="63" MaxLength="8"
                                                                        CssClass="InputSolid" Width="200px"></asp:TextBox>
                                                                    <asp:Label ID="lblBlzInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                    <asp:TextBox ID="txtIBAN" runat="server" Enabled="false" TabIndex="64" MaxLength="34"
                                                                        CssClass="InputSolid TextUpperCase" Visible="false" Width="200px"></asp:TextBox>
                                                                    <asp:Label ID="lblIBANInfo" runat="server"
                                                                        CssClass="TextError" Visible="false"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 220px">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxEinKennz" runat="server" Text="Nur ein Kennzeichen" TabIndex="65" />                                                                        
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 120px">
                                                                    <asp:Label ID="lblKontonummer" runat="server" Text="Kontonummer"></asp:Label>
                                                                    <asp:Label ID="lblSWIFT" runat="server" Text="SWIFT-BIC" Visible="false"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 320px">
                                                                    <asp:TextBox ID="txtKontonummer" runat="server" Enabled="false" TabIndex="66" MaxLength="10"
                                                                        CssClass="InputSolid" Width="200px"></asp:TextBox>
                                                                    <asp:Label ID="lblKontonrInfo" runat="server"
                                                                        CssClass="TextError"></asp:Label>
                                                                    <asp:TextBox ID="txtSWIFT" runat="server" Enabled="false" TabIndex="67" MaxLength="11"
                                                                        CssClass="InputSolid TextUpperCase" Visible="false" Width="200px" 
                                                                        ReadOnly="True" ToolTip="Wird automatisch gefüllt"></asp:TextBox>
                                                                    <asp:Label ID="lblSWIFTInfo" runat="server"
                                                                        CssClass="TextError" Visible="false"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 220px">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxKrad" runat="server" Text="Krad" TabIndex="68"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 120px">
                                                                    <asp:Label ID="lblEinzug" runat="server" Text="Einzugsermächtigung"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 320px">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxEinzug" runat="server" TabIndex="69" Enabled="false"></asp:CheckBox></span>
                                                                    <asp:Label ID="lblEinzugInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 220px">
                                                                    Typ
                                                                    <asp:DropDownList ID="ddlKennzTyp" runat="server" Font-Bold="True" TabIndex="70"
                                                                        CssClass="InputSolid">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 120px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active" style="width: 320px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active" style="width: 220px">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxWunschkennzFlag" runat="server" TabIndex="71" Text="Wunsch-Kennzeichen" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 120px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active" style="width: 320px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active" style="width: 220px">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxReserviert" runat="server" TabIndex="72" Text="Reserviert, Nr." />
                                                                        &nbsp;
                                                                        <asp:TextBox ID="txtReservNr" runat="server" Font-Bold="True" CssClass="InputSolid" TabIndex="73" Width="150px"></asp:TextBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trGutachten" runat="server">
                                                                <td colspan="3" class="firstLeft active" style="background-color: #DFDFDF;">
                                                                    <div style="height: 20px; vertical-align: middle; width: 397px;">
                                                                        <asp:Label ID="Label6" runat="server" Text="Gutachtendaten"></asp:Label></div>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trArtGenehmigung" runat="server">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="Label7" runat="server" Text="Art der Genehmigung"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trPrueforganisation" runat="server">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="lblPrueforganisation" runat="server" Text="Prüforganisation"></asp:Label>
                                                                </td>
                                                                <td colspan="2" class="active">
                                                                    <asp:DropDownList ID="ddlPrueforganisation" runat="server" 
                                                                        CssClass="InputSolid" AutoPostBack="true" TabIndex="74">
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="lblPrueforganisationInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trGutachtenNr" runat="server">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="lblGutachtenNr" runat="server" Text="Nummer des Gutachtens" CssClass=""></asp:Label>
                                                                </td>
                                                                <td colspan="2" class="active" style="width: 220px">
                                                                    <asp:TextBox ID="txtGutachtenNr" runat="server" Width="150px" CssClass="InputSolid"></asp:TextBox><asp:Label
                                                                        ID="lblGutachtenNrInfo" runat="server" CssClass="TextError" MaxLength="30" TabIndex="75"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="active" colspan="2" align="right">
                                                        <asp:LinkButton ID="lbForwardDL" runat="server" CssClass="Tablebutton" Width="78px" TabIndex="76">» Weiter </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="Zusatzdienstleistungen" runat="server" visible="false">
                                            <table style="width: 100%">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="background-color: #DFDFDF;" colspan="2">
                                                        <div style="height: 20px; vertical-align: middle;">
                                                            <asp:Label ID="lblZusatzDLTitle" runat="server" Text="Zusatzdienstleistungen"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="firstLeft active">
                                                        &nbsp;
                                                    </td>
                                                    <td class="active">
                                                        <asp:CheckBoxList runat="server" ID="cblZusatzDL"/>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="active" colspan="2" align="right">
                                                        <asp:LinkButton ID="lbForwardZusatzDL" runat="server" CssClass="Tablebutton" Width="78px" TabIndex="77">» Weiter </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="Zusammenfassung" runat="server" visible="false">
                                            <table style="width: 100%">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="background-color: #DFDFDF;" colspan="2">
                                                        <div style="height: 20px; vertical-align: middle;">
                                                            <asp:Label ID="lblZusHeader" runat="server" Text="Zusammenfassung"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusKunde" runat="server" Text="Kunde"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusKundeData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusStva" runat="server" Text="StVA"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusStvaData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusDL" runat="server" Text="Dienstleistung"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusDLData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusZuldat" runat="server" Text="Zulassungsdatum"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusZuldatData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusKennz" runat="server" Text="Kennzeichen"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusKennzData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZuseVB" runat="server" Text="eVB-Nummer"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZuseVBData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusNurEinKennz" runat="server" Text="Nur ein Kennzeichen"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusNurEinKennzData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusKrad" runat="server" Text="Krad"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusKradData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusKenneichenTyp" runat="server" Text="Kennzeichentyp"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusKenneichenTypData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusWunschKennz" runat="server" Text="Wunsch-Kennzeichen"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusWunschKennzData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusReserviertNr" runat="server" Text="Reserviert, Nr."></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusReserviertNrData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusZusatzDLs" runat="server" Text="Zusatzdienstleistungen"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:ListBox ID="listZusZusatzDLsData" runat="server" Width="100%"/>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" runat="server" ID="trZusBankdatenTitle">
                                                    <td colspan="2" class="firstLeft active" style="background-color: #DFDFDF;">
                                                        <div style="height: 20px; vertical-align: middle;">
                                                            Bankdaten (SEPA)
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusIBAN" runat="server" Text="IBAN"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusIBANData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblZusSWIFT" runat="server" Text="SWIFT-BIC"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:Label ID="lblZusSWIFTData" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="active" colspan="2" align="right">
                                                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px" TabIndex="78">» Absenden </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div>
                        <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                            PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="mb" runat="server" Width="240px" BackColor="White" DefaultButton="btnOK" Style="padding-left: 10px; padding-top: 15px;">
                            <div id="pnlDiv1" runat="server" style=" margin-bottom:10px; text-align: center">
                                <asp:Label ID="lblAdressMessage" runat="server" Text="Bitte geben Sie hier den Barcode ein:"
                                    Font-Bold="True"></asp:Label></div>
                            <div id="pnlDiv2" runat="server" style="padding-top: 15px; margin-bottom: 10px;
                                padding-bottom: 10px; text-align: center">
                                <asp:TextBox ID="txtBarcode" runat="server" MaxLength="13" CssClass="InputSolid"></asp:TextBox></div>
                            <div id="pnlDiv3" runat="server" style="text-align: center">
                                <asp:Label ID="lblSaveInfo" runat="server" Visible="false" Style="margin-bottom: 15px"></asp:Label></div>
                            <div id="divSuccess" runat="server" visible="False" style="text-align: center; background-color: #DFDFDF;
                                margin: 10px 10px 10px 0px; padding-top: 5px">
                                <b><span runat="server" ID="spanSuccessMessage">Ihr Auftrag wurde gespeichert.</span></b>
                                <br />
                                <br />
                                <div id="divPrintPDF" runat="server">
                                    Um sich Ihren Zulassungsantrag als PDF-Dokument ausgeben zu lassen klicken Sie bitte
                                    hier:
                                    <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Visible="true" ImageUrl="/services/images/ZulPDF.gif"
                                    ToolTip="PDF"></asp:ImageButton>
                                </div>
                            </div>
                            <table width="100%" style="text-align: center">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnOK" runat="server" Text="Übernehmen" CssClass="TablebuttonLarge"
                                            Font-Bold="True" Width="90px" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Ablehnen" CssClass="TablebuttonLarge"
                                            Font-Bold="true" Width="90px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="btnFake2" runat="server" Text="Fake" Style="display: none" />
                        <cc1:ModalPopupExtender ID="mpeAgeWarning" runat="server" TargetControlID="btnFake2"
                            PopupControlID="mb2" BackgroundCssClass="modalBackground" DropShadow="true">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="mb2" runat="server" Width="240px" BackColor="White" DefaultButton="btnAgeWarningHold" Style="padding-left: 10px; padding-top: 15px;">
                            Achtung!<br />Das von Ihnen eingegebene Geburtsdatum ergibt ein Alter von <18 oder >90 Jahren.<br />Bitte prüfen Sie diesen Wert vor dem Absenden ggf. noch einmal.<br />
                            <table width="100%" style="text-align: center">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnAgeWarningHold" runat="server" Text="Korrektur" CssClass="TablebuttonLarge"
                                            Font-Bold="True" Width="90px" />
                                        <asp:Button ID="btnAgeWarningContinue" runat="server" Text="Weiter" CssClass="TablebuttonLarge"
                                            Font-Bold="True" Width="90px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="btnFake3" runat="server" Text="Fake" Style="display: none" />
                        <cc1:ModalPopupExtender ID="mpeVinWarning" runat="server" TargetControlID="btnFake3"
                            PopupControlID="mb3" BackgroundCssClass="modalBackground" DropShadow="true">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="mb3" runat="server" Width="240px" BackColor="White" DefaultButton="btnVinWarningHold" Style="padding-left: 10px; padding-top: 15px;">
                            Achtung!<br />Für die angegebene Fahrgestellnummer wurde kein Eintrag zum Hersteller gefunden.<br />Bitte prüfen Sie diesen Wert vor dem Absenden ggf. noch einmal.<br />
                            <table width="100%" style="text-align: center">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnVinWarningHold" runat="server" Text="Korrektur" CssClass="TablebuttonLarge"
                                            Font-Bold="True" Width="90px" />
                                        <asp:Button ID="btnVinWarningContinue" runat="server" Text="Weiter" CssClass="TablebuttonLarge"
                                            Font-Bold="True" Width="90px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="height: 100px">
                    </div>
                    <div id="dataQueryFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
