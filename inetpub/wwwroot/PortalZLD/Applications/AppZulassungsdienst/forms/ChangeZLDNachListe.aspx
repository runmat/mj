<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeZLDNachListe.aspx.cs" Inherits="AppZulassungsdienst.forms.ChangeZLDNachListe" MasterPageFile="../MasterPage/Big.Master" %>
<%@ Register src="/PortalZLD/PageElements/GridNavigation.ascx" tagname="GridNavigation" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
          });</script>
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
					<asp:UpdatePanel ID="UpdatePanel1"   runat="server" >
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                            <asp:PostBackTrigger ControlID="ibtnNoFilter"  />
                        </Triggers>
						<ContentTemplate>
							
							<div id="TableQuery" style="margin-bottom: 10px">
								<table id="tab1"  runat="server" cellpadding="0" cellspacing="0">
									<tbody>
										<tr class="formquery">
											<td class="firstLeft active"  colspan="6" style="vertical-align:top">
												<asp:Label ID="lblError"  runat="server" CssClass="TextError"></asp:Label>
												<asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
											</td>
										</tr>
										<tr class="formquery" id="trSuche" runat="server">
											<td class="firstLeft active">
												<asp:Label ID="lblSuch" runat="server">Suche:</asp:Label>
											</td>
											<td class="firstLeft active">
												<asp:DropDownList ID="ddlSuche" runat="server" Style="width: auto" >
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
													MaxLength="10"></asp:TextBox>
											</td>
											<td class="firstLeft active" colspan="4"  style="width: 100%;">
												<asp:ImageButton ID="ibtnSearch" style="padding-bottom:2px;padding-right:5px;" ImageUrl="/PortalZLD/Images/Filter.gif" 
													runat="server" onclick="ibtnSearch_Click" />
													<asp:ImageButton ID="ibtnNoFilter" Visible="false"   ImageUrl="/PortalZLD/Images/Unfilter.gif" 
													runat="server" onclick="ibtnNoFilter_Click"  />
											</td>
										</tr>

										<tr class="formquery">
											<td class="firstLeft active"  colspan="4" style="vertical-align:top">
										        &nbsp;</td>
										</tr>                                                                                    
									</tbody>
								</table>
                                <table id="tblGebuehr"  runat="server" cellpadding="0" cellspacing="0">
                                    <tr class="formquery" id="tr1" runat="server">
											<td class="firstLeft active" style="font-size: 12px">
												<asp:Label ID="Label1" runat="server">Gesamtgebühr:</asp:Label>
											</td>
											<td class="firstLeft active" style="font-size: 12px;">
												<asp:Label ID="lblGesamtGeb" runat="server" ></asp:Label>
											</td>
										<td class="firstLeft active" style="font-size: 12px">
												<asp:Label ID="Label2" runat="server">Gesamt Amt:</asp:Label>
											</td>
											<td class="firstLeft active" style="font-size: 12px; width: 100%;" colspan="3" >
												<asp:Label ID="lblGesamtGebAmt" runat="server" ></asp:Label>
											</td>
										</tr>
										<tr class="formquery" id="tr2" runat="server">
											<td class="firstLeft active" style="font-size: 12px; height: 24px;">
												<asp:Label ID="Label3" runat="server">Gesamt EC:</asp:Label>
											</td>
											<td class="firstLeft active" style="font-size: 12px; height: 24px;">
												<asp:Label ID="lblGesamtGebEC" runat="server" ></asp:Label>
											</td>
										    <td class="firstLeft active" style="font-size: 12px; height: 24px;">
												<asp:Label ID="Label5" runat="server">Gesamt Bar:</asp:Label>
											</td>
											<td class="firstLeft active" style="font-size: 12px; height: 24px;">
												<asp:Label ID="lblGesamtGebBar" runat="server" ></asp:Label>
											</td>
										    <td class="firstLeft active" style="font-size: 12px; height: 24px;">
												<asp:Label ID="Label4" runat="server">Gesamt RE:</asp:Label>
											</td>
											<td class="firstLeft active" 
                                                style="font-size: 12px; width: 100%; height: 24px;" >
												<asp:Label ID="lblGesamtGebRE" runat="server" ></asp:Label>
											</td>
										</tr>
										    <tr class="formquery">
											    <td class="firstLeft active"  colspan="6" style="vertical-align:top">&nbsp;
										       </td>
										    </tr> 
                                </table>
							</div>
							<div id="Result" runat="Server">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server" 
                                            onclick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
									<div id="pagination">
										<uc1:GridNavigation ID="GridNavigation1" runat="server" />
									</div>

									<div id="data" style="float: none">
									    <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
													CellPadding="0" CellSpacing="0" GridLines="None"
													AllowSorting="true" AllowPaging="true" CssClass="GridView" PageSize="20" onsorting="GridView1_Sorting" 
													DataKeyNames="ID" onrowcommand="GridView1_RowCommand" onpageindexchanging="GridView1_PageIndexChanging" 
                                                    onrowdatabound="GridView1_RowDataBound" >
                            						<HeaderStyle CssClass="GridTableHead" Width="100%" ForeColor="White" />
													<PagerSettings Visible="false" />
 													<Columns>
														<asp:TemplateField Visible="false">
															 <ItemTemplate>
																<asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField> 
														<asp:TemplateField HeaderText="Status" Visible="false" >
															<ItemTemplate>
																<asp:Label ID="lblStatus" runat="server"  Text='<%# Eval("Status") %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField Visible="false">
															 <ItemTemplate>
																<asp:Label ID="lblGebMatnr" runat="server" Text='<%# Eval("GebMatnr") %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField> 																												   
														<asp:TemplateField SortExpression="id_sap" HeaderText="col_ID">
															<HeaderTemplate>
																<asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="id_sap">col_ID</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label  ID="lblsapID" runat="server"  Font-Bold='<%# Eval("bearbeitet") %>'  Text='<%# Eval("id_sap") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="40px" />
																<ItemStyle CssClass="TablePadding"  Width="40px"  />    
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
																<asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="kundenname">col_Kundenname</asp:LinkButton></HeaderTemplate>
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
														<asp:TemplateField Visible="false">
															<ItemTemplate>
																<asp:Label ID="lblMatnr"  runat="server"  Text='<%# Eval("Matnr") %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField>       
                                                                                                      
														<asp:TemplateField SortExpression="Matbez" HeaderText="col_Matbez">
														<HeaderTemplate>
															<asp:LinkButton ID="col_Matbez" runat="server" CommandName="Sort" CommandArgument="Matbez">col_Matbez</asp:LinkButton></HeaderTemplate>
														<ItemTemplate>
															<asp:Label ID="lblMatbez" runat="server" Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("Matbez") %>'></asp:Label>
														    <asp:HiddenField runat="server" ID="hfMenge" Value='<%# Eval("Menge") %>' />
                                                        </ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="105px"  />
																<ItemStyle CssClass="TablePadding"   Width="105px"  />
													</asp:TemplateField>
													<asp:TemplateField SortExpression="Preis" HeaderText="col_Preis">
														<HeaderTemplate>
															<asp:LinkButton ID="col_Preis" runat="server" CommandName="Sort" CommandArgument="Preis">col_Preis</asp:LinkButton></HeaderTemplate>
														<ItemTemplate>
															<asp:TextBox ID="txtPreis"  CssClass="TextBoxNormal"  onKeyPress="return numbersonly(event, true)"   Width="45" Font-Size="8pt" runat="server" Text='<%# Eval("Preis", "{0:F}") %>'></asp:TextBox>
														</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"   Width="52px" />
																<ItemStyle CssClass="TablePadding"   Width="52px"  />
													</asp:TemplateField>
													<asp:TemplateField SortExpression="GebPreis" HeaderText="col_GebPreis">
														<HeaderTemplate>
															<asp:LinkButton ID="col_GebPreis" runat="server" CommandName="Sort" CommandArgument="GebPreis">col_GebPreis</asp:LinkButton></HeaderTemplate>
														<ItemTemplate>
															<asp:TextBox ID="txtGebPreis" onKeyPress="return numbersonly(event, true)"  CssClass="TextBoxNormal" Width="45" Font-Size="8pt" runat="server"
																Text='<%# Eval("GebPreis", "{0:F}") %>' Visible='<%# Eval("GebMatPflicht").ToString() == "X" %>'
                                                                Enabled='<%# proofGebPak(Eval("ID").ToString(), Eval("id_pos").ToString()) %>' ></asp:TextBox>
                                                            <asp:HiddenField ID="txtGebPreisOld" Value='<%# Eval("GebPreis", "{0:F}") %>' runat="server" />
														</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="52px"  />
																<ItemStyle CssClass="TablePadding" Width="52px"   />
													</asp:TemplateField>
													    <asp:TemplateField SortExpression="Preis_Amt" HeaderText="col_Preis_Amt">
														    <HeaderTemplate>
															    <asp:LinkButton ID="col_Preis_Amt" runat="server" CommandName="Sort" CommandArgument="Preis_Amt">col_Preis_Amt</asp:LinkButton></HeaderTemplate>
														    <ItemTemplate>
															    <asp:TextBox ID="txtPreis_Amt" onKeyPress="return numbersonly(event, true)"  CssClass="TextBoxNormal" Width="45" Font-Size="8pt" runat="server"
																    Text='<%# Eval("Preis_Amt", "{0:F}") %>'  Visible='<%# Eval("GebMatPflicht").ToString() == "X" %>'  ></asp:TextBox>
                                                                    <asp:HiddenField ID="txtPreis_AmtOld" Value='<%# Eval("Preis_Amt", "{0:F}") %>' runat="server" />
														    </ItemTemplate>
																    <HeaderStyle CssClass="TablePadding" Width="52px"/>
																    <ItemStyle CssClass="TablePadding" Width="52px"/>
													    </asp:TemplateField> 
														<asp:TemplateField SortExpression="Steuer" HeaderText="col_Steuer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Steuer" runat="server" CommandName="Sort" CommandArgument="Steuer">col_Steuer</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox ID="txtSteuer" onKeyPress="return numbersonly(event, true)"  CssClass="TextBoxNormal" Width="45" Font-Size="8pt" runat="server" 
																Visible='<%# (Int32)Eval("id_pos")== 10 %>' Text='<%# Eval("Steuer", "{0:F}") %>'></asp:TextBox>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="52px"  />
																<ItemStyle CssClass="TablePadding" Width="52px"   />                                                          
														</asp:TemplateField>    
                                                          
														<asp:TemplateField SortExpression="PreisKZ" HeaderText="col_PreisKZ">
															<HeaderTemplate>
																<asp:LinkButton ID="col_PreisKZ" runat="server" CommandName="Sort" CommandArgument="PreisKZ">col_PreisKZ</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox ID="txtPreisKZ" Enabled='<%# proofPauschMat(Eval("PauschalKunde").ToString(), Eval("Matnr").ToString()) %>' 
																Visible='<%# (Int32)Eval("id_pos")== 10 %>' onKeyPress="return numbersonly(event, true)"  CssClass="TextBoxNormal" Width="45"  Font-Size="8pt" runat="server"  Text='<%# Eval("PreisKZ", "{0:F}") %>'></asp:TextBox>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="52px"  />
																<ItemStyle CssClass="TablePadding"  Width="52px"  />                                                           
														</asp:TemplateField>                                                                                                                                                                               
														<asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="col_Zulassungsdatum">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Zulassungsdatum"  runat="server" CommandName="Sort" CommandArgument="Zulassungsdatum">col_Zulassungsdatum</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblZulassungsdatum" Visible='<%# proofDateVisible((Int32)Eval("id_pos"), Eval("Vorgang").ToString()) %>' runat="server"  Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("Zulassungsdatum", "{0:d}") %>'></asp:Label>
															<asp:TextBox ID="txtZulassungsdatum" onKeyPress="return numbersonly(event, false)"  CssClass="TextBoxNormal" MaxLength="6" Width="45" Font-Size="8pt" runat="server"
																Text='<%# Eval("Zulassungsdatum") %>' Visible='<%# proofDateEditable((Int32)Eval("id_pos"), Eval("Vorgang").ToString()) %>' ></asp:TextBox>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="65px" />
																<ItemStyle CssClass="TablePadding"   Width="65px" />
														</asp:TemplateField>
														<asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">col_Referenz1</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblReferenz1" Visible='<%# (Int32)Eval("id_pos")== 10 %>'  runat="server"  Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("Referenz1") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="65px" />
																<ItemStyle CssClass="TablePadding"   Width="65px" />                                                            
														</asp:TemplateField>  
                                                        <asp:TemplateField SortExpression="Referenz2" HeaderText="col_Referenz2" >
															<HeaderTemplate>
																<asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Referenz2</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblReferenz2" Visible='<%# (Int32)Eval("id_pos")== 10 %>'  runat="server"  Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("Referenz2") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="100px" />
																<ItemStyle CssClass="TablePadding"   Width="100px" />                                                            
														</asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KreisKZ" HeaderText="col_Amt">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Amt"  runat="server" CommandName="Sort" CommandArgument="KreisKZ">col_Amt</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
															    <asp:TextBox ID="txtKreisKZ" CssClass="TextBoxNormal" MaxLength="3" Width="30" Font-Size="8pt" runat="server" Visible='<%# (Int32)Eval("id_pos")== 10 %>' Text='<%# Eval("KreisKZ") %>' style="text-transform:uppercase;" ></asp:TextBox>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="35px" />
																<ItemStyle CssClass="TablePadding"   Width="35px" />
														</asp:TemplateField>
														<asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblKennKZ1"  runat="server"  Visible='<%# (Int32)Eval("id_pos")== 10 %>'  Font-Bold='<%# Eval("bearbeitet") %>' Text='<%# Eval("KennKZ") %>'></asp:Label>
																<asp:TextBox ID="txtKennzAbc"  onkeyup="FilterKennz(this,event)"  CssClass="TextBoxNormal" Width="45"  Visible='<%# (Int32)Eval("id_pos")== 10 %>' Font-Size="8pt" MaxLength="6" runat="server"  Text='<%# Eval("KennABC") %>'></asp:TextBox>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="75px"  />
																<ItemStyle CssClass="TablePadding"  Width="75px"  />                                                              
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
														<asp:TemplateField SortExpression="Feinstaub" HeaderText="col_Reserviert">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Feinstaub" runat="server" CommandName="Sort" CommandArgument="Feinstaub">col_Feinstaub</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblFeinstaub" runat="server" Visible='<%#((Boolean)Eval("Feinstaub")) == true %>'  Font-Bold='<%# Eval("bearbeitet") %>' Text="F"></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="10px" />
																<ItemStyle CssClass="TablePadding" Width="10px" />                                                            
														</asp:TemplateField>  
                                                        <asp:TemplateField SortExpression="Bemerkung" HeaderText="col_Bemerkung">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="Bemerkung">col_Bemerkung</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
															    <asp:Image runat="server" Visible='<%# (String.IsNullOrEmpty(Eval("Bemerkung").ToString()) == false) && ((Int32)Eval("id_pos")== 10) %>' 
                                                                    ImageUrl="/PortalZLD/images/info.gif" ToolTip='<%# Eval("Bemerkung") %>'/>
                                                                <asp:Image runat="server" Visible='<%# ((Boolean)Eval("Nachbearbeiten")) && (String.IsNullOrEmpty(Eval("Infotext").ToString()) == false) && ((Int32)Eval("id_pos")== 10) %>' 
                                                                    ImageUrl="/PortalZLD/images/info.gif" ToolTip='<%# Eval("Infotext") %>'/>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="20px" />
																<ItemStyle CssClass="TablePadding"   Width="20px" />                                                            
														</asp:TemplateField>   
                                                        <asp:TemplateField SortExpression="BemerkungAH" HeaderText="col_BemerkungAH">
															<HeaderTemplate>
																<asp:LinkButton ID="col_BemerkungAH" runat="server" CommandName="Sort" CommandArgument="BemerkungAH">col_BemerkungAH</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
															    <asp:Label runat="server" Visible='<%# (Int32)Eval("id_pos")== 10 %>' Text='<%# Eval("Bemerkung") %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="85px" />
																<ItemStyle CssClass="TablePadding"   Width="85px" />                                                            
														</asp:TemplateField> 
                                                        <asp:TemplateField SortExpression="VorerfDatum" HeaderText="col_Vorerfasst">
                                                            <HeaderTemplate>
																<asp:LinkButton ID="col_Vorerfasst"  runat="server" CommandName="Sort" CommandArgument="VorerfDatum">col_Vorerfasst</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblVorerfasst" runat="server" Font-Bold='<%# Eval("bearbeitet") %>' 
                                                                    Text='<%# Eval("VorerfDatum", "{0:d}") + " " + (Eval("VorerfUhrzeit") == null || Eval("VorerfUhrzeit").ToString().Length < 6 ? "" : Eval("VorerfUhrzeit").ToString().Substring(0, 2) + ":" + Eval("VorerfUhrzeit").ToString().Substring(2, 2) + ":" + Eval("VorerfUhrzeit").ToString().Substring(4, 2)) %>'></asp:Label>
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="110px" />
																<ItemStyle CssClass="TablePadding"   Width="110px" />
                                                        </asp:TemplateField> 
														<asp:TemplateField >
															 <ItemTemplate>
																 <asp:ImageButton ID="ibtnedt" Visible='<%# Eval("toDelete").ToString() != "X" %>' ImageUrl="/PortalZLD/images/Edit.gif" CommandArgument='<%# Eval("ID") %>'  runat="server" CommandName="Edt" ToolTip="Bearbeiten" Width="16px" Height="16px" />
																 <asp:ImageButton ID="ibtnDel" ImageUrl="/PortalZLD/images/del.png" CommandArgument='<%#  ((GridViewRow)Container).RowIndex %>'  runat="server" CommandName="Del" ToolTip="Löschen" />
																 <asp:ImageButton ID="ibtnOK" ImageUrl="/PortalZLD/images/haken_gruen.gif" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'  runat="server" CommandName="OK" ToolTip='<%# Eval("BEB_STATUS").ToString() == "1" ? "Annehmen" : "OK" %>' />
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding" Width="58px" />
																<ItemStyle CssClass="TablePadding"  Width="58px" />                                                            
														</asp:TemplateField>                                                                                                       
														<asp:TemplateField SortExpression="EC" HeaderText="col_EC">
															<HeaderTemplate>
																<asp:LinkButton ID="col_EC" runat="server" CommandName="Sort" CommandArgument="EC">col_EC</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
                                                                <asp:RadioButton ID="rbEC" AutoPostBack="true" GroupName="Bezahlung" 
                                                                    Checked='<%# Eval("EC") %>' runat="server" 
                                                                    oncheckedchanged="rbEC_CheckedChanged" />
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="15px"  />
																<ItemStyle CssClass="TablePadding"   Width="15px" />                                                            
														</asp:TemplateField>      
														<asp:TemplateField SortExpression="Bar" HeaderText="col_Bar">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Bar" runat="server" CommandName="Sort" CommandArgument="Bar">col_Bar</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:RadioButton ID="rbBar" GroupName="Bezahlung" 
                                                                     Checked='<%# Eval("Bar") %>' runat="server" 
                                                                    AutoPostBack="True" oncheckedchanged="rbBar_CheckedChanged" />
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="15px" />
																<ItemStyle CssClass="TablePadding"    Width="15px" />                                                       
														</asp:TemplateField>   
							                            <asp:TemplateField SortExpression="RE" HeaderText="col_RE">
															<HeaderTemplate>
																<asp:LinkButton ID="col_RE" runat="server" CommandName="Sort" CommandArgument="RE">col_RE</asp:LinkButton></HeaderTemplate>
															<ItemTemplate>
																<asp:RadioButton  ID="rbRE" GroupName="Bezahlung" 
                                                                    Checked='<%# Eval("RE") %>' runat="server" 
                                                                    AutoPostBack="True" oncheckedchanged="rbRE_CheckedChanged" />
															</ItemTemplate>
																<HeaderStyle CssClass="TablePadding"  Width="15px" />
																<ItemStyle CssClass="TablePadding"    Width="15px" />                                                       
														</asp:TemplateField>																																																																												   
													</Columns>
												</asp:GridView>
									</div>
								</div>
							<script type="text/javascript">
								var scrollTop;
								//Register Begin Request and End Request 
								Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
								Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
								//Get The Div Scroll Position
								function BeginRequestHandler(sender, args) 
								{
								    var m = document.getElementById('data');
								    if (m != null) { scrollTop = m.scrollTop; }
									
								}
								//Set The Div Scroll Position
								function EndRequestHandler(sender, args) {
								    var m = document.getElementById('data');
								    if (m != null) { scrollTop = m.scrollTop; }
								} 

						</script>
							</ContentTemplate>
					</asp:UpdatePanel>
							

					<div id="dataFooter">
						<asp:LinkButton ID="cmdalleEC" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="cmdalleEC_Click" 
							 >» alle EC</asp:LinkButton>			
						<asp:LinkButton ID="cmdalleBar" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="cmdalleBar_Click" 
							 >» alle Bar</asp:LinkButton>		
						<asp:LinkButton ID="cmdalleRE" runat="server" CssClass="Tablebutton" Width="78px" onclick="RE_Click" 
							 >» alle RE</asp:LinkButton>	 
						<asp:LinkButton ID="cmdOK" runat="server" CssClass="TablebuttonMiddle" Width="100px" onclick="cmdOK_Click" 
							 >» alle OK</asp:LinkButton>
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
                     <asp:Button ID="MPEDummy" Width="16px" Height="0" runat="server" Style="display: none" />
                    <cc1:ModalPopupExtender runat="server" ID="MPEBarquittungen" BackgroundCssClass="ui-widget-overlay"
                        Enabled="true" PopupControlID="pnlPrintBar" TargetControlID="MPEDummy">
                    </cc1:ModalPopupExtender>

                    <asp:Panel ID="pnlPrintBar" runat="server" Style="overflow: auto; height: 300px;
                        width: 400px; display: none;" >
                        <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix" style="width: 95%;" >
                                                  <asp:LinkButton ID="cmdClose" runat="server" 
                            Width="10px" onclick="cmdClose_Click"  style="float:right" 
						   >X</asp:LinkButton>  
                        </div>
                        <asp:GridView ID="GridView2" GridLines="None" Style="border: 1px solid #dfdfdf; width: 96%;
                            font-size: 9px; color: #595959" runat="server" BackColor="White" AutoGenerateColumns="False" OnRowCommand="GridView2_RowCommand"
                            CaptionAlign="Left">
                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                            <RowStyle CssClass="ItemStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Quittung">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("Filename") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aufrufen">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="cmdPrint" CommandName="Print" CommandArgument='<%# Eval("Path") %>'
                                            runat="server" ImageUrl="/PortalZLD/Images/iconPDF.gif" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" />
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
				</div>
			</div>
			</div>
	</div>

</asp:Content>
