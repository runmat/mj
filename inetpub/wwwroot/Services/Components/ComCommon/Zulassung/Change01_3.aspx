<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_3.aspx.vb" Inherits="CKG.Components.ComCommon.Zulassung.Change01_3"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>
                <asp:HyperLink ID="lnkFahrzeugauswahl" runat="server" NavigateUrl="Change01_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                <a class="active">| Adressen/Zulassungsdaten</a>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                        <ContentTemplate>
                            <div id="TableQuery">
                                <input id="Hidden1" type="hidden" runat="server" />
                                <asp:Panel ID="Panel1" runat="server" Height="179px" Style="display: none">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr>
                                            <td colspan="3" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                <span style="font-weight: bold">Adresssuche:</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trErrorSearch" runat="server" visible="false">
                                            <td colspan="3" class="firstLeft active">
                                                <asp:Label ID="lblErrorSearch" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Name*:&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                PLZ*, Ort*:&nbsp;
                                            </td>
                                            <td class="active" style="width: 45%">
                                                <asp:TextBox ID="txtPLZ" runat="server" Width="50px"></asp:TextBox>
                                                &nbsp;
                                                <asp:TextBox ID="txtOrt" runat="server" Width="188px"></asp:TextBox>
                                                <asp:Button ID="btnSuchen" runat="server" Text="Suchen" Width="100px" />
                                            </td>
                                            <td class="active" style="width: 60%">
                                                Platzhaltersuche durch Verwendung des Zeichens * möglich.
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Auswahl:
                                            </td>
                                            <td class="active" colspan="2">
                                                <asp:DropDownList ID="drpAdresse" runat="server" Width="500px">
                                                    <asp:ListItem>Auswahl</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                            <td class="active" align="right" colspan="2">
                                                <asp:LinkButton ID="lbCancel" TabIndex="12" runat="server" CssClass="TablebuttonLarge"
                                                    Width="130px" Height="16px">» Abbrechen</asp:LinkButton>
                                                <asp:LinkButton ID="lbGet" TabIndex="12" runat="server" CssClass="TablebuttonLarge"
                                                    Width="130px" Height="16px">» Übernehmen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td align="right" class="active" colspan="2">
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlHalter" runat="server" Style="display: block">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="4" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                <span style="font-weight: bold">Halter:</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trError" runat="server">
                                            <td colspan="4" class="firstLeft active">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="width: 16%">
                                                Name1
                                            </td>
                                            <td class="active" style="width: 42%">
                                                <asp:TextBox ID="txtShName" runat="server" TabIndex="10" Width="295px" MaxLength="35"></asp:TextBox>
                                                <asp:ImageButton ID="ibtRefresh" runat="server" ImageUrl="/Services/images/Pfeile_Kreislauf_01.jpg"
                                                    ToolTip="Aktualisieren" Height="16px" Width="16px" Visible="False" />
                                                &nbsp;<asp:ImageButton ID="ibtSearchLN" runat="server" ImageUrl="/Services/images/Lupe_16x16.gif"
                                                    ToolTip="Adresssuche" Height="16px" Width="16px" OnClientClick="return false;" />
                                            </td>
                                            <td class="firstLeft active">
                                                Name2
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtShName2" runat="server" TabIndex="11" Width="295px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Strasse, Nr.*
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtShStrasse" runat="server" TabIndex="12" Width="295px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active">
                                                PLZ, Ort*
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShPLZ" runat="server" TabIndex="13" Width="50px"></asp:TextBox>
                                                &nbsp;<asp:TextBox ID="txtShOrt" runat="server" TabIndex="14" Width="238px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Land*
                                            </td>
                                            <td class="active">
                                                <asp:DropDownList ID="drpShLand" runat="server" TabIndex="16">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="active">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlZulDaten" runat="server" Style="display: block">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="11" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                <span style="font-weight: bold">Zulassungsdaten:</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Zulassungsdatum*
                                            </td>
                                            <td class="active" colspan="10">
                                                <asp:TextBox ID="txtZulassungsdatum" runat="server" Width="128px" TabIndex="17"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtZulassungsdatum_CE" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtZulassungsdatum">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ID="cv_txtZulassungsdatum" runat="server" ErrorMessage="Falsches Datumsformat"
                                                    Type="Date" ControlToValidate="txtZulassungsdatum" ControlToCompare="TextBox1"
                                                    Operator="DataTypeCheck" CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Zulassungskreis
                                            </td>
                                            <td class="active" colspan="10" style="width: 100%">
                                                <asp:TextBox ID="txtZulkreis" runat="server" TabIndex="17" Width="55px"></asp:TextBox>
                                                <asp:ImageButton ID="btnZulkreis" runat="server" Height="16px" ImageUrl="/Services/images/Lupe_16x16.gif"
                                                    ToolTip="Zulassungskreis ermitteln" Width="16px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trWunschkennz" runat="Server">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                Wunschkennzeichen 1.
                                            </td>
                                            <td class="active" style="width: 17%">
                                                <asp:TextBox ID="txtWKennzeichen1" runat="server" TabIndex="18" Width="78px"></asp:TextBox>
                                            </td>
                                            <td class="active">
                                                &nbsp;2.&nbsp;
                                            </td>
                                            <td class="active" style="width: 17%">
                                                <asp:TextBox ID="txtWKennzeichen2" runat="server" TabIndex="19" Width="78px"></asp:TextBox>
                                            </td>
                                            <td class="active">
                                                &nbsp;3.&nbsp;
                                            </td>
                                            <td class="active" style="width: 17%">
                                                <asp:TextBox ID="txtWKennzeichen3" runat="server" TabIndex="20" Width="78px"></asp:TextBox>
                                            </td>
                                            <td class="active" nowrap="nowrap">
                                                &nbsp;Res. Nr.&nbsp;
                                            </td>
                                            <td class="active" style="width: 17%">
                                                <asp:TextBox ID="txtResNr" runat="server" TabIndex="21" Width="78px"></asp:TextBox>
                                            </td>
                                            <td class="active" nowrap="nowrap">
                                                &nbsp;Res. Name&nbsp;
                                            </td>
                                            <td class="active" style="width: 70%">
                                                <asp:TextBox ID="txtResName" runat="server" TabIndex="22" Width="131px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trWunschInfo" runat="Server">
                                            <td class="firstLeft active">
                                                <span style="font-size: xx-small;">Bei reservierten
                                                    <br />
                                                    Wunschkennzeichen:</span>
                                            </td>
                                            <td class="active" colspan="10">
                                                <span style="color: #C20000; font-size: xx-small;">Ausdrucke der Reservierungsbestätigung
                                                    dringend dem DAD zukommen lassen!</span>
                                            </td>
                                        </tr>
                                        </tr>
                                        <tr runat="server" id="tr_Zulassungsart" class="formquery">
                                            <td class="firstLeft active">
                                                Zulassungsart
                                            </td>
                                            <td class="active" colspan="10">
                                                <span>
                                                    <asp:DropDownList ID="ddlZulassungsart" runat="server" TabIndex="23" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Feinstaubplakette
                                            </td>
                                            <td class="active" colspan="10">
                                                <span>
                                                    <asp:CheckBox ID="chkFeinstaub" runat="server" TabIndex="23" /></span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Tageszulassung
                                            </td>
                                            <td class="active" colspan="10">
                                                <span>
                                                    <asp:CheckBox ID="chkTagesul" runat="server" TabIndex="23" AutoPostBack="True" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="active" colspan="10">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="active" colspan="11">
                                                &nbsp;<asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlVersicherer" runat="server" Style="display: block">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="6" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                <span style="font-weight: bold">Abweichender Versicherungsnehmer:</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="6">
                                                <asp:CompareValidator ID="CV_EVB" runat="server" ErrorMessage="Datum gültig bis muss grösser sein als Datum gültig von!"
                                                    Type="Date" ControlToValidate="txtEVBBis" ControlToCompare="txtEVBVon" Operator="GreaterThan"
                                                    CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Vers.-gesellschaft
                                            </td>
                                            <td class="active" colspan="5">
                                                <asp:TextBox ID="txtVersGesellschaft" runat="server" TabIndex="24" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                eVB-Nummer*
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtEVBNummer" runat="server" Width="78px" MaxLength="7" TabIndex="26"></asp:TextBox>
                                            </td>
                                            <td class="active" nowrap="nowrap">
                                                <asp:ImageButton ID="ibtSearchEVB" runat="server" Height="16px" ImageUrl="/Services/images/Lupe_16x16.gif"
                                                    OnClientClick="return false;" ToolTip="Zulassungskreis ermitteln" Width="16px" />
                                                &nbsp;gültig von&nbsp;
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtEVBVon" runat="server" Width="78px" TabIndex="27"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtEVBVonCE" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtEVBVon">
                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                            <td class="active" nowrap="nowrap">
                                                &nbsp;bis&nbsp;
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txtEVBBis" runat="server" Width="78px" TabIndex="28"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtEVBBisCE" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtEVBBis">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ID="CV_txtEVBVon" runat="server" ErrorMessage="Falsches Datumsformat gültig von! "
                                                    Type="Date" ControlToValidate="txtEVBVon" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                    CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                <asp:CompareValidator ID="CV_txtEVBBis" runat="server" ErrorMessage="Falsches Datumsformat gültig bis! "
                                                    Type="Date" ControlToValidate="txtEVBBis" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                    CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Versicherungsnehmer*
                                            </td>
                                            <td class="active" colspan="5">
                                                <asp:TextBox ID="txtVersNehmer" runat="server" TabIndex="29" Width="295px" MaxLength="35"></asp:TextBox>
                                                <asp:ImageButton ID="ibtSearchVersicherer" runat="server" Height="16px" ImageUrl="/Services/images/Lupe_16x16.gif"
                                                    OnClientClick="return false;" ToolTip="Zulassungskreis ermitteln" Width="16px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Strasse, Nr.*
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtVersStrasse" runat="server" TabIndex="30" Width="295px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                PLZ, Ort*
                                            </td>
                                            <td class="active" colspan="5">
                                                <asp:TextBox ID="txtVersPLZ" runat="server" TabIndex="31" Width="50px" MaxLength="10"></asp:TextBox>
                                                &nbsp;<asp:TextBox ID="txtVersOrt" runat="server" TabIndex="33" Width="238px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Land*
                                            </td>
                                            <td class="active" colspan="5">
                                                <asp:DropDownList ID="drpVersLand" runat="server" TabIndex="34">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="active" colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlVSS" runat="server" Style="display: block">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="4" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                <span style="font-weight: bold">Versand Schein/Schilder:</span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="width: 16%">
                                                Name1
                                            </td>
                                            <td class="active" style="width: 42%">
                                                <asp:TextBox ID="txtVssName1" runat="server" TabIndex="35" Width="295px" MaxLength="35"></asp:TextBox>
                                                &nbsp;<asp:ImageButton ID="ibtSearchVSS" runat="server" ImageUrl="/Services/images/Lupe_16x16.gif"
                                                    ToolTip="Adresssuche" Height="16px" Width="16px" OnClientClick="return false;" />
                                            </td>
                                            <td class="firstLeft active">
                                                Name2
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txtVssName2" runat="server" TabIndex="36" Width="295px" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Strasse, Nr.*
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtVssStrasse" runat="server" TabIndex="37" Width="295px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active">
                                                PLZ, Ort*
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVssPLZ" runat="server" TabIndex="38" Width="50px"></asp:TextBox>
                                                &nbsp;<asp:TextBox ID="txtVssOrt" runat="server" TabIndex="39" Width="238px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Land*
                                            </td>
                                            <td class="active" colspan="3">
                                                <asp:DropDownList ID="drpVSSLand" runat="server" TabIndex="40">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" TabIndex="41">» Weiter</asp:LinkButton>
                                </div>
                            </div>
                            <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                            <ajaxToolkit:ModalPopupExtender ID="MPEZulDate" runat="server" TargetControlID="btnFake"
                                PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnNo">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="mb" runat="server" Width="360px" Height="115px" BackColor="White"
                                Style="display: none; border: solid 2px #bc2b2b">
                                <div style="padding-left: 20px; padding-top: 20px; margin-bottom: 10px;">
                                    <asp:Label ID="lblMessagePopUp" runat="server" Font-Bold="True" CssClass="TextError"
                                        Text="Das Zulassungsdatum liegt nur einen Tag in der Zukunft. Wollen Sie wirklich eine Expresszulassung beauftragen?"></asp:Label></div>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px; padding-left: 65px;">
                                            <asp:Button ID="btnYes" runat="server" Text="Ja" CssClass="TablebuttonLarge" Font-Bold="True"
                                                Width="90px" />
                                        </td>
                                        <td style="width: 100px; padding-right: 35px">
                                            <asp:Button ID="btnNo" runat="server" Text="Nein" CssClass="TablebuttonLarge" Font-Bold="True"
                                                Width="90px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <ajaxToolkit:AnimationExtender ID="AniHalter" runat="Server" TargetControlID="ibtSearchLN">
                                <Animations>
                                    <OnClick>
                                        <Sequence>
                                            <StyleAction AnimationTarget="Panel1" Attribute="display" Value="block"/>
                                            <FadeIn AnimationTarget="Panel1" Duration=".2" MaximumOpacity=".94"/>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" MaximumOpacity=".94" />  
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                           <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" MaximumOpacity=".94" />                                         
                                            </Parallel>                                            
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Attribute="display" Value="none"/> 
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_cmdSearch" Attribute="display" Value="none"/>                                            
                                            <ScriptAction script="SetValue('HALTER');" ></ScriptAction>                          
                                        </Sequence>
                                    </OnClick>
                                </Animations>
                            </ajaxToolkit:AnimationExtender>
                            <ajaxToolkit:AnimationExtender ID="AniVersicherer" runat="Server" TargetControlID="ibtSearchVersicherer">
                                <Animations>
                                    <OnClick>
                                        <Sequence>
                                            <StyleAction AnimationTarget="Panel1" Attribute="display" Value="block"/>
                                            <FadeIn AnimationTarget="Panel1" Duration=".2" MaximumOpacity=".94"/>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" MaximumOpacity=".94" />  
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" MaximumOpacity=".94" />                                         
                                            </Parallel>                                              
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Attribute="display" Value="none"/> 
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Attribute="display" Value="none"/>                                            
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_cmdSearch" Attribute="display" Value="none"/>                                                                                          
                                            <ScriptAction script="SetValue('VERSICHERER');" ></ScriptAction>                          
                                        </Sequence>
                                    </OnClick>
                                </Animations>
                            </ajaxToolkit:AnimationExtender>
                            <ajaxToolkit:AnimationExtender ID="AniVSS" runat="Server" TargetControlID="ibtSearchVSS">
                                <Animations>
                                    <OnClick>
                                        <Sequence>
                                            <StyleAction AnimationTarget="Panel1" Attribute="display" Value="block"/>
                                            <FadeIn AnimationTarget="Panel1" Duration=".2" MaximumOpacity=".94"/>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" MaximumOpacity=".94" />  
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" MaximumOpacity=".94" />                                         
                                            </Parallel>  
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Attribute="display" Value="none"/> 
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Attribute="display" Value="none"/>                                            
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_cmdSearch" Attribute="display" Value="none"/>                                                                                          
                                            <ScriptAction script="SetValue('VSS');" ></ScriptAction>                          
                                        </Sequence>
                                    </OnClick>
                                </Animations>
                            </ajaxToolkit:AnimationExtender>
                            <ajaxToolkit:AnimationExtender ID="AniEVB" runat="Server" TargetControlID="ibtSearchEVB">
                                <Animations>
                                    <OnClick>
                                        <Sequence>
                                            <StyleAction AnimationTarget="Panel1" Attribute="display" Value="block"/>
                                            <FadeIn AnimationTarget="Panel1" Duration=".2" MaximumOpacity=".94"/>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" MaximumOpacity=".94" />  
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" MaximumOpacity=".94" />                                         
                                            </Parallel>
                                            <Parallel AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Duration=".2">
                                            <Fadeout AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" MaximumOpacity=".94" />                                         
                                            </Parallel>  
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlHalter" Attribute="display" Value="none"/> 
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlZulDaten" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVersicherer" Attribute="display" Value="none"/>
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_pnlVSS" Attribute="display" Value="none"/>                                            
                                            <StyleAction AnimationTarget="ctl00_ContentPlaceHolder1_cmdSearch" Attribute="display" Value="none"/>                                                                                          
                                            <ScriptAction script="SetValue('EVB');" ></ScriptAction>                          
                                        </Sequence>
                                    </OnClick>
                                </Animations>
                            </ajaxToolkit:AnimationExtender>
                            <script type="text/javascript" language="javascript">
                                function SetValue(art) {
                                    var hiddenInput = document.getElementById('ctl00_ContentPlaceHolder1_Hidden1');
                                    hiddenInput.value = art;
                                    var InputName = document.getElementById('ctl00_ContentPlaceHolder1_txtName');
                                    InputName.focus();

                                }
                            </script>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
