<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Neuzulassung.aspx.cs" Inherits="AutohausPortal.forms.Neuzulassung"
    MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="BemerkungenNotizen" Src="..\Controls\BemerkungenNotizen.ascx" %>
<%@ Register TagPrefix="uc" TagName="BankdatenAdresse" Src="..\Controls\BankdatenAdresse.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/Helper.js?16042012"></script>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <script type="text/javascript">
        function onClientFocus(sender, eventArgs) {
            var comboBox = sender;

            var input = comboBox.get_inputDomElement();
            input.focus();
        };
        function ChangeColor(sender, eventArgs) {
            var comboBox = sender;

            var input = comboBox.get_inputDomElement();
            $(input).css("color", "#000");
            if (comboBox._uniqueId == "ctl00$ContentPlaceHolder1$ddlStVa1") {
                var wert = eventArgs.get_item().get_value();
                var txtKennz1 = $("#<%= txtKennz1.ClientID %>");
                var txtWunschKZ21 = $("#<%= txtWunschKZ21.ClientID %>");
                var txtWunschKZ31 = $("#<%= txtWunschKZ31.ClientID %>");
                txtKennz1.val(wert);
                txtWunschKZ21.val(wert);
                txtWunschKZ31.val(wert);
                if (ArraySonderStva != null) {
                    for (var i = 0; i < ArraySonderStva.length; i++) {
                        var value = ArraySonderStva[i];
                        if (value[0] == wert) {
                            txtKennz1.val(value[1]);
                            txtWunschKZ21.val(value[1]);
                            txtWunschKZ31.val(value[1]);
                        }
                    }
                }

                enableDefaultValue("ctl00_ContentPlaceHolder1_txtKennz1");
                enableDefaultValue("ctl00_ContentPlaceHolder1_txtWunschKZ21");
                enableDefaultValue("ctl00_ContentPlaceHolder1_txtWunschKZ31");
                txtKennz1.attr("onblur", "");
                txtWunschKZ21.attr("onblur", "");
                txtWunschKZ31.attr("onblur", "");
                txtKennz1.attr("onfocus", "");
                txtWunschKZ21.attr("onfocus", "");
                txtWunschKZ31.attr("onfocus", "");
                txtKennz1.css("color", "#000");
                txtWunschKZ21.css("color", "#000");
                txtWunschKZ31.css("color", "#000");
            }
        }
        function OnClientItemsRequesting(sender, eventArgs) {
            var combo = sender;
            var item = combo.findItemByValue(combo._filterText.toUpperCase());
            if (item) {
                item.select();
            }
        }

        function onClientClose(oWnd, args) {
            __doPostBack('RadWindow1', '');
        }
    </script>
       <telerik:RadAjaxManager ID="AjaxManager" runat="server" ClientEvents-OnResponseEnd ="initiate2">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlKunnr1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz1" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz2" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz3" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz4" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlStVa1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtKennz1" />
                    <telerik:AjaxUpdatedControl ControlID="txtWunschKZ21" />
                    <telerik:AjaxUpdatedControl ControlID="txtWunschKZ31" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>    
    <div class="formulare">
        <!-- WICHTIG:
						Für den IE6 ist es leider notwendig, sowohl die Breite der Formfelder als auch die der umgebenden Layer zu definieren.
						Dabei muß der umgebende Layer immer 30 Pixel breiter sein, als das umschließende Formfeld.
						Wenn ein Input-Feld mit einem eingelagerten Button benötigt wird (z.B. ein Datepicker), muß dieser Wert nochmals um
						25 erhöht werden.
						-->
        <!-- FORMULARLAYER1 -->
        <div class="formlayer">
            <div class="formlayer_top">
                <div class="formopener" id="formopener1" onclick="openforms(1);">
                    <img src="../images/transp.gif" width="42" height="42" alt="" />
                </div>
                <h2>
                    Auftragsdaten
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

            <div class="formlayer_plus" id="form1">
                <!--formularbereich1-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <h3>
                                Kunden- und Fahrzeugdaten</h3>
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                Kunde*</div>
                            <!--dropdown-->
                            <div class="formbereich" id="divKunde" runat="server">
                             <telerik:RadComboBox ID="ddlKunnr1" Width="604px" runat="server" 
                                     LoadingMessage="Laden..." EnableItemCaching = "True"
                                    EnableEmbeddedSkins="false" EnableLoadOnDemand="true" AutoPostBack="true" 
                                    EmptyMessage="Kundennummer wählen"
                                     OnItemsRequested="ddlKunnr1_ItemsRequested" OnClientDropDownOpened="onClientFocus"
                                    MarkFirstMatch="true"  OnClientSelectedIndexChanged="ChangeColor"
                                     OnSelectedIndexChanged="ddlKunnr1_SelectedIndexChanged" EnableTextSelection="true"
                                    ShowMoreResultsBox="True" EnableVirtualScrolling="True" Height="200px" 
                                     ItemsPerRequest="10">
                                    <Localization NoMatches="Keine Treffer" ShowMoreFormatString="Eintrag &lt;b&gt;1&lt;/b&gt;-&lt;b&gt;{0}&lt;/b&gt; von &lt;b&gt;{1}&lt;/b&gt;" />
                                </telerik:RadComboBox>
                            </div>
                            <!--dropdown-->
                            <div class="helpbutton">
                                <div class="helplayer" id="Div1">
                                    <p>
                                        Wählen Sie mit dem Drop-Down-Menü die Kundennummer aus,  an die später die Rechnung gestellt werden soll. 
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile1-->
                            <div class="trenner">
                                &nbsp;</div>
                            <!--formulardaten zeile3-->
                            <div class="formname">
                                Feinstaubplakette</div>
                            <div class="formselects">
                                <asp:RadioButton ID="rbJaFeinstaub" GroupName="Auswahl" runat="server" /><div class="radiolabel">
                                    ja</div>
                                <asp:RadioButton ID="rbNeinFeinstaub" GroupName="Auswahl" Checked="true" runat="server" /><div
                                    class="radiolabel">
                                    nein</div>
                                <asp:CheckBox ID="cbxSave" runat="server" Enabled="False" Text="saved" Visible="False" />
                            </div>
                            <!--formulardaten zeile3-->
                            <div class="trenner">
                                &nbsp;</div>
                            <div class="formname">
                                eVB-Nummer</div>
                            <!--textinput-->
                            <div class="formfeld" id="divEvb" runat="server" style="width: 312px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtEVB" runat="server" Style="width: 282px; text-transform: uppercase;"
                                    MaxLength="7"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <!--textinput-->
                            <!--dropdown-->
                            <div class="formbereich"  id="divFahrzeugart"  runat="server">
                                <asp:DropDownList ID="ddlFahrzeugart" runat="server" Style="width: 290px;">
                                </asp:DropDownList>
                            </div>
                            <!--dropdown-->
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer2">
                                    <p>
                                    Geben Sie hier die elektronische Versicherungsbestätigung ein, die für die Zulassung des Fahrzeugs benötigt wird. 
                                    Achten Sie bitte auf die Fahrzeugart. Diese können Sie über das Dropdown auswählen.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten-->
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
                                Referenzen</h3>
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                Referenzen</div>
                            <!--textinput-->
                            <div class="formfeld" id="divRef1" runat="server" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                 <asp:TextBox CssClass="formtext" ID="txtReferenz1" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20" ></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formfeld" id="divRef2" runat="server" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz2" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer3">
                                    <p>
                                        Hier können Sie individuelle Zuordnungskriterien für die Aufträge eingeben, z.B. Name des Kunden, Fahrgestellnummer, Kennzeichen etc. 
                                        Bitte füllen Sie die Felder entsprechend der Beispieltexte aus.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile1-->
                            <!--formulardaten zeile2-->
                            <div class="formname">
                                Referenzen</div>
                            <!--textinput-->
                            <div class="formfeld" id="divRef3" runat="server" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz3" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formfeld" id="divRef4" runat="server" style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz4" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <!--formulardaten zeile2-->
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
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <h3>
                                Zulassungsdaten</h3>
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                StVA*</div>
                            <!--dropdown-->
                            <div class="formbereich" id="divStVa" runat="server">
                              <telerik:RadComboBox ID="ddlStVa1" Width="604px" runat="server" LoadingMessage="Laden..."
                                    EnableEmbeddedSkins="false" EnableLoadOnDemand="false" AutoPostBack="false" 
                                    OnClientDropDownOpened="onClientFocus"
                                    EmptyMessage="Zulassungskreis suchen...." 
                                    MarkFirstMatch="True" 
                                   
                                    OnClientSelectedIndexChanged="ChangeColor"
                                    ShowMoreResultsBox="True" EnableVirtualScrolling="True" Height="200px" ItemsPerRequest="10" EnableTextSelection="true" >
                                    <Localization NoMatches="Keine Treffer" ShowMoreFormatString="Eintrag &lt;b&gt;1&lt;/b&gt;-&lt;b&gt;{0}&lt;/b&gt; von &lt;b&gt;{1}&lt;/b&gt;" />
                                </telerik:RadComboBox>
                            </div>
                            <!--dropdown-->
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer5">
                                    <p>
                                        Wählen Sie hier den Zulassungskreis aus, in dem das Fahrzeug zugelassen werden soll.

                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile1-->
                            <div class="trenner">
                                &nbsp;</div>
                            <!--formulardaten zeile3-->
                            <div class="formname">
                                Zusatzleistungen</div>
                            <div class="formselects">
                                
                                <asp:CheckBox ID="chkWunschKZ" runat="server" /><div class="checklabel" style="margin-right: 37px;">
                                    Wunsch-Kennzeichen</div>
                                <asp:CheckBox ID="chkReserviert" runat="server" /><div class="checklabel" style="padding-right: 13px;">
                                    Reserviert, Nr</div>
                            </div>
                            <div class="formfeld" id="Div7" style="width: 164px; margin-left: 0px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtNrReserviert" runat="server" Style="width: 134px;"
                                    Text="Reservierungsnummer" MaxLength="10"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <asp:Button ID="Button2" runat="server" Text="Zur Reservierung" 
                                CssClass="dynbutton" onclick="Button2_Click" />
                            <div class="trenner">
                                &nbsp;</div>
                            <div class="formname">
                                &nbsp;</div>
                            <div class="formselects">
                                <asp:CheckBox ID="chkMussRes" runat="server" />
                                <div class="checklabel">
                                    muss noch reserviert werden</div>
                                <asp:CheckBox ID="chkSerie" runat="server" /><div class="checklabel" style="margin-right: 37px;">
                                    Serie
                                </div>
                                <asp:CheckBox ID="chkKennzUebernahme" runat="server" /><div class="checklabel">
                                    Kennzeichen-Übernahme
                                </div>
                            </div>
                            <!--formulardaten zeile3-->
                            <div class="trenner5">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile4-->
                            <div class="formname">
                                Kennzeichen
                            </div>
                            <!--textinput-->
                            <div class="formfeld" id="divKennz1" runat = "server" style="margin-left: 10px; width: 61px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtKennz1" runat="server" Style="width: 31px;
                                    text-transform: uppercase;" MaxLength="3" Text="CK"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formname" style="width: auto;">
                                -
                            </div>
                            <div class="formfeld" id="divKennz2" style="width: 132px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtKennz2" runat="server" Style="width: 70px;
                                    text-transform: uppercase;" Text="XX9999" MaxLength="6"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
							<div class="formname"  style="margin-left: 115px; width: auto;">
								Zulassungsdatum *
							</div>
                            <!--textinput-->
                            <!--datepicker-->
                            <div class="formfeld" id="divZulDate" runat="server" style="width: 200px; margin-left: 0px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox ID="txtZulDate" runat="server" CssClass="formtext jqcalendar jqcalendarWerktage" Width="130px"
                                    MaxLength="10"></asp:TextBox>
                                <div class="formfeld_end_wide">
                                    <img src="../images/icon_datepicker.gif" width="22" height="19" alt="Kalender" class="datepicker" /></div>
                            </div>
                            <!--datepicker-->
                            <!--textinput-->
                            <div class="helpbutton">
                                <div class="helplayer" id="Div11">
                                    <p>
                                        Geben Sie bei einem Wunsch oder einem reservierten Kennzeichen das ent- sprechende Kennzeichen an. 
                                        Das Orts- kennzeichen wird vom Zulassungskreis übernommen. 
                                        Wählen Sie außerdem das gewünschte Zulassungsdatum aus. Dies darf nicht in der Vergangenheit liegen.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile4-->
                            <div class="trenner">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile5-->
                            <div class="formname" style="padding-top: 2px">
                                ggf. weitere Wunschkennzeichen
                            </div>
                            <div class="formname" style="width: auto;">
								2:
							</div>
                            <div class="formfeld" id="divWunschKZ21" runat = "server"  style="width: 61px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtWunschKZ21" runat="server" Style="width: 31px;
                                    text-transform: uppercase;" MaxLength="3"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formname" style="width: auto;">
                                -
                            </div>
                            <div class="formfeld" id="divWunschKZ22" style="width: 132px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtWunschKZ22" runat="server" Style="width: 70px;
                                    text-transform: uppercase;" MaxLength="6"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formname"  style="width: auto;">
								3:
							</div>
                            <div class="formfeld" id="divWunschKZ31" runat = "server"  style="width: 61px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtWunschKZ31" runat="server" Style="width: 31px;
                                    text-transform: uppercase;" MaxLength="3"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formname" style="width: auto;">
                                -
                            </div>
                            <div class="formfeld" id="divWunschKZ32" style="width: 132px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtWunschKZ32" runat="server" Style="width: 70px;
                                    text-transform: uppercase;" MaxLength="6"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="Div3">
                                    <p>
                                        Geben Sie bei einem noch nicht reser- vierten Wunschkennzeichen 2 weitere, alternative Kennzeichenwünsche an, auf die ggf. ausgewichen werden kann. 
                                        Das Ortskennzeichen wird vom Zulassungs- kreis übernommen. 

                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile5-->
                            <div class="trenner">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile6-->
                            <div class="formname">
                                &nbsp;
                            </div>
                            <div class="formselects">
                                <asp:CheckBox ID="chkEinKennz" runat="server" /><div class="checklabel">
                                    Nur ein Kennzeichen</div>
                            </div>
                            <div class="formselects">
                                <asp:CheckBox ID="chkKennzSonder" AutoPostBack="true" runat="server" 
                                    oncheckedchanged="chkKennzSonder_CheckedChanged"  /><div
                                    class="checklabel" style="margin-right: 6px;" >
                                    Kennzeichen-Sondergröße
                                </div>
                            </div>
                            <div class="formbereich" id="divKennzForm" runat="server">
                                <asp:DropDownList ID="ddlKennzForm" runat="server" Style="width: 150px" Enabled="false">
                                </asp:DropDownList>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="Div12">
                                    <p>
                                        Wenn nur ein Kennzeichen benötigt wird, setzen Sie den Haken. 
                                        Wünschen Sie eine Sondergröße, setzen Sie dort ebenfalls den Haken und wählen Sie diese aus dem Drop-Down-Menü aus.

                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <div class="trenner">
                                &nbsp;
                            </div>
                            <!--formulardaten zeile6-->
                            <!--formulardaten zeile7-->
                            <div class="formname">
                            </div>
                            <div class="formselects">
                                <asp:CheckBox ID="chkSaison" runat="server" /><div class="checklabel" style="margin-right: 3px">
                                    Saisonkennzeichen</div>
                            </div>
                            <div class="formbereich">
                                <asp:DropDownList ID="ddlSaisonAnfang" runat="server" style="width: 90px">
                                    <asp:ListItem Value="1" Text="01" />
                                    <asp:ListItem Value="2" Text="02" />
                                    <asp:ListItem Value="3" Text="03" />
                                    <asp:ListItem Value="4" Text="04" />
                                    <asp:ListItem Value="5" Text="05" />
                                    <asp:ListItem Value="6" Text="06" />
                                    <asp:ListItem Value="7" Text="07" />
                                    <asp:ListItem Value="8" Text="08" />
                                    <asp:ListItem Value="9" Text="09" />
                                    <asp:ListItem Value="10" Text="10" />
                                    <asp:ListItem Value="11" Text="11" />
                                    <asp:ListItem Value="12" Text="12" />
                                </asp:DropDownList>
                            </div>
                            <div class="formbereich">
                                <asp:DropDownList ID="ddlSaisonEnde" runat="server" style="width: 90px">
                                    <asp:ListItem Value="1" Text="01" />
                                    <asp:ListItem Value="2" Text="02" />
                                    <asp:ListItem Value="3" Text="03" />
                                    <asp:ListItem Value="4" Text="04" />
                                    <asp:ListItem Value="5" Text="05" />
                                    <asp:ListItem Value="6" Text="06" />
                                    <asp:ListItem Value="7" Text="07" />
                                    <asp:ListItem Value="8" Text="08" />
                                    <asp:ListItem Value="9" Text="09" />
                                    <asp:ListItem Value="10" Text="10" />
                                    <asp:ListItem Value="11" Text="11" />
                                    <asp:ListItem Value="12" Text="12" />
                                </asp:DropDownList>
                            </div>
                            <div class="formselects" style="margin-left: 15px">
                                <asp:CheckBox ID="chkZusatzKZ" runat="server" /><div class="checklabel">
                                    drittes Kennzeichen</div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="Div2">
                                    <p>
                                        Wird ein Saison-Kennzeichen gewünscht, so setzen Sie hier den Haken und wählen den Zeitraum aus.
                                        Benötigen Sie ein zusätzliches Kennzeichen, setzen Sie den Haken bei 'drittes Kennzeichen'.

                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile7-->
                            <div class="trenner">
                                &nbsp;</div>
                        </div>

                            <div class="formlayer_plus_bot">
                                &nbsp;</div>
                            <!--formulardaten-->
                        </div>
                </div>
                        <!--formularbereich3-->
                        <div class="trenner10">
                            &nbsp;</div>
            </div>
        </div>
        <!-- FORMULARLAYER1 -->
        <!-- FORMULARLAYER2 -->
        <div class="linie">
            &nbsp;
        </div>
        <div class="formlayer">
            <div class="formlayer_top">
                <div class="formopener" id="formopener2" onclick="openforms(2);">
                    <img src="../images/transp.gif" width="42" height="42" alt="" />
                </div>
                <h2>
                    Bemerkungen und Notizen</h2>
                <div class="hinweispflicht" id="hinweispflicht2">
                    *Pflichtfelder
                </div>
            </div>
            <div class="formlayer_plus" id="form2" >
                <uc:BemerkungenNotizen runat="server" ID="ucBemerkungenNotizen" />
            </div>
        </div>
        <!-- FORMULARLAYER2 -->
        <!-- FORMULARLAYER3 -->
        <div class="linie">
            &nbsp;
        </div>
        <div class="formlayer">
            <div class="formlayer_top">
                <div class="formopener" id="formopener3" onclick="openforms(3);">
                    <img src="../images/transp.gif" width="42" height="42" alt="" />
                </div>
                <h2>Bankdaten und Adresse für Rechnung an Endkunden</h2>
                <div class="hinweispflicht" id="hinweispflicht3">
                    *Pflichtfelder
                </div>
            </div>
            <div class="formlayer_plus" id="form3" >
                <uc:BankdatenAdresse runat="server" ID="ucBankdatenAdresse" />
            </div>
        </div>
        <!-- FORMULARLAYER3 -->
        <div class="linie">
            &nbsp;
        </div>
        <!--formbuttons-->
        <div class="globalcheckbox" id = "divHoldData" runat="server">
            <asp:CheckBox ID="chkHoldData" runat="server" /><div class="checklabel">
                Daten für nächsten Auftrag übernehmen</div>
        </div>
        <div class="formbuttons">
            <asp:Button ID="cmdSave" runat="server" CssClass="submitbutton" Text="Speichern/Neu"
                OnClick="cmdSave_Click" />
            <asp:Button ID="cmdCancel" runat="server" CssClass="button" Text="Abbrechen" 
                onclick="cmdCancel_Click" />
        </div>
        <!--formbuttons-->
    </div>
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" VisibleOnPageLoad="false" Modal="true" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" Title="Kundenformular herunterladen" runat="server" Behaviors="Resize, Move" 
                NavigateUrl="PrintDialogKundenformular.aspx" Width="275" Height="160" Modal="true" OnClientClose="onClientClose" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>
