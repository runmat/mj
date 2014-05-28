<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02s.aspx.vb" Inherits="CKG.Components.ComCommon.Change02s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function autotab(original, destination) {
            if (original.getAttribute && original.value.length == original.getAttribute("maxlength"))
                destination.focus()
        }</script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <%--<asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>--%>
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
                            <ContentTemplate>
                                <div>
                                    <asp:Button ID="btnDefault" runat="server" Text="" Width="0px" Height="0px" BackColor="White"
                                        Style="height: 0px; width: 0px" BorderStyle="None" />
                                </div>
                                <div style="padding-left: 5px">
                                    <asp:ImageButton ID="ibtGrunddaten" runat="server" ImageUrl="../../../Images/GrunddatenActive.jpg" />
                                    <asp:ImageButton ID="ibtFahrzeugdaten" runat="server" ImageUrl="../../../Images/Fahrzeugdaten.jpg" />
                                    <asp:ImageButton ID="ibtDienstleistung" runat="server" ImageUrl="../../../Images/Dienstleistung.jpg" />
                                </div>
                                <div id="TableQuery">
                                    <div style="border: 1px solid #C0C0C0">
                                        <div id="Grunddaten" runat="server">
                                            <asp:UpdatePanel ID="upKunde" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 635px">
                                                                <table>
                                                                    <tr class="formquery">
                                                                        <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                            <asp:Label ID="lblKunde" runat="server" Text="Kunde*"></asp:Label>
                                                                        </td>
                                                                        <td class="active" align="left" nowrap="nowrap" style="width: 103%">
                                                                            <asp:TextBox ID="txtKunde" runat="server" Width="330px" AutoPostBack="true"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender
                                                                                ID="txtKunde_AutoCompleteExtender" runat="server" DelimiterCharacters="" Enabled="True"
                                                                                ServicePath="../CommonService.asmx" TargetControlID="txtKunde" UseContextKey="True"
                                                                                ServiceMethod="GetCustomerList" MinimumPrefixLength="1" CompletionInterval="100"
                                                                                EnableCaching="true">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <asp:Image ID="imgKunde" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                                                Width="18px" Visible="False" />
                                                                            <asp:Label ID="lblKundeInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="formquery">
                                                                        <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                        </td>
                                                                        <td class="active" style="width: 103%" nowrap="nowrap">
                                                                            <span>
                                                                                <asp:RadioButton ID="rdbGrosskunde" runat="server" AutoPostBack="true" Checked="True"
                                                                                    Text="Großkunde" />
                                                                                <asp:RadioButton ID="rdbHalter" runat="server" AutoPostBack="true" Text="Halter" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trGrossKunde" class="formquery">
                                                                        <td class="firstLeft active" style="width: 130px">
                                                                            <asp:Label ID="lblGrosskunde" runat="server" Text="Großkundennumer*"></asp:Label>
                                                                        </td>
                                                                        <td class="active" style="width: 103%">
                                                                            <asp:TextBox ID="txtGrosskundennummer" runat="server" MaxLength="6" Width="70px"
                                                                                AutoPostBack="true" TabIndex="3"></asp:TextBox>
                                                                            <asp:Image ID="imgGrossKunde" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                                Height="18px" Width="18px" Visible="False" />
                                                                            <asp:Label ID="lblKundeShow" runat="server" ForeColor="GrayText" Style="vertical-align: top"></asp:Label>
                                                                            <asp:Label ID="lblGrosskundeInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <div id="divHalter" runat="server" visible="false">
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                <asp:Label ID="lblAnrede" runat="server" Text="Anrede*"></asp:Label>
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:DropDownList ID="ddlAnrede" runat="server" TabIndex="4" AutoPostBack="True">
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
                                                                                <asp:TextBox ID="txtName" runat="server" Width="330px" TabIndex="5" Style="margin-left: 0px" MaxLength="60"></asp:TextBox>
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
                                                                                <asp:TextBox ID="txtName2" runat="server" Width="330px" TabIndex="6" MaxLength="45"></asp:TextBox>
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
                                                                                <asp:TextBox ID="txtGeburtstag" runat="server" MaxLength="10" TabIndex="7" Width="80px"></asp:TextBox>
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
                                                                        <tr id="trGeburtsort" runat="server" class="formquery" visible="true">
                                                                            <td class="firstLeft active" style="width: 130px">
                                                                                Geburtsort
                                                                            </td>
                                                                            <td class="active" style="width: 103%">
                                                                                <asp:TextBox ID="txtGeburtsort" runat="server" TabIndex="8" Width="330px" MaxLength="40"></asp:TextBox>
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
                                                                                <asp:TextBox ID="txtStrasse" runat="server" Width="257px" TabIndex="9"></asp:TextBox>
                                                                                <asp:TextBox ID="txtHausnummer" runat="server" Width="30px" MaxLength="4" 
                                                                                    TabIndex="10"></asp:TextBox>
                                                                                    <asp:TextBox ID="txtHnrZusatz" runat="server" Width="30px" MaxLength="7" 
                                                                                    TabIndex="10"></asp:TextBox>
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
                                                                                <asp:TextBox ID="txtPLZ" runat="server" MaxLength="5" Width="50px" TabIndex="11"></asp:TextBox>
                                                                                <asp:TextBox ID="txtOrt" runat="server" Width="273px" TabIndex="12"></asp:TextBox>
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
                                                                            <asp:Label ID="lblReferenz" runat="server" Text="Referenz"></asp:Label>
                                                                        </td>
                                                                        <td class="active" style="width: 103%" nowrap="nowrap">
                                                                            <asp:TextBox ID="txtReferenz" runat="server" Width="330px" TabIndex="13" MaxLength="40"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="formquery">
                                                                        <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                            <asp:Label ID="lblBestellnr" runat="server" Text="Bestellnummer"></asp:Label>
                                                                        </td>
                                                                        <td class="active" nowrap="nowrap" style="width: 103%">
                                                                            <asp:TextBox ID="txtBestellnr" runat="server" MaxLength="40" TabIndex="13" Width="330px"></asp:TextBox>
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
                                                                    TabIndex="14"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                <asp:Label ID="lblKundenreferenz" runat="server" Text="Kundeninterne&lt;br /&gt;Referenz"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtKundenreferenz" runat="server" Width="330px" MaxLength="13" TabIndex="15"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                                                <asp:Label ID="lblNotiz" runat="server" Text="Notiz"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtNotiz" runat="server" Width="330px" TabIndex="16"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtKunde" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtGrosskundennummer" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div id="Fahrzeugdaten" runat="server" visible="false">
                                            <table>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblHersteller" runat="server" Text="(2.1)Hersteller*"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtHersteller" runat="server" MaxLength="4" Width="45px" TabIndex="15"></asp:TextBox>
                                                        <asp:Image ID="imgHersteller" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                            Height="18px" Width="18px" Visible="False" />
                                                        <asp:Label ID="lblHerstellerInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <asp:UpdatePanel ID="upTypdaten" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblTyp" runat="server" Text="(2.2)Typ/Variante u. Version/Pr&uuml;fziffer*"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtTyp" runat="server" MaxLength="3" Width="27px" TabIndex="16"></asp:TextBox><asp:TextBox
                                                                    ID="txtVarianteVersion" runat="server" MaxLength="5" Width="35px" TabIndex="17"></asp:TextBox><asp:TextBox
                                                                        ID="txtTypPruef" runat="server" MaxLength="1" Width="10px" AutoPostBack="true"
                                                                        TabIndex="18"></asp:TextBox>
                                                                <asp:Image ID="imgTyp" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                                    Width="18px" Visible="False" />
                                                                <asp:Image ID="imgTypInfo" runat="server" ImageUrl="../../../Images/Info01_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblTypInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblFahrgestellnummer" runat="server" Text="Fahrgestellnummer/Pr&uuml;fziffer"></asp:Label>*
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtFahrgestellnummer" runat="server" Width="150px" TabIndex="19"
                                                                    MaxLength="17"></asp:TextBox><asp:TextBox ID="txtFinPruef" runat="server" AutoPostBack="true"
                                                                        Width="10px" TabIndex="20" MaxLength="1"></asp:TextBox>
                                                                <asp:Image ID="imgFahrgestellnummer" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblPruefzifferInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblBriefnummer" runat="server" Text="Nr. ZBII"></asp:Label>*
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtBriefnummer" runat="server" TabIndex="21" MaxLength="8" AutoPostBack="true"></asp:TextBox>
                                                                <asp:Image ID="imgBriefnummer" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                    Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label ID="lblBriefnummerInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="2" style="background-color: #DFDFDF;">
                                                                <div style="height: 20px; vertical-align: middle;">
                                                                    <asp:Label ID="lblDetails" runat="server" Text="Details"></asp:Label></div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lblMarke" runat="server" Text="Marke"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblMarkeShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lblTypText" runat="server" Text="Typ"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblTypTextShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lblVariante" runat="server" Text="Variante"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblVarianteShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lblVersion" runat="server" Text="Version"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblVersionShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lblHandelsname" runat="server" Text="Handelsbezeichnung"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblHandelsnameShow" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lblHerstellerKurz" runat="server" Text="Hersteller-Kurzbezeichnung"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:Label ID="lblHerstellerKurzShow" runat="server"></asp:Label>
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
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblKreise" runat="server" Text="StVA"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:UpdatePanel ID="upDienstleistung" runat="server" UpdateMode="Conditional">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtKreise" EventName="TextChanged" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtKreise" runat="server" Width="330px" AutoPostBack="true" 
                                                                    TabIndex="23"></asp:TextBox><cc1:AutoCompleteExtender
                                                                    ID="AutoCompleteExtender1" runat="server" DelimiterCharacters="" Enabled="True"
                                                                    ServicePath="../CommonService.asmx" TargetControlID="txtKreise" UseContextKey="True"
                                                                    ServiceMethod="GetKreise" MinimumPrefixLength="1">
                                                                </cc1:AutoCompleteExtender>
                                                                <asp:Image ID="imgKreise" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                                                    Width="18px" Visible="False" />
                                                                <asp:Label ID="lblKreiseInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                               
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblDienstleistung" runat="server" Text="Dienstleistung"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:DropDownList ID="ddlDienstleistung" runat="server" AutoPostBack="true" 
                                                            TabIndex="24" Width="330px" >
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblDienstleisungInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblEVB" runat="server" Text="eVB-Nummer"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtEVB" runat="server" MaxLength="7" TabIndex="25"></asp:TextBox>
                                                        <asp:Label ID="lblEVBInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblZulDatum" runat="server" Text="Datum Zulassung"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtZulDatum" runat="server" Width="80px" TabIndex="26"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtKennz1" runat="server" MaxLength="3" Width="30px" TabIndex="27"></asp:TextBox>-
                                                        <asp:TextBox ID="txtKennz2" runat="server" MaxLength="6" Width="70px" TabIndex="28"></asp:TextBox>
                                                        <asp:Label ID="lblKennzeichenInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblDienstBemerkung" runat="server" Text="Bemerkung"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtDienstBemerkung" runat="server" Width="330px" TabIndex="29" MaxLength="40"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trNaechsteHU" runat="server">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblNaechsteHU" runat="server" Text="Nächste HU"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtNaechsteHU" runat="server" Width="60px" TabIndex="29" MaxLength="6"></asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="MEENaechsteHU" runat="server" TargetControlID="txtNaechsteHU"
                                                            Mask="99/9999" MaskType="Date">
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
                                                                <td class="firstLeft active" style="background-color: #DFDFDF;">
                                                                    <div style="height: 20px; vertical-align: middle;">
                                                                        <asp:Label ID="Label2" runat="server" Text="Zusätze"></asp:Label></div>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="Label3" runat="server" Text="BLZ"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 248px">
                                                                    <asp:TextBox ID="txtBlz" runat="server" Enabled="false" TabIndex="30" MaxLength="8"></asp:TextBox><asp:Label
                                                                        ID="lblBlzInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxEinKennz" runat="server" Text="Nur ein Kennzeichen" TabIndex="33">
                                                                        </asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="lblKontonummer" runat="server" Text="Kontonummer"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 248px">
                                                                    <asp:TextBox ID="txtKontonummer" runat="server" Enabled="false" TabIndex="31" MaxLength="10"></asp:TextBox><asp:Label
                                                                        ID="lblKontonrInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxKrad" runat="server" Text="Krad" TabIndex="34"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="Label5" runat="server" Text="Einzugsermächtigung"></asp:Label>
                                                                </td>
                                                                <td class="active" style="width: 248px">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxEinzug" runat="server" TabIndex="32" Enabled="false"></asp:CheckBox></span>
                                                                    <asp:Label ID="lblEinzugInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    Typ
                                                                    <asp:DropDownList ID="ddlKennzTyp" runat="server" Font-Bold="True" TabIndex="35">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active" style="width: 248px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxFeinstaub" runat="server" TabIndex="36" Text="Feinstaubplakette vom Amt" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active" style="width: 248px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxWunschkennzFlag" runat="server" TabIndex="37" Text="Wunsch-Kennzeichen" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active" style="width: 248px">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxReserviert" runat="server" TabIndex="38" Text="Reserviert, Nr." />
                                                                        &nbsp;
                                                                        <asp:TextBox ID="txtReservNr" runat="server" Font-Bold="True" TabIndex="38" Width="150px"></asp:TextBox></span>
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
                                                                <td colspan="2" class="active">
                                                                    <asp:DropDownList ID="ddlArtGenehmigung" runat="server" DataTextField="DDTEXT" DataValueField="DOMVALUE_L">
                                                                        <%--  <asp:ListItem Text="konform" Value="K" />
                                                                        <asp:ListItem Text="angepasst" Value="A" />
                                                                        <asp:ListItem Text="Einzelgenehmigung" Value="E" />--%>
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="lblArtGenehmigungInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trPrueforganisation" runat="server">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="lblPrueforganisation" runat="server" Text="Prüforganisation"></asp:Label>
                                                                </td>
                                                                <td colspan="2" class="active">
                                                                    <asp:DropDownList ID="ddlPrueforganisation" runat="server" DataTextField="DDTEXT"
                                                                        DataValueField="DOMVALUE_L">
                                                                        <%--<asp:ListItem Text="DEKRA" Value="DEKRA"></asp:ListItem>
                                                                        <asp:ListItem Text="TÜV Rheinland" Value="TUEV_RL"></asp:ListItem>
                                                                        <asp:ListItem Text="TÜV Nord" Value="TUEV_NORD"></asp:ListItem>
                                                                        <asp:ListItem Text="TÜV Süd" Value="TUEV_SUED"></asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="lblPrueforganisationInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trGutachtenNr" runat="server">
                                                                <td class="firstLeft active" style="width: 141px">
                                                                    <asp:Label ID="lblGutachtenNr" runat="server" Text="Nummer des Gutachtens" CssClass=""></asp:Label>
                                                                </td>
                                                                <td colspan="2" class="active">
                                                                    <asp:TextBox ID="txtGutachtenNr" runat="server" Width="150px"></asp:TextBox><asp:Label
                                                                        ID="lblGutachtenNrInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="active" colspan="2" align="right">
                                                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Absenden </asp:LinkButton>
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
                        <asp:Panel ID="mb" runat="server" Width="240px" BackColor="White" DefaultButton="btnOK">
                            <div id="pnlDiv1" runat="server" style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;
                                text-align: center">
                                <asp:Label ID="lblAdressMessage" runat="server" Text="Bitte geben Sie hier den Barcode ein:"
                                    Font-Bold="True"></asp:Label></div>
                            <div id="pnlDiv2" runat="server" style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;
                                padding-bottom: 10px; text-align: center">
                                <asp:TextBox ID="txtBarcode" runat="server" MaxLength="13"></asp:TextBox></div>
                            <div id="pnlDiv3" runat="server" style="text-align: center">
                                <asp:Label ID="lblSaveInfo" runat="server" Visible="false" Style="margin-bottom: 15px"></asp:Label></div>
                            <div id="divPdf" runat="server" visible="false" style="text-align: center; background-color: #DFDFDF;
                                margin: 10px 10px 10px 10px; padding-top: 5px">
                                <b>Ihr Auftrag wurde gespeichert.</b>
                                <br />
                                <br />
                                Um sich Ihren Zulassungsantrag als PDF-Dokument ausgeben zu lassen klicken Sie bitte
                                hier:
                                <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Visible="true" ImageUrl="/services/images/ZulPDF.gif"
                                    ToolTip="PDF"></asp:ImageButton>
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
