<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02Edit.aspx.vb" Inherits="AppFFE.Change02Edit" %>
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
			<input type="hidden" value=" " name="txtStorno"> <input type="hidden" value=" " name="txtFreigeben">
			<input type="hidden" value=" " name="txtAuftragsNummer"> <input type="hidden" value=" " name="txtVertragsnummer">
			<input type="hidden" value=" " name="txtAngefordert"> <input type="hidden" value=" " name="txtFahrgestellnummer">
			<input type="hidden" value=" " name="txtBriefnummer"> <INPUT type="hidden" value=" " name="txtZulassungsart">
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
									<asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top">
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change03.aspx">Händlersuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkVertragssuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change03_2.aspx">Händlerauswahl</asp:hyperlink>&nbsp;
												<asp:hyperlink id="lnkDistrikt" runat="server" NavigateUrl="Change02.aspx" CssClass="TaskTitle" Visible="False">Distriktsuche</asp:hyperlink>
												<asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
										<TR>
											<TD>
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<TD class="" noWrap align="left" colSpan="2">
															<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</tr>
													<TR>
														<TD noWrap align="left" colSpan="2">
															<asp:label id="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:label></TD>
													</TR>
													<TR>
														<TD class="" noWrap width="385"><STRONG>Fälligkeitsdatum (nur Delayed Payment):</STRONG>&nbsp;
															<asp:textbox id="txtFaelligkeit" runat="server" Width="100px" MaxLength="10"></asp:textbox></TD>
														<TD noWrap align="right" >
												            &nbsp; <asp:hyperlink id="HyperLink1" runat="server" NavigateUrl="Druck1.aspx" CssClass="StandardButton" Target="_blank">•&nbsp;Druckversion</asp:hyperlink>&nbsp;
												            <img   alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />&nbsp;<STRONG><asp:LinkButton CssClass="ExcelButton" id="lnkCreateExcel"  runat="server" Visible="False">Excelformat</asp:LinkButton></STRONG>
															&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Visible="False"></asp:dropdownlist></TD>
													</TR>
												</table>
											</TD>
										</TR>
										<TR>
											<TD>
												<asp:datagrid id="DataGrid1" runat="server" Width="100%" PageSize="50" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
													<asp:TemplateColumn SortExpression="VBELN" HeaderText="col_Auftragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Auftragsnummer" runat="server" CommandName="Sort" CommandArgument="VBELN">col_Auftragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VBELN") %>' ID="Label1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
													<asp:TemplateColumn SortExpression="ZZREFNR" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFNR">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFNR") %>' ID="Label2">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>														

													<asp:TemplateColumn SortExpression="ZZANFDT" HeaderText="col_Angefordertam">
															<HeaderTemplate>
																<asp:LinkButton id="col_Angefordertam" runat="server" CommandName="Sort" CommandArgument="ZZANFDT">col_Angefordertam</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZANFDT") %>' ID="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
													<asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>' ID="Label4">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
													<asp:TemplateColumn SortExpression="ZZBRIEF" HeaderText="col_Briefnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="ZZBRIEF">col_Briefnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZBRIEF") %>' ID="Label5">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
													<asp:TemplateColumn SortExpression="ZZFINART" HeaderText="col_Finanzierungsart">
															<HeaderTemplate>
																<asp:LinkButton id="col_Finanzierungsart" runat="server" CommandName="Sort" CommandArgument="ZZFINART">col_Finanzierungsart</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFINART") %>' ID="Label7">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>	
														<asp:TemplateColumn SortExpression="BSTZD" HeaderText="col_Kontingentart">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kontingentart" runat="server" CommandName="Sort" CommandArgument="BSTZD">col_Kontingentart</asp:LinkButton>
															</HeaderTemplate>															
															<ItemTemplate>
																<asp:Label id="Label8" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.BSTZD") %>'>
																</asp:Label>
																<asp:Literal id="Literal2" runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.VBELN") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.BSTZD") &amp; "</a>" %>'>
																</asp:Literal>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id="TextBox4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BSTZD") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
																											
														<asp:TemplateColumn Visible="False" SortExpression="KVGR3" HeaderText="col_Zulassungsart">
															<HeaderTemplate>
																<asp:LinkButton id="col_Zulassungsart" runat="server" CommandName="Sort" CommandArgument="KVGR3">col_Zulassungsart</asp:LinkButton>
															</HeaderTemplate>														
															<ItemTemplate>
																<asp:TextBox id="txt1" runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Width="75px" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.KVGR3") %>'>
																</asp:TextBox>
																<asp:Label id="Label10" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text='<%# DataBinder.Eval(Container, "DataItem.KVGR3") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn  SortExpression="ZZFAEDT" HeaderText="col_Faelligkeit">
															<HeaderTemplate>
																<asp:LinkButton id="col_Faelligkeit" runat="server" CommandName="Sort" CommandArgument="ZZFAEDT">col_Faelligkeit</asp:LinkButton>
															</HeaderTemplate>														
															<ItemTemplate>
																<asp:TextBox id="txtFaell" runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAEDT", "{0:d}") %>' MaxLength="10">
																</asp:TextBox>
																<asp:Label id="Label12" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAEDT", "{0:d}") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn  SortExpression="TEXT50" HeaderText="col_Kunde">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kunde" runat="server" CommandName="Sort" CommandArgument="TEXT50">col_Kunde</asp:LinkButton>
															</HeaderTemplate>	
															<ItemTemplate>
																<asp:TextBox id="txtKunde" runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Width="75px" Text='<%# DataBinder.Eval(Container, "DataItem.TEXT50") %>' MaxLength="50">
																</asp:TextBox>
																<asp:Label id="Label13" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text='<%# DataBinder.Eval(Container, "DataItem.TEXT50") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Aktion">
															<HeaderTemplate>
															<asp:LinkButton id="col_Aktion" runat="server">col_Aktion</asp:LinkButton>
									    					</HeaderTemplate>
															<ItemTemplate>
																<TABLE id="Table11" cellSpacing="1" cellPadding="1" width="100%" border="0">
																	<TR>
																		<TD align="left">
																			<asp:LinkButton id=Linkbutton3 runat="server" CssClass="StandardButtonSmall" Visible='<%# Not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text="Freigeben" CausesValidation="False" CommandName="Freigabe">Freigabe</asp:LinkButton></TD>
																		<TD align="right">
																			<asp:LinkButton id=Linkbutton4 runat="server" CssClass="StandardButtonStorno" Visible='<%# Not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text="Stornieren" CausesValidation="False" CommandName="Storno">Storno</asp:LinkButton></TD>
																	</TR>
																</TABLE>
																<asp:Literal id=Literal4 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text='<%# "Aut.(" &amp; DataBinder.Eval(Container, "DataItem.Initiator") &amp; ")" %>'>
																</asp:Literal>
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
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left">
									<asp:label id="lblLegende" runat="server" Visible="False">*N=Neufahrzeug, S=Selbstfahrervermietfahrzeug, V=Vorführfahrzeug</asp:label></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<SCRIPT language="Javascript">
						<!-- //
						function FreigebenConfirm(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr)
						  {
						    var Check = window.confirm("Wollen Sie diesen Kfz-Brief wirklich freigeben?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
						    if (Check)
						      {
								window.document.Form1.txtStorno.value = " ";
								window.document.Form1.txtFreigeben.value = "X";
								window.document.Form1.txtAuftragsNummer.value = Auftrag;
								window.document.Form1.txtVertragsnummer.value = Vertrag;
								window.document.Form1.txtAngefordert.value = Angefordert;
								window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
								window.document.Form1.txtBriefnummer.value = BriefNr;
								window.document.Form1.txtZulassungsart.value = Zulassungsart;
						      }
						    return (Check);
						  }

						function DoFreigeben(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr,Zulassungsart)
						  {
							window.document.Form1.txtStorno.value = " ";
							window.document.Form1.txtFreigeben.value = "X";
							window.document.Form1.txtAuftragsNummer.value = Auftrag;
							window.document.Form1.txtVertragsnummer.value = Vertrag;
							window.document.Form1.txtAngefordert.value = Angefordert;
							window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
							window.document.Form1.txtBriefnummer.value = BriefNr;
							window.document.Form1.txtZulassungsart.value = Zulassungsart;
						  }

						function StornoConfirm(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr)
						  {
						    var Check = window.confirm("Wollen Sie diesen Kfz-Brief wirklich stornieren?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
						    if (Check)
						      {
								window.document.Form1.txtStorno.value = "X";
								window.document.Form1.txtFreigeben.value = " ";
								window.document.Form1.txtAuftragsNummer.value = Auftrag;
								window.document.Form1.txtVertragsnummer.value = Vertrag;
								window.document.Form1.txtAngefordert.value = Angefordert;
								window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
								window.document.Form1.txtBriefnummer.value = BriefNr;
						      }
						    return (Check);
						  }

						function DoStorno(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr)
						  {
							window.document.Form1.txtStorno.value = "X";
							window.document.Form1.txtFreigeben.value = " ";
							window.document.Form1.txtAuftragsNummer.value = Auftrag;
							window.document.Form1.txtVertragsnummer.value = Vertrag;
							window.document.Form1.txtAngefordert.value = Angefordert;
							window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
							window.document.Form1.txtBriefnummer.value = BriefNr;
						  }
						//-->
						</SCRIPT>
						<asp:literal id="Literal1" runat="server"></asp:literal></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
