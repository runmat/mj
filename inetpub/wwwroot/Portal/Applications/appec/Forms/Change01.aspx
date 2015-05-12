<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01.aspx.vb" Inherits="AppEC.Change01" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
	    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
	    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
	    <meta content="JavaScript" name="vs_defaultClientScript"/>
	    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</head>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colspan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;&nbsp;&nbsp;
								</td>
							</tr>
							<tr>
								<td class="TaskTitle" colspan="2">&nbsp;<asp:label id="lblTask" runat="server"></asp:label></td>
							</tr>
                            <tr>
								<td colspan="2" vAlign="middle" noWrap >
									<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>
                                    &nbsp;
                                    <asp:label id="lblSuccess" runat="server" EnableViewState="False" ForeColor="Blue"></asp:label>
                                </td>
							</tr>
                            <tr id="trBack" runat="server" Visible="False">
                                <td vAlign="top" width="120">
								    <asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" > &#149;&nbsp;Zurück</asp:linkbutton>
								</td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trActionSelection" runat="server">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="cmdNeuanlage"  Height="16px" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neuanlage</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="cmdDatenpflege"  Height="16px" runat="server" CssClass="StandardButton"> &#149;&nbsp;Datenpflege</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trSearchFilter" runat="server" Visible="False">
                                <td vAlign="top" width="120">
                                    <asp:Calendar ID="CalAnlDatVon" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid"
                                        Width="120px" Visible="False">
                                        <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                        <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                        <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                        <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                        <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                        <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                    </asp:Calendar>
                                    <asp:Calendar ID="CalAnlDatBis" runat="server" Visible="False" Width="120px" BorderStyle="Solid"
                                        BorderColor="Black" CellPadding="0">
                                        <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                        <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                        <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                        <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                        <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                        <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                    </asp:Calendar>
                                </td>
                                <td vAlign="top">
                                    <table class="BorderLeftBottom" cellSpacing="1" cellPadding="1" border="0">
										<tr>
											<td></td>
											<td>Batch-ID von - bis:</td>
											<td>
												<asp:TextBox id="txtFilterBatchIdVon" runat="server" MaxLength="8"/>
                                            </td>
                                            <td>
												<asp:TextBox id="txtFilterBatchIdBis" runat="server" MaxLength="8"/>
                                            </td>
											<td>
                                                <asp:image runat="server" ToolTip="Numerisch, max. 8 Stellen" 
                                                    ImageUrl="/Portal/Images/info.gif"></asp:image>
											</td>
										</tr>
                                        <tr>
											<td></td>
											<td>Unit-Nr. von - bis:</td>
											<td>
												<asp:TextBox id="txtFilterUnitVon" runat="server" MaxLength="8"/>
                                            </td>
                                            <td>
												<asp:TextBox id="txtFilterUnitBis" runat="server" MaxLength="8"/>
                                            </td>
											<td>
                                                <asp:image runat="server" ToolTip="Numerisch, 8 Stellen" 
                                                    ImageUrl="/Portal/Images/info.gif"></asp:image>
											</td>
										</tr>
                                        <tr>
											<td></td>
											<td>Model-ID von - bis:</td>
											<td>
												<asp:TextBox id="txtFilterModelIdVon" runat="server" MaxLength="7"/>
                                            </td>
                                            <td>
												<asp:TextBox id="txtFilterModelIdBis" runat="server" MaxLength="7"/>
                                            </td>
											<td>
                                                <asp:image runat="server" ToolTip="Alphanumerisch, max. 7 Stellen" 
                                                    ImageUrl="/Portal/Images/info.gif"></asp:image>
											</td>
										</tr>
                                        <tr>
											<td></td>
											<td>Einsteuerungsmonat von - bis:</td>
											<td>
												<asp:TextBox id="txtFilterEinstMonatVon" runat="server" MaxLength="7"/>
                                            </td>
                                            <td>
												<asp:TextBox id="txtFilterEinstMonatBis" runat="server" MaxLength="7"/>
                                            </td>
											<td>
                                                <asp:image runat="server" ToolTip="Datum, Format: MM.JJJJ" 
                                                    ImageUrl="/Portal/Images/info.gif"></asp:image>
											</td>
										</tr>
                                        <tr>
											<td></td>
											<td>Erfasser:</td>
											<td>
												<asp:TextBox id="txtFilterErfasser" runat="server" MaxLength="12"/>
                                            </td>
                                            <td>
                                            </td>
											<td>
                                                <asp:image runat="server" ToolTip="Max. 12 Stellen" 
                                                    ImageUrl="/Portal/Images/info.gif"></asp:image>
											</td>
										</tr>
                                        <tr>
											<td></td>
											<td>Anlagedatum von:</td>
											<td>
												<asp:TextBox id="txtFilterAnlagedatumVon" runat="server"/>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnKalenderAnlagedatumVon" runat="server" CssClass="StandardButtonTable" 
                                                        Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                            </td>
											<td>
											</td>
										</tr>
                                        <tr>
											<td></td>
											<td>Anlagedatum bis:</td>
											<td>
												<asp:TextBox id="txtFilterAnlagedatumBis" runat="server"/>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnKalenderAnlagedatumBis" runat="server" CssClass="StandardButtonTable" 
                                                        Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
                                            </td>
											<td>
											</td>
										</tr>
										<tr>
											<td colspan="5">&nbsp;</td>
										</tr>
										<tr>
											<td colspan="4"></td>
											<td>
												<asp:LinkButton id="cmdSearch" runat="server" CssClass="StandardButton"> •&nbsp;Suchen</asp:LinkButton>
                                            </td>
										</tr>
									</table>
                                </td>
                            </tr>
                            <tr id="trSearchResult" runat="server" Visible="False">
                                <td vAlign="top" width="120">&nbsp;</td>
                                <td vAlign="top">
                                    <asp:DataGrid id="dgBatches" runat="server" CssClass="tableMain" BackColor="White" Width="100%" AutoGenerateColumns="False" 
                                        AllowSorting="True" AllowPaging="True" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" bodyHeight="300">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
										<Columns>
										    <asp:TemplateColumn HeaderText="Bearbeiten">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:ImageButton runat="server" ImageUrl="/Portal/Images/EditTableHS.png" CommandName="Bearbeiten" 
                                                    CommandArgument='<%# Eval("ZBATCH_ID") %>' Visible='<%# Eval("ZLOEVM_BATCH_ID") <> "X" AndAlso Eval("ZUNITVERGEBEN") <> "X" %>'/>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="ZBATCH_ID" SortExpression="ZBATCH_ID" HeaderText="Batch-ID"/>
											<asp:BoundColumn DataField="ZMODEL_ID" SortExpression="ZMODEL_ID" HeaderText="Model-ID"/>
											<asp:BoundColumn DataField="ZSIPP_CODE" SortExpression="ZSIPP_CODE" HeaderText="SIPP"/>
											<asp:BoundColumn DataField="ZMOD_DESCR" SortExpression="ZMOD_DESCR" HeaderText="Modell"/>
											<asp:BoundColumn DataField="ZMAKE" SortExpression="ZMAKE" HeaderText="Marke"/>
											<asp:BoundColumn DataField="ZANZAHL" SortExpression="ZANZAHL" HeaderText="Anzahl Fzg"/>
                                            <asp:BoundColumn DataField="ZUNIT_NR_VON" SortExpression="ZUNIT_NR_VON" HeaderText="Unit von"/>
											<asp:BoundColumn DataField="ZUNIT_NR_BIS" SortExpression="ZUNIT_NR_BIS" HeaderText="Unit bis"/>
											<asp:BoundColumn DataField="ZFZG_GROUP" SortExpression="ZFZG_GROUP" HeaderText="Gruppe"/>
											<asp:BoundColumn DataField="ZNAVI" SortExpression="ZNAVI" HeaderText="Navi"/>
											<asp:BoundColumn DataField="ZAHK" SortExpression="ZAHK" HeaderText="Ahk"/>
											<asp:BoundColumn DataField="ZLAUFZEIT" SortExpression="ZLAUFZEIT" HeaderText="Laufzeit"/>
											<asp:BoundColumn DataField="ZLZBINDUNG" SortExpression="ZLZBINDUNG" HeaderText="Bindung"/>
                                            <asp:BoundColumn DataField="ZAUFNR_VON" SortExpression="ZAUFNR_VON" HeaderText="Auftrag von"/>
                                            <asp:BoundColumn DataField="ZAUFNR_BIS" SortExpression="ZAUFNR_BIS" HeaderText="Auftrag bis"/>
                                            <asp:BoundColumn DataField="ZMS_REIFEN" SortExpression="ZMS_REIFEN" HeaderText="M+S-Reifen"/>
                                            <asp:BoundColumn DataField="ZSECU_FLEET" SortExpression="ZSECU_FLEET" HeaderText="SF"/>
                                            <asp:BoundColumn DataField="ZLEASING" SortExpression="ZLEASING" HeaderText="Leasing"/>
                                            <asp:BoundColumn DataField="ZERNAM" SortExpression="ZERNAM" HeaderText="Erfasser"/>
                                            <asp:BoundColumn DataField="ERDAT" SortExpression="ERDAT" HeaderText="Anlage" DataFormatString="{0:dd.MM.yyyy}"/>
                                            <asp:BoundColumn DataField="ZLOEVM_BATCH_ID" SortExpression="ZLOEVM_BATCH_ID" HeaderText="Löschung"/>
                                            <asp:BoundColumn DataField="ZUNITVERGEBEN" SortExpression="ZUNITVERGEBEN" HeaderText="Vergabe"/>
										</Columns>
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
									</asp:DataGrid>
                                </td>
                            </tr>
							<tr id="trEditBatch" runat="server" Visible="False">
								<td vAlign="top" width="120">&nbsp;</td>
								<td vAlign="top">
									<table class="BorderLeftBottom" cellSpacing="0" cellPadding="0" border="0">
										<tr>
											<td vAlign="top" align="left">
												<table cellSpacing="0" cellPadding="2" bgColor="white" border="0">
													<tr>
														<td vAlign="middle" noWrap></td>
														<td vAlign="middle" noWrap>Model-Id:</td>
														<td vAlign="middle" noWrap><asp:textbox id="txtModelId" runat="server" CssClass="ButtonRowBackground" MaxLength="7"></asp:textbox></td>
														<td vAlign="top" noWrap colspan="4">
															<P><FONT color="red"><asp:image runat="server" ToolTip="Alphanumerisch, max. 7 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></P>
														</td>
													</tr>
													<tr>
														<td vAlign="middle" noWrap></td>
														<td vAlign="middle" noWrap>Modellbezeichnung:</td>

														<td vAlign="middle" noWrap><asp:textbox id="txtModell" runat="server" CssClass="InputDisableStyle" Width="100%" ReadOnly="False"></asp:textbox></td><td vAlign="middle" noWrap>
														<td vAlign="top" noWrap colspan="3"></td>
													</tr>
													<tr>
														<td vAlign="middle" noWrap></td>
														<td vAlign="middle" noWrap>SIPP-Code:</td>
														<td vAlign="middle"><asp:textbox id="txtSippcode" runat="server" CssClass="ButtonRowBackground" MaxLength="4"></asp:textbox></td>
														<td vAlign="top" colspan="4"><FONT color="red"><asp:image runat="server" ToolTip="Alphanumerisch, max. 4 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Hersteller:</td>
														<td vAlign="middle" colspan="4"><asp:Dropdownlist id="ddlHersteller" runat="server" CssClass="DropDownStyle" Enabled="False" Width="100%"></asp:Dropdownlist></td>
														<td></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Batch-Id:</td>
														<td vAlign="middle"><asp:textbox id="txtBatchId" runat="server" CssClass="ButtonRowBackground" MaxLength="8"></asp:textbox></td>
														<td vAlign="top" colspan="4"><FONT color="red"><asp:image id="Image4" runat="server" ToolTip="Numerisch, max. 8 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Geplanter Liefermonat:</td>
														<td vAlign="middle"><asp:textbox id="txtDatEinsteuerung" runat="server" CssClass="ButtonRowBackground" MaxLength="7" ToolTip="Datum, Format: MM.JJJJ"></asp:textbox></td>
														<td vAlign="top" colspan="4"><FONT color="red"><asp:image id="Image6" runat="server" ToolTip="Datum, Format: MM.JJJJ" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Anzahl Fahrzeuge:</td>
														<td vAlign="middle"><asp:textbox id="txtAnzahlFahrzeuge" runat="server" CssClass="ButtonRowBackground" MaxLength="5"></asp:textbox></td>
														<td vAlign="top" colspan="4"><FONT color="red"><asp:image id="Image7" runat="server" ToolTip="Numerisch, max. 5 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Unit-Nr. von -&nbsp;bis:</td>
														<td vAlign="middle"><asp:textbox id="txtUnitNrVon" runat="server" CssClass="ButtonRowBackground" MaxLength="8"></asp:textbox></td>
														<td vAlign="middle" noWrap align="left" colspan="2"><asp:textbox id="txtUnitNrBis" runat="server" CssClass="ButtonRowBackground" MaxLength="8"></asp:textbox></td>
														<td vAlign="top" colspan="2"><FONT color="red"><asp:image id="Image8" runat="server" ToolTip="Numerisch, 8 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*</FONT></td>
													</tr>
                                                    <tr id="trUnitNrUpload" runat="server">
                                                        <td vAlign="middle"></td>
														<td vAlign="middle">Unit-Nr. Upload:</td>
														<td vAlign="middle" noWrap colspan="3"><input class="InfoBoxFlat" id="upFileUnitNr" type="file" size="40" name="File1" runat="server" /></td>
														<td vAlign="top" colspan="2"><asp:image id="Image10" runat="server" ToolTip="alternativ zu (von..bis): Datei im Excel-Format, Unit-Nummern in Spalte A (ohne Überschrift)" ImageUrl="/Portal/Images/info.gif"></asp:image></td>
                                                    </tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Laufzeit in Tagen:</td>
														<td vAlign="middle"><asp:textbox id="txtLaufzeit" runat="server" CssClass="ButtonRowBackground" MaxLength="4"></asp:textbox></td>
														<td vAlign="top" noWrap colspan="4">
															<P align="left"><FONT color="red"><asp:image id="Image9" runat="server" ToolTip="Numerisch, max. 4 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image>&nbsp;*&nbsp;</FONT>
																<asp:checkbox id="cbxLaufz" runat="server" TextAlign="Left" Text="Laufzeitbindung"></asp:checkbox></P>
														</td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Bemerkungen:</td>
														<td vAlign="middle" noWrap colspan="3">
															<P><asp:textbox id="txtBemerkung" runat="server" CssClass="ButtonRowBackground" MaxLength="60" Width="100%"></asp:textbox></P>
														</td>
														<td class="" vAlign="top" colspan="2"><asp:image id="Image12" runat="server" ToolTip="Alphanumerisch, max. 60 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Auftragsnummer von - bis:</td>
														<td vAlign="middle" noWrap><asp:textbox id="txtAuftragsnummerVon" runat="server" CssClass="ButtonRowBackground" MaxLength="20"></asp:textbox></td>
														<td vAlign="top" align="left" colspan="2"><asp:textbox id="txtAuftragsnummerBis" runat="server" CssClass="ButtonRowBackground" MaxLength="20"></asp:textbox></td>
														<td vAlign="top" colspan="2"><asp:image id="Image3" runat="server" ToolTip="Alphanumerisch, max. 20 Stellen" ImageUrl="/Portal/Images/info.gif"></asp:image></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Verwendungszweck:</td>
														<td vAlign="middle" noWrap colspan="3"><asp:dropdownlist id="ddlVerwendung" runat="server" CssClass="DropDownStyle" Width="100%"></asp:dropdownlist></td>
														<td vAlign="top" colspan="2"></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Kennzeichenserie:</td>
														<td vAlign="middle" noWrap colspan="3"><asp:dropdownlist id="ddlKennzeichenserie1" runat="server" CssClass="DropDownStyle"></asp:dropdownlist></td>
														<td vAlign="top" colspan="2"></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Fahrzeuggruppe:</td>
														<td vAlign="top">
															<table cellSpacing="1" cellPadding="1" border="0">
																<tr>
																	<td noWrap><asp:radiobutton id="rbLKW" runat="server" Width="60px" Text="LKW" GroupName="rbFahrzeuggruppe"></asp:radiobutton></td>
																	<td noWrap><asp:radiobutton id="rbPKW" runat="server" Width="60px" Text="PKW" GroupName="rbFahrzeuggruppe" Checked="True"></asp:radiobutton></td>
																</tr>
															</table>
														</td>
														<td vAlign="middle" colspan="4">&nbsp;</td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle" noWrap>Wintertaugliche Bereifung:</td>
														<td vAlign="middle">
															<table cellSpacing="1" cellPadding="1" border="0">
																<tr>
																	<td noWrap><asp:radiobutton id="rbJ1" runat="server" Width="60px" Text="Ja" GroupName="rbWinterreifen"></asp:radiobutton></td>
																	<td noWrap><asp:radiobutton id="rbN1" runat="server" Width="60px" Text="Prüfen" GroupName="rbWinterreifen" Checked="True"></asp:radiobutton></td>
																</tr>
															</table>
														</td>
														<td vAlign="middle" colspan="4">
														    <asp:dropdownlist id="ddlModellZuHersteller" runat="server" Enabled="False" style="display: none"></asp:dropdownlist>
                                                            <asp:dropdownlist id="ddlModellZuSipp" runat="server" Enabled="False" style="display: none"></asp:dropdownlist>
														    <asp:dropdownlist id="ddlModellZuLaufzeit" runat="server" Enabled="False" style="display: none"></asp:dropdownlist>
                                                            <asp:dropdownlist id="ddlModellZuLaufzeitbindung" runat="server" Enabled="False" style="display: none"></asp:dropdownlist>
														</td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle" noWrap>Anhängerkupplung:</td>
														<td vAlign="middle">
															
															<table cellSpacing="1" cellPadding="1" border="0">
																<tr>
																	<td noWrap><asp:radiobutton id="rbJAnhaenger" runat="server" Width="60px" Text="Ja" 
                                                                GroupName="rbAnhaenger"></asp:radiobutton></td>
																	<td noWrap><asp:radiobutton id="rbNAnhaenger" runat="server" Width="60px" Text="Prüfen" 
                                                                GroupName="rbAnhaenger" Checked="True"></asp:radiobutton></td>
																</tr>
															</table>
															
                                                            
														</td>
														<td vAlign="middle" colspan="4">&nbsp;</td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle" noWrap>Navigationssystem:</td>
														<td vAlign="middle">
															<table cellSpacing="1" cellPadding="1" border="0">
																<tr>
																	<td noWrap><asp:radiobutton id="rb_NaviJa" runat="server" Width="60px" Text="Ja" GroupName="rbNavi"></asp:radiobutton></td>
																	<td noWrap><asp:radiobutton id="rb_NaviNein" runat="server" Width="60px" Text="Nein" GroupName="rbNavi" Checked="True"></asp:radiobutton></td>
																</tr>
															</table>
														</td>
														<td vAlign="middle" colspan="4"></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Securiti Fleet:</td>
														<td vAlign="middle">
															<table cellSpacing="1" cellPadding="1" border="0">
																<tr>
																	<td noWrap><asp:radiobutton id="rbJ2" runat="server" Width="60px" Text="Ja" GroupName="rbSecurFleet"></asp:radiobutton></td>
																	<td noWrap><asp:radiobutton id="rbN2" runat="server" Width="60px" Text="Nein" GroupName="rbSecurFleet" Checked="True"></asp:radiobutton></td>
																</tr>
															</table>
														</td>
														<td vAlign="middle" colspan="4"></td>
													</tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle">Leasing</td>
														<td vAlign="middle">
															<table cellSpacing="1" cellPadding="1" border="0">
																<tr>
																	<td noWrap>
																		<asp:radiobutton id="rbLeasingJa" runat="server" Width="60px" Text="Ja" GroupName="rbLeasing"></asp:radiobutton></td>
																	<td noWrap>
																		<asp:radiobutton id="rbLeasingNein" runat="server" Width="60px" Text="Nein" GroupName="rbLeasing" Checked="True"></asp:radiobutton></td>
																</tr>
															</table>
														</td>
														<td vAlign="middle" colspan="4"></td>
													</tr>
													<tr>
														<td class="ButtonRowBackground" vAlign="middle"></td>
														<td class="ButtonRowBackground" vAlign="middle">&nbsp;<FONT color="#ff0000"><FONT face="Arial">*<FONT size="1">Eingabe 
																		erforderlich</FONT></FONT></FONT></td>
														<td class="ButtonRowBackground" vAlign="middle"></td>
														<td class="ButtonRowBackground" vAlign="middle" colspan="4"></td>
													</tr>
                                                    <tr id="trKeepData" runat="server">
                                                        <td vAlign="middle"></td>
                                                        <td vAlign="middle" colspan="6">
                                                            <asp:CheckBox runat="server" ID="cbxKeepData" Text="Eingaben für nächsten Vorgang merken" Checked="True"/>
                                                        </td>
                                                    </tr>
													<tr>
														<td vAlign="middle"></td>
														<td vAlign="middle"><asp:dropdownlist id="ddlModellHidden" runat="server" EnableViewState="True" Enabled="False" style="display: none"></asp:dropdownlist></td>
														<td vAlign="middle" colspan="1">
															<P align="right"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Width="150px"> &#149;&nbsp;Speichern</asp:linkbutton></P>
														</td>
                                                        <td vAlign="middle" colspan="1">
															<P align="right"><asp:linkbutton id="cmdReset" runat="server" 
                                                                    CssClass="StandardButton" Width="150px"> &#149;&nbsp;Zurücksetzen</asp:linkbutton></P>
														</td>
														<td vAlign="middle" colspan="3">
														</td>
													</tr>
												</table>
											    <input id="txtHerstellerHidden" type="hidden" size="1" runat="server"/> <input id="txtHerstellerBezeichnungHidden" type="hidden" size="1" runat="server"/></td>
										</tr>
									</table>
									&nbsp;
								</td>
							</tr>
						</table> 
                        <!--#include File="../../../PageElements/Footer.html" -->
                    </td>
				</tr>
				<script type="text/javascript">										
							<!--													
								function SetHersteller()
								{
									document.Form1.txtHerstellerHidden.value = document.Form1.ddlHersteller.value;																	
									for (var i = 0; i < document.Form1.ddlHersteller.length; i++)
									{
										if (document.Form1.ddlHersteller.options[i].value == document.Form1.txtHerstellerHidden.value)
										{												
											document.Form1.txtHerstellerBezeichnungHidden.value = document.Form1.ddlHersteller.options[i].text;
											break;
										}
									}																				
								}
								
								function setFocus()
								{
							        document.Form1.txtModelId.focus();	
								}
								
								function SetModell()
								{
									var ok;
									document.Form1.txtModell.value = "";
									ok=0;
									for (var i = 0; i < document.Form1.ddlModellHidden.length; i++)
									{
										if (document.Form1.ddlModellHidden.options[i].value == document.Form1.txtModelId.value)
										{
											document.Form1.txtModell.value = document.Form1.ddlModellHidden.options[i].text;
											ok=1;
											break;
										}
									}
									
									if ((document.Form1.txtModelId.value != "") && (ok==0)){	
										alert("Model-Id unbekannt!");
										document.Form1.txtModelId.value = "";
										document.Form1.txtModelId.focus();												
									}										
									else{								
									document.Form1.ddlModellZuHersteller.selectedIndex = 0;
									var id;
									for (var i = 0; i < document.Form1.ddlModellZuHersteller.length-1; i++)
									{
										
										if (document.Form1.ddlModellZuHersteller.options[i].value == document.Form1.txtModelId.value)
										{
											id = document.Form1.ddlModellZuHersteller.options[i].text;
											
											for (var j = 0; j < document.Form1.ddlHersteller.length-1; j++)
											{
												if (document.Form1.ddlHersteller.options[j].value == id)
												{
													document.Form1.ddlHersteller.selectedIndex = j;
													document.Form1.txtHerstellerHidden.value=id;
													document.Form1.txtHerstellerBezeichnungHidden.value=document.Form1.ddlHersteller.options[j].text;
												}
											}											
											break;
										}										
									}
									
									document.Form1.ddlModellZuSipp.selectedIndex = 0;
									var sipp;
									for (var i = 0; i < document.Form1.ddlModellZuSipp.length-1; i++)
									{
										if (document.Form1.ddlModellZuSipp.options[i].value == document.Form1.txtModelId.value)
										{											
											document.Form1.txtSippcode.value = document.Form1.ddlModellZuSipp.options[i].text;
											break;
										}										
									}

									document.Form1.ddlModellZuLaufzeit.selectedIndex = 0;
									//var Laufzeit;
									for (var i = 0; i < document.Form1.ddlModellZuLaufzeit.length - 1; i++) {
									    if (document.Form1.ddlModellZuLaufzeit.options[i].value == document.Form1.txtModelId.value) {
									        document.Form1.txtLaufzeit.value = document.Form1.ddlModellZuLaufzeit.options[i].text;
									        break;
									    }
									}

									document.Form1.ddlModellZuLaufzeitbindung.selectedIndex = 0;
									//var Laufzeitbindung;
									for (var i = 0; i < document.Form1.ddlModellZuLaufzeitbindung.length - 1; i++) {
									    if (document.Form1.ddlModellZuLaufzeitbindung.options[i].value == document.Form1.txtModelId.value) {

									        if (document.Form1.ddlModellZuLaufzeitbindung.options[i].text == 'X') {
									            document.Form1.cbxLaufz.checked = true;
									        }
									        
									        break;
									    }
									}
									
									
									document.Form1.txtBatchId.focus();											
								}
								}
							-->
				</script>
			</table>
		</form>
	</body>
</html>
