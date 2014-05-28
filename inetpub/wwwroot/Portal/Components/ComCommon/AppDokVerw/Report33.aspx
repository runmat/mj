﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report33.aspx.vb" Inherits="CKG.Components.ComCommon.Report33" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>

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
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"></asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top">
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;<asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD align="right">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD>
															<asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
														<TD align="right">
															<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</TR>
													<TR>
														<TD class="" colSpan="2"><STRONG>
																<TABLE id="Table4" cellSpacing="0" cellPadding="5" width="100%" bgColor="#ffffff" border="0">
																	<TR id="trMemo" runat="server">
																		<TD noWrap rowSpan="2"><STRONG>Memo für Vertrag:</STRONG> &nbsp;
																			<asp:label id="lblVertragsNummer" runat="server" Font-Bold="True"></asp:label>&nbsp;
																			<asp:textbox id="Memo" runat="server" MaxLength="200" Width="247px"></asp:textbox></TD>
																		<TD noWrap width="100%">
																			<asp:LinkButton id="SaveMemo" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Speichern</asp:LinkButton>&nbsp;
																			<asp:LinkButton id="CancelMemo" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Abbrechen</asp:LinkButton></TD>
																	</TR>
																</TABLE>
															</STRONG>
														</TD>
													</TR>
												</table>
												<strong>
                                                            <img alt="" id="ExcelImg" runat="server" visible="false" src="../../../images/excel.gif" style="width: 15px; height: 17px" /> <asp:LinkButton
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" visible="false" runat="server">Excelformat</asp:LinkButton> </strong>&nbsp;
												&nbsp;
												<asp:dropdownlist id="ddlPageSize" runat="server"  AutoPostBack="True"></asp:dropdownlist>
											</TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
											</TD>
										</TR>
										<TR>
											<TD>
												<asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Vertragsnummer" CommandName="sort" CommandArgument="Vertragsnummer" Runat="server">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="lblVertrag" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Fahrgestellnummer" CommandName="sort" CommandArgument="Fahrgestellnummer" Runat="server">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:HyperLink id="VIN" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' ToolTip="Anzeige Fahrzeughistorie" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Kennzeichen" CommandName="sort" CommandArgument="Kennzeichen" Runat="server">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label2">
																	<%#DataBinder.Eval(Container, "DataItem.Kennzeichen")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														<asp:TemplateColumn SortExpression="Briefnummer" HeaderText="col_Briefnummer">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Briefnummer" CommandName="sort" CommandArgument="Briefnummer" Runat="server">col_Briefnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label3">
																	<%#DataBinder.Eval(Container, "DataItem.Briefnummer")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>															
														<asp:TemplateColumn SortExpression="Versand" HeaderText="col_Versand">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Versand" CommandName="sort" CommandArgument="Versand" Runat="server">col_Versand</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label4">
																	<%#DataBinder.Eval(Container, "DataItem.Versand", "{0:d}")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>		
														<asp:TemplateColumn SortExpression="Versandgrund" HeaderText="col_Versandgrund">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Versandgrund" CommandName="sort" CommandArgument="Versandgrund" Runat="server">col_Versandgrund</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label5">
																	<%#DataBinder.Eval(Container, "DataItem.Versandgrund")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														<asp:TemplateColumn SortExpression="Name1" HeaderText="col_Name1">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Name1" CommandName="sort" CommandArgument="Name1" Runat="server">col_Name1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label6">
																	<%#DataBinder.Eval(Container, "DataItem.Name1")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Strasse" HeaderText="col_Strasse">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Strasse" CommandName="sort" CommandArgument="Strasse" Runat="server">col_Strasse</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label7">
																	<%#DataBinder.Eval(Container, "DataItem.Strasse")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>														
														<asp:TemplateColumn SortExpression="Ort" HeaderText="col_Ort">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Ort" CommandName="sort" CommandArgument="Ort" Runat="server">col_Ort</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label8">
																	<%#DataBinder.Eval(Container, "DataItem.Ort")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>															
														<asp:TemplateColumn SortExpression="Mahnstufe" HeaderText="col_Mahnstufe">
															<HeaderTemplate>
																<asp:LinkButton ID="col_Mahnstufe" CommandName="sort" CommandArgument="Mahnstufe" Runat="server">col_Mahnstufe</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label Visible="True" Runat="server" ID="Label9">
																	<%#DataBinder.Eval(Container, "DataItem.Mahnstufe")%>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														
														<asp:TemplateColumn SortExpression="MemoString" HeaderText="col_MemoString">
															<HeaderTemplate>
																<asp:LinkButton ID="col_MemoString" CommandName="sort" CommandArgument="MemoString" Runat="server">col_MemoString</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:LinkButton id="WriteMemo" runat="server" CssClass="StandardButtonSmall" Text='<%# DataBinder.Eval(Container, "DataItem.MemoString") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.Memo") %>' CommandName="WriteMemo" CausesValidation="False">
																</asp:LinkButton>
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
