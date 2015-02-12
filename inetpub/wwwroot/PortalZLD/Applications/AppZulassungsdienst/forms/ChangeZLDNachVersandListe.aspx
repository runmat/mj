<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeZLDNachVersandListe.aspx.cs" Inherits="AppZulassungsdienst.forms.ChangeZLDNachVersandListe"  MasterPageFile="../MasterPage/Big.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="DialogErfassungDLBezeichnung" Src="../Controls/DialogErfassungDLBezeichnung.ascx" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?26082013"></script>
	<script src="/PortalZLD/Applications/AppZulassungsdienst/Javascript/GridViewHelper.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="JavaScript" type="text/javascript">
        $(document).ready(function () {
            $("#dialog").dialog({
                bgiframe: true,
                autoOpen: false,
                height: 150,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: {
                    Abbrechen: function () {
                        $(this).dialog('close');
                    },
                    Absenden: function () {
                        $(this).dialog('close');
                        __doPostBack('ctl00$ContentPlaceHolder1$cmdSend', '');

                    }
                }
            });
            $('#ctl00_ContentPlaceHolder1_cmdSend').click(function () {
                $('#dialog').dialog('open');
            });
        });

        function pageLoad() {
            var mpe = $find('bDLBezeichnung');
            if (mpe != null) {
                mpe.add_shown(function () {
                    SetDLBezeichnung("");
                });
            }
        }
    </script>
	<div id="site">
		<div id="content">
			<div id="navigationSubmenu">
				<asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
					Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
			</div>
			<div id="innerContent">
				<div id="innerContentRight" style="width: 100%">
					<div id="innerContentRightHeading">
						<h1>
							<asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
						</h1>
					</div>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
						<ContentTemplate>
							<div id="TableQuery" style="margin-bottom: 10px">
								<table id="tab1"  runat="server" cellpadding="0" cellspacing="0">
									<tbody>
										<tr class="formquery">
											<td class="firstLeft active"  colspan="4" style="vertical-align:top">
												<asp:Label ID="lblError"  runat="server" CssClass="TextError"></asp:Label>
												<asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
											</td>
										</tr>
											<tr class="formquery" id="trSuche" runat="server">
												<td class="firstLeft active">
													<asp:Label ID="lblSuch" runat="server">Suche:</asp:Label>
												</td>
												<td class="firstLeft active">
													<asp:DropDownList ID="ddlSuche" runat="server" Style="width: auto" AutoPostBack="True">
													<asp:ListItem Text="Kennzeichen" Value="Kennzeichen"></asp:ListItem>
														<asp:ListItem Text="Kundennummer" Value="kundennr"></asp:ListItem>
														<asp:ListItem Text="Dienstleistung" Value="Matbez"></asp:ListItem>
														<asp:ListItem Text="Gebühr" Value="GebPreis"></asp:ListItem>
														<asp:ListItem Text="Referenz1" Value="Referenz1"></asp:ListItem>
                                                        <asp:ListItem Text="Amt" Value="KreisKZ"></asp:ListItem>
                                                        <asp:ListItem Text="Zulassungsdatum(ttmmjj)" Value="Zulassungsdatum"></asp:ListItem>
                                                        <asp:ListItem Text="ID" Value="id_sap"></asp:ListItem>
													</asp:DropDownList>  
				
												</td>
												<td class="firstLeft active">
													<asp:TextBox ID="txtSuche" runat="server" CssClass="TextBoxNormal" 
														MaxLength="8"></asp:TextBox>
												</td>
												<td class="firstLeft active"  style="width: 100%;">
												<asp:ImageButton ID="ibtnSearch" style="padding-bottom:2px;padding-right:5px;" ImageUrl="/PortalZLD/Images/Filter.gif" 
													runat="server" onclick="ibtnSearch_Click" />
													<asp:ImageButton ID="ibtnNoFilter" Visible="false"   ImageUrl="/PortalZLD/Images/Unfilter.gif" 
													runat="server" onclick="ibtnNoFilter_Click"  />
												</td>
											</tr>
										<tr class="formquery">
											<td class="firstLeft active"  colspan="4" style="vertical-align:top">
										   </td>
										</tr>                                                                                    
									</tbody>
								</table>
							</div>
							<div id="Result" runat="Server">
								<div id="data">
									<table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
										<tr>
											<td>
												<asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
													CellPadding="0" CellSpacing="0" GridLines="None"
													AllowSorting="true" AllowPaging="False" CssClass="GridView" PageSize="1000" 
													onsorting="GridView1_Sorting" DataKeyNames="ID" onselectedindexchanged="GridView1_SelectedIndexChanged" 
													onrowcommand="GridView1_RowCommand" onrowdatabound="GridView1_RowDataBound" >
													<HeaderStyle CssClass="GridTableHead" Width="100%" ForeColor="White" />
													<PagerSettings Visible="False" />
													<Columns>
													   
														<asp:TemplateField Visible="false">
															 <ItemTemplate>
																<asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
															</ItemTemplate>
															
														</asp:TemplateField> 
														<asp:TemplateField Visible="false">
															 <ItemTemplate>
																<asp:Label ID="lblMatnr" runat="server" Text='<%# Eval("Matnr") %>'></asp:Label>
															</ItemTemplate>
															
														</asp:TemplateField>                                                        
														<asp:TemplateField HeaderText="Status"  Visible="false">
															<ItemTemplate>
																<asp:Label ID="lblStatus" runat="server"  Text='<%# Eval("Status") %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField>                                                           
														<asp:TemplateField SortExpression="id_sap" HeaderText="col_ID">
															<HeaderTemplate>
																<asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="id_sap">col_ID</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblsapID" runat="server" Font-Bold='<%# Eval("bearbeitet") %>'  Text='<%# Eval("id_sap") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="30px" />
																<ItemStyle CssClass="TablePadding"  Width="30px" />    
														</asp:TemplateField>     

														<asp:TemplateField SortExpression="PosLoesch"  HeaderText="col_LoeschKZ">
															<HeaderTemplate>
																<asp:LinkButton ID="col_LoeschKZ" runat="server" CommandName="Sort" CommandArgument="PosLoesch">col_LoeschKZ</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
															<asp:Label ID="lblPosLoesch" runat="server" Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("PosLoesch").ToString() %>'></asp:Label>
														</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="30px" />
																<ItemStyle CssClass="TablePadding"  Width="30px" />    
														</asp:TemplateField>                                                      
														<asp:TemplateField SortExpression="kundennr" HeaderText="col_Kundennr">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kundennr" runat="server" CommandName="Sort" CommandArgument="kundennr">col_Kundennr</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblKundennr" runat="server" Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("kundennr") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="57px" />
																<ItemStyle CssClass="TablePadding"  Width="57px" /> 
														</asp:TemplateField>
														<asp:TemplateField SortExpression="kundenname" HeaderText="col_Kundenname">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="kundennr">col_Kundenname</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblKundenname" runat="server" Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("kundenname") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="125px"/>
																<ItemStyle CssClass="TablePadding"  Width="125px"  />
														</asp:TemplateField> 
														<asp:TemplateField Visible="false" HeaderText="col_id_pos">
															<ItemTemplate>
																<asp:Label ID="lblid_pos"  runat="server"  Text='<%# Eval("id_pos") %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField>      
														<asp:TemplateField SortExpression="Matbez" HeaderText="col_Matbez">
														<HeaderTemplate>
															<asp:LinkButton ID="col_Matbez" runat="server" CommandName="Sort" CommandArgument="Matbez">col_Matbez</asp:LinkButton></HeaderTemplate>
														<ItemTemplate>
															<asp:Label ID="lblMatbez" runat="server" Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("Matbez") %>'></asp:Label>
														</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="125px"  />
																<ItemStyle CssClass="TablePadding"   Width="125px"  />
													</asp:TemplateField>
                                                    <asp:TemplateField>
														<ItemTemplate>
															<asp:ImageButton ID="ibtnDLBez" ImageUrl="/PortalZLD/images/Blatt_08.jpg" 
                                                                CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Visible='<%# (Eval("Matnr").ToString() == "570") && (Eval("DLBezeichnung").ToString() != "") %>' 
                                                                runat="server" CommandName="SetDLBez" ToolTip="Dienstleistungsbeschreibung erfassen" />
														</ItemTemplate>
														<HeaderStyle CssClass="TablePadding" Width="40px"/>
													</asp:TemplateField>
													<asp:TemplateField SortExpression="GebPreis" HeaderText="col_GebPreis">
														<HeaderTemplate>
															<asp:LinkButton ID="col_GebPreis" runat="server" CommandName="Sort" CommandArgument="GebPreis">col_GebPreis</asp:LinkButton></HeaderTemplate>
														<ItemTemplate>
															<asp:TextBox ID="txtGebPreis" onKeyPress="return numbersonly(event, true)"  CssClass="TextBoxNormal" Width="45" Font-Size="8pt" runat="server"
																Text='<%# Eval("Preis_Amt", "{0:F}") %>'  Visible='<%# Eval("GebMatPflicht").ToString() == "X" %>'  ></asp:TextBox>
														</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="55px"/>
																<ItemStyle CssClass="TablePadding" Width="55px"/>
													</asp:TemplateField>

														<asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="col_Zulassungsdatum">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Zulassungsdatum"   runat="server" CommandName="Sort" CommandArgument="Zulassungsdatum">col_Zulassungsdatum</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
															<asp:TextBox ID="txtZulassungsdatum" onKeyPress="return numbersonly(event, false)"  CssClass="TextBoxNormal" MaxLength="6" Width="45" Font-Size="8pt" runat="server"
																Text='<%# Eval("Zulassungsdatum") %>' Visible='<%# (Int32)Eval("id_pos")== 10 %>' ></asp:TextBox>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="65px" />
																<ItemStyle CssClass="TablePadding"   Width="65px" />
														</asp:TemplateField>
														<asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">col_Referenz1</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblReferenz1"  runat="server" Visible='<%# (Int32)Eval("id_pos")== 10 %>' Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("Referenz1") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="60px" />
																<ItemStyle CssClass="TablePadding"   Width="60px" />                                                            
														</asp:TemplateField>  
														<asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblKennKZ1"  runat="server" Visible='<%# (Int32)Eval("id_pos")== 10 %>' Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("KennKZ") %>'></asp:Label>
																<asp:TextBox ID="txtKennzAbc" onkeyup="FilterKennz(this,event)" Visible='<%# (Int32)Eval("id_pos")== 10 %>' CssClass="TextBoxNormal" Width="45"  Font-Size="8pt" MaxLength="6" runat="server"  Text='<%# Eval("KennABC") %>'></asp:TextBox>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="80px"  />
																<ItemStyle CssClass="TablePadding"  Width="80px"  />                                                              
														</asp:TemplateField> 
														<asp:TemplateField SortExpression="Reserviert" HeaderText="col_Reserviert">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Reserviert" runat="server" CommandName="Sort" CommandArgument="Reserviert">col_Reserviert</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblReserviert" runat="server" Visible='<%# ((Boolean)Eval("Reserviert")) == true  %>' Font-Bold='<%# Eval("bearbeitet") %>' Text="R"></asp:Label>
																<asp:Label ID="lblWunschKennz" runat="server" Visible='<%# ((Boolean)Eval("WunschKenn")) == true  && ((Boolean)Eval("Reserviert")) == false %>' Font-Bold='<%# Eval("bearbeitet") %>' Text="W"></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="14px"  />
																<ItemStyle CssClass="TablePadding"  Width="14px"  />                                                            
														</asp:TemplateField>
														<asp:TemplateField SortExpression="Feinstaub" HeaderText="col_Feinstaub">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Feinstaub" runat="server" CommandName="Sort" CommandArgument="Feinstaub">col_Feinstaub</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblFeinstaub" runat="server" Visible='<%#((Boolean)Eval("Feinstaub")) == true %>'  Font-Bold='<%# Eval("bearbeitet") %>' Text="F"></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="10px" />
																<ItemStyle CssClass="TablePadding" Width="10px" />                                                            
														</asp:TemplateField>
														<asp:TemplateField SortExpression="VersandVKbur" HeaderText="col_VersandVKbur">
															<HeaderTemplate>
																<asp:LinkButton ID="col_VersandVKbur" runat="server" CommandName="Sort" CommandArgument="VersandVKbur">col_VersandVKbur</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblVersandVKbur" runat="server"  Text='<%# Eval("Filiale").ToString().Replace("1010","") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="20px" />
																<ItemStyle CssClass="TablePadding" Width="20px" />                                                            
														</asp:TemplateField>                                                              
														<asp:TemplateField >
															 <ItemTemplate>
                                                                 <asp:ImageButton ID="ibtnedt" Visible='<%# Eval("toDelete").ToString() != "X" %>' ImageUrl="/PortalZLD/images/Edit.gif" CommandArgument='<%#Eval("ID") %>'  runat="server" CommandName="Edt" ToolTip="Bearbeiten" Width="16px" Height="16px" />
																 <asp:ImageButton ID="ibtnDel" ImageUrl="/PortalZLD/images/del.png" CommandArgument='<%#  ((GridViewRow)Container).RowIndex %>'  runat="server" CommandName="Del" ToolTip="Löschen" />
																 <asp:ImageButton ID="ibtnOK" ImageUrl="/PortalZLD/images/haken_gruen.gif" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'  Enabled='<%# Eval("toDelete").ToString() != "X" %>' runat="server" CommandName="OK" ToolTip="OK" />
																
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="45px" />
																<ItemStyle CssClass="TablePadding"  Width="45px" />                                                            
														</asp:TemplateField>                                                                                                                                                                       
														<asp:TemplateField SortExpression="EC" HeaderText="col_EC">
															<HeaderTemplate>
																<asp:LinkButton ID="col_EC" runat="server" CommandName="Sort" CommandArgument="EC">col_EC</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:RadioButton ID="rbEC" GroupName="Bezahlung" Visible='<%# (Int32)Eval("id_pos")== 10 %>' Checked='<%# Eval("EC") %>' runat="server" />
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="18px"  />
																<ItemStyle CssClass="TablePadding"   Width="18px" />                                                            
														</asp:TemplateField>      
														<asp:TemplateField SortExpression="Bar" HeaderText="col_Bar">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Bar" runat="server" CommandName="Sort" CommandArgument="Bar">col_Bar</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:RadioButton  ID="rbBar" GroupName="Bezahlung" Visible='<%# (Int32)Eval("id_pos")== 10 %>' Checked='<%# Eval("Bar") %>' runat="server" />
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="22px" />
																<ItemStyle CssClass="TablePadding"    Width="22px" />                                                       
														</asp:TemplateField>   
														<asp:TemplateField SortExpression="RE" HeaderText="col_RE">
															<HeaderTemplate>
																<asp:LinkButton ID="col_RE" runat="server" CommandName="Sort" CommandArgument="Bar">col_RE</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:RadioButton  ID="rbRE" GroupName="Bezahlung" Visible='<%# (Int32)Eval("id_pos")== 10 %>' Checked='<%# Eval("RE") %>' runat="server" />
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="18px" />
																<ItemStyle CssClass="TablePadding"    Width="18px" />                                                       
														</asp:TemplateField>	
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDLBezeichnung" runat="server" Text='<%# Eval("DLBezeichnung") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>																																																																													  
													</Columns>
												</asp:GridView>
											</td>
										</tr>
									</table>
								</div>
							</div>
                            <input type="hidden" id="ihAktuellerDatensatz" runat="server" value="" />
						</ContentTemplate>
					</asp:UpdatePanel>
					<div id="dataFooter">
						<asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="cmdSave_Click"  
						   >» Speichern</asp:LinkButton>   
						<asp:LinkButton ID="cmdSend" runat="server" OnClientClick="javascript: return false;" CssClass="Tablebutton" Width="78px" onclick="cmdSend_Click"
						   >» Absenden</asp:LinkButton>
                        <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="cmdContinue_Click" Visible="false"
						   >» Weiter</asp:LinkButton>
				</div>
                <div id="dialog" title="Absenden">
                    Sollen die Aufträge jetzt gesendet werden?
                </div>
                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="mpeDLBezeichnung" runat="server" TargetControlID="btnFake"
                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" X="600" Y="400" BehaviorID="bDLBezeichnung" >
                </ajaxToolkit:ModalPopupExtender>           
                <asp:Panel ID="mb" runat="server" Width="385px" Height="140px" 
                    BackColor="White" style="display:none">
                    <uc2:DialogErfassungDLBezeichnung ID="dlgErfassungDLBez" runat="server" 
                    OnTexteingabeBestaetigt="dlgErfassungDLBez_TexteingabeBestaetigt" />                               
                </asp:Panel>
			</div>
		</div>
	</div>
	</div>
</asp:Content>
