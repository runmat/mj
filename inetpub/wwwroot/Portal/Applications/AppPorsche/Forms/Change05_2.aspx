<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change05_2.aspx.vb" Inherits="AppPorsche.Change05_2" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<TD></TD>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<TD></TD>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Werte ändern)</asp:label></td>
							</TR>
							<TR>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkVertragssuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change05.aspx">Händlerauswahl</asp:hyperlink>&nbsp;
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
														<TD class="" noWrap align="left" colSpan="2"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</tr>
													<TR >
														<TD noWrap align="left" colSpan="2">&nbsp;<asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
													</TR>
                                                    <tr id="trInanspruchnahme" runat="server">
                                                        <td nowrap align="left" colspan="2" >
                                                            &nbsp;<asp:Label ID="lbl_InanspruchnahmeText" runat="server" Visible="True" Font-Bold="True"></asp:Label> &nbsp; 
                                                             <asp:HyperLink ID="lnkInanspruchnahme" runat="server" Target="_blank" Text="Inanspruchnahme" CssClass="StandardButtonTable"></asp:hyperlink>
                                                        </td>
                                                    </tr>
													<TR>
														<TD class="" noWrap width="100%"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">
																<strong>Excelformat</strong></asp:hyperlink>&nbsp;
															<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label>&nbsp;</TD>
														<TD noWrap align="right"><asp:hyperlink id="HyperLink1" runat="server" CssClass="StandardButton" NavigateUrl="Druck1.aspx" Visible="False" Target="_blank">&#149;&nbsp;Druckversion</asp:hyperlink>&nbsp;&nbsp;&nbsp;
															<asp:dropdownlist id="ddlPageSize" runat="server" Visible="False" AutoPostBack="True"></asp:dropdownlist></TD>
													</TR>
												</table>
												<asp:datagrid id="DataGrid1" runat="server" PageSize="50" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" Width="100%">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="EQUNR" SortExpression="EQUNR" HeaderText="Equipment"></asp:BoundColumn>
														<asp:BoundColumn DataField="VBELN" SortExpression="VBELN" HeaderText="Auftragsnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" HeaderText="Vertragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZANFDT" HeaderText="Anforderungsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="ERZET" HeaderText="Anforderungsuhrzeit"></asp:BoundColumn>
														<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="Nummer ZB2"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="ABCKZ" SortExpression="ABCKZ" HeaderText="Finanz.&lt;br&gt;Art"></asp:BoundColumn>
														<asp:BoundColumn DataField="KontingentartText" SortExpression="KontingentartText" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Versandart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
														<asp:TemplateColumn Visible="False" SortExpression="ABCKZ" HeaderText="Art">
															<ItemTemplate>
																<asp:Label id=Label5 runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ABCKZ") %>'>
																</asp:Label>
																<asp:Literal id=Literal2 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.VBELN") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.ABCKZ") &amp; "</a>" %>'>
																</asp:Literal>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id="TextBox4" runat="server"></asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" HeaderText="Zulassungsart*">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn Visible="False" SortExpression="Faelligkeit" HeaderText="F&#228;lligkeit">
															<ItemTemplate>
																<asp:TextBox id="TextBox2" runat="server" Width="60px" MaxLength="10"></asp:TextBox>
																<asp:Label id="Label2" runat="server"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" SortExpression="Kunde" HeaderText="Kunde">
															<ItemTemplate>
																<asp:TextBox id="Textbox1" runat="server" Width="75px" MaxLength="50"></asp:TextBox>
																<asp:Label id="Label3" runat="server"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<TABLE id="Table11" cellSpacing="1" cellPadding="1" width="100%" border="0">
																	<TR>
																		<TD align="right">
																			<asp:LinkButton id=Linkbutton3 runat="server" CssClass="StandardButtonTable" Visible='<%# DataBinder.Eval(Container, "DataItem.Status")<>"Freigabe OK." AND DataBinder.Eval(Container, "DataItem.Status")<>"Storno OK." %>' CausesValidation="False" CommandName="Freigabe" Text="Freigeben">Freigabe</asp:LinkButton>&nbsp;
																			<asp:LinkButton id=Linkbutton1 runat="server" CssClass="StandardButtonStorno" Visible='<%# DataBinder.Eval(Container, "DataItem.Status")<>"Freigabe OK." AND DataBinder.Eval(Container, "DataItem.Status")<>"Storno OK." %>' CausesValidation="False" CommandName="Storno" Text="Stornieren">Storno</asp:LinkButton></TD>
																	</TR>
																</TABLE>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
										<TR>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"></TD>
							</TR>
							<TR>
							</TR>
						</TABLE>
						<!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD></TD>
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
