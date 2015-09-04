<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kennzeichenbestellung.aspx.cs" Inherits="AutohausPortal.forms.Kennzeichenbestellung" MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="BemerkungenNotizen" Src="..\Controls\BemerkungenNotizen.ascx" %>
<%@ Register TagPrefix="uc" TagName="BankdatenAdresse" Src="..\Controls\BankdatenAdresse.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/AutohausPortal/Scripts/Helper.js?22052012"></script>
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

                var item = eventArgs.get_item();
                var txtAnzahl = $("#ctl00_ContentPlaceHolder1_txtAnzahl");
                var kennzSonder = "";

                if (ArraySonderStva != null) {
                    var sonder = false;
                    for (var i = 0; i < ArraySonderStva.length; i++) {
                        var value = ArraySonderStva[i];
                        if (value[0] == item.get_value()) {
                            kennzSonder = value[1];
                        }
                    }
                }

                for (var i2 = 0; i2 < parseInt(txtAnzahl.val()); i2++) {
                    var ctrl = String(i2);
                    if (ctrl.length == 1) { ctrl = "0" + ctrl; }
                    var txtKennz1 = $("#ctl00_ContentPlaceHolder1_Repeater1_ctl" + ctrl + "_txtKennz1");
                    if (kennzSonder != "") {
                        txtKennz1.val(kennzSonder);
                    }
                    else {
                        txtKennz1.val(item.get_value());
                    }
                    enableDefaultValue("ctl00_ContentPlaceHolder1_Repeater1_ctl" + ctrl + "_txtKennz1");
                    txtKennz1.attr("onblur", "");
                    txtKennz1.attr("onfocus", "");
                    txtKennz1.css("color", "#000");
                }

            }
        }
        function onClientClose(oWnd, args) {
            __doPostBack('RadWindow1', '');
        }
    </script>
    <telerik:RadAjaxManager  ID="AjaxManager" runat="server" ClientEvents-OnResponseEnd ="initiate2"  >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlKunnr1" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz1"   />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz2" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz3" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz4" />
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
                        <img src="../images/transp.gif" width="42" height="42" alt="" /></div>
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
                        *Pflichtfelder</div>
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
                                         OnItemsRequested="ddlKunnr1_ItemsRequested" 
                                        MarkFirstMatch="true"     OnClientSelectedIndexChanged="ChangeColor" 
                                         OnSelectedIndexChanged="ddlKunnr1_SelectedIndexChanged" EnableTextSelection="true"
                                        ShowMoreResultsBox="True" EnableVirtualScrolling="True" Height="200px" 
                                         ItemsPerRequest="10">
                                        <Localization NoMatches="Keine Treffer" ShowMoreFormatString="Eintrag &lt;b&gt;1&lt;/b&gt;-&lt;b&gt;{0}&lt;/b&gt; von &lt;b&gt;{1}&lt;/b&gt;" />
                                    </telerik:RadComboBox>
                            </div>
                            <!--dropdown-->
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer2">
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
                                Kennzeichenart</div>
                            <div class="formselects">
                                <asp:RadioButton ID="rbKennzPrae" GroupName="Auswahl"  Checked="true" 
                                    runat="server" AutoPostBack="True" 
                                    oncheckedchanged="rbKennzPrae_CheckedChanged" /><div class="radiolabel" id= "lblKennzprea" runat="server">
                                    Euro-Kennzeichen geprägt</div>
                                <asp:RadioButton ID="rbKennzFun" GroupName="Auswahl" runat="server" 
                                    AutoPostBack="True" oncheckedchanged="rbKennzFun_CheckedChanged" /><div
                                    class="radiolabel" id= "lblKennzfun" runat="server">
                                    Fun-/Parkschilder</div>
                            </div>
                            <!--formulardaten zeile3-->
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
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
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
                        </div>                        <!--formulardaten-->
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
							<h3> Kennzeichendaten</h3>
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
                                    ShowMoreResultsBox="True" EnableVirtualScrolling="True" Height="200px" 
                                    ItemsPerRequest="10" EnableTextSelection="true" EnableViewState="False" >
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
							<div class="trenner5">
								&nbsp;
							</div>											

							<!--formulardaten zeile2-->
							<div class="formname">
								Anzahl der Kennzeichen*
							</div>
							<!--textinput-->
							<div class="formfeld" id="formdiv9" style="width: 81px;">
							    <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtAnzahl" runat="server" Style="width: 51px;"
                                    MaxLength="3" onKeyPress="return numbersonly(event, false)" Text = "1" ></asp:TextBox>
								<div class="formfeld_end"></div>
							</div>
                            <asp:Button ID="cmdRefresh" runat="server" Text="Aktualisieren" 
                                CssClass="dynbutton" onclick="cmdRefresh_Click" />
                                <div class="helpbutton">
                                    <div class="helplayer" id="Div1">
                                        <p>
                                            Bei Anzahl der Abmeldungen erfassen Sie die Stückzahl. 
                                            Durch Klicken auf &#8222;Aktualisieren&#8220; geht die entsprechende Anzahl der Felder zum Erfassen der Kennzeichen auf. 
                                            Wählen Sie außerdem das gewünschte Ausführungsdatum aus.
                                        </p>
                                    </div>
                                    <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                                </div>											 
							<!--formulardaten zeile2-->											

							<div class="trenner">
								&nbsp;
							</div>
											
							<!--formulardaten zeile3-->
							<div class="formname" style="width: auto;">
								Bitte tragen Sie hier die Kennzeichen ein:
							</div>
							<!--formulardaten zeile3-->
							<div class="trenner">
								&nbsp;
							</div>		
							<!--formulardaten zeile4 dynamische Menge!-->	
                            <div id="dynFields" runat="server" class="dynFields">
                                <asp:Repeater ID="Repeater1" runat="server" 
                                    onitemdatabound="Repeater1_ItemDataBound">
                                    <ItemTemplate>
                                    <div class="formfeld" id="divKennz1" runat="server"  style="width: 81px;">
                                        <div class="formfeld_start"></div>
                                        <asp:Label runat="server" ID="lblID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "ID").ToString() %>'></asp:Label>
                                        <asp:TextBox CssClass="formtext" ID="txtKennz1" runat="server" Style="width: 51px;
                                            text-transform: uppercase;" MaxLength="3" Text='<%# DataBinder.Eval(Container.DataItem, "Kennz1").ToString() %>'></asp:TextBox>
                                        <div class="formfeld_end"></div>
                                    </div>
                                <div class="formname" id="divTrennKennz" runat="server" style="width: auto;">
                                    -
                                </div>
                                <div class="formfeld" id="divKennz2" runat="server"  style="width: 162px; margin-right: 20px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtKennz2" runat="server" Style="width: 132px;
                                        text-transform: uppercase;" Text='<%# DataBinder.Eval(Container.DataItem, "Kennz2").ToString() %>' MaxLength="6"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <div class="formfeld" id="divKennzFun" runat="server" visible="false" style="width: 247px; margin-right: 20px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtKennzFun" runat="server" Style="width: 217px;
                                        text-transform: uppercase;" Text='<%# DataBinder.Eval(Container.DataItem, "KennzFun").ToString() %>' MaxLength="10"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <div class="formselects">
                                    <asp:CheckBox ID="chkEinKennz"  Checked='<%# (Boolean)DataBinder.Eval(Container.DataItem, "EinKennz") %>' runat="server" /><div class="checklabel">
                                        <asp:Label ID="lblEinkz" runat="server" Text="Nur ein Kennzeichen"></asp:Label> </div>
                                </div>
                                <div class="formselects">
                                    <asp:CheckBox ID="chkKennzSonder" OnCheckedChanged="Check_Changed" AutoPostBack="true" Checked='<%# (Boolean)DataBinder.Eval(Container.DataItem, "KennzSonder") %>' runat="server" />
                                    <div class="checklabel">
                                        <asp:Label ID="lblKennzSonder" runat="server" Text="Kennzeichen-Sondergröße"></asp:Label>
                                    </div>
                                </div>
								<div class="formbereich">
                                    <asp:DropDownList ID="ddlKennzForm" Enabled= "false" runat="server" Style="width: 175px" >
                                    </asp:DropDownList>
								</div>
                                <div class="trenner">
                                    &nbsp;
                                </div>                                                       
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
							<!--formulardaten zeile5 -->
							<div class="formname">
								Ausführungstermin
							</div>
							<!--textinput-->
							<!--datepicker-->
							<div class="formfeld" id="divDatum" runat="server" style="width: 200px; margin-left: 0px;">
							    <div class="formfeld_start"></div>
                                <asp:TextBox ID="txtDatum" runat="server" CssClass="formtext jqcalendar jqcalendarWerktage" Width="130px"
                                    MaxLength="10"></asp:TextBox>
								<div class="formfeld_end_wide">
									<img src="../images/icon_datepicker.gif" width="22" height="19" alt="Kalender" class="datepicker" />
								</div>
							</div>
							<!--datepicker-->			
							<!--formulardaten zeile5 -->								
							<div class="trenner">
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
                    &nbsp;</div>
            </div>
        </div>
                <div class="formlayer">
                    <div class="formlayer_top">
                        <div class="formopener" id="formopener2" onclick="openforms(2);">
                            <img src="../images/transp.gif" width="42" height="42" alt="" /></div>
                        <h2>
                            Bemerkungen und Notizen</h2>
                        <div class="hinweispflicht" id="hinweispflicht2">
                            *Pflichtfelder</div>
                    </div>
                     <div class="formlayer_plus" id="form2">
                        <uc:BemerkungenNotizen runat="server" ID="ucBemerkungenNotizen" />
                     </div> 
                   </div> 
                <!-- FORMULARLAYER2 -->
                <!-- FORMULARLAYER3 -->
            <div class="linie">
                &nbsp;</div>
                <div class="formlayer">
                    <div class="formlayer_top">
                        <div class="formopener" id="formopener3" onclick="openforms(3);">
                            <img src="../images/transp.gif" width="42" height="42" alt="" /></div>
                        <h2>Bankdaten und Adresse für Rechnung an Endkunden</h2>
                        <div class="hinweispflicht" id="hinweispflicht3">
                            *Pflichtfelder</div>
                    </div>
            <div class="formlayer_plus" id="form3">
                <uc:BankdatenAdresse runat="server" ID="ucBankdatenAdresse" />
            </div>
                <!-- FORMULARLAYER3 -->
                <div class="linie">
                    &nbsp;</div>
                <!--formbuttons-->
                <div class="globalcheckbox" id ="divHoldData" runat="server">
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
    </div>
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" VisibleOnPageLoad="false" Modal="true" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" Title="Kundenformulare herunterladen" runat="server" Behaviors="Resize, Move" 
                NavigateUrl="PrintDialogKundenformulare.aspx" Width="550" Height="300" Modal="true" OnClientClose="onClientClose" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>