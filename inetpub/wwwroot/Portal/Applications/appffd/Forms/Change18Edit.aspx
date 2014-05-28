<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change18Edit.aspx.vb" Inherits="AppFFD.Change18Edit"%>
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
			<INPUT type="hidden" value=" " name="txtAnfragenr">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkKreditlimit" runat="server" NavigateUrl="Change03.aspx" CssClass="TaskTitle">Händlersuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkVertragssuche" runat="server" NavigateUrl="Change03_2.aspx" CssClass="TaskTitle">Händlerauswahl</asp:hyperlink>&nbsp;
												<asp:hyperlink id="lnkDistrikt" runat="server" NavigateUrl="Change02.aspx" CssClass="TaskTitle" Visible="False">Distriktsuche</asp:hyperlink><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
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
														<TD class="" noWrap align="left" colSpan="2"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</tr>
													<TR>
														<TD noWrap align="left" colSpan="2"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
													</TR>
													<TR>
														<TD class="" noWrap width="385">&nbsp;</TD>
														<TD noWrap align="right"><asp:hyperlink id="HyperLink1" runat="server" NavigateUrl="Druck2Retail.aspx" CssClass="StandardButton" Target="_blank">&#149;&nbsp;Druckversion</asp:hyperlink>&nbsp;&nbsp;&nbsp;
															<asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">
																<strong>Excelformat</strong></asp:hyperlink>&nbsp;
															<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label>&nbsp;
															<asp:dropdownlist id="ddlPageSize" runat="server" Visible="False" AutoPostBack="True"></asp:dropdownlist></TD>
													</TR>
												</table>
											</TD>
										</TR>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" PageSize="50">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Auftragsnummer" SortExpression="Auftragsnummer" HeaderText="Auftragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Vertragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Anfragenr" SortExpression="Anfragenr" HeaderText="Anfragenummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Angefordert am" SortExpression="Angefordert am" HeaderText="Angefordert am:" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Finanzierungsart" SortExpression="Finanzierungsart" HeaderText="Finanz.&lt;br&gt;Art"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Kontingentart" HeaderText="Kontingentart">
															<ItemTemplate>
																<asp:Label id=Label5 runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart") %>'>
																</asp:Label>
																<asp:Literal id=Literal2 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.Auftragsnummer") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.Kontingentart") &amp; "</a>" %>'>
																</asp:Literal>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="ART" SortExpression="ART" HeaderText="Zulassungsart*">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Betrag" HeaderText="Betrag">
															<ItemTemplate>
																<asp:TextBox id=txtBetrag runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Width="75px" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.Betrag") %>'>
																</asp:TextBox>
																	</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" SortExpression="Faelligkeit" HeaderText="F&#228;lligkeit">
															<ItemTemplate>
																<asp:TextBox id=txtFaell runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.Faelligkeit", "{0:dd.MM.yyyy}") %>' MaxLength="10">
																</asp:TextBox>
																<asp:Label id=Label2 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Faelligkeit", "{0:dd.MM.yyyy}") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" SortExpression="Kunde" HeaderText="Kunde">
															<ItemTemplate>
																<asp:TextBox id=txtKunde runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Width="75px" Text='<%# DataBinder.Eval(Container, "DataItem.Kunde") %>' MaxLength="50">
																</asp:TextBox>
																<asp:Label id=Label3 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Kunde") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:Label id="Label4" runat="server">Aktion</asp:Label>
															</HeaderTemplate>
															<ItemTemplate>
																<TABLE id="Table11" cellSpacing="1" cellPadding="1" width="100%" border="0">
																	<TR>
																		<TD align="left">
																			<asp:LinkButton id=Linkbutton3 runat="server" CssClass="StandardButtonSmall" Visible='<%# Not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' CausesValidation="False" CommandName="Freigabe" Text="Freigeben">Freigabe</asp:LinkButton></TD>
																		<TD align="right">
																			<asp:LinkButton id=Linkbutton4 runat="server" CssClass="StandardButtonStorno" Visible='<%# Not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>' CausesValidation="False" CommandName="Storno" Text="Stornieren">Storno</asp:LinkButton></TD>
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
								<TD vAlign="top" align="left"><asp:label id="lblLegende" runat="server" Visible="False">*N=Neufahrzeug, S=Selbstfahrervermietfahrzeug, V=Vorführfahrzeug</asp:label></TD>
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
						function FreigebenConfirm(Auftrag,Vertrag,Anfragenr,Angefordert,Fahrgest,BriefNr)
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
								window.document.Form1.txtAnfragenr.value = Anfragenr;
						      }
						    return (Check);
						  }

						function DoFreigeben(Auftrag,Vertrag,Anfragenr,Angefordert,Fahrgest,BriefNr,Zulassungsart)
						  {
							window.document.Form1.txtStorno.value = " ";
							window.document.Form1.txtFreigeben.value = "X";
							window.document.Form1.txtAuftragsNummer.value = Auftrag;
							window.document.Form1.txtVertragsnummer.value = Vertrag;
							window.document.Form1.txtAngefordert.value = Angefordert;
							window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
							window.document.Form1.txtBriefnummer.value = BriefNr;
							window.document.Form1.txtZulassungsart.value = Zulassungsart;
							window.document.Form1.txtAnfragenr.value = Anfragenr;
						  }

						function StornoConfirm(Auftrag,Vertrag,Anfragenr,Angefordert,Fahrgest,BriefNr)
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
								window.document.Form1.txtAnfragenr.value = Anfragenr;
						      }
						    return (Check);
						  }

						function DoStorno(Auftrag,Vertrag,Anfragenr,Angefordert,Fahrgest,BriefNr)
						  {
							window.document.Form1.txtStorno.value = "X";
							window.document.Form1.txtFreigeben.value = " ";
							window.document.Form1.txtAuftragsNummer.value = Auftrag;
							window.document.Form1.txtVertragsnummer.value = Vertrag;
							window.document.Form1.txtAngefordert.value = Angefordert;
							window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
							window.document.Form1.txtBriefnummer.value = BriefNr;
							window.document.Form1.txtAnfragenr.value = Anfragenr;
						  }
						//-->
						</SCRIPT>
						<asp:literal id="Literal1" runat="server"></asp:literal></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
