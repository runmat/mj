<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report06.aspx.vb" Inherits="AppF1.Report06" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="2">
								    <asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label>
								</td>
							</tr>
							<tr>
								<td class="TaskTitle" valign="top" width="120">&nbsp;</td>
								<td class="TaskTitle" valign="top">
								    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Report06.aspx" CssClass="TaskTitle">Zusammenstellung von Abfragekriterien</asp:hyperlink>
                                            </td>
                                            <td align="right" valign="bottom">
                                                <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />
                                                <asp:LinkButton ID="lnkCreateExcel" CssClass="ExcelButton" runat="server" Visible="False"><strong>Excelformat</strong></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
								</td>
							</tr>
							<tr>
								<td valign="top" width="120">
									<table borderColor="#ffffff" cellspacing="0" cellpadding="0" width="120" border="0">
										<tr>
											<td class="TextHeader" width="150"></td>
										</tr>
										<tr>
											<td valign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Enabled="False">OK</asp:linkbutton></td>
										</tr>
									</table>
								</td>
								<td valign="top">
									<table cellspacing="0" cellpadding="0" width="100%" bgColor="white" border="0">
										<tr>
											<td>
												<p align="center"><b><font size="4"><u>Erinnerung</u></font></b></p>
												<p>
													Für die unten aufgeführten Fahrzeugdokumente konnten wir leider noch keinen Wiedereingang bzw. Zahlungseingang feststellen.
												</p>
											</td>
										</tr>
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="right" valign="bottom">
                                                            Ergebnisse/Seite:&nbsp;
                                                            <asp:DropDownList ID="ddlPageSize1" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
								</td>
                            </tr>
							<tr>
							    <td valign="top" width="120">&nbsp;</td>
								<td class="LabelExtraLarge">
								    <asp:label id="lblNoData1" runat="server" Visible="False"></asp:label>
								</td>
							</tr>
							<tr>
							    <td valign="top" width="120">&nbsp;</td>
								<td>
									<asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
										    <asp:TemplateColumn SortExpression="MahnArtText" HeaderText="col_MahnArtText" >
												<HeaderTemplate>
													<asp:LinkButton ID="col_MahnArtText" CommandName="sort" CommandArgument="MahnArtText" Runat="server">col_MahnArtText</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label0">
														<%#DataBinder.Eval(Container, "DataItem.MahnArtText")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>										
											<asp:TemplateColumn SortExpression="HAENDLER_EX" HeaderText="col_HAENDLER_EX" >
												<HeaderTemplate>
													<asp:LinkButton ID="col_HAENDLER_EX" CommandName="sort" CommandArgument="HAENDLER_EX" Runat="server">col_HAENDLER_EX</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label1">
														<%#DataBinder.Eval(Container, "DataItem.HAENDLER_EX")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="ZZLSDAT" HeaderText="col_ZZLSDAT">
												<HeaderTemplate>
													<asp:LinkButton ID="col_ZZLSDAT" CommandName="sort" CommandArgument="ZZLSDAT" Runat="server">col_ZZLSDAT</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label  Runat="server" ID="Label3">
														<%#DataBinder.Eval(Container, "DataItem.ZZLSDAT", "{0:d}")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Abrufgrund" HeaderText="col_Abrufgrund" >
												<HeaderTemplate>
													<asp:LinkButton ID="col_Abrufgrund" CommandName="sort" CommandArgument="Abrufgrund" Runat="server">col_Abrufgrund</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label  Runat="server" ID="Label2">
														<%#DataBinder.Eval(Container, "DataItem.Abrufgrund")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_TIDNR" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_TIDNR" CommandName="sort" CommandArgument="TIDNR" Runat="server">col_TIDNR</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="Label4">
														<%#DataBinder.Eval(Container, "DataItem.TIDNR")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="CHASSIS_NUM" HeaderText="col_CHASSIS_NUM" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_CHASSIS_NUM" CommandName="sort" CommandArgument="CHASSIS_NUM" Runat="server">col_CHASSIS_NUM</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                    </asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_LICENSE_NUM" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_LICENSE_NUM" CommandName="sort" CommandArgument="LICENSE_NUM" Runat="server">col_LICENSE_NUM</asp:LinkButton>
												</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label Visible="True" Runat="server" ID="lblVertragsnummer">
														<%#DataBinder.Eval(Container, "DataItem.LICENSE_NUM")%>
													</asp:Label>
                                                </ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="OFFENER_BETRAG" HeaderText="col_OFFENER_BETRAG" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_OFFENER_BETRAG" CommandName="sort" CommandArgument="OFFENER_BETRAG" Runat="server">col_OFFENER_BETRAG</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label6" Text='<%# String.Format("{0:C}", Convert.ToDouble(Eval("[OFFENER_BETRAG]"))) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="MELDUNG_AN_AG" HeaderText="col_MELDUNG_AN_AG" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_MELDUNG_AN_AG" CommandName="sort" CommandArgument="MELDUNG_AN_AG" Runat="server">col_MELDUNG_AN_AG</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label5">
														<%#DataBinder.Eval(Container, "DataItem.MELDUNG_AN_AG", "{0:d}")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"></PagerStyle>
									</asp:datagrid>
                                </td>
							</tr>
                            <tr>
                                <td valign="top" width="120">&nbsp;</td>
							    <td valign="top">&nbsp;</td>
							</tr>
                            <tr>
								<td valign="top" width="120">&nbsp;</td>
								<td valign="top">
									<table cellspacing="0" cellpadding="0" width="100%" bgColor="white" border="0">
									    <tr>
                                            <td valign="top">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td valign="top">&nbsp;</td>
                                        </tr>
										<tr>
											<td>
												<p>
													Die unten stehenden Fahrzeugdokumente haben Sie als Opel Bank-Endkundengeschäft angefordert.
                                                    <br/>
                                                    Bitte reichen Sie die Fahrzeugdokumente mit den zugehörigen Verträgen beim RSC-Ankauf ein.
												</p>
											</td>
										</tr>
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="right" valign="bottom">
                                                            Ergebnisse/Seite:&nbsp;
                                                            <asp:DropDownList ID="ddlPageSize2" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
								</td>
                            </tr>
							<tr>
							    <td valign="top" width="120">&nbsp;</td>
								<td class="LabelExtraLarge">
								    <asp:label id="lblNoData2" runat="server" Visible="False"></asp:label>
								</td>
							</tr>
							<tr>
							    <td valign="top" width="120">&nbsp;</td>
								<td>
									<asp:datagrid id="DataGrid2" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
										    <asp:TemplateColumn SortExpression="MahnArtText" HeaderText="col_MahnArtText" >
												<HeaderTemplate>
													<asp:LinkButton ID="col_MahnArtText" CommandName="sort" CommandArgument="MahnArtText" Runat="server">col_MahnArtText</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label0">
														<%#DataBinder.Eval(Container, "DataItem.MahnArtText")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>										
											<asp:TemplateColumn SortExpression="HAENDLER_EX" HeaderText="col_HAENDLER_EX" >
												<HeaderTemplate>
													<asp:LinkButton ID="col_HAENDLER_EX" CommandName="sort" CommandArgument="HAENDLER_EX" Runat="server">col_HAENDLER_EX</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label1">
														<%#DataBinder.Eval(Container, "DataItem.HAENDLER_EX")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="ZZLSDAT" HeaderText="col_ZZLSDAT">
												<HeaderTemplate>
													<asp:LinkButton ID="col_ZZLSDAT" CommandName="sort" CommandArgument="ZZLSDAT" Runat="server">col_ZZLSDAT</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label  Runat="server" ID="Label3">
														<%#DataBinder.Eval(Container, "DataItem.ZZLSDAT", "{0:d}")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Abrufgrund" HeaderText="col_Abrufgrund" >
												<HeaderTemplate>
													<asp:LinkButton ID="col_Abrufgrund" CommandName="sort" CommandArgument="Abrufgrund" Runat="server">col_Abrufgrund</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label  Runat="server" ID="Label2">
														<%#DataBinder.Eval(Container, "DataItem.Abrufgrund")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_TIDNR" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_TIDNR" CommandName="sort" CommandArgument="TIDNR" Runat="server">col_TIDNR</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Visible="True" Runat="server" ID="Label4">
														<%#DataBinder.Eval(Container, "DataItem.TIDNR")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="CHASSIS_NUM" HeaderText="col_CHASSIS_NUM" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_CHASSIS_NUM" CommandName="sort" CommandArgument="CHASSIS_NUM" Runat="server">col_CHASSIS_NUM</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                    </asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_LICENSE_NUM" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_LICENSE_NUM" CommandName="sort" CommandArgument="LICENSE_NUM" Runat="server">col_LICENSE_NUM</asp:LinkButton>
												</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label Visible="True" Runat="server" ID="lblVertragsnummer">
														<%#DataBinder.Eval(Container, "DataItem.LICENSE_NUM")%>
													</asp:Label>
                                                </ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="OFFENER_BETRAG" HeaderText="col_OFFENER_BETRAG" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_OFFENER_BETRAG" CommandName="sort" CommandArgument="OFFENER_BETRAG" Runat="server">col_OFFENER_BETRAG</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label6" Text='<%# String.Format("{0:C}", Convert.ToDouble(Eval("[OFFENER_BETRAG]"))) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="MELDUNG_AN_AG" HeaderText="col_MELDUNG_AN_AG" Visible="True">
												<HeaderTemplate>
													<asp:LinkButton ID="col_MELDUNG_AN_AG" CommandName="sort" CommandArgument="MELDUNG_AN_AG" Runat="server">col_MELDUNG_AN_AG</asp:LinkButton>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:Label Runat="server" ID="Label5">
														<%#DataBinder.Eval(Container, "DataItem.MELDUNG_AN_AG", "{0:d}")%>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"></PagerStyle>
									</asp:datagrid>
                                </td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td width="100"></td>
					<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
				</tr>
				<tr>
					<td width="100"></td>
					<td><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			    <tr id="ShowScript" runat="server" visible="False">
				    <td>
					    <script language="Javascript">
						    <!-- //
						    function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						    var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
						    return (Check);
						    }
						    //-->
					    </script>
				    </td>
			    </tr>
            </table>
        </form>
	</body>
</HTML>
