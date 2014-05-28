<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="CKG.Components.ComCommon.Change01"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        var currentTabIndex = 0;

        function switchToTab(sender, args) { 
            // Active Tab Index actually gives us the tab the client clicked, not the current tab that we want to validate.               
            nextTabIndex = sender.get_activeTabIndex();
            
            // Figure out which validation group to check based on the current tab index
            switch (currentTabIndex) {
                case 0:
                    validationGroup = 'vgrGrunddaten';
                    break;
                case 1:
                    validationGroup = 'vgrFahrzeugdaten';
                    break;
                case 2:
                    validationGroup = 'vgrDienstleistung';
                    break;                    
            }
            if (Page_ClientValidate(validationGroup)) {
                // Save the current tab index so we can reference it next time.
                currentTabIndex = nextTabIndex;
            }
            else {
                // Setting the activeTab below will trigger this function again
                // So, to make sure we don't get caught in a loop, make sure these values aren't the same                
                if (nextTabIndex != currentTabIndex) sender.set_activeTab(sender.get_tabs()[currentTabIndex]);           
            }        
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
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TabCon" EventName="OnClientActiveTabChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="innerContentRightHeading">
                                <h1>
                                    <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                                </h1>
                            </div>
                            <div>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                            </div>
                            
                            <div>
                            <br />
                            * = Pflichteingabe</div>
                            <div>
                                <br />
                                <cc1:TabContainer ID="TabCon" runat="server" ActiveTabIndex="0" 
                                    AutoPostBack="true" OnClientActiveTabChanged="switchToTab">
                                    
                                    <cc1:TabPanel runat="server" HeaderText="Grunddaten" ID="TabPanel1">
                                        <ContentTemplate>
                                            <table>
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpKunde">
                                                <ContentTemplate>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblKunde" runat="server" Text="Kunde*"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtKunde" runat="server" Width="450px" AutoPostBack="true"></asp:TextBox><cc1:AutoCompleteExtender
                                                            ID="txtKunde_AutoCompleteExtender" runat="server" DelimiterCharacters="" Enabled="True"
                                                            ServicePath="../CommonService.asmx" TargetControlID="txtKunde" UseContextKey="True"
                                                            ServiceMethod="GetCustomerList" MinimumPrefixLength="1">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Image ID="imgKunde" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                        
                                                        <asp:Label ID="lblKundeInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtKunde" EventName="TextChanged" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpGrossKunde">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rdbKunde" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtGrosskundennummer" EventName="TextChanged" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:RadioButtonList ID="rdbKunde" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" TabIndex="1">
                                                                    <asp:ListItem Selected="True" Value="1">Großkunde</asp:ListItem>
                                                                    <asp:ListItem Value="2">Halter</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trGrossKunde" class="formquery">
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lblGrosskunde" runat="server" Text="Großkundennumer*"></asp:Label>
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtGrosskundennummer" runat="server" MaxLength="6" Width="70px" AutoPostBack="true" TabIndex="3"></asp:TextBox>
                                                                <asp:Image ID="imgGrossKunde" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                                        <asp:Label ID="lblKundeShow" runat="server" ForeColor="GrayText" style="vertical-align:top"></asp:Label>
                                                                <asp:Label
                                                                    ID="lblGrosskundeInfo" runat="server" CssClass="TextError" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <div id="divHalter" runat="server" visible="false">
                                                        
                                                        <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblAnrede" runat="server" Text="Anrede*"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlAnrede" runat="server" TabIndex="4"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblName" runat="server" Text="Haltername1*"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtName" runat="server" Width="330px" TabIndex="5"></asp:TextBox>
                                                                    <asp:Image ID="imgHaltername" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                                    <asp:Label ID="lblNameInfo"
                                                                        runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblName2" runat="server" Text="Haltername2*"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtName2" runat="server" Width="330px" TabIndex="6"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblStrasse" runat="server" Text="Strasse und Hausnummer*"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtStrasse" runat="server" Width="330px" TabIndex="7"></asp:TextBox>
                                                                    <asp:TextBox ID="txtHausnummer" runat="server" Width="50px" MaxLength="5" TabIndex="8"></asp:TextBox>
                                                                    <asp:Image ID="imgHalterstrasse" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                                    <asp:Label
                                                                        ID="lblStrasseInfo" runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblOrt" runat="server" Text="PLZ und Ort*"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtPLZ" runat="server" MaxLength="5" Width="50px" TabIndex="9"></asp:TextBox><asp:TextBox
                                                                        ID="txtOrt" runat="server" Width="273px" TabIndex="10"></asp:TextBox>
                                                                        <asp:Image ID="imgHalterPlzOrt" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                                        <asp:Label ID="lblOrtInfo"
                                                                            runat="server" CssClass="TextError"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </div>
                                                        
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblReferenz" runat="server" Text="Referenz"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtReferenz" runat="server" Width="330px" TabIndex="11"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="2" style="background-color: #DFDFDF;">
                                                        <div style="height: 20px; vertical-align: middle;">
                                                            <asp:Label ID="lblKundenInfoHeader" runat="server">Kundeninterne Information</asp:Label></div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblVerkaeuferkuerzel" runat="server" Text="Verk&auml;uferk&uuml;rzel"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtVerkaeuferkuerzel" runat="server" MaxLength="5" Width="50px" TabIndex="12"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblKundenreferenz" runat="server" Text="Kundeninterne Referenz"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtKundenreferenz" runat="server" Width="330px" MaxLength="13" TabIndex="13"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblNotiz" runat="server" Text="Notiz"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:TextBox ID="txtNotiz" runat="server" Width="330px" TabIndex="14"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel runat="server" HeaderText="Fahrzeugdaten" ID="TabPanel2">
                                        <ContentTemplate>
                                            <asp:UpdatePanel runat="server" ID="UP2" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblHersteller" runat="server" Text="(2.1)Hersteller*"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtHersteller" runat="server" MaxLength="4" Width="45px"></asp:TextBox>
                                                                <asp:Image ID="imgHersteller" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                                <asp:Label
                                                                    ID="lblHerstellerInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblTyp" runat="server" Text="(2.2)Typ/Variante u. Version/Pr&uuml;fziffer*"></asp:Label>
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtTyp" runat="server" MaxLength="3" Width="27px" TabIndex="1"></asp:TextBox><asp:TextBox
                                                                    ID="txtVarianteVersion" runat="server" MaxLength="5" Width="35px" TabIndex="2"></asp:TextBox><asp:TextBox
                                                                        ID="txtTypPruef" runat="server" MaxLength="1" Width="10px" AutoPostBack="true" TabIndex="3"></asp:TextBox>
                                                                       <asp:Image ID="imgTyp" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" /> 
                                                                        <asp:Image ID="imgTypInfo" runat="server" ImageUrl="../../../Images/Info01_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                                        <asp:Label
                                                                            ID="lblTypInfo" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                <asp:Label ID="lblFahrgestellnummer" runat="server" Text="Fahrgestellnummer/Pr&uuml;fziffer"></asp:Label>*
                                                            </td>
                                                            <td class="active" style="width: 100%" nowrap="nowrap">
                                                                <asp:TextBox ID="txtFahrgestellnummer" runat="server" Width="150px" TabIndex="4"></asp:TextBox><asp:TextBox
                                                                    ID="txtFinPruef" runat="server" AutoPostBack="true" Width="10px"></asp:TextBox>
                                                                    
                                                                    <asp:Image ID="imgFahrgestellnummer" runat="server" ImageUrl="../../../Images/ok_10.jpg"
                                                                        Height="18px" Width="18px" Visible="False" />
                                                                        <asp:Label ID="lblPruefzifferInfo" runat="server" CssClass="TextError"></asp:Label>
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
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtFinPruef" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtTypPruef" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel runat="server" HeaderText="Dienstleistung" ID="TabPanel3">
                                        
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="upDienstleistung" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtKreise" EventName="TextChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                            <table>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblDienstleistung" runat="server" Text="Dienstleistung"></asp:Label>
                                                    </td>
                                                    <td class="active" style="width: 100%" nowrap="nowrap">
                                                        <asp:DropDownList ID="ddlDienstleistung" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblDienstleisungInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblKreise" runat="server" Text="StVA"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtKreise" runat="server" Width="330px" AutoPostBack="true"></asp:TextBox><cc1:AutoCompleteExtender
                                                            ID="AutoCompleteExtender1" runat="server" DelimiterCharacters="" Enabled="True"
                                                            ServicePath="../CommonService.asmx" TargetControlID="txtKreise" UseContextKey="True"
                                                            ServiceMethod="GetKreise" MinimumPrefixLength="1">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Image ID="imgKreise" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px" Width="18px" Visible="False" />
                                                        <asp:Label ID="lblKreiseInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblEVB" runat="server" Text="eVB-Nummer"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtEVB" runat="server" MaxLength="7"></asp:TextBox>
                                                        <asp:Label ID="lblEVBInfo" runat="server" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblZulDatum" runat="server" Text="Datum Zulassung"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtZulDatum" runat="server" Width="80px"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtKennz1" runat="server" MaxLength="3" Width="30px"></asp:TextBox>-
                                                        <asp:TextBox ID="txtKennz2" runat="server" MaxLength="6" Width="70px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        <asp:Label ID="lblDienstBemerkung" runat="server" Text="Bemerkung"></asp:Label>
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtDienstBemerkung" runat="server" Width="330px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="2" style="background-color: #DFDFDF;">
                                                        <div style="height: 20px; vertical-align: middle;">
                                                            <asp:Label ID="Label2" runat="server" Text="Zusätze"></asp:Label></div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                        <asp:CheckBox ID="cbxEinKennz" runat="server" Text="Nur ein Kennzeichen"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                        <asp:CheckBox ID="cbxKrad" runat="server" Text="Krad"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                        Typ&nbsp;<asp:DropDownList ID="ddlKennzTyp" runat="server" Font-Bold="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                        <asp:CheckBox ID="cbxFeinstaub" runat="server" Text="Feinstaubplakette vom Amt">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                        <asp:CheckBox ID="cbxWunschkennzFlag" runat="server" Text="Wunsch-Kennzeichen"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                    </td>
                                                    <td class="active">
                                                        <asp:CheckBox ID="cbxReserviert" runat="server" Text="Reserviert, Nr."></asp:CheckBox>&nbsp;
                                                        <asp:TextBox ID="txtReservNr" runat="server" Font-Bold="True" Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="active" colspan="2" align="right">
                                                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Absenden </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer>
                            </div>
                            <div>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                                <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                                    X="450" Y="200">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="mb" runat="server" Width="240px" Height="150px" BackColor="White"
                                    Style="display: none">
                                    <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; text-align: center">
                                        <asp:Label ID="lblAdressMessage" runat="server" Text="Bitte geben Sie hier den Barcode ein:"
                                            Font-Bold="True"></asp:Label>
                                    </div>
                                    <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px;
                                        text-align: center">
                                        <asp:TextBox ID="txtBarcode" runat="server" MaxLength="12"></asp:TextBox>
                                        
                                    </div>
                                    <div style="text-align: center">
                                        <asp:Label ID="lblSaveInfo" runat="server" Visible="false" style="margin-bottom: 15px"></asp:Label>
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
