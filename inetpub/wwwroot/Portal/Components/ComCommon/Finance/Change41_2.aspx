<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change41_2.aspx.vb" Inherits="CKG.Components.ComCommon.Change41_2" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<input type="hidden" name="txtVertragsnummer"> <input type="hidden" name="txtFaelligkeit">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td colSpan="2">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</TR>
							<TR>
								<TD colSpan="2">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;
												<asp:linkbutton id="lb_HaendlerSuche" runat="server" CssClass="TaskTitel"></asp:linkbutton>&nbsp;&nbsp;<asp:linkbutton id="lb_HaendlerAuswahl" runat="server" CssClass="TaskTitel"></asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<tr>
											<td>
												<uc1:Kopfdaten id="Kopfdaten1" runat="server"></uc1:Kopfdaten></td>
										</tr>
										<tr>
											<td></td>
										</tr>
										<tr>
											<td></td>
										</tr>
										<TR>
											<TD align="right">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
														<TD align="right"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</TR>
													<tr>
														<td></td>
														<td></td>
													</tr>
													<TR>
														<TD colSpan="2">
															<TABLE id="Table4" cellSpacing="0" cellPadding="5" width="100%" bgColor="#ffffff" border="0">
																<TR id="trMemo" runat="server">
																	<TD noWrap rowSpan="2"><STRONG>Memo für Vertrag:</STRONG> &nbsp; <STRONG>
																			<asp:label id="lblVertragsNummer" runat="server" Font-Bold="True"></asp:label></STRONG>&nbsp;
																		<asp:textbox id="Memo" runat="server" Width="247px" MaxLength="200"></asp:textbox></TD>
																	<TD noWrap width="100%"><asp:linkbutton id="SaveMemo" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Speichern</asp:linkbutton>&nbsp;
																		<asp:linkbutton id="CancelMemo" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Abbrechen</asp:linkbutton></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</table>
												<asp:linkbutton id="lbExcel" Visible="False" Runat="server">
													<strong>Excelformat</strong></asp:linkbutton>&nbsp;&nbsp;&nbsp;
												<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge"></TD>
										</TR>
										<tr>
											<td></td>
										</tr>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer"></asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="lblVertragsNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer">
															<ItemTemplate>
																<asp:Label id=Label3 runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
																</asp:Label>
																<asp:Literal id=Literal5 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.Vertragsnummer") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") &amp; "</a>" %>'>
																</asp:Literal>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Versand" SortExpression="Versand" HeaderText="Versand" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="MemoString" HeaderText="Memo">
															<ItemTemplate>
																<asp:LinkButton id=WriteMemo runat="server" CssClass="StandardButtonSmall" Text='<%# DataBinder.Eval(Container, "DataItem.MemoString") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Memo") %>' CommandName="WriteMemo" CausesValidation="False">
																</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="faellig am" HeaderText="col_faelligAm">
															<HeaderTemplate>
																<asp:LinkButton ID="col_faelligAm" Runat="server" CommandName="Sort" CommandArgument="faellig am">faellig am</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox id=txtFaelligkeit runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.DelayedPayment") AND DataBinder.Eval(Container, "DataItem.DatumAendern") %>' Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.faellig am", "{0:dd.MM.yyyy}") %>'>
																</asp:TextBox>
																<asp:Label id=Label1 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.DelayedPayment") AND (NOT DataBinder.Eval(Container, "DataItem.DatumAendern")) %>' Text='<%# DataBinder.Eval(Container, "DataItem.faellig am", "{0:dd.MM.yyyy}") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Versandadresse">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Versandadresse" Runat="server">col_Versandadresse</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label ID="lblAdresse" Runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Name1") &amp; "<br>" &amp; DataBinder.Eval(Container,"DataItem.Street") &amp; "<br>" &amp; DataBinder.Eval(Container,"DataItem.Post_Code1") &amp; "&nbsp;" &amp; DataBinder.Eval(Container,"DataItem.City1")%>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="FaelligkeitString" HeaderText="col_Faelligkeit">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Faelligkeit" Runat="server" CommandName="Sort" CommandArgument="FaelligkeitString">Faelligkeit</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:LinkButton id=Linkbutton2 runat="server" CssClass="StandardButtonSmall" Visible='<%# DataBinder.Eval(Container, "DataItem.DelayedPayment") AND (NOT DataBinder.Eval(Container, "DataItem.DatumAendern")) %>' Text='<%# DataBinder.Eval(Container, "DataItem.FaelligkeitString") %>' CausesValidation="False" CommandName="Aendern">
																</asp:LinkButton>
																<asp:LinkButton id=LinkButton1 runat="server" CssClass="StandardButtonSmall" Visible='<%# DataBinder.Eval(Container, "DataItem.DelayedPayment") AND DataBinder.Eval(Container, "DataItem.DatumAendern") %>' CausesValidation="False" CommandName="Speichern">Speichern</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Mahndatum" HeaderText="Mahndatum" DataFormatString="{0:dd.MM.yyyy}" SortExpression="Mahndatum" ReadOnly="True"></asp:BoundColumn>
														<asp:BoundColumn DataField="Mahnstufe" HeaderText="Mahnstufe" SortExpression="Mahnstufe" ReadOnly="True"></asp:BoundColumn>
														<asp:BoundColumn DataField="Abrufgrund" HeaderText="Abrufgrund" SortExpression="Abrufgrund" ReadOnly="True"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_ZZREFERENZ1">
															<HeaderTemplate>
																<asp:LinkButton id="col_ZZREFERENZ1" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_ZZREFERENZ1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Statusänderung">
															<ItemTemplate>
																<asp:LinkButton id="lbStatusaenderung" runat="server" Visible="True" CssClass="StandardButtonSmall" Text='Statusänderung' CommandName="Statusaenderung" CausesValidation="True"></asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD height="19">&nbsp;</TD>
								<TD vAlign="top" align="left" height="19"></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD colSpan="2">
						<SCRIPT language="Javascript">
						<!-- //
						function FaelligkeitConfirm(Vertrag)
						  {
						    var Faelligkeit;
							for(var i=0;i<window.document.Form1.length;++i)
							{
								if(window.document.Form1.elements[i].type == "text")
								{
								  var varteile = window.document.Form1.elements[i].name.split(":");
								  for(var j=0;j<varteile.length;++j)
								  {
									if(varteile[j] == "txt"+Vertrag)
									{
									  Faelligkeit = window.document.Form1.elements[i].value;
									}
								  }
								}
							}
							
						    var Teile = Faelligkeit.split(".");
						    var FaelligkeitTag = Teile[0];
						    if (isNaN(FaelligkeitTag) == true)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
								return false;
						    }
						    if (FaelligkeitTag < 1)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
 								return false;
						    }
						    if (FaelligkeitTag > 31)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
								return false;
						    }

						    var FaelligkeitMonat = Teile[1];
						    if (isNaN(FaelligkeitMonat) == true)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
								return false;
						    }
						    if (FaelligkeitMonat < 1)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
 								return false;
						    }
						    if (FaelligkeitMonat > 12)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
								return false;
						    }

						    var FaelligkeitJahr = Teile[2];
						    if (isNaN(FaelligkeitJahr) == true)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
								return false;
						    }
						    if (FaelligkeitJahr < 1900)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
 								return false;
						    }
						    if (FaelligkeitJahr > 3000)
						    {
								alert("Bitte geben Sie ein Datum im Format TT.MM.JJJJ ein.")
								return false;
						    }

						    var jetzt = new Date();
						    jetzt = new Date(jetzt.getFullYear(),jetzt.getMonth(),jetzt.getDate(),0,0,0);
						    var Zeit = jetzt.getTime() / 86400000;
						    var ziel = new Date(FaelligkeitJahr,FaelligkeitMonat-1,FaelligkeitTag,0,0,0);
						    var Endzeit = ziel.getTime() / 86400000;
						    var Rest = Math.floor(Endzeit - Zeit);
						    var Alarm = " \n\n";
						    var Ausgabe = "Wollen Sie dieses Fälligkeitsdatum wirklich setzen?\n\n\tVertrag\t\t" + Vertrag + "\n\tFälligkeitsdatum\t" + FaelligkeitTag + "." + FaelligkeitMonat + "." + FaelligkeitJahr;
						    
						    if (Rest < 0)
						    {
								Alarm = "Das Datum liegt in der Vergangenheit.\n\n";
						    }
						    
						    if (Zeit == Endzeit)
						    {
								Alarm = "Das Datum ist heute.\n\n";
						    }
						    
						    if (Rest > 30)
						    {
								Alarm = "Das Datum liegt mehr als 30 Tage in der Zukunft.\n\n";
						    }
						    
						    if (Rest > 60)
						    {
								Alarm = "Das Datum liegt mehr als 60 Tage in der Zukunft.\n\n";
						    }

						    
						    var Check = window.confirm(Alarm + Ausgabe);
						    if (Check)
						      {
								window.document.Form1.txtVertragsnummer.value = Vertrag;
								window.document.Form1.txtFaelligkeit.value = Faelligkeit;
						      }
						    return (Check);
						  }

						function FaelligkeitEdit(Vertrag)
						  {
							window.document.Form1.txtVertragsnummer.value = Vertrag;
						  }
						//-->
						</SCRIPT>
						<asp:literal id="Literal1" runat="server"></asp:literal></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
