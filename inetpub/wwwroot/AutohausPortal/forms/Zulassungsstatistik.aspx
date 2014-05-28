<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zulassungsstatistik.aspx.cs"
    Inherits="AutohausPortal.forms.Zulassungsstatistik" MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/Helper.js?16042012"></script>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <script type="text/javascript">
        function onClientFocus(sender, eventArgs) {
            var comboBox = sender;

            var input = comboBox.get_inputDomElement();
            input.focus();
            comboBox.selectText(0, comboBox.get_text().length);
        };
    </script>
        <telerik:RadAjaxManager ID="AjaxManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlKunnr1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz1" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz2" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz3" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz4" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="formulare">
        <!-- FORMULARLAYER1 -->
        <div class="formlayer">
            <div class="formlayer_top">
                <div class="formopener" id="formopener1" onclick="openforms(1);">
                    <img src="../images/transp.gif" width="42" height="42" alt="" />
                </div>
                <h2>
                    Zulassungsdaten
                </h2>
                <div class="ErrorMessageArea">
                    <h3>
                        <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text=""></asp:Label>      
                    </h3>
                </div>
                <div class="hinweispflicht" id="hinweispflicht1">
                    *Pflichtfelder
                </div>
            </div>
            <div class="formlayer_plus" style="display: block" id="form1">
                <!--formularbereich1-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <span style="float: right">Max. selektierbarer Zeitraum 90 Tage</span>
                            <h3>
                                Kunden- und Fahrzeugdaten</h3>
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                Kunde</div>
                            <!--dropdown-->
                            <div class="formbereich" id="divKunde" runat="server">
                                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" ClientEvents-OnResponseEnd="initiate2">
                                    <telerik:RadComboBox ID="ddlKunnr1" Width="604px" runat="server" LoadingMessage="Laden..."
                                        EnableItemCaching="True" EnableEmbeddedSkins="false" EnableLoadOnDemand="true"
                                        AutoPostBack="true" EmptyMessage="Kundennummer wählen" OnItemsRequested="ddlKunnr1_ItemsRequested"
                                        OnClientDropDownOpened="onClientFocus" MarkFirstMatch="true" OnSelectedIndexChanged="ddlKunnr1_SelectedIndexChanged"
                                        EnableTextSelection="true" ShowMoreResultsBox="True" EnableVirtualScrolling="True"
                                        Height="200px" ItemsPerRequest="10">
                                        <Localization NoMatches="Keine Treffer" ShowMoreFormatString="Eintrag &lt;b&gt;1&lt;/b&gt;-&lt;b&gt;{0}&lt;/b&gt; von &lt;b&gt;{1}&lt;/b&gt;" />
                                    </telerik:RadComboBox>
                                </telerik:RadAjaxPanel>
                            </div>
                            <!--dropdown-->
                            <!--formulardaten zeile1-->
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer2">
                                    <p>
Wählen Sie mit dem Drop-Down-Menü die Kundennummer aus, zu der Sie sich die Statistik anzeigen lassen möchten. Wählen Sie keine Kundennummer aus, so werden alle Ihre Kunden angezeigt.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten-->
                            <div class="trenner">
                                &nbsp;</div>
                        </div>
                        <div class="formlayer_plus_bot">
                            &nbsp;</div>
                    </div>
                </div>
                <!--formularbereich1-->
                <!--formularbereich2-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <h3>
                                Spezieller Datensatz</h3>
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                Referenzen</div>
                            <!--textinput-->
                            <div class="formfeld" id="Div3" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz1" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formfeld" id="Div4" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz2" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer3">
                                    <p>
Hier können Sie die Selektion nach den individuellen Zuordnungskriterien für die Aufträge eingrenzen, z.B. Name des Kunden, Fahrgestellnummer, Kennzeichen etc. Bitte füllen Sie die Felder entsprechend der Beispieltexte.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile1-->
                            <div class="trenner">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile2-->
                            <div class="formname">
                                Referenzen</div>
                            <!--textinput-->
                            <div class="formfeld" id="Div5" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz3" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formfeld" id="Div6" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz4" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>

                            <!--formulardaten zeile2-->
                            <div class="trenner">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile3-->
                            <div class="formname">
                                Kennzeichen
                            </div>
                            <div class="formfeld" id="formdiv97" style="width: 81px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtKennz1" runat="server" Style="width: 51px;
                                    text-transform: uppercase;" MaxLength="3" Text="CK"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formname" style="width: auto;">
                                -
                            </div>
                            <div class="formfeld" id="formdiv87" style="width: 162px; margin-right: 20px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtKennz2" runat="server" Style="width: 132px;
                                    text-transform: uppercase;" Text="XX9999" MaxLength="6"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer4">
                                    <p>
                                        Erfassen Sie hier das Kennzeichen, wenn Sie nach einem Datensatz zu einem bestimmten Kennzeichen suchen möchten.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile3-->
                        </div>
                        <!--formulardaten-->
                        <div class="formlayer_plus_bot">
                            &nbsp;</div>
                    </div>
                </div>
                <!--formularbereich2-->
                <!--formularbereich3-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;
                    </div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <h3>
                                Unspezifische Suche</h3>
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                Beauftragungsdatum von
                            </div>
                            <!--textinput-->
                            <!--datepicker-->
                            <div class="formfeld" id="divBDateVon" runat="server" style="width: 200px; margin-left: 0px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox ID="txtBeauftragtVon" runat="server" CssClass="formtext jqcalendar" Width="130px"
                                    MaxLength="10"></asp:TextBox>
                                <div class="formfeld_end_wide">
                                    <img src="/AutohausPortal/images/icon_datepicker.gif" width="22" height="19" alt="Kalender"
                                        class="datepicker" />
                                </div>
                            </div>
                            <!--datepicker-->
                            <div class="formname" style="margin-left: 14px; margin-right: 30px; width: auto;">
                                bis
                            </div>
                            <!--textinput-->
                            <!--datepicker-->
                            <div class="formfeld" id="divBDateBis" runat="server" style="width: 200px; margin-left: 0px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox ID="txtBeauftragtBis" runat="server" CssClass="formtext jqcalendar" Width="130px"
                                    MaxLength="10"></asp:TextBox>
                                <div class="formfeld_end_wide">
                                    <img src="/AutohausPortal/images/icon_datepicker.gif" width="22" height="19" alt="Kalender"
                                        class="datepicker" />
                                </div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="Div1">
                                    <p>
                                        Wählen Sie das Beauftragungsdatum von bis aus, um in dem Beauftragungszeitraum nach den Aufträgen zu suchen. 
                                        Wählen Sie das Zulassungsdatum von bis aus, um in dem Zulassungszeitraum nach den Aufträgen zu suchen.  
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--datepicker-->
                            <!--formulardaten zeile1-->
                            <div class="trenner">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                Zulassungsdatum von
                            </div>
                            <!--textinput-->
                            <!--datepicker-->
                            <div class="formfeld" id="divZulDateVon" runat="server" style="width: 200px; margin-left: 0px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox ID="txtZuldateVon" runat="server" CssClass="formtext jqcalendar" Width="130px"
                                    MaxLength="10"></asp:TextBox>
                                <div class="formfeld_end_wide">
                                    <img src="/AutohausPortal/images/icon_datepicker.gif" width="22" height="19" alt="Kalender"
                                        class="datepicker" />
                                </div>
                            </div>
                            <!--datepicker-->
                            <div class="formname" style="margin-left: 14px; margin-right: 30px; width: auto;">
                                bis
                            </div>
                            <!--textinput-->
                            <!--datepicker-->
                            <div class="formfeld"  id="divZulDateBis" runat="server" style="width: 200px; margin-left: 0px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox ID="txtZuldateBis" runat="server" CssClass="formtext jqcalendar" Width="130px"
                                    MaxLength="10"></asp:TextBox>
                                <div class="formfeld_end_wide">
                                    <img src="/AutohausPortal/images/icon_datepicker.gif" width="22" height="19" alt="Kalender"
                                        class="datepicker" />
                                </div>
                            </div>

                            <!--datepicker-->
                            <!--formulardaten zeile1-->
                            <div class="trenner">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile3-->
                            <div class="formname">
                                &nbsp;
                            </div>
                            <div class="formselects">
                                    <asp:RadioButton ID="rbAlle" GroupName="Auswahl" runat="server" 
                                        Checked="True" />
                                <div class="radiolabel">
                                    Alle Datensätze
                                </div>
                                <asp:RadioButton ID="rbDurch" GroupName="Auswahl" runat="server" />
                                <div class="radiolabel">
                                    Durchgeführte Zulassungen
                                </div>
                                <asp:RadioButton ID="rbOffen" GroupName="Auswahl" runat="server" />
                                <div class="radiolabel">
                                    Offene Zulassungen
                                </div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="Div2">
                                    <p>
                                         Über die Selektion „alle Datensätze“ werden Ihnen alle Vorgänge angezeigt. 
                                         Über die Selektion „Durchgeführte Zulassungen“ werden Ihnen die durchgeführten Vorgänge angezeigt. 
                                        Über die Selektion „Offene Zulassungen“ werden Ihnen die noch offenen Vorgänge angezeigt.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile3-->
                            <div class="trenner5">
                                &nbsp;
                            </div>
                        </div>
                        <div class="formlayer_plus_bot">
                            &nbsp;
                        </div>
                    </div>
                    <!--formulardaten-->
                </div>
                <!--formularbereich3-->
                <div class="trenner10">
                    &nbsp;
                </div>
            </div>
        </div>
        <!-- FORMULARLAYER1 -->
						<div class="linie">
							&nbsp;
						</div>
						<!--formbuttons-->
						<div class="formbuttons">
						    <asp:Button ID="cmdSearch" runat="server" CssClass="submitbutton" Text="Suchen" 
                                onclick="cmdSearch_Click" />
						</div>
						<!--formbuttons-->

    </div>
</asp:Content>
