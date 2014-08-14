<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dokumentenanforderung.aspx.cs" Inherits="AutohausPortal.forms.Dokumentenanforderung" MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
								<h2>Suchanfrage</h2>
								<div class="hinweispflicht" id="hinweispflicht1">
									*Pflichtfelder
								</div>
							</div>
                            <div style="margin-left: 65px">
                                <h3>
                                    <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""></asp:Label>
                                    <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text=""></asp:Label>      
                                </h3>
                            </div>
							<div class="formlayer_plus" id="form1">
								<!--formularbereich1-->
								<div class="formularbereich">
									<div class="formlayer_plus_top">
										&nbsp;
									</div>
									<div class="formlayer_plus_content">
										<!--formulardaten-->
										<div class="formulardaten">										
											<!--formulardaten zeile1-->
											<div class="formname">
												Ortskennzeichen*
											</div>
											<!--textinput-->
											<div class="formfeld" id="divKennz" runat="server" style="width: 200px;">
											    <div class="formfeld_start"></div>
												<asp:TextBox CssClass="formtext" ID="txtKennz" runat="server" Style="width: 170px;
                                                    text-transform: uppercase;" MaxLength="6"></asp:TextBox>
												<div class="formfeld_end"></div>
											</div>
											<!--textinput-->
                                            <div class="helpbutton">
                                                <div class="helplayer" id="Div2">
                                                    <p>
                                                        Erfassen Sie hier das Kürzel des Zulassungsbezirkes, zu dem Sie sich die benötigten Unterlagen anzeigen lassen möchten.
                                                    </p>
                                                </div>
                                                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
                                            </div>
											<!--formulardaten zeile1-->
											<div class="trenner">
												&nbsp;
											</div>
										</div>
										<!--formulardaten-->
										<div class="formlayer_plus_bot">
											&nbsp;
										</div>
									</div>
								</div>
								<!--formularbereich1-->
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