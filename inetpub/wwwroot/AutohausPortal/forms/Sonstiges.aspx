<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sonstiges.aspx.cs" Inherits="AutohausPortal.forms.Sonstiges" 
 MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

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
                    <telerik:AjaxUpdatedControl ControlID="txtService" />
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
                                Fahrzeugart</div>
                            <!--dropdown-->
                            <div class="formbereich" id="divFahrzeugart" runat="server" >
                                <asp:DropDownList ID="ddlFahrzeugart" runat="server" Style="width: 290px;">
                                </asp:DropDownList>
                            </div>
                            <!--dropdown-->
                            <div class="helpbutton">
                                <div class="helplayer" id="Div1">
                                    <p>
                                       Wählen Sie die entsprechende Fahrzeugart über das Drop-Down-Menü aus
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
												&nbsp;
											</div>		
											<!--formulardaten zeile2-->
											<div class="formname" style="width: auto;">
												Bitte tragen Sie hier die gewünschte Dienstleistung ein(max. 240 Zeichen):
											</div>
											<!--formulardaten zeile2-->
											<div class="trenner">
												&nbsp;
											</div>		
											<!--formulardaten zeile3 -->
                                            <telerik:RadTextBox ID="txtService2" MaxLength="240" Width="395px"
                                                Rows="3" Columns="80" runat="server" TextMode="MultiLine">
                                            </telerik:RadTextBox>	

											<div class="trenner">
												&nbsp;
											</div>												
											<!--formulardaten zeile3 -->	
                                            <!--formulardaten zeile4 -->
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
													<img src="/AutohausPortal/images/icon_datepicker.gif" width="22" height="19" alt="Kalender" class="datepicker" />
												</div>
											</div>
											<!--datepicker-->			
											<!--formulardaten zeile4 -->								
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
			<div class="formlayer_plus" id="form3" >
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
