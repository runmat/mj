<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change06.aspx.vb" Inherits="AppF1.Change06" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>


<%@ Register src="../controls/SucheHaendler.ascx" tagname="SucheHaendler" tagprefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	    <style type="text/css">
            .style1
            {
                width: 97%;
            }
            .style2
            {
                width: 259px;
            }
        </style>
	</HEAD>
	<body topmargin="0" leftmargin="0" MS_POSITIONING="FlowLayout">
    
        <uc1:BusyIndicator runat="server" />

		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td>
					  
                    
                    
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>&nbsp;
									</td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">
												&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:LinkButton id="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" 
                                                    CssClass="StandardButton" Visible="False">Weiter</asp:LinkButton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
                            <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" Visible="false" CssClass="StandardButton">Neue Suche</asp:LinkButton>
                                            </TD>
										</TR>
										</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
										    <td ID="tdHaendlersuche" runat="server" class="TaskTitle" colspan="1">
										        
                                        <asp:Label runat="server" ID="lbl_AnzeigeHaendlerSuche" Font-Underline="true" Font-Bold="true"
                                            ForeColor="Black" Font-Size="Larger" Text="lbl_AnzeigeHaendlerSuche"></asp:Label>
										        
										    </td>
										</tr>
										<tr>
										    <td class="style1" colspan="2">
										        <uc2:SucheHaendler ID="SucheHaendler1" runat="server" />
										    </td>
										</tr>
										<tr>
											<td class="style2" colspan="1" valign="bottom">
															<asp:label id="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:label>
											</td>
											<td ID="tdExcel" runat="server" vAlign="bottom" align="right" 
                                                style="text-align: right" colspan="1" height="50" nowrap="nowrap">
												<STRONG> 
												            <img   alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /><asp:LinkButton CssClass="ExcelButton" id="lnkCreateExcel"  runat="server" Visible="False">Excelformat</asp:LinkButton></STRONG>
															<asp:dropdownlist id="ddlPageSize" runat="server" 
                                                    AutoPostBack="True"></asp:dropdownlist>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colspan="2">
                                            <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400"
                                                CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="col_HaendlerEx" SortExpression="HAENDLER_EX">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_HaendlerEx" runat="server" CommandArgument="Händlernummer" 
                                                                CommandName="Sort">col_HaendlerEx</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHaendlerEx" runat="server" 
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Händlernummer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Auftragsnummer" HeaderText="col_Auftragsnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Auftragsnummer" runat="server" CommandName="Sort" CommandArgument="Auftragsnummer">col_Auftragsnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auftragsnummer") %>'
                                                                ID="lblAuftragsnummer">
                                                            </asp:Label>
                                                       
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Vertragsnummer" CommandName="sort" CommandArgument="Vertragsnummer"
                                                                runat="server">col_Vertragsnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'
                                                                ID="lblVertragsnummer">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Angefordert am" HeaderText="col_Angefordertam">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Angefordertam" runat="server" CommandName="Sort" CommandArgument="Angefordert am">col_Angefordertam</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Angefordert am", "{0:d}" ) %>'
                                                                ID="Label3">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Briefnummer" HeaderText="col_Briefnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Briefnummer" CommandName="sort" CommandArgument="Briefnummer"
                                                                runat="server">col_Briefnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer")%>'
                                                                ID="Label5">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Zustellart" HeaderText="col_Zustellart">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Zustellart" CommandName="sort" CommandArgument="Zustellart"
                                                                runat="server">col_Zustellart</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zustellart")%>'
                                                                ID="lblZustellart">                                                                
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>                                                     
                                                    <asp:TemplateColumn SortExpression="Kontingentart" HeaderText="col_Kontingentart">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kontingentart" CommandName="sort" CommandArgument="Kontingentart"
                                                                runat="server">col_Kontingentart</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart")%>'
                                                                ID="Label2">                                                                
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Status" HeaderText="col_Gesperrt">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Gesperrt" runat="server" CommandName="Sort" CommandArgument="Status">col_Gesperrt</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'
                                                                ID="Label1">
																                                   
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" Text="Storno" CommandName="Storno">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:ButtonColumn>
                                                    <asp:TemplateColumn SortExpression="Kontingent" HeaderText="col_Kontingent">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kontingent" CommandName="sort" CommandArgument="Kontingent"
                                                                runat="server">col_Kontingent</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent")%>'
                                                                ID="Label6">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Equinummer" HeaderText="col_Equinummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Equinummer" CommandName="sort" CommandArgument="Equinummer"
                                                                runat="server">col_Equinummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                                ID="lblEquinummer" Visible="False">
                                                            </asp:Label>    
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>                                                                                                        
                                                </Columns>
                                                <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
												<br>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
                        <tr id="ShowScript" runat="server" visible="False">
                            <td colspan="2">

                                <script type="text/javascript">
									<!--                                    //
                                    function StornoConfirm(Auftragsnummer, Vertragsnummer, AngefordertAm, Fahrgestellnummer, Briefnummer, Kontingentart) {
                                        var Check = window.confirm("Wollen Sie diesen Versandauftrag wirklich stornieren?\n\tAngefordert am:\t" + AngefordertAm + "\n\tFahrgestellnr.:\t" + Fahrgestellnummer + "\t\n\tNummer ZBII:\t" + Briefnummer);
                                        if (Check) {
                                            window.document.form1.txtAuftragsnummer.value = Auftragsnummer;
                                        }
                                        return (Check);
                                    }
									//-->
                                </script>

                            </td>
                        </tr>							
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	
	</body>
</HTML>