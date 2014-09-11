<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change07_3.aspx.vb" Inherits="AppSIXT.Change07_3" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
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
		<form id="Form1" name="Form1" method="post" runat="server">
			<input type="hidden" value="empty" name="PDINummer"> <input type="hidden" value="empty" name="NummerInGrid">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Auswahl von Modellen bzw. Fahrzeugen)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdWeiter" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdAbsenden" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdVerwerfen" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdWeitereAuswahl" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdZurueck" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change07.aspx"> Auswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr id="rowExcelLink" runat="server">
											<td><asp:hyperlink id="lnkExcel" runat="server" Target="_blank">
													<strong>Excelformat</strong></asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label>&nbsp;</td>
										</tr>
										<TR>
											<TD><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left"><DBWC:HIERARGRID id="HG1" style="Z-INDEX: 101" runat="server" AutoGenerateColumns="False" Width="100%" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="0" TemplateCachingBase="Tablename" LoadControlMode="UserControl" TemplateDataMode="Table">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="GridTableItem"></ItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn Visible="False" SortExpression="Details" HeaderText="Details">
															<ItemTemplate>
																<asp:CheckBox id=chkDetails runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Details") %>'>
																</asp:CheckBox>
																<asp:CheckBox id=chkLoaded runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Loaded") %>'>
																</asp:CheckBox>
															</ItemTemplate>
															<FooterStyle Wrap="False"></FooterStyle>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="REGELNAME" SortExpression="REGELNAME" HeaderText="Regelbezeichnung">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<FooterStyle Wrap="False"></FooterStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Erstellungsdatum" SortExpression="Erstellungsdatum" HeaderText="Erstellungsdatum">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<FooterStyle Wrap="False"></FooterStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Regelersteller" SortExpression="Regelersteller" HeaderText="Regelersteller">
															<HeaderStyle Wrap="False" Width="70%"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<FooterStyle Wrap="False"></FooterStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Regelgruppe" SortExpression="Regelgruppe" HeaderText="Regelgruppe">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<FooterStyle Wrap="False"></FooterStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Anzahl" SortExpression="Anzahl" HeaderText="Anzahl gesucht">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Link zur Anzeige der kompl. Regel" HeaderText="Link zur Anzeige der kompl. Regel">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<ItemTemplate>
																<asp:Literal id=litLink runat="server" Text='<%# "<a href=""Change07_2.aspx?RegelID=" &amp; DataBinder.Eval(Container, "DataItem.RegelID") &amp;  """ Target=""_blank"">Anzeige</a>" %>'>
																</asp:Literal>
															</ItemTemplate>
															<FooterStyle Wrap="False"></FooterStyle>
														</asp:TemplateColumn>
													</Columns>
												</DBWC:HIERARGRID></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD class="LabelExtraLarge"><asp:label id="lblMessage" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD> <!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<script language="JavaScript">
		<!-- //
			function DisableControls()
			{
			  var j;
			  for(var i=0;i<window.document.Form1.length;++i)
			  {
			    if(window.document.Form1.elements[i].type == "checkbox")
			    {
			      document.getElementById(window.document.Form1.elements[i].name).disabled = true;
			    }
			    if(window.document.Form1.elements[i].type == "text")
			    {
			      document.getElementById(window.document.Form1.elements[i].name).disabled = true;
			    }
			  }
			}
			function DetailsSuchen(PDI,Zeile,Task)
			{
			window.document.Form1.PDINummer.value = PDI;
			window.document.Form1.NummerInGrid.value = Zeile;						
			}
			
			function SetValues(ModellID,Task)
			{
			  var j;
			  for(var i=0;i<window.document.Form1.length;++i)	//Haken suchen, der markiert wurde (Auswahl rechts). j = Spaltenindex.
			  {
			    if(window.document.Form1.elements[i].name == "Anzahl_alt_"+ModellID)
			    {
			      j = i;
			      break;
			    }
			  }
			  var k = j;
			  var Bemerkung = "";
			  var BemerkungDatum = "";											//Bemerkung Datum
			  var DatumErstZulassung = "";
			  var ZielPDI = "";
			  var Anzahl_alt=Number(window.document.Form1.elements[j].value);
			  var Anzahl_neu=Number(window.document.Form1.elements[j+1].value);
			  switch(Task)
			  {
			   case "Zulassen":													//Werte aus der Modell - Ebene holen...
			     DatumErstZulassung = window.document.Form1.elements[j-1].value;
			     window.document.Form1.elements[j-3].disabled = true;
				 break;
			   case "Verschieben":
			     ZielPDI = window.document.Form1.elements[j-1].value;
			     Bemerkung = window.document.Form1.elements[j-2].value;
			     BemerkungDatum = window.document.Form1.elements[j-3].value;	//Bemerkung Datum
			     break;
			   case "Sperren":
				 Bemerkung = window.document.Form1.elements[j-2].value;
			     BemerkungDatum = window.document.Form1.elements[j-1].value;	//Bemerkung Datum
			     break;
			    case "Entsperren":
				 Bemerkung = window.document.Form1.elements[j-2].value;
			     BemerkungDatum = window.document.Form1.elements[j-1].value;	//Bemerkung Datum
			     break;
			   default:
			     Bemerkung = window.document.Form1.elements[j-1].value;
			     break;
			  }
			  if(isNaN(Anzahl_neu) == true)
			  {
			    alert("Bitte geben Sie einen Zahlenwert für die Anzahl ein.");
			  }
			  else
			  {
			    if(Anzahl_alt<Anzahl_neu)
			    {
			      alert("Der Zahlenwert für die Anzahl ist zu groß.");
			    }
			    else
			    {
			      if(Anzahl_neu<0)
			      {
			        alert("Der Zahlenwert für die Anzahl ist zu klein.");
			      }
			      else
			      {
			        j=0;
			        for(i=k;i<window.document.Form1.length;++i)			//Werte in die Fahrzeugebene übertragen...
			        {
					  if(window.document.Form1.elements[i].value == "Modell_ID_" + ModellID)
					  {
					    if(j<Anzahl_alt)
					    {
							if(j<Anzahl_neu)
							{
								window.document.Form1.elements[i-1].checked = true;
								switch(Task)
								{
								case "Zulassen":
									window.document.Form1.elements[i-3].value = DatumErstZulassung;									
									window.document.Form1.elements[i-4].disabled = true;
									break;
								case "Verschieben":
									window.document.Form1.elements[i-3].value = ZielPDI;
									window.document.Form1.elements[i-4].value = Bemerkung;			
									window.document.Form1.elements[i-5].value = BemerkungDatum;	//Bemerkung Datum
									break;
								case "Sperren":
									window.document.Form1.elements[i-4].value = Bemerkung;			
									window.document.Form1.elements[i-3].value = BemerkungDatum;	//Bemerkung Datum
									break;
								case "Entsperren":
									window.document.Form1.elements[i-4].value = Bemerkung;			
									window.document.Form1.elements[i-3].value = BemerkungDatum;	//Bemerkung Datum
									break;
								default:
									window.document.Form1.elements[i-3].value = Bemerkung;
									break;
								}
							}
							else
							{
								window.document.Form1.elements[i-1].checked = false;
								switch(Task)
								{
								case "Zulassen":
									window.document.Form1.elements[i-3].value = "";
								case "Verschieben":
									window.document.Form1.elements[i-3].value = "";
									window.document.Form1.elements[i-4].value = "";
								case "Sperren":
									window.document.Form1.elements[i-4].value = "";			
									window.document.Form1.elements[i-3].value = "";	//Bemerkung Datum
									break;
								case "Entsperren":
									window.document.Form1.elements[i-4].value = "";			
									window.document.Form1.elements[i-3].value = "";	//Bemerkung Datum
									break;
								default:
									window.document.Form1.elements[i-3].value = "";
								}
							}
							j = j + 1;
					    }
					    else
					    {
							break;
					    }
					  }
			        }
			        document.getElementById("Picture_" + ModellID).src = "../Images/Confirm_Mini.GIF"
			      }
			    }
			  }
			}
			
			function openinfo (url) {
				fenster=window.open(url, "Zulassungsstatus", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=800,height=600");
				fenster.focus();
			}
		//-->
			</script>
			<asp:literal id="litAddScript" runat="server"></asp:literal></form>
	</body>
</HTML>
