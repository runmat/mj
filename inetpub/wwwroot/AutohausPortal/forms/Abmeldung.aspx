<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Abmeldung.aspx.cs" Inherits="AutohausPortal.forms.Abmeldung" MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

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

        }
        function onClientClose(oWnd, args) {
            __doPostBack('RadWindow1', '');
        }
    </script>
    <telerik:RadAjaxManager ID="AjaxManager" runat="server" ClientEvents-OnResponseEnd ="initiate2">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlKunnr1" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz1" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz2" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz3" />
                    <telerik:AjaxUpdatedControl ControlID="txtReferenz4" />
                    <telerik:AjaxUpdatedControl ControlID="Repeater1" />
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

                <div class="formlayer_plus"  id="form1">
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
                            <div class="formbereich" id="divKunde" runat="server"  >
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  ClientEvents-OnResponseEnd ="initiate2">
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
                                    </telerik:RadAjaxPanel>
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
                            <div class="formfeld"  id="divRef1" runat="server"  style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz1" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="formfeld"  id="divRef2" runat="server"  style="width: 307px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtReferenz2" runat="server" Style="width: 277px;
                                    text-transform: uppercase;" MaxLength="20"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="helpbutton">
                                <div class="helplayer" id="helplayer6">
                                    <p>
                                        Hier können Sie individuelle Zuordnungskriterien für die Aufträge eingeben, z.B. Name des Kunden, Fahrgestellnummer, Kennzeichen etc. 
                                        Bitte füllen Sie die Felder entsprechend der Beispieltexte aus.
                                    </p>
                                </div>
                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                            </div>
                            <!--formulardaten zeile1-->
                            <!--formulardaten zeile2-->
                            <div class="formname" id="divLabelReferenzen3und4" runat="server">
                                Referenzen</div>
                            <!--textinput-->
                            <div class="formfeld" id="divRef3" runat="server"  style="width: 307px;">
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
                                                                    <asp:CheckBox ID="cbxSave" runat="server" Enabled="False" Text="saved" Visible="False" />
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
											<h3> Fahrzeugdaten</h3>
											<!--formulardaten zeile1-->
											<div class="formname">
												Fahrzeugart
											</div>
											<div class="formbereich"  id="divFahrzeugart" runat="server" style="margin-right: 50px;">
                                                <asp:DropDownList ID="ddlFahrzeugart" runat="server" Style="width: 150px;">
                                                </asp:DropDownList>
											</div>											
											<!--formulardaten zeile1-->		
                                            <div class="helpbutton">
                                                <div class="helplayer" id="helplayer3">
                                                    <p>
                                                        Wählen Sie die entsprechende Fahrzeugart über das Drop-Down-Menü aus.  
                                                    </p>
                                                </div>
                                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                                            </div>
                                            									
											<div class="trenner5">
												&nbsp;
											</div>											

											<!--formulardaten zeile2-->
											<div class="formname">
												Anzahl der Abmeldungen*
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
											 
											<!--formulardaten zeile2-->											
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
											<div class="trenner">
												&nbsp;
											</div>
											
											<!--formulardaten zeile3-->
											<div class="formname" style="width: auto;">
												Bitte tragen Sie hier die Kennzeichen und die jeweiligen Referenzen ein:
											</div>
											<!--formulardaten zeile3-->
											<div class="trenner">
												&nbsp;
											</div>		
											<!--formulardaten zeile4 dynamische Menge!-->	
                                            <div id="dynFields" runat="server" class="dynFields">
                                                <asp:Repeater ID="Repeater1" runat="server">
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <div class="formfeld" id="divKennz1" runat="server"  style="width: 61px;">
                                                                        <div class="formfeld_start"></div>
                                                                        <asp:Label runat="server" ID="lblID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "ID").ToString() %>'></asp:Label>
                                                                        <asp:TextBox CssClass="formtext" ID="txtKennz1" runat="server" Style="width: 31px;
                                                                            text-transform: uppercase;" MaxLength="3" Text='<%# DataBinder.Eval(Container.DataItem, "Kennz1").ToString() %>'></asp:TextBox>
                                                                        <div class="formfeld_end"></div>
                                                                    </div>
                                                                    <div class="formname" style="width: auto;">
                                                                        -
                                                                    </div>
                                                                    <div class="formfeld" id="divKennz2" runat="server"  style="width: 122px; margin-right: 15px;">
                                                                        <div class="formfeld_start"></div>
                                                                        <asp:TextBox CssClass="formtext" ID="txtKennz2" runat="server" Style="width: 70px;
                                                                            text-transform: uppercase;" Text='<%# DataBinder.Eval(Container.DataItem, "Kennz2").ToString() %>' MaxLength="6"></asp:TextBox>
                                                                        <div class="formfeld_end"></div>
                                                                    </div>
                                                                    <div class="formselects">
                                                                        <asp:CheckBox ID="chkKennzVorhanden" Checked='<%# (Boolean)DataBinder.Eval(Container.DataItem, "KennzVorhanden") %>' runat="server" />
                                                                        <div class="checklabel">
                                                                            vorhandenes Kennzeichen reservieren
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div class="formfeld" id="divRepReferenz2" runat="server"  style="width: 200px; margin-right: 20px;display: none;" >
                                                                        <div class="formfeld_start"></div>
                                                                        <asp:TextBox CssClass="formtext" ID="txtRepReferenz2" runat="server" Style="width: 150px;
                                                                            text-transform: uppercase;" Text='<%# DataBinder.Eval(Container.DataItem, "REF2").ToString() %>' MaxLength="20"></asp:TextBox>
                                                                        <div class="formfeld_end"></div>
                                                                    </div>
                                                                    <div class="formfeld" id="divRepReferenz3" runat="server"  style="width: 200px; margin-right: 20px;display: none;" >
                                                                        <div class="formfeld_start"></div>
                                                                        <asp:TextBox CssClass="formtext" ID="txtRepReferenz3" runat="server" Style="width: 150px;
                                                                            text-transform: uppercase;" Text='<%# DataBinder.Eval(Container.DataItem, "REF3").ToString() %>' MaxLength="20"></asp:TextBox>
                                                                        <div class="formfeld_end"></div>
                                                                    </div>
                                                                    <div class="formfeld" id="divRepReferenz4" runat="server"  style="width: 200px; margin-right: 20px;display: none;" >
                                                                        <div class="formfeld_start"></div>
                                                                        <asp:TextBox CssClass="formtext" ID="txtRepReferenz4" runat="server" Style="width: 150px;
                                                                            text-transform: uppercase;" Text='<%# DataBinder.Eval(Container.DataItem, "REF4").ToString() %>' MaxLength="20"></asp:TextBox>
                                                                        <div class="formfeld_end"></div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                    <div class="formlayer_plus" id="form2" >
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
            <telerik:RadWindow ID="RadWindow1" Title="Kundenformular herunterladen" runat="server" Behaviors="Resize, Move" 
                NavigateUrl="PrintDialogKundenformular.aspx" Width="275" Height="180" Modal="true" OnClientClose="onClientClose" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>